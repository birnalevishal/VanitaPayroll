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
    public partial class Bank : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGrid();      
            }
            txtBank.Focus();
        }

        private void BindData()
        {
            //string strQry = "SELECT Country, CountryCd FROM M_Country Where IsActive='Y' ORDER BY Country";
            //DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            //ddlContry.DataSource = objDT;
            //ddlContry.DataTextField = "Country";
            //ddlContry.DataValueField = "CountryCd";
            //ddlContry.DataBind();
            
        }
        private void BindGrid()
        {
            string strQry = "select * from M_Bank order by BankName ";
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
            int nID = 0;
            
            //strQry = "SELECT Count(*) FROM M_Bank Where BankCd<>"+ nID + " and BankName='" + txtBank.Text.Trim() + "'";
            //int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            //if (nCnt > 0)
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Bank Name already exists!'); ", true);
            //    clearControls();
            //    return;
            //}
            nID = SqlHelper.GetMaxID("M_Bank", "BankCd", AppGlobal.strConnString);
            strQry = "INSERT INTO M_Bank(BankCd, BankName, Address,Panno, IsActive) VALUES(@BankCd, @BankName, @Address,@Panno, @IsActive)";
          
            SqlParameter[] para = new SqlParameter[5];
            para[0] = new SqlParameter("@BankCd", nID);
            para[1] = new SqlParameter("@BankName", txtBank.Text);
            para[2] = new SqlParameter("@Address", txtAddress.Text);
            para[3] = new SqlParameter("@Panno", txtPANNo.Text.ToUpper());
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
            
            strQry = "UPDATE M_Bank SET BankName=@BankName, Address=@Address,Panno=@Panno, IsActive=@IsActive WHERE BankCd=@BankCd";
            SqlParameter[] para = new SqlParameter[5];
            para[0] = new SqlParameter("@BankCd", nId);
            para[1] = new SqlParameter("@BankName", txtBank.Text);
            para[2] = new SqlParameter("@Address", txtAddress.Text);
            para[3] = new SqlParameter("@Panno", txtPANNo.Text.ToUpper());
            para[4] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");

            result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

            if (result)
            {
                clearControls();
                BindGrid();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
            }
        }

        protected bool formValidation()
        {
            int nID = 0;
            if(ViewState["ID"]!=null)
            {
                nID = Convert.ToInt32(ViewState["ID"]);
            }
            string strQry = "SELECT Count(*) FROM M_Bank Where BankCd<>" + nID + " and BankName='" + txtBank.Text.Trim() + "'";
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Bank Name already exists!'); ", true);
                clearControls();
                return false;
            }
            return true;
        }
        private void clearControls()
        {
            txtBank.Text = "";
            txtAddress.Text= "";
            txtPANNo.Text = "";
            chkIsActive.Checked = true;
            btnSave.Text = "Save";
            ViewState["ID"] = null;
            txtBank.Focus();
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
                    txtBank.Focus();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ViewRecord(int i)
        {
            DataTable objDT = (DataTable)ViewState["objDTList"];

            ViewState["ID"] = objDT.Rows[i]["BankCd"].ToString();
            txtBank.Text = objDT.Rows[i]["BankName"].ToString();
            txtAddress.Text= objDT.Rows[i]["Address"].ToString();
            txtPANNo.Text = objDT.Rows[i]["Panno"].ToString();
            chkIsActive.Checked = objDT.Rows[i]["IsActive"].ToString() == "Y" ? true : false;
        }
    }
}