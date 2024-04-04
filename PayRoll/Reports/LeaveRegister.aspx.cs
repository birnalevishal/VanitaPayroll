using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SqlClient;
using Microsoft.Reporting.WebForms;

namespace PayRoll.Reports
{
    public partial class LeaveRegister : System.Web.UI.Page
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
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                string strFromMnthCd =  ddlYear.SelectedValue;
                string strToMnthCd = ddlToYear.SelectedValue;

                //Last Day of Month/Year
                int Days = DateTime.DaysInMonth(Convert.ToInt32(strToMnthCd), 12);
                DateTime dt1 = Convert.ToDateTime(strToMnthCd + "-12-" + Days);

                dsRegister.udfLeaveregisterDataTable dt = new dsRegister.udfLeaveregisterDataTable();
                dsRegisterTableAdapters.udfLeaveregisterTableAdapter objDA = new dsRegisterTableAdapters.udfLeaveregisterTableAdapter();

                objDA.Fill(dt, Convert.ToInt16(Session["OrgID"]), Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlToYear.SelectedValue), dt1);
                DataTable dt3 = dt;

                DataView dv = new DataView(dt3);
                string filter = "";

                if(txtEmpCode.Text != "")
                {
                    filter += " Employeecd=" + txtEmpCode.Text + " AND "; 
                }

                if(filter.Length > 0)
                {
                    filter = filter.Remove(filter.Length - 4, 3);
                    dv.RowFilter = filter;
                }             

                ReportDataSource datasource = new ReportDataSource("LeaveRegister", dv.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/LeaveRegister.rdlc");

                ReportParameter[] p = new ReportParameter[4];
                p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                p[1] = new ReportParameter("FrMoncd", ddlYear.SelectedValue.ToString());
                p[2] = new ReportParameter("ToMoncd", ddlToYear.SelectedValue.ToString());


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
            catch (Exception ex)
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
            catch (Exception ex)
            {

            }

        }
               
        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlToYear.SelectedValue = ddlYear.SelectedValue;
        }

    }
}