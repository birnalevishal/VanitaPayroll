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
    public partial class LeaveBalanceRegister : System.Web.UI.Page
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

                int FrYearMonthcd = Convert.ToInt32(ddlYear.SelectedValue + ddlMnth.SelectedValue);
                int ToYearMonthcd = Convert.ToInt32(ddlToYear.SelectedValue + ddlToMnth.SelectedValue);

                dsRegister.udfLeaveBalancecAllLeaveDataTable objDT = new dsRegister.udfLeaveBalancecAllLeaveDataTable();
                DataTable objTemp = objDT.Clone();
                dsRegisterTableAdapters.udfLeaveBalancecAllLeaveTableAdapter objDA = new dsRegisterTableAdapters.udfLeaveBalancecAllLeaveTableAdapter();

                for (int i = FrYearMonthcd; i <= ToYearMonthcd; i++)
                {
                    if(i.ToString().Substring(4,2)=="12") {
                        i = Convert.ToInt32((Convert.ToInt32(i.ToString().Substring(0, 4)) + 1).ToString() + "01");
                    }

                    objDA.Fill(objDT, Session["OrgID"].ToString(), i.ToString());
                    objTemp.ImportRow(objDT.Rows[0]);
                }

                ReportDataSource rds = new ReportDataSource("LeaveBalanceAll", objTemp);
                ReportViewer1.LocalReport.DataSources.Add(rds);


                dsRegister.udfLeaveBalanceCalculationDataTable dt1 = new dsRegister.udfLeaveBalanceCalculationDataTable();
                dsRegisterTableAdapters.udfLeaveBalanceCalculationTableAdapter dt = new dsRegisterTableAdapters.udfLeaveBalanceCalculationTableAdapter();

                DateTime Docdate = new DateTime(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMnth.SelectedValue), 1);

                dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]), Docdate,  ddlYear.SelectedValue + ddlMnth.SelectedValue,  ddlToYear.SelectedValue + ddlToMnth.SelectedValue);
                DataTable dt3 = dt1;
                DataView dv = new DataView(dt3);
                
                ReportDataSource datasource = new ReportDataSource("LeaveBalanceRegister", dv.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/LeaveBalanceRegister.rdlc");

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