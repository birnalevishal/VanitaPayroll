using SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlTypes;
using Microsoft.Reporting.WebForms;

namespace PayRoll.Reports
{
    public partial class AdvReport : System.Web.UI.Page
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

            ddlYear.Items.Insert(0, new ListItem("Select", "00"));

            strQry = "SELECT Employeename,Employeecd FROM M_Emp where OrgID=" + Convert.ToInt16(Session["OrgID"]) + " ORDER BY Employeename";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlEmpName.DataSource = objDT;
            ddlEmpName.DataTextField = "Employeename";
            ddlEmpName.DataValueField = "Employeecd";
            ddlEmpName.DataBind();
            ddlEmpName.Items.Insert(0, "Select");

        }

        protected void txtEmpCode_TextChanged(object sender, EventArgs e)
        {
            if (txtEmpCode.Text != "")
            {
                string strQry = "SELECT Employeename  FROM M_Emp Where Employeecd='" + txtEmpCode.Text + "' and OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and IsActive='Y'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    ddlEmpName.SelectedValue = txtEmpCode.Text;
                }
                else
                {
                    ddlEmpName.SelectedIndex = 0;
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Wrong Employee Code'); ", true);
                    return;
                }
            }
        }
        protected void ddlEmpName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEmpName.SelectedIndex != 0)
            {
                txtEmpCode.Text = ddlEmpName.SelectedValue.ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                dsRegister.vw_AdvanceReportDataTable objDT = new dsRegister.vw_AdvanceReportDataTable();
                string strMnthYr = "All";

                string filter = "";
                string strQry = @"SELECT OrgId, Employeecd, Employeename, DatofJoin, AdvId, AdvTypeId, AdvName, AdvDt, AdvAmount, AdvApproved, ApproveDt, 
                                    AdvBalance, DedAmount, DedMonYrcd, AdvPaySrNo, MonYrcd, PayAmount, PayDt, Paid 
                                  FROM  vw_AdvanceReport WHERE OrgId=" + Convert.ToInt32(Session["OrgID"]) + " ";

                if (ddlMon.SelectedIndex != 0 && ddlYear.SelectedIndex != 0)
                {
                    filter += "AND MonYrcd = '" + ddlMon.SelectedValue + ddlYear.SelectedValue +"' ";
                    strMnthYr = ddlMon.SelectedValue + ddlYear.SelectedValue;
                }

                if(txtEmpCode.Text.Trim()!="")
                {
                    filter += "AND Employeecd = '" + txtEmpCode.Text + "' ";
                }

                if(ddlPayStatus.SelectedValue!="")
                {
                    filter += "AND Paid = '" + ddlPayStatus.SelectedValue.ToString() + "'";
                }

                strQry = strQry + filter;
                DataTable dt = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);

                ReportDataSource datasource = new ReportDataSource("AdvanceReport", dt);
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/AdvReport.rdlc");

                ReportParameter[] p = new ReportParameter[3];
                p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                p[1] = new ReportParameter("MonYrCd", strMnthYr, true);
                //--- To Display Logo -----------------------------------
                ReportViewer1.LocalReport.EnableExternalImages = true;
                strQry = "select LogoPath from M_Organization where OrgId=" + Convert.ToInt32(Session["OrgID"]);
                DataTable objDTP = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                string path = "";

                if (objDTP.Rows[0]["LogoPath"] != DBNull.Value)
                    path = new Uri(Server.MapPath(objDTP.Rows[0]["LogoPath"].ToString())).AbsoluteUri;
                else
                    path = new Uri(Server.MapPath("~/Upload/Logo.png")).AbsoluteUri;

                p[2] = new ReportParameter("LogoPath", path, true);
                //-----------------------------------------------------

                this.ReportViewer1.LocalReport.SetParameters(p);

                ReportViewer1.LocalReport.Refresh();
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ddlMon.SelectedIndex = 0;
                ddlYear.SelectedIndex = 0;
                txtEmpCode.Text = "";
                ddlEmpName.SelectedIndex = 0;
                ddlMon.Focus();

            }
            catch (Exception ex)
            {

            }

        }


    }
}