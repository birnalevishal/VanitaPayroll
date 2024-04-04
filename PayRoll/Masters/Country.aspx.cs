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
    public partial class Country : System.Web.UI.Page
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
            string strQry = "SELECT * FROM M_Country Order By Country";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvCountryList.DataSource = objDT;
            gvCountryList.DataBind();

            ViewState["objDTCountry"] = objDT;
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

            //strQry = "SELECT Count(*) FROM M_Country Where Country='" + txtName.Text.Trim() + "'";
            //int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            //if (nCnt > 0)
            //{
            //    //alertMessage("Duplicate Entry, Please Check");
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Country Name already exists!'); ", true);
            //    clearControls();
            //    return;
            //}

            strQry = "INSERT INTO M_Country(CountryCd, Country, IsActive) VALUES(@CountryCd, @Country, @IsActive)";
            int nID = SqlHelper.GetMaxID("M_Country", "CountryCd", AppGlobal.strConnString);

            SqlParameter[] para = new SqlParameter[3];
            para[0] = new SqlParameter("@CountryCd", nID);
            para[1] = new SqlParameter("@Country", txtName.Text);
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

        private void UpdateRecord()
        {
            string strQry = "";
            bool result = false;
            int nId = Convert.ToInt32(ViewState["CID"]);

            strQry = "UPDATE M_Country SET Country=@Country, IsActive=@IsActive WHERE CountryCd=@CountryCd";
            SqlParameter[] para = new SqlParameter[3];
            para[0] = new SqlParameter("@Country", txtName.Text);
            para[1] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");
            para[2] = new SqlParameter("@CountryCd", nId);

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
        }

        protected void gvCountryList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCountryList.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvCountryList_RowCommand(object sender, GridViewCommandEventArgs e)
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
            DataTable objDT = (DataTable)ViewState["objDTCountry"];

            ViewState["CID"] = objDT.Rows[i]["CountryCd"].ToString();
            txtName.Text = objDT.Rows[i]["Country"].ToString();
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

            string strQry = "SELECT Count(*) FROM M_Country Where CountryCd<>" + nID + " and Country ='" + txtName.Text.Trim() + "' ";
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Country is Already Exists!'); ", true);
                clearControls();
                return false;
            }


            return true;
        }


    }
}