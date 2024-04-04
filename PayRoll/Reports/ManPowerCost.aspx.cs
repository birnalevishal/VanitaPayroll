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
    public partial class ManPowerCost : System.Web.UI.Page
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
                //DataTable dt1= calculation();
                
                if (ddlMnth.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Month'); ", true);
                    return;
                }
                if (ddlYear.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Year'); ", true);
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

                //for Wages Data
                dsMaster.udf_ManPowerCostDataTable dt1 = new dsMaster.udf_ManPowerCostDataTable();
                dsMaster.udf_ManPowerCostDataTable dt2 = new dsMaster.udf_ManPowerCostDataTable();
                dsMasterTableAdapters.udf_ManPowerCostTableAdapter dt = new dsMasterTableAdapters.udf_ManPowerCostTableAdapter();

                string[] mnthcd = Moncd.Split(',');

                foreach (string monthcd in mnthcd)
                {
                    //Last Day of Month/Year
                    int Days = DateTime.DaysInMonth(Convert.ToInt32(monthcd.Substring(2, 4)), Convert.ToInt32(monthcd.Substring(0, 2)));
                    DateTime date = Convert.ToDateTime(monthcd.Substring(2, 4) + "-" + monthcd.Substring(0, 2) + "-" + Days);

                    dt.Fill(dt1, monthcd, date);
                    dt2.Merge(dt1);
                }

                DataTable dt3 = dt2;

                DataView dv = new DataView(dt3);

                //For Nashta Insentive
                dsMaster.udf_ManPowerCostNashtaDataTable dt4 = new dsMaster.udf_ManPowerCostNashtaDataTable();
                dsMaster.udf_ManPowerCostNashtaDataTable dt5 = new dsMaster.udf_ManPowerCostNashtaDataTable();
                dsMasterTableAdapters.udf_ManPowerCostNashtaTableAdapter dt6 = new dsMasterTableAdapters.udf_ManPowerCostNashtaTableAdapter();

                foreach (string monthcd in mnthcd)
                {
                    //Last Day of Month/Year
                    int Days = DateTime.DaysInMonth(Convert.ToInt32(monthcd.Substring(2, 4)), Convert.ToInt32(monthcd.Substring(0, 2)));
                    DateTime date = Convert.ToDateTime(monthcd.Substring(2, 4) + "-" + monthcd.Substring(0, 2) + "-" + Days);

                    dt6.Fill(dt4, monthcd, date);
                    dt5.Merge(dt4);
                }

                DataTable dt7 = dt5;

                DataView dv1 = new DataView(dt7);

                ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                ReportDataSource datasourceNashta = new ReportDataSource("DataSet2", dv1.ToTable());

                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.DataSources.Add(datasourceNashta);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptManPowerCost.rdlc");

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

        protected DataTable calculation()
        {
            int lastDay = 0; ;
            DateTime fromDt = Convert.ToDateTime("01/" + ddlMnth.SelectedValue + "/" + ddlYear.SelectedValue);
            lastDay = DateTime.DaysInMonth(Convert.ToInt32(ddlToYear.SelectedValue), Convert.ToInt32(ddlToMnth.SelectedValue));
            DateTime toDt = Convert.ToDateTime(lastDay + "/" + ddlToMnth.SelectedValue + "/" + ddlToYear.SelectedValue);
            DateTime prvQFromDt=Convert.ToDateTime(Convert.ToDateTime(fromDt.AddMonths(-3)));
            DateTime prvQToDt = Convert.ToDateTime(Convert.ToDateTime(fromDt.AddDays(-1)));

            DataTable dtReport = new DataTable();
            dtReport.Columns.Add("empName", typeof(string));
            dtReport.Columns.Add("empCode", typeof(string));
            dtReport.Columns.Add("Male", typeof(string));
            dtReport.Columns.Add("Female", typeof(string));
            dtReport.Columns.Add("Total", typeof(string));
            DataRow dr;
            int opnMaleCount , opnFemaleCount , opnCount, newCount, print, printNew;
            opnMaleCount = opnFemaleCount = opnCount = newCount = print= printNew = 0;

            string qryOpnEmp = "select count(*) as count from M_Emp where orgID=" + Convert.ToInt16(Session["OrgID"]) + " and leaveDate is null  and  DatofJoin<='" + Convert.ToDateTime(prvQToDt).ToString("dd MMM yyyy") + "' group by gendercd";
            DataTable objOpnEmp = SqlHelper.ExecuteDataTable(qryOpnEmp, AppGlobal.strConnString);
            if(objOpnEmp.Rows.Count>0)
            {
                dr = dtReport.NewRow();
                opnMaleCount = Convert.ToInt16(objOpnEmp.Rows[0]["count"]);
                opnFemaleCount = Convert.ToInt16(objOpnEmp.Rows[1]["count"]);
                opnCount = opnMaleCount + opnFemaleCount;
                dr["empName"] = Convert.ToDateTime(prvQFromDt).ToString("dd/MM/yyyy") + " To " + Convert.ToDateTime(prvQToDt).ToString("dd/MM/yyyy");
                dr["empCode"] = "";
                dr["Male"] = opnMaleCount;
                dr["Female"] = opnFemaleCount;
                dr["Total"] = opnCount;
                dtReport.Rows.Add(dr);
            }

            lastDay = 0;
            DateTime lastDate=DateTime.Now;
            //DateTime lastDate = toDt;

            int totalMonths = 0;//((Convert.ToInt16(ddlYear.SelectedValue) - Convert.ToInt16(ddlToYear.SelectedValue)) * 12) + (Convert.ToInt16(ddlToMnth.SelectedValue) - Convert.ToInt16(ddlMnth.SelectedValue));
           
            int months1 = fromDt.Year * 12 + fromDt.Month;
            int months2 = toDt.Year * 12 + toDt.Month;

            totalMonths = months2 - months1;

            totalMonths += 1;
            string qryNewJoin = "select employeecd, employeename,Gendercd, DatofJoin, leaveDate from M_Emp where  orgID=" + Convert.ToInt16(Session["OrgID"]) + " and (DatofJoin>='" + fromDt + "' and DatofJoin<='" + Convert.ToDateTime(toDt).ToString("dd MMM yyyy") + "') or orgID=" + Convert.ToInt16(Session["OrgID"]) + " and  (leaveDate>='" + Convert.ToDateTime(fromDt).ToString("dd MMM yyyy") + "' and leaveDate<='" + Convert.ToDateTime(toDt).ToString("dd MMM yyyy") + "') order by DatofJoin,leaveDate";
            DataTable objNewJoin= SqlHelper.ExecuteDataTable(qryNewJoin, AppGlobal.strConnString);
            if(objNewJoin.Rows.Count>0)
            {
                for (int m = 1; m <= totalMonths; m++)
                {
                    if (m == 1)
                    {
                        lastDay = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMnth.SelectedValue));
                        lastDate = Convert.ToDateTime(lastDay + "/" + ddlMnth.SelectedValue + "/" + ddlYear.SelectedValue);
                        
                    }
                    else
                    {
                        fromDt = lastDate.AddDays(1);
                        lastDay = DateTime.DaysInMonth(Convert.ToInt32(fromDt.Year), Convert.ToInt32(fromDt.Month));
                        lastDate = Convert.ToDateTime(lastDay + "/" + fromDt.Month + "/" + fromDt.Year);
                    }
                    printNew = 1;
                    print = 0;

                    //Join
                    for (int i = 0; i < objNewJoin.Rows.Count; i++)
                    {
                        if (objNewJoin.Rows[i]["DatofJoin"] != DBNull.Value)
                        {
                            if (Convert.ToDateTime(objNewJoin.Rows[i]["DatofJoin"]) >= fromDt && Convert.ToDateTime(objNewJoin.Rows[i]["DatofJoin"]) <= lastDate)
                            {
                                if(printNew == 1)
                                {
                                    //Join Heading
                                    dr = dtReport.NewRow();
                                    dr["empName"] = "Joining " + fromDt.ToString("MMMM") + " " + fromDt.Year;
                                    dr["empCode"] = "";
                                    dr["Male"] = "";
                                    dr["Female"] = "";
                                    dr["Total"] = "";
                                    dtReport.Rows.Add(dr);
                                    printNew = 0;
                                }
                                dr = dtReport.NewRow();
                                dr["empName"] = objNewJoin.Rows[i]["employeename"].ToString();
                                dr["empCode"] = objNewJoin.Rows[i]["employeecd"].ToString();
                                if (objNewJoin.Rows[i]["Gendercd"].ToString() == "1")
                                {
                                    dr["Male"] = "1";
                                    opnMaleCount += 1;
                                }
                                else
                                {
                                    dr["Female"] = "1";
                                    opnFemaleCount += 1;
                                }
                                dr["Total"] = "";
                                opnCount += 1;
                                dtReport.Rows.Add(dr);
                                print = 1;
                            }
                            else
                            {
                                printNew = 1;
                            }
                        }
                        else
                        {
                            printNew = 1;
                        }
                    }

                    //Total After Join
                    if (print==1)
                    {
                        dr = dtReport.NewRow();
                        dr["empName"] = "Total";
                        dr["empCode"] = "";
                        dr["Male"] = opnMaleCount;
                        dr["Female"] = opnFemaleCount;
                        dr["Total"] = opnCount;
                        dtReport.Rows.Add(dr);
                        print = 0;
                    }

                    //left 
                    printNew = 1;
                    print = 0;

                    for (int i = 0; i < objNewJoin.Rows.Count; i++)
                    {
                        if (objNewJoin.Rows[i]["leaveDate"] != DBNull.Value)
                        {
                            if (Convert.ToDateTime(objNewJoin.Rows[i]["leaveDate"]) >= fromDt && Convert.ToDateTime(objNewJoin.Rows[i]["leaveDate"]) <= lastDate)
                            {
                                if (printNew == 1)
                                {
                                    //Leaft Heading
                                    dr = dtReport.NewRow();
                                    dr["empName"] = "Left " + fromDt.ToString("MMMM") + " " + fromDt.Year;
                                    dr["empCode"] = "";
                                    dr["Male"] = "";
                                    dr["Female"] = "";
                                    dr["Total"] = "";
                                    dtReport.Rows.Add(dr);
                                    printNew = 0;
                                }
                                dr = dtReport.NewRow();
                                dr["empName"] = objNewJoin.Rows[i]["employeename"].ToString();
                                dr["empCode"] = objNewJoin.Rows[i]["employeecd"].ToString();
                                if (objNewJoin.Rows[i]["Gendercd"].ToString() == "1")
                                {
                                    dr["Male"] = "1";
                                    opnMaleCount -= 1;
                                }
                                else
                                {
                                    dr["Female"] = "1";
                                    opnFemaleCount -= 1;
                                }
                                dr["Total"] = "";
                                opnCount -= 1;
                                dtReport.Rows.Add(dr);
                                print = 1;
                            }
                            else
                            {
                                //printNew = 1;
                            }
                        }
                        else
                        {
                            //printNew = 1;
                        }
                    }

                    //Total After Left
                    if (print == 1)
                    {
                        dr = dtReport.NewRow();
                        dr["empName"] = "Total";
                        dr["empCode"] = "";
                        dr["Male"] = opnMaleCount;
                        dr["Female"] = opnFemaleCount;
                        dr["Total"] = opnCount;
                        dtReport.Rows.Add(dr);
                        print = 0;
                    }
                }
            }
            return dtReport;
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
    }
}