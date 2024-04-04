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
    public partial class Skill : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
                BindGrid();
                clearControls();

            }
        }
        private void BindData()
        {
            string strQry = "SELECT Skill, Skillcd FROM M_Skill Where IsActive='Y' ORDER BY Skill";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
           

        }
        private void BindGrid()
        {
            string strQry = "SELECT * FROM M_Skill Order By Skillcd";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvSkillList.DataSource = objDT;
            gvSkillList.DataBind();

            ViewState["objDTSkill"] = objDT;
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
                        clearControls();
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

            //strQry = "SELECT Count(*) FROM M_Skill Where Skill='" + txtName.Text.Trim() + "'";
            //int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            //if (nCnt > 0)
            //{
            //    //alertMessage("Duplicate Entry, Please Check");
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Skill Name already exists!'); ", true);
            //    clearControls();
            //    return;
            //}

            strQry = "INSERT INTO M_Skill(Skillcd, Skill, IsActive) VALUES(@Skillcd, @Skill,@IsActive)";
            int nID = SqlHelper.GetMaxID("M_Skill", "Skillcd", AppGlobal.strConnString);

            SqlParameter[] para = new SqlParameter[3];
            para[0] = new SqlParameter("@Skillcd", nID);
            para[1] = new SqlParameter("@Skill", txtName.Text);
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

            strQry = "UPDATE M_Skill SET Skill=@Skill,Skillcd=@Skillcd,IsActive=@IsActive WHERE Skillcd=@Skillcd";
            SqlParameter[] para = new SqlParameter[3];
            para[0] = new SqlParameter("@Skill", txtName.Text);
            para[1] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");
            para[2] = new SqlParameter("@Skillcd", nId);

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

        protected void gvSkillList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSkillList.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvSkillList_RowCommand(object sender, GridViewCommandEventArgs e)
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
            DataTable objDT = (DataTable)ViewState["objDTSkill"];

            ViewState["CID"] = objDT.Rows[i]["Skillcd"].ToString();
            txtName.Text = objDT.Rows[i]["Skill"].ToString();
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

            string strQry = "SELECT Count(*) FROM M_Skill Where Skillcd<>" + nID + " and Skill ='" + txtName.Text.Trim() + "' ";
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Skill is Already Exists!'); ", true);
                clearControls();
                return false;
            }


            return true;
        }

    }
}