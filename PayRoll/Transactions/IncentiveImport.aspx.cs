using System;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using SqlClient;
using System.Configuration;
using System.Data.OleDb;
using System.IO;

namespace PayRoll.Transactions
{
    public partial class IncentiveImport : System.Web.UI.Page
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
                throw ex;
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
                        dtExcelData.Columns.AddRange(new DataColumn[4] {new DataColumn("OrgId", typeof(int)),
                                                                        new DataColumn("MonYrcd", typeof(string)),
                                                                        new DataColumn("Employeecd", typeof(string)),
                                                                        new DataColumn("Incentive", typeof(double)),
                                                                        });

                        dtExcelData.Columns["OrgId"].DefaultValue = Session["OrgID"].ToString();
                        dtExcelData.Columns["MonYrcd"].DefaultValue = ddlMon.SelectedValue + ddlYear.SelectedValue;

                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT Employeecd, Incentive FROM [" + sheet1 + "]", excel_con))
                        {
                            oda.Fill(dtExcelData);
                            if (dtExcelData.Rows.Count > 0)
                            {
                                string strQry = "SELECT * FROM T_Incentive Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and orgID=" + Convert.ToInt32(Session["OrgID"]);
                                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                                if (objDT.Rows.Count > 0)
                                {
                                    string strQry1 = "delete FROM T_Incentive Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and orgID=" + Convert.ToInt32(Session["OrgID"]);
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
                                sqlBulkCopy.DestinationTableName = "dbo.T_Incentive";

                                con.Open();
                                sqlBulkCopy.WriteToServer(dtExcelData);
                                con.Close();

                                //Insert Log
                                string strQry = "";
                                strQry = "INSERT INTO T_Log(OrgId, Employeecd, MenuId, Mode, Computername) VALUES(@OrgId, @Employeecd, @MenuId, @Mode, @Computername)";
                                bool result = false;
                                SqlParameter[] paraLog = new SqlParameter[5];
                                paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                                paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                                paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                                paraLog[3] = new SqlParameter("@Mode", "A");
                                paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());
                                result = SqlHelper.ExecuteNonQuery(strQry, paraLog, AppGlobal.strConnString);

                                if (result)
                                {
                                    clearControls();
                                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Incentive File Imported Successfully.'); ", true);
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
            ddlMon.Focus();
            ReportViewer1.LocalReport.DataSources.Clear();
        }
        protected bool formValidation()
        {
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
            return true;
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                formValidation();
                //int lastDay = 0;
                //lastDay = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMon.SelectedValue));
                string MonYrCd = ddlMon.SelectedValue + ddlYear.SelectedValue;

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                dsExpCert.IncentiveShowDataTable dt1 = new dsExpCert.IncentiveShowDataTable();
                dsExpCertTableAdapters.IncentiveShowTableAdapter dt = new dsExpCertTableAdapters.IncentiveShowTableAdapter();
               
                dt.Fill(dt1, MonYrCd, Convert.ToInt16(Session["OrgID"]));
                DataTable dt3 = dt1;
                DataView dv = new DataView(dt3);       

                ReportDataSource datasource = new ReportDataSource("dsIncentiveShow", dv.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptIncentive.rdlc");

                ReportParameter[] p = new ReportParameter[3];
                p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                p[1] = new ReportParameter("MonYrCd", ddlMon.SelectedItem.Text + " " + ddlYear.SelectedItem.Text, true);
                p[2] = new ReportParameter("AsOnDate", DateTime.Now.ToString("dd/MM/yyyy"), true);

                this.ReportViewer1.LocalReport.SetParameters(p);
                ReportViewer1.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}