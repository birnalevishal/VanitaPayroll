using Microsoft.Reporting.WebForms;
using SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PayRoll.Reports
{
    public partial class EmployeeJoinLeft : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
                ddlFrMon.SelectedValue = Convert.ToInt16(Convert.ToDateTime(DateTime.Now).Month).ToString("00");
                ddlToMon.SelectedValue = Convert.ToInt16(Convert.ToDateTime(DateTime.Now).Month).ToString("00");
                ddlFrYear.SelectedValue = Convert.ToInt16(Convert.ToDateTime(DateTime.Now).Year).ToString("0000");
                ddlToYear.SelectedValue = Convert.ToInt16(Convert.ToDateTime(DateTime.Now).Year).ToString("0000");
            }
        }
        private void BindData()
        {
            string strQry = "SELECT Year  FROM M_Year ORDER BY Year desc";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlFrYear.DataSource = objDT;
            ddlFrYear.DataTextField = "Year";
            ddlFrYear.DataValueField = "Year";
            ddlFrYear.DataBind();
            ddlFrYear.Items.Insert(0, new ListItem("Select", "0000"));

            ddlToYear.DataSource = objDT;
            ddlToYear.DataTextField = "Year";
            ddlToYear.DataValueField = "Year";
            ddlToYear.DataBind();
            ddlToYear.Items.Insert(0, new ListItem("Select", "0000"));


            string strQry1 = "SELECT OrgId, Organization FROM M_Organization Where IsActive='Y' ";
            DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            ddlOrg.DataSource = objDT1;
            ddlOrg.DataTextField = "Organization";
            ddlOrg.DataValueField = "OrgId";
            ddlOrg.DataBind();
            ddlOrg.Items.Insert(0, new ListItem("All Organizations", "0"));

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                DateTime FromDt = Convert.ToDateTime(ddlFrYear.SelectedValue + "/" + ddlFrMon.SelectedValue + "/01");
                int lastDay = DateTime.DaysInMonth(Convert.ToInt32(ddlToYear.SelectedValue), Convert.ToInt32(ddlToMon.SelectedValue));
                DateTime ToDt = Convert.ToDateTime(ddlToYear.SelectedValue + "/" + ddlToMon.SelectedValue + "/" + lastDay);

                DataTable objDT = new DataTable();
                string strFilter = "";
                string OrderBy = "";
                string strQry = "SELECT * FROM udfEmployeeInfo() WHERE 1=1 ";
                if (ddlType.SelectedIndex == 0)
                {
                    strFilter += " AND DatofJoin BETWEEN '" + Convert.ToDateTime(FromDt).ToString("dd MMM yyyy") + "' AND '" + Convert.ToDateTime(ToDt).ToString("dd MMM yyyy") + "' ";
                    OrderBy = " Order By DatofJoin Asc";
                }
                if (ddlType.SelectedIndex == 1)
                {
                    strFilter += " AND Leavedate BETWEEN '" + Convert.ToDateTime(FromDt).ToString("dd MMM yyyy") + "' AND '" + Convert.ToDateTime(ToDt).ToString("dd MMM yyyy") + "' ";
                    OrderBy = " Order By Leavedate Asc";
                }
                if (ddlOrg.SelectedIndex > 0)
                {
                    strFilter += " AND OrgId=" + ddlOrg.SelectedValue.ToString();
                }

                strQry = strQry + strFilter + OrderBy;

                objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                               
                ReportDataSource datasource = new ReportDataSource("EmployeeList", objDT);
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/EmployeeJoinLeftReport.rdlc");

                ReportParameter[] p = new ReportParameter[5];
                p[0] = new ReportParameter("OrgName", ddlOrg.SelectedItem.ToString(), true);
                p[1] = new ReportParameter("FrDate", ddlFrMon.SelectedItem.Text + " " + ddlFrYear.SelectedValue, true);
                p[2] = new ReportParameter("ToDate", ddlToMon.SelectedItem.Text + " " + ddlToYear.SelectedValue, true);
                p[3] = new ReportParameter("ReportType", ddlType.SelectedItem.ToString(), true);

                //--- To Display Logo -----------------------------------
                ReportViewer1.LocalReport.EnableExternalImages = true;
                string strqry = "select LogoPath from M_Organization where OrgId=" + Convert.ToInt32(Session["OrgID"]);
                DataTable objDTP = SqlHelper.ExecuteDataTable(strqry, AppGlobal.strConnString);
                string path = "";

                if (objDTP.Rows[0]["LogoPath"] != DBNull.Value)
                    path = new Uri(Server.MapPath(objDTP.Rows[0]["LogoPath"].ToString())).AbsoluteUri;
                else
                    path = new Uri(Server.MapPath("~/Upload/Logo.png")).AbsoluteUri;

                p[4] = new ReportParameter("LogoPath", path, true);
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
                
            }
            catch (Exception ex)
            {

            }

        }


    }
}