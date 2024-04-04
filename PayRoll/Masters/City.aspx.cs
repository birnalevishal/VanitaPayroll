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
    public partial class City : System.Web.UI.Page
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
            string strQry = "SELECT Taluka, TalCd FROM M_Taluka Where IsActive='Y' ORDER BY Taluka";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlList.DataSource = objDT;
            ddlList.DataTextField = "Taluka";
            ddlList.DataValueField = "TalCd";
            ddlList.DataBind();

        }
        private void BindGrid()
        {
            string strQry = "SELECT * FROM M_City Order By Citycd";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvCityList.DataSource = objDT;
            gvCityList.DataBind();

            ViewState["objDTCity"] = objDT;
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

            //strQry = "SELECT Count(*) FROM M_City Where City='" + txtName.Text.Trim() + "'";
            //int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            //if (nCnt > 0)
            //{
            //    //alertMessage("Duplicate Entry, Please Check");
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('City Name already exists!'); ", true);
            //    clearControls();
            //    return;
            //}

            strQry = "INSERT INTO M_City(Citycd, City, PinCode,TalCd,IsActive) VALUES(@Citycd, @City,@PinCode,@TalCd, @IsActive)";
            int nID = SqlHelper.GetMaxID("M_City", "Citycd", AppGlobal.strConnString);

            SqlParameter[] para = new SqlParameter[5];
            para[0] = new SqlParameter("@Citycd", nID);
            para[1] = new SqlParameter("@City", txtName.Text);
            para[2] = new SqlParameter("@PinCode", txtName1.Text);
            para[3] = new SqlParameter("@TalCd", ddlList.Text);
            para[4] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");

            result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

            if (result)
            {
                clearControls();
                BindGrid();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
                //alertMessage("Your details have been saved successfully.");

            }
        }

        private void UpdateRecord()
        {
            string strQry = "";
            bool result = false;
            int nId = Convert.ToInt32(ViewState["CID"]);

            strQry = "UPDATE M_City SET City=@City,PinCode=@PinCode,TalCd=@TalCd,IsActive=@IsActive WHERE Citycd=@Citycd";
            SqlParameter[] para = new SqlParameter[5];
            para[0] = new SqlParameter("@City", txtName.Text);
            para[1] = new SqlParameter("@PinCode", txtName1.Text);
            para[2] = new SqlParameter("@TalCd", ddlList.Text);
            para[3] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");
            para[4] = new SqlParameter("@Citycd", nId);

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
            txtName1.Text = "";
            chkIsActive.Checked = true;
            ddlList.SelectedIndex = 0;
            ViewState["CID"] = null;
            btnSave.Text = "Save";
        }

        protected void gvCityList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCityList.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvCityList_RowCommand(object sender, GridViewCommandEventArgs e)
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
            DataTable objDT = (DataTable)ViewState["objDTCity"];

            ViewState["CID"] = objDT.Rows[i]["Citycd"].ToString();
            txtName.Text = objDT.Rows[i]["City"].ToString();
            txtName1.Text = objDT.Rows[i]["PinCode"].ToString();
            ddlList.SelectedValue = objDT.Rows[i]["TalCd"].ToString();
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

            string strQry = "SELECT Count(*) FROM M_City Where Citycd<>" + nID + " and City='" + txtName.Text + "'";
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('City Name is Already Exists!'); ", true);
                clearControls();
                return false;
            }
            

            return true;
        }
    }
}