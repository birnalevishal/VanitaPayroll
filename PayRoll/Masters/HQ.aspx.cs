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
    public partial class HQ : System.Web.UI.Page
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
            string strQry = "SELECT * FROM M_HQ Order By HQName";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvHQList.DataSource = objDT;
            gvHQList.DataBind();

            ViewState["objDTHQ"] = objDT;
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
                      
            strQry = "INSERT INTO M_HQ(HQId, HQName, IsActive) VALUES(@HQId, @HQName, @IsActive)";
            int nID = SqlHelper.GetMaxID("M_HQ", "HQId", AppGlobal.strConnString);

            SqlParameter[] para = new SqlParameter[3];
            para[0] = new SqlParameter("@HQId", nID);
            para[1] = new SqlParameter("@HQName", txtName.Text);
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
            int nId = Convert.ToInt32(ViewState["HQId"]);

            strQry = "UPDATE M_HQ SET HQName=@HQName, IsActive=@IsActive WHERE HQId=@HQId";
            SqlParameter[] para = new SqlParameter[3];
            para[0] = new SqlParameter("@HQName", txtName.Text);
            para[1] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");
            para[2] = new SqlParameter("@HQId", nId);

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
            ViewState["HQId"] = null;
        }

        protected void gvCountryList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvHQList.PageIndex = e.NewPageIndex;
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
            DataTable objDT = (DataTable)ViewState["objDTHQ"];

            ViewState["HQId"] = objDT.Rows[i]["HQId"].ToString();
            txtName.Text = objDT.Rows[i]["HQName"].ToString();
            chkIsActive.Checked = objDT.Rows[i]["IsActive"].ToString() == "Y" ? true : false;
        }

        protected bool formValidation()
        {
            int nID = 0;
            if (ViewState["HQId"] != null)
            {
                nID = Convert.ToInt32(ViewState["HQId"]);
            }

            string strQry = "SELECT Count(*) FROM M_HQ Where HQId<>" + nID + " and HQName ='" + txtName.Text.Trim() + "' ";
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Head Quarter Already Exists!'); ", true);
                clearControls();
                return false;
            }


            return true;
        }


    }
}