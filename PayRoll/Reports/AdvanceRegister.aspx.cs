using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SqlClient;

namespace PayRoll.Reports
{
    public partial class AdvanceRegister : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                ddlMnth.Focus();
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

            strQry = "SELECT YearId, CONVERT(VARCHAR(10), Frdate, 103) +'-' +  CONVERT(VARCHAR(10), Todate, 103) as date  FROM M_FinanceYear Where IsActive='Y' ORDER BY YearId desc";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlFinanceYear.DataSource = objDT;
            ddlFinanceYear.DataTextField = "date";
            ddlFinanceYear.DataValueField = "YearId";
            ddlFinanceYear.DataBind();

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlMnth.SelectedIndex==0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select From Month'); ", true);
                    return;
                }
                if (ddlYear.SelectedIndex==0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select From Year'); ", true);
                    return;
                }
                if (ddlToMnth.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select To Month'); ", true);
                    return;
                }
                if (ddlToYear.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select To Year'); ", true);
                    return;
                }
                


                //Get Mnth Code for Selected Month & Year
                string Moncd = ddlMnth.SelectedValue + ddlYear.SelectedValue;

                if (ddlMnth.SelectedValue == ddlToMnth.SelectedValue && ddlYear.SelectedValue == ddlToYear.SelectedValue)
                {
                    Moncd = ddlMnth.SelectedValue + ddlYear.SelectedValue;
                }
                else if (Convert.ToInt32(ddlToYear.SelectedValue) < Convert.ToInt32(ddlYear.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select To Year Greater than or equal to From Year'); ", true);
                    return;
                }
                else
                {
                    Moncd = GetMnthCd();
                }
                DateTime fromDate=DateTime.Now, toDate=DateTime.Now;
                string strQry = "SELECT * FROM M_FinanceYear Where YearID=" + ddlFinanceYear.SelectedValue;
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    fromDate = Convert.ToDateTime(objDT.Rows[0]["FrDate"]);
                    toDate = Convert.ToDateTime(objDT.Rows[0]["ToDate"]);
                }
                //int lastDay = 0;
                //lastDay= DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMnth.SelectedValue));
                //string fDate = "01/" + ddlMnth.SelectedValue + "/" + ddlYear.SelectedValue;
                //string tDate = lastDay + "/" + ddlMnth.SelectedValue + "/" + ddlYear.SelectedValue;

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                

                if (rblType.SelectedValue == "1")
                {
                    dsRegister.Udf_AdvanceSummaryDataTable dt1 = new dsRegister.Udf_AdvanceSummaryDataTable();
                    dsRegisterTableAdapters.Udf_AdvanceSummaryTableAdapter dt = new dsRegisterTableAdapters.Udf_AdvanceSummaryTableAdapter();
                    //dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]), ddlMnth.SelectedValue + ddlYear.SelectedValue,  ddlToMnth.SelectedValue+ ddlToYear.SelectedValue, Moncd, ddlYear.SelectedValue+ddlMnth.SelectedValue  );

                    //dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]),  ddlToYear.SelectedValue + ddlToMnth.SelectedValue, fromDate, toDate,  ddlYear.SelectedValue + ddlMnth.SelectedValue, Moncd);
                    dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]), fromDate, toDate, ddlMnth.SelectedValue + ddlYear.SelectedValue, ddlToMnth.SelectedValue + ddlToYear.SelectedValue);
                    DataTable dt3 = dt1;
                    DataView dv = new DataView(dt3);
                    
                    ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptAdvanceRegister.rdlc");

                }
                else
                {
                    dsRegister.Udf_AdvanceDetailDataTable dt1 = new dsRegister.Udf_AdvanceDetailDataTable();
                    dsRegisterTableAdapters.Udf_AdvanceDetailTableAdapter dt = new dsRegisterTableAdapters.Udf_AdvanceDetailTableAdapter();
                    //dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]), ddlMnth.SelectedValue + ddlYear.SelectedValue,  ddlToMnth.SelectedValue+ ddlToYear.SelectedValue, Moncd, ddlYear.SelectedValue+ddlMnth.SelectedValue  );

                    //dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]), Moncd, fromDate, toDate, ddlYear.SelectedValue + ddlMnth.SelectedValue, ddlToYear.SelectedValue + ddlToMnth.SelectedValue);
                    dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]), fromDate, toDate,  ddlToMnth.SelectedValue+ ddlToYear.SelectedValue,  ddlMnth.SelectedValue+ ddlYear.SelectedValue);
                    DataTable dt3 = dt1;
                    DataView dv = new DataView(dt3);

                    ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptAdvanceRegisterDet.rdlc");

                }

                ReportParameter[] p = new ReportParameter[3];
                p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                p[1] = new ReportParameter("asOnDate", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy"), true);
                //--- To Display Logo -----------------------------------
                ReportViewer1.LocalReport.EnableExternalImages = true;
                string strqry = "select LogoPath from M_Organization where OrgId=" + Convert.ToInt32(Session["OrgID"]);
                DataTable objDTP = SqlHelper.ExecuteDataTable(strqry, AppGlobal.strConnString);
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
                throw ex;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ddlMnth.SelectedIndex = 0;
                ddlYear.SelectedIndex = 0;
                ddlToMnth.SelectedIndex = 0;
                ddlToYear.SelectedIndex = 0;
                ReportViewer1.LocalReport.DataSources.Clear();
                ddlMnth.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetMnthCd()
        {
            string strMnthCd = "";
            int sMnth = ddlMnth.SelectedIndex;
            int sYr = Convert.ToInt32(ddlYear.SelectedValue);
            int eMnth = ddlToMnth.SelectedIndex;
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
        protected void ddlMnth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMnth.SelectedIndex > 0)
            {
                ddlToMnth.SelectedIndex = ddlMnth.SelectedIndex;
            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlYear.SelectedIndex > 0)
            {
                ddlToYear.SelectedIndex = ddlYear.SelectedIndex;
            }
        }
    }
}