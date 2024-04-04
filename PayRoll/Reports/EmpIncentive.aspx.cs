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
    public partial class EmpIncentive : System.Web.UI.Page
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
            string strQry = "SELECT Year  FROM M_Year Where IsActive='Y' ORDER BY Year desc";
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
                if (ddlMnth.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Month'); ", true);
                    return;
                }
                if (ddlYear.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Year'); ", true);
                    return;
                }
                int lastDay = 0;
                lastDay = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMnth.SelectedValue));
                string salaryDate = ddlYear.SelectedValue + "/" + ddlMnth.SelectedValue + "/" + lastDay;

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                dsExpCert.EmpIncentiveDataTable dt1 = new dsExpCert.EmpIncentiveDataTable();
                dsExpCertTableAdapters.EmpIncentiveTableAdapter dt = new dsExpCertTableAdapters.EmpIncentiveTableAdapter();
                string filter = "";
                dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]), Convert.ToDateTime(salaryDate));
                DataTable dt3 = dt1;
                DataView dv = new DataView(dt3);
               
                if (filter.Length > 0)
                {
                    dv.RowFilter = filter;
                }

                ReportDataSource datasource = new ReportDataSource("dsEmpIncentive", dv.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/EmpEncentiveInfo.rdlc");

                ReportParameter[] p = new ReportParameter[3];
                p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                p[1] = new ReportParameter("MonYrCd", ddlMnth.SelectedItem.Text + " " + ddlYear.SelectedItem.Text, true);
                p[2] = new ReportParameter("AsOnDate", DateTime.Now.ToString("dd/MM/yyyy"), true);

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
                ddlMnth.SelectedIndex = 0;
                ddlYear.SelectedIndex = 0;
                ReportViewer1.LocalReport.DataSources.Clear();

                ddlMnth.Focus();
            }
            catch (Exception ex)
            {

            }
        }
    }
}