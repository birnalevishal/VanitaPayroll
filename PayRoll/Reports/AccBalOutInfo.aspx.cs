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
    public partial class AccBalOutInfo : System.Web.UI.Page
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
                dsExpCert.AccBalOutInfoDataTable dt1 = new dsExpCert.AccBalOutInfoDataTable();
                dsExpCertTableAdapters.AccBalOutInfoTableAdapter dt = new dsExpCertTableAdapters.AccBalOutInfoTableAdapter();

                //Get Employee Leave Date
                string strquery = "Select Leavedate from M_Emp where (Leavedate <> null or Leavedate <> '') and Employeecd = " + txtEmpCode.Text;
                DataTable objdt = SqlHelper.ExecuteDataTable(strquery, AppGlobal.strConnString);
                if(objdt.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Employee Leave date not found'); ", true);
                    return;          
                }
                if (objdt.Rows.Count > 0)
                {
                    DateTime Ldt = Convert.ToDateTime(objdt.Rows[0]["Leavedate"]);

                    dt.Fill(dt1, Convert.ToInt32(Session["OrgID"].ToString()), Ldt, Ldt, txtEmpCode.Text);
                    DataTable dt3 = dt1;

                    DataView dv = new DataView(dt3);

                    ReportDataSource datasource = new ReportDataSource("dsAccBalOutInfo", dv.ToTable());

                    ReportViewer1.LocalReport.DataSources.Add(datasource);

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptAccBalOutInfo.rdlc");

                    ReportParameter[] p = new ReportParameter[2];
                    p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                    p[1] = new ReportParameter("MonYrCd", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy"), true);
                   

                    this.ReportViewer1.LocalReport.SetParameters(p);
                    ReportViewer1.LocalReport.Refresh();
                }

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