using SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlTypes;

namespace PayRoll.Masters
{
    public partial class Canteen : System.Web.UI.Page
    {
        public string monthYear = "";
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
            string strQry = "";
            DataTable objDT;

            strQry = "SELECT Year FROM M_Year ORDER BY Year desc";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlYear.DataSource = objDT;
            ddlYear.DataTextField = "Year";
            ddlYear.DataValueField = "Year";
            ddlYear.DataBind();
        }

        private void BindGrid()
        {
            string strQry = "SELECT *,DATENAME(MONTH, DateAdd( month , CONVERT(INT,LEFT(MonYrcd,2)) , -1 ) -1 ) as monthname,right(MonYrcd,4) as year FROM M_CanteenRate where orgid=" + Convert.ToInt16(Session["OrgID"]) + " order by right(MonYrcd,4) + left(MonYrcd,2) DESC";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvList.DataSource = objDT;
            gvList.DataBind();

            ViewState["objDTList"] = objDT;
        }
        private void GetData()
        {
            if (ddlMon.SelectedIndex != 0 )
            {
                string strQry = "SELECT * FROM M_CanteenRate where OrgID=" + Convert.ToInt32(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    CanteenRate.Text = objDT.Rows[0]["Canteen"].ToString();

                }
            }
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

        private void yearname()
        {
            monthYear = ddlMon.SelectedValue + "" + ddlYear.SelectedValue;
        }

        private void InsertRecord()
        {
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            SqlTransaction sqlTrans = null;
            bool result = false;

            try
            {
                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);

                string strQry = "";

                strQry = "select * from T_SalaryLock where OrgId=" + Convert.ToInt16(Session["orgID"]) + " and RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)>='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "' and Lock='Y'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    clearControls();
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Salary Already Processed, Cant Modify Now.'); ", true);
                    return;
                }

                strQry = @"DELETE FROM M_CanteenRate  WHERE OrgId=@OrgId AND  MonYrcd=@MonYrcd";
                SqlParameter[] para1 = new SqlParameter[2];
                para1[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
                para1[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + "" + ddlYear.SelectedValue);
                result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, para1);

                strQry = @"INSERT INTO M_CanteenRate(OrgId, MonYrcd, Canteen) 
                                        VALUES(@OrgId, @MonYrcd, @Canteen)";

                SqlParameter[] para = new SqlParameter[3];
                para[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
                para[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + "" + ddlYear.SelectedValue);
                para[2] = new SqlParameter("@Canteen", CanteenRate.Text.Trim());

                result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, para);

                if (result)
                {
                    sqlTrans.Commit();
                    clearControls();
                    BindGrid();
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
            }
        }

        private void clearControls()
        {
            ddlMon.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;
            CanteenRate.Text = "";
            btnSave.Text = "Save";
        }

        private void ViewRecord(int i)
        {
            DataTable objDT = (DataTable)ViewState["objDTList"];

            ViewState["ID"] = objDT.Rows[i]["OrgId"].ToString();
            string m, y;
            m = objDT.Rows[i]["MonYrcd"].ToString().Substring(0, 2);
            y = objDT.Rows[i]["MonYrcd"].ToString().Substring(2, 4);

            ddlMon.SelectedValue = m;
            ddlYear.SelectedValue = y;
            CanteenRate.Text = objDT.Rows[i]["Canteen"].ToString();

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
                    clearControls();
                    ViewRecord(i);

                    btnSave.Text = "Update";
                }

            }
            catch (Exception ex)
            {

            }
        }
        protected bool formValidation()
        {
            if (ddlMon.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Month'); ", true);
                return false;
            }
            if (ddlYear.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Year'); ", true);

                return false;
            }
            string strQry = "SELECT Count(*) FROM M_CanteenRate Where OrgId=" + Convert.ToInt32(Session["OrgID"]) + " and MonYrcd ='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Canteen Rate is Already Exists!'); ", true);
                clearControls();
                return false;
            }
            return true;
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            yearname();
            GetData();
            ddlMon.Focus();
        }

        protected void ddlMon_SelectedIndexChanged(object sender, EventArgs e)
        {
            yearname();
            GetData();
            CanteenRate.Focus();
        }
    }
}