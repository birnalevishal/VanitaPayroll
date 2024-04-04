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
    public partial class District : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
                BindGrid();
            }
            ddlState.Focus();
        }

        private void BindData()
        {
            //string strQry = "SELECT Country, CountryCd FROM M_Country Where IsActive='Y' ORDER BY Country";
            //DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            //ddlContry.DataSource = objDT;
            //ddlContry.DataTextField = "Country";
            //ddlContry.DataValueField = "CountryCd";
            //ddlContry.DataBind();

            string strQry1 = "SELECT State, StateCd FROM M_State Where IsActive='Y' ORDER BY State";
            DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            ddlState.DataSource = objDT1;
            ddlState.DataTextField = "State";
            ddlState.DataValueField = "StateCd";
            ddlState.DataBind();
        }
        private void BindGrid()
        {
            //string strQry = "SELECT *, (Select Country FROM M_Country Where CountryCd=st.CountryCd) As Country FROM M_State As st Order By State";
            string strQry = "select M_State.state,M_State.statecd, M_District.district, M_District.distcd,M_District.IsActive from M_District join M_State on M_District.StateCd =M_State.StateCd Order By state,district";

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

            //strQry = "SELECT Count(*) FROM M_District Where District='" + txtName.Text.Trim() + "'";
            //int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            //if (nCnt > 0)
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('District Name already exists!'); ", true);
            //    clearControls();
            //    return;
            //}

            strQry = "INSERT INTO M_District(DistCd, District, StateCd, IsActive) VALUES(@DistCd, @District, @StateCd, @IsActive)";
            int nID = SqlHelper.GetMaxID("M_District", "DistCd", AppGlobal.strConnString);

            SqlParameter[] para = new SqlParameter[4];
            para[0] = new SqlParameter("@DistCd", nID);
            para[1] = new SqlParameter("@District", txtName.Text);
            para[2] = new SqlParameter("@StateCd", ddlState.SelectedValue);
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

            strQry = "UPDATE M_District SET District=@District, StateCd=@StateCd, IsActive=@IsActive WHERE DistCd=@DistCd";
            SqlParameter[] para = new SqlParameter[4];
            para[0] = new SqlParameter("@DistCd", nId);
            para[1] = new SqlParameter("@District", txtName.Text);
            para[2] = new SqlParameter("@StateCd", ddlState.SelectedValue);
            para[3] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");

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
            ddlState.SelectedIndex = 0;
            //ddlContry.SelectedIndex = 0;
            chkIsActive.Checked = true;
            btnSave.Text = "Save";
            ViewState["ID"] = null;
            ddlState.Focus();
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
                    ddlState.Focus();
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void ViewRecord(int i)
        {
            DataTable objDT = (DataTable)ViewState["objDTList"];

            ViewState["ID"] = objDT.Rows[i]["DistCd"].ToString();
            txtName.Text = objDT.Rows[i]["District"].ToString();
            ddlState.SelectedValue = objDT.Rows[i]["Statecd"].ToString();
            chkIsActive.Checked = objDT.Rows[i]["IsActive"].ToString() == "Y" ? true : false;
        }

        protected bool formValidation()
        {
            int nID = 0;
            if (ViewState["ID"] != null)
            {
                nID = Convert.ToInt32(ViewState["ID"]);
            }
            string strQry = "SELECT Count(*) FROM M_District Where DistCd<>" + nID + " and District='" + txtName.Text.Trim() + "'";
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('District Name already exists!'); ", true);
                clearControls();
                return false;
            }
            return true;
        }
    }

}