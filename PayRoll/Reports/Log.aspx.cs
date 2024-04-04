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
    public partial class Log : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindData();

                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        private void BindData()
        {
            string strqry1 = "Select MenuId, Title from  M_Menu where ParentMenuId = 0 and Logs='Y' and MenuId <> 1";
            DataTable objdt1 = SqlHelper.ExecuteDataTable(strqry1, AppGlobal.strConnString);
            ddlMenu.DataSource = objdt1;
            ddlMenu.DataValueField = "MenuId";
            ddlMenu.DataTextField = "Title";
            ddlMenu.DataBind();
            //ddlMenu.Items.Insert(0, new ListItem("Select", "00"));

            string strqry2 = "Select MenuId, Title from M_Menu where Logs='Y' and ParentMenuId = " + ddlMenu.SelectedValue;
            DataTable objdt2 = SqlHelper.ExecuteDataTable(strqry2, AppGlobal.strConnString);
            ddlSubMenu.DataSource = objdt2;
            ddlSubMenu.DataValueField = "MenuId";
            ddlSubMenu.DataTextField = "Title";
            ddlSubMenu.DataBind();
            //ddlSubMenu.Items.Insert(0, new ListItem("Select", "00"));
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                dsExpCert.LogDataTable dt1 = new dsExpCert.LogDataTable();
                dsExpCertTableAdapters.LogTableAdapter dt = new PayRoll.dsExpCertTableAdapters.LogTableAdapter();

                dt.Fill(dt1, Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text));
                DataTable dt3 = dt1;
                DataView dv = new DataView(dt3);

                string filter = "";

                if (ddlSubMenu.SelectedValue != "0" && ddlSubMenu.SelectedValue != "")
                {
                    if (ddlSubMenu.SelectedValue != "0")
                    {
                        filter += "MenuId =" + ddlSubMenu.SelectedValue + " And ";
                    }

                    if (filter.Length > 0)
                    {
                        filter = filter.Remove(filter.Length - 4, 3);
                        dv.RowFilter = filter;
                    }

                    ReportDataSource datasource = new ReportDataSource("dsLog", dv.ToTable());
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptLog.rdlc");

                    ReportParameter[] p = new ReportParameter[2];
                    p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                    p[1] = new ReportParameter("asOnDate", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy"), true);

                    this.ReportViewer1.LocalReport.SetParameters(p);
                    ReportViewer1.LocalReport.Refresh();
                }
                else
                {
                    return;
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
                ddlMenu.SelectedIndex = 0;
                ddlSubMenu.SelectedIndex = 0;
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                ReportViewer1.LocalReport.DataSources.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ddlMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strqry2 = "Select MenuId, Title from M_Menu where Logs='Y' and ParentMenuId = " + ddlMenu.SelectedValue;
            DataTable objdt2 = SqlHelper.ExecuteDataTable(strqry2, AppGlobal.strConnString);
            ddlSubMenu.DataSource = objdt2;
            ddlSubMenu.DataValueField = "MenuId";
            ddlSubMenu.DataTextField = "Title";
            ddlSubMenu.DataBind();
            //ddlSubMenu.Items.Insert(0, new ListItem("Select", "00"));
        }
    }
}