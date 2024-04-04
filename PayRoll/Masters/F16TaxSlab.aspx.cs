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
    public partial class F16TaxSlab : System.Web.UI.Page
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
            string strQry = "SELECT DISTINCT Convert(Varchar,AgeCatgFrom) +'-'+ Convert(varchar, AgeCatgTo) As AgeCatg FROM T_IncTax ";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlList.DataSource = objDT;
            ddlList.DataTextField = "AgeCatg";
            ddlList.DataValueField = "AgeCatg";
            ddlList.DataBind();

        }

        private void BindGrid()
        {
            string strQry = "SELECT Yrno, PayFr, PayTo, TaxRatePer, SurchagePer, CessPer, AgeCatgFrom, AgeCatgTo, Convert(Varchar,AgeCatgFrom) +'-'+ Convert(varchar, AgeCatgTo) As AgeCatg FROM T_IncTax WHERE Yrno =" + Convert.ToInt16(Session["YearID"]);
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvList.DataSource = objDT;
            gvList.DataBind();

            ViewState["objDT"] = objDT;
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
            string[] AgeCatg = ddlList.SelectedValue.ToString().Split('-');

            strQry = "SELECT Count(*) FROM T_IncTax WHERE Yrno=" + Session["YearID"].ToString() + " AND AgeCatgFrom=" + AgeCatg[0] + " AND AgeCatgTo=" + AgeCatg[1] + " AND PayFr="+ txtAmtFrom.Text.Trim() + " AND PayTo=" + txtAmtTo.Text.Trim();
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Income tax Slab Already Saved'); ", true);
                return;
            }

            strQry = "INSERT INTO T_IncTax(Yrno, PayFr, PayTo, TaxRatePer, SurchagePer, CessPer, AgeCatgFrom, AgeCatgTo) VALUES(@Yrno, @PayFr, @PayTo, @TaxRatePer, @SurchagePer, @CessPer, @AgeCatgFrom, @AgeCatgTo)";

            SqlParameter[] para = new SqlParameter[8];
            para[0] = new SqlParameter("@Yrno", Session["YearID"].ToString());
            para[1] = new SqlParameter("@PayFr", txtAmtFrom.Text.Trim());
            para[2] = new SqlParameter("@PayTo", txtAmtTo.Text.Trim());
            para[3] = new SqlParameter("@TaxRatePer", txtTaxRate.Text.Trim());
            para[4] = new SqlParameter("@SurchagePer", txtSurcharge.Text.Trim());
            para[5] = new SqlParameter("@CessPer", txtCess.Text.Trim());
            para[6] = new SqlParameter("@AgeCatgFrom", AgeCatg[0]);
            para[7] = new SqlParameter("@AgeCatgTo", AgeCatg[1]);

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
            string[] AgeCatg = ddlList.SelectedValue.ToString().Split('-');

            strQry = "UPDATE T_IncTax SET TaxRatePer=@TaxRatePer,SurchagePer=@SurchagePer, CessPer=@CessPer  WHERE Yrno=@Yrno AND AgeCatgFrom=@AgeCatgFrom AND AgeCatgTo=@AgeCatgTo AND PayFr=@PayFr AND PayTo=@PayTo";

            SqlParameter[] para = new SqlParameter[8];
            para[0] = new SqlParameter("@Yrno", Session["YearID"].ToString());
            para[1] = new SqlParameter("@PayFr", txtAmtFrom.Text.Trim());
            para[2] = new SqlParameter("@PayTo", txtAmtTo.Text.Trim());
            para[3] = new SqlParameter("@TaxRatePer", txtTaxRate.Text.Trim());
            para[4] = new SqlParameter("@SurchagePer", txtSurcharge.Text.Trim());
            para[5] = new SqlParameter("@CessPer", txtCess.Text.Trim());
            para[6] = new SqlParameter("@AgeCatgFrom", AgeCatg[0]);
            para[7] = new SqlParameter("@AgeCatgTo", AgeCatg[1]);

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
            txtAmtFrom.Text = "0";
            txtAmtTo.Text = "0";
            txtTaxRate.Text = "0";
            txtSurcharge.Text = "0";
            txtCess.Text = "0";
            //ddlList.SelectedIndex = 0;

            btnSave.Text = "Save";
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
            DataTable objDT = (DataTable)ViewState["objDT"];

            ddlList.SelectedValue = objDT.Rows[i]["AgeCatg"].ToString();
            txtAmtFrom.Text = Convert.ToDouble(objDT.Rows[i]["PayFr"]).ToString("0.00");
            txtAmtTo.Text = Convert.ToDouble(objDT.Rows[i]["PayTo"]).ToString("0.00");
            txtTaxRate.Text = objDT.Rows[i]["TaxRatePer"].ToString();
            txtSurcharge.Text = objDT.Rows[i]["SurchagePer"].ToString();
            txtCess.Text = objDT.Rows[i]["CessPer"].ToString();
        }



    }
}