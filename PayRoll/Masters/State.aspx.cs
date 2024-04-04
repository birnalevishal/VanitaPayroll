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
    public partial class State : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
                BindGrid();
            }
        }

        private void BindData()
        {
            string strQry = "SELECT Country, CountryCd FROM M_Country Where IsActive='Y' ORDER BY Country";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlList.DataSource = objDT;
            ddlList.DataTextField = "Country";
            ddlList.DataValueField = "CountryCd";
            ddlList.DataBind();

        }
        private void BindGrid()
        {
            string strQry = "SELECT *, (Select Country FROM M_Country Where CountryCd=st.CountryCd) As Country FROM M_State As st Order By State";
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

            //strQry = "SELECT Count(*) FROM M_State Where State='" + txtName.Text.Trim() + "'";
            //int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            //if (nCnt > 0)
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('State Name already exists!'); ", true);
            //    clearControls();
            //    return;
            //}

            strQry = "INSERT INTO M_State(StateCd, State, CountryCd, IsActive) VALUES(@StateCd, @State, @CountryCd, @IsActive)";
            int nID = SqlHelper.GetMaxID("M_State", "StateCd", AppGlobal.strConnString);

            SqlParameter[] para = new SqlParameter[4];
            para[0] = new SqlParameter("@StateCd", nID);
            para[1] = new SqlParameter("@State", txtName.Text);
            para[2] = new SqlParameter("@CountryCd", ddlList.SelectedValue);
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

            strQry = "UPDATE M_State SET State=@State, CountryCd=@CountryCd, IsActive=@IsActive WHERE StateCd=@StateCd";
            SqlParameter[] para = new SqlParameter[4];
            para[0] = new SqlParameter("@State", txtName.Text);
            para[1] = new SqlParameter("@CountryCd", ddlList.SelectedValue);
            para[2] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");
            para[3] = new SqlParameter("@StateCd", nId);

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

            }
        }

        private void ViewRecord(int i)
        {
            DataTable objDT = (DataTable)ViewState["objDTList"];

            ViewState["ID"] = objDT.Rows[i]["StateCd"].ToString();
            txtName.Text = objDT.Rows[i]["State"].ToString();
            ddlList.SelectedValue = objDT.Rows[i]["CountryCd"].ToString();
            chkIsActive.Checked = objDT.Rows[i]["IsActive"].ToString() == "Y" ? true : false;
        }
        protected bool formValidation()
        {
            int nID = 0;
            if (ViewState["ID"] != null)
            {
                nID = Convert.ToInt32(ViewState["ID"]);
            }

            string strQry = "SELECT Count(*) FROM M_State Where StateCd<>" + nID + " and State ='" + txtName.Text.Trim() + "' ";
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('State is Already Exists!'); ", true);
                clearControls();
                return false;
            }


            return true;
        }


    }
}