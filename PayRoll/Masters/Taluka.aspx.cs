using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using SqlClient;
using System.Data;


namespace PayRoll.Masters
{
    public partial class Taluka : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                BindData();
                BindGrid();
            }
        }
        private void BindData()
        {
            string strQry = "select District, DistCd from M_District where IsActive ='Y'";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlList.DataSource = objDT;
            ddlList.DataTextField = "District";
            ddlList.DataValueField = "DistCd";
            ddlList.DataBind();
        }
        private void BindGrid()
        {
            string strQry = "SELECT *, (Select District FROM M_District Where DistCd=tal.DistCd) As District FROM M_Taluka As tal Order By Taluka";
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
                throw ex;
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
                throw ex;
            }
        }

        private void InsertRecord()
        {
            string strQry = "";
            bool result = false;

            //strQry = "SELECT Count(*) FROM M_Taluka Where Taluka='" + txtName.Text.Trim() + "'";
            //int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            //if (nCnt > 0)
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Taluka Name already exists!'); ", true);
            //    clearControls();
            //    return;
            //}

            strQry = "INSERT INTO M_Taluka(TalCd, Taluka, DistCd, IsActive) VALUES(@TalCd, @Taluka, @DistCd, @IsActive)";
            int nID = SqlHelper.GetMaxID("M_Taluka", "TalCd", AppGlobal.strConnString);

            SqlParameter[] para = new SqlParameter[4];
            para[0] = new SqlParameter("@TalCd", nID);
            para[1] = new SqlParameter("@Taluka", txtName.Text);
            para[2] = new SqlParameter("@DistCd", ddlList.SelectedValue);
            para[3] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");

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

            strQry = "UPDATE M_Taluka SET Taluka=@Taluka, DistCd=@DistCd, IsActive=@IsActive WHERE TalCd=@TalCd";
            SqlParameter[] para = new SqlParameter[4];
            para[0] = new SqlParameter("@Taluka", txtName.Text);
            para[1] = new SqlParameter("@DistCd", ddlList.SelectedValue);
            para[2] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");
            para[3] = new SqlParameter("@TalCd", nId);

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
            txtName.Text = "";
            ddlList.SelectedIndex = 0;
            chkIsActive.Checked = true;
            btnSave.Text = "Save";
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
            ViewState["ID"] = objDT.Rows[i]["TalCd"].ToString();
            txtName.Text = objDT.Rows[i]["Taluka"].ToString();
            ddlList.SelectedValue = objDT.Rows[i]["DistCd"].ToString();
            chkIsActive.Checked = objDT.Rows[i]["IsActive"].ToString() == "Y" ? true : false;
        }
        protected bool formValidation()
        {
            int nID = 0;
            if (ViewState["ID"] != null)
            {
                nID = Convert.ToInt32(ViewState["ID"]);
            }

            string strQry = "SELECT Count(*) FROM M_Taluka Where TalCd<>" + nID + " and Taluka ='" + txtName.Text.Trim() + "' ";
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Taluka is Already Exists!'); ", true);
                clearControls();
                return false;
            }


            return true;
        }
    }
}