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
    public partial class F16SecDedLimit : System.Web.UI.Page
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
            string strQry = "SELECT section + '#' + convert(varchar, SubSrNo) as section, section +' ' + isnull(narr,'') as sectionNarr FROM  M_F16DedLimit  ";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlList.DataSource = objDT;
            ddlList.DataTextField = "sectionNarr";
            ddlList.DataValueField = "Section";
            ddlList.DataBind();

        }
        private void BindGrid()
        {
            string strQry = "SELECT YearId,  section, isnull(narr,'') as Narr, subSrNo,LimitInAmt, LimitInPct FROM M_F16DedLimit WHERE YearId =" + Convert.ToInt16(Session["YearID"]) + " order by section, subsrno";
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

            strQry = "SELECT Count(*) FROM M_F16DedLimit WHERE YearId="+ Session["YearID"].ToString() + " AND Section='" + ddlList.SelectedValue + "'";
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if(nCnt>0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Section Limit Already Saved'); ", true);
                return;
            }
            string[] section = ddlList.SelectedValue.Split('#');
            
            
            strQry = "INSERT INTO M_F16DedLimit(YearId,SubSrNo, Section, LimitInAmt, LimitInPct) VALUES(@YearId, @SubSrNo, @Section, @LimitInAmt, @LimitInPct)";

            SqlParameter[] para = new SqlParameter[5];
            para[0] = new SqlParameter("@YearId", Session["YearID"].ToString());
            para[1] = new SqlParameter("@Section", section[0]);
            para[2] = new SqlParameter("@SubSrNo", section[1]);
            para[3] = new SqlParameter("@LimitInAmt", txtDedInRs.Text);
            para[4] = new SqlParameter("@LimitInPct", txtDedInPct.Text);
           
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
            string[] section = ddlList.SelectedValue.Split('#');


            strQry = "UPDATE M_F16DedLimit SET LimitInAmt=@LimitInAmt,LimitInPct=@LimitInPct WHERE YearId=@YearId AND Section=@Section and SubSrNo=@SubSrNo";

            SqlParameter[] para = new SqlParameter[5];
            para[0] = new SqlParameter("@YearId", Session["YearID"].ToString());
            para[1] = new SqlParameter("@Section", section[0]);
            para[2] = new SqlParameter("@SubSrNo", section[1]);
            para[3] = new SqlParameter("@LimitInAmt", txtDedInRs.Text);
            para[4] = new SqlParameter("@LimitInPct", txtDedInPct.Text);

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
            txtDedInRs.Text = "";
            txtDedInPct.Text = "";
            ddlList.SelectedIndex = 0;
 
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

            ddlList.SelectedValue = objDT.Rows[i]["Section"].ToString() + "#" + objDT.Rows[i]["SubSrNo"].ToString();
            txtDedInRs.Text = Convert.ToDouble(objDT.Rows[i]["LimitInAmt"]).ToString("0.00");
            txtDedInPct.Text = Convert.ToDouble(objDT.Rows[i]["LimitInPct"]).ToString("0.00");
        }


    }
}