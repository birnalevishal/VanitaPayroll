using SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace PayRoll.Reports
{
    public partial class BonusRegister : System.Web.UI.Page
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
            ReportDS.BonusYearsDataTable objDT = new ReportDS.BonusYearsDataTable();
            ReportDSTableAdapters.BonusYearsTableAdapter obj = new ReportDSTableAdapters.BonusYearsTableAdapter();
            obj.Fill(objDT, Convert.ToInt32(Session["OrgID"].ToString()));
            
            ddlYear.DataSource = objDT;
            ddlYear.DataTextField = "Dt";
            ddlYear.DataValueField = "moncd";
            ddlYear.DataBind();
            ddlYear.Items.Insert(0, new ListItem("Select", ""));

        }

        private string GetMnthCd(string sMoncd, string eMoncd)
        {
            string strMnthCd = "";
            int sMnth = Convert.ToInt32(sMoncd.Substring(0,2));
            int sYr = Convert.ToInt32(sMoncd.Substring(2, 4));
            int eMnth = Convert.ToInt32(eMoncd.Substring(0, 2));
            int eYr = Convert.ToInt32(eMoncd.Substring(2, 4));
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

        private string MonthName(string strMoncd)
        {
            string strName = "";

            switch (strMoncd)
            {
                case "01":
                    strName = "January";
                    break;
                case "02":
                    strName = "February";
                    break;
                case "03":
                    strName = "March";
                    break;
                case "04":
                    strName = "April";
                    break;
                case "05":
                    strName = "May";
                    break;
                case "06":
                    strName = "June";
                    break;
                case "07":
                    strName = "Jul";
                    break;
                case "08":
                    strName = "August";
                    break;
                case "09":
                    strName = "September";
                    break;
                case "10":
                    strName = "October";
                    break;
                case "11":
                    strName = "November";
                    break;
                case "12":
                    strName = "December";
                    break;

            }

            return strName;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlYear.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Year'); ", true);
                return;
            }

            //Get Mnth Code for Selected Month & Year
            string[] arr = ddlYear.SelectedValue.Split('-');
            string Moncd = GetMnthCd(arr[0],arr[1]);

            //Last Day of Month/Year
            int Days = DateTime.DaysInMonth(Convert.ToInt32(arr[1].Substring(2, 4)), Convert.ToInt32(arr[1].Substring(0, 2)));
            DateTime dt = Convert.ToDateTime(arr[1].Substring(2, 4) + "-" + arr[1].Substring(0, 2) + "-" + Days);

            int nYrCd = Convert.ToInt32(Session["YearID"]);
            string strQry = "SELECT Frdate FROM  M_FinanceYear WHERE YearId=" + nYrCd;
            DateTime frdt = Convert.ToDateTime(SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString));

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.ProcessingMode = ProcessingMode.Local;

            ReportDS.BonusRegisterDataTable objDT = new ReportDS.BonusRegisterDataTable();
            ReportDSTableAdapters.BonusRegisterTableAdapter obj = new ReportDSTableAdapters.BonusRegisterTableAdapter();

            obj.Fill(objDT, Convert.ToInt32(Session["OrgID"].ToString()), dt, Moncd, arr[0], arr[1]);
            DataTable dt1 = objDT;

            ReportDataSource datasource = new ReportDataSource("BonusRegister", dt1);
            ReportViewer1.LocalReport.DataSources.Add(datasource);
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/BonusRegister.rdlc");

            ReportParameter p1 = new ReportParameter("para1", ddlYear.SelectedItem.Text);
            ReportParameter p2 = new ReportParameter("frdt", frdt.ToString("dd MMM yyyy"));
            ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2 });

            ReportViewer1.LocalReport.Refresh();
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ddlYear.SelectedIndex = 0;
        }
    }
}