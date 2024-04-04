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
    public partial class LeaveBalance : System.Web.UI.Page
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
            string strQry = "SELECT Year FROM M_Year Where IsActive='Y' ORDER BY Year DESC";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlYear.DataSource = objDT;
            ddlYear.DataTextField = "Year";
            ddlYear.DataValueField = "Year";
            ddlYear.DataBind();
            ddlYear.Items.Insert(0, new ListItem("Select", "0000"));

            ddlToYear.DataSource = objDT;
            ddlToYear.DataTextField = "Year";
            ddlToYear.DataValueField = "Year";
            ddlToYear.DataBind();
            ddlToYear.Items.Insert(0, new ListItem("Select", "0000"));

            //strQry = "SELECT Division, Divcd FROM M_Division Where IsActive='Y' ORDER BY Division";
            //objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            //ddlDivision.DataSource = objDT;
            //ddlDivision.DataTextField = "Division";
            //ddlDivision.DataValueField = "Divcd";
            //ddlDivision.DataBind();
            //ddlDivision.Items.Insert(0, new ListItem("All", "0"));

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                dsRegister.LeaveBalanceDataTable dt1 = new dsRegister.LeaveBalanceDataTable();
                dsRegisterTableAdapters.LeaveBalanceTableAdapter dt = new dsRegisterTableAdapters.LeaveBalanceTableAdapter();

                dt.Fill(dt1,Convert.ToInt16(Session["OrgID"]), ddlMnth.SelectedValue+ddlYear.SelectedValue, ddlToMnth.SelectedValue+ddlToYear.SelectedValue);
                DataTable dt3 = dt1;
                DataView dv = new DataView(dt3);

               
                ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptLeaveBalance.rdlc");

                ReportParameter[] p = new ReportParameter[4];
                p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                p[1] = new ReportParameter("FrMoncd", ddlMnth.SelectedItem.ToString() + " " + ddlYear.SelectedValue.ToString());
                p[2] = new ReportParameter("ToMoncd", ddlToMnth.SelectedItem.ToString() + " " + ddlToYear.SelectedValue.ToString());
                //--- To Display Logo -----------------------------------
                ReportViewer1.LocalReport.EnableExternalImages = true;
                string strqry = "select LogoPath from M_Organization where OrgId=" + Convert.ToInt32(Session["OrgID"]);
                DataTable objDTP = SqlHelper.ExecuteDataTable(strqry, AppGlobal.strConnString);
                string path = "";

                if (objDTP.Rows[0]["LogoPath"] != DBNull.Value)
                    path = new Uri(Server.MapPath(objDTP.Rows[0]["LogoPath"].ToString())).AbsoluteUri;
                else
                    path = new Uri(Server.MapPath("~/Upload/Logo.png")).AbsoluteUri;

                p[3] = new ReportParameter("LogoPath", path, true);
                //-----------------------------------------------------
                this.ReportViewer1.LocalReport.SetParameters(p);
                
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
                
                ReportViewer1.LocalReport.DataSources.Clear();
                ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:txtVisible();", true);

            }
            catch(Exception ex)
            {

            }

        }

        protected void ddlMnth_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlToMnth.SelectedValue = ddlMnth.SelectedValue;
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlToYear.SelectedValue = ddlYear.SelectedValue;
        }
    }
}