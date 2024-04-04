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
    public partial class ER1Calculation : System.Web.UI.Page
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
                DataTable dt1= calculation();
                //dsRegister.LeaveBalanceDataTable dt1 = new dsRegister.LeaveBalanceDataTable();
                //dsRegisterTableAdapters.LeaveBalanceTableAdapter dt = new dsRegisterTableAdapters.LeaveBalanceTableAdapter();

                //dt.Fill(dt1,Convert.ToInt16(Session["OrgID"]), ddlMnth.SelectedValue+ddlYear.SelectedValue, ddlToMnth.SelectedValue+ddlToYear.SelectedValue);
                //DataTable dt3 = dt1;

                DataView dv = new DataView(dt1);

                ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptER1Calculation.rdlc");

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

            //Opening For Previous Quarter
            string qryOpnEmp = "select count(*) as count, gendercd from M_Emp where orgID=" + Convert.ToInt16(Session["OrgID"]) + " and( leaveDate is null or leaveDate>'" + Convert.ToDateTime(prvQToDt).ToString("dd MMM yyyy") + "') and  DatofJoin<='" + Convert.ToDateTime(prvQToDt).ToString("dd MMM yyyy") + "' group by gendercd";
            DataTable objOpnEmp = SqlHelper.ExecuteDataTable(qryOpnEmp, AppGlobal.strConnString);
            if(objOpnEmp.Rows.Count>0)
            {
                dr = dtReport.NewRow();
                if(objOpnEmp.Rows.Count==1 && objOpnEmp.Rows[0]["gendercd"].ToString() == "1")
                {
                    opnMaleCount = Convert.ToInt16(objOpnEmp.Rows[0]["count"]);
                }
                if (objOpnEmp.Rows.Count == 1 && objOpnEmp.Rows[0]["gendercd"].ToString() == "2")
                {
                    opnFemaleCount = Convert.ToInt16(objOpnEmp.Rows[0]["count"]);
                }

                if (objOpnEmp.Rows.Count == 2 && objOpnEmp.Rows[0]["gendercd"].ToString() == "1")
                {
                    opnMaleCount = Convert.ToInt16(objOpnEmp.Rows[0]["count"]);
                }
                if (objOpnEmp.Rows.Count == 2 && objOpnEmp.Rows[1]["gendercd"].ToString() == "2")
                {
                    opnFemaleCount = Convert.ToInt16(objOpnEmp.Rows[1]["count"]);
                }
                
                
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
            string qryNewJoin = "select employeecd, employeename,Gendercd, DatofJoin, leaveDate from M_Emp where  orgID=" + Convert.ToInt16(Session["OrgID"]) + " and (DatofJoin>='" + Convert.ToDateTime(fromDt).ToString("dd MMM yyyy") + "' and DatofJoin<='" + Convert.ToDateTime(toDt).ToString("dd MMM yyyy") + "') or orgID=" + Convert.ToInt16(Session["OrgID"]) + " and  (leaveDate>='" + Convert.ToDateTime(fromDt).ToString("dd MMM yyyy") + "' and leaveDate<='" + Convert.ToDateTime(toDt).ToString("dd MMM yyyy") + "') order by DatofJoin,leaveDate";
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
    }
}