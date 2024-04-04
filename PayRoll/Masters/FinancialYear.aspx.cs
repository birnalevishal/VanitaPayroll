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
    public partial class FinancialYear : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                BindGrid();
            }
        }
        private void BindGrid()
        {
            string strQry = "SELECT * FROM M_Year Order By Year DESC";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvYearList.DataSource = objDT;
            gvYearList.DataBind();

            ViewState["objDTYear"] = objDT;
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

        private void InsertRecord()
        {
            string strQry = "";
            bool result = false;

            strQry = "SELECT Count(*) FROM M_Year Where Year='" + txtYearNo.Text.Trim() + "'";
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
                //alertMessage("Duplicate Entry, Please Check");
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Year already exists!'); ", true);
                clearControls();
                return;
            }

            strQry = "INSERT INTO M_YEAR(Year, IsActive) VALUES(@Year, @IsActive)";
            SqlParameter[] para = new SqlParameter[2];
            para[0] = new SqlParameter("@Year", txtYearNo.Text);
            para[1] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");

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

            strQry = "UPDATE M_YEAR SET IsActive=@IsActive WHERE Year=@Year";
            SqlParameter[] para = new SqlParameter[2];
            para[0] = new SqlParameter("@Year", txtYearNo.Text);
            para[1] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");

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
            txtYearNo.Text = "";
            chkIsActive.Checked = true;
            btnSave.Text = "Save";
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

        protected void gvYearList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvYearList.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvYearList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            int i = e.NewSelectedIndex;

            DataTable objDT = (DataTable)ViewState["objDTYear"];

            txtYearNo.Text = objDT.Rows[i]["Year"].ToString();
            chkIsActive.Checked = objDT.Rows[i]["Year"].ToString() == "Y" ? true : false;

            btnSave.Text = "Update";
        }

        protected void gvYearList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if(e.CommandName=="Show")
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
            DataTable objDT = (DataTable)ViewState["objDTYear"];

            txtYearNo.Text = objDT.Rows[i]["Year"].ToString();
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
    }
}