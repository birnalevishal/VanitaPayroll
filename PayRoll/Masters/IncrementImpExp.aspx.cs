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
    public partial class IncrementImpExp : System.Web.UI.Page
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
                ddlMon.Focus();
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
                if (Page.IsValid)
                {
                    if (formValidation())
                    {
                        //string strQry = "select * from T_SalaryLock where OrgId=" + Convert.ToInt16(Session["orgID"]) + " and RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)>='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "' and Lock='Y'";
                        //DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                        //if (objDT.Rows.Count > 0)
                        //{
                        //    clearControls();
                        //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Salary Already Processed, Cant Modify Now.'); ", true);
                        //    return;
                        //}
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
                        dtExcelData.Columns.AddRange(new DataColumn[20] {
                                                                    new DataColumn("OrgID", typeof(int)),
                                                                    new DataColumn("MonYrcd", typeof(string)),
                                                                    new DataColumn("docdate", typeof(string)),
                                                                    new DataColumn("IsActive", typeof(string)),
                                                                    new DataColumn("Approval", typeof(string)),

                                                                    new DataColumn("EmpCode", typeof(string)),
                                                                    new DataColumn("wef", typeof(string)),
                                                                    new DataColumn("Arrears", typeof(string)),

                                                                    new DataColumn("BASICDA", typeof(decimal)),
                                                                    new DataColumn("HRA", typeof(decimal)),
                                                                    new DataColumn("MedicalAllowance", typeof(decimal)),
                                                                    new DataColumn("EducationalAllowance", typeof(decimal)),
                                                                    new DataColumn("ConveyanceTravellingAllowance", typeof(decimal)),
                                                                    new DataColumn("TeaTiffinAllowance", typeof(decimal)),
                                                                    new DataColumn("UniformShoesAllowance", typeof(decimal)),
                                                                    new DataColumn("WashingAllowance", typeof(decimal)),
                                                                    new DataColumn("Add1", typeof(decimal)),
                                                                    new DataColumn("Add2", typeof(decimal)),
                                                                    new DataColumn("Add3", typeof(decimal)),
                                                                    new DataColumn("GrossEarning", typeof(decimal)),
                                                });

                        dtExcelData.Columns["OrgID"].DefaultValue = Session["OrgID"].ToString();
                        dtExcelData.Columns["MonYrcd"].DefaultValue = ddlMon.SelectedValue + ddlYear.SelectedValue;
                        dtExcelData.Columns["docdate"].DefaultValue = Convert.ToDateTime(txtDate.Text);
                        dtExcelData.Columns["IsActive"].DefaultValue = "Y";
                        dtExcelData.Columns["Approval"].DefaultValue = "Y";

                        //using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT Employeecd,witheffect,Arrears,BasicDA,HRA,Medical,Education,Conveyance,Canteen,Uniform,Washing,Gross  FROM[" + sheet1 + "]", excel_con))
                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT EmpCode,wef,Arrears, BASICDA,HRA,MedicalAllowance,EducationalAllowance,ConveyanceTravellingAllowance,TeaTiffinAllowance,UniformShoesAllowance,WashingAllowance,Add1,Add2,Add3,GrossEarning  FROM[" + sheet1 + "]", excel_con))
                        {
                            oda.Fill(dtExcelData);
                            //string strQry = "SELECT * FROM M_Salary Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and orgID=" + Convert.ToInt32(Session["OrgID"]);
                            //DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                            //if (objDT.Rows.Count > 0)
                            //{
                            //    string strQry1 = "delete FROM M_Salary Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and orgID=" + Convert.ToInt32(Session["OrgID"]);
                            //    DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
                            //}
                        }
                        excel_con.Close();

                        string consString = ConfigurationManager.ConnectionStrings["VanitaPayrollConnectionString"].ConnectionString;
                        using (SqlConnection con = new SqlConnection(consString))
                        {
                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                            {
                                //Set the database table name
                                sqlBulkCopy.DestinationTableName = "dbo.M_Salary";

                                //[OPTIONAL]: Map the Excel columns with that of the database table
                                sqlBulkCopy.ColumnMappings.Add("OrgID", "OrgId");
                                sqlBulkCopy.ColumnMappings.Add("MonYrcd", "MonYrcd");
                                sqlBulkCopy.ColumnMappings.Add("docdate", "Docdate");
                                sqlBulkCopy.ColumnMappings.Add("IsActive", "IsActive");
                                sqlBulkCopy.ColumnMappings.Add("Approval", "Approval");


                                sqlBulkCopy.ColumnMappings.Add("EmpCode", "Employeecd");
                                sqlBulkCopy.ColumnMappings.Add("wef", "witheffect");
                                sqlBulkCopy.ColumnMappings.Add("Arrears", "Arrears");
                                
                                sqlBulkCopy.ColumnMappings.Add("BASICDA", "BasicDA");
                                sqlBulkCopy.ColumnMappings.Add("HRA", "HRA");
                                sqlBulkCopy.ColumnMappings.Add("MedicalAllowance", "Medical");
                                sqlBulkCopy.ColumnMappings.Add("EducationalAllowance", "Education");
                                sqlBulkCopy.ColumnMappings.Add("ConveyanceTravellingAllowance", "Conveyance");
                                sqlBulkCopy.ColumnMappings.Add("TeaTiffinAllowance", "Canteen");
                                sqlBulkCopy.ColumnMappings.Add("UniformShoesAllowance", "Uniform");
                                sqlBulkCopy.ColumnMappings.Add("WashingAllowance", "Washing");
                                sqlBulkCopy.ColumnMappings.Add("Add1", "Add1");
                                sqlBulkCopy.ColumnMappings.Add("Add2", "Add2");
                                sqlBulkCopy.ColumnMappings.Add("Add3", "Add3");
                                sqlBulkCopy.ColumnMappings.Add("GrossEarning", "Gross");
                               
                                con.Open();
                                sqlBulkCopy.WriteToServer(dtExcelData);
                                con.Close();

                               
                                //Insert Log
                                string strQry = "";
                                strQry = "INSERT INTO T_Log(OrgId,MonthYrcd,docdate, Employeecd, MenuId, Mode, Computername) VALUES(@OrgId,@MonthYrcd,@docdate, @Employeecd, @MenuId, @Mode, @Computername)";
                                bool result = false;
                                SqlParameter[] paraLog = new SqlParameter[7];
                                paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                                paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                                paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                                paraLog[3] = new SqlParameter("@Mode", "A");
                                paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());
                                paraLog[5] = new SqlParameter("@MonthYrcd", ddlMon.SelectedValue + ddlYear.SelectedValue);
                                paraLog[6] = new SqlParameter("@docdate", Convert.ToDateTime(txtDate.Text).ToString("dd MMM yyyy"));

                                result = SqlHelper.ExecuteNonQuery(strQry, paraLog, AppGlobal.strConnString);
                                if (result)
                                {
                                    sqlTrans.Commit();
                                    if (gvSalary.Rows.Count == 0 && gvEmployee.Rows.Count == 0)
                                    {
                                        clearControls();
                                    }

                                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Increment Uploaded Successfully.'); ", true);
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
        protected void gvAttendence_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEmployee.PageIndex = e.NewPageIndex;
        }
        private void clearControls()
        {
            ddlMon.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;
            ddlMon.Focus();
            lblAttHeading.Text = "Salary Not Found List";
            btnSave.Text = "Save";

            gvEmployee.DataSource = null;
            gvEmployee.DataBind();
            gvSalary.DataSource = null;
            gvSalary.DataBind();
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