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
    public partial class F16StdDedRebate : System.Web.UI.Page
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
            string strQry = "select Yrno, StdDed, Rebate, RebateLimit from M_F16StdDedRebate where Yrno= " + Convert.ToInt16(Session["YearID"]);
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            
            if(objDT.Rows.Count> 0)
            {
                txtStdDed.Text = Convert.ToDouble(objDT.Rows[0]["StdDed"]).ToString("0.00");
                txtRebate.Text = Convert.ToDouble(objDT.Rows[0]["Rebate"]).ToString("0.00");
                txtRebateAmt.Text = Convert.ToDouble(objDT.Rows[0]["RebateLimit"]).ToString("0.00");
            }

        }

        private void BindGrid()
        {
            string strQry = "select Yrno, StdDed, Rebate, RebateLimit from M_F16StdDedRebate where Yrno= " + Convert.ToInt16(Session["YearID"]);
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                //clearControls();
            }
            catch (Exception ex)
            {

            }
        }

        private void InsertRecord()
        {
            string strQry = "";
            bool result = false;
            strQry = "DELETE FROM M_F16StdDedRebate WHERE Yrno=" + Convert.ToInt16(Session["YearID"]);
            result = SqlHelper.ExecuteNonQuery(strQry, AppGlobal.strConnString);

            strQry = "INSERT INTO M_F16StdDedRebate(Yrno, StdDed, Rebate, RebateLimit) VALUES(@Yrno, @StdDed, @Rebate, @RebateLimit)";
            SqlParameter[] para = new SqlParameter[4];
            para[0] = new SqlParameter("@Yrno", Session["YearID"].ToString());
            para[1] = new SqlParameter("@StdDed", txtStdDed.Text.Trim());
            para[2] = new SqlParameter("@Rebate", txtRebate.Text.Trim());
            para[3] = new SqlParameter("@RebateLimit", txtRebateAmt.Text.Trim());

            result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

            if (result)
            {
                BindGrid();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
            }

        }

        private void UpdateRecord()
        {
        }

        private void clearControls()
        {
            txtStdDed.Text = "";
            txtRebate.Text = "";
            txtRebateAmt.Text = "";
            txtStdDed.Focus();
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