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
    public partial class Designation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
                BindGrid();
            }
            txtName.Focus();
        }
        private void BindData()
        {
            string strQry = "SELECT Code, Description FROM M_MasterType Where FilterBy='LWFReport'";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlLWFReportType.DataSource = objDT;
            ddlLWFReportType.DataTextField = "Description";
            ddlLWFReportType.DataValueField = "Code";
            ddlLWFReportType.DataBind();
            ddlLWFReportType.Items.Insert(0, new ListItem("Select", ""));

            strQry = "SELECT Code, Description FROM M_MasterType Where FilterBy='STAT'";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlList.DataSource = objDT;
            ddlList.DataTextField = "Description";
            ddlList.DataValueField = "Code";
            ddlList.DataBind();
            ddlList.Items.Insert(0, new ListItem("Select", ""));

        }
        private void BindGrid()
        {
            string strQry = @"SELECT d.*, m1.Description As LWFDesc, m2.Description As STATDesc FROM M_Designation d 
                                Left outer join M_MasterType m1 ON d.LWFReportType=m1.code AND m1.FilterBy='LWFReport' 
                                Left Outer Join M_MasterType m2 ON d.StatReportLevel=m2.code AND m2.FilterBy='STAT' 
                                ORDER BY Designation";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvDesignationList.DataSource = objDT;
            gvDesignationList.DataBind();

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

            //strQry = "SELECT Count(*) FROM M_Designation Where Designation='" + txtName.Text.Trim() + "'";
            //int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            //if (nCnt > 0)
            //{
            //    //alertMessage("Duplicate Entry, Please Check");
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Designation already exists!'); ", true);
            //    clearControls();
            //    return;
            //}

            strQry = "INSERT INTO M_Designation(Desigcd, Designation,LWFReportType, StatReportLevel, IsActive) VALUES(@Desigcd, @Designation,@LWFReportType,@StatReportLevel, @IsActive)";
            int nID = SqlHelper.GetMaxID("M_Designation", "Desigcd", AppGlobal.strConnString);

            SqlParameter[] para = new SqlParameter[5];
            para[0] = new SqlParameter("@Desigcd", nID);
            para[1] = new SqlParameter("@Designation", txtName.Text);
            para[2] = new SqlParameter("@LWFReportType", ddlLWFReportType.SelectedValue.ToString());
            para[3] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");
            para[4] = new SqlParameter("@StatReportLevel", ddlList.SelectedValue.ToString());

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

            strQry = "UPDATE M_Designation SET Designation=@Designation,LWFReportType=@LWFReportType, StatReportLevel=@StatReportLevel, IsActive=@IsActive WHERE Desigcd=@Desigcd";
            SqlParameter[] para = new SqlParameter[5];
            para[0] = new SqlParameter("@Desigcd", nId);
            para[1] = new SqlParameter("@Designation", txtName.Text);
            para[2] = new SqlParameter("@LWFReportType", ddlLWFReportType.SelectedValue.ToString());
            para[3] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");
            para[4] = new SqlParameter("@StatReportLevel", ddlList.SelectedValue.ToString());

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
            ddlLWFReportType.SelectedIndex=0;
            chkIsActive.Checked = true;
            btnSave.Text = "Save";
            txtName.Focus();
            ddlList.SelectedIndex = 0;
            ViewState["CID"] = null;
        }

        protected void gvDesignationList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDesignationList.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvDesignationList_RowCommand(object sender, GridViewCommandEventArgs e)
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

            ViewState["CID"] = objDT.Rows[i]["Desigcd"].ToString();
            txtName.Text = objDT.Rows[i]["Designation"].ToString();
            ddlLWFReportType.SelectedValue = objDT.Rows[i]["LWFReportType"].ToString();
            chkIsActive.Checked = objDT.Rows[i]["IsActive"].ToString() == "Y" ? true : false;
            ddlList.SelectedValue = objDT.Rows[i]["StatReportLevel"].ToString();
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
            string strQry = "SELECT Count(*) FROM M_Designation Where Desigcd<>" + nID + " and Designation='" + txtName.Text.Trim() + "'";
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Designation Already Exists!'); ", true);
                clearControls();
                return false;
            }
            return true;
        }

        protected void gvDesignationList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {

            }
        }
    }
}