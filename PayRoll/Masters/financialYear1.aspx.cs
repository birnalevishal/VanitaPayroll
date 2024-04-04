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
    public partial class financialYear1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGrid();
            }
            txtFromDate.Focus();
        }

        private void BindGrid()
        {
            string strQry = "select YearId,CONVERT(VARCHAR(10), Frdate, 103) as Frdate ,CONVERT(VARCHAR(10), Todate, 103) as Todate,IsActive from M_FinanceYear order by YearID desc";
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
                    if (btnSave.Text == "Save")
                    {
                        if (formValidation())
                        {
                            InsertRecord();
                        }
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

            //strQry = "SELECT Count(*) FROM M_FinanceYear Where Frdate='" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy/MM/dd") + "'";
            //int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            //if (nCnt > 0)
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Financial Year already exists!'); ", true);
            //    clearControls();
            //    return;
            //}

            strQry = "INSERT INTO M_FinanceYear(YearId, Frdate, Todate, IsActive) VALUES(@YearId, @Frdate, @Todate, @IsActive)";
            int nID = SqlHelper.GetMaxID("M_FinanceYear", "YearId", AppGlobal.strConnString);

            SqlParameter[] para = new SqlParameter[4];
            para[0] = new SqlParameter("@YearId", nID);
            para[1] = new SqlParameter("@Frdate", Convert.ToDateTime(txtFromDate.Text).ToString("dd MMM yyyy"));
            para[2] = new SqlParameter("@Todate", Convert.ToDateTime(txtToDate.Text).ToString("dd MMM yyyy"));
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

            strQry = "UPDATE M_FinanceYear SET IsActive=@IsActive WHERE YearId=@YearId";
            SqlParameter[] para = new SqlParameter[4];
            para[0] = new SqlParameter("@YearId", nId);
            para[1] = new SqlParameter("@Frdate", Convert.ToDateTime(txtFromDate.Text).ToString("dd MMM yyyy"));
            para[2] = new SqlParameter("@Todate", Convert.ToDateTime(txtToDate.Text).ToString("dd MMM yyyy"));
            para[3] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");

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
            txtFromDate.Text = "";
            txtToDate.Text = "";
            chkIsActive.Checked = true;
            btnSave.Text = "Save";
            ViewState["ID"] = null;
            txtFromDate.Focus();
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
                    txtFromDate.Focus();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ViewRecord(int i)
        {
            DataTable objDT = (DataTable)ViewState["objDTList"];

            ViewState["ID"] = objDT.Rows[i]["YearId"].ToString();
            txtFromDate.Text = Convert.ToDateTime(objDT.Rows[i]["Frdate"]).ToString("dd/MM/yyyy");
            txtToDate.Text = Convert.ToDateTime(objDT.Rows[i]["Todate"]).ToString("dd/MM/yyyy");
            chkIsActive.Checked = objDT.Rows[i]["IsActive"].ToString() == "Y" ? true : false;
        }

        protected bool formValidation()
        {
            int nID = 0;
            if (ViewState["ID"] != null)
            {
                nID = Convert.ToInt32(ViewState["ID"]);
            }
            string strQry = "SELECT Count(*) FROM M_FinanceYear Where YearId<>" + nID + " and Frdate='" + Convert.ToDateTime(txtFromDate.Text).ToString("dd MMM yyyy") + "' or Todate='" + Convert.ToDateTime(txtToDate.Text).ToString("dd MMM yyyy") + "'";
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Finance Year Already Exists!'); ", true);
                clearControls();
                return false;
            }
            return true;
        }
    }
}