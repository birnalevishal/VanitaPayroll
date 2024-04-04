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
    public partial class BankList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             if(!IsPostBack)
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
            ddlBankName.Items.Insert(0, new ListItem("All", "0000"));

            strQry = "SELECT Designation, Desigcd FROM M_Designation Where IsActive='Y' ORDER BY Designation";
            objDT1 = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlDesignation.DataSource = objDT1;
            ddlDesignation.DataTextField = "Designation";
            ddlDesignation.DataValueField = "Desigcd";
            ddlDesignation.DataBind();
            ddlDesignation.Items.Insert(0, new ListItem("All", "0"));

        }

        protected void btnSave_Click(object sender, EventArgs e)
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
                dsMaster.udf_BankListDataTable dt1 = new dsMaster.udf_BankListDataTable();

                dsMasterTableAdapters.udf_BankListTableAdapter dt = new dsMasterTableAdapters.udf_BankListTableAdapter();
                dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]), Moncd, dtt);
                DataTable dt3 = dt1;
                
                DataView dv = new DataView(dt3);
                string filter = "";

                if (ddlBankName.SelectedIndex != 0)
                {
                    filter += " BankCd=" + ddlBankName.SelectedValue + " AND ";
                }

                if (ddlDesignation.SelectedIndex != 0)
                {
                    filter += " DesgId=" + ddlDesignation.SelectedValue + " AND ";
                }

                if (filter.Length > 0)
                {
                    filter = filter.Remove(filter.Length - 4, 3);
                    dv.RowFilter = filter;
                }

                ReportDataSource datasource = new ReportDataSource("BankList", dv.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptBankList.rdlc");

                ReportParameter p = new ReportParameter("OrgName", Session["OrgName"].ToString());
                ReportParameter p1 = new ReportParameter("MonYrCd", ddlMon.SelectedItem.ToString() + " " + ddlYear.SelectedValue.ToString());
                ReportParameter p2 = new ReportParameter("BankName", ddlBankName.SelectedItem.ToString());
                ReportParameter p3 = new ReportParameter("Designation", ddlDesignation.SelectedItem.ToString());
                //--- To Display Logo -----------------------------------
                ReportViewer1.LocalReport.EnableExternalImages = true;
                string strqry = "select LogoPath from M_Organization where OrgId=" + Convert.ToInt32(Session["OrgID"]);
                DataTable objDTP = SqlHelper.ExecuteDataTable(strqry, AppGlobal.strConnString);
                string path = "";

                if (objDTP.Rows[0]["LogoPath"] != DBNull.Value)
                    path = new Uri(Server.MapPath(objDTP.Rows[0]["LogoPath"].ToString())).AbsoluteUri;
                else
                    path = new Uri(Server.MapPath("~/Upload/Logo.png")).AbsoluteUri;

                ReportParameter p4 = new ReportParameter("LogoPath", path, true);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p, p1, p2, p3, p4 });

                ReportViewer1.LocalReport.Refresh();
            }
            catch(Exception ex)
            {

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
            catch(Exception ex)
            {

            }

        }
    }
}