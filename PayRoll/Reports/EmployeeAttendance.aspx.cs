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
    public partial class EmployeeAttendance : System.Web.UI.Page
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
            //ddlFrYear.Items.Insert(0, new ListItem("Select", "0000"));

            ddlToYear.DataSource = objDT;
            ddlToYear.DataTextField = "Year";
            ddlToYear.DataValueField = "Year";
            ddlToYear.DataBind();
            //ddlToYear.Items.Insert(0, new ListItem("Select", "0000"));


            string strQry1 = "SELECT OrgId, Organization FROM M_Organization Where IsActive='Y' ";
            DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            ddlOrg.DataSource = objDT1;
            ddlOrg.DataTextField = "Organization";
            ddlOrg.DataValueField = "OrgId";
            ddlOrg.DataBind();
            //ddlOrg.Items.Insert(0, new ListItem("All Organizations", "0"));

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
               
                //Get Mnth Code for Selected Month & Year
                string Moncd = ddlFrMon.SelectedValue + ddlFrYear.SelectedValue;

                if (ddlFrMon.SelectedValue == ddlToMon.SelectedValue && ddlFrYear.SelectedValue == ddlToYear.SelectedValue)
                {
                    Moncd = ddlToMon.SelectedValue + ddlFrYear.SelectedValue;
                }
                else if (Convert.ToInt32(ddlToYear.SelectedValue) < Convert.ToInt32(ddlFrYear.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select To Year Greater than or equal to From Year'); ", true);
                    return;
                }
                else
                {
                    Moncd = GetMnthCd();
                }

                DateTime maxEndDt = DateTime.Now;
                string[] mnthcd = Moncd.Split(',');
                foreach (string monthcd in mnthcd)
                {
                    //Last Day of Month/Year
                    int Days = DateTime.DaysInMonth(Convert.ToInt32(monthcd.Substring(2, 4)), Convert.ToInt32(monthcd.Substring(0, 2)));
                    maxEndDt = Convert.ToDateTime(monthcd.Substring(2, 4) + "-" + monthcd.Substring(0, 2) + "-" + Days);
                }

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                dsRegister.udfEmployeeAttendanceDataTable dt = new dsRegister.udfEmployeeAttendanceDataTable();
                dsRegisterTableAdapters.udfEmployeeAttendanceTableAdapter da = new dsRegisterTableAdapters.udfEmployeeAttendanceTableAdapter();
                da.Fill(dt, Convert.ToInt16(ddlOrg.SelectedValue), Moncd, maxEndDt);
                               
                DataTable objDT = new DataTable();
                objDT = dt;

                ReportDataSource datasource = new ReportDataSource("EmployeeAttendance", objDT);
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/EmployeeAttendance.rdlc");

                ReportParameter[] p = new ReportParameter[4];
                p[0] = new ReportParameter("OrgName", ddlOrg.SelectedItem.ToString(), true);
                p[1] = new ReportParameter("FrDate", ddlFrMon.SelectedItem.Text + " " + ddlFrYear.SelectedValue, true);
                p[2] = new ReportParameter("ToDate", ddlToMon.SelectedItem.Text + " " + ddlToYear.SelectedValue, true);

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

        private string GetMnthCd()
        {
            string strMnthCd = "";
            int sMnth = ddlFrMon.SelectedIndex;
            int sYr = Convert.ToInt32(ddlFrYear.SelectedValue);
            int eMnth = ddlToMon.SelectedIndex;
            int eYr = Convert.ToInt32(ddlToYear.SelectedValue);
            int nMnthCnt = 0;
            int nYrCnt = 0;
            int Y = 1;
            int M = 1;

            if (sMnth < eMnth)
            {
                nMnthCnt = (eMnth - sMnth) + 1;
            }
            else
            {
                if (sMnth == eMnth)
                {
                    nMnthCnt = 13;
                }
                else
                {
                    nMnthCnt = (12 - (sMnth - eMnth)) + 1;
                }
            }

            if (sYr < eYr)
            {
                nYrCnt = eYr - sYr;
                if (nYrCnt > 1)
                {
                    nMnthCnt = nMnthCnt + ((nYrCnt - 1) * 12);
                }
            }

            if (sYr == eYr)
            {
                nYrCnt = 1;
            }

            while (Y <= nYrCnt)
            {
                while (M <= nMnthCnt)
                {
                    if (sMnth < 10)
                    {
                        strMnthCd += "0" + sMnth + sYr + ",";
                    }
                    else
                    {
                        strMnthCd += "" + sMnth + sYr + ",";
                    }
                    if (sMnth == 12)
                    {
                        sMnth = 1;
                        sYr += 1;
                        M += 1;
                        break;
                    }
                    else
                    {
                        sMnth += 1;
                        M += 1;
                    }
                }
                if (M > nMnthCnt)
                {
                    Y += 1;
                }
            }

            strMnthCd = strMnthCd.Remove(strMnthCd.Length - 1, 1);

            return strMnthCd;
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