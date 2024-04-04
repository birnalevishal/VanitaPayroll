﻿using Microsoft.Reporting.WebForms;
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
    public partial class SalaryRegister : System.Web.UI.Page
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
            string strQry = "SELECT Year  FROM M_Year Where IsActive='Y' ORDER BY Year desc";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlYear.DataSource = objDT;
            ddlYear.DataTextField = "Year";
            ddlYear.DataValueField = "Year";
            ddlYear.DataBind();

            ddlYear.Items.Insert(0, new ListItem("Select", "0000"));

            string strQry1 = "SELECT Category, Categcd FROM M_Category Where IsActive='Y' ORDER BY Category";
            DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            ddlCategory.DataSource = objDT1;
            ddlCategory.DataTextField = "Category";
            ddlCategory.DataValueField = "Categcd";
            ddlCategory.DataBind();

            ddlCategory.Items.Insert(0, new ListItem("Select", "0"));

            strQry1 = "SELECT Designation, Desigcd FROM M_Designation Where IsActive='Y' ORDER BY Designation";
            objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            ddlDesignation.DataSource = objDT1;
            ddlDesignation.DataTextField = "Designation";
            ddlDesignation.DataValueField = "Desigcd";
            ddlDesignation.DataBind();

            ddlDesignation.Items.Insert(0, new ListItem("Select", "0"));

            strQry1 = "SELECT LocationDep, LocDepCd FROM M_LocationDep Where IsActive='Y' ORDER BY LocationDep";
            objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            ddlDepartment.DataSource = objDT1;
            ddlDepartment.DataTextField = "LocationDep";
            ddlDepartment.DataValueField = "LocDepCd";
            ddlDepartment.DataBind();

            ddlDepartment.Items.Insert(0, new ListItem("Select", "0"));

            //strQry1 = "SELECT distinct(Employeecd), Employeename  FROM M_Emp Where IsActive='Y' and HODInchAppl='Y' ORDER BY Employeename";
            //objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            //ddlHOD.DataSource = objDT1;
            //ddlHOD.DataTextField = "Employeename";
            //ddlHOD.DataValueField = "Employeecd";
            //ddlHOD.DataBind();

            //ddlHOD.Items.Insert(0, new ListItem("Select", "0"));

            strQry1 = "SELECT Division, Divcd FROM M_Division Where IsActive='Y' ORDER BY Division";
            objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            ddlDivision.DataSource = objDT1;
            ddlDivision.DataTextField = "Division";
            ddlDivision.DataValueField = "Divcd";
            ddlDivision.DataBind();

            ddlDivision.Items.Insert(0, new ListItem("Select", "0"));

            strQry1 = "SELECT Skill, Skillcd FROM M_Skill Where IsActive='Y' ORDER BY [Skill]";
            objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            ddlSkill.DataSource = objDT1;
            ddlSkill.DataTextField = "Skill";
            ddlSkill.DataValueField = "Skillcd";
            ddlSkill.DataBind();

            ddlSkill.Items.Insert(0, new ListItem("Select", "0"));

            strQry1 = "SELECT Status, Stacd FROM M_Status Where IsActive='Y' ORDER BY [Status]";
            objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            ddlStatus.DataSource = objDT1;
            ddlStatus.DataTextField = "Status";
            ddlStatus.DataValueField = "Stacd";
            ddlStatus.DataBind();

            ddlStatus.Items.Insert(0, new ListItem("Select", "0"));

            strQry = "SELECT     StateCd, State FROM  M_State  WHERE (IsActive = 'Y') Order By State";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);

            chkState.DataSource = objDT;
            chkState.DataTextField = "State";
            chkState.DataValueField = "StateCd";
            chkState.DataBind();
            chkState.Items.Insert(0, new ListItem("All", "0"));
            chkState.SelectedIndex = 0;

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(ddlMon.SelectedIndex==0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Month'); ", true);
                    return;
                }
                if (ddlYear.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Year'); ", true);
                    return;
                }
                int lastDay = 0;
                lastDay = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMon.SelectedValue));
                string salaryDate = ddlYear.SelectedValue + "/" + ddlMon.SelectedValue + "/" + lastDay;

                string statechecked = "";
                foreach (ListItem st in chkState.Items)
                {
                    if (st.Selected)
                    {
                        statechecked = "1";
                    }
                }
                if (statechecked == "")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select State'); ", true);
                    return;
                }

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                DataSet1.SalaryRegisterDataTable dt1 = new DataSet1.SalaryRegisterDataTable();
                DataSet1TableAdapters.SalaryRegisterTableAdapter dt = new DataSet1TableAdapters.SalaryRegisterTableAdapter();

                if (chkAll.Checked)
                {
                    string strQry = "SELECT OrgId FROM M_Organization WHERE IsActive='Y'";
                    DataTable OrgIds = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                    foreach (DataRow item in OrgIds.Rows)
                    {
                        DataSet1.SalaryRegisterDataTable temp = new DataSet1.SalaryRegisterDataTable();
                        dt.Fill(temp,  Convert.ToInt32(item["OrgId"]), Convert.ToDateTime(salaryDate), ddlMon.SelectedValue + ddlYear.SelectedValue, ddlYear.SelectedValue + ddlMon.SelectedValue);
                        dt1.Merge(temp);
                    }
                }
                else
                {
                    dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]), Convert.ToDateTime(salaryDate), ddlMon.SelectedValue + ddlYear.SelectedValue, ddlYear.SelectedValue + ddlMon.SelectedValue);
                }

                //orignal
                //dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]),Convert.ToDateTime(salaryDate), ddlMon.SelectedValue+ddlYear.SelectedValue, ddlYear.SelectedValue+ddlMon.SelectedValue);

                DataTable dt3 = dt1;
                DataView dv = new DataView(dt3);
                string filter = "";
                
                if (ddlMon.SelectedIndex != 0 && ddlYear.SelectedIndex!=0)
                {
                    filter += " MonYrcd = " + ddlMon.SelectedValue +ddlYear.SelectedValue + " and ";
                }

                //if (ddlHOD.SelectedIndex != 0)
                //{
                //    filter += " hodid = " + ddlHOD.SelectedValue + " and ";
                //}
                if (ddlCategory.SelectedIndex != 0)
                {
                    filter += " catid = " + ddlCategory.SelectedValue + " and ";
                }

                if (ddlDepartment.SelectedIndex != 0)
                {
                    filter += " deptid=" + ddlDepartment.SelectedValue + " and ";
                }
                if (ddlDesignation.SelectedIndex != 0)
                {
                    filter += " desgid=" + ddlDesignation.SelectedValue + " and ";
                }
                if (ddlDivision.SelectedIndex != 0)
                {
                    filter += " Divid=" + ddlDivision.SelectedValue + " and ";
                }
                if (ddlSkill.SelectedIndex != 0)
                {
                    filter += " skillid=" + ddlSkill.SelectedValue + " and ";
                }
                if (ddlStatus.SelectedIndex != 0)
                {
                    filter += " staid=" + ddlStatus.SelectedValue + " and ";
                }
                if (ddlType.SelectedIndex != 0)
                {
                    if (ddlType.SelectedValue == "1")
                    {
                        filter += " Employeecd='" + txt1.Text + "' and ";
                    }
                    if (ddlType.SelectedValue == "2")
                    {
                        filter += " Employeename like'%" + txt1.Text + "%' and ";
                    }
                    //if (ddlType.SelectedValue == "3")
                    //{
                    //    filter += " OrigJoindate>= '" + Convert.ToDateTime(txt1.Text) + "' and OrigJoindate<='" + Convert.ToDateTime(txt2.Text) + "'";
                    //}
                    //if (ddlType.SelectedValue == "4")
                    //{
                    //    filter += " Leavedate>= '" + Convert.ToDateTime(txt1.Text) + "' and Leavedate<='" + Convert.ToDateTime(txt2.Text) + "'";
                    //}
                }
                if (chkState.SelectedIndex != 0)
                {
                    foreach (ListItem st in chkState.Items)
                    {
                        if (st.Selected)
                        {
                            filter += " WStatecd=" + st.Value + " or ";
                        }
                    }
                }
                if (filter.Length > 0)
                {
                    filter = filter.Remove(filter.Length - 4, 3);
                    dv.RowFilter = filter;
                }
                string AddHead1 = "", AddHead2 = "", AddHead3 = "";

                string strQry1 = "SELECT * FROM M_AddHeading Where OrgID=" + Convert.ToInt16(Session["OrgID"]);
                DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
                if (objDT1.Rows.Count > 0)
                {
                    if (objDT1.Rows[0]["Add1Heading"] != DBNull.Value)
                        AddHead1 = objDT1.Rows[0]["Add1Heading"].ToString();
                    if (objDT1.Rows[0]["Add2Heading"] != DBNull.Value)
                        AddHead2 = objDT1.Rows[0]["Add2Heading"].ToString();
                    if (objDT1.Rows[0]["Add3Heading"] != DBNull.Value)
                        AddHead3 = objDT1.Rows[0]["Add3Heading"].ToString();
                }

                ReportDataSource datasource = new ReportDataSource("DataSet2", dv.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptSalaryRegisterNew.rdlc");

                //ReportParameter[] p = new ReportParameter[5];
                //p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                //p[1] = new ReportParameter("monYrCd", ddlMon.SelectedItem.Text +" "  +  ddlYear.SelectedItem.Text, true);
                //p[2] = new ReportParameter("AddHead1", AddHead1, true);
                //p[3] = new ReportParameter("AddHead2", AddHead2, true);
                //p[4] = new ReportParameter("AddHead3", AddHead3, true);

                ReportParameter[] p = new ReportParameter[12];
                p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                p[1] = new ReportParameter("monYrCd", ddlMon.SelectedItem.Text + " " + ddlYear.SelectedItem.Text, true);
                if (ddlDepartment.SelectedIndex != 0)
                    p[2] = new ReportParameter("department", ddlDepartment.SelectedItem.Text, true);
                else
                    p[2] = new ReportParameter("department", "All", true);
                if (ddlDesignation.SelectedIndex != 0)
                    p[3] = new ReportParameter("designation", ddlDesignation.SelectedItem.Text, true);
                else
                    p[3] = new ReportParameter("designation", "All", true);
                if (ddlCategory.SelectedIndex != 0)
                    p[4] = new ReportParameter("category", ddlCategory.SelectedItem.Text, true);
                else
                    p[4] = new ReportParameter("category", "All", true);
                //if (ddlHOD.SelectedIndex != 0)
                //    p[5] = new ReportParameter("hod", ddlCategory.SelectedItem.Text, true);
                //else
                //    p[5] = new ReportParameter("hod", "All", true);
                if (ddlDivision.SelectedIndex != 0)
                    p[5] = new ReportParameter("division", ddlDivision.SelectedItem.Text, true);
                else
                    p[5] = new ReportParameter("division", "All", true);
                if (ddlSkill.SelectedIndex != 0)
                    p[6] = new ReportParameter("skill", ddlSkill.SelectedItem.Text, true);
                else
                    p[6] = new ReportParameter("skill", "All", true);
                if (ddlStatus.SelectedIndex != 0)
                    p[7] = new ReportParameter("status", ddlStatus.SelectedItem.Text, true);
                else
                    p[7] = new ReportParameter("status", "All", true);
                
                p[8] = new ReportParameter("AddHead1", AddHead1, true);
                p[9] = new ReportParameter("AddHead2", AddHead2, true);
                p[10] = new ReportParameter("AddHead3", AddHead3, true);

                //--- To Display Logo -----------------------------------
                ReportViewer1.LocalReport.EnableExternalImages = true;
                string strqry = "select LogoPath from M_Organization where OrgId=" + Convert.ToInt32(Session["OrgID"]);
                DataTable objDTP = SqlHelper.ExecuteDataTable(strqry, AppGlobal.strConnString);
                string path = "";

                if (objDTP.Rows[0]["LogoPath"] != DBNull.Value)
                    path = new Uri(Server.MapPath(objDTP.Rows[0]["LogoPath"].ToString())).AbsoluteUri;
                else
                    path = new Uri(Server.MapPath("~/Upload/Logo.png")).AbsoluteUri;

                p[11] = new ReportParameter("LogoPath", path, true);
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
                ddlMon.SelectedIndex = 0;
                ddlYear.SelectedIndex = 0;
                ReportViewer1.LocalReport.DataSources.Clear();
                ddlCategory.SelectedIndex = 0;
                ddlDepartment.SelectedIndex = 0;
                ddlDesignation.SelectedIndex = 0;
                ddlDivision.SelectedIndex = 0;
                ddlSkill.SelectedIndex = 0;
                ddlStatus.SelectedIndex = 0;
                txt1.Text = "";
                txt2.Text = "";
                ddlType.SelectedIndex = 0;
                //txt1.Visible = false;
                //txt2.Visible = false;
                //lbl1.Text = "";
                //lbl2.Text = "";
                //lbl1.Visible = false;
                //lbl2.Visible = false;
                chkState.SelectedIndex = 0;
                
                ddlMon.Focus();
            }
            catch(Exception ex)
            {

            }

        }
    }
}