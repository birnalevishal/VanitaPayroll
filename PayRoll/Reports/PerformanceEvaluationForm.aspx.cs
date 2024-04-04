using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PayRoll.Reports
{
    public partial class PerformanceEvaluationForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                if(Request.QueryString.HasKeys())
                {
                    string EmpCodes = Request.QueryString["EmpCodes"].ToString();
                    txtEmpCode.Text = EmpCodes;
                    btnShow_Click(null, null);
                }
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtEmpCode.Text == "")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Please Enter Employee Code'); ", true);
                    return;
                }
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                dsExpCert.PerformanceEvaluationFormDataTable dt1 = new dsExpCert.PerformanceEvaluationFormDataTable();
                dsExpCertTableAdapters.PerformanceEvaluationFormTableAdapter dt = new dsExpCertTableAdapters.PerformanceEvaluationFormTableAdapter();

                string yrMonCd = Convert.ToInt32(Convert.ToDateTime(DateTime.Now).Year).ToString("0000");
                yrMonCd += Convert.ToInt16(Convert.ToDateTime(DateTime.Now).Month).ToString("00");

                dt.Fill(dt1, Convert.ToInt32(Session["OrgID"].ToString()),  DateTime.Now , yrMonCd, txtEmpCode.Text);
                DataTable dt3 = dt1;
                DataView dv = new DataView(dt3);

                ReportDataSource datasource = new ReportDataSource("dsPerformanceEvaluationForm", dv.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptPerformanceEvaluationForm.rdlc");

                ReportParameter[] p = new ReportParameter[2];
                p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                p[1] = new ReportParameter("MonYrCd", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy"), true);

                this.ReportViewer1.LocalReport.SetParameters(p);
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
                txtEmpCode.Text = "";
                ReportViewer1.LocalReport.DataSources.Clear();
                txtEmpCode.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}