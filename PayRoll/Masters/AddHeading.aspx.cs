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
    public partial class AddHeading : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGrid();
            }
            fillHeading();
            txtAdd1.Focus();
        }

        protected void fillHeading()
        {
            string strQry = "SELECT * FROM M_AddHeading Where OrgID=" + Convert.ToInt16(Session["OrgID"]);
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if(objDT.Rows.Count>0)
            {
                if(objDT.Rows[0]["Add1Heading"] !=DBNull.Value)
                {
                    txtAdd1.Text = objDT.Rows[0]["Add1Heading"].ToString();
                    txtAdd1.ReadOnly = true;
                }
                if (objDT.Rows[0]["Add2Heading"] != DBNull.Value)
                {
                    txtAdd2.Text = objDT.Rows[0]["Add2Heading"].ToString();
                    txtAdd2.ReadOnly = true;
                }
                if (objDT.Rows[0]["Add3Heading"] != DBNull.Value)
                {
                    txtAdd3.Text = objDT.Rows[0]["Add3Heading"].ToString();
                    txtAdd3.ReadOnly = true;
                }
            }
        }

        private void BindData()
        {
            //string strQry = "SELECT Country, CountryCd FROM M_Country Where IsActive='Y' ORDER BY Country";
            //DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            //ddlContry.DataSource = objDT;
            //ddlContry.DataTextField = "Country";
            //ddlContry.DataValueField = "CountryCd";
            //ddlContry.DataBind();
            
        }

        private void BindGrid()
        {
            string strQry = "select OrgID, Add1Heading,Add2Heading,Add3Heading from M_AddHeading where orgID= " + Convert.ToInt16(Session["OrgID"]);
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
            int nID = 0;
            int nCnt = 0;

            //if (txtAdd1.Text != "")
            //{
            //    strQry = "SELECT Count(*) FROM M_AddHeading Where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and Add1Heading is not null";
            //    nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            //    if (nCnt > 0)
            //    {
            //        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Add1Heading Already Exists!'); ", true);
            //        clearControls();
            //        return;
            //    }
            //}
            //nCnt = 0;
            //if (txtAdd2.Text != "")
            //{
            //    strQry = "SELECT Count(*) FROM M_AddHeading Where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and Add2Heading is not null";
            //    nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            //    if (nCnt > 0)
            //    {
            //        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Add2Heading Already Exists!'); ", true);
            //        clearControls();
            //        return;
            //    }
            //}


            //nCnt = 0;
            //if (txtAdd3.Text != "")
            //{
            //    strQry = "SELECT Count(*) FROM M_AddHeading Where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and Add3Heading is not null";
            //    nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            //    if (nCnt > 0)
            //    {
            //        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Add3Heading Already Exists!'); ", true);
            //        clearControls();
            //        return;
            //    }
            //}

            strQry = "SELECT Count(*) FROM M_AddHeading Where OrgId=" + Convert.ToInt16(Session["OrgID"]);
            nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt == 0)
            {
                strQry = "INSERT INTO M_AddHeading(OrgId, Add1Heading, Add2Heading,Add3Heading) VALUES(@OrgId, @Add1Heading, @Add2Heading,@Add3Heading)";

                SqlParameter[] para = new SqlParameter[4];
                para[0] = new SqlParameter("@OrgId", Convert.ToInt16(Session["OrgID"]));
                if (txtAdd1.Text != "")
                    para[1] = new SqlParameter("@Add1Heading", txtAdd1.Text);
                else
                    para[1] = new SqlParameter("@Add1Heading", DBNull.Value);
                if (txtAdd2.Text != "")
                    para[2] = new SqlParameter("@Add2Heading", txtAdd2.Text);
                else
                    para[2] = new SqlParameter("@Add2Heading", DBNull.Value);
                if (txtAdd3.Text != "")
                    para[3] = new SqlParameter("@Add3Heading", txtAdd3.Text);
                else
                    para[3] = new SqlParameter("@Add3Heading", DBNull.Value);


                result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
            }
            else
            {
                strQry = "update M_AddHeading set Add1Heading=@Add1Heading, Add2Heading=@Add2Heading, Add3Heading=@Add3Heading where OrgID=@OrgID";

                SqlParameter[] para = new SqlParameter[4];
                para[0] = new SqlParameter("@OrgId", Convert.ToInt16(Session["OrgID"]));
                if (txtAdd1.Text != "")
                    para[1] = new SqlParameter("@Add1Heading", txtAdd1.Text);
                else
                    para[1] = new SqlParameter("@Add1Heading", DBNull.Value);
                if (txtAdd2.Text != "")
                    para[2] = new SqlParameter("@Add2Heading", txtAdd2.Text);
                else
                    para[2] = new SqlParameter("@Add2Heading", DBNull.Value);
                if (txtAdd3.Text != "")
                    para[3] = new SqlParameter("@Add3Heading", txtAdd3.Text);
                else
                    para[3] = new SqlParameter("@Add3Heading", DBNull.Value);

                result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
            }
            if (result)
            {
                clearControls();
                BindGrid();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
            }
        }

        protected bool formValidation()
        {
            //int nID = 0;
            //if(ViewState["ID"]!=null)
            //{
            //    nID = Convert.ToInt32(ViewState["ID"]);
            //}
            //string strQry = "SELECT Count(*) FROM M_Bank Where BankCd<>" + nID + " and BankName='" + txtBank.Text.Trim() + "'";
            //int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            //if (nCnt > 0)
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Bank Name already exists!'); ", true);
            //    clearControls();
            //    return false;
            //}
            return true;
        }
        private void clearControls()
        {
            txtAdd1.Text = "";
            txtAdd2.Text = "";
            txtAdd3.Text = "";
            fillHeading();
            //txtAdd1.ReadOnly = false;
            //txtAdd2.ReadOnly = false;
            //txtAdd3.ReadOnly = false;
            txtAdd1.Focus();
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
            //DataTable objDT = (DataTable)ViewState["objDTList"];

            //ViewState["ID"] = objDT.Rows[i]["BankCd"].ToString();
            //txtBank.Text = objDT.Rows[i]["BankName"].ToString();
            //txtAddress.Text= objDT.Rows[i]["Address"].ToString();
            //txtPANNo.Text = objDT.Rows[i]["Panno"].ToString();
            //chkIsActive.Checked = objDT.Rows[i]["IsActive"].ToString() == "Y" ? true : false;
        }
    }
}