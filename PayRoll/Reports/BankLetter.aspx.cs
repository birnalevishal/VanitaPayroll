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
    public partial class BankLetter : System.Web.UI.Page
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

            string strQry1 = "SELECT BankCd,BankName  FROM M_Bank Where IsActive='Y' ORDER BY BankCd";
            DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            ddlBankName.DataSource = objDT1;
            ddlBankName.DataTextField = "BankName";
            ddlBankName.DataValueField = "BankCd";
            ddlBankName.DataBind();
            //ddlBankName.Items.Insert(0, new ListItem("All", "0000"));

            string strQry2 = "SELECT BankBranchId, BankBranch FROM M_BankBranch Where IsActive='Y' and BankCd = " + ddlBankName.SelectedValue;
            DataTable objDT2 = SqlHelper.ExecuteDataTable(strQry2, AppGlobal.strConnString);
            ddlBranchName.DataSource = objDT2;
            ddlBranchName.DataTextField = "BankBranch";
            ddlBranchName.DataValueField = "BankBranchId";
            ddlBranchName.DataBind();

            ddlBranchName.Items.Insert(0, new ListItem("All", "0"));

        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
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

                //Get Mnth Code for Selected Month & Year
                string Moncd = ddlMon.SelectedValue + ddlYear.SelectedValue;

                //Last Day of Month/Year
                int Days = DateTime.DaysInMonth(Convert.ToInt32(Moncd.Substring(2, 4)), Convert.ToInt32(Moncd.Substring(0, 2)));
                DateTime dtt = Convert.ToDateTime(Moncd.Substring(2, 4) + "-" + Moncd.Substring(0, 2) + "-" + Days);

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                dsExpCert.udf_BankLetterDataTable dt1 = new dsExpCert.udf_BankLetterDataTable();
                dsExpCertTableAdapters.udf_BankLetterTableAdapter dt = new dsExpCertTableAdapters.udf_BankLetterTableAdapter();

                //-------------------Organisation Detail-----------------------------------------
                dsExpCert.udf_organisationListDataTable dtorg = new dsExpCert.udf_organisationListDataTable();
                dsExpCertTableAdapters.udf_organisationListTableAdapter dtaorg = new dsExpCertTableAdapters.udf_organisationListTableAdapter();
                dtaorg.Fill(dtorg);
                DataTable dtorg1 = dtorg;
                DataView dvorg = new DataView(dtorg1);
                string filter2 = "";
                filter2 += " OrgId=" + Convert.ToInt32(Session["OrgID"]);
                dvorg.RowFilter = filter2;
                //------------------------------------------------------------------------------

                dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]), Moncd, dtt);
                DataTable dt3 = dt1;

                DataView dv = new DataView(dt3);
                string filter = "";

                //if (ddlBankName.SelectedIndex != 0)
                //{
                //    filter += " BankCd=" + ddlBankName.SelectedValue + " AND ";
                //}
                filter += " BankCd=" + ddlBankName.SelectedValue + " AND ";
                if (ddlBranchName.SelectedIndex != 0 && ddlBranchName.SelectedIndex != -1)
                {
                    filter += " BankBranchId=" + ddlBranchName.SelectedValue + " AND ";
                }
                if (filter.Length > 0)
                {
                    filter = filter.Remove(filter.Length - 4, 3);
                    dv.RowFilter = filter;
                }

                ReportDataSource datasource = new ReportDataSource("dsBankLetter", dv.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasource);

                ReportDataSource datasourceOrg = new ReportDataSource("dsOrg", dvorg.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasourceOrg);

                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptBankLetter.rdlc");

                ReportParameter p = new ReportParameter("OrgName", Session["OrgName"].ToString());
                ReportParameter p1 = new ReportParameter("MonYrCd", ddlMon.SelectedItem.ToString() + " " + ddlYear.SelectedValue.ToString());
                ReportParameter p2 = new ReportParameter("BankName", ddlBankName.SelectedItem.ToString());
                ReportParameter p3 = new ReportParameter("AsOnDAte", DateTime.Now.ToString("dd/MM/yyyy"));
                ReportParameter p4 = new ReportParameter("BranchName", ddlBranchName.SelectedItem.ToString());
                
              

                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p, p1, p2,p3, p4 });
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
                ReportViewer1.LocalReport.DataSources.Clear();

                ddlMon.Focus();
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlBankName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strQry2 = "SELECT BankBranchId, BankBranch FROM M_BankBranch Where IsActive='Y' and BankCd = " + ddlBankName.SelectedValue;
            DataTable objDT2 = SqlHelper.ExecuteDataTable(strQry2, AppGlobal.strConnString);
            ddlBranchName.DataSource = objDT2;
            ddlBranchName.DataTextField = "BankBranch";
            ddlBranchName.DataValueField = "BankBranchId";
            ddlBranchName.DataBind();
            ddlBranchName.Items.Insert(0, new ListItem("All", "0"));
        }
    }
}