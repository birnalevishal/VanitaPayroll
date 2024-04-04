using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SqlClient;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Configuration;

namespace PayRoll.Masters
{
    public partial class GratuityImpExcel : System.Web.UI.Page
    {
        SqlConnection sqlConn = null;
        SqlCommand sqlCmd = null;
        SqlTransaction sqlTrans = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            if (!Page.IsPostBack)
            {
                BindData();
                clearControls();
            }
        }
        private void BindData()
        {
            string strQry = "SELECT Year  FROM M_Year Where IsActive='Y' ORDER BY Year desc";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlYear.DataSource = objDT;
            ddlYear.DataTextField = "Year";
            ddlYear.DataValueField = "Year";
            ddlYear.DataBind();

            ddlYear.Items.Insert(0, new ListItem("Select", "00"));

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);

                if (Page.IsValid)
                {
                    if (formValidation())
                    {
                        if (btnSave.Text == "Save")
                        {
                            InsertRecord();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                clearControls();

            }
            catch (Exception ex)
            {

            }
        }

        private void InsertRecord()
        {
            try
            {
                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);

                //file upload path
                if (FUExcel.HasFiles)
                {
                    string excelPath = Server.MapPath("~/Imports/") + Path.GetFileName(FUExcel.PostedFile.FileName);
                    FUExcel.SaveAs(excelPath);

                    string conString = string.Empty;
                    conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
                    conString = string.Format(conString, excelPath);

                    using (OleDbConnection excel_con = new OleDbConnection(conString))
                    {
                        excel_con.Open();
                        string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                        DataTable dtExcelData = new DataTable();

                        //[OPTIONAL]: It is recommended as otherwise the data will be considered as String by default.
                        dtExcelData.Columns.AddRange(new DataColumn[13] { new DataColumn("OrgID", typeof(int)),
                                                                    new DataColumn("MonYrcd", typeof(string)),
                                                                    new DataColumn("EmpCode", typeof(string)),
                                                                    new DataColumn("PolicyNo", typeof(decimal)),
                                                                    new DataColumn("LICID", typeof(decimal)),
                                                                    new DataColumn("Category", typeof(string)),
                                                                    new DataColumn("Birthdate", typeof(string)),
                                                                    new DataColumn("OrigJoindate", typeof(string)),
                                                                    new DataColumn("Salary", typeof(decimal)),
                                                                    new DataColumn("Freequency", typeof(string)),
                                                                    new DataColumn("EmployeeName", typeof(string)),
                                                                    new DataColumn("Gender", typeof(string)),
                                                                    new DataColumn("DOJScheme", typeof(string)),
                                                                   // new DataColumn("Amount", typeof(decimal)),
                                                        });

                        dtExcelData.Columns["OrgID"].DefaultValue = Session["OrgID"].ToString();
                        dtExcelData.Columns["MonYrcd"].DefaultValue = ddlMon.SelectedValue + ddlYear.SelectedValue;

                        //using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT EmpCode, PolicyNo, LICID, DOJScheme, Amount FROM[" + sheet1 + "]", excel_con))
                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT EmpCode, PolicyNo, LICID, DOJScheme FROM[" + sheet1 + "]", excel_con))
                        {
                            oda.Fill(dtExcelData);

                            string strQry = "SELECT * FROM T_Gratuity Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and orgID=" + Convert.ToInt32(Session["OrgID"]);
                            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                            if (objDT.Rows.Count > 0)
                            {
                                string strQry1 = "delete FROM T_Gratuity Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'and orgID=" + Convert.ToInt32(Session["OrgID"]);
                                DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
                            }
                        }
                        excel_con.Close();

                        string consString = ConfigurationManager.ConnectionStrings["VanitaPayrollConnectionString"].ConnectionString;
                        using (SqlConnection con = new SqlConnection(consString))
                        {
                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                            {
                                //Set the database table name
                                sqlBulkCopy.DestinationTableName = "dbo.T_Gratuity";

                                //[OPTIONAL]: Map the Excel columns with that of the database table
                                sqlBulkCopy.ColumnMappings.Add("OrgID", "OrgID");
                                sqlBulkCopy.ColumnMappings.Add("MonYrcd", "MonYrCd");
                                sqlBulkCopy.ColumnMappings.Add("EMPCODE", "Employeecd");
                                sqlBulkCopy.ColumnMappings.Add("PolicyNo", "LIC_PolicyNo");
                                sqlBulkCopy.ColumnMappings.Add("LICID", "LIC_Id");
                                sqlBulkCopy.ColumnMappings.Add("DOJScheme", "DOJScheme");
                               // sqlBulkCopy.ColumnMappings.Add("Amount", "Amount");

                                con.Open();
                                sqlBulkCopy.WriteToServer(dtExcelData);
                                con.Close();


                                //Update Employee Master
                                string strQryEmp = "";
                                bool resultEmp = false;
                                SqlParameter[] paraEmp = new SqlParameter[5];
                                string strQryGratuity = "select * from T_Gratuity where OrgID=" + Convert.ToInt32(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' order by Employeecd";
                                DataTable objDtGratuity = SqlHelper.ExecuteDataTable(strQryGratuity, AppGlobal.strConnString);
                                if(objDtGratuity.Rows.Count>0)
                                {
                                    for(int i=0;i<objDtGratuity.Rows.Count;i++)
                                    {
                                        resultEmp = false;
                                        strQryEmp = "update M_Emp Set LIC_Id=@LIC_Id,LIC_PolicyNo=@LIC_PolicyNo,Gratuitydate=@Gratuitydate where OrgID=@OrgId and Employeecd=@Employeecd";
                                        
                                        paraEmp[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                                        paraEmp[1] = new SqlParameter("@Employeecd", objDtGratuity.Rows[i]["Employeecd"].ToString());
                                        paraEmp[2] = new SqlParameter("@LIC_Id", objDtGratuity.Rows[i]["LIC_Id"] != DBNull.Value ? objDtGratuity.Rows[i]["LIC_Id"].ToString() : "");
                                        paraEmp[3] = new SqlParameter("@LIC_PolicyNo", objDtGratuity.Rows[i]["LIC_PolicyNo"] != DBNull.Value ? Convert.ToInt64(objDtGratuity.Rows[i]["LIC_PolicyNo"]) :0);
                                        paraEmp[4] = new SqlParameter("@Gratuitydate", objDtGratuity.Rows[i]["DOJScheme"]!=DBNull.Value? Convert.ToDateTime(objDtGratuity.Rows[i]["DOJScheme"]).ToString("dd MMM yyyy") :(object)DBNull.Value);

                                        resultEmp = SqlHelper.ExecuteNonQuery(strQryEmp, paraEmp, AppGlobal.strConnString);
                                    }
                                }
                                string strQry = "";
                                strQry = "INSERT INTO T_Log(OrgId,MonthYrcd, Employeecd, MenuId, Mode, Computername) VALUES(@OrgId,@MonthYrcd, @Employeecd, @MenuId, @Mode, @Computername)";
                                bool result = false;
                                SqlParameter[] paraLog = new SqlParameter[6];
                                paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                                paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                                paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                                paraLog[3] = new SqlParameter("@Mode", "A");
                                paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());
                                paraLog[5] = new SqlParameter("@MonthYrcd", ddlMon.SelectedValue + ddlYear.SelectedValue);

                                result = SqlHelper.ExecuteNonQuery(strQry, paraLog, AppGlobal.strConnString);
                                if (result)
                                {
                                    sqlTrans.Commit();
                                    if (gvSalary.Rows.Count == 0 && gvAttendence.Rows.Count == 0 && gvDeduction.Rows.Count == 0)
                                    {
                                        clearControls();
                                    }
                                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Gratuity Uploaded Successfully.'); ", true);
                                    return;
                                }
                            }
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Excel File'); ", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
            }
        }

        private void clearControls()
        {
            ddlMon.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;

            btnSave.Text = "Save";

            lblAttHeading.Text = "Attendence Not Found List";
            lblDedHeading.Text = "Employee Not Found List";

            gvAttendence.DataSource = null;
            gvAttendence.DataBind();
            gvSalary.DataSource = null;
            gvSalary.DataBind();

            gvDeduction.DataSource = null;
            gvDeduction.DataBind();
        }

        protected bool formValidation()
        {
            int nID = 0;
            if (ddlMon.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Month'); ", true);
                return false;
            }
            if (ddlYear.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Year'); ", true);
                return false;
            }
            //if(!FUExcel.HasFile)
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Excel File'); ", true);
            //    return false;
            //}
            return true;
        }
        
    }
}