using SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PayRoll.Masters
{
    public partial class AllowanceConfig : System.Web.UI.Page
    {
        SqlConnection sqlConn = null;
        SqlCommand sqlCmd = null;
        SqlTransaction sqlTrans = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
                clearControls();
            }
            AllowanceApproved();
            txtEmpCode.Focus();
        }

        protected void AllowanceApproved()
        {
            if (txtEmpCode.Text != "" && txtDocDate.Text!="")
            {
                string strQry = "SELECT isnull(Approval,'N') as Approval FROM M_AllowanceConfig where orgID=" + Convert.ToInt32(Session["OrgID"]) + " and Employeecd='" + txtEmpCode.Text + "' and Docdate='" + Convert.ToDateTime(txtDocDate.Text).ToString("dd MMM yyyy")+ "'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    if (objDT.Rows[0]["Approval"].ToString() == "Y")
                    {
                        txtDA.ReadOnly = true;
                        txtPhone.ReadOnly = true;
                        txtLodge.ReadOnly = true;
                        txtOther.ReadOnly = true;
                        txtRemark.ReadOnly = true;
                        txtDocDate.ReadOnly = true;
                        txtTA.ReadOnly = true;
                    }
                    else
                    {
                        txtDA.ReadOnly = false;
                        txtPhone.ReadOnly = false;
                        txtLodge.ReadOnly = false;
                        txtOther.ReadOnly = false;
                        txtRemark.ReadOnly = false;
                        txtDocDate.ReadOnly = false;
                        txtTA.ReadOnly = false;
                    }
                }
            }
        }

        private void clearControls()
        {
            txtEmpCode.Text = "";
            txtEmpName.Text = "";
            txtDocDate.Text = "";
            txtTA.Text = "";
            txtDA.Text = "";
            txtPhone.Text = "";
            txtLodge.Text = "";
            btnSave.Text = "Save";
            txtEmpCode.ReadOnly = false;
            txtEmpCode.Focus();
            txtOther.Text = "";
            txtRemark.Text = "";
            txtTotal.Text = "";
            gvList.DataSource = null;
            gvList.DataBind();
            chkApproval.Checked = false;

            txtDA.ReadOnly = false;
            txtPhone.ReadOnly = false;
            txtLodge.ReadOnly = false;
            txtOther.ReadOnly = false;
            txtRemark.ReadOnly = false;
            txtDocDate.ReadOnly = false;
            txtTA.ReadOnly = false;

            string strQry = "select * from M_Authority where OrgId=" + Convert.ToInt32(Session["OrgID"]) + " and Employeecd='" + Session["UserName"].ToString() + "' and FormCode=61";
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
        private void BindData()
        {
           
        }
        private void BindGrid()
        {
            string strQry = @"select allowConfig.Docdate, allowConfig.TA , allowConfig.DA,  allowConfig.phone,allowConfig.lodge,allowConfig.other,allowConfig.remark, isnull(allowConfig.Approval,'N') as Approval
                                from M_AllowanceConfig allowConfig where allowConfig.Employeecd ='" + txtEmpCode.Text + "' and allowConfig.OrgId=" + Convert.ToInt32(Session["OrgID"]) + " order by Docdate desc";

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
                        string monthYear = "";

                        int month = Convert.ToDateTime(txtDocDate.Text).Month;
                        int year = Convert.ToDateTime(txtDocDate.Text).Year;

                        monthYear =  year + month.ToString("00") ;

                        string strQry = "select * from T_SalaryLock where OrgId=" + Convert.ToInt16(Session["orgID"]) + " and RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)>='" + monthYear + "' and Lock='Y'";
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
                            strQry = "select  max(docdate) as DocDate from M_AllowanceConfig where OrgId=" + Convert.ToInt16(Session["orgID"]) + " and Employeecd='" + txtEmpCode.Text + "'";
                            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                            if (objDT.Rows.Count > 0)
                            {
                                if (objDT.Rows[0]["DocDate"] != DBNull.Value)
                                {
                                    if (Convert.ToDateTime(txtDocDate.Text) > Convert.ToDateTime(objDT.Rows[0]["DocDate"]))
                                    {
                                        clearControls();
                                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Cant Modify Document Date'); ", true);
                                        return;
                                    }
                                }
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
            string strQry = "";
            bool result = false;
            try
            {
                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);

                strQry = @"DELETE FROM M_AllowanceConfig  WHERE OrgId=" + Convert.ToInt32(Session["OrgID"]) + " AND  Employeecd='" + txtEmpCode.Text + "' and Docdate='" + Convert.ToDateTime(txtDocDate.Text).ToString("dd MMM yyyy") + "'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);

                strQry = @"INSERT INTO M_AllowanceConfig(OrgId, Employeecd, Docdate, MonYrcd, TA, DA, phone, lodge, other, remark, Approval) 
                                             VALUES(@OrgId, @Employeecd, @Docdate, @MonYrcd,@TA, @DA, @phone, @lodge, @other, @remark, @Approval)";

                SqlParameter[] para = new SqlParameter[11];
                para[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                para[1] = new SqlParameter("@Employeecd", txtEmpCode.Text);
                para[2] = new SqlParameter("@Docdate", Convert.ToDateTime(txtDocDate.Text).ToString("dd MMM yyyy"));
                string monthYear = "";
                int month = Convert.ToDateTime(txtDocDate.Text).Month;
                int year = Convert.ToDateTime(txtDocDate.Text).Year;
                monthYear = month.ToString("00") + year;
                para[3] = new SqlParameter("@MonYrcd", monthYear);

                para[4] = new SqlParameter("@TA", txtTA.Text != "" ? txtTA.Text : "0");
                para[5] = new SqlParameter("@DA", txtDA.Text != "" ? txtDA.Text : "0");
                para[6] = new SqlParameter("@phone", txtPhone.Text != "" ? txtPhone.Text : "0");
                para[7] = new SqlParameter("@lodge", txtLodge.Text != "" ? txtLodge.Text : "0");
                para[8] = new SqlParameter("@other", txtOther.Text != "" ? txtOther.Text : "0");
                para[9] = new SqlParameter("@remark", txtRemark.Text != "" ? txtRemark.Text : "0");
                para[10] = new SqlParameter("@Approval", chkApproval.Checked ? "Y" : "N");

                result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

                if (result)
                {
                    ViewState["EmpCode"] = txtEmpCode.Text;
                    ViewState["EmpName"] = txtEmpName.Text;

                    txtEmpCode.Text = ViewState["EmpCode"].ToString();
                    txtEmpName.Text = ViewState["EmpName"].ToString();
                   
                    double totalAllow = 0;
                    if (txtTA.Text != "")
                        totalAllow = Convert.ToDouble(txtTA.Text);
                    if (txtDA.Text != "")
                        totalAllow += Convert.ToDouble(txtDA.Text);
                    if (txtPhone.Text != "")
                        totalAllow += Convert.ToDouble(txtPhone.Text);
                    if (txtLodge.Text != "")
                        totalAllow += Convert.ToDouble(txtLodge.Text);
                    if (txtOther.Text != "")
                        totalAllow += Convert.ToDouble(txtOther.Text);

                    strQry = "";
                    strQry = "INSERT INTO T_Log(OrgId,docdate, Employeecd, MenuId, Mode, Computername, amount) VALUES(@OrgId,@docdate, @Employeecd, @MenuId, @Mode, @Computername,@amount)";

                    SqlParameter[] paraLog = new SqlParameter[7];
                    paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                    paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                    paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                    paraLog[3] = new SqlParameter("@Mode", "A");
                    paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());
                    paraLog[5] = new SqlParameter("@amount", totalAllow);
                    paraLog[6] = new SqlParameter("@docdate", Convert.ToDateTime(txtDocDate.Text).ToString("dd MMM yyyy"));

                    result = SqlHelper.ExecuteNonQuery(strQry, paraLog, AppGlobal.strConnString);
                    if (result)
                    {
                        sqlTrans.Commit();
                        clearControls();
                        BindGrid();
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
                string strQry = "";
                bool result = false;
                int nId = Convert.ToInt32(ViewState["ID"]);

                string monthYear = "";

                int month = Convert.ToDateTime(txtDocDate.Text).Month;
                int year = Convert.ToDateTime(txtDocDate.Text).Year;

                monthYear = month.ToString("00")+ year;

                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);

                strQry = "UPDATE M_AllowanceConfig SET Docdate=@Docdate, MonYrcd=@MonYrcd, TA=@TA, DA=@DA, phone=@phone, lodge=@lodge, other=@other, remark=@remark,Approval=@Approval WHERE OrgId=" + Convert.ToInt32(Session["orgID"]) + " and Employeecd='" + txtEmpCode.Text + "' and docDate = '" + Convert.ToDateTime(ViewState["DocDate"].ToString()).ToString("dd MMM yyyy") + "'";
                SqlParameter[] para = new SqlParameter[11];

                para[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                para[1] = new SqlParameter("@Employeecd", txtEmpCode.Text);
                para[2] = new SqlParameter("@Docdate", Convert.ToDateTime(txtDocDate.Text).ToString("dd MMM yyyy"));
                para[3] = new SqlParameter("@MonYrcd", monthYear);
                para[4] = new SqlParameter("@TA", txtTA.Text != "" ? txtTA.Text : "0");
                para[5] = new SqlParameter("@DA", txtDA.Text != "" ? txtDA.Text : "0");
                para[6] = new SqlParameter("@phone", txtPhone.Text != "" ? txtPhone.Text : "0");
                para[7] = new SqlParameter("@lodge", txtLodge.Text != "" ? txtLodge.Text : "0");
                para[8] = new SqlParameter("@other", txtOther.Text != "" ? txtOther.Text : "0");
                para[9] = new SqlParameter("@remark", txtRemark.Text != "" ? txtRemark.Text : "0");
                para[10] = new SqlParameter("@Approval", chkApproval.Checked ? "Y" : "N");

                result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

                if (result)
                {
                    double totalAllow = 0;
                    if (txtTA.Text != "")
                        totalAllow = Convert.ToDouble(txtTA.Text);
                    if (txtDA.Text != "")
                        totalAllow += Convert.ToDouble(txtDA.Text);
                    if (txtPhone.Text != "")
                        totalAllow += Convert.ToDouble(txtPhone.Text);
                    if (txtLodge.Text != "")
                        totalAllow += Convert.ToDouble(txtLodge.Text);
                    if (txtOther.Text != "")
                        totalAllow += Convert.ToDouble(txtOther.Text);

                    strQry = "";
                    strQry = "INSERT INTO T_Log(OrgId, docdate, Employeecd, MenuId, Mode, Computername, amount) VALUES(@OrgId,@docdate, @Employeecd, @MenuId, @Mode, @Computername,@amount)";

                    SqlParameter[] paraLog = new SqlParameter[7];
                    paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                    paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                    paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                    paraLog[3] = new SqlParameter("@Mode", "U");
                    paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());
                    paraLog[5] = new SqlParameter("@amount", totalAllow);
                    paraLog[6] = new SqlParameter("@docdate", Convert.ToDateTime(txtDocDate.Text).ToString("dd MMM yyyy"));

                    result = SqlHelper.ExecuteNonQuery(strQry, paraLog, AppGlobal.strConnString);
                    if (result)
                    {
                        sqlTrans.Commit();
                        clearControls();
                        BindGrid();
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
                    txtEmpCode.Focus();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ViewRecord(int i)
        {
            GridViewRow row = gvList.Rows[i];

            string dt = row.Cells[1].Text; 
            
            string strQry = "SELECT * FROM M_AllowanceConfig Where orgID=" + Convert.ToInt32(Session["OrgID"]) + " and Employeecd=" + txtEmpCode.Text + "  and Docdate='" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "'";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (objDT.Rows.Count > 0)
            {
                txtDocDate.Text = Convert.ToDateTime(objDT.Rows[0]["Docdate"]).ToString("dd/MM/yyyy");
                ViewState["DocDate"]= Convert.ToDateTime(objDT.Rows[0]["Docdate"]).ToString("dd/MM/yyyy");
                txtDA.Text = objDT.Rows[0]["DA"] != DBNull.Value ? Convert.ToDouble(objDT.Rows[0]["DA"]).ToString("0.00") : "0";
                txtTA.Text = objDT.Rows[0]["TA"] != DBNull.Value ? Convert.ToDouble(objDT.Rows[0]["TA"]).ToString("0.00") : "0";
                txtPhone.Text = objDT.Rows[0]["phone"] !=DBNull.Value ? Convert.ToDouble(objDT.Rows[0]["phone"]).ToString("0.00") : "0";
                txtLodge.Text = objDT.Rows[0]["lodge"] !=DBNull.Value ? Convert.ToDouble(objDT.Rows[0]["lodge"]).ToString("0.00") : "0";
                txtOther.Text = objDT.Rows[0]["other"] !=DBNull.Value ? Convert.ToDouble(objDT.Rows[0]["other"]).ToString("0.00") : "0";
                txtRemark.Text = objDT.Rows[0]["remark"].ToString();
                chkApproval.Checked = objDT.Rows[0]["Approval"].ToString() == "Y" ? true : false;
                Total();
                AllowanceApproved();
            }
            
            txtEmpCode.ReadOnly = true;
            btnSave.Text = "Update";
        }

        protected bool formValidation()
        {
           
            if (btnSave.Text == "Save")
            {
                string strQry = "SELECT Count(*) FROM M_AllowanceConfig Where OrgId=" + Convert.ToInt32(Session["OrgID"]) + " and Employeecd=" + txtEmpCode.Text + " and Docdate='" + Convert.ToDateTime(txtDocDate.Text.Trim()).ToString("dd MMM yyyy") + "'";
                int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
                if (nCnt > 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Record Already Exists!'); ", true);
                    //clearControls();
                    return false;
                }
            }
            return true;
        }

        protected void txtEmpCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int nCnt = 0;
                string strQry = "";

                if (txtEmpCode.Text != "")
                {
                    strQry = "SELECT Employeecd,Employeename FROM M_Emp Where OrgId='" + Convert.ToInt32(Session["OrgID"]) + "' and Employeecd='" + txtEmpCode.Text + "'";
                    DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                    if (objDT1.Rows.Count==0)
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Wrong Employee Code'); ", true);
                        clearControls();
                        return;
                    }
                    txtEmpName.Text = objDT1.Rows[0]["EmployeeName"].ToString();
                    txtDocDate.Focus();
                    BindGrid();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void txtTA_TextChanged(object sender, EventArgs e)
        {
            Total();
            txtDA.Focus();
        }

        protected void txtDA_TextChanged(object sender, EventArgs e)
        {
            Total();
            txtPhone.Focus();
        }

        protected void txtPhone_TextChanged(object sender, EventArgs e)
        {
            Total();
            txtLodge.Focus();
        }
        protected void Total()
        {
            double ta=0, da=0, phone=0, total=0;
            if (txtTA.Text != "")
                total = Convert.ToDouble(txtTA.Text);
            if (txtDA.Text != "")
                total += Convert.ToDouble(txtDA.Text);
            if (txtPhone.Text != "")
                total += Convert.ToDouble(txtPhone.Text);
            txtTotal.Text = Convert.ToDouble(total).ToString();

        }
    }
}