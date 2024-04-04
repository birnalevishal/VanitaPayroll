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
    public partial class PFConfig : System.Web.UI.Page
    {
        SqlConnection sqlConn = null;
        SqlCommand sqlCmd = null;
        SqlTransaction sqlTrans = null;

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
            string strQry = "SELECT *,DATENAME(MONTH, DateAdd( month , CONVERT(INT,LEFT(MonYrcd,2)) , -1 ) -1 ) as monthname,right(MonYrcd,4) as year FROM M_PFConfiguration where OrgID=" + Convert.ToInt16(Session["OrgID"]) + " order by right(MonYrcd,4) + left(MonYrcd,2) DESC";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvList.DataSource = objDT;
            gvList.DataBind();

            ViewState["objDTList"] = objDT;
        }
        private void GetData()
        {
            //if (ddlMon.SelectedIndex != 0 && ddlYear.SelectedIndex != 0)
            if (ddlMon.SelectedIndex != 0 )
            {
                string strQry = "SELECT * FROM M_PFConfiguration where OrgID=" + Convert.ToInt32(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    PF.Text = objDT.Rows[0]["PF"].ToString();
                    Epf.Text = objDT.Rows[0]["EPF"].ToString();
                    EPS.Text = objDT.Rows[0]["EPS"].ToString();
                    PfCondition.Text = objDT.Rows[0]["PFConditional"].ToString();
                    PfAmount.Text = objDT.Rows[0]["PFAmtLimit"].ToString();
                    AgeLimit.Text = objDT.Rows[0]["PFAgeLimit"].ToString();
                    HRA.Text = objDT.Rows[0]["HRA"].ToString();
                    AdCharge1.Text = objDT.Rows[0]["AdminCharge1"].ToString();
                    AdCharge2.Text = objDT.Rows[0]["AdminCharge2"].ToString();
                    AdminCharge.Text = objDT.Rows[0]["AdminCharge1min"].ToString();

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

                strQry = @"DELETE FROM M_PFConfiguration  WHERE OrgId=@OrgId AND  MonYrcd=@MonYrcd";
                SqlParameter[] para1 = new SqlParameter[2];
                para1[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
                para1[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + "" + ddlYear.SelectedValue);
                result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, para1);

                strQry = @"INSERT INTO M_PFConfiguration(OrgId, MonYrcd, PF, EPF, EPS, PFConditional, PFAmtLimit, PFAgeLimit,HRA,AdminCharge1,AdminCharge2,AdminCharge1min) 
                                        VALUES(@OrgId, @MonYrcd, @PF, @EPF, @EPS, @PFConditional, @PFAmtLimit, @PFAgeLimit,@HRA,@AdminCharge1,@AdminCharge2,@AdminCharge1min)";

                SqlParameter[] para = new SqlParameter[12];
                para[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
                para[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + "" + ddlYear.SelectedValue);
                para[2] = new SqlParameter("@PF", PF.Text.Trim());
                para[3] = new SqlParameter("@EPF", Epf.Text);
                para[4] = new SqlParameter("@EPS", EPS.Text);
                para[5] = new SqlParameter("@PFConditional", PfCondition.Text);
                para[6] = new SqlParameter("@PFAmtLimit", PfAmount.Text);
                para[7] = new SqlParameter("@PFAgeLimit", AgeLimit.Text);
                para[8] = new SqlParameter("@HRA", HRA.Text);
                para[9] = new SqlParameter("@AdminCharge1", AdCharge1.Text);
                para[10] = new SqlParameter("@AdminCharge2", AdCharge2.Text);
                para[11] = new SqlParameter("@AdminCharge1min", AdminCharge.Text);

                result = SqlHelper.ExecuteTransaction(sqlCmd,strQry, para);

                if (result)
                {
                    
                    strQry = "";
                    strQry = "INSERT INTO T_Log(OrgId, MonthYrcd,Employeecd, MenuId, Mode, Computername) VALUES(@OrgId,@MonthYrcd, @Employeecd, @MenuId, @Mode, @Computername)";

                    SqlParameter[] paraLog = new SqlParameter[6];
                    paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                    paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                    paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                    paraLog[3] = new SqlParameter("@Mode", "A");
                    paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());
                    paraLog[5] = new SqlParameter("@MonthYrcd", ddlMon.SelectedValue + ddlYear.SelectedValue);
                    result = SqlHelper.ExecuteNonQuery(strQry, paraLog, AppGlobal.strConnString);
                    if (result)
                    {
                        sqlTrans.Commit();
                        clearControls();
                        BindGrid();
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
                    }
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
            PF.Text = "";
            Epf.Text = "";
            EPS.Text = "";
            PfCondition.Text = "";
            PfAmount.Text = "";
            AgeLimit.Text = "";
            HRA.Text = "";
            AdCharge1.Text = "";
            AdminCharge.Text = "";
            AdCharge2.Text = "";
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
            PF.Text = objDT.Rows[i]["PF"].ToString();
            Epf.Text = objDT.Rows[i]["EPF"].ToString();
            EPS.Text = objDT.Rows[i]["EPS"].ToString();
            PfCondition.Text = objDT.Rows[i]["PFConditional"].ToString();
            PfAmount.Text = objDT.Rows[i]["PFAmtLimit"].ToString();
            AgeLimit.Text = objDT.Rows[i]["PFAgeLimit"].ToString();
            HRA.Text = objDT.Rows[i]["HRA"].ToString();
            AdCharge1.Text = objDT.Rows[i]["AdminCharge1"].ToString();
            AdminCharge.Text = objDT.Rows[i]["AdminCharge1min"].ToString();
            AdCharge2.Text = objDT.Rows[i]["AdminCharge2"].ToString();

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
            string strQry = "SELECT Count(*) FROM M_PFConfiguration Where OrgId=" + Convert.ToInt32(Session["OrgID"]) + " and MonYrcd ='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Organization is Already Exists!'); ", true);
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
            PF.Focus();
        }


    }
}