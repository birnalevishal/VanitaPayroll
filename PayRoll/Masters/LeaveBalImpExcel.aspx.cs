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
    public partial class LeaveBalImpExcel : System.Web.UI.Page
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
                    string extension = Path.GetExtension(FUExcel.PostedFile.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(FUExcel.PostedFile.FileName);
                    string strConcat = DateTime.Now.ToString("ddMMyyyy_HHmmss");

                    string excelPath = Server.MapPath("~/Imports/" + fileName + "_" + strConcat + extension);
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
                        dtExcelData.Columns.AddRange(new DataColumn[4] { new DataColumn("OrgId", typeof(int)),
                                                                    new DataColumn("MonYrcd", typeof(string)),
                                                                    new DataColumn("EMPCODE", typeof(string)),
                                                                    new DataColumn("PaidBal", typeof(decimal)),
                                                     });

                        dtExcelData.Columns["OrgID"].DefaultValue = Session["OrgID"].ToString();
                        dtExcelData.Columns["MonYrcd"].DefaultValue = ddlMon.SelectedValue + ddlYear.SelectedValue;

                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT EMPCODE, PaidBal FROM[" + sheet1 + "]", excel_con))
                        {
                            oda.Fill(dtExcelData);

                            string strQry = "SELECT * FROM T_LeaveBalPaid Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and orgID=" + Convert.ToInt32(Session["OrgID"]);
                            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                            if (objDT.Rows.Count > 0)
                            {
                                string strQry1 = "delete FROM T_LeaveBalPaid Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'and orgID=" + Convert.ToInt32(Session["OrgID"]);
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
                                sqlBulkCopy.DestinationTableName = "dbo.T_LeaveBalPaid";

                                //[OPTIONAL]: Map the Excel columns with that of the database table
                                sqlBulkCopy.ColumnMappings.Add("OrgId", "OrgId");
                                sqlBulkCopy.ColumnMappings.Add("MonYrcd", "MonYrcd");
                                sqlBulkCopy.ColumnMappings.Add("EMPCODE", "Employeecd");
                                sqlBulkCopy.ColumnMappings.Add("PaidBal", "PaidBal");

                                con.Open();
                                sqlBulkCopy.WriteToServer(dtExcelData);
                                con.Close();

                              
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
                                    clearControls();
                                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Leave Balance Uploaded Successfully.'); ", true);
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

            }
        }

        private void clearControls()
        {
            ddlMon.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;

            btnSave.Text = "Save";
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