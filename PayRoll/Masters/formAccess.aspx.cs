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
    public partial class formAccess : System.Web.UI.Page
    {
        DataTable dtSource = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
                BindGrid();
            }
            txtEmpCode.Focus();
        }

        private void BindData()
        {
            string strQry = "SELECT OrgId, Organization FROM M_Organization Where IsActive='Y' ORDER BY OrgId";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlList.DataSource = objDT;
            ddlList.DataTextField = "Organization";
            ddlList.DataValueField = "OrgId";
            ddlList.DataBind();

            strQry = "SELECT MenuId, Title FROM M_Menu ORDER BY Title ";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlMenu.DataSource = objDT;
            ddlMenu.DataTextField = "Title";
            ddlMenu.DataValueField = "MenuId";
            ddlMenu.DataBind();

            //chkList.DataSource = objDT;
            //chkList.DataTextField = "Title";
            //chkList.DataValueField = "MenuId";
            //chkList.DataBind();
        }
        private void BindGrid()
        {
            string strQry = "select emp.Employeename as Employeecd,org.Organization as orgID, men.title as MenuID, CONVERT(VARCHAR(10), Frdate, 103) +'-' +  CONVERT(VARCHAR(10), Todate, 103) as yearID from M_AccessForm accFrm " +
                            " join M_Emp emp on accFrm.Employeecd = emp.Employeecd " +
                            " join M_Organization org on accFrm.OrgId = org.OrgId " +
                            " join M_Menu men on accFrm.menuID = men.menuId " +
                            " join M_FinanceYear fa on accFrm.YearID = fa.yearID " +
                            " where emp.Employeecd ='" + txtEmpCode.Text + "'" ;
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvList.DataSource = objDT;
            gvList.DataBind();

            strQry = "select * from M_AccessForm where Employeecd='" + txtEmpCode.Text + "'";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
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

            //strQry = "SELECT Count(*) FROM M_AccessForm Where OrgId='" + ddlList.SelectedValue + "' and YearId='" + Convert.ToInt32(Session["YearID"]) + "' and Employeecd='" + txtEmpCode.Text + "' and menuID=" + ddlMenu.SelectedValue;
            //int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            //if (nCnt > 0)
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Menu Access already exists!'); ", true);
            //    //clearControls();
            //    return;
            //}

            strQry = "INSERT INTO M_AccessForm(OrgId, YearId, Employeecd, MenuID, Lock) VALUES(@OrgId, @YearId, @Employeecd,@MenuID,@Lock)";
            //int nID = SqlHelper.GetMaxID("M_Bank", "BankCd", AppGlobal.strConnString);

            SqlParameter[] para = new SqlParameter[5];
            para[0] = new SqlParameter("@OrgId", Convert.ToInt16(ddlList.SelectedValue));
            para[1] = new SqlParameter("@YearId", Convert.ToInt32(Session["YearID"]));
            para[2] = new SqlParameter("@Employeecd", txtEmpCode.Text);
            para[3] = new SqlParameter("@MenuID", Convert.ToInt32(ddlMenu.SelectedValue));
            para[4] = new SqlParameter("@Lock", "N");
            result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

            if (result)
            {
                //clearControls();
                BindGrid();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
                ddlList.SelectedIndex = 0;
            }
        }

        private void UpdateRecord()
        {
            string strQry = "";
            bool result = false;
            int nId = Convert.ToInt32(ViewState["ID"]);

            //strQry = "SELECT Count(*) FROM M_AccessForm Where OrgId='" + ddlList.SelectedValue + "' and YearId='" + Convert.ToInt32(Session["YearID"]) + "' and Employeecd='" + txtEmpCode.Text + "' and menuID=" + ddlMenu.SelectedValue; 
            //int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            //if (nCnt > 0)
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Menu Access already exists!'); ", true);
            //    //clearControls();
            //    return;
            //}

            string strQry1 = "select * from M_AccessForm where Employeecd='" + txtEmpCode.Text + "'";
            DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            if(objDT1.Rows.Count>0)
            {
                strQry = "UPDATE M_AccessForm set OrgId=@OrgId, YearId=@YearId, Employeecd=@Employeecd, MenuID=@MenuID where OrgId=" + Convert.ToInt64(objDT1.Rows[0]["OrgId"])  + "and YearId=" + Convert.ToInt64(objDT1.Rows[0]["YearId"]) + " and Employeecd=" + Convert.ToInt64(objDT1.Rows[0]["Employeecd"]) + " and MenuID=" + Convert.ToInt16(objDT1.Rows[0]["MenuID"]);

                SqlParameter[] para = new SqlParameter[5];
                para[0] = new SqlParameter("@OrgId", Convert.ToInt16(ddlList.SelectedValue));
                para[1] = new SqlParameter("@YearId", Convert.ToInt32(Session["YearID"]));
                para[2] = new SqlParameter("@Employeecd", txtEmpCode.Text);
                para[3] = new SqlParameter("@MenuID", Convert.ToInt32(ddlMenu.SelectedValue));
                para[4] = new SqlParameter("@Lock", "N");

                result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

                if (result)
                {
                    clearControls();
                    BindGrid();
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
                }
            }
        }

        private void clearControls()
        {
            txtEmpCode.Text = "";
            txtEmpName.Text= "";
            ddlList.SelectedIndex= 0;
            ddlMenu.SelectedIndex = 0;
            btnSave.Text = "Save";
            gvList.DataSource = null;
            txtEmpName.ReadOnly = true;
            gvList.DataBind();
            txtEmpCode.Focus();
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
                    txtEmpCode.Focus();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ViewRecord(int i)
        {
            DataTable objDT = (DataTable)ViewState["objDTList"];

            ViewState["ID"] = objDT.Rows[i]["OrgId"].ToString();
            txtEmpCode.Text = objDT.Rows[i]["Employeecd"].ToString();
           // txtEmpName.Text= objDT.Rows[i]["Address"].ToString();
            ddlList.SelectedValue= objDT.Rows[i]["OrgId"].ToString();
            ddlMenu.SelectedValue = objDT.Rows[i]["MenuID"].ToString();

        }

        protected void txtEmpCode_TextChanged(object sender, EventArgs e)
        {
            if(txtEmpCode.Text!=string.Empty)
            {
                string strQry = "select * from M_Emp where Employeecd='" + txtEmpCode.Text + "'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if(objDT.Rows.Count>0)
                {
                    BindGrid();
                    if (objDT.Rows[0]["Employeename"] != DBNull.Value)
                        txtEmpName.Text = objDT.Rows[0]["Employeename"].ToString();
                    else
                    {
                        clearControls();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Employee ID Does Not Exist'); ", true);
                    clearControls();
                }
            }
            else
            {
                clearControls();
            }
        }

        protected bool formValidation()
        {
            int nID = 0;
            if (ViewState["ID"] != null)
            {
                nID = Convert.ToInt32(ViewState["ID"]);
            }
            string strQry = "SELECT Count(*) FROM M_AccessForm Where OrgId='" + ddlList.SelectedValue + "' and YearId='" + Convert.ToInt32(Session["YearID"]) + "' and Employeecd='" + txtEmpCode.Text + "' and menuID=" + ddlMenu.SelectedValue;
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Menu Access already exists!'); ", true);
                //clearControls();
                return false;
            }
            return true;
        }
    }
}