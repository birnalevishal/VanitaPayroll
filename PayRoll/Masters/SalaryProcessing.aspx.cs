using SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlTypes;

namespace PayRoll.Masters
{
    public partial class SalaryProcessing : System.Web.UI.Page
    {
        SqlConnection sqlConn = null;
        SqlCommand sqlCmd = null;
        SqlTransaction sqlTrans = null;

        string empName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                clearControls();
                checkHeading();
            }
        }

        private void BindData()
        {
            string strQry = "SELECT Year  FROM M_Year Where IsActive='Y' ORDER BY Year DESC";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlYear.DataSource = objDT;
            ddlYear.DataTextField = "Year";
            ddlYear.DataValueField = "Year";
            ddlYear.DataBind();

            ddlYear.Items.Insert(0, new ListItem("Select", "00"));

            strQry = "SELECT Employeename,Employeecd FROM M_Emp where OrgID=" + Convert.ToInt16(Session["OrgID"]) + " ORDER BY Employeename";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlEmpName.DataSource = objDT;
            ddlEmpName.DataTextField = "Employeename";
            ddlEmpName.DataValueField = "Employeecd";
            ddlEmpName.DataBind();
            ddlEmpName.Items.Insert(0, "Select");

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    if (ddlMon.SelectedIndex != 0 || ddlMon.SelectedIndex != 0)
                    {
                        salaryProcess(ddlMon.SelectedValue, ddlYear.SelectedValue, "", "2");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Month and Year '); ", true);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                clearControls();
            }
            catch (Exception ex)
            {

            }
        }

        private void clearControls()
        {
            BindData();
            ddlMon.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;
            txtEmpCode.Text = "";
            ddlEmpName.SelectedIndex = 0;
            txtNetAmount.BorderColor = System.Drawing.Color.White;
            HFEmployee.Value = "0";
            foreach (var item in pnlSalaryData.Controls)
            {
                if (item is TextBox)
                {
                    ((TextBox)item).Text = "";
                }
            }
            gvList.DataSource = null;
            gvList.DataBind();
            pnlGVList.Visible = false;
            pnlSalaryData.Visible = false;
            btnSave.Visible = true;
        }

        protected void checkHeading()
        {
            string strQry = "";

            strQry = "SELECT Add1Heading FROM M_AddHeading Where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and Add1Heading is not null";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (objDT.Rows.Count > 0)
            {
                lblAdd1.Text = objDT.Rows[0]["Add1Heading"].ToString();
                lblAdd1.Visible = true;
                txtAdd1.ReadOnly = false;
            }
            else
            {
                //lblAdd1.Visible = false;
                lblAdd1.Text = ".";
                txtAdd1.ReadOnly = true;
            }

            strQry = "SELECT Add2Heading FROM M_AddHeading Where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and Add2Heading is not null";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (objDT.Rows.Count > 0)
            {
                lblAdd2.Text = objDT.Rows[0]["Add2Heading"].ToString();
                lblAdd2.Visible = true;
                txtAdd2.ReadOnly = false;
            }
            else
            {
                //lblAdd2.Visible = false;
                lblAdd2.Text = ".";
                txtAdd2.ReadOnly = true;
            }

            strQry = "SELECT Add3Heading FROM M_AddHeading Where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and Add3Heading is not null";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (objDT.Rows.Count > 0)
            {
                lblAdd3.Text = objDT.Rows[0]["Add3Heading"].ToString();
                lblAdd3.Visible = true;
                txtAdd3.ReadOnly = false;
            }
            else
            {
                //lblAdd3.Visible = false;
                lblAdd3.Text = ".";
                txtAdd3.ReadOnly = true;

            }
        }
        protected void txtEmpCode_TextChanged(object sender, EventArgs e)
        {
            if (txtEmpCode.Text != "")
            {
                string strQry = "SELECT Employeename  FROM M_Emp Where Employeecd='" + txtEmpCode.Text + "' and OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and IsActive='Y'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    ddlEmpName.SelectedValue = txtEmpCode.Text;
                }
                else
                {
                    ddlEmpName.SelectedIndex = 0;
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Wrong Employee Code'); ", true);
                    return;
                }
            }
        }

        protected void salaryProcess(string month, string year, string empCode, string opr)
        {
            #region[Variables]
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            SqlTransaction sqlTrans = null;
            string strQry = "";
            string strQry1 = "";
            string strQry2 = "";
            string strPfConfig = "";
            string strProfTaxConfig = "";
            string strLabWelFunConfig = "";
            string strESIConfig = "";
            string strESIConfig1 = "";
            string strQrySalary = "";
            DataTable objDT;
            DataTable objDT1;
            DataTable objDT2;
            DataTable objPFConfig;
            DataTable objProfTaxConfig;
            DataTable objESIConfig;
            DataTable objESIConfig1;
            DataTable objLabWelFunConfig;

            string prvMonth = "";
            double esiConfigAmt = 0, esiEmpPer = 0, esiCompPer = 0;
            double presentDays = 0, PL = 0, COff = 0, weeklyOff = 0, payDays = 0, absentDays = 0, payHolidays = 0;
            int workingDays = 0,actualWorkingDays=0, empAge = 0, lastDay = 0;
            //string presentDays = "0", PL = "0", COff = "0", weeklyOff = "0";
            double standardBasic = 0, standardHRA = 0, standardMedical = 0, standardWashing = 0, standConveyance = 0, stadEducation = 0, stadCanteen = 0, stadUniform = 0, stadInsentiv = 0, stadGross = 0, add1 = 0, add2 = 0, add3 = 0;
            double actualBasic = 0, actualHRA = 0, actualMedical = 0, actualWashing = 0, insentive = 0, grossSalary = 0, deduction = 0, deductionImp=0, netSalary = 0, lwf = 0, lwfCompContri = 0;
            double pfAmount = 0, pfPension = 0, epf = 0, eps = 0, monAmt = 0, profTaxAmt = 0, esiEmpAmount = 0, esiCompAmount = 0, basicfrEsi = 0, grossSal = 0; ;
            int daysInMonth = 0;
            double basicForEsi = 0, grossForEsi = 0;
            double oneDaySal = 0, presentDaySal = 0;

            DataRow dr;
            DataRow drl;
            DataTable dtEmpSalary = new DataTable();
            DataTable dtEmpLessSalary = new DataTable();
            bool result = false;
            string salDate;
            #endregion

            #region[DataTable]
            //Less Salary Table
            dtEmpLessSalary.Columns.Add("empCode", typeof(string));
            dtEmpLessSalary.Columns.Add("empName", typeof(string));
            dtEmpLessSalary.Columns.Add("gross", typeof(double));
            dtEmpLessSalary.Columns.Add("deduction", typeof(double));
            dtEmpLessSalary.Columns.Add("net", typeof(double));

            //T_Attendence
            dtEmpSalary.Columns.Add("empCode", typeof(string));
            dtEmpSalary.Columns.Add("empName", typeof(string));
            dtEmpSalary.Columns.Add("daysInMonth", typeof(double));
            dtEmpSalary.Columns.Add("payDays", typeof(double));
            dtEmpSalary.Columns.Add("presentDays", typeof(double));
            dtEmpSalary.Columns.Add("absentDays", typeof(double));
            dtEmpSalary.Columns.Add("PL", typeof(double));
            dtEmpSalary.Columns.Add("weeklyOff", typeof(double));
            dtEmpSalary.Columns.Add("paybleDays", typeof(double));
            dtEmpSalary.Columns.Add("payholiday", typeof(double));
            dtEmpSalary.Columns.Add("cOff", typeof(double));
            dtEmpSalary.Columns.Add("EL", typeof(double));
            dtEmpSalary.Columns.Add("CL", typeof(double));
            dtEmpSalary.Columns.Add("SL", typeof(double));
            dtEmpSalary.Columns.Add("SPCL", typeof(double));
            dtEmpSalary.Columns.Add("LWP", typeof(double));
            dtEmpSalary.Columns.Add("tour", typeof(double));

            //M_Salary
            //Earnings
            dtEmpSalary.Columns.Add("basicDA", typeof(double));
            dtEmpSalary.Columns.Add("HRA", typeof(double));
            dtEmpSalary.Columns.Add("CA", typeof(double));
            dtEmpSalary.Columns.Add("education", typeof(double));
            dtEmpSalary.Columns.Add("medical", typeof(double));
            dtEmpSalary.Columns.Add("canteen", typeof(double));
            dtEmpSalary.Columns.Add("washing", typeof(double));
            dtEmpSalary.Columns.Add("uniform", typeof(double));
            dtEmpSalary.Columns.Add("insentive", typeof(double));
            dtEmpSalary.Columns.Add("rent", typeof(double));
            dtEmpSalary.Columns.Add("plant", typeof(double));
            dtEmpSalary.Columns.Add("target", typeof(double));
            dtEmpSalary.Columns.Add("fabri", typeof(double));
            dtEmpSalary.Columns.Add("presenty", typeof(double));
            dtEmpSalary.Columns.Add("productivity", typeof(double));
            dtEmpSalary.Columns.Add("maintenance", typeof(double));
            dtEmpSalary.Columns.Add("special", typeof(double));
            dtEmpSalary.Columns.Add("LTA", typeof(double));
            dtEmpSalary.Columns.Add("add1", typeof(double));
            dtEmpSalary.Columns.Add("add2", typeof(double));
            dtEmpSalary.Columns.Add("add3", typeof(double));
            dtEmpSalary.Columns.Add("add4", typeof(double));
            dtEmpSalary.Columns.Add("add5", typeof(double));

            //Deduction
            dtEmpSalary.Columns.Add("salaryAdvance", typeof(double));
            dtEmpSalary.Columns.Add("providendFund", typeof(double));
            dtEmpSalary.Columns.Add("pfPension", typeof(double));
            dtEmpSalary.Columns.Add("EPF", typeof(double));
            dtEmpSalary.Columns.Add("EPS", typeof(double));
            dtEmpSalary.Columns.Add("TDS", typeof(double));
            dtEmpSalary.Columns.Add("professionalTax", typeof(double));
            dtEmpSalary.Columns.Add("lwf", typeof(double));
            dtEmpSalary.Columns.Add("lwfCompContri", typeof(double));
            dtEmpSalary.Columns.Add("ESIEmpContribution", typeof(double));
            dtEmpSalary.Columns.Add("ESICompContribution", typeof(double));
            dtEmpSalary.Columns.Add("esiEmpPer", typeof(double));
            dtEmpSalary.Columns.Add("esiCompPer", typeof(double));
            dtEmpSalary.Columns.Add("loan", typeof(double));
            dtEmpSalary.Columns.Add("TardalPathsansth", typeof(double));
            dtEmpSalary.Columns.Add("iTax", typeof(double));
            dtEmpSalary.Columns.Add("LIC", typeof(double));
            dtEmpSalary.Columns.Add("SRD", typeof(double));
            dtEmpSalary.Columns.Add("stamp", typeof(double));
            dtEmpSalary.Columns.Add("bank", typeof(double));
            dtEmpSalary.Columns.Add("postRec", typeof(double));
            dtEmpSalary.Columns.Add("society", typeof(double));
            dtEmpSalary.Columns.Add("ded1", typeof(double));
            dtEmpSalary.Columns.Add("ded2", typeof(double));
            dtEmpSalary.Columns.Add("ded3", typeof(double));

            dtEmpSalary.Columns.Add("gross", typeof(double));
            dtEmpSalary.Columns.Add("deduction", typeof(double));
            dtEmpSalary.Columns.Add("net", typeof(double));
            dtEmpSalary.Columns.Add("lessNetSal", typeof(string));
            #endregion

            try
            {
                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);
                if (opr == "2")
                {
                    strQry = "select * from T_SalaryLock where OrgId=" + Convert.ToInt16(Session["orgID"]) + " and RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)>='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "' and Lock='Y'";
                    objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                    if (objDT.Rows.Count > 0)
                    {
                        clearControls();
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Salary Already Processed And Locked.'); ", true);
                        return;
                    }
                }

                //last day of month
                lastDay = 0;
                lastDay = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMon.SelectedValue));
                string dt = lastDay + "/" + ddlMon.SelectedValue + "/" + ddlYear.SelectedValue;
                string firstDate = "01/" + ddlMon.SelectedValue + "/" + ddlYear.SelectedValue;
                strQry = "";


                workingDays = 26;
                actualWorkingDays = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));

                strQry = " select * from udfMonthlysalary(" + Convert.ToInt16(Session["OrgID"]) +",'" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "','" + ddlMon.SelectedValue + ddlYear.SelectedValue + "','" + ddlYear.SelectedValue+ddlMon.SelectedValue + "'," + workingDays + "," + actualWorkingDays + ")";
                if (opr =="1")
                {
                    strQry += " where Employeecd='" + empCode + "' ";
                    //strQry += " and Employeecd='" + empCode + "' ";
                }
                strQry += " union ";
                strQry += " SELECT dbo.M_Emp.OrgId,dbo.T_MonthlyHamaliSalary.MonYrcd, dbo.M_Emp.Employeecd, 0 as Weeklyoff, 0 as Payholiday,0 as COff, 0 as Absent, 0 as PL,0 as PresentDay,0 as SNACKCOUNT,0 as OT, ";
                strQry += " dbo.M_Emp.Employeename, dbo.M_Emp.Birthdate, dbo.M_Emp.Gendercd, dbo.M_Emp.WStatecd, dbo.M_Emp.PFApplicable,dbo.M_Emp.PFJoindate, dbo.M_Emp.ProfTaxApplicable, dbo.M_Emp.LabWelApplicable, dbo.M_Emp.ESIApplicable, dbo.M_Emp.ESINotApplicableDt, dbo.M_Emp.ESICalculate, dbo.M_Emp.OrigJoindate,";
                strQry += " isnull(udf_EmpSalMaster_1.BasicDA, 0)+isnull(udf_EmpSalMaster_1.HRA, 0)+isnull(udf_EmpSalMaster_1.Education, 0)+isnull(udf_EmpSalMaster_1.Medical, 0) AS stdBasic, isnull(udf_EmpSalMaster_1.Gross, 0) as grossSal ,ISNULL(Gross, 0) AS stadGross, udf_EmpSalMaster_1.Gross as Docdate, '' AS Expr1, 0 as PfMan, 0 as PfEmpPct, '' as Approval, ";
                strQry += "  isnull(dbo.T_MonthlyDeduction.Advance,0),  isnull(dbo.T_MonthlyDeduction.Loan,0),  isnull(dbo.T_MonthlyDeduction.TDS,0),  isnull(dbo.T_MonthlyDeduction.TardalPathsansth,0),  isnull(dbo.T_MonthlyDeduction.Ded1,0), 0 as PF,0 as EPF, 0 as EPS, 0 as PFConditional,0 as PFAmtLimit, 0 as PFAgeLimit, ";
                strQry += " 0 AS canteenRate,";
                strQry += " isnull(udf_EmpSalMaster_1.BasicDA, 0) as BasicDA, isnull(udf_EmpSalMaster_1.HRA, 0) as HRA, isnull(udf_EmpSalMaster_1.Conveyance, 0) as Conveyance,isnull(udf_EmpSalMaster_1.Education, 0) as Education,isnull(udf_EmpSalMaster_1.Medical, 0) as Medical,";
                strQry += "	isnull(udf_EmpSalMaster_1.Canteen, 0) as Canteen, isnull(udf_EmpSalMaster_1.Washing, 0) as Washing, isnull(udf_EmpSalMaster_1.Uniform, 0) as Uniform, isnull(udf_EmpSalMaster_1.Add1, 0) Add1, isnull(udf_EmpSalMaster_1.Add2, 0) Add2, isnull(udf_EmpSalMaster_1.Add3, 0) Add3,";
                strQry += " 0 as Lwf, 0 as LWFCompContri, ISNULL(udf_EmpConfigMax_1.conId, 0)  AS empDesignation, isnull(dbo.T_MonthlyHamaliSalary.Wages, 0) +isnull(dbo.T_MonthlyHamaliSalary.VehicleHamali, 0) AS TotalHamali, null as witheffect ";
                strQry += " FROM dbo.M_Emp ";
                strQry += " INNER JOIN dbo.udf_EmpConfigMax('" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "', 'desg') AS udf_EmpConfigMax_1 ON dbo.M_Emp.OrgId = udf_EmpConfigMax_1.OrgId AND dbo.M_Emp.Employeecd = udf_EmpConfigMax_1.Employeecd";
                strQry += " INNER JOIN dbo.T_MonthlyHamaliSalary ON dbo.M_Emp.OrgId = dbo.T_MonthlyHamaliSalary.OrgId AND dbo.M_Emp.Employeecd = dbo.T_MonthlyHamaliSalary.Employeecd";
                //strQry += " INNER JOIN dbo.udf_EmpSalMaster(" + Convert.ToInt16(Session["OrgID"]) + ", '" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "') AS udf_EmpSalMaster_1 ON dbo.M_Emp.OrgId = udf_EmpSalMaster_1.OrgId AND dbo.M_Emp.Employeecd = udf_EmpSalMaster_1.Employeecd";
                strQry += " INNER JOIN dbo.udfEmployeesalarymax(" + Convert.ToInt16(Session["OrgID"]) + ", '" + ddlYear.SelectedValue + ddlMon.SelectedValue + "') AS udf_EmpSalMaster_1 ON dbo.M_Emp.OrgId = udf_EmpSalMaster_1.OrgId AND dbo.M_Emp.Employeecd = udf_EmpSalMaster_1.Employeecd";
                strQry += " left outer JOIN dbo.T_MonthlyDeduction ON dbo.T_MonthlyHamaliSalary.OrgId = dbo.T_MonthlyDeduction.OrgId AND dbo.T_MonthlyHamaliSalary.MonYrcd = dbo.T_MonthlyDeduction.MonYrcd AND dbo.T_MonthlyHamaliSalary.Employeecd = dbo.T_MonthlyDeduction.Employeecd ";
                strQry += " where dbo.M_Emp.OrgID = " + Convert.ToInt16(Session["OrgID"]) + " and dbo.T_MonthlyHamaliSalary.MonYrCd = '" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                if (opr == "1")
                {
                    strQry += " and dbo.M_Emp.Employeecd='" + empCode + "'";
                }
                objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    DataSet1.EmpSalaryDataTable empSalary = new DataSet1.EmpSalaryDataTable();

                    for (int i = 0; i < objDT.Rows.Count; i++)
                    {
                        presentDays = PL = COff = weeklyOff = payDays = absentDays = 0;
                        esiEmpPer = 0; esiCompPer = 0; esiEmpAmount = 0; esiCompAmount = 0;
                        workingDays = 26; empAge = 0; lastDay = 0;
                        salDate = "";
                        standardBasic = standardHRA = standardMedical = standardWashing = standConveyance = stadEducation = stadCanteen = stadUniform = stadInsentiv = stadGross = add1 = add2 = add3 = 0;
                        actualBasic = actualHRA = actualMedical = actualWashing = grossSalary = deduction= deductionImp = netSalary = 0;
                        pfAmount = pfPension= epf=eps= monAmt = profTaxAmt = lwfCompContri = lwf = 0; basicfrEsi = 0;
                        daysInMonth = 0;
                        grossSal = 0;
                        basicForEsi = 0;

                        // Working Days 
                        daysInMonth = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));
                        if (objDT.Rows[i]["Weeklyoff"] != DBNull.Value)
                           weeklyOff = Convert.ToDouble(objDT.Rows[i]["Weeklyoff"]);
                        //workingDays = daysInMonth - Convert.ToInt16(weeklyOff);


                        //for Hamali salary processing............Reverse Calculation  Employee Designation =21 Loading Unloading
                        if (objDT.Rows[i]["empDesignation"].ToString() == "21")
                        {
                            #region[HamaliSalary]
                            string strQryHamaliSal = "SELECT dbo.M_Emp.OrgId, dbo.M_Emp.Employeecd, dbo.M_Emp.Employeename, dbo.M_Emp.Birthdate, dbo.M_Emp.PFApplicable, dbo.M_Emp.ProfTaxApplicable,dbo.M_Emp.gendercd, dbo.M_Emp.WStatecd,  dbo.M_Emp.LabWelApplicable, dbo.M_Emp.ESIApplicable, dbo.M_Emp.ESINotApplicableDt, dbo.M_Emp.ESICalculate, ";
                            strQryHamaliSal += " dbo.T_MonthlyHamaliSalary.Wages, dbo.T_MonthlyHamaliSalary.VehicleHamali, isnull(udf_EmpSalMaster_1.BasicDA,0) as BasicDA, ";
                            strQryHamaliSal += " isnull(udf_EmpSalMaster_1.HRA,0) as HRA, isnull(udf_EmpSalMaster_1.Conveyance,0) as Conveyance, isnull(udf_EmpSalMaster_1.Education,0) as Education, isnull(udf_EmpSalMaster_1.Medical,0) as Medical, isnull(udf_EmpSalMaster_1.Canteen,0) as Canteen, isnull(udf_EmpSalMaster_1.Uniform,0) as Uniform,";
                            strQryHamaliSal += " isnull(udf_EmpSalMaster_1.Add1,0) Add1, isnull(udf_EmpSalMaster_1.Add2,0) as Add2, isnull(udf_EmpSalMaster_1.Washing,0) as Washing, isnull(udf_EmpSalMaster_1.Add3,0) as Add3, isnull(dbo.T_MonthlyHamaliSalary.Wages,0) + isnull(dbo.T_MonthlyHamaliSalary.VehicleHamali,0) AS TotalHamali, ";
                            strQryHamaliSal += " isnull(udf_EmpSalMaster_1.Gross,0) as Gross ";
                            strQryHamaliSal += " FROM dbo.M_Emp INNER JOIN dbo.T_MonthlyHamaliSalary ON dbo.M_Emp.OrgId = dbo.T_MonthlyHamaliSalary.OrgId AND dbo.M_Emp.Employeecd = dbo.T_MonthlyHamaliSalary.Employeecd ";
                            //strQryHamaliSal += " INNER JOIN dbo.udf_EmpSalMaster(" + Convert.ToInt16(Session["OrgID"]) + ",'" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "') AS udf_EmpSalMaster_1 ON dbo.M_Emp.OrgId = udf_EmpSalMaster_1.OrgId AND dbo.M_Emp.Employeecd = udf_EmpSalMaster_1.Employeecd ";
                            strQryHamaliSal += " INNER JOIN dbo.udfEmployeesalarymax(" + Convert.ToInt16(Session["OrgID"]) + ",'" + ddlYear.SelectedValue + ddlMon.SelectedValue + "') AS udf_EmpSalMaster_1 ON dbo.M_Emp.OrgId = udf_EmpSalMaster_1.OrgId AND dbo.M_Emp.Employeecd = udf_EmpSalMaster_1.Employeecd ";
                            strQryHamaliSal += " where dbo.M_Emp.OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and dbo.T_MonthlyHamaliSalary.MonYrCd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and dbo.M_Emp.Employeecd='" + objDT.Rows[i]["Employeecd"].ToString() + "'";
                            DataTable objDTHamaliSal = SqlHelper.ExecuteDataTable(strQryHamaliSal, AppGlobal.strConnString);
                            if (objDTHamaliSal.Rows.Count > 0)
                            {

                                for (int j = 0; j < objDTHamaliSal.Rows.Count; j++)
                                {
                                    presentDays = PL = COff = weeklyOff = payDays = absentDays = 0;
                                    esiEmpPer = 0; esiCompPer = 0; esiEmpAmount = 0; esiCompAmount = 0;
                                    empAge = 0; lastDay = 0;
                                    salDate = "";
                                    standardBasic = standardHRA = standardMedical = standardWashing = standConveyance = stadEducation = stadCanteen = stadUniform = stadInsentiv = stadGross = add1 = add2 = add3 = 0;
                                    actualBasic = actualHRA = actualMedical = actualWashing = grossSalary = deduction = netSalary = 0;
                                    pfAmount = pfPension = epf = eps = monAmt = profTaxAmt = lwfCompContri = lwf = 0; basicfrEsi = 0;
                                    daysInMonth = 0;
                                    grossSal = 0;
                                    insentive = 0;
                                    basicForEsi = 0;

                                    dr = dtEmpSalary.NewRow();

                                    //common
                                    dr["empCode"] = objDTHamaliSal.Rows[j]["Employeecd"].ToString();
                                    dr["empName"] = objDTHamaliSal.Rows[j]["Employeename"].ToString();
                                    daysInMonth = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));

                                    dr["daysInMonth"] = daysInMonth;

                                    basicfrEsi = Convert.ToDouble(objDT.Rows[i]["stdBasic"]);
                                    grossSal = Convert.ToDouble(objDT.Rows[i]["grossSal"]);

                                    //Total Hamali Greater is equal to 0
                                    if(Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["TotalHamali"]))==0)
                                    {
                                        //Attendance
                                        dr["daysInMonth"] = daysInMonth;
                                        if (objDT.Rows[i]["Weeklyoff"] != DBNull.Value)
                                            dr["weeklyOff"] = weeklyOff = Convert.ToDouble(objDT.Rows[i]["Weeklyoff"]);
                                        if (objDT.Rows[i]["Payholiday"] != DBNull.Value)
                                            dr["payholiday"] = payHolidays = Convert.ToDouble(objDT.Rows[i]["Payholiday"]);
                                        dr["payDays"] = 0;
                                        if (objDT.Rows[i]["PresentDay"] != DBNull.Value)
                                            dr["presentDays"] = presentDays = 0;
                                        if (objDT.Rows[i]["PL"] != DBNull.Value)
                                            dr["PL"] = PL = 0;
                                        if (objDT.Rows[i]["COff"] != DBNull.Value)
                                            dr["cOff"] = COff = 0;
                                        if (objDT.Rows[i]["Absent"] != DBNull.Value)
                                            dr["AbsentDays"] = absentDays = Convert.ToDouble(objDT.Rows[i]["Absent"]);
                                        dr["PaybleDays"] = 0;

                                        //Salary
                                        // Earnings
                                        #region[Earnings]

                                        dr["BasicDA"] = 0;
                                        dr["HRA"] = 0;
                                        dr["CA"] = 0;
                                        dr["medical"] = standardMedical;
                                        dr["Canteen"] = stadCanteen;
                                        dr["Washing"] = standardWashing;
                                        dr["Uniform"] = stadUniform;
                                        dr["Add1"] = add1;
                                        dr["Add2"] = add2;
                                        dr["Add3"] = add3;
                                        dr["insentive"] = 0;
                                        dr["gross"] = 0;

                                        #endregion

                                        //From T_Deduction
                                        #region[Deduction]
                                        #region[T_Deduction]
                                        dr["salaryAdvance"] = 0;
                                        dr["Loan"] = 0;
                                        dr["TDS"] = 0;
                                        dr["TardalPathsansth"] = 0;
                                        dr["Ded1"] = 0;
                                        

                                        #endregion


                                        dr["providendFund"] = 0;
                                        dr["pfPension"] = 0;
                                        dr["EPF"] = 0;
                                        dr["EPS"] = 0;

                                        dr["professionalTax"] = 0;

                                        dr["lwf"] = 0;
                                        dr["lwfCompContri"] = 0;

                                        dr["esiEmpPer"] = 0;
                                        dr["esiCompPer"] = 0;

                                        dr["ESIEmpContribution"] = 0;
                                        dr["ESICompContribution"] = 0;

                                        dr["deduction"] = 0;
                                        #endregion

                                        dr["net"] = 0;

                                    }

                                    // if TotalHamali (payment) is greater than gross salary
                                    else if (Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["TotalHamali"])) > Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Gross"])))
                                    {
                                        //If (totalhamali - gross >= 60) then Extra amount added in incentive ------Modified on 06/01/2021
                                        if (Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["TotalHamali"])) - Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Gross"])) >= 70)//60
                                        {
                                            dr["payDays"] = workingDays;
                                            dr["PaybleDays"] = workingDays;
                                            dr["presentDays"] = workingDays;
                                            #region[Earning]

                                            grossSalary = standardBasic = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["BasicDA"]));
                                            dr["BasicDA"] = grossSalary = standardBasic;
                                            basicForEsi = basicForEsi + Math.Round(Convert.ToDouble(objDT.Rows[i]["BasicDA"]));

                                            standardHRA = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["HRA"]));
                                            grossSalary = grossSalary + standardHRA;
                                            dr["HRA"] = standardHRA;
                                            basicForEsi = basicForEsi + Math.Round(Convert.ToDouble(objDT.Rows[i]["HRA"]));

                                            standConveyance = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Conveyance"]));
                                            grossSalary = grossSalary + standConveyance;
                                            dr["CA"] = standConveyance;

                                            stadEducation = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Education"]));
                                            grossSalary = grossSalary + stadEducation;
                                            dr["education"] = stadEducation;
                                            basicForEsi = basicForEsi + Math.Round(Convert.ToDouble(objDT.Rows[i]["Education"]));

                                            standardMedical = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["medical"]));
                                            grossSalary = grossSalary + standardMedical;
                                            dr["medical"] = standardMedical;
                                            basicForEsi = basicForEsi + Math.Round(Convert.ToDouble(objDT.Rows[i]["medical"]));

                                            stadCanteen = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Canteen"]));
                                            grossSalary = grossSalary + stadCanteen;
                                            dr["Canteen"] = stadCanteen;
                                            basicForEsi = basicForEsi + Math.Round(Convert.ToDouble(objDT.Rows[i]["Canteen"])); // Newlly added for esi calculation on 1/10/2021

                                            standardWashing = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Washing"]));
                                            grossSalary = grossSalary + standardWashing;
                                            dr["Washing"] = standardWashing;

                                            stadUniform = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Uniform"]));
                                            grossSalary = grossSalary + stadUniform;
                                            dr["Uniform"] = stadUniform;
                                            basicForEsi = basicForEsi + Math.Round(Convert.ToDouble(objDT.Rows[i]["Uniform"])); // Newlly added for esi calculation on 1/10/2021

                                            add1 = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Add1"]));
                                            grossSalary = grossSalary + add1;
                                            dr["Add1"] = add1;
                                            basicForEsi = basicForEsi + Math.Round(Convert.ToDouble(objDT.Rows[i]["Add1"])); // Newlly added for esi calculation on 10/03/2021

                                            add2 = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Add2"]));
                                            grossSalary = grossSalary + add2;
                                            dr["Add2"] = add2;

                                            add3 = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Add3"]));
                                            grossSalary = grossSalary + add3;
                                            dr["Add3"] = add3;

                                            //insentive = Convert.ToDouble(objDTHamaliSal.Rows[j]["TotalHamali"]) - Convert.ToDouble(objDTHamaliSal.Rows[j]["Gross"]);

                                            //------ 
                                            //if (Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["TotalHamali"])) - grossSalary >= 60)
                                            //{
                                            //    insentive = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["TotalHamali"])) - Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Gross"]));
                                            //}
                                            ////-----------------------------------------------------------------------------

                                            insentive = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["TotalHamali"])) - Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Gross"]));

                                            grossSalary = grossSalary + Math.Round(Convert.ToDouble(insentive));

                                            dr["insentive"] = Math.Round(Convert.ToDouble(insentive));//insentive;

                                            dr["gross"] = Math.Round(Convert.ToDouble(grossSalary));

                                            //For ESI
                                            basicForEsi = basicForEsi + Math.Round(Convert.ToDouble(insentive)); ;

                                            #endregion

                                            #region[Deduction]
                                            #region[T_Deduction]
                                            if (objDT.Rows[i]["Advance"] != DBNull.Value)
                                            {
                                                dr["salaryAdvance"] = Math.Round(Convert.ToDouble(objDT.Rows[i]["Advance"]));
                                                deduction = deduction + Math.Round(Convert.ToDouble(objDT.Rows[i]["Advance"]));
                                            }
                                            if (objDT.Rows[i]["Loan"] != DBNull.Value)
                                            {
                                                dr["Loan"] = Math.Round(Convert.ToDouble(objDT.Rows[i]["Loan"]));
                                                deduction = deduction + Math.Round(Convert.ToDouble(objDT.Rows[i]["Loan"]));
                                            }
                                            if (objDT.Rows[i]["TDS"] != DBNull.Value)
                                            {
                                                dr["TDS"] = Math.Round(Convert.ToDouble(objDT.Rows[i]["TDS"]));
                                                deduction = deduction + Math.Round(Convert.ToDouble(objDT.Rows[i]["TDS"]));
                                            }
                                            if (objDT.Rows[i]["TardalPathsansth"] != DBNull.Value)
                                            {
                                                dr["TardalPathsansth"] = Math.Round(Convert.ToDouble(objDT.Rows[i]["TardalPathsansth"]));
                                                deduction = deduction + Math.Round(Convert.ToDouble(objDT.Rows[i]["TardalPathsansth"]));
                                            }
                                            if (objDT.Rows[i]["Ded1"] != DBNull.Value)
                                            {
                                                dr["Ded1"] = Math.Round(Convert.ToDouble(objDT.Rows[i]["Ded1"]));
                                                deduction = deduction + Math.Round(Convert.ToDouble(objDT.Rows[i]["Ded1"]));
                                            }
                                            #endregion

                                            #region[PF Config]
                                            //PF Calculation  
                                            if (objDTHamaliSal.Rows[j]["PFApplicable"] != DBNull.Value)
                                            {
                                                if (objDTHamaliSal.Rows[j]["PFApplicable"].ToString() != "N" && Convert.ToDateTime(dt) >= Convert.ToDateTime(objDT.Rows[i]["PFJoindate"]))
                                                {
                                                    strPfConfig = "select * from M_PFConfiguration WHERE RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) = (SELECT  max(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) as MonYrcd  FROM dbo.M_PFConfiguration WHERE RIGHT(MonYrcd, 4) +LEFT(MonYrcd, 2) <= '" + ddlYear.SelectedValue + ddlMon.SelectedValue + "' and orgid = " + Convert.ToInt32(Session["OrgID"]) + " ) AND(OrgId = " + Convert.ToInt32(Session["OrgID"]) + ")";
                                                    objPFConfig = SqlHelper.ExecuteDataTable(strPfConfig, AppGlobal.strConnString);
                                                    if (objPFConfig.Rows.Count > 0)
                                                    {
                                                        empAge = getAge(Convert.ToDateTime(objDTHamaliSal.Rows[j]["Birthdate"]));
                                                        if (empAge <= Convert.ToInt32(objPFConfig.Rows[0]["PFAgeLimit"]))
                                                        {
                                                            epf = Convert.ToDouble(objPFConfig.Rows[0]["EPF"]);
                                                            eps = Convert.ToDouble(objPFConfig.Rows[0]["EPS"]);
                                                            if ((standardBasic > Convert.ToInt32(objPFConfig.Rows[0]["PFAmtLimit"])) && objDTHamaliSal.Rows[j]["PFApplicable"].ToString() != "A")
                                                            {
                                                                pfAmount = Convert.ToInt32(objPFConfig.Rows[0]["PFAmtLimit"]) * Convert.ToDouble(objPFConfig.Rows[0]["EPF"]) / 100;
                                                                pfPension = Convert.ToInt32(objPFConfig.Rows[0]["PFAmtLimit"]) * Convert.ToDouble(objPFConfig.Rows[0]["EPS"]) / 100;
                                                            }
                                                            else
                                                            {
                                                                pfAmount = standardBasic * Convert.ToDouble(objPFConfig.Rows[0]["EPF"]) / 100;
                                                                pfPension = standardBasic * Convert.ToDouble(objPFConfig.Rows[0]["EPS"]) / 100;
                                                            }
                                                                
                                                        }
                                                        else
                                                        {
                                                            if (standardBasic > Convert.ToInt32(objPFConfig.Rows[0]["PFAmtLimit"]))
                                                            {
                                                                pfAmount = Convert.ToInt32(objPFConfig.Rows[0]["PFAmtLimit"]) * Convert.ToDouble(objPFConfig.Rows[0]["PF"]) / 100;
                                                            }
                                                            else
                                                            {
                                                                pfAmount = standardBasic * Convert.ToDouble(objPFConfig.Rows[0]["PF"]) / 100;
                                                            }
                                                             
                                                        }
                                                    }
                                                }
                                            }

                                            //dr["providendFund"] = Math.Round(pfAmount);
                                            //dr["pfPension"] = Math.Round(pfPension);

                                            deduction = deduction + Math.Round(pfAmount) + Math.Round(pfPension);

                                            #endregion

                                            #region[Prof Tax]
                                            //Prof Tax Calculation
                                            if (objDTHamaliSal.Rows[j]["ProfTaxApplicable"] != DBNull.Value)
                                            {
                                                if (objDTHamaliSal.Rows[j]["ProfTaxApplicable"].ToString() == "Y")
                                                {
                                                    strProfTaxConfig = "select * from M_ProfTaxConfiguration WHERE (RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) = (SELECT  max(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) AS Expr1 FROM dbo.M_ProfTaxConfiguration WHERE(RIGHT(MonYrcd, 4) +LEFT(MonYrcd, 2) <='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "') AND(OrgId =" + Convert.ToInt32(Session["OrgID"]) + ") and statecd= " + Convert.ToInt16(objDTHamaliSal.Rows[j]["WStatecd"]) + " and gendercd=" + Convert.ToInt16(objDTHamaliSal.Rows[j]["gendercd"]) + " )) AND(OrgId = " + Convert.ToInt32(Session["OrgID"]) + ") and statecd= " + Convert.ToInt16(objDTHamaliSal.Rows[j]["WStatecd"]) + " and gendercd=" + Convert.ToInt16(objDTHamaliSal.Rows[j]["gendercd"]);
                                                    objProfTaxConfig = SqlHelper.ExecuteDataTable(strProfTaxConfig, AppGlobal.strConnString);
                                                    if (objProfTaxConfig.Rows.Count > 0)
                                                    {
                                                        for (int k = 0; k < objProfTaxConfig.Rows.Count; k++)
                                                        {
                                                            if (objProfTaxConfig.Rows[k]["Mon"] != DBNull.Value)
                                                            {
                                                                if (objProfTaxConfig.Rows[k]["Mon"].ToString() == ddlMon.SelectedValue)
                                                                {
                                                                    monAmt = Convert.ToDouble(objProfTaxConfig.Rows[k]["MonAmount"]);
                                                                }
                                                            }
                                                            if (grossSalary >= Convert.ToDouble(objProfTaxConfig.Rows[k]["FrAmount"]) && grossSalary <= Convert.ToDouble(objProfTaxConfig.Rows[k]["ToAmount"]))
                                                            {
                                                                profTaxAmt = monAmt + Convert.ToDouble(objProfTaxConfig.Rows[k]["TaxAmount"]);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            //dr["professionalTax"] = Math.Round(profTaxAmt);
                                            deduction = deduction + Math.Round(profTaxAmt);
                                            #endregion

                                            #region[Labour Walfare ]
                                            //Labour Walfare Applicable
                                            if (objDTHamaliSal.Rows[j]["LabWelApplicable"] != DBNull.Value)
                                            {
                                                if (objDTHamaliSal.Rows[j]["LabWelApplicable"].ToString() == "Y")
                                                {
                                                    strLabWelFunConfig = "SELECT * FROM M_LwfConfiguration WHERE(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) = (SELECT MAX(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) AS Expr1 FROM M_LwfConfiguration AS M_LwfConfiguration1 WHERE (RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) <= '" + ddlYear.SelectedValue + ddlMon.SelectedValue + "') AND  LEFT(MonYrcd, 2)='" + ddlMon.SelectedValue + "' AND (OrgId = " + Convert.ToInt32(Session["OrgID"]) + "))) AND (OrgId = " + Convert.ToInt32(Session["OrgID"]) + ")";
                                                    objLabWelFunConfig = SqlHelper.ExecuteDataTable(strLabWelFunConfig, AppGlobal.strConnString);
                                                    if (objLabWelFunConfig.Rows.Count > 0)
                                                    {
                                                        if (objLabWelFunConfig.Rows[0]["lwf"] != DBNull.Value)
                                                        {
                                                            lwf = Convert.ToDouble(objLabWelFunConfig.Rows[0]["lwf"]);
                                                        }
                                                        if (objLabWelFunConfig.Rows[0]["LWFCompContri"] != DBNull.Value)
                                                        {
                                                            lwfCompContri = Convert.ToDouble(objLabWelFunConfig.Rows[0]["LWFCompContri"]);
                                                        }
                                                    }
                                                }
                                            }
                                            //dr["lwf"] = Math.Round(lwf);
                                            //dr["lwfCompContri"] = Math.Round(lwfCompContri);
                                            deduction = deduction + Math.Round(lwf);
                                            #endregion

                                            #region[ESI]
                                            //ESI Calculation 
                                            if (objDTHamaliSal.Rows[j]["ESIApplicable"] != DBNull.Value)
                                            {
                                                string ESIApplicable = objDTHamaliSal.Rows[j]["ESIApplicable"].ToString();
                                                string ESINotApplicableDt = objDTHamaliSal.Rows[j]["ESINotApplicableDt"].ToString();
                                                string YrMonthCd = "";
                                                if (ESINotApplicableDt != "")
                                                {
                                                    YrMonthCd = Convert.ToDateTime(ESINotApplicableDt).Year.ToString() + (Convert.ToDateTime(ESINotApplicableDt).Month.ToString().Length==1 ? "0" + Convert.ToDateTime(ESINotApplicableDt).Month.ToString() : Convert.ToDateTime(ESINotApplicableDt).Month.ToString());
                                                }
                                                if (YrMonthCd != "")
                                                {
                                                    int CurrYrMnthCd = Convert.ToInt32(ddlYear.SelectedValue + ddlMon.SelectedValue);
                                                    int ESINotApplicableYrMnthCd = Convert.ToInt32(YrMonthCd);
                                                    if(CurrYrMnthCd< ESINotApplicableYrMnthCd)
                                                    {
                                                        ESIApplicable = "Y";
                                                    }
                                                }

                                                if (ESIApplicable == "Y")
                                                {
                                                    if (objDTHamaliSal.Rows[j]["ESICalculate"] != DBNull.Value)
                                                    {
                                                        strESIConfig = "select top(1) * FROM dbo.M_ESIConfigure AS M_ESIConfigure1 WHERE (convert(int, RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) <=" + ddlYear.SelectedValue + ddlMon.SelectedValue + ") and orgid=" + Convert.ToInt32(Session["OrgID"]) + " order by convert(int, RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) desc";
                                                        objESIConfig = SqlHelper.ExecuteDataTable(strESIConfig, AppGlobal.strConnString);
                                                        if (objESIConfig.Rows.Count > 0)
                                                        {
                                                            if (objESIConfig.Rows[0]["amount"] != DBNull.Value)
                                                                esiConfigAmt = Convert.ToDouble(objESIConfig.Rows[0]["amount"]);
                                                            if (objESIConfig.Rows[0]["ESIEmpPer"] != DBNull.Value)
                                                                esiEmpPer = Convert.ToDouble(objESIConfig.Rows[0]["ESIEmpPer"]);
                                                            if (objESIConfig.Rows[0]["ESICompPer"] != DBNull.Value)
                                                                esiCompPer = Convert.ToDouble(objESIConfig.Rows[0]["ESICompPer"]);

                                                            //increament 21000
                                                            //if (basicfrEsi <= esiConfigAmt)
                                                            //{
                                                                lastDay = 0;
                                                                lastDay = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMon.SelectedValue));
                                                                salDate = ddlYear.SelectedValue + "/" + ddlMon.SelectedValue + "/" + lastDay;
                                                                //DateTime salDate = Convert.ToDateTime(dtSalaryDate);

                                                                strESIConfig1 = " select top(1) esi.*, emp.origjoindate,month(" + salDate + "),";
                                                                strESIConfig1 += " case ";
                                                                strESIConfig1 += " when " + salDate + "  < origjoindate then 'false'";
                                                                strESIConfig1 += " when (month(origjoindate) >=4 and month(origjoindate) <=9 and year(origjoindate) = year(" + salDate + ") and month(" + salDate + ")>=4 and month(" + salDate + ")<=9) ";
                                                                strESIConfig1 += " or (month(origjoindate) < 4 and month(" + salDate + ") < 4 and  year(origjoindate) = year(" + salDate + "))";
                                                                strESIConfig1 += " or (month(origjoindate) > 9 and ((year(origjoindate) = year(" + salDate + ") and  month(" + salDate + ")> 9) or (year(origjoindate) = year(" + salDate + ")-1 and  month(" + salDate + ")< 4)))";
                                                                strESIConfig1 += " then 'True' else 'flase'  end as esi, ";
                                                                strESIConfig1 += " case when ESICalculate = 1 then " + basicForEsi * esiEmpPer / 100 + "   else  " + grossSalary * esiEmpPer / 100 + "  end as esiEmpAmt , ";
                                                                strESIConfig1 += " case when ESICalculate = 1 then " + basicForEsi * esiCompPer / 100 + "   else  " + grossSalary * esiCompPer / 100 + "  end as esiCompAmt  ";
                                                                strESIConfig1 += " from M_Emp as emp left outer join M_ESIConfigure as esi on emp.orgid=esi.orgID where employeecd=" + objDTHamaliSal.Rows[j]["employeecd"].ToString() + " and (convert(int, RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) <='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "') and esi.orgid=" + Convert.ToInt16(Session["OrgID"]) + " order by convert(int, RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) desc";
                                                                objESIConfig1 = SqlHelper.ExecuteDataTable(strESIConfig1, AppGlobal.strConnString);
                                                                if (objESIConfig1.Rows.Count > 0)
                                                                {
                                                                    esiEmpAmount = Convert.ToDouble(objESIConfig1.Rows[0]["esiEmpAmt"]);
                                                                    esiCompAmount = Convert.ToDouble(objESIConfig1.Rows[0]["esiCompAmt"]);
                                                                }
                                                           // }
                                                        }
                                                    }
                                                }
                                            }

                                            //dr["esiEmpPer"] = Math.Round(esiEmpPer);
                                            //dr["esiCompPer"] = Math.Round(esiCompPer);

                                            //dr["ESIEmpContribution"] = Math.Round(esiEmpAmount);
                                            //dr["ESICompContribution"] = Math.Round(esiCompAmount);

                                            deduction = deduction + Math.Ceiling(esiEmpAmount);

                                            #endregion


                                            dr["providendFund"] = Math.Round(pfAmount);
                                            dr["pfPension"] = Math.Round(pfPension);
                                            dr["EPF"] = epf;
                                            dr["EPS"] = eps;

                                            dr["professionalTax"] = Math.Round(profTaxAmt);

                                            dr["lwf"] = Math.Round(lwf);
                                            dr["lwfCompContri"] = Math.Round(lwfCompContri);

                                            dr["esiEmpPer"] = esiEmpPer;
                                            dr["esiCompPer"] = esiCompPer;

                                            dr["ESIEmpContribution"] = Math.Ceiling(esiEmpAmount);
                                            dr["ESICompContribution"] = Math.Round(esiCompAmount);

                                            dr["deduction"] = Math.Round(Convert.ToDouble(deduction));
                                            #endregion

                                            netSalary = Math.Round(Convert.ToDouble(grossSalary) - Convert.ToDouble(deduction));

                                            dr["net"] = Convert.ToDouble(netSalary);
                                        }

                                        //If (totalhamali - gross < 60) then Extra amount and half day amount added in incentive 
                                        if (Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["TotalHamali"])) - Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Gross"])) < 70)//60
                                        {
                                            double halfday = 0.5;
                                            presentDays = workingDays - 0.5;
                                            dr["payDays"] = presentDays;                                           
                                            dr["PaybleDays"] = presentDays;
                                            dr["presentDays"] = presentDays;                                            

                                            absentDays = workingDays - presentDays;
                                            dr["AbsentDays"] = absentDays;

                                            #region[Earning]
                                            standardBasic = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["BasicDA"]) / workingDays * presentDays);
                                            insentive = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["BasicDA"]) / workingDays * halfday);
                                            grossSalary = standardBasic;

                                            standardHRA = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["HRA"]) / workingDays * presentDays);
                                            insentive = insentive + Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["HRA"]) / workingDays * halfday);
                                            grossSalary = grossSalary + standardHRA;

                                            standConveyance = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Conveyance"]) / workingDays * presentDays);
                                            insentive = insentive + Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Conveyance"]) / workingDays * halfday);
                                            grossSalary = grossSalary + standConveyance;

                                            stadEducation = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Education"]) / workingDays * presentDays);
                                            insentive = insentive + Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Education"]) / workingDays * halfday);
                                            grossSalary = grossSalary + stadEducation;

                                            standardMedical = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["medical"]) / workingDays * presentDays);
                                            insentive = insentive + Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["medical"]) / workingDays * halfday);
                                            grossSalary = grossSalary + standardMedical;

                                            stadCanteen = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Canteen"]) / workingDays * presentDays);
                                            insentive = insentive + Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Canteen"]) / workingDays * halfday);
                                            grossSalary = grossSalary + stadCanteen;

                                            standardWashing = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Washing"]) / workingDays * presentDays);
                                            insentive = insentive + Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Washing"]) / workingDays * halfday);
                                            grossSalary = grossSalary + standardWashing;

                                            stadUniform = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Uniform"]) / workingDays * presentDays);
                                            insentive = insentive + Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Uniform"]) / workingDays * halfday);
                                            grossSalary = grossSalary + stadUniform;

                                            add1 = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Add1"]) / workingDays * presentDays);
                                            insentive = insentive + Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Add1"]) / workingDays * halfday);
                                            grossSalary = grossSalary + add1;

                                            add2 = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Add2"]) / workingDays * presentDays);
                                            insentive = insentive + Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Add2"]) / workingDays * halfday);
                                            grossSalary = grossSalary + add2;

                                            add3 = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Add3"]) / workingDays * presentDays);
                                            insentive = insentive + Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Add3"]) / workingDays * halfday);
                                            grossSalary = grossSalary + add3;

                                            //insentive = insentive + Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["TotalHamali"]) - grossSalary);

                                            insentive = insentive + Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["TotalHamali"])) - Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Gross"]));

                                            grossSalary = grossSalary + Math.Round(Convert.ToDouble(insentive));


                                            //dr["payDays"] = presentDays;
                                            //dr["PaybleDays"] = presentDays;
                                            //dr["presentDays"] = presentDays;

                                            dr["BasicDA"] = standardBasic;
                                            basicForEsi = basicForEsi + standardBasic;

                                            dr["HRA"] = standardHRA;
                                            basicForEsi = basicForEsi + standardHRA;

                                            dr["CA"] = standConveyance;
                                            dr["education"] = stadEducation;
                                            basicForEsi = basicForEsi + stadEducation;

                                            dr["medical"] = standardMedical;
                                            basicForEsi = basicForEsi + standardMedical;
                                            dr["Canteen"] = stadCanteen;
                                            basicForEsi = basicForEsi + stadCanteen; // Newlly added for esi calculation on 1/10/2021
                                            dr["Washing"] = standardWashing;
                                            dr["Uniform"] = stadUniform;
                                            basicForEsi = basicForEsi + stadUniform; // Newlly added for esi calculation on 1/10/2021
                                            dr["Add1"] = add1;
                                            basicForEsi = basicForEsi + add1; // Newlly added for esi calculation on 10/03/2021

                                            dr["Add2"] = add2;
                                            dr["Add3"] = add3;
                                            dr["insentive"] = insentive;


                                            dr["gross"] = Math.Round(Convert.ToDouble(grossSalary));
                                            //For ESI
                                            basicForEsi = basicForEsi + insentive;
                                            #endregion

                                            #region[Deduction]

                                            if (objDT.Rows[i]["Advance"] != DBNull.Value)
                                            {
                                                dr["salaryAdvance"] = Math.Round(Convert.ToDouble(objDT.Rows[i]["Advance"]));
                                                deduction = deduction + Math.Round(Convert.ToDouble(objDT.Rows[i]["Advance"]));
                                            }
                                            if (objDT.Rows[i]["Loan"] != DBNull.Value)
                                            {
                                                dr["Loan"] = Math.Round(Convert.ToDouble(objDT.Rows[i]["Loan"]));
                                                deduction = deduction + Math.Round(Convert.ToDouble(objDT.Rows[i]["Loan"]));
                                            }
                                            if (objDT.Rows[i]["TDS"] != DBNull.Value)
                                            {
                                                dr["TDS"] = Math.Round(Convert.ToDouble(objDT.Rows[i]["TDS"]));
                                                deduction = deduction + Math.Round(Convert.ToDouble(objDT.Rows[i]["TDS"]));
                                            }
                                            if (objDT.Rows[i]["TardalPathsansth"] != DBNull.Value)
                                            {
                                                dr["TardalPathsansth"] = Math.Round(Convert.ToDouble(objDT.Rows[i]["TardalPathsansth"]));
                                                deduction = deduction + Math.Round(Convert.ToDouble(objDT.Rows[i]["TardalPathsansth"]));
                                            }
                                            if (objDT.Rows[i]["Ded1"] != DBNull.Value)
                                            {
                                                dr["Ded1"] = Math.Round(Convert.ToDouble(objDT.Rows[i]["Ded1"]));
                                                deduction = deduction + Math.Round(Convert.ToDouble(objDT.Rows[i]["Ded1"]));
                                            }

                                            #region[PF Config]
                                            //PF Calculation  
                                            if (objDTHamaliSal.Rows[j]["PFApplicable"] != DBNull.Value)
                                            {
                                                if (objDTHamaliSal.Rows[j]["PFApplicable"].ToString() != "N" && Convert.ToDateTime(dt) >= Convert.ToDateTime(objDT.Rows[i]["PFJoindate"]))
                                                {
                                                    strPfConfig = "select * from M_PFConfiguration WHERE RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) = (SELECT  max(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) as MonYrcd  FROM dbo.M_PFConfiguration WHERE RIGHT(MonYrcd, 4) +LEFT(MonYrcd, 2) <= '" + ddlYear.SelectedValue + ddlMon.SelectedValue + "' and orgid = " + Convert.ToInt32(Session["OrgID"]) + " ) AND(OrgId = " + Convert.ToInt32(Session["OrgID"]) + ")";
                                                    objPFConfig = SqlHelper.ExecuteDataTable(strPfConfig, AppGlobal.strConnString);
                                                    if (objPFConfig.Rows.Count > 0)
                                                    {
                                                        empAge = getAge(Convert.ToDateTime(objDTHamaliSal.Rows[j]["Birthdate"]));
                                                        if (empAge <= Convert.ToInt32(objPFConfig.Rows[0]["PFAgeLimit"]))
                                                        {
                                                            epf = Convert.ToDouble(objPFConfig.Rows[0]["EPF"]);
                                                            eps = Convert.ToDouble(objPFConfig.Rows[0]["EPS"]);
                                                            if ((standardBasic > Convert.ToInt32(objPFConfig.Rows[0]["PFAmtLimit"])) && objDTHamaliSal.Rows[j]["PFApplicable"].ToString() != "A")
                                                            {
                                                                pfAmount = Convert.ToInt32(objPFConfig.Rows[0]["PFAmtLimit"]) * Convert.ToDouble(objPFConfig.Rows[0]["EPF"]) / 100;
                                                                pfPension = Convert.ToInt32(objPFConfig.Rows[0]["PFAmtLimit"]) * Convert.ToDouble(objPFConfig.Rows[0]["EPS"]) / 100;
                                                            }
                                                            else
                                                            {
                                                                pfAmount = standardBasic * Convert.ToDouble(objPFConfig.Rows[0]["EPF"]) / 100;
                                                                pfPension = standardBasic * Convert.ToDouble(objPFConfig.Rows[0]["EPS"]) / 100;
                                                            }
                                                                
                                                        }
                                                        else
                                                        {
                                                            if (standardBasic > Convert.ToInt32(objPFConfig.Rows[0]["PFAmtLimit"]))
                                                            {
                                                                pfAmount = Convert.ToInt32(objPFConfig.Rows[0]["PFAmtLimit"]) * Convert.ToDouble(objPFConfig.Rows[0]["PF"]) / 100;
                                                            }
                                                            else
                                                            {
                                                                pfAmount = standardBasic * Convert.ToDouble(objPFConfig.Rows[0]["PF"]) / 100;
                                                            }
                                                                
                                                        }
                                                    }
                                                }
                                            }
                                            //dr["providendFund"] = Math.Round(pfAmount);
                                            //dr["pfPension"] = Math.Round(pfPension);

                                            deduction = deduction + Math.Round(pfAmount) + Math.Round(pfPension);
                                            #endregion

                                            #region[Prof Tax]
                                            //Prof Tax Calculation
                                            if (objDTHamaliSal.Rows[j]["ProfTaxApplicable"] != DBNull.Value)
                                            {
                                                if (objDTHamaliSal.Rows[j]["ProfTaxApplicable"].ToString() == "Y")
                                                {
                                                    strProfTaxConfig = "select * from M_ProfTaxConfiguration WHERE (RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) = (SELECT  max(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) AS Expr1 FROM dbo.M_ProfTaxConfiguration WHERE(RIGHT(MonYrcd, 4) +LEFT(MonYrcd, 2) <='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "') AND(OrgId =" + Convert.ToInt32(Session["OrgID"]) + ") and statecd= " + Convert.ToInt16(objDTHamaliSal.Rows[j]["WStatecd"]) + " and gendercd=" + Convert.ToInt16(objDTHamaliSal.Rows[j]["gendercd"]) + " )) AND(OrgId = " + Convert.ToInt32(Session["OrgID"]) + ") and statecd= " + Convert.ToInt16(objDTHamaliSal.Rows[j]["WStatecd"]) + " and gendercd=" + Convert.ToInt16(objDTHamaliSal.Rows[j]["gendercd"]);
                                                    objProfTaxConfig = SqlHelper.ExecuteDataTable(strProfTaxConfig, AppGlobal.strConnString);
                                                    if (objProfTaxConfig.Rows.Count > 0)
                                                    {
                                                        for (int k = 0; k < objProfTaxConfig.Rows.Count; k++)
                                                        {
                                                            if (objProfTaxConfig.Rows[k]["Mon"] != DBNull.Value)
                                                            {
                                                                if (objProfTaxConfig.Rows[k]["Mon"].ToString() == ddlMon.SelectedValue)
                                                                {
                                                                    monAmt = Convert.ToDouble(objProfTaxConfig.Rows[k]["MonAmount"]);
                                                                }
                                                            }
                                                            if (grossSalary >= Convert.ToDouble(objProfTaxConfig.Rows[k]["FrAmount"]) && grossSalary <= Convert.ToDouble(objProfTaxConfig.Rows[k]["ToAmount"]))
                                                            {
                                                                profTaxAmt = monAmt + Convert.ToDouble(objProfTaxConfig.Rows[k]["TaxAmount"]);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            //dr["professionalTax"] = Math.Round(profTaxAmt);
                                            deduction = deduction + Math.Round(profTaxAmt);
                                            #endregion

                                            #region[Labour Walfare ]
                                            //Labour Walfare Applicable
                                            if (objDTHamaliSal.Rows[j]["LabWelApplicable"] != DBNull.Value)
                                            {
                                                if (objDTHamaliSal.Rows[j]["LabWelApplicable"].ToString() == "Y")
                                                {
                                                    strLabWelFunConfig = "SELECT* FROM M_LwfConfiguration WHERE(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) = (SELECT MAX(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) AS Expr1 FROM M_LwfConfiguration AS M_LwfConfiguration1 WHERE(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) <= '" + ddlYear.SelectedValue + ddlMon.SelectedValue + "') AND LEFT(MonYrcd, 2)='" + ddlMon.SelectedValue + "' AND (OrgId = " + Convert.ToInt32(Session["OrgID"]) + "))) AND(OrgId = " + Convert.ToInt32(Session["OrgID"]) + ")";
                                                    objLabWelFunConfig = SqlHelper.ExecuteDataTable(strLabWelFunConfig, AppGlobal.strConnString);
                                                    if (objLabWelFunConfig.Rows.Count > 0)
                                                    {
                                                        if (objLabWelFunConfig.Rows[0]["lwf"] != DBNull.Value)
                                                        {
                                                            lwf = Convert.ToDouble(objLabWelFunConfig.Rows[0]["lwf"]);
                                                        }
                                                        if (objLabWelFunConfig.Rows[0]["LWFCompContri"] != DBNull.Value)
                                                        {
                                                            lwfCompContri = Convert.ToDouble(objLabWelFunConfig.Rows[0]["LWFCompContri"]);
                                                        }
                                                    }
                                                }
                                            }
                                            //dr["lwf"] = Math.Round(lwf);
                                            //dr["lwfCompContri"] = Math.Round(lwfCompContri);
                                            deduction = deduction + Math.Round(lwf);
                                            #endregion

                                            #region[ESI]
                                            //ESI Calculation 
                                            if (objDTHamaliSal.Rows[j]["ESIApplicable"] != DBNull.Value)
                                            {
                                                string ESIApplicable = objDTHamaliSal.Rows[j]["ESIApplicable"].ToString();
                                                string ESINotApplicableDt = objDTHamaliSal.Rows[j]["ESINotApplicableDt"].ToString();
                                                string YrMonthCd = "";
                                                if (ESINotApplicableDt != "")
                                                {
                                                    YrMonthCd = Convert.ToDateTime(ESINotApplicableDt).Year.ToString() + (Convert.ToDateTime(ESINotApplicableDt).Month.ToString().Length == 1 ? "0" + Convert.ToDateTime(ESINotApplicableDt).Month.ToString() : Convert.ToDateTime(ESINotApplicableDt).Month.ToString());
                                                }
                                                if (YrMonthCd != "")
                                                {
                                                    int CurrYrMnthCd = Convert.ToInt32(ddlYear.SelectedValue + ddlMon.SelectedValue);
                                                    int ESINotApplicableYrMnthCd = Convert.ToInt32(YrMonthCd);
                                                    if (CurrYrMnthCd < ESINotApplicableYrMnthCd)
                                                    {
                                                        ESIApplicable = "Y";
                                                    }
                                                }

                                                if (ESIApplicable == "Y")
                                                {
                                                    if (objDTHamaliSal.Rows[j]["ESICalculate"] != DBNull.Value)
                                                    {
                                                        strESIConfig = "select top(1) * FROM dbo.M_ESIConfigure AS M_ESIConfigure1 WHERE (convert(int, RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) <=" + ddlYear.SelectedValue + ddlMon.SelectedValue + ") and orgid=" + Convert.ToInt32(Session["OrgID"]) + " order by convert(int, RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) desc";
                                                        objESIConfig = SqlHelper.ExecuteDataTable(strESIConfig, AppGlobal.strConnString);
                                                        if (objESIConfig.Rows.Count > 0)
                                                        {
                                                            if (objESIConfig.Rows[0]["amount"] != DBNull.Value)
                                                                esiConfigAmt = Convert.ToDouble(objESIConfig.Rows[0]["amount"]);
                                                            if (objESIConfig.Rows[0]["ESIEmpPer"] != DBNull.Value)
                                                                esiEmpPer = Convert.ToDouble(objESIConfig.Rows[0]["ESIEmpPer"]);
                                                            if (objESIConfig.Rows[0]["ESICompPer"] != DBNull.Value)
                                                                esiCompPer = Convert.ToDouble(objESIConfig.Rows[0]["ESICompPer"]);

                                                            // increament 21000
                                                           // if (basicfrEsi <= esiConfigAmt)
                                                           // {
                                                                lastDay = 0;
                                                                lastDay = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMon.SelectedValue));
                                                                salDate = ddlYear.SelectedValue + "/" + ddlMon.SelectedValue + "/" + lastDay;
                                                                //DateTime salDate = Convert.ToDateTime(dtSalaryDate);

                                                                strESIConfig1 = " select top(1) esi.*, emp.origjoindate,month(" + salDate + "),";
                                                                strESIConfig1 += " case ";
                                                                strESIConfig1 += " when " + salDate + "  < origjoindate then 'false'";
                                                                strESIConfig1 += " when (month(origjoindate) >=4 and month(origjoindate) <=9 and year(origjoindate) = year(" + salDate + ") and month(" + salDate + ")>=4 and month(" + salDate + ")<=9) ";
                                                                strESIConfig1 += " or (month(origjoindate) < 4 and month(" + salDate + ") < 4 and  year(origjoindate) = year(" + salDate + "))";
                                                                strESIConfig1 += " or (month(origjoindate) > 9 and ((year(origjoindate) = year(" + salDate + ") and  month(" + salDate + ")> 9) or (year(origjoindate) = year(" + salDate + ")-1 and  month(" + salDate + ")< 4)))";
                                                                strESIConfig1 += " then 'True' else 'flase'  end as esi, ";
                                                                strESIConfig1 += " case when ESICalculate = 1 then " + basicForEsi * esiEmpPer / 100 + "   else  " + grossSalary * esiEmpPer / 100 + "  end as esiEmpAmt , ";
                                                                strESIConfig1 += " case when ESICalculate = 1 then " + basicForEsi * esiCompPer / 100 + "   else  " + grossSalary * esiCompPer / 100 + "  end as esiCompAmt  ";
                                                                strESIConfig1 += " from M_Emp as emp left outer join M_ESIConfigure as esi on emp.orgid=esi.orgID where employeecd=" + objDTHamaliSal.Rows[j]["employeecd"].ToString() + " and (convert(int, RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) <='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "') and esi.orgid=" + Convert.ToInt16(Session["OrgID"]) + " order by convert(int, RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) desc";
                                                                objESIConfig1 = SqlHelper.ExecuteDataTable(strESIConfig1, AppGlobal.strConnString);
                                                                if (objESIConfig1.Rows.Count > 0)
                                                                {
                                                                    esiEmpAmount = Convert.ToDouble(objESIConfig1.Rows[0]["esiEmpAmt"]);
                                                                    esiCompAmount = Convert.ToDouble(objESIConfig1.Rows[0]["esiCompAmt"]);
                                                                }
                                                            //}
                                                        }
                                                    }
                                                }
                                            }

                                            //dr["esiEmpPer"] = Math.Round(esiEmpPer);
                                            //dr["esiCompPer"] = Math.Round(esiCompPer);

                                            //dr["ESIEmpContribution"] = Math.Round(esiEmpAmount);
                                            //dr["ESICompContribution"] = Math.Round(esiCompAmount);

                                            deduction = deduction + Math.Ceiling(esiEmpAmount);

                                            #endregion


                                            dr["providendFund"] = Math.Round(pfAmount);
                                            dr["pfPension"] = Math.Round(pfPension);
                                            dr["EPF"] = epf;
                                            dr["EPS"] = eps;

                                            dr["professionalTax"] = Math.Round(profTaxAmt);

                                            dr["lwf"] = Math.Round(lwf);
                                            dr["lwfCompContri"] = Math.Round(lwfCompContri);

                                            dr["esiEmpPer"] = esiEmpPer;
                                            dr["esiCompPer"] = esiCompPer;

                                            dr["ESIEmpContribution"] = Math.Ceiling(esiEmpAmount);
                                            dr["ESICompContribution"] = Math.Round(esiCompAmount);

                                            dr["deduction"] = Math.Round(Convert.ToDouble(deduction));
                                            #endregion

                                            netSalary = Math.Round(Convert.ToDouble(grossSalary) - Convert.ToDouble(deduction));
                                            dr["net"] = Convert.ToDouble(netSalary);

                                        }
                                        //-------------------------------------------------------------------------------

                                    }
                                    else
                                    {
                                        //Regular Hamali Salary Processing
                                        #region[Earning]
                                        stadGross = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Gross"]));

                                        oneDaySal = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Gross"])) / workingDays;

                                        presentDays = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["TotalHamali"])) / oneDaySal;
                                        int day = (int)presentDays;
                                        double fractional = presentDays - day;
                                        if (fractional > 0.5)
                                            fractional = 0.5;
                                        else if (fractional >= 0.000001 && fractional < 0.5)
                                            fractional = 0.0;

                                        presentDays = day + fractional;

                                        absentDays = workingDays - presentDays;

                                        while (grossSalary != Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["TotalHamali"])))
                                        {

                                            standardBasic = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["BasicDA"]) / workingDays * presentDays);
                                            grossSalary = standardBasic;

                                            standardHRA = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["HRA"]) / workingDays * presentDays);
                                            grossSalary = grossSalary + standardHRA;

                                            standConveyance = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Conveyance"]) / workingDays * presentDays);
                                            grossSalary = grossSalary + standConveyance;

                                            stadEducation = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Education"]) / workingDays * presentDays);
                                            grossSalary = grossSalary + stadEducation;

                                            standardMedical = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["medical"]) / workingDays * presentDays);
                                            grossSalary = grossSalary + standardMedical;

                                            stadCanteen = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Canteen"]) / workingDays * presentDays);
                                            grossSalary = grossSalary + stadCanteen;

                                            standardWashing = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Washing"]) / workingDays * presentDays);
                                            grossSalary = grossSalary + standardWashing;

                                            stadUniform = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Uniform"]) / workingDays * presentDays);
                                            grossSalary = grossSalary + stadUniform;

                                            add1 = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Add1"]) / workingDays * presentDays);
                                            grossSalary = grossSalary + add1;

                                            add2 = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Add2"]) / workingDays * presentDays);
                                            grossSalary = grossSalary + add2;

                                            add3 = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["Add3"]) / workingDays * presentDays);
                                            grossSalary = grossSalary + add3;

                                            if (grossSalary != Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["TotalHamali"])))
                                            {
                                                if (Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["TotalHamali"])) - grossSalary >= 70)//60
                                                {
                                                    insentive = Math.Round(Convert.ToDouble(objDTHamaliSal.Rows[j]["TotalHamali"]) - grossSalary);
                                                }
                                                else
                                                {
                                                    presentDays -= 0.50;
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }

                                            grossSalary = grossSalary + Math.Round(Convert.ToDouble(insentive));
                                        }
                                        //Changed for PF/ECR Report on 30-11-2023
                                        //dr["payDays"] = presentDays; 
                                        dr["payDays"] = workingDays;
                                        dr["PaybleDays"] = presentDays;
                                        dr["presentDays"] = presentDays;
                                        dr["AbsentDays"] = absentDays;

                                        dr["BasicDA"] = standardBasic;
                                        basicForEsi = basicForEsi + standardBasic;

                                        dr["HRA"] = standardHRA;
                                        basicForEsi = basicForEsi + standardHRA;

                                        dr["CA"] = standConveyance;
                                        dr["education"] = stadEducation;
                                        basicForEsi = basicForEsi + stadEducation;

                                        dr["medical"] = standardMedical;
                                        basicForEsi = basicForEsi + standardMedical;
                                        dr["Canteen"] = stadCanteen;
                                        basicForEsi = basicForEsi + stadCanteen;// Newlly added for esi calculation on 1/10/2021
                                        dr["Washing"] = standardWashing;
                                        dr["Uniform"] = stadUniform;
                                        basicForEsi = basicForEsi + stadUniform;// Newlly added for esi calculation on 1/10/2021
                                        dr["Add1"] = add1;
                                        basicForEsi = basicForEsi + add1;// Newlly added for esi calculation on 10/03/2021

                                        dr["Add2"] = add2;
                                        dr["Add3"] = add3;
                                        dr["insentive"] = insentive;


                                        dr["gross"] = Math.Round(Convert.ToDouble(grossSalary));

                                        //For ESI
                                        basicForEsi = basicForEsi + insentive;
                                        #endregion

                                        #region[Deduction]

                                        if (objDT.Rows[i]["Advance"] != DBNull.Value)
                                        {
                                            dr["salaryAdvance"] = Math.Round(Convert.ToDouble(objDT.Rows[i]["Advance"]));
                                            deduction = deduction + Math.Round(Convert.ToDouble(objDT.Rows[i]["Advance"]));
                                        }
                                        if (objDT.Rows[i]["Loan"] != DBNull.Value)
                                        {
                                            dr["Loan"] = Math.Round(Convert.ToDouble(objDT.Rows[i]["Loan"]));
                                            deduction = deduction + Math.Round(Convert.ToDouble(objDT.Rows[i]["Loan"]));
                                        }
                                        if (objDT.Rows[i]["TDS"] != DBNull.Value)
                                        {
                                            dr["TDS"] = Math.Round(Convert.ToDouble(objDT.Rows[i]["TDS"]));
                                            deduction = deduction + Math.Round(Convert.ToDouble(objDT.Rows[i]["TDS"]));
                                        }
                                        if (objDT.Rows[i]["TardalPathsansth"] != DBNull.Value)
                                        {
                                            dr["TardalPathsansth"] = Math.Round(Convert.ToDouble(objDT.Rows[i]["TardalPathsansth"]));
                                            deduction = deduction + Math.Round(Convert.ToDouble(objDT.Rows[i]["TardalPathsansth"]));
                                        }
                                        if (objDT.Rows[i]["Ded1"] != DBNull.Value)
                                        {
                                            dr["Ded1"] = Math.Round(Convert.ToDouble(objDT.Rows[i]["Ded1"]));
                                            deduction = deduction + Math.Round(Convert.ToDouble(objDT.Rows[i]["Ded1"]));
                                        }

                                        #region[PF Config]
                                        //PF Calculation  
                                        if (objDTHamaliSal.Rows[j]["PFApplicable"] != DBNull.Value)
                                        {
                                            if (objDTHamaliSal.Rows[j]["PFApplicable"].ToString() != "N" && Convert.ToDateTime(dt) >= Convert.ToDateTime(objDT.Rows[i]["PFJoindate"]))
                                            {
                                                strPfConfig = "select * from M_PFConfiguration WHERE RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) = (SELECT  max(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) as MonYrcd  FROM dbo.M_PFConfiguration WHERE RIGHT(MonYrcd, 4) +LEFT(MonYrcd, 2) <= '" + ddlYear.SelectedValue + ddlMon.SelectedValue + "' and orgid = " + Convert.ToInt32(Session["OrgID"]) + " ) AND(OrgId = " + Convert.ToInt32(Session["OrgID"]) + ")";
                                                objPFConfig = SqlHelper.ExecuteDataTable(strPfConfig, AppGlobal.strConnString);
                                                if (objPFConfig.Rows.Count > 0)
                                                {
                                                    empAge = getAge(Convert.ToDateTime(objDTHamaliSal.Rows[j]["Birthdate"]));
                                                    if (empAge <= Convert.ToInt32(objPFConfig.Rows[0]["PFAgeLimit"]))
                                                    {
                                                        epf = Convert.ToDouble(objPFConfig.Rows[0]["EPF"]);
                                                        eps = Convert.ToDouble(objPFConfig.Rows[0]["EPS"]);

                                                        if ((standardBasic > Convert.ToInt32(objPFConfig.Rows[0]["PFAmtLimit"])) && objDTHamaliSal.Rows[j]["PFApplicable"].ToString() != "A")
                                                        {
                                                            pfAmount = Convert.ToInt32(objPFConfig.Rows[0]["PFAmtLimit"]) * Convert.ToDouble(objPFConfig.Rows[0]["EPF"]) / 100;
                                                            pfPension = Convert.ToInt32(objPFConfig.Rows[0]["PFAmtLimit"]) * Convert.ToDouble(objPFConfig.Rows[0]["EPS"]) / 100;
                                                        }
                                                        else
                                                        {
                                                            pfAmount = standardBasic * Convert.ToDouble(objPFConfig.Rows[0]["EPF"]) / 100;
                                                            pfPension = standardBasic * Convert.ToDouble(objPFConfig.Rows[0]["EPS"]) / 100;
                                                        }
                                                            
                                                    }
                                                    else
                                                    {
                                                        if (standardBasic > Convert.ToInt32(objPFConfig.Rows[0]["PFAmtLimit"]))
                                                        {
                                                            pfAmount = Convert.ToInt32(objPFConfig.Rows[0]["PFAmtLimit"]) * Convert.ToDouble(objPFConfig.Rows[0]["PF"]) / 100;
                                                        }
                                                        else
                                                        {
                                                            pfAmount = standardBasic * Convert.ToDouble(objPFConfig.Rows[0]["PF"]) / 100;
                                                        }
                                                            
                                                    }
                                                }
                                            }
                                        }
                                        //dr["providendFund"] = Math.Round(pfAmount);
                                        //dr["pfPension"] = Math.Round(pfPension);

                                        deduction = deduction + Math.Round(pfAmount) + Math.Round(pfPension);
                                        #endregion

                                        #region[Prof Tax]
                                        //Prof Tax Calculation
                                        if (objDTHamaliSal.Rows[j]["ProfTaxApplicable"] != DBNull.Value)
                                        {
                                            if (objDTHamaliSal.Rows[j]["ProfTaxApplicable"].ToString() == "Y")
                                            {
                                                strProfTaxConfig = "select * from M_ProfTaxConfiguration WHERE (RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) = (SELECT  max(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) AS Expr1 FROM dbo.M_ProfTaxConfiguration WHERE(RIGHT(MonYrcd, 4) +LEFT(MonYrcd, 2) <='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "') AND(OrgId =" + Convert.ToInt32(Session["OrgID"]) + ") and statecd= " + Convert.ToInt16(objDTHamaliSal.Rows[j]["WStatecd"]) + " and gendercd=" + Convert.ToInt16(objDTHamaliSal.Rows[j]["gendercd"]) + " )) AND(OrgId = " + Convert.ToInt32(Session["OrgID"]) + ") and statecd= " + Convert.ToInt16(objDTHamaliSal.Rows[j]["WStatecd"]) + " and gendercd=" + Convert.ToInt16(objDTHamaliSal.Rows[j]["gendercd"]);
                                                objProfTaxConfig = SqlHelper.ExecuteDataTable(strProfTaxConfig, AppGlobal.strConnString);
                                                if (objProfTaxConfig.Rows.Count > 0)
                                                {
                                                    for (int k = 0; k < objProfTaxConfig.Rows.Count; k++)
                                                    {
                                                        if (objProfTaxConfig.Rows[k]["Mon"] != DBNull.Value)
                                                        {
                                                            if (objProfTaxConfig.Rows[k]["Mon"].ToString() == ddlMon.SelectedValue)
                                                            {
                                                                monAmt = Convert.ToDouble(objProfTaxConfig.Rows[k]["MonAmount"]);
                                                            }
                                                        }
                                                        if (grossSalary >= Convert.ToDouble(objProfTaxConfig.Rows[k]["FrAmount"]) && grossSalary <= Convert.ToDouble(objProfTaxConfig.Rows[k]["ToAmount"]))
                                                        {
                                                            profTaxAmt = monAmt + Convert.ToDouble(objProfTaxConfig.Rows[k]["TaxAmount"]);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        //dr["professionalTax"] = Math.Round(profTaxAmt);
                                        deduction = deduction + Math.Round(profTaxAmt);
                                        #endregion

                                        #region[Labour Walfare ]
                                        //Labour Walfare Applicable
                                        if (objDTHamaliSal.Rows[j]["LabWelApplicable"] != DBNull.Value)
                                        {
                                            if (objDTHamaliSal.Rows[j]["LabWelApplicable"].ToString() == "Y")
                                            {
                                                strLabWelFunConfig = "SELECT* FROM M_LwfConfiguration WHERE(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) = (SELECT MAX(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) AS Expr1 FROM M_LwfConfiguration AS M_LwfConfiguration1 WHERE(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) <= '" + ddlYear.SelectedValue + ddlMon.SelectedValue + "') AND LEFT(MonYrcd, 2)='" + ddlMon.SelectedValue + "' AND (OrgId = " + Convert.ToInt32(Session["OrgID"]) + "))) AND(OrgId = " + Convert.ToInt32(Session["OrgID"]) + ")";
                                                objLabWelFunConfig = SqlHelper.ExecuteDataTable(strLabWelFunConfig, AppGlobal.strConnString);
                                                if (objLabWelFunConfig.Rows.Count > 0)
                                                {
                                                    if (objLabWelFunConfig.Rows[0]["lwf"] != DBNull.Value)
                                                    {
                                                        lwf = Convert.ToDouble(objLabWelFunConfig.Rows[0]["lwf"]);
                                                    }
                                                    if (objLabWelFunConfig.Rows[0]["LWFCompContri"] != DBNull.Value)
                                                    {
                                                        lwfCompContri = Convert.ToDouble(objLabWelFunConfig.Rows[0]["LWFCompContri"]);
                                                    }
                                                }
                                            }
                                        }
                                        //dr["lwf"] = Math.Round(lwf);
                                        //dr["lwfCompContri"] = Math.Round(lwfCompContri);
                                        deduction = deduction + Math.Round(lwf);
                                        #endregion

                                        #region[ESI]
                                        //ESI Calculation 
                                        if (objDTHamaliSal.Rows[j]["ESIApplicable"] != DBNull.Value)
                                        {
                                            string ESIApplicable = objDTHamaliSal.Rows[j]["ESIApplicable"].ToString();
                                            string ESINotApplicableDt = objDTHamaliSal.Rows[j]["ESINotApplicableDt"].ToString();
                                            string YrMonthCd = "";
                                            if (ESINotApplicableDt != "")
                                            {
                                                YrMonthCd = Convert.ToDateTime(ESINotApplicableDt).Year.ToString() + (Convert.ToDateTime(ESINotApplicableDt).Month.ToString().Length == 1 ? "0" + Convert.ToDateTime(ESINotApplicableDt).Month.ToString() : Convert.ToDateTime(ESINotApplicableDt).Month.ToString());
                                            }
                                            if (YrMonthCd != "")
                                            {
                                                int CurrYrMnthCd = Convert.ToInt32(ddlYear.SelectedValue + ddlMon.SelectedValue);
                                                int ESINotApplicableYrMnthCd = Convert.ToInt32(YrMonthCd);
                                                if (CurrYrMnthCd < ESINotApplicableYrMnthCd)
                                                {
                                                    ESIApplicable = "Y";
                                                }
                                            }

                                            if (ESIApplicable == "Y")
                                            {
                                                if (objDTHamaliSal.Rows[j]["ESICalculate"] != DBNull.Value)
                                                {
                                                    strESIConfig = "select top(1) * FROM dbo.M_ESIConfigure AS M_ESIConfigure1 WHERE (convert(int, RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) <=" + ddlYear.SelectedValue + ddlMon.SelectedValue + ") and orgid=" + Convert.ToInt32(Session["OrgID"]) + " order by convert(int, RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) desc";
                                                    objESIConfig = SqlHelper.ExecuteDataTable(strESIConfig, AppGlobal.strConnString);
                                                    if (objESIConfig.Rows.Count > 0)
                                                    {
                                                        if (objESIConfig.Rows[0]["amount"] != DBNull.Value)
                                                            esiConfigAmt = Convert.ToDouble(objESIConfig.Rows[0]["amount"]);
                                                        if (objESIConfig.Rows[0]["ESIEmpPer"] != DBNull.Value)
                                                            esiEmpPer = Convert.ToDouble(objESIConfig.Rows[0]["ESIEmpPer"]);
                                                        if (objESIConfig.Rows[0]["ESICompPer"] != DBNull.Value)
                                                            esiCompPer = Convert.ToDouble(objESIConfig.Rows[0]["ESICompPer"]);

                                                        //  increament 21000
                                                        //if (basicfrEsi <= esiConfigAmt)
                                                        //{
                                                        lastDay = 0;
                                                            lastDay = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMon.SelectedValue));
                                                            salDate = ddlYear.SelectedValue + "/" + ddlMon.SelectedValue + "/" + lastDay;
                                                            //DateTime salDate = Convert.ToDateTime(dtSalaryDate);

                                                            strESIConfig1 = " select top(1) esi.*, emp.origjoindate,month(" + salDate + "),";
                                                            strESIConfig1 += " case ";
                                                            strESIConfig1 += " when " + salDate + "  < origjoindate then 'false'";
                                                            strESIConfig1 += " when (month(origjoindate) >=4 and month(origjoindate) <=9 and year(origjoindate) = year(" + salDate + ") and month(" + salDate + ")>=4 and month(" + salDate + ")<=9) ";
                                                            strESIConfig1 += " or (month(origjoindate) < 4 and month(" + salDate + ") < 4 and  year(origjoindate) = year(" + salDate + "))";
                                                            strESIConfig1 += " or (month(origjoindate) > 9 and ((year(origjoindate) = year(" + salDate + ") and  month(" + salDate + ")> 9) or (year(origjoindate) = year(" + salDate + ")-1 and  month(" + salDate + ")< 4)))";
                                                            strESIConfig1 += " then 'True' else 'flase'  end as esi, ";
                                                            strESIConfig1 += " case when ESICalculate = 1 then " + basicForEsi * esiEmpPer / 100 + "   else  " + grossSalary * esiEmpPer / 100 + "  end as esiEmpAmt , ";
                                                            strESIConfig1 += " case when ESICalculate = 1 then " + basicForEsi * esiCompPer / 100 + "   else  " + grossSalary * esiCompPer / 100 + "  end as esiCompAmt  ";
                                                            strESIConfig1 += " from M_Emp as emp left outer join M_ESIConfigure as esi on emp.orgid=esi.orgID where employeecd=" + objDTHamaliSal.Rows[j]["employeecd"].ToString() + " and (convert(int, RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) <='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "') and esi.orgid=" + Convert.ToInt16(Session["OrgID"]) + " order by convert(int, RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) desc";
                                                            objESIConfig1 = SqlHelper.ExecuteDataTable(strESIConfig1, AppGlobal.strConnString);
                                                            if (objESIConfig1.Rows.Count > 0)
                                                            {
                                                                esiEmpAmount = Convert.ToDouble(objESIConfig1.Rows[0]["esiEmpAmt"]);
                                                                esiCompAmount = Convert.ToDouble(objESIConfig1.Rows[0]["esiCompAmt"]);
                                                            }
                                                        //}
                                                    }
                                                }
                                            }
                                        }

                                        //dr["esiEmpPer"] = Math.Round(esiEmpPer);
                                        //dr["esiCompPer"] = Math.Round(esiCompPer);

                                        //dr["ESIEmpContribution"] = Math.Round(esiEmpAmount);
                                        //dr["ESICompContribution"] = Math.Round(esiCompAmount);

                                        deduction = deduction + Math.Ceiling(esiEmpAmount);

                                        #endregion


                                        dr["providendFund"] = Math.Round(pfAmount);
                                        dr["pfPension"] = Math.Round(pfPension);
                                        dr["EPF"] = epf;
                                        dr["EPS"] = eps;

                                        dr["professionalTax"] = Math.Round(profTaxAmt);

                                        dr["lwf"] = Math.Round(lwf);
                                        dr["lwfCompContri"] = Math.Round(lwfCompContri);

                                        dr["esiEmpPer"] = esiEmpPer;
                                        dr["esiCompPer"] = esiCompPer;

                                        dr["ESIEmpContribution"] = Math.Ceiling(esiEmpAmount);
                                        dr["ESICompContribution"] = Math.Round(esiCompAmount);

                                        dr["deduction"] = Math.Round(Convert.ToDouble(deduction));
                                        #endregion

                                        netSalary = Math.Round(Convert.ToDouble(grossSalary) - Convert.ToDouble(deduction));
                                        dr["net"] = Convert.ToDouble(netSalary);
                                    }

                                    //Added on 03-01-2020 
                                    #region[Add NetSalary Less Than Below 50%]
                                    if ((grossSalary - deduction) < (grossSalary * 50 / 100))
                                    {
                                        if ((grossSalary - deduction) < (grossSalary * 49 / 100))
                                        {
                                            drl = dtEmpLessSalary.NewRow();
                                            drl["empCode"] = objDT.Rows[i]["Employeecd"].ToString();
                                            drl["empName"] = objDT.Rows[i]["Employeename"].ToString();
                                            drl["net"] = Convert.ToDouble((grossSalary - deduction));
                                            dtEmpLessSalary.Rows.Add(drl);
                                        }
                                    }
                                    #endregion

                                    dtEmpSalary.Rows.Add(dr);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            if (objDT.Rows[i]["Absent"] != DBNull.Value)
                                absentDays = Convert.ToDouble(objDT.Rows[i]["Absent"]);
                            
                            //For Total Absent 
                            if ((daysInMonth - weeklyOff) == absentDays)
                            {
                                //for Total Days Absent
                                #region[TotalAbsent]
                                dr = dtEmpSalary.NewRow();
                                
                                //From T_Attendence
                                #region[T_Attendence]
                                dr["empCode"] = objDT.Rows[i]["Employeecd"].ToString();
                                dr["empName"] = objDT.Rows[i]["Employeename"].ToString();
                                dr["daysInMonth"] = daysInMonth;
                                if (objDT.Rows[i]["Weeklyoff"] != DBNull.Value)
                                    dr["weeklyOff"] = weeklyOff = Convert.ToDouble(objDT.Rows[i]["Weeklyoff"]);
                                if (objDT.Rows[i]["Payholiday"] != DBNull.Value)
                                    dr["payholiday"] = payHolidays = Convert.ToDouble(objDT.Rows[i]["Payholiday"]);
                                dr["payDays"] = 0;
                                if (objDT.Rows[i]["PresentDay"] != DBNull.Value)
                                    dr["presentDays"] = presentDays = 0;
                                if (objDT.Rows[i]["PL"] != DBNull.Value)
                                    dr["PL"] = PL = 0;
                                if (objDT.Rows[i]["COff"] != DBNull.Value)
                                    dr["cOff"] = COff = 0;
                                if (objDT.Rows[i]["Absent"] != DBNull.Value)
                                    dr["AbsentDays"] = absentDays = Convert.ToDouble(objDT.Rows[i]["Absent"]);
                                dr["PaybleDays"] = 0;
                                #endregion

                                //M_Salary
                                #region[M_Salary]

                                // Earnings
                                #region[Earnings]

                                dr["BasicDA"] = 0;
                                dr["HRA"] = 0;
                                dr["CA"] = 0;
                                dr["medical"] = standardMedical;
                                dr["Canteen"] = stadCanteen;
                                dr["Washing"] = standardWashing;
                                dr["Uniform"] = stadUniform;
                                dr["Add1"] = add1;
                                dr["Add2"] = add2;
                                dr["Add3"] = add3;
                                dr["insentive"] = 0;
                                dr["gross"] = 0;

                                #endregion

                                //From T_Deduction
                                #region[Deduction]
                                #region[T_Deduction]
                                dr["salaryAdvance"] = 0;
                                dr["Loan"] = 0;
                                dr["TDS"] = 0;
                                dr["TardalPathsansth"] = 0;
                                dr["Ded1"] = 0;
                                #endregion

                                dr["providendFund"] = 0;
                                dr["pfPension"] = 0;
                                dr["EPF"] = 0;
                                dr["EPS"] = 0;

                                dr["professionalTax"] = 0;

                                dr["lwf"] = 0;
                                dr["lwfCompContri"] = 0;

                                dr["esiEmpPer"] = 0;
                                dr["esiCompPer"] = 0;

                                dr["ESIEmpContribution"] = 0;
                                dr["ESICompContribution"] = 0;

                                dr["deduction"] = 0;
                                #endregion

                                dr["net"] = 0;
                                #endregion
                                
                                dtEmpSalary.Rows.Add(dr);
                                #endregion
                            }
                            else
                            {
                                //Regular Salary Processing
                                #region[RegularSalaryProcessing]
                                dr = dtEmpSalary.NewRow();
                                //From T_Attendence
                                #region[T_Attendence]

                                basicfrEsi = Convert.ToDouble(objDT.Rows[i]["stdBasic"]);
                                grossSal = Convert.ToDouble(objDT.Rows[i]["grossSal"]);

                                dr["empCode"] = objDT.Rows[i]["Employeecd"].ToString();
                                dr["empName"] = objDT.Rows[i]["Employeename"].ToString();
                                daysInMonth = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));

                                dr["daysInMonth"] = daysInMonth;
                                if (objDT.Rows[i]["Weeklyoff"] != DBNull.Value)
                                    dr["weeklyOff"] = weeklyOff = Convert.ToDouble(objDT.Rows[i]["Weeklyoff"]);
                                if (objDT.Rows[i]["Payholiday"] != DBNull.Value)
                                    dr["payholiday"] = payHolidays = Convert.ToDouble(objDT.Rows[i]["Payholiday"]);

                                dr["payDays"] = (daysInMonth - weeklyOff - payHolidays);

                                if (objDT.Rows[i]["PresentDay"] != DBNull.Value)
                                    dr["presentDays"] = presentDays = Convert.ToDouble(objDT.Rows[i]["PresentDay"]);
                                if (objDT.Rows[i]["PL"] != DBNull.Value)
                                    dr["PL"] = PL = Convert.ToDouble(objDT.Rows[i]["PL"]);
                                if (objDT.Rows[i]["COff"] != DBNull.Value)
                                    dr["cOff"] = COff = Convert.ToDouble(objDT.Rows[i]["COff"]);

                                if (objDT.Rows[i]["Absent"] != DBNull.Value)
                                    dr["AbsentDays"] = absentDays = Convert.ToDouble(objDT.Rows[i]["Absent"]);

                                dr["PaybleDays"] = Convert.ToDecimal(Convert.ToDecimal(presentDays) + Convert.ToDecimal(PL) + Convert.ToDecimal(COff));
                                #endregion

                                //M_Salary
                                #region[M_Salary]

                                // Earnings
                                #region[Earnings]

                                //if (objDT1.Rows[0]["Approval"] == DBNull.Value)
                                //{
                                //    clearControls();
                                //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Salary Not Approved'); ", true);
                                //    return;
                                //}
                                //if (objDT1.Rows[0]["Approval"].ToString() == "N")
                                //{
                                //    clearControls();
                                //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Salary Not Approved'); ", true);
                                //    return;
                                //}

                                grossSalary = standardBasic = Math.Round(Convert.ToDouble(objDT.Rows[i]["BasicDA"]));
                                dr["BasicDA"] = grossSalary = standardBasic;
                                basicForEsi = basicForEsi + Math.Round(Convert.ToDouble(objDT.Rows[i]["BasicDA"]));

                                standardHRA = Math.Round(Convert.ToDouble(objDT.Rows[i]["HRA"]));
                                grossSalary = grossSalary + standardHRA;
                                dr["HRA"] = standardHRA;
                                basicForEsi = basicForEsi + Math.Round(Convert.ToDouble(objDT.Rows[i]["HRA"]));

                                standConveyance = Math.Round(Convert.ToDouble(objDT.Rows[i]["Conveyance"]));
                                grossSalary = grossSalary + standConveyance;
                                dr["CA"] = standConveyance;

                                stadEducation = Math.Round(Convert.ToDouble(objDT.Rows[i]["Education"]));
                                grossSalary = grossSalary + stadEducation;
                                dr["education"] = stadEducation;
                                basicForEsi = basicForEsi + Math.Round(Convert.ToDouble(objDT.Rows[i]["Education"]));


                                standardMedical = Math.Round(Convert.ToDouble(objDT.Rows[i]["medical"]));
                                grossSalary = grossSalary + standardMedical;
                                dr["medical"] = standardMedical;
                                basicForEsi = basicForEsi + Math.Round(Convert.ToDouble(objDT.Rows[i]["medical"]));


                                stadCanteen = Math.Round(Convert.ToDouble(objDT.Rows[i]["Canteen"]));
                                grossSalary = grossSalary + stadCanteen;
                                dr["Canteen"] = stadCanteen;
                                basicForEsi = basicForEsi + Math.Round(Convert.ToDouble(objDT.Rows[i]["Canteen"])); // Newlly added for esi calculation on 1/10/2021

                                standardWashing = Math.Round(Convert.ToDouble(objDT.Rows[i]["Washing"]));
                                grossSalary = grossSalary + standardWashing;
                                dr["Washing"] = standardWashing;

                                stadUniform = Math.Round(Convert.ToDouble(objDT.Rows[i]["Uniform"]));
                                grossSalary = grossSalary + stadUniform;
                                dr["Uniform"] = stadUniform;
                                basicForEsi = basicForEsi + Math.Round(Convert.ToDouble(objDT.Rows[i]["Uniform"])); // Newlly added for esi calculation on 1/10/2021

                                add1 = Math.Round(Convert.ToDouble(objDT.Rows[i]["Add1"]));
                                grossSalary = grossSalary + add1;
                                dr["Add1"] = add1;
                                basicForEsi = basicForEsi + Math.Round(Convert.ToDouble(objDT.Rows[i]["Add1"])); // Newlly added for esi calculation on 10/03/2021

                                add2 = Math.Round(Convert.ToDouble(objDT.Rows[i]["Add2"]));
                                grossSalary = grossSalary + add2;
                                dr["Add2"] = add2;

                                add3 = Math.Round(Convert.ToDouble(objDT.Rows[i]["Add3"]));
                                grossSalary = grossSalary + add3;
                                dr["Add3"] = add3;

                                stadGross = Math.Round(Convert.ToDouble(objDT.Rows[i]["stadGross"]));

                                //if (objDT.Rows[i]["SNACKCOUNT"] != DBNull.Value && objDT.Rows[i]["CanteenRate"] != DBNull.Value)
                                //    insentive = Math.Round(((stadGross / 26 / 8) * Convert.ToDouble(objDT.Rows[i]["OT"])) + (Convert.ToDouble(objDT.Rows[i]["SNACKCOUNT"]) * Convert.ToDouble(objDT.Rows[i]["CanteenRate"])));

                               
                                insentive = stadGross / 26 / 8;
                                insentive = insentive * Convert.ToDouble(objDT.Rows[i]["OT"]);
                                insentive = Math.Round(insentive + (Convert.ToDouble(objDT.Rows[i]["SNACKCOUNT"]) * Convert.ToDouble(objDT.Rows[i]["CanteenRate"])));

                                grossSalary = grossSalary + insentive;
                                dr["insentive"] = Math.Round(Convert.ToDouble(insentive));

                                dr["gross"] = Math.Round(Convert.ToDouble(grossSalary));

                                //For ESI
                                basicForEsi = basicForEsi + insentive;

                                #endregion

                                //From T_Deduction
                                #region[Deduction]
                                #region[T_Deduction]
                                if (objDT.Rows[i]["Advance"] != DBNull.Value)
                                {
                                    dr["salaryAdvance"] = Math.Round(Convert.ToDouble(objDT.Rows[i]["Advance"]));
                                    deduction = deduction + Math.Round(Convert.ToDouble(objDT.Rows[i]["Advance"]));
                                    deductionImp = Math.Round(Convert.ToDouble(objDT.Rows[i]["Advance"]));
                                }
                                if (objDT.Rows[i]["Loan"] != DBNull.Value)
                                {
                                    dr["Loan"] = Math.Round(Convert.ToDouble(objDT.Rows[i]["Loan"]));
                                    deduction = deduction + Math.Round(Convert.ToDouble(objDT.Rows[i]["Loan"]));
                                    deductionImp += Math.Round(Convert.ToDouble(objDT.Rows[i]["Loan"]));
                                }
                                if (objDT.Rows[i]["TDS"] != DBNull.Value)
                                {
                                    dr["TDS"] = Math.Round(Convert.ToDouble(objDT.Rows[i]["TDS"]));
                                    deduction = deduction + Math.Round(Convert.ToDouble(objDT.Rows[i]["TDS"]));
                                    deductionImp += Math.Round(Convert.ToDouble(objDT.Rows[i]["TDS"]));
                                }
                                if (objDT.Rows[i]["TardalPathsansth"] != DBNull.Value)
                                {
                                    dr["TardalPathsansth"] = Math.Round(Convert.ToDouble(objDT.Rows[i]["TardalPathsansth"]));
                                    deduction = deduction + Math.Round(Convert.ToDouble(objDT.Rows[i]["TardalPathsansth"]));
                                    deductionImp += Math.Round(Convert.ToDouble(objDT.Rows[i]["TardalPathsansth"]));
                                }
                                if (objDT.Rows[i]["Ded1"] != DBNull.Value)
                                {
                                    dr["Ded1"] = Math.Round(Convert.ToDouble(objDT.Rows[i]["Ded1"]));
                                    deduction = deduction + Math.Round(Convert.ToDouble(objDT.Rows[i]["Ded1"]));
                                    deductionImp += (Convert.ToDouble(objDT.Rows[i]["Ded1"]));
                                }
                                #endregion

                                #region[PF Config]
                                //PF Calculation  
                                //strPfConfig = "select * from M_PFConfiguration WHERE (MonYrcd =(SELECT MAX(MonYrcd) AS Expr1 FROM dbo.M_PFConfiguration AS M_PFConfiguration1 WHERE(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) <='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "') AND(OrgId =" + Convert.ToInt32(Session["OrgID"]) + "))) AND(OrgId = " + Convert.ToInt32(Session["OrgID"]) + ") ";
                                if (objDT.Rows[i]["PFApplicable"] != DBNull.Value)
                                {
                                    if (objDT.Rows[i]["PFApplicable"].ToString() != "N" && Convert.ToDateTime(dt) >= Convert.ToDateTime(objDT.Rows[i]["PFJoindate"]))
                                    {
                                        strPfConfig = "select * from M_PFConfiguration WHERE RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) = (SELECT  max(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) as MonYrcd  FROM dbo.M_PFConfiguration WHERE RIGHT(MonYrcd, 4) +LEFT(MonYrcd, 2) <= '" + ddlYear.SelectedValue + ddlMon.SelectedValue + "' and orgid = " + Convert.ToInt32(Session["OrgID"]) + " ) AND(OrgId = " + Convert.ToInt32(Session["OrgID"]) + ")";
                                        objPFConfig = SqlHelper.ExecuteDataTable(strPfConfig, AppGlobal.strConnString);
                                        if (objPFConfig.Rows.Count > 0)
                                        {
                                            empAge = getAge(Convert.ToDateTime(objDT.Rows[i]["Birthdate"]));
                                            if (empAge <= Convert.ToInt32(objPFConfig.Rows[0]["PFAgeLimit"]))
                                            {
                                                epf = Convert.ToDouble(objPFConfig.Rows[0]["EPF"]);
                                                eps = Convert.ToDouble(objPFConfig.Rows[0]["EPS"]);

                                                if ((standardBasic > Convert.ToInt32(objPFConfig.Rows[0]["PFAmtLimit"])) && objDT.Rows[i]["PFApplicable"].ToString() != "A")
                                                {
                                                    pfAmount = Convert.ToInt32(objPFConfig.Rows[0]["PFAmtLimit"]) * Convert.ToDouble(objPFConfig.Rows[0]["EPF"]) / 100;
                                                    pfPension = Convert.ToInt32(objPFConfig.Rows[0]["PFAmtLimit"]) * Convert.ToDouble(objPFConfig.Rows[0]["EPS"]) / 100;
                                                }
                                                else
                                                {
                                                    pfAmount = standardBasic * Convert.ToDouble(objPFConfig.Rows[0]["EPF"]) / 100;
                                                    pfPension = standardBasic * Convert.ToDouble(objPFConfig.Rows[0]["EPS"]) / 100;
                                                }
                                               
                                            }
                                            else
                                            {
                                                if (standardBasic > Convert.ToInt32(objPFConfig.Rows[0]["PFAmtLimit"]))
                                                {
                                                    pfAmount = Convert.ToInt32(objPFConfig.Rows[0]["PFAmtLimit"]) * Convert.ToDouble(objPFConfig.Rows[0]["PF"]) / 100;
                                                }
                                                else
                                                {
                                                    pfAmount = standardBasic * Convert.ToDouble(objPFConfig.Rows[0]["PF"]) / 100;
                                                }
                                                    
                                            }
                                        }
                                    }
                                }

                                //dr["providendFund"] = Math.Round(pfAmount);
                                //dr["pfPension"] = Math.Round(pfPension);

                                deduction = deduction + Math.Round(pfAmount) + Math.Round(pfPension);
                                #endregion

                                #region[Prof Tax]
                                //Prof Tax Calculation
                                if (objDT.Rows[i]["ProfTaxApplicable"] != DBNull.Value)
                                {
                                    if (objDT.Rows[i]["ProfTaxApplicable"].ToString() == "Y")
                                    {

                                        strProfTaxConfig = "select * from M_ProfTaxConfiguration WHERE (RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) = (SELECT  max(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) AS Expr1 FROM dbo.M_ProfTaxConfiguration WHERE(RIGHT(MonYrcd, 4) +LEFT(MonYrcd, 2) <='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "') AND(OrgId =" + Convert.ToInt32(Session["OrgID"]) + ") and statecd= " + Convert.ToInt16(objDT.Rows[i]["WStatecd"]) + " and gendercd=" + Convert.ToInt16(objDT.Rows[i]["gendercd"]) + " )) AND(OrgId = " + Convert.ToInt32(Session["OrgID"]) + ") and statecd= " + Convert.ToInt16(objDT.Rows[i]["WStatecd"]) + " and gendercd=" + Convert.ToInt16(objDT.Rows[i]["gendercd"]);
                                        objProfTaxConfig = SqlHelper.ExecuteDataTable(strProfTaxConfig, AppGlobal.strConnString);
                                        if (objProfTaxConfig.Rows.Count > 0)
                                        {
                                            for (int j = 0; j < objProfTaxConfig.Rows.Count; j++)
                                            {
                                                if (objProfTaxConfig.Rows[j]["Mon"] != DBNull.Value)
                                                {
                                                    if (objProfTaxConfig.Rows[j]["Mon"].ToString() == ddlMon.SelectedValue)
                                                    {
                                                        monAmt = Convert.ToDouble(objProfTaxConfig.Rows[j]["MonAmount"]);
                                                    }
                                                }
                                                if (grossSalary >= Convert.ToDouble(objProfTaxConfig.Rows[j]["FrAmount"]) && grossSalary <= Convert.ToDouble(objProfTaxConfig.Rows[j]["ToAmount"]))
                                                {
                                                    profTaxAmt = monAmt + Convert.ToDouble(objProfTaxConfig.Rows[j]["TaxAmount"]);
                                                }
                                            }
                                        }
                                    }
                                }
                                //dr["professionalTax"] = Math.Round(profTaxAmt);
                                deduction = deduction + Math.Round(profTaxAmt);
                                #endregion

                                #region[Labour Walfare ]
                                //Labour Walfare Applicable
                                if (objDT.Rows[i]["lwf"] != DBNull.Value)
                                {
                                    if (objDT.Rows[i]["LabWelApplicable"] != DBNull.Value)
                                    {
                                        if (objDT.Rows[i]["LabWelApplicable"].ToString() == "Y")
                                        {
                                            //strLabWelFunConfig = "SELECT top(1) * FROM dbo.M_LwfConfiguration AS M_PFConfiguration1 WHERE orgid=" + Convert.ToInt32(Session["OrgID"]) + " and (convert(int, RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) <=201902) order by convert(int, RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) desc";
                                            //strLabWelFunConfig = "select* from M_LwfConfiguration WHERE RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) = (SELECT  max(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) as MonYrcd  FROM dbo.M_LwfConfiguration WHERE  LEFT(MonYrcd, 2) = " + ddlMon.SelectedValue + " and RIGHT(MonYrcd, 4) +LEFT(MonYrcd, 2) <= '" + ddlYear.SelectedValue + ddlMon.SelectedValue + "' and orgid =" + Convert.ToInt32(Session["OrgID"]) + ") AND(OrgId = " + Convert.ToInt32(Session["OrgID"]) + ")";
                                            strLabWelFunConfig = "SELECT* FROM M_LwfConfiguration WHERE(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) = (SELECT MAX(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) AS Expr1 FROM M_LwfConfiguration AS M_LwfConfiguration1 WHERE(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) <= '" + ddlYear.SelectedValue + ddlMon.SelectedValue + "') AND(OrgId = " + Convert.ToInt32(Session["OrgID"]) + "))) AND(OrgId = " + Convert.ToInt32(Session["OrgID"]) + ")";
                                            objLabWelFunConfig = SqlHelper.ExecuteDataTable(strLabWelFunConfig, AppGlobal.strConnString);
                                            if (objLabWelFunConfig.Rows.Count > 0)
                                            {
                                                if (objLabWelFunConfig.Rows[0]["lwf"] != DBNull.Value)
                                                {
                                                    lwf = Convert.ToDouble(objLabWelFunConfig.Rows[0]["lwf"]);
                                                }
                                                if (objLabWelFunConfig.Rows[0]["LWFCompContri"] != DBNull.Value)
                                                {
                                                    lwfCompContri = Convert.ToDouble(objLabWelFunConfig.Rows[0]["LWFCompContri"]);
                                                }
                                            }
                                        }
                                    }
                                }
                                //dr["lwf"] = Math.Round(lwf);
                                //dr["lwfCompContri"] = Math.Round(lwfCompContri);
                                deduction = deduction + Math.Round(lwf);
                                #endregion

                                #region[ESI]
                                //ESI Calculation
                                if (objDT.Rows[i]["ESIApplicable"] != DBNull.Value)
                                {
                                    string ESIApplicable = objDT.Rows[i]["ESIApplicable"].ToString();
                                    string ESINotApplicableDt = objDT.Rows[i]["ESINotApplicableDt"].ToString();
                                    string YrMonthCd = "";
                                    if (ESINotApplicableDt != "")
                                    {
                                        YrMonthCd = Convert.ToDateTime(ESINotApplicableDt).Year.ToString() + (Convert.ToDateTime(ESINotApplicableDt).Month.ToString().Length == 1 ? "0" + Convert.ToDateTime(ESINotApplicableDt).Month.ToString() : Convert.ToDateTime(ESINotApplicableDt).Month.ToString());
                                    }
                                    if (YrMonthCd != "")
                                    {
                                        int CurrYrMnthCd = Convert.ToInt32(ddlYear.SelectedValue + ddlMon.SelectedValue);
                                        int ESINotApplicableYrMnthCd = Convert.ToInt32(YrMonthCd);
                                        if (CurrYrMnthCd < ESINotApplicableYrMnthCd)
                                        {
                                            ESIApplicable = "Y";
                                        }
                                    }

                                    if (ESIApplicable == "Y")
                                    {
                                        if (objDT.Rows[i]["ESICalculate"] != DBNull.Value)
                                        {
                                            strESIConfig = "select top(1) * FROM dbo.M_ESIConfigure AS M_ESIConfigure1 WHERE (convert(int, RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) <=" + ddlYear.SelectedValue + ddlMon.SelectedValue + ") and orgid=" + Convert.ToInt32(Session["OrgID"]) + " order by convert(int, RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) desc";
                                            objESIConfig = SqlHelper.ExecuteDataTable(strESIConfig, AppGlobal.strConnString);
                                            if (objESIConfig.Rows.Count > 0)
                                            {
                                                if (objESIConfig.Rows[0]["amount"] != DBNull.Value)
                                                    esiConfigAmt = Convert.ToDouble(objESIConfig.Rows[0]["amount"]);
                                                if (objESIConfig.Rows[0]["ESIEmpPer"] != DBNull.Value)
                                                    esiEmpPer = Convert.ToDouble(objESIConfig.Rows[0]["ESIEmpPer"]);
                                                if (objESIConfig.Rows[0]["ESICompPer"] != DBNull.Value)
                                                    esiCompPer = Convert.ToDouble(objESIConfig.Rows[0]["ESICompPer"]);

                                                 //increament 21000
                                                //if (basicfrEsi <= esiConfigAmt)
                                                //{
                                                    lastDay = 0;
                                                    lastDay = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMon.SelectedValue));
                                                    salDate = ddlYear.SelectedValue + "/" + ddlMon.SelectedValue + "/" + lastDay;
                                                    //DateTime salDate = Convert.ToDateTime(dtSalaryDate);

                                                    strESIConfig1 = " select top(1) esi.*, emp.origjoindate,month(" + salDate + "),";
                                                    strESIConfig1 += " case ";
                                                    strESIConfig1 += " when " + salDate + "  < origjoindate then 'false'";
                                                    strESIConfig1 += " when (month(origjoindate) >=4 and month(origjoindate) <=9 and year(origjoindate) = year(" + salDate + ") and month(" + salDate + ")>=4 and month(" + salDate + ")<=9) ";
                                                    strESIConfig1 += " or (month(origjoindate) < 4 and month(" + salDate + ") < 4 and  year(origjoindate) = year(" + salDate + "))";
                                                    strESIConfig1 += " or (month(origjoindate) > 9 and ((year(origjoindate) = year(" + salDate + ") and  month(" + salDate + ")> 9) or (year(origjoindate) = year(" + salDate + ")-1 and  month(" + salDate + ")< 4)))";
                                                    strESIConfig1 += " then 'True' else 'flase'  end as esi, ";
                                                    strESIConfig1 += " case when ESICalculate = 1 then " + basicForEsi * esiEmpPer / 100 + "   else  " + grossSalary * esiEmpPer / 100 + "  end as esiEmpAmt , ";
                                                    strESIConfig1 += " case when ESICalculate = 1 then " + basicForEsi * esiCompPer / 100 + "   else  " + grossSalary * esiCompPer / 100 + "  end as esiCompAmt  ";
                                                    strESIConfig1 += " from M_Emp as emp left outer join M_ESIConfigure as esi on emp.orgid=esi.orgID where employeecd=" + objDT.Rows[i]["employeecd"].ToString() + " and (convert(int, RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) <='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "') and esi.orgid=" + Convert.ToInt16(Session["OrgID"]) + " order by convert(int, RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) desc";
                                                    objESIConfig1 = SqlHelper.ExecuteDataTable(strESIConfig1, AppGlobal.strConnString);
                                                    if (objESIConfig1.Rows.Count > 0)
                                                    {
                                                        //    if (Convert.ToBoolean(objESIConfig1.Rows[0]["esi"]) == true)
                                                        //    {
                                                        //        if(objDT.Rows[i]["ESICalculate"].ToString()=="1")
                                                        //            esiAmount = Math.Round((basicfrEsi) * esiEmpPer / 100);
                                                        //        else
                                                        //            esiAmount = Math.Round((basicfrEsi+standardHRA+stadEducation+standardMedical+insentive) * esiEmpPer / 100);
                                                        //    }
                                                        esiEmpAmount = Convert.ToDouble(objESIConfig1.Rows[0]["esiEmpAmt"]);
                                                        esiCompAmount = Convert.ToDouble(objESIConfig1.Rows[0]["esiCompAmt"]);
                                                    }
                                                //}
                                            }
                                        }
                                    }
                                }

                                //dr["esiEmpPer"] = Math.Round(esiEmpPer);
                                //dr["esiCompPer"] = Math.Round(esiCompPer);

                                //dr["ESIEmpContribution"] = Math.Round(esiEmpAmount);
                                //dr["ESICompContribution"] = Math.Round(esiCompAmount);

                                deduction = deduction + Math.Ceiling(esiEmpAmount);

                                #endregion


                                dr["providendFund"] = Math.Round(pfAmount);
                                dr["pfPension"] = Math.Round(pfPension);
                                dr["EPF"] = epf;
                                dr["EPS"] = eps;

                                dr["professionalTax"] = Math.Round(profTaxAmt);

                                dr["lwf"] = Math.Round(lwf);
                                dr["lwfCompContri"] = Math.Round(lwfCompContri);

                                dr["esiEmpPer"] = esiEmpPer;
                                dr["esiCompPer"] = esiCompPer;

                                dr["ESIEmpContribution"] = Math.Ceiling(esiEmpAmount);
                                dr["ESICompContribution"] = Math.Round(esiCompAmount);

                                dr["deduction"] = Math.Round(Convert.ToDouble(deduction));
                                #endregion

                                netSalary = Math.Round(Convert.ToDouble(grossSalary) - Convert.ToDouble(deduction));
                                dr["net"] = Math.Round(Convert.ToDouble(netSalary));
                                #endregion

                                //#region[Add NetSalary Less Than Below 50%]
                                //if ((grossSalary - deductionImp) < (grossSalary * 50 / 100))
                                //{
                                //    if ((grossSalary - deductionImp) < (grossSalary * 49 / 100))
                                //    {
                                //        drl = dtEmpLessSalary.NewRow();
                                //        drl["empCode"] = objDT.Rows[i]["Employeecd"].ToString();
                                //        drl["empName"] = objDT.Rows[i]["Employeename"].ToString();
                                //        drl["net"] = Convert.ToDouble((grossSalary - deductionImp));
                                //        dtEmpLessSalary.Rows.Add(drl);
                                //    }
                                //}

                                //Changed deduction like loading and unloading employees - 26/10/2020  
                                #region[Add NetSalary Less Than Below 50%]
                                if ((grossSalary - deduction) < (grossSalary * 50 / 100))
                                {
                                    if ((grossSalary - deduction) < (grossSalary * 49 / 100))
                                    {
                                        drl = dtEmpLessSalary.NewRow();
                                        drl["empCode"] = objDT.Rows[i]["Employeecd"].ToString();
                                        drl["empName"] = objDT.Rows[i]["Employeename"].ToString();
                                        drl["net"] = Convert.ToDouble((grossSalary - deduction));
                                        dtEmpLessSalary.Rows.Add(drl);
                                    }
                                }


                                #endregion
                                dtEmpSalary.Rows.Add(dr);
                                #endregion
                            }
                        }
                    }


                    #region[DisplaySalary]
                    if (opr == "1")
                    {
                        //txtEmpName.Text = dtEmpSalary.Rows[0]["empName"].ToString();
                        if (dtEmpSalary.Rows[0]["daysInMonth"] != DBNull.Value)
                            txtDaysInMonth.Text = dtEmpSalary.Rows[0]["daysInMonth"].ToString();
                        if (dtEmpSalary.Rows[0]["payDays"] != DBNull.Value)
                            txtPayDays.Text = dtEmpSalary.Rows[0]["payDays"].ToString();
                        if (dtEmpSalary.Rows[0]["presentDays"] != DBNull.Value)
                            txtPresentDays.Text = dtEmpSalary.Rows[0]["presentDays"].ToString();
                        if (dtEmpSalary.Rows[0]["PL"] != DBNull.Value)
                            txtPL.Text = dtEmpSalary.Rows[0]["PL"].ToString();
                        if (dtEmpSalary.Rows[0]["weeklyOff"] != DBNull.Value)
                            txtWeeklyOff.Text = dtEmpSalary.Rows[0]["weeklyOff"].ToString();
                        if (dtEmpSalary.Rows[0]["paybleDays"] != DBNull.Value)
                            txtPaybleDays.Text = dtEmpSalary.Rows[0]["paybleDays"].ToString();
                        if (dtEmpSalary.Rows[0]["payholiday"] != DBNull.Value)
                            txtPayHolidays.Text = dtEmpSalary.Rows[0]["payholiday"].ToString();
                        if (dtEmpSalary.Rows[0]["cOff"] != DBNull.Value)
                            txtCOff.Text = dtEmpSalary.Rows[0]["cOff"].ToString();
                        txtAbsentDays.Text = dtEmpSalary.Rows[0]["absentDays"].ToString();

                        if (dtEmpSalary.Rows[0]["basicDA"] != DBNull.Value)
                            txtBasicDA.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["basicDA"]).ToString("0.00");
                        if (dtEmpSalary.Rows[0]["HRA"] != DBNull.Value)
                            txtHRA.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["HRA"]).ToString("0.00");
                        if (dtEmpSalary.Rows[0]["CA"] != DBNull.Value)
                            txtConveyance.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["CA"]).ToString("0.00");
                        if (dtEmpSalary.Rows[0]["education"] != DBNull.Value)
                            txtEducation.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["education"]).ToString("0.00");
                        if (dtEmpSalary.Rows[0]["medical"] != DBNull.Value)
                            txtMedical.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["medical"]).ToString("0.00");
                        if (dtEmpSalary.Rows[0]["canteen"] != DBNull.Value)
                            txtCanteen.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["canteen"]).ToString("0.00");
                        if (dtEmpSalary.Rows[0]["washing"] != DBNull.Value)
                            txtWashing.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["washing"]).ToString("0.00");
                        if (dtEmpSalary.Rows[0]["uniform"] != DBNull.Value)
                            txtUniform.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["uniform"]).ToString("0.00");

                        if (dtEmpSalary.Rows[0]["Add1"] != DBNull.Value)
                            txtAdd1.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["Add1"]).ToString("0.00");
                        if (dtEmpSalary.Rows[0]["Add2"] != DBNull.Value)
                            txtAdd2.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["Add2"]).ToString("0.00");
                        if (dtEmpSalary.Rows[0]["Add3"] != DBNull.Value)
                            txtAdd3.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["Add3"]).ToString("0.00");

                        if (dtEmpSalary.Rows[0]["insentive"] != DBNull.Value)
                            txtIsentive.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["insentive"]).ToString("0.00");

                        if (dtEmpSalary.Rows[0]["salaryAdvance"] != DBNull.Value)
                            txtAdvance.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["salaryAdvance"]).ToString("0.00");
                        if (dtEmpSalary.Rows[0]["providendFund"] != DBNull.Value)
                            txtPf.Text =Convert.ToDouble(Convert.ToDouble(dtEmpSalary.Rows[0]["pfPension"])+ Convert.ToDouble(dtEmpSalary.Rows[0]["providendFund"])).ToString("0.00");

                        //txtPf.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["EPF"]).ToString("0.00");
                        //txtEducation.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["EPS"]).ToString("0.00");
                        if (dtEmpSalary.Rows[0]["TDS"] != DBNull.Value)
                            txtTDS.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["TDS"]).ToString("0.00");
                        if (dtEmpSalary.Rows[0]["professionalTax"] != DBNull.Value)
                            txtProfessionalTax.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["professionalTax"]).ToString("0.00");
                        if (dtEmpSalary.Rows[0]["ESIEmpContribution"] != DBNull.Value)
                            txtESI.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["ESIEmpContribution"]).ToString("0.00");
                        if (dtEmpSalary.Rows[0]["LWF"] != DBNull.Value)
                            txtLWF.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["LWF"]).ToString("0.00");
                        if (dtEmpSalary.Rows[0]["loan"] != DBNull.Value)
                            txtLoan.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["loan"]).ToString("0.00");
                        if (dtEmpSalary.Rows[0]["TardalPathsansth"] != DBNull.Value)
                            txtPathSanstha.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["TardalPathsansth"]).ToString("0.00");
                        if (dtEmpSalary.Rows[0]["Ded1"] != DBNull.Value)
                            txtEntertainmentCost.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["Ded1"]).ToString("0.00");

                        txtGross.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["gross"]).ToString("0.00");
                        txtDeduction.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["deduction"]).ToString("0.00");
                        if (dtEmpLessSalary.Rows.Count > 0)
                        {
                            txtNetAmount.BackColor = System.Drawing.Color.Red;
                            txtNetAmount.ForeColor = System.Drawing.Color.White;
                        }
                        txtNetAmount.Text = Convert.ToDouble(dtEmpSalary.Rows[0]["net"]).ToString("0.00");
                        if (dtEmpLessSalary.Rows.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Net Salary Is Below 50% Than Gross Salary'); ", true);
                            return;
                        }
                    }
                    #endregion

                    #region[NetSalary Less Than Below 50% List]
                    if (dtEmpLessSalary.Rows.Count > 0)
                    {
                        gvList.DataSource = dtEmpLessSalary;
                        gvList.DataBind();
                        pnlGVList.Visible = true;
                        pnlSalaryData.Visible = false;
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Net Salary Is Below 50% Than Gross Salary'); ", true);
                        return;
                    }
                    #endregion

                    #region[SalaryProcessing]
                    if (opr == "2")
                    {
                        if (dtEmpSalary.Rows.Count > 0)
                        {   
                            strQrySalary = "delete from T_MonthlySalary where OrgId=@OrgID and MonYrcd=@MonYrcd";
                            SqlParameter[] para1 = new SqlParameter[2];
                            para1[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                            para1[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + ddlYear.SelectedValue);

                            SqlHelper.ExecuteTransaction(sqlCmd, strQrySalary, para1);

                            for (int i = 0; i < dtEmpSalary.Rows.Count; i++)
                            {
                                //strQrySalary = "delete from T_MonthlySalary where OrgId=@OrgID and MonYrcd=@MonYrcd and Employeecd=@Employeecd";
                                //SqlParameter[] para2 = new SqlParameter[3];
                                //para2[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                                //para2[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + ddlYear.SelectedValue);
                                //para2[2] = new SqlParameter("@Employeecd", dtEmpSalary.Rows[i]["empCode"].ToString());

                                //SqlHelper.ExecuteTransaction(sqlCmd, strQrySalary, para2);

                                strQrySalary = @"INSERT INTO T_MonthlySalary(OrgId, MonYrcd, Docdate,Employeecd,MonthDay,PayDay,AbsentDay,PL,PresentDay,Weeklyoff,Holiday,Coff,PayableDay,
                                BasicDA,HRA,Conveyance,Education,Medical,Canteen,Washing,Uniform, Add1,Add2, Add3,Incentive,EPF,EPS,
                                Advance,Loan,TDS,TardalPathsasnth,ProfTax,Provfund,pfPension, lwf,LwfCompContri,esiEmp, esiComp,ESIEmpContribution,ESICompContribution, Ded1, Gross,Deduction,NetAmount) 
                                VALUES(@OrgId, @MonYrcd, @Docdate,@Employeecd,@MonthDay,@PayDay,@AbsentDay,@PL,@PresentDay,@Weeklyoff,@payholiday,@Coff,@PayableDay,
                                @BasicDA,@HRA,@Conveyance,@Education,@Medical,@Canteen,@Washing,@Uniform,  @Add1,@Add2, @Add3,@Incentive,@EPF,@EPS,
                                @Advance,@Loan,@TDS,@TardalPathsasnth,@ProfTax,@Provfund, @pfPension, @lwf,@LwfCompContri, @esiEmp, @esiComp,@ESIEmpContribution,@ESICompContribution, @Ded1, @Gross,@Deduction,@NetAmount)";

                                SqlParameter[] para = new SqlParameter[44];
                                para[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgId"]));
                                para[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + ddlYear.SelectedValue);
                                para[2] = new SqlParameter("@Docdate", Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy"));
                                para[3] = new SqlParameter("@Employeecd", dtEmpSalary.Rows[i]["empCode"].ToString());
                                para[4] = new SqlParameter("@MonthDay", dtEmpSalary.Rows[i]["daysInMonth"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["daysInMonth"]));
                                para[5] = new SqlParameter("@PayDay", dtEmpSalary.Rows[i]["payDays"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["payDays"]));
                                para[6] = new SqlParameter("@PresentDay", dtEmpSalary.Rows[i]["presentDays"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["presentDays"]));
                                para[7] = new SqlParameter("@AbsentDay", dtEmpSalary.Rows[i]["absentDays"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["absentDays"]));
                                para[8] = new SqlParameter("@PL", dtEmpSalary.Rows[i]["PL"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["PL"]));
                                para[9] = new SqlParameter("@Weeklyoff", dtEmpSalary.Rows[i]["weeklyOff"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["weeklyOff"]));
                                para[10] = new SqlParameter("@PayableDay", dtEmpSalary.Rows[i]["paybleDays"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["paybleDays"]));
                                para[11] = new SqlParameter("@Coff", dtEmpSalary.Rows[i]["cOff"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["cOff"]));

                                para[12] = new SqlParameter("@BasicDA", Convert.ToDouble(dtEmpSalary.Rows[i]["basicDA"]));
                                para[13] = new SqlParameter("@HRA", dtEmpSalary.Rows[i]["HRA"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["HRA"]));
                                para[14] = new SqlParameter("@Conveyance", dtEmpSalary.Rows[i]["CA"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["CA"]));
                                para[15] = new SqlParameter("@Education", dtEmpSalary.Rows[i]["education"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["education"]));
                                para[16] = new SqlParameter("@Medical", dtEmpSalary.Rows[i]["medical"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["medical"]));
                                para[17] = new SqlParameter("@Canteen", dtEmpSalary.Rows[i]["canteen"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["canteen"]));
                                para[18] = new SqlParameter("@Washing", dtEmpSalary.Rows[i]["washing"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["washing"]));
                                para[19] = new SqlParameter("@Uniform", dtEmpSalary.Rows[i]["uniform"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["uniform"]));
                                para[20] = new SqlParameter("@Incentive", dtEmpSalary.Rows[i]["insentive"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["insentive"]));

                                para[21] = new SqlParameter("@Advance", dtEmpSalary.Rows[i]["salaryAdvance"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["salaryAdvance"]));
                                para[22] = new SqlParameter("@Provfund", dtEmpSalary.Rows[i]["providendFund"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["providendFund"]));
                                para[23] = new SqlParameter("@TDS", dtEmpSalary.Rows[i]["TDS"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["TDS"]));
                                para[24] = new SqlParameter("@ProfTax", dtEmpSalary.Rows[i]["professionalTax"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["professionalTax"]));
                                para[26] = new SqlParameter("@loan", dtEmpSalary.Rows[i]["loan"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["loan"]));
                                para[27] = new SqlParameter("@TardalPathsasnth", dtEmpSalary.Rows[i]["TardalPathsansth"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["TardalPathsansth"]));

                                para[25] = new SqlParameter("@Gross", dtEmpSalary.Rows[i]["gross"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["gross"]));
                                para[28] = new SqlParameter("@Deduction", dtEmpSalary.Rows[i]["deduction"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["deduction"]));
                                para[29] = new SqlParameter("@NetAmount", dtEmpSalary.Rows[i]["net"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["net"]));

                                para[30] = new SqlParameter("@EPF", dtEmpSalary.Rows[i]["EPF"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["EPF"]));
                                para[31] = new SqlParameter("@EPS", dtEmpSalary.Rows[i]["EPS"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["EPS"]));

                                para[32] = new SqlParameter("@payholiday", dtEmpSalary.Rows[i]["payholiday"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["payholiday"]));
                                para[33] = new SqlParameter("@lwf", dtEmpSalary.Rows[i]["lwf"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["lwf"]));
                                para[34] = new SqlParameter("@esiEmp", dtEmpSalary.Rows[i]["esiEmpPer"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["esiEmpPer"]));
                                para[35] = new SqlParameter("@esiComp", dtEmpSalary.Rows[i]["esiCompPer"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["esiCompPer"]));
                                para[36] = new SqlParameter("@ESIEmpContribution", dtEmpSalary.Rows[i]["ESIEmpContribution"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["ESIEmpContribution"]));
                                para[37] = new SqlParameter("@ESICompContribution", dtEmpSalary.Rows[i]["ESICompContribution"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["ESICompContribution"]));

                                para[38] = new SqlParameter("@Add1", dtEmpSalary.Rows[i]["Add1"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["Add1"]));
                                para[39] = new SqlParameter("@Add2", dtEmpSalary.Rows[i]["Add2"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["Add2"]));
                                para[40] = new SqlParameter("@Add3", dtEmpSalary.Rows[i]["Add3"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["Add3"]));
                                para[41] = new SqlParameter("@lwfCompContri", dtEmpSalary.Rows[i]["lwfCompContri"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["lwfCompContri"]));
                                para[42] = new SqlParameter("@pfPension", dtEmpSalary.Rows[i]["pfPension"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["pfPension"]));
                                para[43] = new SqlParameter("@Ded1", dtEmpSalary.Rows[i]["Ded1"] == DBNull.Value ? 0 : Convert.ToDouble(dtEmpSalary.Rows[i]["Ded1"]));
                                //result = SqlHelper.ExecuteNonQuery(strQrySalary, para, AppGlobal.strConnString);
                                result = SqlHelper.ExecuteTransaction(sqlCmd, strQrySalary, para);
                            }
                            if (result)
                            {
                                strQry = "";
                                //Lock Processed Salary
                                #region[Lock Processed Salary]
                                strQry = "select * from T_SalaryLock where orgID=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrCd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                DataTable objDTLock = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                                if (objDTLock.Rows.Count > 0)
                                {
                                    strQry = "update T_SalaryLock set Lock='Y' where OrgId=@OrgID and MonYrCd=@MonYrCd";
                                }
                                else
                                {
                                    strQry = "INSERT INTO T_SalaryLock(OrgId, MonYrCd, Lock) VALUES(@OrgId, @MonYrCd, @Lock)";
                                }

                                SqlParameter[] paraLock = new SqlParameter[3];
                                paraLock[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                                paraLock[1] = new SqlParameter("@MonYrCd", ddlMon.SelectedValue + ddlYear.SelectedValue);
                                paraLock[2] = new SqlParameter("@Lock", "Y");

                                result = SqlHelper.ExecuteNonQuery(strQry, paraLock, AppGlobal.strConnString);
                                #endregion
                                if (result)
                                {
                                    // Log Entry
                                    strQry = "";
                                    strQry = "INSERT INTO T_Log(OrgId,MonthYrcd, Employeecd, MenuId, Mode, Computername) VALUES(@OrgId,@MonthYrcd, @Employeecd, @MenuId, @Mode, @Computername)";

                                    SqlParameter[] paraLog = new SqlParameter[6];
                                    paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                                    paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                                    paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                                    paraLog[3] = new SqlParameter("@Mode", "A");
                                    paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());
                                    paraLog[5] = new SqlParameter("@MonthYrcd", ddlMon.SelectedValue + ddlYear.SelectedValue);
                                    result = SqlHelper.ExecuteNonQuery(strQry, paraLog, AppGlobal.strConnString);
                                    if (result)
                                    {
                                        sqlTrans.Commit();
                                        clearControls();
                                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
            }
        }
        protected void btnGetData_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtEmpCode.Text != "" && ddlMon.SelectedIndex != 0 && ddlYear.SelectedIndex != 0)
                {
                    salaryProcess(ddlMon.SelectedValue, ddlYear.SelectedValue, txtEmpCode.Text, "1");
                    pnlSalaryData.Visible = true;
                    btnSave.Visible = false;
                }
                else
                {
                    clearControls();
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Month/Year and Enter Employee Code'); ", true);
                }
            }
            catch (Exception ex)
            {

            }
        }

        //protected int getAge(DateTime birthDate)
        //{
        //    int lastDay = 0, Curage = 0, d,m;
        //    lastDay = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMon.SelectedValue));
        //    string dtSalaryDate = ddlYear.SelectedValue + "/" + ddlMon.SelectedValue + "/" + lastDay;
        //    DateTime salDate = Convert.ToDateTime(dtSalaryDate);
        //    //int age = Convert.ToDateTime(dtSalaryDate).Year - Convert.ToDateTime(birthDate).Year;

        //    DateTime age = new DateTime(salDate.Subtract(birthDate).Ticks);
        //    Curage = age.Year;
        //    d = 365 - age.DayOfYear;
        //    if(Curage == 58 && d>0)
        //    {
        //        Curage += 1;
        //    }
        //    //if(Curage >= 58)
        //    //{
        //    //    if (age.Month >= 1 || age.Day >= 1)
        //    //        Curage = Curage + 1;
        //    //}

        //    return Curage;
        //}
        static int getAge(DateTime Dob)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1;
            DateTime PastYearDate = Dob.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
            int Hours = Now.Subtract(PastYearDate).Hours;
            int Minutes = Now.Subtract(PastYearDate).Minutes;
            int Seconds = Now.Subtract(PastYearDate).Seconds;
            if (Years == 58 && (Months > 0 || Days > 0))
            {
                Years += 1;
            }

            //return String.Format("Age: {0} Year(s) {1} Month(s) {2} Day(s) {3} Hour(s) {4} Second(s)",
            //Years, Months, Days, Hours, Seconds);
            return Years;
        }

        protected bool getPreviousESI(string month, string empCode)
        {
            bool prvEsi = false;
            string strESI = "select * from T_MonthlySalary where LEFT(MonYrcd, 2)='" + month + "' and Employeecd='" + empCode + "'";
            DataTable objESI = SqlHelper.ExecuteDataTable(strESI, AppGlobal.strConnString);
            if (objESI.Rows.Count > 0)
            {
                prvEsi = true;
            }
            return prvEsi;
        }

        protected string getPreviousMonth(string currMonth)
        {
            string month = "";
            month = (Convert.ToInt16(currMonth) - 1).ToString("00");
            if (month == "00")
                month = "12";

            return month;
        }

        protected void ddlEmpName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEmpName.SelectedIndex != 0)
            {
                txtEmpCode.Text = ddlEmpName.SelectedValue.ToString();
            }
        }
    }
}