using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SqlClient;

namespace PayRoll.Reports
{
    public partial class BankTransferList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            string strQry = "SELECT Year  FROM M_Year Where IsActive='Y' ORDER BY Year DESC";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlYear.DataSource = objDT;
            ddlYear.DataTextField = "Year";
            ddlYear.DataValueField = "Year";
            ddlYear.DataBind();

            ddlYear.Items.Insert(0, new ListItem("Select", "0000"));
            
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;


                if (ddlMon.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Month'); ", true);
                    return;
                }
                if (ddlYear.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Year'); ", true);
                    return;
                }
                if(txtDate.Text=="")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter Date'); ", true);
                    return;
                }
                if (txtChequeNo.Text == "")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter Cheque No.'); ", true);
                    return;
                }

                DataTable dt3 = bankAcList();
                DataView dv = new DataView(dt3);
                string filter = "";


                ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasource);

                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptBankTransferList.rdlc");

                ReportParameter p = new ReportParameter("OrgName", Session["OrgName"].ToString());
                ReportParameter p1 = new ReportParameter("MonYrCd", ddlMon.SelectedItem.ToString() + " " + ddlYear.SelectedValue.ToString());
                //--- To Display Logo -----------------------------------
                ReportViewer1.LocalReport.EnableExternalImages = true;
                string strqry = "select LogoPath from M_Organization where OrgId=" + Convert.ToInt32(Session["OrgID"]);
                DataTable objDTP = SqlHelper.ExecuteDataTable(strqry, AppGlobal.strConnString);
                string path = "";

                if (objDTP.Rows[0]["LogoPath"] != DBNull.Value)
                    path = new Uri(Server.MapPath(objDTP.Rows[0]["LogoPath"].ToString())).AbsoluteUri;
                else
                    path = new Uri(Server.MapPath("~/Upload/Logo.png")).AbsoluteUri;

                ReportParameter p2 = new ReportParameter("LogoPath", path, true);
                //-----------------------------------------------------
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p, p1,p2});
                ReportViewer1.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                // txtEmpName.Text = "";
                ddlMon.SelectedIndex = 0;
                ddlYear.SelectedIndex = 0;
                txtChequeNo.Text = "";
                txtDate.Text = "";
                ReportViewer1.LocalReport.DataSources.Clear();

                ddlMon.Focus();
            }
            catch (Exception ex)
            {

            }
        }

        protected DataTable bankAcList()
        {
            DataRow dr;
            DataTable dt = new DataTable();
            try
            {
                double total = 0;
                string acNo = "";
                dt.Columns.Add("A", typeof(string));
                dt.Columns.Add("AcNo", typeof(string));
                dt.Columns.Add("Amount", typeof(string));
                dt.Columns.Add("D", typeof(string));
                dt.Columns.Add("Details", typeof(string));
                dt.Columns.Add("ChequeNo", typeof(string));
                dt.Columns.Add("Date", typeof(string));

                string strQrySal = "SELECT dbo.M_Emp.BankActNo, dbo.T_MonthlySalary.NetAmount FROM dbo.M_Emp RIGHT OUTER JOIN ";
                strQrySal += " dbo.T_MonthlySalary ON dbo.M_Emp.OrgId = dbo.T_MonthlySalary.OrgId AND dbo.M_Emp.Employeecd = dbo.T_MonthlySalary.Employeecd";
                strQrySal += " WHERE(dbo.T_MonthlySalary.OrgId = "+ Convert.ToInt16(Session["OrgID"]) +") AND(dbo.T_MonthlySalary.MonYrcd = '"+ ddlMon.SelectedValue+ddlYear.SelectedValue +"') AND(dbo.M_Emp.BankCd = 1) ";
                DataTable objDTSal = SqlHelper.ExecuteDataTable(strQrySal, AppGlobal.strConnString);
                if (objDTSal.Rows.Count > 0)
                {
                    for (int i=0;i<objDTSal.Rows.Count;i++)
                    {
                        dr = dt.NewRow();

                        dr["A"] = "01";
                        acNo = "";
                        acNo = objDTSal.Rows[i]["BankActNo"].ToString();
                        if(acNo.Contains("SB/")|| acNo.Contains("sb/")|| acNo.Contains("Sb/")|| acNo.Contains("sB/"))
                        {
                            acNo = acNo.Remove(0, 3);
                        }
                        dr["AcNo"] = Convert.ToDouble(acNo).ToString("00000000000000000");
                        total += Convert.ToDouble(objDTSal.Rows[i]["NetAmount"]);
                        dr["Amount"] = Convert.ToDouble(objDTSal.Rows[i]["NetAmount"]).ToString("00000000000000");
                        dr["D"] = "00";
                        dr["Details"] = "BY SALARY " + Convert.ToDateTime("01/" + ddlMon.SelectedValue + "/" + ddlYear.SelectedValue).ToString("MMM") + " , " + ddlYear.SelectedValue;
                        dr["ChequeNo"] = txtChequeNo.Text;
                        dr["Date"] = Convert.ToDateTime(txtDate.Text).ToString("ddMMyyyy");

                        dt.Rows.Add(dr);
                    }


                    dr = dt.NewRow();

                    dr["A"] = "51";
                    acNo = "";
                    acNo = "00000060046377444";
                    dr["AcNo"] = Convert.ToDouble(acNo).ToString("00000000000000000");
                    dr["Amount"] = Convert.ToDouble(total).ToString("00000000000000");
                    dr["D"] = "00";
                    dr["Details"] = "TO PAID SALARY AS PER LIST";
                    dr["ChequeNo"] = txtChequeNo.Text;
                    dr["Date"] = Convert.ToDateTime(txtDate.Text).ToString("ddMMyyyy");

                    dt.Rows.Add(dr);
                }

                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }
        }

    }
}