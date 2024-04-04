using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SqlClient;
using System.Data.SqlClient;

namespace PayRoll.Masters
{
    public partial class RoleMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGrid();
               
            }
        }

        private void BindGrid()
        {
            string strQry = "SELECT * FROM M_Role As st Order By RoleId";
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

        protected bool formValidation()
        {
            int nID = 0;
            if (ViewState["ID"] != null)
            {
                nID = Convert.ToInt32(ViewState["ID"]);
            }

            string strQry = "SELECT Count(*) FROM M_Role Where RoleName ='" + txtName.Text.Trim() + "' ";
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0 && btnSave.Text == "Save")
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Role Name Already Exists!'); ", true);
                clearControls();
                return false;
            }


            return true;
        }

        private void InsertRecord()
        {
            string strQry = "";
            bool result = false;
          
            strQry = "INSERT INTO M_Role(RoleId, RoleName,SeqNo,form16,PaySlip, IsActive) VALUES(@RoleId, @RoleName,@SeqNo,@form16,@PaySlip,  @IsActive)";
            int nID = SqlHelper.GetMaxID("M_Role", "RoleId", AppGlobal.strConnString);
            int seqNo = SqlHelper.GetMaxID("M_Role", "SeqNo", AppGlobal.strConnString);


            SqlParameter[] para = new SqlParameter[6];
            para[0] = new SqlParameter("@RoleId", nID);
            para[1] = new SqlParameter("@RoleName", txtName.Text);
            para[2] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");
            para[3] = new SqlParameter("@SeqNo", seqNo);
            para[4] = new SqlParameter("@form16", chkForm16.Checked ? "Y" : "N");
            para[5] = new SqlParameter("@PaySlip", chkPaySlip.Checked ? "Y" : "N");

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

            strQry = "UPDATE M_Role SET RoleName=@RoleName, IsActive=@IsActive, form16=@form16,PaySlip=@PaySlip WHERE RoleId=@RoleId";
            SqlParameter[] para = new SqlParameter[5];
            para[0] = new SqlParameter("@RoleId", nId);
            para[1] = new SqlParameter("@RoleName", txtName.Text);
            para[2] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");
            para[3] = new SqlParameter("@form16", chkForm16.Checked ? "Y" : "N");
            para[4] = new SqlParameter("@PaySlip", chkPaySlip.Checked ? "Y" : "N");

            result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

            if (result)
            {
                clearControls();
                BindGrid();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
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
            txtName.Text = "";
            chkIsActive.Checked = true;
            btnSave.Text = "Save";
            chkForm16.Checked = true;
            chkPaySlip.Checked = true;
            ViewState["ID"] = null;
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
                throw ex;
            }
        }

        private void ViewRecord(int i)
        {
            DataTable objDT = (DataTable)ViewState["objDTList"];
            ViewState["ID"] = objDT.Rows[i]["RoleId"].ToString();
            txtName.Text = objDT.Rows[i]["RoleName"].ToString();
            chkIsActive.Checked = objDT.Rows[i]["IsActive"].ToString() == "Y" ? true : false;
            chkForm16.Checked = objDT.Rows[i]["form16"].ToString() == "Y" ? true : false;
            chkPaySlip.Checked = objDT.Rows[i]["PaySlip"].ToString() == "Y" ? true : false;
        }


    }
}