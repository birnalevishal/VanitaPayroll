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
    public partial class MonthlyEarningDeduction : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             if(!IsPostBack)
            {
                BindData();
                btnCancel_Click(sender, e);
            }
        }
        private void BindData()
        {
            string strQry = "SELECT Year  FROM M_Year Where IsActive='Y' ORDER BY Year desc";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlYear.DataSource = objDT;
            ddlYear.DataTextField = "Year";
            ddlYear.DataValueField = "Year";

            ddlToYear.DataSource = objDT;
            ddlToYear.DataTextField = "Year";
            ddlToYear.DataValueField = "Year";

            ddlYear.DataBind();
            ddlToYear.DataBind();

            ddlYear.Items.Insert(0, new ListItem("Select", "0000"));
            ddlToYear.Items.Insert(0, new ListItem("Select", "0000"));

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

            strQry1 = "SELECT distinct(Employeecd), Employeename  FROM M_Emp Where IsActive='Y' and HODInchAppl='Y' ORDER BY Employeename";
            objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            ddlHOD.DataSource = objDT1;
            ddlHOD.DataTextField = "Employeename";
            ddlHOD.DataValueField = "Employeecd";
            ddlHOD.DataBind();

            ddlHOD.Items.Insert(0, new ListItem("Select", "0"));

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
                //Get Mnth Code for Selected Month & Year
                string Moncd = ddlMon.SelectedValue + ddlYear.SelectedValue;

                if (ddlMon.SelectedValue == ddlToMonth.SelectedValue && ddlYear.SelectedValue == ddlToYear.SelectedValue)
                {
                    Moncd = ddlMon.SelectedValue + ddlYear.SelectedValue;
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

                string statechecked = "";
                foreach (ListItem st in chkState.Items)
                {
                    if (st.Selected)
                    {
                        statechecked = "1";
                    }
                }
                if (ddlEarningFeilds.SelectedValue == "ProfTax")
                {
                    if (statechecked == "")
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select State'); ", true);
                        return;
                    }
                }


                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                dsRegister.udf_EarningDeduction1DataTable dt1 = new dsRegister.udf_EarningDeduction1DataTable();

                dsRegisterTableAdapters.udf_EarningDeduction1TableAdapter dt = new dsRegisterTableAdapters.udf_EarningDeduction1TableAdapter();

                //ReportDS.udfSalarySlipDataTable objdt1 = new ReportDS.udfSalarySlipDataTable();
                //ReportDS.udfSalarySlipDataTable objdt = new ReportDS.udfSalarySlipDataTable();
                //ReportDSTableAdapters.udfSalarySlipTableAdapter obj = new ReportDSTableAdapters.udfSalarySlipTableAdapter();

                dsRegister.udf_EarningDeduction1DataTable objdt1 = new dsRegister.udf_EarningDeduction1DataTable();
                dsRegister.udf_EarningDeduction1DataTable objdt = new dsRegister.udf_EarningDeduction1DataTable();
                dsRegisterTableAdapters.udf_EarningDeduction1TableAdapter obj = new dsRegisterTableAdapters.udf_EarningDeduction1TableAdapter();
                
                string[] mnthcd = Moncd.Split(',');
                foreach (string monthcd in mnthcd)
                {
                    //Last Day of Month/Year
                    int Days = DateTime.DaysInMonth(Convert.ToInt32(monthcd.Substring(2, 4)), Convert.ToInt32(monthcd.Substring(0, 2)));
                    DateTime date = Convert.ToDateTime(monthcd.Substring(2, 4) + "-" + monthcd.Substring(0, 2) + "-" + Days);

                    dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]), ddlEarningFeilds.SelectedValue, Moncd, date);
                    objdt1.Merge(objdt);
                }

               
                


                //dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]), ddlYear.SelectedValue, ddlMon.SelectedValue);
                DataTable dt3 = dt1;
                DataView dv = new DataView(dt3);
                string filter = "";

                if (ddlHOD.SelectedIndex != 0)
                {
                    filter += " hodid = " + ddlHOD.SelectedValue + " and ";
                }
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
                if (chkState.SelectedIndex != 0)
                {
                    foreach (ListItem st in chkState.Items)
                    {
                        if (st.Selected)
                        {
                            filter += " WStatecd=" + st.Value + " or ";
                        }
                    }
                    //filter += " WStatecd=" + ddlState.SelectedValue + " AND ";

                }

              
                if (filter.Length > 0)
                {
                    filter = filter.Remove(filter.Length - 4, 3);
                    dv.RowFilter = filter;
                }

                ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptEarningDeductionNew.rdlc");


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


                ReportParameter[] p = new ReportParameter[13];
                p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                p[1] = new ReportParameter("MonYrCd", ddlMon.SelectedItem.Text +" "  +  ddlYear.SelectedItem.Text, true);
                if(ddlMonEarDed.SelectedValue=="1")
                {
                    p[2] = new ReportParameter("ReportType", ddlEarningFeilds.SelectedItem.Text + " Earning Report", true);
                    p[3] = new ReportParameter("ReportVal", "1", true);
                    p[4] = new ReportParameter("Period", "From " + ddlMon.SelectedItem.Text + " " + ddlYear.SelectedValue + " To " + ddlToMonth.SelectedItem.Text + " " + ddlToYear.SelectedValue, true);
                }
                else
                {
                    p[2] = new ReportParameter("ReportType", ddlEarningFeilds.SelectedItem.Text +  " Deduction Report", true);
                    p[3] = new ReportParameter("ReportVal", "2", true);
                    p[4] = new ReportParameter("Period", "From " + ddlMon.SelectedItem.Text + " " + ddlYear.SelectedValue + " To " + ddlToMonth.SelectedItem.Text + " " + ddlToYear.SelectedValue, true);
                }

                if (ddlDepartment.SelectedIndex != 0)
                    p[5] = new ReportParameter("department", ddlDepartment.SelectedItem.Text, true);
                else
                    p[5] = new ReportParameter("department", "All", true);
                if (ddlDesignation.SelectedIndex != 0)
                    p[6] = new ReportParameter("designation", ddlDesignation.SelectedItem.Text, true);
                else
                    p[6] = new ReportParameter("designation", "All", true);
                if (ddlCategory.SelectedIndex != 0)
                    p[7] = new ReportParameter("category", ddlCategory.SelectedItem.Text, true);
                else
                    p[7] = new ReportParameter("category", "All", true);
                if (ddlHOD.SelectedIndex != 0)
                    p[8] = new ReportParameter("hod", ddlCategory.SelectedItem.Text, true);
                else
                    p[8] = new ReportParameter("hod", "All", true);
                if (ddlDivision.SelectedIndex != 0)
                    p[9] = new ReportParameter("division", ddlDivision.SelectedItem.Text, true);
                else
                    p[9] = new ReportParameter("division", "All", true);
                if (ddlSkill.SelectedIndex != 0)
                    p[10] = new ReportParameter("skill", ddlSkill.SelectedItem.Text, true);
                else
                    p[10] = new ReportParameter("skill", "All", true);
                if (ddlStatus.SelectedIndex != 0)
                    p[11] = new ReportParameter("status", ddlStatus.SelectedItem.Text, true);
                else
                    p[11] = new ReportParameter("status", "All", true);
                //p[12] = new ReportParameter("AddHead1", AddHead1, true);
                //p[13] = new ReportParameter("AddHead2", AddHead2, true);
                //p[14] = new ReportParameter("AddHead3", AddHead3, true);

                //--- To Display Logo -----------------------------------
                ReportViewer1.LocalReport.EnableExternalImages = true;
                string strqry = "select LogoPath from M_Organization where OrgId=" + Convert.ToInt32(Session["OrgID"]);
                DataTable objDTP = SqlHelper.ExecuteDataTable(strqry, AppGlobal.strConnString);
                string path = "";

                if (objDTP.Rows[0]["LogoPath"] != DBNull.Value)
                    path = new Uri(Server.MapPath(objDTP.Rows[0]["LogoPath"].ToString())).AbsoluteUri;
                else
                    path = new Uri(Server.MapPath("~/Upload/Logo.png")).AbsoluteUri;

                p[12] = new ReportParameter("LogoPath", path, true);
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
                ddlMonEarDed.SelectedIndex = 0;
                ddlMon.SelectedIndex = 0;
                ddlYear.SelectedIndex = 0;
                ddlToMonth.SelectedIndex = 0;
                ddlToYear.SelectedIndex = 0;
                ReportViewer1.LocalReport.DataSources.Clear();

                ddlEarningFeilds.Items.Clear();
                ddlEarningFeilds.DataBind();

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


                ddlEarningFeilds.Items.Add(new ListItem("Basic+DA", "BasicDA"));
                ddlEarningFeilds.Items.Add(new ListItem("HRA", "HRA"));
                ddlEarningFeilds.Items.Add(new ListItem("Conveyance", "Conveyance"));
                ddlEarningFeilds.Items.Add(new ListItem("Education", "Education"));
                ddlEarningFeilds.Items.Add(new ListItem("Medical", "Medical"));
                ddlEarningFeilds.Items.Add(new ListItem("Tea & Tiffin", "Canteen"));
                ddlEarningFeilds.Items.Add(new ListItem("Washing", "Washing"));
                ddlEarningFeilds.Items.Add(new ListItem("Uniform & Shoes", "Uniform"));
                ddlEarningFeilds.Items.Add(new ListItem("Incentive", "Incentive"));
                if(AddHead1!="")
                    ddlEarningFeilds.Items.Add(new ListItem(AddHead1, "Add1"));
                if (AddHead2 != "")
                    ddlEarningFeilds.Items.Add(new ListItem(AddHead2, "Add2"));
                if (AddHead3 != "")
                    ddlEarningFeilds.Items.Add(new ListItem(AddHead3, "Add3"));

                ddlEarningFeilds.Items.Add(new ListItem("Gross", "Gross"));

                ddlMon.Focus();
            }
            catch(Exception ex)
            {

            }

        }

        protected void ddlMonEarDed_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlEarningFeilds.DataSource = null;
            ddlEarningFeilds.DataBind();
            ddlEarningFeilds.Items.Clear();

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

            if (ddlMonEarDed.SelectedValue == "1")
            {
                ddlEarningFeilds.Items.Add(new ListItem("BasicDA", "BasicDA"));
                ddlEarningFeilds.Items.Add(new ListItem("HRA", "HRA"));
                ddlEarningFeilds.Items.Add(new ListItem("Conveyance", "Conveyance"));
                ddlEarningFeilds.Items.Add(new ListItem("Education", "Education"));
                ddlEarningFeilds.Items.Add(new ListItem("Medical", "Medical"));
                ddlEarningFeilds.Items.Add(new ListItem("Tea & Tiffin", "Canteen"));
                ddlEarningFeilds.Items.Add(new ListItem("Washing", "Washing"));
                ddlEarningFeilds.Items.Add(new ListItem("Uniform & Shoes", "Uniform"));
                ddlEarningFeilds.Items.Add(new ListItem("Incentive", "Incentive"));
                if (AddHead1 != "")
                    ddlEarningFeilds.Items.Add(new ListItem(AddHead1, "Add1"));
                if (AddHead2 != "")
                    ddlEarningFeilds.Items.Add(new ListItem(AddHead2, "Add2"));
                if (AddHead3 != "")
                    ddlEarningFeilds.Items.Add(new ListItem(AddHead3, "Add3"));

                ddlEarningFeilds.Items.Add(new ListItem("Gross", "Gross"));
            }
            else
            {
                ddlEarningFeilds.Items.Add(new ListItem("Advance", "Advance"));
                ddlEarningFeilds.Items.Add(new ListItem("Loan", "Loan"));
                ddlEarningFeilds.Items.Add(new ListItem("TDS", "TDS"));
                ddlEarningFeilds.Items.Add(new ListItem("TardalPathsasnth", "TardalPathsasnth"));
                ddlEarningFeilds.Items.Add(new ListItem("ESIEmpContribution", "ESIEmpContribution"));
                ddlEarningFeilds.Items.Add(new ListItem("ESICompContribution", "ESICompContribution"));
                ddlEarningFeilds.Items.Add(new ListItem("ProfTax", "ProfTax"));
                ddlEarningFeilds.Items.Add(new ListItem("Provfund", "Provfund"));
                ddlEarningFeilds.Items.Add(new ListItem("LWF", "LWF"));
                ddlEarningFeilds.Items.Add(new ListItem("EntertainmentCost", "Ded1"));
            }
        }

        protected void ddlEarningFeilds_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlEarningFeilds.SelectedValue== "ProfTax")
            {
                chkState.Visible = true;
            }
            else
            {
                chkState.Visible = false;
            }
        }
        private string GetMnthCd()
        {
            string strMnthCd = "";
            int sMnth = ddlMon.SelectedIndex;
            int sYr = Convert.ToInt32(ddlYear.SelectedValue);
            int eMnth = ddlToMonth.SelectedIndex;
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

        protected void ddlMon_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlToMonth.SelectedValue = ddlMon.SelectedValue;
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlToYear.SelectedValue = ddlYear.SelectedValue;
        }
    }
}