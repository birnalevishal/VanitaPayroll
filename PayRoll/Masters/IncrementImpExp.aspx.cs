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
                //clearControls();
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

            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            ddlMon.SelectedIndex = DateTime.Now.Month;
            ddlYear.SelectedValue = DateTime.Now.Year.ToString();

           
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
                string strQry = "";
                bool result = false;

                if (ViewState["dtExcelData"] == null)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('No data found for Upload'); ", true);
                    return;
                }
                DataTable dtExcelData = (DataTable)ViewState["dtExcelData"];

                int nSucess = 0;
                int nExists = 0;
                int nToalRows = dtExcelData.Rows.Count;

                if (nToalRows > 0)
                {
                    foreach (DataRow row in dtExcelData.Rows)
                    {
                        strQry = string.Format("SELECT COUNT(Employeecd) FROM M_Salary WHERE OrgID={0} AND MonYrcd='{1}' and Employeecd='{2}'", row["OrgId"].ToString(), row["MonYrcd"].ToString(), row["EmpCode"].ToString());
                        int nCnt = Convert.ToInt32(SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString));

                        if(nCnt > 0) {
                            nExists++;
                            row["Uploaded"] = false;
                            continue;
                        }

                        strQry = @"INSERT INTO M_Salary(OrgID, MonYrcd, Docdate, IsActive, Approval, Employeecd, witheffect, Arrears, BasicDA,HRA, Medical, Education, Conveyance, Canteen, Uniform, Washing, Add1, Add2,Add3, Gross) 
                               VALUES(@OrgID, @MonYrcd, @Docdate, @IsActive, @Approval, @Employeecd, @witheffect, @Arrears, @BasicDA, @HRA, @Medical, @Education, @Conveyance, @Canteen, @Uniform, @Washing, @Add1, @Add2,@Add3, @Gross)";

                        SqlParameter[] para = new SqlParameter[20];
                        para[0] = new SqlParameter("@OrgId", row["OrgId"].ToString());
                        para[1] = new SqlParameter("@MonYrcd", row["MonYrcd"].ToString());
                        para[2] = new SqlParameter("@Docdate", Convert.ToDateTime(row["docdate"]).ToString("dd MMM yyyy"));
                        para[3] = new SqlParameter("@IsActive", row["IsActive"].ToString());
                        para[4] = new SqlParameter("@Approval", row["Approval"].ToString());
                        para[5] = new SqlParameter("@Employeecd", row["EmpCode"].ToString());
                        para[6] = new SqlParameter("@witheffect", Convert.ToDateTime(row["wef"]).ToString("dd MMM yyyy"));
                        para[7] = new SqlParameter("@Arrears", row["Arrears"].ToString());
                        para[8] = new SqlParameter("@BasicDA", row["BASICDA"].ToString());
                        para[9] = new SqlParameter("@HRA", row["HRA"].ToString());
                        para[10] = new SqlParameter("@Medical", row["MedicalAllowance"].ToString());
                        para[11] = new SqlParameter("@Education", row["EducationalAllowance"].ToString());
                        para[12] = new SqlParameter("@Conveyance", row["ConveyanceTravellingAllowance"].ToString());
                        para[13] = new SqlParameter("@Canteen", row["TeaTiffinAllowance"].ToString());
                        para[14] = new SqlParameter("@Uniform", row["UniformShoesAllowance"].ToString());
                        para[15] = new SqlParameter("@Washing", row["WashingAllowance"].ToString());
                        para[16] = new SqlParameter("@Add1", row["Add1"].ToString());
                        para[17] = new SqlParameter("@Add2", row["Add2"].ToString());
                        para[18] = new SqlParameter("@Add3", row["Add3"].ToString());
                        para[19] = new SqlParameter("@Gross", row["GrossEarning"].ToString());

                        result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

                        nSucess++;
                        row["Uploaded"] = true;
                    }

                    string strMsg = "alert('"+ nSucess.ToString() +" uploaded successfully out of "+ nToalRows.ToString() + "\\n" + nExists+" already in salary master');";
                    //Insert Log
                    strQry = @"INSERT INTO T_Log(OrgId,MonthYrcd,docdate, Employeecd, MenuId, Mode, Computername) 
                           VALUES(@OrgId,@MonthYrcd,@docdate, @Employeecd, @MenuId, @Mode, @Computername)";
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
                        //clearControls();
                        gvUploadList.DataSource = dtExcelData;
                        gvUploadList.DataBind();

                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", strMsg, true);

                    }
                }

                //string consString = ConfigurationManager.ConnectionStrings["VanitaPayrollConnectionString"].ConnectionString;
                //using (SqlConnection con = new SqlConnection(consString))
                //{
                //    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                //    {
                //        //Set the database table name
                //        sqlBulkCopy.DestinationTableName = "dbo.M_Salary";

                //        //[OPTIONAL]: Map the Excel columns with that of the database table
                //        sqlBulkCopy.ColumnMappings.Add("OrgID", "OrgId");
                //        sqlBulkCopy.ColumnMappings.Add("MonYrcd", "MonYrcd");
                //        sqlBulkCopy.ColumnMappings.Add("docdate", "Docdate");
                //        sqlBulkCopy.ColumnMappings.Add("IsActive", "IsActive");
                //        sqlBulkCopy.ColumnMappings.Add("Approval", "Approval");

                        
                //        sqlBulkCopy.ColumnMappings.Add("EmpCode", "Employeecd");
                //        sqlBulkCopy.ColumnMappings.Add("wef", "witheffect");
                //        sqlBulkCopy.ColumnMappings.Add("Arrears", "Arrears");

                //        sqlBulkCopy.ColumnMappings.Add("BASICDA", "BasicDA");
                //        sqlBulkCopy.ColumnMappings.Add("HRA", "HRA");
                //        sqlBulkCopy.ColumnMappings.Add("MedicalAllowance", "Medical");
                //        sqlBulkCopy.ColumnMappings.Add("EducationalAllowance", "Education");
                //        sqlBulkCopy.ColumnMappings.Add("ConveyanceTravellingAllowance", "Conveyance");
                //        sqlBulkCopy.ColumnMappings.Add("TeaTiffinAllowance", "Canteen");
                //        sqlBulkCopy.ColumnMappings.Add("UniformShoesAllowance", "Uniform");
                //        sqlBulkCopy.ColumnMappings.Add("WashingAllowance", "Washing");
                //        sqlBulkCopy.ColumnMappings.Add("Add1", "Add1");
                //        sqlBulkCopy.ColumnMappings.Add("Add2", "Add2");
                //        sqlBulkCopy.ColumnMappings.Add("Add3", "Add3");
                //        sqlBulkCopy.ColumnMappings.Add("GrossEarning", "Gross");

                //        con.Open();
                //        sqlBulkCopy.WriteToServer(dtExcelData);
                //        con.Close();
                        
                //    }
                //}


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error while saving data'); ", true);
            }
        }
        protected void gvAttendence_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvEmployee.PageIndex = e.NewPageIndex;
        }
        private void clearControls()
        {
            //ddlMon.SelectedIndex = 0;
            //ddlYear.SelectedIndex = 0;
            ddlMon.Focus();
            btnSave.Text = "Save";

            gvUploadList.DataSource = null;
            gvUploadList.DataBind();
            
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

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
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
                        dtExcelData.Columns.AddRange(new DataColumn[22] {
                                                                    new DataColumn("OrgID", typeof(int)),
                                                                    new DataColumn("MonYrcd", typeof(string)),
                                                                    new DataColumn("docdate", typeof(string)),
                                                                    new DataColumn("IsActive", typeof(string)),
                                                                    new DataColumn("Approval", typeof(string)),

                                                                    new DataColumn("EmpCode", typeof(string)),
                                                                    new DataColumn("Name", typeof(string)),
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
                                                                    new DataColumn("Uploaded", typeof(bool))
                                                });

                        dtExcelData.Columns["OrgID"].DefaultValue = Session["OrgID"].ToString();
                        dtExcelData.Columns["MonYrcd"].DefaultValue = ddlMon.SelectedValue + ddlYear.SelectedValue;
                        dtExcelData.Columns["docdate"].DefaultValue = Convert.ToDateTime(txtDate.Text);
                        dtExcelData.Columns["IsActive"].DefaultValue = "Y";
                        dtExcelData.Columns["Approval"].DefaultValue = "Y";

                        //using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT Employeecd,witheffect,Arrears,BasicDA,HRA,Medical,Education,Conveyance,Canteen,Uniform,Washing,Gross  FROM[" + sheet1 + "]", excel_con))
                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT EmpCode,Name, wef,Arrears, BASICDA,HRA,MedicalAllowance,EducationalAllowance,ConveyanceTravellingAllowance,TeaTiffinAllowance,UniformShoesAllowance,WashingAllowance,Add1,Add2,Add3,GrossEarning  FROM[" + sheet1 + "]", excel_con))
                        {
                            oda.Fill(dtExcelData);
                            //string strQry = "SELECT * FROM M_Salary Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and orgID=" + Convert.ToInt32(Session["OrgID"]);
                            //DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                            //if (objDT.Rows.Count > 0)
                            //{
                            //    string strQry1 = "delete FROM M_Salary Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and orgID=" + Convert.ToInt32(Session["OrgID"]);
                            //    DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
                            //}
                            ViewState["dtExcelData"] = dtExcelData;
                        }
                        excel_con.Close();

                        gvUploadList.DataSource = dtExcelData;
                        gvUploadList.DataBind();
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

        protected void gvUploadList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                if(lblStatus.Text=="False")
                {
                    e.Row.Cells[0].BackColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}