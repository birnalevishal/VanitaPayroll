using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using SqlClient;

namespace PayRoll.Reports
{
    public partial class PaidLeavesReport : System.Web.UI.Page
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
            ddlYear.Items.Insert(0, new ListItem("Select", "0"));

            ddlToYear.DataSource = objDT;
            ddlToYear.DataTextField = "Year";
            ddlToYear.DataValueField = "Year";
            ddlToYear.DataBind();
            ddlToYear.Items.Insert(0, new ListItem("Select", "0"));

           
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ddlMnth.SelectedIndex = 0;
            ddlToMnth.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;
            ddlToYear.SelectedIndex = 0;

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                int lastDay = DateTime.DaysInMonth(Convert.ToInt32(ddlToYear.SelectedValue), Convert.ToInt32(ddlToMnth.SelectedValue));
                string lastDate = ddlToYear.SelectedValue + "/" + ddlToMnth.SelectedValue + "/" + lastDay;

                dsRegister.udfLeaveBalanceCalculationDataTable dt1 = new dsRegister.udfLeaveBalanceCalculationDataTable();
                dsRegisterTableAdapters.udfLeaveBalanceCalculationTableAdapter dt = new dsRegisterTableAdapters.udfLeaveBalanceCalculationTableAdapter();

                dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]), Convert.ToDateTime(lastDate), ddlMnth.SelectedValue + ddlYear.SelectedValue, ddlToMnth.SelectedValue + ddlToYear.SelectedValue);
                DataTable dt3 = dt1;
                DataView dv = new DataView(dt3);


                ReportDataSource datasource = new ReportDataSource("PaidLeavesReport", dv.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/PaidLeavesReport.rdlc");

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
            catch (Exception ex)
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