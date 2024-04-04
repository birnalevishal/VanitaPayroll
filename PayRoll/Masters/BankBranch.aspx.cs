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
    public partial class BankBranch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGrid();
                BindData();
            }
            txtbranch.Focus();
        }

        private void BindData()
        {
            string strQry = "SELECT BankName, BankCd FROM M_Bank Where IsActive='Y' ORDER BY BankName";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlBank.DataSource = objDT;
            ddlBank.DataTextField = "BankName";
            ddlBank.DataValueField = "BankCd";
            ddlBank.DataBind();
        }

        private void BindGrid()
        {
            string strQry = "select bb.BankBranchId, bb.IFSCCode as IFSCCode,bb.BankBranch as BankBranch, b.BankName,  b.BankCd, bb.IsActive as IsActive from M_BankBranch bb " +
                          " join M_Bank b on bb.BankCd = b.BankCd ORDER BY BankName,BankBranch";
                          
            //strQry = "select * from M_BankBranch ";
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
            DataTable objDT;

            strQry = "SELECT * FROM M_BankBranch Where IFSCCode='" + txtIFSC.Text + "'";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if(objDT.Rows.Count>0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('IFSC Code Already Exists!'); ", true);
                return;
            }

            //strQry = "SELECT * FROM M_BankBranch Where BankCd=" + ddlBank.SelectedValue + " and BankBranch='" + txtbranch.Text + "'";
            //objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            //if (objDT.Rows.Count > 0)
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Branch Name Already Exists!'); ", true);
            //    return;
            //}

            //strQry = "SELECT Count(*) FROM M_BankBranch Where BankCd=" + ddlBank.SelectedValue + " and IFSCCode='" + txtIFSC.Text + "' and BankBranch='" + txtbranch.Text + "'";
            //int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            //if (nCnt > 0)
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Bank Branch already exists!'); ", true);
            //    clearControls();
            //    return;
            //}

            strQry = "INSERT INTO M_BankBranch(BankBranchId,BankCd, BankBranch, IFSCCode,IsActive) VALUES(@BankBranchId,@BankCd, @BankBranch, @IFSCCode, @IsActive)";
            int nID = SqlHelper.GetMaxID("M_BankBranch", "BankBranchId", AppGlobal.strConnString);

            SqlParameter[] para = new SqlParameter[5];
            para[0] = new SqlParameter("@BankBranchId", nID);
            para[1] = new SqlParameter("@BankCd", ddlBank.SelectedValue);
            para[2] = new SqlParameter("@BankBranch", txtbranch.Text);
            para[3] = new SqlParameter("@IFSCCode", txtIFSC.Text);
            para[4] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");

            result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

            if (result)
            {
                clearControls();
                BindGrid();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
            }
        }

        private void UpdateRecord()
        {
            string strQry = "";
            bool result = false;
            int nId = Convert.ToInt32(ViewState["ID"]);
            DataTable objDT;

            //strQry = "SELECT * FROM M_BankBranch Where IFSCCode='" + txtIFSC.Text + "'";
            //objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            //if (objDT.Rows.Count > 0)
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('IFSC Code Already Exists!'); ", true);
            //    return;
            //}

            //strQry = "SELECT * FROM M_BankBranch Where BankCd=" + ddlBank.SelectedValue + " and BankBranch='" + txtbranch.Text + "'";
            //objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            //if (objDT.Rows.Count > 0)
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Branch Name Already Exists!'); ", true);
            //    return;
            //}

            //strQry = "SELECT Count(*) FROM M_BankBranch Where BankCd=" + ddlBank.SelectedValue + " and IFSCCode='" + txtIFSC.Text + "' and BankBranch='" + txtbranch.Text + "'";
            //int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            //if (nCnt > 0)
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Bank Branch already exists!'); ", true);
            //    clearControls();
            //    return;
            //}

            strQry = "UPDATE M_BankBranch SET BankBranch=@BankBranch, BankCd=@BankCd, IFSCCode=@IFSCCode, IsActive=@IsActive WHERE BankBranchId=@BankBranchId";
            SqlParameter[] para = new SqlParameter[5];
            para[0] = new SqlParameter("@BankBranchId", nId);
            para[1] = new SqlParameter("@BankCd", ddlBank.SelectedValue);
            para[2] = new SqlParameter("@BankBranch", txtbranch.Text);
            para[3] = new SqlParameter("@IFSCCode", txtIFSC.Text);
            para[4] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");


            result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

            if (result)
            {
                clearControls();
                BindGrid();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
            }
        }

        private void clearControls()
        {
            txtbranch.Text = "";
            txtIFSC.Text= "";
            chkIsActive.Checked = true;
            btnSave.Text = "Save";
            ViewState["ID"] = null;
            txtbranch.Focus();
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
                    txtbranch.Focus();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ViewRecord(int i)
        {
            DataTable objDT = (DataTable)ViewState["objDTList"];

            ViewState["ID"] = objDT.Rows[i]["BankBranchId"].ToString();
            ddlBank.SelectedValue = objDT.Rows[i]["BankCd"].ToString();
            txtbranch.Text = objDT.Rows[i]["BankBranch"].ToString();
            txtIFSC.Text = objDT.Rows[i]["IFSCCode"].ToString();
            chkIsActive.Checked = objDT.Rows[i]["IsActive"].ToString() == "Y" ? true : false;
        }

        protected bool formValidation()
        {
            int nID = 0;
            if (ViewState["ID"] != null)
            {
                nID = Convert.ToInt32(ViewState["ID"]);
            }

            string strQry = "SELECT Count(*) FROM M_BankBranch Where BankBranchId<>" + nID + " and IFSCCode='" + txtIFSC.Text + "'";
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('IFSC Code Already Exists!'); ", true);
                clearControls();
                return false;
            }
            strQry = "SELECT Count(*) FROM M_BankBranch Where BankBranchId<>" + nID + " and BankCd=" + ddlBank.SelectedValue + " and BankBranch='" + txtbranch.Text + "'";
            nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Branch Name Already Exists!'); ", true);
                return false;
            }

            strQry = "SELECT Count(*) FROM M_BankBranch Where BankBranchId<>" + nID + " and BankCd = " + ddlBank.SelectedValue + " and IFSCCode='" + txtIFSC.Text + "' and BankBranch='" + txtbranch.Text + "'";
            nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Bank Branch Already Exists!'); ", true);
                clearControls();
                return false;
            }

            return true;
        }
    }
}