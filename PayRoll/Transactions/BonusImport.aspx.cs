using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SqlClient;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Configuration;

namespace PayRoll.Transactions
{
    public partial class BonusImport : System.Web.UI.Page
    {
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

            ddlYearTo.DataSource = objDT;
            ddlYearTo.DataTextField = "Year";
            ddlYearTo.DataValueField = "Year";
            ddlYearTo.DataBind();
            ddlYearTo.Items.Insert(0, new ListItem("Select", "00"));

        }
        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    if (formValidation())
                    {
                        InsertRecord();
                    }
                }
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Data not imported completely'); ", true);
            }
        }

        private void InsertRecord()
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
                        dtExcelData.Columns.AddRange(new DataColumn[6] { new DataColumn("OrgId", typeof(int)),
                                                                        new DataColumn("BonusDt", typeof(string)),
                                                                        new DataColumn("MonYrcd", typeof(string)),
                                                                        new DataColumn("ToMonYrcd", typeof(string)),
                                                                        new DataColumn("Employeecd", typeof(string)),
                                                                        new DataColumn("Bonus", typeof(double)),
                                                                        });

                        dtExcelData.Columns["OrgId"].DefaultValue = Session["OrgID"].ToString();
                        dtExcelData.Columns["BonusDt"].DefaultValue = Convert.ToDateTime(txtDt.Text).ToString("dd MMM yyyy");
                        dtExcelData.Columns["MonYrcd"].DefaultValue = ddlMon.SelectedValue + ddlYear.SelectedValue;
                        dtExcelData.Columns["ToMonYrcd"].DefaultValue = ddlMonTo.SelectedValue + ddlYearTo.SelectedValue;

                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT Employeecd, Bonus FROM [" + sheet1 + "]", excel_con))
                        {
                            oda.Fill(dtExcelData);
                            if (dtExcelData.Rows.Count > 0)
                            {
                                string strQry = "SELECT * FROM T_Bonus Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' AND ToMonYrcd='" + ddlMonTo.SelectedValue + ddlYearTo.SelectedValue + "' and orgID=" + Convert.ToInt32(Session["OrgID"]);
                                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                                if (objDT.Rows.Count > 0)
                                {
                                    string strQry1 = "delete FROM T_Bonus Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' AND ToMonYrcd='" + ddlMonTo.SelectedValue + ddlYearTo.SelectedValue + "' and orgID=" + Convert.ToInt32(Session["OrgID"]);
                                    bool nResult = SqlHelper.ExecuteNonQuery(strQry1, AppGlobal.strConnString);
                                }
                            }
                            else
                            {
                                excel_con.Close();
                                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Data not available for Import'); ", true);
                                return;
                            }
                        }
                        excel_con.Close();


                        using (SqlConnection con = new SqlConnection(AppGlobal.strConnString))
                        {
                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                            {
                                //Set the database table name
                                sqlBulkCopy.DestinationTableName = "dbo.T_Bonus";

                                sqlBulkCopy.ColumnMappings.Add("OrgId", "OrgId");
                                sqlBulkCopy.ColumnMappings.Add("BonusDt", "BonusDt");
                                sqlBulkCopy.ColumnMappings.Add("Employeecd", "Employeecd");
                                sqlBulkCopy.ColumnMappings.Add("MonYrcd", "MonYrcd");
                                sqlBulkCopy.ColumnMappings.Add("ToMonYrcd", "ToMonYrcd");
                                sqlBulkCopy.ColumnMappings.Add("Bonus", "Bonus");

                                con.Open();
                                sqlBulkCopy.WriteToServer(dtExcelData);
                                con.Close();

                                //Insert Log
                                string strQry = "";
                                strQry = "INSERT INTO T_Log(OrgId, Employeecd, MenuId, Mode, Computername, docdate) VALUES(@OrgId, @Employeecd, @MenuId, @Mode, @Computername, @docdate)";
                                bool result = false;
                                SqlParameter[] paraLog = new SqlParameter[6];
                                paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                                paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                                paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                                paraLog[3] = new SqlParameter("@Mode", "A");
                                paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());
                                paraLog[5] = new SqlParameter("@docdate", Convert.ToDateTime(txtDt.Text).ToString("dd MMM yyyy"));
                                result = SqlHelper.ExecuteNonQuery(strQry, paraLog, AppGlobal.strConnString);

                                if (result)
                                {
                                    clearControls();
                                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Bouns File Imported Successfully.'); ", true);
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
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error: Import Format not Matching'); ", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clearControls();
        }

        private void clearControls()
        {
            ddlMon.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;
            ddlMonTo.SelectedIndex = 0;
            ddlYearTo.SelectedIndex = 0;
            ddlMon.Focus();

        }

        protected bool formValidation()
        {
            int nID = 0;
            if (ddlMon.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select From Month'); ", true);
                return false;
            }
            if (ddlYear.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select From Year'); ", true);
                return false;
            }

            if (ddlMonTo.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select To Month'); ", true);
                return false;
            }
            if (ddlYearTo.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select To Year'); ", true);
                return false;
            }

            return true;
        }

    }
}