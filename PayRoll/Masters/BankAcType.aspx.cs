using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SqlClient;
using System.Data;
using System.Data.SqlClient;

namespace PayRoll.Masters
{
    public partial class BankAcType : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGrid();
            }
            txtName.Focus();
        }

        private void BindGrid()
        {
            string strQry = "SELECT * FROM M_BankActType Order By ActType";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvAcTypeList.DataSource = objDT;
            gvAcTypeList.DataBind();

            ViewState["objDTDesig"] = objDT;
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

            //strQry = "SELECT Count(*) FROM M_BankActType Where ActType='" + txtName.Text.Trim() + "'";
            //int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            //if (nCnt > 0)
            //{
            //    //alertMessage("Duplicate Entry, Please Check");
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Bank A/c Type already exists!'); ", true);
            //    clearControls();
            //    return;
            //}

            strQry = "INSERT INTO M_BankActType(ActTypecd, ActType, IsActive) VALUES(@ActTypecd, @ActType, @IsActive)";
            int nID = SqlHelper.GetMaxID("M_BankActType", "ActTypecd", AppGlobal.strConnString);

            SqlParameter[] para = new SqlParameter[3];
            para[0] = new SqlParameter("@ActTypecd", nID);
            para[1] = new SqlParameter("@ActType", txtName.Text);
            para[2] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");

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
            int nId = Convert.ToInt32(ViewState["CID"]);

            strQry = "UPDATE M_BankActType SET ActType=@ActType, IsActive=@IsActive WHERE ActTypecd=@ActTypecd";
            SqlParameter[] para = new SqlParameter[3];
            para[0] = new SqlParameter("@ActTypecd", nId);
            para[1] = new SqlParameter("@ActType", txtName.Text);
            para[2] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");

            result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

            if (result)
            {
                clearControls();
                BindGrid();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
                //alertMessage("Your details have been saved successfully.");
            }
        }

        private void clearControls()
        {
            txtName.Text = "";
            chkIsActive.Checked = true;
            btnSave.Text = "Save";
            ViewState["CID"] = null;
            txtName.Focus();
        }

        protected void gvAcTypeList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAcTypeList.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvAcTypeList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Show")
                {
                    int i = Convert.ToInt32(e.CommandArgument);
                    ViewRecord(i);
                    btnSave.Text = "Update";
                    txtName.Focus();
                }

            }
            catch (Exception ex)
            {

            }
        }
     
        private void ViewRecord(int i)
        {
            DataTable objDT = (DataTable)ViewState["objDTDesig"];

            ViewState["CID"] = objDT.Rows[i]["ActTypecd"].ToString();
            txtName.Text = objDT.Rows[i]["ActType"].ToString();
            chkIsActive.Checked = objDT.Rows[i]["IsActive"].ToString() == "Y" ? true : false;
        }

        private void alertMessage(string str)
        {
            //Display success message.
            string message = str;
            string script = "window.onload = function(){ alert('";
            script += message;
            script += "')};";
            ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
        }

        protected bool formValidation()
        {
            int nID = 0;
            if (ViewState["CID"] != null)
            {
                nID = Convert.ToInt32(ViewState["CID"]);
            }
            string strQry = "SELECT Count(*) FROM M_BankActType Where ActTypecd<>" + nID + " and ActType='" + txtName.Text.Trim() + "'";
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Bank A/c Type Already Exists!'); ", true);
                clearControls();
                return false;
            }
            return true;
        }
    }
}