using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PayRoll.Reports
{
    public partial class NoDues : System.Web.UI.Page
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
                dsExpCertTableAdapters.ExpCertificateTableAdapter adt1 = new dsExpCertTableAdapters.ExpCertificateTableAdapter();

                dsExpCert.M_NoDuesDataTable dt2 = new dsExpCert.M_NoDuesDataTable();                
                dsExpCertTableAdapters.M_NoDuesTableAdapter adt2 = new dsExpCertTableAdapters.M_NoDuesTableAdapter();

                adt1.Fill(dt1, Convert.ToInt32(Session["OrgID"].ToString()), DateTime.Now, txtEmpCode.Text);
                adt2.Fill(dt2);

                DataTable dt3 = dt1;
                DataTable dt4 = dt2;

                DataView dv1 = new DataView(dt3);
                DataView dv2 = new DataView(dt4);

                ReportDataSource datasource1 = new ReportDataSource("dsExpCertificate", dv1.ToTable());
                ReportDataSource datasource2 = new ReportDataSource("dsNoDues", dv2.ToTable());

                ReportViewer1.LocalReport.DataSources.Add(datasource1);
                ReportViewer1.LocalReport.DataSources.Add(datasource2);

                //if (dt3.Rows.Count == 0)
                //{
                //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Employee Code does not exist'); ", true);
                //    ReportViewer1.LocalReport.DataSources.Clear();
                //    return;
                //}

                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptNoDues.rdlc");

                ReportParameter[] p = new ReportParameter[1];
                //p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                p[0] = new ReportParameter("MonYrCd", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy"), true);

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