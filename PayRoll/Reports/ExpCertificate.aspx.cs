using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SqlClient;

namespace PayRoll.Reports
{
    public partial class ExpCertificate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
                dsExpCert.ExpCertificateDataTable dt1 = new dsExpCert.ExpCertificateDataTable();
                dsExpCertTableAdapters.ExpCertificateTableAdapter dt = new dsExpCertTableAdapters.ExpCertificateTableAdapter();

                dt.Fill(dt1, Convert.ToInt32(Session["OrgID"].ToString()), DateTime.Now, txtEmpCode.Text);
                DataTable dt3 = dt1;

                DataView dv = new DataView(dt3);

                ReportDataSource datasource = new ReportDataSource("DSExpCertificate", dv.ToTable());

                ReportViewer1.LocalReport.DataSources.Add(datasource);

                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptExpCertificate.rdlc");

                string strQry = "SELECT isnull(ShortName,'') as ShortName FROM M_Organization Where OrgID=" + Session["OrgID"].ToString();
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);

                ReportParameter[] p = new ReportParameter[4];
                p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                p[1] = new ReportParameter("MonYrCd", Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy"), true);
                p[2] = new ReportParameter("OrgID", Session["OrgID"].ToString(), true);
                p[3] = new ReportParameter("ShortName", objDT.Rows[0]["ShortName"].ToString(), true);


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