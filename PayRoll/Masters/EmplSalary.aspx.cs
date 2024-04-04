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
    public partial class EmplSalary : System.Web.UI.Page
    {
        SqlConnection sqlConn = null;
        SqlCommand sqlCmd = null;
        SqlTransaction sqlTrans = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
                BindGrid();
                clearControls();
                checkHeading();
            }
            SalApproved();
        }

        protected void SalApproved()
        {
            if (txtEmpCode.Text != "" && ddlMon.SelectedValue != "00" && ddlYear.SelectedValue != "0000")
            {
                string strQry = "SELECT isnull(Approval,'N') as Approval FROM M_Salary where orgID=" + Convert.ToInt32(Session["OrgID"]) + " and Employeecd='" + txtEmpCode.Text + "' and MonYrCd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    if (objDT.Rows[0]["Approval"].ToString() == "Y")
                    {
                        txtBasic.ReadOnly = true;
                        txtHRA.ReadOnly = true;
                        txtConveyance.ReadOnly = true;
                        txtCanteen.ReadOnly = true;
                        txtEducation.ReadOnly = true;
                        txtMedical.ReadOnly = true;
                        txtWashing.ReadOnly = true;
                        txtUniform.ReadOnly = true;
                        txtAdd1.ReadOnly = true;
                        txtAdd2.ReadOnly = true;
                        txtAdd3.ReadOnly = true;
                        //txtTotal.ReadOnly = true;
                        txtPfEmpPct.ReadOnly = true;
                        txtPFMan.ReadOnly = true;
                        chkIsActive.Enabled = false;
                    }
                    else
                    {
                        txtBasic.ReadOnly = false;
                        txtHRA.ReadOnly = false;
                        txtConveyance.ReadOnly = false;
                        txtCanteen.ReadOnly = false;
                        txtEducation.ReadOnly = false;
                        txtMedical.ReadOnly = false;
                        txtWashing.ReadOnly = false;
                        txtUniform.ReadOnly = false;
                        txtAdd1.ReadOnly = false;
                        txtAdd2.ReadOnly = false;
                        txtAdd3.ReadOnly = false;
                        //txtTotal.ReadOnly = false;
                        txtPfEmpPct.ReadOnly = true;
                        txtPFMan.ReadOnly = true;
                        chkIsActive.Enabled = true;

                        strQry = "select * from M_Emp where OrgId=" + Convert.ToInt32(Session["OrgID"]) + " and Employeecd='" + txtEmpCode.Text + "'";
                        objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                        if (objDT.Rows.Count > 0)
                        {
                            txtEmpName.Text = objDT.Rows[0]["Employeename"].ToString();
                            if (objDT.Rows[0]["HRAApplicable"].ToString() == "Y")
                                txtHRA.ReadOnly = false;
                            if (objDT.Rows[0]["PFApplicable"].ToString() == "E")
                            {
                                txtPfEmpPct.ReadOnly = false;
                                txtPFMan.ReadOnly = false;
                            }
                            else
                            {
                                txtPfEmpPct.ReadOnly = true;
                                txtPFMan.ReadOnly = true;
                            }
                        }
                        checkHeading();
                    }
                }
            }
        }

        protected void checkHeading()
        {
            string strQry = "";
            
            strQry = "SELECT Add1Heading FROM M_AddHeading Where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and Add1Heading is not null";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if(objDT.Rows.Count>0)
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
        private void BindData()
        {
            string strQry = "SELECT Year  FROM M_Year Where IsActive='Y' ORDER BY Year desc";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlYear.DataSource = objDT;
            ddlYear.DataTextField = "Year";
            ddlYear.DataValueField = "Year";
            ddlYear.DataBind();

            ddlYear.Items.Insert(0, new ListItem("Select", "0000"));

            txtTotal.Attributes.Add("readonly", "readonly");
        }

        private void BindGrid()
        {
            //string strQry = "SELECT emp.Employeecd, emp.Employeename, isnull(sal.Docdate,'') as IsActive,isnull(sal.IsActive,'N') as IsActive, isnull(sal.Approval,'N') as Approval FROM M_Emp as emp LEFT OUTER JOIN M_Salary as sal on emp.orgID=sal.orgID and emp.Employeecd=sal.Employeecd where emp.orgID=" + Convert.ToInt32(Session["OrgID"]) + " and emp.Employeecd='" + txtEmpCode.Text + "'";
            string strQry = "SELECT emp.Employeecd, emp.Employeename,emp.PFApplicable,emp.HRAApplicable, sal.OrgId, isnull(convert(varchar, sal.Docdate, 103),'') as Docdate,DATENAME(MONTH, DateAdd( month , CONVERT(INT,LEFT(MonYrcd,2)) , -1 ) -1 ) + right(MonYrcd,4) as MonYrCd, BasicDA, HRA, Conveyance, Education, Medical, Canteen,Washing,Uniform,isnull(Add1,0) as Add1,isnull(Add2,0) as Add2,isnull(Add3,0) as Add3, isnull(PfMan,0) as PfMan,isnull(PfEmpPct,0) as PfEmpPct, isnull(sal.IsActive,'N') as IsActive, isnull(sal.Approval,'N') as Approval, Gross, MonYrcd as Monyr FROM M_Emp as emp LEFT OUTER JOIN M_Salary as sal on emp.orgID=sal.orgID and emp.Employeecd=sal.Employeecd where emp.orgID=" + Convert.ToInt32(Session["OrgID"]) + " and emp.Employeecd='" + txtEmpCode.Text + "' order by right(MonYrcd,4) + left(MonYrcd,2) desc";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvList.DataSource = objDT;
            gvList.DataBind();

            ViewState["objDTList"] = objDT;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    if (formValidation())
                    {
                        string strQry = "select * from T_SalaryLock where OrgId=" + Convert.ToInt16(Session["orgID"]) + " and RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)>='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "' and Lock='Y'";
                        DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                        if (objDT.Rows.Count > 0)
                        {
                            clearControls();
                            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Salary Already Processed, Cant Modify Now.'); ", true);
                            return;
                        }

                        if (btnSave.Text == "Save")
                        {
                            InsertRecord();
                        }
                        else if (btnSave.Text == "Update")
                        {
                            strQry = "select monyrcd from M_Salary where OrgId=" + Convert.ToInt16(Session["orgID"]) + " and RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)>='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "' and Employeecd='" + txtEmpCode.Text + "'";
                            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                            if (objDT.Rows.Count == 0)
                            {
                                clearControls();
                                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Cant Modify Month Year'); ", true);
                                return;
                            }

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
            try
            {
                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);

                string strQry = "";
                bool result = false;

                strQry = "select * from M_Salary where OrgId=" + Convert.ToInt16(Session["orgID"]) + " and RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "' and Employeecd='" + txtEmpCode.Text + "'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    clearControls();
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Record Already Exist.'); ", true);
                    return;
                }

                strQry = @"INSERT INTO M_Salary(OrgId, Employeecd, Docdate, MonYrcd, BasicDA, HRA, Conveyance,Education,Medical, Canteen, Washing, Uniform, Add1, Add2, Add3, PfMan, PfEmpPct, gross, IsActive, Approval) 
                       VALUES(@OrgId, @Employeecd, @Docdate, @MonYrcd, @BasicDA, @HRA, @Conveyance, @Education, @Medical, @Canteen, @Washing, @Uniform, @Add1, @Add2, @Add3, @PfMan, @PfEmpPct, @gross, @IsActive, @Approval)";
                int nID = SqlHelper.GetMaxID("M_Organization", "OrgId", AppGlobal.strConnString);

                SqlParameter[] para = new SqlParameter[20];
                para[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                para[1] = new SqlParameter("@Employeecd", txtEmpCode.Text.Trim());
                //para[2] = new SqlParameter("@Docdate", Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy"));
                para[2] = new SqlParameter("@Docdate", Convert.ToDateTime("01-" + ddlMon.SelectedValue +"-"+ ddlYear.SelectedValue ).ToString("dd MMM yyyy"));
                para[3] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + ddlYear.SelectedValue);
                para[4] = new SqlParameter("@BasicDA", Convert.ToDouble(txtBasic.Text));
                para[5] = new SqlParameter("@HRA", txtHRA.Text != "" ? Convert.ToDouble(txtHRA.Text) : 0);
                para[6] = new SqlParameter("@Conveyance", txtConveyance.Text != "" ? Convert.ToDouble(txtConveyance.Text) : 0);
                para[7] = new SqlParameter("@Education", txtEducation.Text != "" ? Convert.ToDouble(txtEducation.Text) : 0);
                para[8] = new SqlParameter("@Medical", txtMedical.Text != "" ? Convert.ToDouble(txtMedical.Text) : 0);
                para[9] = new SqlParameter("@Canteen", txtCanteen.Text != "" ? Convert.ToDouble(txtCanteen.Text) : 0);
                para[10] = new SqlParameter("@Washing", txtWashing.Text != "" ? Convert.ToDouble(txtWashing.Text) : 0);
                para[11] = new SqlParameter("@Uniform", txtUniform.Text != "" ? Convert.ToDouble(txtUniform.Text) : 0);
                para[12] = new SqlParameter("@PfMan", txtPFMan.Text != "" ? Convert.ToDouble(txtPFMan.Text) : 0);
                para[13] = new SqlParameter("@PfEmpPct", txtPfEmpPct.Text != "" ? Convert.ToDouble(txtPfEmpPct.Text) : 0);
                para[14] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");
                para[15] = new SqlParameter("@Approval", chkApproval.Checked ? "Y" : "N");

                para[16] = new SqlParameter("@Add1", txtAdd1.Text != "" ? Convert.ToDouble(txtAdd1.Text) : 0);
                para[17] = new SqlParameter("@Add2", txtAdd2.Text != "" ? Convert.ToDouble(txtAdd2.Text) : 0);
                para[18] = new SqlParameter("@Add3", txtAdd3.Text != "" ? Convert.ToDouble(txtAdd3.Text) : 0);
                para[19] = new SqlParameter("@Gross", txtTotal.Text != "" ? Convert.ToDouble(txtTotal.Text) : 0);

                result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

                if (result)
                {
                   
                    double total = 0;
                    if (txtBasic.Text != "")
                        total = Convert.ToDouble(txtBasic.Text);
                    if (txtHRA.Text != "")
                        total += Convert.ToDouble(txtHRA.Text);
                    if (txtConveyance.Text != "")
                        total += Convert.ToDouble(txtConveyance.Text);
                    if (txtEducation.Text != "")
                        total += Convert.ToDouble(txtEducation.Text);
                    if (txtMedical.Text != "")
                        total += Convert.ToDouble(txtMedical.Text);
                    if (txtCanteen.Text != "")
                        total += Convert.ToDouble(txtCanteen.Text);
                    if (txtWashing.Text != "")
                        total += Convert.ToDouble(txtWashing.Text);
                    if (txtUniform.Text != "")
                        total += Convert.ToDouble(txtUniform.Text);

                    if (txtAdd1.Text != "")
                        total += Convert.ToDouble(txtAdd1.Text);
                    if (txtAdd2.Text != "")
                        total += Convert.ToDouble(txtAdd2.Text);
                    if (txtAdd3.Text != "")
                        total += Convert.ToDouble(txtAdd3.Text);

                    strQry = "";
                    strQry = "INSERT INTO T_Log(OrgId, Employeecd, MenuId, Mode, Computername, amount) VALUES(@OrgId, @Employeecd, @MenuId, @Mode, @Computername,@amount)";

                    SqlParameter[] paraLog = new SqlParameter[6];
                    paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                    paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                    paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                    paraLog[3] = new SqlParameter("@Mode", "A");
                    paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());
                    paraLog[5] = new SqlParameter("@amount", total);

                    result = SqlHelper.ExecuteNonQuery(strQry, paraLog, AppGlobal.strConnString);
                    if (result)
                    {
                        sqlTrans.Commit();
                        clearControls();
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
                    }
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
            }
          
        }

        private void UpdateRecord()
        {
            try
            {
                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);

                string strQry = "";
                bool result = false;
                int nId = Convert.ToInt32(ViewState["ID"]);

                strQry = "";
                strQry = @"UPDATE M_Salary SET Docdate=@Docdate, BasicDA=@BasicDA, HRA=@HRA, Conveyance=@Conveyance, 
                     Education=@Education, Medical=@Medical, Canteen=@Canteen, Washing=@Washing, Uniform=@Uniform, Add1=@Add1, Add2=@Add2, Add3=@Add3, PfMan=@PfMan, PfEmpPct=@PfEmpPct, IsActive=@IsActive, Approval=@Approval, gross=@gross
                     WHERE OrgId=@OrgId and Employeecd=@Employeecd and MonYrcd=@MonYrcd";

                SqlParameter[] para = new SqlParameter[20];
                para[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                para[1] = new SqlParameter("@Employeecd", txtEmpCode.Text.Trim());
                para[2] = new SqlParameter("@Docdate", Convert.ToDateTime(ViewState["docDate"]).ToString("dd MMM yyyy"));
                para[3] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + ddlYear.SelectedValue);
                para[4] = new SqlParameter("@BasicDA", Convert.ToDouble(txtBasic.Text));
                para[5] = new SqlParameter("@HRA", txtHRA.Text != "" ? Convert.ToDouble(txtHRA.Text) : 0);
                para[6] = new SqlParameter("@Conveyance", txtConveyance.Text != "" ? Convert.ToDouble(txtConveyance.Text) : 0);
                para[7] = new SqlParameter("@Education", txtEducation.Text != "" ? Convert.ToDouble(txtEducation.Text) : 0);
                para[8] = new SqlParameter("@Medical", txtMedical.Text != "" ? Convert.ToDouble(txtMedical.Text) : 0);
                para[9] = new SqlParameter("@Canteen", txtCanteen.Text != "" ? Convert.ToDouble(txtCanteen.Text) : 0);
                para[10] = new SqlParameter("@Washing", txtWashing.Text != "" ? Convert.ToDouble(txtWashing.Text) : 0);
                para[11] = new SqlParameter("@Uniform", txtUniform.Text != "" ? Convert.ToDouble(txtUniform.Text) : 0);
                para[12] = new SqlParameter("@PfMan", txtPFMan.Text != "" ? Convert.ToDouble(txtPFMan.Text) : 0);
                para[13] = new SqlParameter("@PfEmpPct", txtPfEmpPct.Text != "" ? Convert.ToDouble(txtPfEmpPct.Text) : 0);
                para[14] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");
                para[15] = new SqlParameter("@Approval", chkApproval.Checked ? "Y" : "N");

                para[16] = new SqlParameter("@Add1", txtAdd1.Text != "" ? Convert.ToDouble(txtAdd1.Text) : 0);
                para[17] = new SqlParameter("@Add2", txtAdd2.Text != "" ? Convert.ToDouble(txtAdd2.Text) : 0);
                para[18] = new SqlParameter("@Add3", txtAdd3.Text != "" ? Convert.ToDouble(txtAdd3.Text) : 0);

                para[19] = new SqlParameter("@Gross", txtTotal.Text != "" ? Convert.ToDouble(txtTotal.Text) : 0);


                result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

                if (result)
                {
                    
                    double total = 0;
                    if (txtBasic.Text != "")
                        total = Convert.ToDouble(txtBasic.Text);
                    if (txtHRA.Text !="")
                        total += Convert.ToDouble(txtHRA.Text);
                    if (txtConveyance.Text != "")
                        total += Convert.ToDouble(txtConveyance.Text);
                    if (txtEducation.Text != "")
                        total += Convert.ToDouble(txtEducation.Text);
                    if (txtMedical.Text != "")
                        total += Convert.ToDouble(txtMedical.Text);
                    if (txtCanteen.Text != "")
                        total += Convert.ToDouble(txtCanteen.Text);
                    if (txtWashing.Text != "")
                        total += Convert.ToDouble(txtWashing.Text);
                    if (txtUniform.Text != "")
                        total += Convert.ToDouble(txtUniform.Text);

                    if (txtAdd1.Text != "")
                        total += Convert.ToDouble(txtAdd1.Text);
                    if (txtAdd2.Text != "")
                        total += Convert.ToDouble(txtAdd2.Text);
                    if (txtAdd3.Text != "")
                        total += Convert.ToDouble(txtAdd3.Text);
                    strQry = "";
                    strQry = "INSERT INTO T_Log(OrgId, Employeecd, MenuId, Mode, Computername, amount) VALUES(@OrgId, @Employeecd, @MenuId, @Mode, @Computername,@amount)";

                    SqlParameter[] paraLog = new SqlParameter[6];
                    paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                    paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                    paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                    paraLog[3] = new SqlParameter("@Mode", "U");
                    paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());
                    paraLog[5] = new SqlParameter("@amount", total);

                    result = SqlHelper.ExecuteNonQuery(strQry, paraLog, AppGlobal.strConnString);
                    if (result)
                    {
                        sqlTrans.Commit();
                        clearControls();
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
                    }
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
            foreach (var item in pnlSalaryData.Controls)
            {
                if (item is TextBox)
                {
                    ((TextBox)item).Text = "";
                }
            }
            //lblAdd1.Text = "";
            //lblAdd2.Text = "";
            //lblAdd3.Text = "";
            ddlMon.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;

            gvList.DataSource = null;
            gvList.DataBind();

            txtPfEmpPct.ReadOnly = true;
            txtPFMan.ReadOnly = true;
            txtHRA.ReadOnly = true;

            chkApproval.Checked = false;
            chkIsActive.Checked = true;
            btnSave.Text = "Save";
            txtEmpCode.Focus();

            txtBasic.ReadOnly = false;
            txtHRA.ReadOnly = false;
            txtConveyance.ReadOnly = false;
            txtCanteen.ReadOnly = false;
            txtEducation.ReadOnly = false;
            txtMedical.ReadOnly = false;
            txtWashing.ReadOnly = false;
            txtUniform.ReadOnly = false;
            txtAdd1.ReadOnly = false;
            txtAdd2.ReadOnly = false;
            txtAdd3.ReadOnly = false;
            //txtTotal.ReadOnly = false;
            txtPfEmpPct.ReadOnly = true;
            txtPFMan.ReadOnly = true;
            chkIsActive.Enabled = true;

            string strQry = "select * from M_Authority where OrgId=" + Convert.ToInt32(Session["OrgID"]) + " and Employeecd='" + Session["UserName"].ToString() + "' and FormCode=44";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (objDT.Rows.Count > 0)
            {
                chkApproval.Visible = true;
            }
            else
            {
                chkApproval.Visible = false;
            }
        }

        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Show")
                {
                    int i = Convert.ToInt32(e.CommandArgument);
                    ViewRecord(i);
                    btnSave.Text = "Update";
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ViewRecord(int i)
        {
            DataTable objDT = (DataTable)ViewState["objDTList"];
            //string strQry = "SELECT emp.Employeecd, emp.Employeename,emp.PFApplicable,emp.HRAApplicable, sal.OrgId, isnull(convert(varchar, sal.Docdate, 103),'') as Docdate, MonYrCd, isnull(BasicDA,0) as BasicDA, isNull(HRA,0) as HRA, isNull(Conveyance,0) as Conveyance, isnull(Education,0) as Education, isnull(Medical,0) as Medical ,isnull(Canteen,0) as Canteen,isnull(Washing,0) as Washing , isnull(Uniform,0) as Uniform,isnull(Add1,0) as Add1,isnull(Add2,0) as Add2, isnull(Add3,0) as Add3, isnull(gross,0) as gross, isnull(PfMan,0) as PfMan,isnull(PfEmpPct,0) as PfEmpPct, isnull(sal.IsActive,'N') as IsActive, isnull(sal.Approval,'N') as Approval FROM M_Emp as emp LEFT OUTER JOIN M_Salary as sal on emp.orgID=sal.orgID and emp.Employeecd=sal.Employeecd where emp.orgID=" + Convert.ToInt32(Session["OrgID"]) + " and emp.Employeecd='" + txtEmpCode.Text + "' and docDate='" + Convert.ToDateTime(objDTl.Rows[i]["docDate"]).ToString("dd MMM yyyy") + "'";
            //DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);

          //  DataTable objDT = (DataTable)ViewState["objDTList"];
            string month = "00", year = "0000", str="";
            str = objDT.Rows[i]["Monyr"].ToString();//Monyrcd
            month = str.Substring(0, 2);
            year = str.Substring(2, 4);
            ddlMon.SelectedValue = month;
            ddlYear.SelectedValue = year;
            //txtDocDate.Text= Convert.ToDateTime(objDT.Rows[i]["docDate"]).ToString("dd/MM/yyyy");
            ViewState["docDate"] = Convert.ToDateTime(objDT.Rows[i]["docDate"]);
            
            ViewState["ID"] = objDT.Rows[i]["OrgId"].ToString();
            txtBasic.Text = Convert.ToDouble(objDT.Rows[i]["BasicDA"]).ToString("0.00");
            if (objDT.Rows[i]["HRAApplicable"].ToString() == "Y")
            {
                txtHRA.Text = Convert.ToDouble(objDT.Rows[i]["HRA"]).ToString("0.00");
                txtHRA.ReadOnly = false;
            }
            if (objDT.Rows[i]["PFApplicable"].ToString() == "Y")
            {
                txtPFMan.Text = Convert.ToDouble(objDT.Rows[i]["PfMan"]).ToString("0.00");
                txtPfEmpPct.Text = Convert.ToDouble(objDT.Rows[i]["PfEmpPct"]).ToString("0.00");
                txtPFMan.ReadOnly = false;
                txtPfEmpPct.ReadOnly = false;
            }
            txtConveyance.Text = Convert.ToDouble(objDT.Rows[i]["Conveyance"]).ToString("0.00");
            txtEducation.Text = Convert.ToDouble(objDT.Rows[i]["Education"]).ToString("0.00");
            txtMedical.Text = Convert.ToDouble(objDT.Rows[i]["Medical"]).ToString("0.00");
            txtCanteen.Text = Convert.ToDouble(objDT.Rows[i]["Canteen"]).ToString("0.00");
            txtWashing.Text = Convert.ToDouble(objDT.Rows[i]["Washing"]).ToString("0.00");
            txtUniform.Text = Convert.ToDouble(objDT.Rows[i]["Uniform"]).ToString("0.00");

            txtAdd1.Text = Convert.ToDouble(objDT.Rows[i]["Add1"]).ToString("0.00");
            txtAdd2.Text = Convert.ToDouble(objDT.Rows[i]["Add2"]).ToString("0.00");
            txtAdd3.Text = Convert.ToDouble(objDT.Rows[i]["Add3"]).ToString("0.00");
            txtTotal.Text = Convert.ToDouble(objDT.Rows[i]["gross"]).ToString("0.00"); 
           // ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "MyFun1", "total();", true);
            chkIsActive.Checked = objDT.Rows[i]["IsActive"].ToString() == "Y" ? true : false;
            chkApproval.Checked = objDT.Rows[i]["Approval"].ToString() == "Y" ? true : false;

            //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "total(); ", true);

            txtBasic.Focus();
            SalApproved();
        }
        protected bool formValidation()
        {
            if(ddlMon.SelectedIndex==0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Month'); ", true);
                return false;
            }
            if (ddlYear.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Year'); ", true);
                return false;
            }
            if (txtBasic.Text=="" || txtBasic.Text=="0")
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter Valid Basic Salary'); ", true);
                return false;
            }
            return true;
        }

        protected void txtEmpCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if(txtEmpCode.Text!="")
                {
                    string strQry = "select * from M_Emp where OrgId=" + Convert.ToInt32(Session["OrgID"]) + " and Employeecd='" + txtEmpCode.Text + "'";
                    DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                    if(objDT.Rows.Count>0)
                    {
                       
                        txtEmpName.Text = objDT.Rows[0]["Employeename"].ToString();
                        if (objDT.Rows[0]["HRAApplicable"].ToString() == "Y")
                            txtHRA.ReadOnly = false;
                        if (objDT.Rows[0]["PFApplicable"].ToString() == "Y")
                        {
                            txtPfEmpPct.ReadOnly = false;
                            txtPFMan.ReadOnly = false;
                        }
                        ddlMon.Focus();
                        BindGrid();
                    }
                    else
                    {
                      
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Wrong Employee Code'); ", true);
                        return;
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

       
    }
}