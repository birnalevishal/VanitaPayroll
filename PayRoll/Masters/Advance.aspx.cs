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
    public partial class Advance : System.Web.UI.Page
    {
        SqlConnection sqlConn = null;
        SqlCommand sqlCmd = null;
        SqlTransaction sqlTrans = null;

        public string monthYear = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                clearControls();
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
            ddlYear.Items.Insert(0, new ListItem("Select", "00"));

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    if (btnSave.Text == "Save")
                    {
                        InsertRecord();
                    }
                    else if (btnSave.Text == "Update")
                    {
                        UpdateRecord();
                        btnSave.Text = "Save";
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
            bool result = false;
            try
            {
                string strQry = "";
                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);

                string strQry1 = "INSERT INTO T_Advance(OrgId,Employeecd,AdvApplicationDate,Advamount,SanctionDate,DeductionAmt,DeductionFrom) values(@OrgId,@Employeecd,@AdvApplicationDate,@Advamount,@SanctionDate, @DeductionAmt, @DeductionFrom)";
                SqlParameter[] para = new SqlParameter[7];
                para[0] = new SqlParameter("@OrgID", Session["OrgId"].ToString());
                para[1] = new SqlParameter("@Employeecd", txtEmpCode.Text);
                para[2] = new SqlParameter("@AdvApplicationDate", Convert.ToDateTime(txtApplicationDate.Text).ToString("dd MMM yyyy"));
                if(txtSanctionDt.Text!="")
                    para[3] = new SqlParameter("@SanctionDate", Convert.ToDateTime(txtSanctionDt.Text).ToString("dd MMM yyyy"));
                else
                    para[3] = new SqlParameter("@SanctionDate", (object)DBNull.Value);
                para[4] = new SqlParameter("@Advamount", txtAmount.Text);
                para[5] = new SqlParameter("@DeductionAmt", txtDeduction.Text);
                para[6] = new SqlParameter("@DeductionFrom", ddlMon.SelectedValue+ddlYear.SelectedValue);

                result = SqlHelper.ExecuteNonQuery(strQry1, para, AppGlobal.strConnString);

                if (result)
                {
                    strQry = "";
                    strQry = "INSERT INTO T_Log(OrgId,docdate, Employeecd, MenuId, Mode, Amount, Computername) VALUES(@OrgId,@docdate, @Employeecd, @MenuId, @Mode, @Amount, @Computername)";

                    SqlParameter[] paraLog = new SqlParameter[7];
                    paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                    paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                    paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                    paraLog[3] = new SqlParameter("@Mode", "A");
                    paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());
                    paraLog[5] = new SqlParameter("@Amount", txtAmount.Text);
                    paraLog[6] = new SqlParameter("@docdate", Convert.ToDateTime(txtApplicationDate.Text).ToString("dd MMM yyyy"));

                    result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, paraLog);
                    if (result)
                    {
                        sqlTrans.Commit();
                        //FillGrid();
                        //txtAmount.Text = "";
                        //txtSanctionDt.Text = "";
                        //txtApplicationDate.Text = "";
                        //txtDeduction.Text = "";
                        //ddlMon.SelectedIndex = 0;
                        //ddlYear.SelectedIndex = 0;
                        //txtEmpCode.Text = "";
                        //txtEmpName.Text = "";
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
            bool result = false;
            try
            {
                string strQry = "";
                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);
                if(txtSanctionDt.Text=="")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter Sanction Date'); ", true);
                    return;
                }

                string strQry1 = "update T_Advance set Advamount=" + txtAmount.Text + ",AdvApplicationDate='" + Convert.ToDateTime(txtApplicationDate.Text).ToString("dd MMM yyyy") + "', SanctionDate='" + Convert.ToDateTime(txtSanctionDt.Text).ToString("dd MMM yyyy") + "', DeductionAmt=" + Convert.ToDouble(txtDeduction.Text) + " , DeductionFrom='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'  where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and Employeecd='" + txtEmpCode.Text + "' and AdvApplicationDate='" + Convert.ToDateTime(ViewState["AppDate"]).ToString("dd MMM yyyy") + "'";
                result = SqlHelper.ExecuteNonQuery(strQry1, AppGlobal.strConnString);
                if (result)
                {
                    strQry = "";
                    strQry = "INSERT INTO T_Log(OrgId,docdate, Employeecd, MenuId, Mode, Amount, Computername) VALUES(@OrgId,@docdate, @Employeecd, @MenuId, @Mode, @Amount, @Computername)";

                    SqlParameter[] paraLog = new SqlParameter[7];
                    paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                    paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                    paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                    paraLog[3] = new SqlParameter("@Mode", "U");
                    paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());
                    paraLog[5] = new SqlParameter("@Amount", txtAmount.Text);
                    paraLog[6] = new SqlParameter("@docdate", Convert.ToDateTime(txtApplicationDate.Text).ToString("dd MMM yyyy"));

                    result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, paraLog);
                    if (result)
                    {
                        sqlTrans.Commit();
                        //FillGrid();
                        //txtAmount.Text = "";
                        //txtSanctionDt.Text = "";
                        //txtApplicationDate.Text = "";
                        //txtDeduction.Text = "";
                        //ddlMon.SelectedIndex = 0;
                        //ddlYear.SelectedIndex = 0;
                        //txtEmpCode.Text = "";
                        //txtEmpName.Text = "";
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

        protected void Gridview1_RowCommand(object sender, GridViewCommandEventArgs e)
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
            GridViewRow row = Gridview1.Rows[i];
            string dt = row.Cells[2].Text;

            string strQry = "select * from T_AdvanceDeduction where OrgID=" + Convert.ToInt32(Session["OrgId"]) + " and Employeecd='" + txtEmpCode.Text + "' and AdvApplicationDate='" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "'";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if(objDT.Rows.Count>0)
            {
                txtApplicationDate.ReadOnly = true;
                CEtxtApplicationDate.Enabled = false;
            }
            else
            {
                txtApplicationDate.ReadOnly = false;
                CEtxtApplicationDate.Enabled = true;
            }
            strQry = "SELECT * from T_Advance where OrgId=" + Convert.ToInt32(Session["OrgId"]) + " and Employeecd='" + txtEmpCode.Text + "' and AdvApplicationDate='" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "'";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (objDT.Rows.Count > 0)
            {
                txtApplicationDate.Text = Convert.ToDateTime(objDT.Rows[0]["AdvApplicationDate"]).ToString("dd/MM/yyyy");
                txtSanctionDt.Text = Convert.ToDateTime(objDT.Rows[0]["SanctionDate"]).ToString("dd/MM/yyyy");
                txtAmount.Text = Convert.ToDouble(objDT.Rows[0]["Advamount"]).ToString();
                txtDeduction.Text = objDT.Rows[0]["DeductionAmt"] != DBNull.Value ? Convert.ToDouble(objDT.Rows[0]["DeductionAmt"]).ToString() : "";
                ddlMon.SelectedValue = objDT.Rows[0]["DeductionFrom"] != DBNull.Value ? objDT.Rows[0]["DeductionFrom"].ToString().Substring(0, 2) : "00";

                ddlYear.SelectedValue = objDT.Rows[0]["DeductionFrom"] != DBNull.Value ? objDT.Rows[0]["DeductionFrom"].ToString().Substring(2, 4) : "00";
                ViewState["AppDate"] = Convert.ToDateTime(objDT.Rows[0]["AdvApplicationDate"]).ToString("dd/MM/yyyy");
            }

            txtEmpCode.ReadOnly = true;
            btnSave.Text = "Update";
        }

        private void clearControls()
        {
            txtEmpCode.ReadOnly = false;
            txtEmpCode.Text = "";
            txtEmpName.Text = "";
            txtApplicationDate.Text = "";
            txtAmount.Text = "";
            txtSanctionDt.Text = "";
            txtDeduction.Text = "";
            ddlMon.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;
            btnSave.Text = "Save";
            txtEmpCode.Focus();
            Gridview1.DataSource = null;
            Gridview1.DataBind();
            txtApplicationDate.ReadOnly = false;
            CEtxtApplicationDate.Enabled = true;
        }

        protected void txtEmpCode_TextChanged(object sender, EventArgs e)
        {
            if (txtEmpCode.Text != "")
            {
                string strQry = "select * from M_Emp where OrgID=" + Convert.ToInt32(Session["OrgId"]) + " and Employeecd='" + txtEmpCode.Text + "'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    txtEmpName.Text = objDT.Rows[0]["EmployeeName"].ToString();
                    FillGrid();
                    
                    txtAmount.Focus();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Wrong Employee Code'); ", true);
                    return;
                }
            }
        }

        protected void FillGrid()
        {
            if (txtEmpCode.Text != "")
            {
                string strQry = "select convert(varchar, AdvApplicationDate, 103) as AdvApplicationDate,Advamount,convert(varchar, SanctionDate, 103) as SanctionDate,DeductionAmt,DeductionFrom from T_Advance where OrgId=" + Convert.ToInt32(Session["OrgId"]) + " and Employeecd='" + txtEmpCode.Text + "'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    Gridview1.DataSource = objDT;
                    Gridview1.DataBind();
                    ViewState["CurrentTable"] = objDT;
                }
            }
        }
    }
}