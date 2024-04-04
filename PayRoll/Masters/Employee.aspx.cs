
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
    public partial class Employee : System.Web.UI.Page
    {
        string empName = "";
        SqlConnection sqlConn = null;
        SqlCommand sqlCmd = null;
        SqlTransaction sqlTrans = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                clearControls();
            }
        }
       
        private void BindData()
        {
            string strQry = "";
            DataTable objDT;

            strQry = "SELECT State, StateCd FROM M_State ORDER BY State";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlPState.DataSource = objDT;
            ddlPState.DataTextField = "State";
            ddlPState.DataValueField = "StateCd";
            ddlPState.DataBind();
           
            ddlPState.Items.Insert(0, new ListItem("Select", "0"));

            ddlWState.DataSource = objDT;
            ddlWState.DataTextField = "State";
            ddlWState.DataValueField = "StateCd";
            ddlWState.DataBind();
           
            ddlWState.Items.Insert(0, new ListItem("Select", "0"));
           

            strQry = "SELECT BankName, BankCd FROM M_Bank ORDER BY BankName";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlBank.DataSource = objDT;
            ddlBank.DataTextField = "BankName";
            ddlBank.DataValueField = "BankCd";
            ddlBank.DataBind();

            ddlBank.Items.Insert(0, new ListItem("Select", "0"));
           

            strQry = "SELECT BankBranch, BankBranchId FROM M_BankBranch ORDER BY BankBranch";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlBankBranch.DataSource = objDT;
            ddlBankBranch.DataTextField = "BankBranch";
            ddlBankBranch.DataValueField = "BankBranchId";
            ddlBankBranch.DataBind();
            
            ddlBankBranch.Items.Insert(0, new ListItem("Select", "0"));

            strQry = "SELECT Gender, Gendercd FROM M_Gender ORDER BY Gendercd";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlGender.DataSource = objDT;
            ddlGender.DataTextField = "Gender";
            ddlGender.DataValueField = "Gendercd";
            ddlGender.DataBind();

            strQry = "SELECT RoleId, RoleName FROM M_Role ORDER BY SeqNo";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlRole.DataSource = objDT;
            ddlRole.DataTextField = "RoleName";
            ddlRole.DataValueField = "RoleId";
            ddlRole.DataBind();

            strQry = "SELECT Name, castID FROM M_Cast ORDER BY Name";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlCast.DataSource = objDT;
            ddlCast.DataTextField = "Name";
            ddlCast.DataValueField = "castID";
            ddlCast.DataBind();

            ddlCast.Items.Insert(0, new ListItem("Select", "0"));
            
        }

        private void BindData1()
        {
            string strQry = "";
            DataTable objDT;

            //strQry = "SELECT Country, CountryCd FROM M_Country ORDER BY Country";
            //objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            //ddlCountry.DataSource = objDT;
            //ddlCountry.DataTextField = "Country";
            //ddlCountry.DataValueField = "CountryCd";
            //ddlCountry.DataBind();

            strQry = "SELECT State, StateCd FROM M_State ORDER BY State";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlPState.DataSource = objDT;
            ddlPState.DataTextField = "State";
            ddlPState.DataValueField = "StateCd";
            ddlPState.DataBind();

            ddlPState.Items.Insert(0, new ListItem("Select", "0"));

            ddlWState.DataSource = objDT;
            ddlWState.DataTextField = "State";
            ddlWState.DataValueField = "StateCd";
            ddlWState.DataBind();

            ddlWState.Items.Insert(0, new ListItem("Select", "0"));

            strQry = "SELECT District, DistCd FROM M_District ORDER BY District";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlPDist.DataSource = objDT;
            ddlPDist.DataTextField = "District";
            ddlPDist.DataValueField = "DistCd";
            ddlPDist.DataBind();

            ddlPDist.Items.Insert(0, new ListItem("Select", "0"));

            ddlWDist.DataSource = objDT;
            ddlWDist.DataTextField = "District";
            ddlWDist.DataValueField = "DistCd";
            ddlWDist.DataBind();

            ddlWDist.Items.Insert(0, new ListItem("Select", "0"));

            strQry = "SELECT Taluka, TalCd FROM M_Taluka ORDER BY Taluka";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlPTaluka.DataSource = objDT;
            ddlPTaluka.DataTextField = "Taluka";
            ddlPTaluka.DataValueField = "TalCd";
            ddlPTaluka.DataBind();

            ddlPTaluka.Items.Insert(0, new ListItem("Select", "0"));

            ddlWTaluka.DataSource = objDT;
            ddlWTaluka.DataTextField = "Taluka";
            ddlWTaluka.DataValueField = "TalCd";
            ddlWTaluka.DataBind();

            ddlWTaluka.Items.Insert(0, new ListItem("Select", "0"));

            strQry = "SELECT City, Citycd FROM M_City ORDER BY City";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlPCity.DataSource = objDT;
            ddlPCity.DataTextField = "City";
            ddlPCity.DataValueField = "Citycd";
            ddlPCity.DataBind();

            ddlPCity.Items.Insert(0, new ListItem("Select", "0"));

            ddlWCity.DataSource = objDT;
            ddlWCity.DataTextField = "City";
            ddlWCity.DataValueField = "Citycd";
            ddlWCity.DataBind();

            ddlWCity.Items.Insert(0, new ListItem("Select", "0"));

            strQry = "SELECT    RegId, Regname FROM M_Regional";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlRegional.DataSource = objDT;
            ddlRegional.DataTextField = "Regname";
            ddlRegional.DataValueField = "RegId";
            ddlRegional.DataBind();

            ddlRegional.Items.Insert(0, new ListItem("Select", "0"));
            ddlSubRegional.Items.Insert(0, new ListItem("Select", "0"));

            strQry = "SELECT DiIncode, Description FROM M_DI";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlDI.DataSource = objDT;
            ddlDI.DataTextField = "Description";
            ddlDI.DataValueField = "DiIncode";
            ddlDI.DataBind();
            ddlDI.Items.Insert(0, new ListItem("Select", "0"));

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    if (formValidation())
                    {
                        if (HFEmployee.Value == "0" || HFEmployee.Value == "3")
                        {
                            InsertRecord();
                        }
                        else if (HFEmployee.Value == "1")
                        {
                            UpdateRecord();
                            btnSave.Text = "Save";
                        }
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

        private void InsertRecord()
        {
            /*PF Dropdown Values

              Applicable                        Value = "Y"
              Not Applicable                    Value = "N" 
              PNRPY                             Value = "P"
           
            */

            /*
            if ESI Applicable
            value="1"       Basic+DA+HRA+Medical+Education+Insentiv
            value="2"       Gross
            */

            string strQry = "";
            bool result = false;
            try
            {
                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);

                strQry = @"INSERT INTO M_Emp(OrgId, Employeecd, CommCodeAdhno,FName,MName,LName,Employeename,Gendercd,BloodGroup,Marital,Qual,PrevExper,UserRolecd, " +
                     " PAddre,pPincode, PCitycd, PTalcd, PDistCd, PStateCd, WAddre,wPincode, WCitycd, WTalcd, WDistCd, WStateCd ," +
                     " MobNo, EmailId, Birthdate, Password, OrigJoindate,DatofJoin,HODInchAppl,PANNo,UANNO,PFApplicable,PFJoindate,PFNo," +
                     " HRAApplicable,ProfTaxApplicable,LabWelApplicable,ESIApplicable,ESIIP,ESICalculate,BankCd, BankActNo, BankBranchId,Gratuitydate,DrivingLicence,RegignationDate, " +
                     " Leavedate,RasonLeavingjob,LIC_Id,LIC_PolicyNo, IsActive, castID,Regid, SubRegid,DiIncode, ESINotApplicableDt, LINNo) " +

                     " VALUES(@OrgId, @Employeecd, @CommCodeAdhno, @FName, @MName, @LName, @Employeename, @Gendercd, @BloodGroup, @Marital,@Qual,@PrevExper,@UserRolecd, " +
                     " @PAddre,@pPincode, @PCitycd, @PTalcd, @PDistCd,@PStateCd, @WAddre,@wPincode, @WCitycd, @WTalcd, @WDistCd, @WStateCd, " +
                     " @MobNo, @EmailId, @Birthdate,@Password, @OrigJoindate,@DatofJoin,@HODInchAppl,@PANNo,@UANNO,@PFApplicable,@PFJoindate,@PFNo," +
                     " @HRAApplicable,@ProfTaxApplicable,@LabWelApplicable,@ESIApplicable,@ESIIP,@ESICalculate, @BankCd,@BankActNo, @BankBranchId,@Gratuitydate,@DrivingLicence,@RegignationDate, " +
                     " @Leavedate,@RasonLeavingjob,@LIC_Id, @LIC_PolicyNo, @IsActive, @castID,@Regid, @SubRegid,@DiIncode, @ESINotApplicableDt, @LINNo)";


                SqlParameter[] para = new SqlParameter[60];
                para[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                para[1] = new SqlParameter("@Employeecd", txtEmpCode.Text.Trim());
                para[2] = new SqlParameter("@CommCodeAdhno", txtAadharNo.Text.Trim());
                para[3] = new SqlParameter("@FName", txtFName.Text);
                para[4] = new SqlParameter("@MName", txtMName.Text);
                para[5] = new SqlParameter("@LName", txtLName.Text);

                string empName = char.ToUpper(txtFName.Text[0]) + txtFName.Text.Substring(1);
                empName = empName + " " + char.ToUpper(txtMName.Text[0]) + txtMName.Text.Substring(1);
                empName = empName + " " + char.ToUpper(txtLName.Text[0]) + txtLName.Text.Substring(1);

                para[6] = new SqlParameter("@Employeename", empName);
                para[7] = new SqlParameter("@Gendercd", ddlGender.SelectedValue);

                para[8] = new SqlParameter("@PAddre", txtPAddress.Text);
                para[9] = new SqlParameter("@PCitycd", ddlPCity.SelectedValue != "" ? Convert.ToInt16(ddlPCity.SelectedValue) : 0);
                para[10] = new SqlParameter("@PTalcd", ddlPTaluka.SelectedValue != "" ? Convert.ToInt16(ddlPTaluka.SelectedValue) : 0);
                para[11] = new SqlParameter("@PDistCd", ddlPDist.SelectedValue != "" ? Convert.ToInt16(ddlPDist.SelectedValue) : 0);
                para[12] = new SqlParameter("@PStateCd", ddlPState.SelectedValue != "" ? Convert.ToInt16(ddlPState.SelectedValue) : 0);
                para[13] = new SqlParameter("@WAddre", txtWAddress.Text);
                para[14] = new SqlParameter("@WCitycd", ddlWCity.SelectedValue != "" ? Convert.ToInt16(ddlWCity.SelectedValue) : 0);
                para[15] = new SqlParameter("@WTalcd", ddlWTaluka.SelectedValue != "" ? Convert.ToInt16(ddlWTaluka.SelectedValue) : 0);
                para[16] = new SqlParameter("@WDistCd", ddlWDist.SelectedValue != "" ? Convert.ToInt16(ddlWDist.SelectedValue) : 0);
                para[17] = new SqlParameter("@WStateCd", ddlWState.SelectedValue != "" ? Convert.ToInt16(ddlWState.SelectedValue) : 0);

                para[18] = new SqlParameter("@MobNo", txtMobileNo.Text);
                para[19] = new SqlParameter("@EmailId", txtEmailID.Text);
                para[20] = new SqlParameter("@Birthdate", txtBirthdate.Text.Trim() == "" ? (object)DBNull.Value : Convert.ToDateTime(txtBirthdate.Text).ToString("dd MMM yyyy"));
                para[21] = new SqlParameter("@OrigJoindate", txtDOJ.Text.Trim() == "" ? (object)DBNull.Value : Convert.ToDateTime(txtDOJ.Text.Trim()).ToString("dd MMM yyyy"));
                para[22] = new SqlParameter("@HODInchAppl", ddlHODApplicable.SelectedValue);// chkHODApplicable.Checked ? "Y" : "N"
                para[23] = new SqlParameter("@PANNo", txtpanNo.Text.ToUpper());

                if (ddlPFApplicable.SelectedValue == "N")
                {
                    para[25] = new SqlParameter("@PFApplicable", ddlPFApplicable.SelectedValue);
                    para[26] = new SqlParameter("@PFJoindate", SqlDateTime.Null);
                    para[27] = new SqlParameter("@PFNo", "");
                    para[24] = new SqlParameter("@UANNO", "");
                }
                else
                {
                    para[25] = new SqlParameter("@PFApplicable", ddlPFApplicable.SelectedValue);
                    para[26] = new SqlParameter("@PFJoindate", txtPFJoindDt.Text.Trim() == "" ? (object)DBNull.Value : Convert.ToDateTime(txtPFJoindDt.Text.Trim()).ToString("dd MMM yyyy"));
                    para[27] = new SqlParameter("@PFNo", txtPFNo.Text);
                    para[24] = new SqlParameter("@UANNO", txtUANNo.Text);
                }


                para[28] = new SqlParameter("@HRAApplicable", ddlHRAApplicable.SelectedValue);
                para[29] = new SqlParameter("@ProfTaxApplicable", ddlProfessionalTax.SelectedValue);
                para[30] = new SqlParameter("@LabWelApplicable", ddlLabourWalfare.SelectedValue);
                if (ddlESI.SelectedValue == "N")
                {
                    para[31] = new SqlParameter("@ESIApplicable", ddlESI.SelectedValue);
                    para[32] = new SqlParameter("@ESIIP", "");
                    para[37] = new SqlParameter("@ESICalculate", "");
                }
                else
                {
                    para[31] = new SqlParameter("@ESIApplicable", ddlESI.SelectedValue);
                    para[32] = new SqlParameter("@ESIIP", txtESINo.Text);
                    para[37] = new SqlParameter("@ESICalculate", rblESICal.SelectedValue);
                }
                para[33] = new SqlParameter("@BankCd", ddlBank.SelectedValue);
                para[34] = new SqlParameter("@BankActNo", txtBankAcNo.Text);
                para[35] = new SqlParameter("@BankBranchId", ddlBankBranch.SelectedValue);

                para[36] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");

                para[38] = new SqlParameter("@BloodGroup", ddlBloodGroup.SelectedValue);
                para[39] = new SqlParameter("@Marital", ddlMaritalStatus.SelectedValue);

                para[40] = new SqlParameter("@Qual", txtQualification.Text);
                para[41] = new SqlParameter("@PrevExper", txtPreviousExp.Text);
                para[42] = new SqlParameter("@UserRolecd", ddlRole.SelectedValue);

                para[43] = new SqlParameter("@pPincode", txtPPinCode.Text);
                para[44] = new SqlParameter("@wPincode", txtWPinCode.Text);
                string passward = "";
                if(txtBirthdate.Text!="")
                {
                    int day = Convert.ToDateTime(txtBirthdate.Text).Day;
                    int month= Convert.ToDateTime(txtBirthdate.Text).Month;
                    int year= Convert.ToDateTime(txtBirthdate.Text).Year;
                    string y = year.ToString();
                    passward = Convert.ToInt16(day).ToString("00") + Convert.ToInt16(month).ToString("00") + y.Substring(y.Length - 2) + txtEmpCode.Text;

                }
                para[45] = new SqlParameter("@Password", passward);
                para[46] = new SqlParameter("@Gratuitydate", txtGratuityDate.Text.Trim() == "" ? (object)DBNull.Value : Convert.ToDateTime(txtGratuityDate.Text.Trim()).ToString("dd MMM yyyy"));
                para[47] = new SqlParameter("@DrivingLicence", txtDrivingLicence.Text);
                para[48] = new SqlParameter("@Leavedate", txtLeaveDate.Text.Trim() == "" ? (object)DBNull.Value : Convert.ToDateTime(txtLeaveDate.Text.Trim()).ToString("dd MMM yyyy"));
                para[49] = new SqlParameter("@RasonLeavingjob", ddlLeaveReason.SelectedValue);
                para[50] = new SqlParameter("@DatofJoin", txtJoinTrf1.Text.Trim() == "" ? (object)DBNull.Value : Convert.ToDateTime(txtJoinTrf1.Text.Trim()).ToString("dd MMM yyyy"));
                para[51] = new SqlParameter("@LIC_Id", txtLICID.Text);
                para[52] = new SqlParameter("@LIC_PolicyNo", txtLICPolicyNo.Text != "" ? Convert.ToInt64(txtLICPolicyNo.Text) : 0);
                para[53] = new SqlParameter("@RegignationDate", txtResignation.Text.Trim() == "" ? (object)DBNull.Value : Convert.ToDateTime(txtResignation.Text.Trim()).ToString("dd MMM yyyy"));
                para[54] = new SqlParameter("@castID", ddlCast.SelectedValue);
                para[55] = new SqlParameter("@Regid", ddlRegional.SelectedValue != "" ? Convert.ToInt16(ddlRegional.SelectedValue) : 0);
                para[56] = new SqlParameter("@SubRegid", ddlSubRegional.SelectedValue != "" ? Convert.ToInt16(ddlSubRegional.SelectedValue) : 0);
                para[57] = new SqlParameter("@DiIncode", ddlDI.SelectedValue != "" ? Convert.ToInt16(ddlDI.SelectedValue) : 0);
                para[58] = new SqlParameter("@ESINotApplicableDt", txtESIEffectDt.Text.Trim() == "" ? (object)DBNull.Value : Convert.ToDateTime(txtESIEffectDt.Text).ToString("dd MMM yyyy"));
                para[59] = new SqlParameter("@LINNo", txtLINNo.Text);

                //result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                //-----Check Employee already exists in all organisation-23/11/2020-------------
                string chkdup = "select Employeecd from M_Emp where Employeecd=" + txtEmpCode.Text.Trim();
                DataTable objchkdup = SqlHelper.ExecuteDataTable(chkdup, AppGlobal.strConnString);
                if (objchkdup.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Employee code already exists!'); ", true);
                    return;
                }
                else
                {
                    result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                }
                //--------------------------------------------------------------------------------

                if (result)
                {
                    // Employee Transfer
                    if(HFEmployee.Value=="3")
                    {
                        string strQryLeave = "SELECT OrgId,Employeecd,DatofJoin FROM M_Emp where CommCodeAdhno=" + txtAadharNo.Text + " and DatofJoin<'" + Convert.ToDateTime(txtJoinTrf1.Text).ToString("dd MMM yyyy") + "' ORDER BY DatofJoin desc";
                        DataTable objDTLeave = SqlHelper.ExecuteDataTable(strQryLeave, AppGlobal.strConnString);
                        if(objDTLeave.Rows.Count>0)
                        {
                            double leave = 0;
                            double prvPresenty = 0;
                            string year="";
                            
                            year = Convert.ToDateTime(txtJoinTrf1.Text).ToString("yyyy");

                            string strQryLeave1 = "SELECT dbo.T_LeaveBalance.PL as PL, SUM(dbo.T_Attendance.PresentDay) AS prvPresenty FROM dbo.T_Attendance INNER JOIN dbo.T_LeaveBalance ON dbo.T_Attendance.OrgId = dbo.T_LeaveBalance.OrgId AND dbo.T_Attendance.Employeecd = dbo.T_LeaveBalance.Employeecd ";
                            strQryLeave1 += " WHERE(dbo.T_Attendance.OrgId = " + Convert.ToInt16(objDTLeave.Rows[0]["OrgId"])  + ") AND(dbo.T_Attendance.Employeecd = " + objDTLeave.Rows[0]["Employeecd"].ToString() + ") AND(RIGHT(dbo.T_Attendance.MonYrcd, 4) >= "+ year+") AND (LEFT(dbo.T_Attendance.MonYrcd, 2) >= '01') GROUP BY dbo.T_Attendance.Employeecd, dbo.T_LeaveBalance.PL"; 
                            DataTable objDTLeave1 = SqlHelper.ExecuteDataTable(strQryLeave1, AppGlobal.strConnString);
                            if (objDTLeave1.Rows.Count > 0)
                            {
                                leave = Convert.ToDouble(objDTLeave1.Rows[0]["PL"]);
                                prvPresenty = Convert.ToDouble(objDTLeave1.Rows[0]["prvPresenty"]);
                            }
                            year = "01" + Convert.ToDateTime(DateTime.Now).ToString("yyyy");
                            strQry = "INSERT INTO T_LeaveBalance(OrgId,MonYrCd, Employeecd, PL,prvPresenty) VALUES(@OrgId,@MonYrCd, @Employeecd, @PL, @prvPresenty)";

                            SqlParameter[] paraLeave = new SqlParameter[5];
                            paraLeave[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                            paraLeave[1] = new SqlParameter("@Employeecd", txtEmpCode.Text);
                            paraLeave[2] = new SqlParameter("@MonYrCd", year);
                            paraLeave[3] = new SqlParameter("@PL", leave);
                            paraLeave[4] = new SqlParameter("@prvPresenty", prvPresenty);

                            result = SqlHelper.ExecuteNonQuery(strQry, paraLeave, AppGlobal.strConnString);

                            strQry = "";
                            strQry = "INSERT INTO T_Log(OrgId, Employeecd, MenuId, Mode, Computername) VALUES(@OrgId, @Employeecd, @MenuId, @Mode, @Computername)";

                            SqlParameter[] paraLog = new SqlParameter[5];
                            paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                            paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                            paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                            paraLog[3] = new SqlParameter("@Mode", "A");
                            paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());

                            result = SqlHelper.ExecuteNonQuery(strQry, paraLog, AppGlobal.strConnString);
                            if (result)
                            {
                                sqlTrans.Commit();
                                clearControls();
                                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
                            }
                        }
                    }
                    else
                    {
                        strQry = "";
                        strQry = "INSERT INTO T_Log(OrgId, Employeecd, MenuId, Mode, Computername) VALUES(@OrgId, @Employeecd, @MenuId, @Mode, @Computername)";

                        SqlParameter[] paraLog = new SqlParameter[5];
                        paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                        paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                        paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                        paraLog[3] = new SqlParameter("@Mode", "A");
                        paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());

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
            catch(Exception ex)
            {
                sqlTrans.Rollback();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
            }

        }

        private void UpdateRecord()
        {
            string strQry = "";
            bool result = false;
            int nId = Convert.ToInt32(ViewState["ID"]);

            //strQry = "select * from T_SalaryLock where OrgId=" + Convert.ToInt16(Session["orgID"]) + " and RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)>='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "' and Lock='Y'";
            //DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            //if (objDT.Rows.Count > 0)
            //{
            //    clearControls();
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Salary Already Processed, Cant Modify Now.'); ", true);
            //    return;
            //}

            try
            {
                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);
                strQry = @"UPDATE M_Emp SET CommCodeAdhno=@CommCodeAdhno, FName=@FName,MName=@MName,LName=@LName,Employeename=@Employeename,Gendercd=@Gendercd,BloodGroup=@BloodGroup,Marital=@Marital,Qual=@Qual,PrevExper=@PrevExper,
                            PAddre=@PAddre,pPincode=@pPincode, PCitycd=@PCitycd, PTalcd=@PTalcd, PDistCd=@PDistCd, PStateCd=@PStateCd, WAddre=@WAddre,wPincode=@wPincode, WCitycd=@WCitycd, WTalcd=@WTalcd, WDistCd=@WDistCd, WStateCd=@WStateCd,
                            MobNo=@MobNo, EmailId=@EmailId, Birthdate=@Birthdate, password=@password, OrigJoindate=@OrigJoindate, DatofJoin=@DatofJoin, HODInchAppl=@HODInchAppl,PANNo=@PANNo,UANNO=@UANNO,PFApplicable=@PFApplicable,PFJoindate=@PFJoindate,PFNo=@PFNo,
                            HRAApplicable=@HRAApplicable,ProfTaxApplicable=@ProfTaxApplicable,LabWelApplicable=@LabWelApplicable,ESIApplicable=@ESIApplicable,ESIIP=@ESIIP, ESICalculate=@ESICalculate, BankCd=@BankCd, BankActNo=@BankActNo, BankBranchId=@BankBranchId,
                            Gratuitydate=@Gratuitydate, DrivingLicence=@DrivingLicence,RegignationDate=@RegignationDate, Leavedate=@Leavedate,RasonLeavingjob=@RasonLeavingjob,IsActive=@IsActive,UserRolecd=@UserRolecd,LIC_Id=@LIC_Id,LIC_PolicyNo=@LIC_PolicyNo, 
                            castID=@castID,Regid=@Regid, SubRegid=@SubRegid,DiIncode=@DiIncode, ESINotApplicableDt=@ESINotApplicableDt, LINNo=@LINNo  
                       WHERE OrgId=@OrgId and Employeecd=@Employeecd";

                SqlParameter[] para = new SqlParameter[60];
                para[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                para[1] = new SqlParameter("@Employeecd", txtEmpCode.Text.Trim());
                para[2] = new SqlParameter("@CommCodeAdhno", txtAadharNo.Text.Trim());
                para[3] = new SqlParameter("@FName", txtFName.Text);
                para[4] = new SqlParameter("@MName", txtMName.Text);
                para[5] = new SqlParameter("@LName", txtLName.Text);

                string empName = char.ToUpper(txtFName.Text[0]) + txtFName.Text.Substring(1);
                empName = empName + " " + char.ToUpper(txtMName.Text[0]) + txtMName.Text.Substring(1);
                empName = empName + " " + char.ToUpper(txtLName.Text[0]) + txtLName.Text.Substring(1);

                para[6] = new SqlParameter("@Employeename", empName);
                para[7] = new SqlParameter("@Gendercd", ddlGender.SelectedValue);

                para[8] = new SqlParameter("@PAddre", txtPAddress.Text);
                para[9] = new SqlParameter("@PCitycd", ddlPCity.SelectedValue);
                para[10] = new SqlParameter("@PTalcd", ddlPTaluka.SelectedValue);
                para[11] = new SqlParameter("@PDistCd", ddlPDist.SelectedValue);
                para[12] = new SqlParameter("@PStateCd", ddlPState.SelectedValue);
                para[13] = new SqlParameter("@WAddre", txtWAddress.Text);
                para[14] = new SqlParameter("@WCitycd", ddlWCity.SelectedValue);
                para[15] = new SqlParameter("@WTalcd", ddlWTaluka.SelectedValue);
                para[16] = new SqlParameter("@WDistCd", ddlWDist.SelectedValue);
                para[17] = new SqlParameter("@WStateCd", ddlWState.SelectedValue);

                para[18] = new SqlParameter("@MobNo", txtMobileNo.Text);
                para[19] = new SqlParameter("@EmailId", txtEmailID.Text);
                para[20] = new SqlParameter("@Birthdate", txtBirthdate.Text.Trim() == "" ? (object)DBNull.Value : Convert.ToDateTime(txtBirthdate.Text.Trim()).ToString("dd MMM yyyy"));
                para[21] = new SqlParameter("@OrigJoindate", txtDOJ.Text.Trim() == "" ? (object)DBNull.Value : Convert.ToDateTime(txtDOJ.Text.Trim()).ToString("dd MMM yyyy"));
                para[22] = new SqlParameter("@HODInchAppl", ddlHODApplicable.SelectedValue); // chkHODApplicable.Checked == true ? "Y" : "N"
                para[23] = new SqlParameter("@PANNo", txtpanNo.Text.ToUpper());
                if (ddlPFApplicable.SelectedValue == "N")
                {
                    para[25] = new SqlParameter("@PFApplicable", ddlPFApplicable.SelectedValue);
                    para[26] = new SqlParameter("@PFJoindate", SqlDateTime.Null);
                    para[27] = new SqlParameter("@PFNo", "");
                    para[24] = new SqlParameter("@UANNO", "");
                }
                else
                {
                    para[25] = new SqlParameter("@PFApplicable", ddlPFApplicable.SelectedValue);
                    para[26] = new SqlParameter("@PFJoindate", txtPFJoindDt.Text.Trim() == "" ? (object)DBNull.Value : Convert.ToDateTime(txtPFJoindDt.Text.Trim()).ToString("dd MMM yyyy"));
                    para[27] = new SqlParameter("@PFNo", txtPFNo.Text);
                    para[24] = new SqlParameter("@UANNO", txtUANNo.Text);
                }

                para[28] = new SqlParameter("@HRAApplicable", ddlHRAApplicable.SelectedValue);
                para[29] = new SqlParameter("@ProfTaxApplicable", ddlProfessionalTax.SelectedValue);
                para[30] = new SqlParameter("@LabWelApplicable", ddlLabourWalfare.SelectedValue);
                if (ddlESI.SelectedValue == "N")
                {
                    para[31] = new SqlParameter("@ESIApplicable", ddlESI.SelectedValue);
                    para[32] = new SqlParameter("@ESIIP", "");
                    para[37] = new SqlParameter("@ESICalculate", "");
                }
                else
                {
                    para[31] = new SqlParameter("@ESIApplicable", ddlESI.SelectedValue);
                    para[32] = new SqlParameter("@ESIIP", txtESINo.Text);
                    para[37] = new SqlParameter("@ESICalculate", rblESICal.SelectedValue);
                }
                para[33] = new SqlParameter("@BankCd", ddlBank.SelectedValue);
                para[34] = new SqlParameter("@BankActNo", txtBankAcNo.Text);
                para[35] = new SqlParameter("@BankBranchId", ddlBankBranch.SelectedValue);

                para[36] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");

                para[38] = new SqlParameter("@BloodGroup", ddlBloodGroup.SelectedValue);
                para[39] = new SqlParameter("@Marital", ddlMaritalStatus.SelectedValue);

                para[40] = new SqlParameter("@Qual", txtQualification.Text);
                para[41] = new SqlParameter("@PrevExper", txtPreviousExp.Text);
                para[42] = new SqlParameter("@UserRolecd", ddlRole.SelectedValue);
                para[43] = new SqlParameter("@pPincode", txtPPinCode.Text);
                para[44] = new SqlParameter("@wPincode", txtWPinCode.Text);

                string strQry1 = "SELECT Password FROM M_EMP WHERE OrgId=" + Convert.ToInt32(Session["OrgID"]) + " AND Employeecd='" + txtEmpCode.Text.Trim() + "'";
                string passward = SqlHelper.ExecuteScalar(strQry1, AppGlobal.strConnString).ToString();

                if (txtBirthdate.Text != "" && passward == "")
                {
                    int day = Convert.ToDateTime(txtBirthdate.Text).Day;
                    int month = Convert.ToDateTime(txtBirthdate.Text).Month;
                    int year = Convert.ToDateTime(txtBirthdate.Text).Year;
                    string y = year.ToString();
                    passward = Convert.ToInt16(day).ToString("00") + Convert.ToInt16(month).ToString("00") + y.Substring(y.Length - 2) + txtEmpCode.Text;

                }
                para[45] = new SqlParameter("@Password", passward);
                para[46] = new SqlParameter("@Gratuitydate", txtGratuityDate.Text.Trim() == "" ? (object)DBNull.Value : Convert.ToDateTime(txtGratuityDate.Text.Trim()).ToString("dd MMM yyyy"));
                para[47] = new SqlParameter("@DrivingLicence", txtDrivingLicence.Text);
                para[48] = new SqlParameter("@Leavedate", txtLeaveDate.Text.Trim() == "" ? (object)DBNull.Value : Convert.ToDateTime(txtLeaveDate.Text.Trim()).ToString("dd MMM yyyy"));
                para[49] = new SqlParameter("@RasonLeavingjob", ddlLeaveReason.SelectedValue);
                para[50] = new SqlParameter("@DatofJoin", txtJoinTrf1.Text.Trim() == "" ? (object)DBNull.Value : Convert.ToDateTime(txtJoinTrf1.Text.Trim()).ToString("dd MMM yyyy"));
                para[51] = new SqlParameter("@LIC_Id", txtLICID.Text);
                para[52] = new SqlParameter("@LIC_PolicyNo", txtLICPolicyNo.Text !=""?Convert.ToInt64(txtLICPolicyNo.Text):0);
                para[53] = new SqlParameter("@RegignationDate", txtResignation.Text.Trim() == "" ? (object)DBNull.Value : Convert.ToDateTime(txtResignation.Text.Trim()).ToString("dd MMM yyyy"));
                para[54] = new SqlParameter("@castID", ddlCast.SelectedValue);
                para[55] = new SqlParameter("@Regid", ddlRegional.SelectedValue);
                para[56] = new SqlParameter("@SubRegid", ddlSubRegional.SelectedValue);
                para[57] = new SqlParameter("@DiIncode", ddlDI.SelectedValue);
                para[58] = new SqlParameter("@ESINotApplicableDt", txtESIEffectDt.Text.Trim() == "" ? (object)DBNull.Value : Convert.ToDateTime(txtESIEffectDt.Text).ToString("dd MMM yyyy"));
                para[59] = new SqlParameter("@LINNo", txtLINNo.Text);

                result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

                if (result)
                {
                    strQry = "";
                    strQry = "INSERT INTO T_Log(OrgId, Employeecd, MenuId, Mode, Computername) VALUES(@OrgId, @Employeecd, @MenuId, @Mode, @Computername)";

                    SqlParameter[] paraLog = new SqlParameter[5];
                    paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                    paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                    paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                    paraLog[3] = new SqlParameter("@Mode", "U");
                    paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());

                    result = SqlHelper.ExecuteNonQuery(strQry, paraLog, AppGlobal.strConnString);
                    if (result)
                    {
                        sqlTrans.Commit();
                        clearControls();
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
                    }
                    clearControls();

                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
            }
        }

        private void clearControls()
        {
            BindData();
            BindData1();
            RFVtxtJoinTrf1.Enabled = false;
            HFEmployee.Value = "0";

            txtEmpCode.Text = "";
            txtAadharNo.Text = "";
            txtFName.Text = "";
            txtMName.Text = "";
            txtLName.Text = "";
            txtEmpName.Text = "";
            txtBirthdate.Text = "";
            ddlGender.SelectedIndex = 0;
            ddlBloodGroup.SelectedIndex = 0;
            ddlMaritalStatus.SelectedIndex = 0;
           
            txtQualification.Text = "";
            txtPreviousExp.Text = "";

            txtMobileNo.Text = "";
            txtEmailID.Text = "";
            txtDOJ.Text = "";

            txtDOJ.ReadOnly = false;
            CEtxtDOJ.Enabled = true;

            txtJoinTrf1.Text = "";
            txtGratuityDate.Text = "";
            txtPAddress.Text = "";
            ddlPState.SelectedIndex = 0;
            ddlPDist.SelectedIndex = 0;
            ddlPTaluka.SelectedIndex = 0;
            ddlPCity.SelectedIndex = 0;
            txtPPinCode.Text = "";
            txtWAddress.Text = "";
            ddlWState.SelectedIndex = 0;
            ddlWDist.SelectedIndex = 0;
            ddlWTaluka.SelectedIndex = 0;
            ddlWCity.SelectedIndex = 0;
            txtWPinCode.Text = "";
            ddlBank.SelectedIndex = 0;
            ddlBankBranch.SelectedIndex = 0;
            txtBankAcNo.Text = "";
            txtpanNo.Text = "";
            ddlPFApplicable.SelectedIndex = 0;
            txtPFJoindDt.ReadOnly = false;
            txtPFNo.ReadOnly = false;
            txtUANNo.ReadOnly = false;
            txtPFNo.Text = "";
            txtPFJoindDt.Text = "";
            txtUANNo.Text = "";
            ddlHRAApplicable.SelectedIndex = 0;
            ddlProfessionalTax.SelectedIndex = 0;
            ddlLabourWalfare.SelectedIndex = 0;
            ddlESI.SelectedIndex = 0;
            ddlESI_SelectedIndexChanged(null,null);
            txtESIEffectDt.Text = "";
            txtESINo.ReadOnly = false;
            txtESINo.Text = "";
            rblESICal.ClearSelection();
            rblESICal.SelectedIndex = -1;
            ddlHODApplicable.SelectedIndex = 0;
            chkIsActive.Checked = true;
            txtDrivingLicence.Text = "";
            txtLeaveDate.Text = "";
            ddlLeaveReason.SelectedIndex = 0;
            txtLICID.Text = "";
            txtLICPolicyNo.Text = "";
            txtResignation.Text = "";
            txtDesignation.Text = "";

            ddlRole.SelectedIndex = 0;
            ddlCast.SelectedIndex = 0;

            btnSave.Text = "Save";
            btnUpdate.Visible = true;
            btnGetData.Visible = true;
            HFEmployee.Value = "0";
            txtEmpCode.ReadOnly = false;
            txtEmpCode.Focus();
            ddlRegional.SelectedIndex =  0;
            ddlRegional_SelectedIndexChanged(null, null);
            ddlDI.SelectedIndex = 0;
        }

        private void ViewRecord()
        {
            string strQry = "SELECT * FROM M_Emp Where OrgId='" + Convert.ToInt32(Session["OrgID"]) + "' and Employeecd='" + txtEmpCode.Text + "'";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (objDT.Rows.Count > 0)
            {

                string strQry1 = "";
                strQry1 = "SELECT dbo.M_Designation.Designation FROM dbo.M_Designation RIGHT OUTER JOIN dbo.M_Emp AS M_Emp_1 LEFT OUTER JOIN";
                strQry1 += " dbo.udfEmpConfigurationmax1(" + Convert.ToInt32(Session["OrgID"]) + ", '" + Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy") + "', 'desg') AS desg1 ON M_Emp_1.OrgId = desg1.OrgId AND M_Emp_1.Employeecd = desg1.Employeecd ON dbo.M_Designation.Desigcd = desg1.conId";
                strQry1 += " WHERE(M_Emp_1.OrgId = " + Convert.ToInt32(Session["OrgID"]) + ") AND(M_Emp_1.Employeecd = '"+ txtEmpCode.Text +"')";
                DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
                if(objDT1.Rows.Count>0)
                {
                    txtDesignation.Text = objDT1.Rows[0]["Designation"].ToString();
                }
                BindData();
                BindData1();
                txtAadharNo.Text = objDT.Rows[0]["CommCodeAdhno"].ToString();
                txtFName.Text = objDT.Rows[0]["FName"].ToString();
                txtMName.Text = objDT.Rows[0]["MName"].ToString();
                txtLName.Text = objDT.Rows[0]["LName"].ToString();
                txtEmpName.Text = objDT.Rows[0]["Employeename"].ToString();

                txtBirthdate.Text = objDT.Rows[0]["Birthdate"] != DBNull.Value ? Convert.ToDateTime(objDT.Rows[0]["Birthdate"]).ToString("dd/MM/yyyy") : "";
                if (objDT.Rows[0]["Gendercd"] != DBNull.Value)
                    ddlGender.SelectedValue = objDT.Rows[0]["Gendercd"].ToString();

                if (objDT.Rows[0]["BloodGroup"] != DBNull.Value)
                    ddlBloodGroup.SelectedValue = objDT.Rows[0]["BloodGroup"].ToString();

                if (objDT.Rows[0]["Marital"] != DBNull.Value)
                    ddlMaritalStatus.SelectedValue = objDT.Rows[0]["Marital"].ToString();

                txtQualification.Text = objDT.Rows[0]["Qual"].ToString();
                txtPreviousExp.Text = objDT.Rows[0]["PrevExper"].ToString();

                txtMobileNo.Text = objDT.Rows[0]["MobNo"].ToString();
                txtEmailID.Text = objDT.Rows[0]["EmailId"].ToString();
                
                txtDOJ.Text = objDT.Rows[0]["OrigJoindate"] != DBNull.Value ? Convert.ToDateTime(objDT.Rows[0]["OrigJoindate"]).ToString("dd/MM/yyyy") : "";
                if (Session["RoleId"].ToString() == "1")
                {
                    txtDOJ.ReadOnly = false;
                    CEtxtDOJ.Enabled = true;
                }
                else
                {
                    txtDOJ.ReadOnly = true;
                    CEtxtDOJ.Enabled = false;
                }
                txtJoinTrf1.Text = objDT.Rows[0]["DatofJoin"] != DBNull.Value ? Convert.ToDateTime(objDT.Rows[0]["DatofJoin"]).ToString("dd/MM/yyyy") : "";

                txtGratuityDate.Text = objDT.Rows[0]["Gratuitydate"] != DBNull.Value ? Convert.ToDateTime(objDT.Rows[0]["Gratuitydate"]).ToString("dd/MM/yyyy") : "";

                txtPAddress.Text = objDT.Rows[0]["PAddre"].ToString();
                txtPPinCode.Text = objDT.Rows[0]["pPincode"].ToString();

                if (objDT.Rows[0]["PStatecd"]!=DBNull.Value)
                    ddlPState.SelectedValue = objDT.Rows[0]["PStatecd"].ToString();
                if(objDT.Rows[0]["PDistcd"]!=DBNull.Value)
                    ddlPDist.SelectedValue = objDT.Rows[0]["PDistcd"].ToString();
                if(objDT.Rows[0]["PTalcd"]!=DBNull.Value)
                    ddlPTaluka.SelectedValue = objDT.Rows[0]["PTalcd"].ToString();
                if(objDT.Rows[0]["PCitycd"]!=DBNull.Value)
                    ddlPCity.SelectedValue = objDT.Rows[0]["PCitycd"].ToString();

                txtWAddress.Text = objDT.Rows[0]["WAddre"].ToString();
                txtWPinCode.Text = objDT.Rows[0]["wPincode"].ToString();
                if (objDT.Rows[0]["WStatecd"]!=DBNull.Value)
                    ddlWState.SelectedValue = objDT.Rows[0]["WStatecd"].ToString();
                if(objDT.Rows[0]["WDistcd"]!=DBNull.Value)
                    ddlWDist.SelectedValue = objDT.Rows[0]["WDistcd"].ToString();
                if(objDT.Rows[0]["WTalcd"]!=DBNull.Value)
                    ddlWTaluka.SelectedValue = objDT.Rows[0]["WTalcd"].ToString();
                if(objDT.Rows[0]["WCitycd"]!=DBNull.Value)
                    ddlWCity.SelectedValue = objDT.Rows[0]["WCitycd"].ToString();

                if(objDT.Rows[0]["BankCd"]!=DBNull.Value)
                    ddlBank.SelectedValue = objDT.Rows[0]["BankCd"].ToString();
                if(objDT.Rows[0]["BankBranchId"]!=DBNull.Value)
                    ddlBankBranch.SelectedValue = objDT.Rows[0]["BankBranchId"].ToString();
                txtBankAcNo.Text = objDT.Rows[0]["BankActNo"].ToString();
                txtpanNo.Text = objDT.Rows[0]["PANNo"].ToString();

                if(objDT.Rows[0]["PFApplicable"]!=DBNull.Value)
                    ddlPFApplicable.SelectedValue = objDT.Rows[0]["PFApplicable"].ToString();
                txtPFJoindDt.Text = objDT.Rows[0]["PFJoindate"] != DBNull.Value ? Convert.ToDateTime(objDT.Rows[0]["PFJoindate"]).ToString("dd/MM/yyyy") : "";
                txtPFNo.Text = objDT.Rows[0]["PFNo"].ToString();
                txtUANNo.Text = objDT.Rows[0]["UANNO"].ToString();
                if(objDT.Rows[0]["PFApplicable"].ToString() == "N")
                {
                    txtPFJoindDt.ReadOnly = true;
                    txtPFNo.ReadOnly = true;
                    txtUANNo.ReadOnly = true;
                }
                else
                {
                    txtPFJoindDt.ReadOnly = false;
                    txtPFNo.ReadOnly = false;
                    txtUANNo.ReadOnly = false;
                }

                if(objDT.Rows[0]["HRAApplicable"]!=DBNull.Value)
                    ddlHRAApplicable.SelectedValue = objDT.Rows[0]["HRAApplicable"].ToString();
                if(objDT.Rows[0]["ProfTaxApplicable"]!=DBNull.Value)
                    ddlProfessionalTax.SelectedValue = objDT.Rows[0]["ProfTaxApplicable"].ToString();
                if(objDT.Rows[0]["LabWelApplicable"]!=DBNull.Value)
                    ddlLabourWalfare.SelectedValue = objDT.Rows[0]["LabWelApplicable"].ToString();

                txtLINNo.Text = objDT.Rows[0]["LINNo"].ToString();
                ddlLabourWalfare_SelectedIndexChanged(null, null);
                

                if (objDT.Rows[0]["ESIApplicable"] != DBNull.Value)
                    ddlESI.SelectedValue = objDT.Rows[0]["ESIApplicable"].ToString();
                else
                    ddlESI.SelectedValue = "N";

                //ddlESI_SelectedIndexChanged(null, null);

                txtESIEffectDt.Text = objDT.Rows[0]["ESINotApplicableDt"] != DBNull.Value ? Convert.ToDateTime(objDT.Rows[0]["ESINotApplicableDt"]).ToString("dd/MM/yyyy") : "";

                txtESINo.Text = objDT.Rows[0]["ESIIP"].ToString();
                if (objDT.Rows[0]["ESIApplicable"].ToString() == "N")
                {
                    txtESINo.ReadOnly = true;
                    //rblESICal.Enabled = false;

                    //ddlRegional.Enabled = false;
                    //ddlSubRegional.Enabled = false;                    
                    //ddlRegional.SelectedIndex = 0;
                    //ddlSubRegional.Items.Clear();
                    //ddlSubRegional.Items.Insert(0, new ListItem("Select", "0"));
                }
                else
                {
                    txtESINo.ReadOnly = false;
                    //rblESICal.Enabled = true;
                    //ddlRegional.Enabled = true;
                    //ddlSubRegional.Enabled = true;

                    if (objDT.Rows[0]["ESIIP"] != DBNull.Value)
                        txtESINo.Text = objDT.Rows[0]["ESIIP"].ToString();
                    if (objDT.Rows[0]["ESICalculate"] != DBNull.Value)
                        rblESICal.SelectedValue = objDT.Rows[0]["ESICalculate"].ToString();
                }

                ddlESI_SelectedIndexChanged(null, null);


                txtDrivingLicence.Text = objDT.Rows[0]["DrivingLicence"].ToString();

                chkIsActive.Checked = objDT.Rows[0]["IsActive"].ToString() == "Y" ? true : false;
                txtLeaveDate.Text = objDT.Rows[0]["Leavedate"] != DBNull.Value ? Convert.ToDateTime(objDT.Rows[0]["Leavedate"]).ToString("dd/MM/yyyy") : "";
                txtResignation.Text = objDT.Rows[0]["RegignationDate"] != DBNull.Value ? Convert.ToDateTime(objDT.Rows[0]["RegignationDate"]).ToString("dd/MM/yyyy") : "";
                ddlLeaveReason.SelectedValue = objDT.Rows[0]["RasonLeavingjob"].ToString();
                if (objDT.Rows[0]["LIC_Id"] != DBNull.Value)
                    txtLICID.Text = objDT.Rows[0]["LIC_Id"].ToString();
                if (objDT.Rows[0]["LIC_PolicyNo"] != DBNull.Value)
                    txtLICPolicyNo.Text = objDT.Rows[0]["LIC_PolicyNo"].ToString();

                //chkHODApplicable.Checked = objDT.Rows[0]["HODInchAppl"].ToString() == "Y" ? true : false;
                if (objDT.Rows[0]["HODInchAppl"]!=DBNull.Value)
                    ddlHODApplicable.SelectedValue = objDT.Rows[0]["HODInchAppl"].ToString();
                if (objDT.Rows[0]["UserRolecd"] != DBNull.Value)
                    ddlRole.SelectedValue = objDT.Rows[0]["UserRolecd"].ToString();
                if(objDT.Rows[0]["castID"]!=DBNull.Value)
                    ddlCast.SelectedValue = objDT.Rows[0]["castID"].ToString();
                if (objDT.Rows[0]["Regid"] != DBNull.Value)
                    ddlRegional.SelectedValue = objDT.Rows[0]["Regid"].ToString();
                ddlRegional_SelectedIndexChanged(null, null);
                if (objDT.Rows[0]["SubRegid"] != DBNull.Value)
                    ddlSubRegional.SelectedValue = objDT.Rows[0]["SubRegid"].ToString();
                if (objDT.Rows[0]["DiIncode"] != DBNull.Value)
                    ddlDI.SelectedValue = objDT.Rows[0]["DiIncode"].ToString();
                txtEmpCode.ReadOnly = true;
                txtAadharNo.Focus();
            }
        }

        protected void txtFName_TextChanged(object sender, EventArgs e)
        {
            if (txtFName.Text != string.Empty)
            {
                setEmployeeName();
                txtMName.Focus();
            }
        }

        protected void txtMName_TextChanged(object sender, EventArgs e)
        {
            if (txtMName.Text != string.Empty)
            {
                setEmployeeName();
                txtLName.Focus();
            }
        }

        protected void txtLName_TextChanged(object sender, EventArgs e)
        {
            setEmployeeName();
            txtBirthdate.Focus();
        }

        protected void setEmployeeName()
        {
            txtEmpName.Text = txtFName.Text + " " + txtMName.Text + " " + txtLName.Text;
        }

    
        //protected void txtPAddress_TextChanged(object sender, EventArgs e)
        //{
        //    if (txtWAddress.Text == string.Empty)
        //        txtWAddress.Text = txtPAddress.Text;
        //    ddlPState.Focus();
        //}
        //---------------------------------------------------------------------

        protected void ddlPState_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string strQry = "SELECT State, StateCd FROM M_State where CountryCd=1 ORDER BY State";
            //DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            //ddlPState.DataSource = objDT;
            //ddlPState.DataTextField = "State";
            //ddlPState.DataValueField = "StateCd";
            //ddlPState.DataBind();
            if (ddlPState.SelectedIndex > 0)
            {
                string strQry = "SELECT District, DistCd FROM M_District where StateCd=" + ddlPState.SelectedValue + " ORDER BY District";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                ddlPDist.DataSource = objDT;
                ddlPDist.DataTextField = "District";
                ddlPDist.DataValueField = "DistCd";
                ddlPDist.DataBind();

                ddlPDist.Items.Insert(0, new ListItem("Select", "0"));
                //ddlPTaluka.Items.Insert(0, new ListItem("Select", "0"));
                //ddlPCity.Items.Insert(0, new ListItem("Select", "0"));

                ddlPDist.SelectedIndex = 0;
                ddlPTaluka.SelectedIndex = 0;
                ddlPCity.SelectedIndex = 0;
            }

            if (ddlWState.SelectedIndex == 0)
            {
                ddlWDist.Items.Insert(0, new ListItem("Select", "0"));
                ddlWTaluka.Items.Insert(0, new ListItem("Select", "0"));
                ddlWCity.Items.Insert(0, new ListItem("Select", "0"));

                ddlWDist.SelectedIndex = 0;
                ddlWTaluka.SelectedIndex = 0;
                ddlWCity.SelectedIndex = 0;

                //-- Changed on 15/01/2021--- (Commented)------------------------------
                //ddlWState.SelectedValue = ddlPState.SelectedValue;
                //---------------------------------------------------------------------
            }
            //ddlWDist.DataSource = objDT;
            //ddlWDist.DataTextField = "District";
            //ddlWDist.DataValueField = "DistCd";
            //ddlWDist.DataBind();

            // ddlPState.Focus();
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "FocusOnState", "document.getElementById('" + ddlPState.ClientID + "').focus(); ", true);

        }

        protected void ddlPDist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPDist.SelectedIndex > 0)
            {
                string strQry = "SELECT Taluka, TalCd FROM M_Taluka where DistCd=" + ddlPDist.SelectedValue + " ORDER BY Taluka";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                ddlPTaluka.DataSource = objDT;
                ddlPTaluka.DataTextField = "Taluka";
                ddlPTaluka.DataValueField = "TalCd";
                ddlPTaluka.DataBind();

                ddlPTaluka.Items.Insert(0, new ListItem("Select", "0"));
                //ddlPCity.Items.Insert(0, new ListItem("Select", "0"));

                ddlPTaluka.SelectedIndex = 0;
                ddlPCity.SelectedIndex = 0;
            }
            if (ddlWDist.SelectedIndex == 0)
            {
                ddlWTaluka.Items.Insert(0, new ListItem("Select", "0"));
                ddlWCity.Items.Insert(0, new ListItem("Select", "0"));

                ddlWTaluka.SelectedIndex = 0;
                ddlWCity.SelectedIndex = 0;

               
                //ddlWDist.SelectedValue = ddlPDist.SelectedValue;
                //---------------------------------------------------------------------
            }
            //ddlWTaluka.DataSource = objDT;
            //ddlWTaluka.DataTextField = "Taluka";
            //ddlWTaluka.DataValueField = "TalCd";
            //ddlWTaluka.DataBind();
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "FocusOnState","document.getElementById('" + ddlPDist.ClientID + "').focus(); ", true);
            //ddlPDist.Focus();
        }

        protected void ddlPTaluka_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPTaluka.SelectedIndex > 0)
            {
                string strQry = "SELECT City, Citycd FROM M_City where TalCd=" + ddlPTaluka.SelectedValue + " ORDER BY City";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                ddlPCity.DataSource = objDT;
                ddlPCity.DataTextField = "City";
                ddlPCity.DataValueField = "Citycd";
                ddlPCity.DataBind();

                ddlPCity.Items.Insert(0, new ListItem("Select", "0"));
                ddlPCity.SelectedIndex = 0;
            }
            if (ddlWTaluka.SelectedIndex == 0)
            {
                ddlWCity.Items.Insert(0, new ListItem("Select", "0"));
                ddlWCity.SelectedIndex = 0;

                //-- Changed on 15/01/2021--- (Commented)------------------------------
                //ddlWTaluka.SelectedValue = ddlPTaluka.SelectedValue;
                //-- ------------------------------------------------------------------
            }
            //ddlWCity.DataSource = objDT;
            //ddlWCity.DataTextField = "City";
            //ddlWCity.DataValueField = "Citycd";
            //ddlWCity.DataBind();

            // ddlPTaluka.Focus();
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "FocusOnState", "document.getElementById('" + ddlPTaluka.ClientID + "').focus(); ", true);
        }

        protected void ddlPCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            //-- Changed on 15/01/2021--- (Commented)------------------------------
            //if (ddlWCity.SelectedIndex == 0)
            //{
            //    ddlWCity.SelectedValue = ddlPCity.SelectedValue;
            //}
            //--------------------------------------------------------------------
            // ddlPCity.Focus();
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "FocusOnState", "document.getElementById('" + ddlPCity.ClientID + "').focus(); ", true);
        }


        //-- Changed on 15/01/2021--- (Commented)------------------------------
        //protected void txtPPinCode_TextChanged(object sender, EventArgs e)
        //{        
        //    //txtWPinCode.Text = txtPPinCode.Text;
        //    //txtWAddress.Focus();        
        //}
        //---------------------------------------------------------------------

        protected void txtWAddress_TextChanged(object sender, EventArgs e)
        {
            if (txtPAddress.Text != txtWAddress.Text)
            {
                ddlWState.SelectedIndex = 0;
                ddlWDist.SelectedIndex = 0;
                ddlWTaluka.SelectedIndex = 0;
                ddlWCity.SelectedIndex = 0;
                txtWPinCode.Text = "";
            }
            ddlWState.Focus();
        }
        protected void ddlWState_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strQry = "SELECT District, DistCd FROM M_District where StateCd=" + ddlWState.SelectedValue + " ORDER BY District";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlWDist.DataSource = objDT;
            ddlWDist.DataTextField = "District";
            ddlWDist.DataValueField = "DistCd";
            ddlWDist.DataBind();

            ddlWDist.Items.Insert(0, new ListItem("Select", "0"));
            //ddlWTaluka.Items.Insert(0, new ListItem("Select", "0"));
            //ddlWCity.Items.Insert(0, new ListItem("Select", "0"));

            ddlWDist.SelectedIndex = 0;
            ddlWTaluka.SelectedIndex = 0;
            ddlWCity.SelectedIndex = 0;

            //ddlWState.Focus();
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "FocusOnState", "document.getElementById('" + ddlWState.ClientID + "').focus(); ", true);
        }

        protected void ddlWDist_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strQry = "SELECT Taluka, TalCd FROM M_Taluka where DistCd=" + ddlWDist.SelectedValue + " ORDER BY Taluka";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlWTaluka.DataSource = objDT;
            ddlWTaluka.DataTextField = "Taluka";
            ddlWTaluka.DataValueField = "TalCd";
            ddlWTaluka.DataBind();

            ddlWTaluka.Items.Insert(0, new ListItem("Select", "0"));
            //ddlWCity.Items.Insert(0, new ListItem("Select", "0"));

            ddlWTaluka.SelectedIndex = 0;
            ddlWCity.SelectedIndex = 0;

            //ddlWDist.Focus();
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "FocusOnState", "document.getElementById('" + ddlWDist.ClientID + "').focus(); ", true);
        }

        protected void ddlWTaluka_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strQry = "SELECT City, Citycd FROM M_City where TalCd=" + ddlWTaluka.SelectedValue + " ORDER BY City";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlWCity.DataSource = objDT;
            ddlWCity.DataTextField = "City";
            ddlWCity.DataValueField = "Citycd";
            ddlWCity.DataBind();

            ddlWCity.Items.Insert(0, new ListItem("Select", "0"));
            ddlWCity.SelectedIndex = 0;

            //ddlWTaluka.Focus();
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "FocusOnState", "document.getElementById('" + ddlWTaluka.ClientID + "').focus(); ", true);
        }
        protected void ddlWCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ddlWCity.Focus();
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "FocusOnState", "document.getElementById('" + ddlWCity.ClientID + "').focus(); ", true);
        }

        protected void ddlBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strQry = "SELECT BankBranch, BankBranchId FROM M_BankBranch where BankCd=" + ddlBank.SelectedValue + " ORDER BY BankBranch";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlBankBranch.DataSource = objDT;
            ddlBankBranch.DataTextField = "BankBranch";
            ddlBankBranch.DataValueField = "BankBranchId";
            ddlBankBranch.DataBind();

            //ddlBankBranch.Focus();
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "FocusOnState", "document.getElementById('" + ddlBank.ClientID + "').focus(); ", true);
        }

        protected bool formValidation()
        {
            //Birthdate 
            if (Convert.ToDateTime(txtBirthdate.Text) >= Convert.ToDateTime(System.DateTime.Now.ToString("dd/MM/yyyy")))
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Birthdate Should Be Less Than System Date'); ", true);
                return false;
            }
            if (txtDOJ.Text != "")
            {
                if (Convert.ToDateTime(txtBirthdate.Text) > Convert.ToDateTime(txtDOJ.Text))
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Birthdate Should Be Less Than Joining Date'); ", true);
                    return false;
                }
            }

            //PFApplicable
            if (ddlPFApplicable.SelectedValue=="Y")
            {
                if(txtPFJoindDt.Text=="")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter PF Joining Date'); ", true);
                    return false;
                }
                if(txtDOJ.Text!="")
                {
                    if(Convert.ToDateTime(txtPFJoindDt.Text)<Convert.ToDateTime(txtDOJ.Text))
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('PF Joining Date Should Be Greater Than Equal To Joining Date'); ", true);
                        return false;
                    }
                }
                if (Convert.ToDateTime(txtPFJoindDt.Text) > System.DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('PF Joining Date Should Be Greater Than Equal System Date'); ", true);
                    return false;
                }
                if (txtPFNo.Text == "" && txtUANNo.Text == "")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter PF No. Or UAN No.'); ", true);
                    return false;
                }
                //if (txtUANNo.Text == string.Empty)
                //{
                //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter UAN No.'); ", true);
                //    return false;
                //}
                if (txtUANNo.Text != "")
                {
                    if (txtUANNo.Text.Length!=12)
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter Valid UAN No.'); ", true);
                        return false;
                    }
                }
            }
          
            //ESI Applicable
            if (ddlESI.SelectedValue=="Y")
            {
                if(txtESINo.Text=="")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter ESI No.'); ", true);
                    return false;
                }
                else
                {
                    if (txtESINo.Text.Length != 10)
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter Valid ESI No.'); ", true);
                        return false;
                    }
                }
                if(rblESICal.SelectedValue=="")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select ESI Calculation'); ", true);
                    return false;
                }
                


                if (ddlRegional.SelectedIndex ==0  )
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Regional'); ", true);
                    return false;
                }
                if (ddlSubRegional.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select SubRegional'); ", true);
                    return false;
                }
            }
            if (txtDOJ.Text != "" && txtGratuityDate.Text!="")
            {
                if (Convert.ToDateTime(txtGratuityDate.Text) < Convert.ToDateTime(txtDOJ.Text))
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Orignal Joining Should Be Greater Than Gratuity Date'); ", true);
                    return false;
                }
            }
            if(txtLeaveDate.Text!="" && txtDOJ.Text!="")
            {
                if (Convert.ToDateTime(txtLeaveDate.Text) < Convert.ToDateTime(txtDOJ.Text))
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Leave Date Should Be Greater Than Joining Date'); ", true);
                    return false;
                }
            }
            if (txtDOJ.Text != "" && txtJoinTrf1.Text != "")
            {
                if (Convert.ToDateTime(txtDOJ.Text) > Convert.ToDateTime(txtJoinTrf1.Text))
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Orignal Joining Should Be Greater Than New Joining Date'); ", true);
                    return false;
                }
            }

            int nID = 0;
            if (ViewState["ID"] != null)
            {
                nID = Convert.ToInt32(ViewState["ID"]);
            }

            string strQry = "";
            int nCnt = 0;
            if (HFEmployee.Value == "0")
            {
                 strQry = "SELECT Count(*) FROM M_Emp Where OrgId='" + Convert.ToInt32(Session["OrgID"]) + "' and Employeecd='" + txtEmpCode.Text + "'";
                 nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
                if (nCnt > 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Employee Code Already Exists!'); ", true);
                    clearControls();
                    return false;
                }
            }

            nID = 0;
            strQry = "SELECT Count(*) FROM M_Emp Where CommCodeAdhno='" + txtAadharNo.Text + "' and OrgId=" + Convert.ToInt32(Session["OrgID"]) + " and leavedate is null";
            nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 1)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Employee With This Aadhar No. Already Exists!'); ", true);
                clearControls();
                return false;
            }
            return true;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            HFEmployee.Value = "1";
            btnUpdate.Visible = false;
            btnGetData.Visible = false;
            txtEmpCode.ReadOnly = false;
            txtEmpCode.Focus();
        }

        protected void txtEmpCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int nCnt = 0;
                string strQry = "";

                if (txtEmpCode.Text != "")
                {
                    if (HFEmployee.Value == "1")
                    {
                        strQry = "SELECT Count(*) FROM M_Emp Where OrgId='" + Convert.ToInt32(Session["OrgID"]) + "' and Employeecd='" + txtEmpCode.Text + "'";
                        nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
                        if (nCnt == 0)
                        {
                            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Wrong Employee Code'); ", true);
                            clearControls();
                            return;
                        }
                        ViewRecord();
                        txtAadharNo.Focus();
                    }
                    else
                    {
                        strQry = "SELECT Count(*) FROM M_Emp Where OrgId='" + Convert.ToInt32(Session["OrgID"]) + "' and Employeecd='" + txtEmpCode.Text + "'";
                        nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
                        if (nCnt > 0)
                        {
                            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Employee Code Already Exists!'); ", true);
                            clearControls();
                            return;
                        }
                        txtAadharNo.Focus();
                        btnUpdate.Visible = false;
                    }
                }
                else
                {
                    clearControls();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlESI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlESI.SelectedValue=="N")
            {
                rblESICal.ClearSelection();
                rblESICal.Attributes.Add("Disabled", "Disabled");
                txtESINo.ReadOnly = true;
                txtESINo.Text = "";
                ddlRegional.Attributes.Add("Disabled", "Disabled");
                ddlSubRegional.Attributes.Add("Disabled", "Disabled");

                txtESIEffectDt.Attributes.Remove("ReadOnly");
                CalendarExtender4.Enabled = true;

                ddlRegional.SelectedIndex = 0;
                ddlSubRegional.Items.Clear();
                ddlSubRegional.Items.Insert(0, new ListItem("Select", "0"));
               // chkIsActive.Focus();
            }
            else
            {
                rblESICal.Attributes.Remove("Disabled");
                txtESINo.ReadOnly = false;
                ddlRegional.Attributes.Remove("Disabled");
                ddlSubRegional.Attributes.Remove("Disabled");
                //  txtESINo.Focus();
                txtESIEffectDt.Attributes.Add("ReadOnly", "ReadOnly");
                txtESIEffectDt.Text = "";
                CalendarExtender4.Enabled = false;
            }
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "FocusOnState", "document.getElementById('" + ddlESI.ClientID + "').focus(); ", true);

        }

        protected void btnGetData_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtAadharNo.Text!=string.Empty)
                {
                    HFEmployee.Value = "3";
                    btnUpdate.Visible = false;
                    string strQry = "SELECT * FROM M_Emp where CommCodeAdhno='" + txtAadharNo.Text + "' and isActive='Y'";
                    DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                    if(objDT.Rows.Count>0)
                    {
                        RFVtxtJoinTrf1.Enabled = true;
                        BindData();
                        BindData1();
                        txtEmpCode.Text = objDT.Rows[0]["Employeecd"].ToString();
                        txtAadharNo.Text = objDT.Rows[0]["CommCodeAdhno"].ToString();
                        txtFName.Text = objDT.Rows[0]["FName"].ToString();
                        txtMName.Text = objDT.Rows[0]["MName"].ToString();
                        txtLName.Text = objDT.Rows[0]["LName"].ToString();
                        txtEmpName.Text = objDT.Rows[0]["Employeename"].ToString();

                        txtBirthdate.Text = objDT.Rows[0]["Birthdate"] != DBNull.Value ? Convert.ToDateTime(objDT.Rows[0]["Birthdate"]).ToString("dd/MM/yyyy") : "";
                        if (objDT.Rows[0]["Gendercd"] != DBNull.Value)
                            ddlGender.SelectedValue = objDT.Rows[0]["Gendercd"].ToString();

                        if (objDT.Rows[0]["BloodGroup"] != DBNull.Value)
                            ddlBloodGroup.SelectedValue = objDT.Rows[0]["BloodGroup"].ToString();

                        if (objDT.Rows[0]["Marital"] != DBNull.Value)
                            ddlMaritalStatus.SelectedValue = objDT.Rows[0]["Marital"].ToString();

                        txtMobileNo.Text = objDT.Rows[0]["MobNo"].ToString();
                        txtEmailID.Text = objDT.Rows[0]["EmailId"].ToString();

                        txtQualification.Text = objDT.Rows[0]["Qual"].ToString();
                        txtPreviousExp.Text = objDT.Rows[0]["PrevExper"].ToString();
                        
                        txtDOJ.Text = objDT.Rows[0]["OrigJoindate"] != DBNull.Value ? Convert.ToDateTime(objDT.Rows[0]["OrigJoindate"]).ToString("dd/MM/yyyy") : "";
                        txtJoinTrf1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
                        txtGratuityDate.Text = objDT.Rows[0]["Gratuitydate"] != DBNull.Value ? Convert.ToDateTime(objDT.Rows[0]["Gratuitydate"]).ToString("dd/MM/yyyy") : "";

                        txtPAddress.Text = objDT.Rows[0]["PAddre"].ToString();
                        if (objDT.Rows[0]["PStatecd"] != DBNull.Value)
                            ddlPState.SelectedValue = objDT.Rows[0]["PStatecd"].ToString();
                        if (objDT.Rows[0]["PDistcd"] != DBNull.Value)
                            ddlPDist.SelectedValue = objDT.Rows[0]["PDistcd"].ToString();
                        if (objDT.Rows[0]["PTalcd"] != DBNull.Value)
                            ddlPTaluka.SelectedValue = objDT.Rows[0]["PTalcd"].ToString();
                        if (objDT.Rows[0]["PCitycd"] != DBNull.Value)
                            ddlPCity.SelectedValue = objDT.Rows[0]["PCitycd"].ToString();

                        txtWAddress.Text = objDT.Rows[0]["WAddre"].ToString();
                        if (objDT.Rows[0]["WStatecd"] != DBNull.Value)
                            ddlWState.SelectedValue = objDT.Rows[0]["WStatecd"].ToString();
                        if (objDT.Rows[0]["WDistcd"] != DBNull.Value)
                            ddlWDist.SelectedValue = objDT.Rows[0]["WDistcd"].ToString();
                        if (objDT.Rows[0]["WTalcd"] != DBNull.Value)
                            ddlWTaluka.SelectedValue = objDT.Rows[0]["WTalcd"].ToString();
                        if (objDT.Rows[0]["WCitycd"] != DBNull.Value)
                            ddlWCity.SelectedValue = objDT.Rows[0]["WCitycd"].ToString();

                        if (objDT.Rows[0]["BankCd"] != DBNull.Value)
                            ddlBank.SelectedValue = objDT.Rows[0]["BankCd"].ToString();
                        if (objDT.Rows[0]["BankBranchId"] != DBNull.Value)
                            ddlBankBranch.SelectedValue = objDT.Rows[0]["BankBranchId"].ToString();
                        txtBankAcNo.Text = objDT.Rows[0]["BankActNo"].ToString();
                        txtpanNo.Text = objDT.Rows[0]["PANNo"].ToString();

                        if (objDT.Rows[0]["PFApplicable"] != DBNull.Value)
                            ddlPFApplicable.SelectedValue = objDT.Rows[0]["PFApplicable"].ToString();
                        txtPFJoindDt.Text = objDT.Rows[0]["PFJoindate"] != DBNull.Value ? Convert.ToDateTime(objDT.Rows[0]["PFJoindate"]).ToString("dd/MM/yyyy") : "";
                        txtPFNo.Text = objDT.Rows[0]["PFNo"].ToString();
                        txtUANNo.Text = objDT.Rows[0]["UANNO"].ToString();
                        if (objDT.Rows[0]["PFApplicable"].ToString() == "N")
                        {
                            txtPFJoindDt.ReadOnly = true;
                            txtPFNo.ReadOnly = true;
                            txtUANNo.ReadOnly = true;
                        }
                        else
                        {
                            txtPFJoindDt.ReadOnly = false;
                            txtPFNo.ReadOnly = false;
                            txtUANNo.ReadOnly = false;
                        }

                        if (objDT.Rows[0]["HRAApplicable"] != DBNull.Value)
                            ddlHRAApplicable.SelectedValue = objDT.Rows[0]["HRAApplicable"].ToString();
                        if (objDT.Rows[0]["ProfTaxApplicable"] != DBNull.Value)
                            ddlProfessionalTax.SelectedValue = objDT.Rows[0]["ProfTaxApplicable"].ToString();
                        if (objDT.Rows[0]["LabWelApplicable"] != DBNull.Value)
                            ddlLabourWalfare.SelectedValue = objDT.Rows[0]["LabWelApplicable"].ToString();
                        if (objDT.Rows[0]["ESIApplicable"] != DBNull.Value)
                            ddlESI.SelectedValue = objDT.Rows[0]["ESIApplicable"].ToString();

                        txtESINo.Text = objDT.Rows[0]["ESIIP"].ToString();
                        if (objDT.Rows[0]["ESIApplicable"].ToString() == "N")
                        {
                            txtESINo.ReadOnly = true;
                            rblESICal.Enabled = false;
                        }
                        else
                        {
                            txtESINo.ReadOnly = false;
                            rblESICal.Enabled = true;
                            if (objDT.Rows[0]["ESIIP"] != DBNull.Value)
                                txtESINo.Text = objDT.Rows[0]["ESIIP"].ToString();
                            if (objDT.Rows[0]["ESICalculate"] != DBNull.Value)
                                rblESICal.SelectedValue = objDT.Rows[0]["ESICalculate"].ToString();

                        }
                        txtDrivingLicence.Text = objDT.Rows[0]["DrivingLicence"].ToString();

                        if (objDT.Rows[0]["LIC_Id"] != DBNull.Value)
                            txtLICID.Text = objDT.Rows[0]["LIC_Id"].ToString();
                        if (objDT.Rows[0]["LIC_PolicyNo"] != DBNull.Value)
                            txtLICPolicyNo.Text = objDT.Rows[0]["LIC_PolicyNo"].ToString();

                        chkIsActive.Checked = objDT.Rows[0]["IsActive"].ToString() == "Y" ? true : false;
                        //chkHODApplicable.Checked = objDT.Rows[0]["HODInchAppl"].ToString() == "Y" ? true : false;
                        if (objDT.Rows[0]["HODInchAppl"] != DBNull.Value)
                            ddlHODApplicable.SelectedValue = objDT.Rows[0]["HODInchAppl"].ToString();

                        if (objDT.Rows[0]["castID"] != DBNull.Value)
                            ddlCast.SelectedValue = objDT.Rows[0]["castID"].ToString();

                        if (objDT.Rows[0]["Regid"] != DBNull.Value)
                            ddlRegional.SelectedValue = objDT.Rows[0]["Regid"].ToString();
                            ddlRegional_SelectedIndexChanged(null, null);
                        if (objDT.Rows[0]["SubRegid"] != DBNull.Value)
                            ddlSubRegional.SelectedValue = objDT.Rows[0]["SubRegid"].ToString();
                        if (objDT.Rows[0]["DiIncode"] != DBNull.Value)
                            ddlDI.SelectedValue = objDT.Rows[0]["DiIncode"].ToString();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Aadhar No Does Not Exist)'); ", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter Common Code(Aadhar No)'); ", true);
                    return;
                }
            }
            catch(Exception ex)
            {

            }
        }

        protected void AllReadOnly()
        {
            
        }

        protected void ddlPFApplicable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPFApplicable.SelectedValue=="N")
            {
                txtPFJoindDt.Text = "";
                txtPFNo.Text = "";
                txtUANNo.Text = "";

                txtPFJoindDt.ReadOnly = true;
                txtUANNo.ReadOnly = true;
                txtPFNo.ReadOnly = true;
                //ddlHRAApplicable.Focus();
            }
            else
            {
                txtPFJoindDt.ReadOnly = false;
                txtUANNo.ReadOnly = false;
                txtPFNo.ReadOnly = false;
                //txtPFJoindDt.Focus();
            }
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "FocusOnState", "document.getElementById('" + ddlPFApplicable.ClientID + "').focus(); ", true);
        }

        protected void ddlRegional_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlRegional.SelectedIndex>0)
            {
                string str = "SELECT        SubRegId, SubRegName FROM M_SubRegional WHERE(RegId = '" + ddlRegional.SelectedValue + "')";
                DataTable dt = SqlHelper.ExecuteDataTable(str, AppGlobal.strConnString);

                ddlSubRegional.DataSource = dt;
                ddlSubRegional.DataTextField = "SubRegName";
                ddlSubRegional.DataValueField = "SubRegId";
                ddlSubRegional.DataBind();

                ddlSubRegional.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddlSubRegional.Items.Clear();
                ddlSubRegional.Items.Insert(0, new ListItem("Select", "0"));
            }            

            //
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "FocusOnState", "document.getElementById('" + ddlRegional.ClientID + "').focus();", true);
        }

      

        protected void ddlSubRegional_SelectedIndexChanged1(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "FocusOnState", "document.getElementById('" + ddlSubRegional.ClientID + "').focus(); ", true);

        }

        protected void ddlDI_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "FocusOnState", "document.getElementById('" + ddlDI.ClientID + "').focus(); ", true);
        }

        protected void txtESIEffectDt_TextChanged(object sender, EventArgs e)
        {
            if (txtESIEffectDt.Text != "")
            {
                string YrMonthCd = Convert.ToDateTime(txtESIEffectDt.Text).Year.ToString() + (Convert.ToDateTime(txtESIEffectDt.Text).Month.ToString().Length == 1 ? "0" + Convert.ToDateTime(txtESIEffectDt.Text).Month.ToString() : Convert.ToDateTime(txtESIEffectDt.Text).Month.ToString());
                string strQryy = "SELECT * FROM T_MonthlySalary Where OrgId='" + Convert.ToInt32(Session["OrgID"]) + "' and Employeecd='" + txtEmpCode.Text + "' and ESIEmpContribution<>0 and " + YrMonthCd + " <=right(MonYrcd,4)+left(MonYrcd,2)";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQryy, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('ESI Exempted date not valid'); ", true);
                    txtESIEffectDt.Text = "";              
                }

            }
        }

        protected void ddlLabourWalfare_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlLabourWalfare.SelectedValue=="Y")
            {
                txtLINNo.ReadOnly = false;
            }
            else
            {
                txtLINNo.ReadOnly = true;
                txtLINNo.Text = "";
            }
        }
    }
}