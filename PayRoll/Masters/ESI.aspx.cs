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
    public partial class ESI : System.Web.UI.Page
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
            string strQry = "SELECT *,DATENAME(MONTH, DateAdd( month , CONVERT(INT,LEFT(MonYrcd,2)) , -1 ) -1 ) as monthname,right(MonYrcd,4) as year FROM M_ESIConfigure where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " order by right(MonYrcd,4) + left(MonYrcd,2) DESC";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvList.DataSource = objDT;
            gvList.DataBind();

            ViewState["objDTList"] = objDT;
        }
        private void GetData()
        {
            if (ddlMon.SelectedIndex != 0 )
            {
                string strQry = "SELECT * FROM M_ESIConfigure where OrgID=" + Convert.ToInt32(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    ESIEmp.Text = objDT.Rows[0]["ESIEmpPer"].ToString();
                    ESIComp.Text = objDT.Rows[0]["ESICompPer"].ToString();
                    txtAmount.Text = objDT.Rows[0]["Amount"].ToString();
                    txtOhfm.Text = objDT.Rows[0]["HYr1FrMon"].ToString();
                    txtOhtm.Text = objDT.Rows[0]["HYr1ToMon"].ToString();
                    txtthfm.Text = objDT.Rows[0]["HYr2FrMon"].ToString();
                    txtthtm.Text = objDT.Rows[0]["HYr2ToMon"].ToString();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    //if (formValidation()==true)
                    //{
                    if (btnSave.Text == "Save")
                    {
                        InsertRecord();
                    }
                    //else if (btnSave.Text == "Update")
                    //{
                    //    UpdateRecord();
                    //    btnSave.Text = "Save";
                    //}
                    //}
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
            try
            {
                string strQry = "";
                bool result = false;
                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);

                strQry = "select * from T_SalaryLock where OrgId=" + Convert.ToInt16(Session["orgID"]) + " and RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)>='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "' and Lock='Y'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    clearControls();
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Salary Already Processed, Cant Modify Now.'); ", true);
                    return;
                }
                strQry = @"DELETE FROM M_ESIConfigure  WHERE OrgId=" + Convert.ToInt32(Session["OrgID"]) + " AND  MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);


                strQry = @"INSERT INTO M_ESIConfigure(OrgId, MonYrcd, ESIEmpPer, ESICompPer, Amount, HYr1FrMon, HYr1ToMon, HYr2FrMon, HYr2ToMon) 
                                        VALUES(@OrgId, @MonYrcd, @ESIEmpPer, @ESICompPer, @Amount, @HYr1FrMon, @HYr1ToMon, @HYr2FrMon, @HYr2ToMon)";

                SqlParameter[] para = new SqlParameter[9];
                para[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
                para[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + "" + ddlYear.SelectedValue);
                para[2] = new SqlParameter("@ESIEmpPer", ESIEmp.Text.Trim());
                para[3] = new SqlParameter("@ESICompPer", ESIComp.Text);
                para[4] = new SqlParameter("@Amount", txtAmount.Text);
                para[5] = new SqlParameter("@HYr1FrMon", txtOhfm.Text);
                para[6] = new SqlParameter("@HYr1ToMon", txtOhtm.Text);
                para[7] = new SqlParameter("@HYr2FrMon", txtthfm.Text);
                para[8] = new SqlParameter("@HYr2ToMon", txtthtm.Text);

                result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

                if (result)
                {

                    clearControls();
                    BindGrid();

                    strQry = "";
                    strQry = "INSERT INTO T_Log(OrgId, Employeecd, MenuId, Mode, Computername) VALUES(@OrgId, @Employeecd, @MenuId, @Mode, @Computername)";

                    SqlParameter[] paraLog = new SqlParameter[5];
                    paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                    paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                    paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                    paraLog[3] = new SqlParameter("@Mode", "A");
                    paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());

                    result = SqlHelper.ExecuteNonQuery(strQry, paraLog, AppGlobal.strConnString);
                    if (result)
                    {
                        sqlTrans.Commit();
                        clearControls();
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
                    }
                }
            }
            catch(Exception ex)
            {
                sqlTrans.Rollback();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
            }
            
        }

        private void UpdateRecord()
        {
            try
            {
                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);

                string strQry = "";
                bool result = false;
                int nId = Convert.ToInt32(ViewState["ID"]);

                strQry = "select * from T_SalaryLock where OrgId=" + Convert.ToInt16(Session["orgID"]) + " and RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)>='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "' and Lock='Y'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    clearControls();
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Salary Already Processed, Cant Modify Now.'); ", true);
                    return;
                }


                strQry = @"UPDATE M_ESIConfigure SET MonYrcd=@MonYrcd, ESIEmpPer=@ESIEmpPer, ESICompPer=@ESICompPer,
                        Amount=@Amount, HYr1FrMon=@HYr1FrMon, HYr1ToMon=@HYr1ToMon,
                        HYr2FrMon=@HYr2FrMon, HYr2ToMon=@HYr2ToMon 
                       WHERE OrgId=@OrgId AND MonYrcd=@MonYrcd";
                SqlParameter[] para = new SqlParameter[9];
                para[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
                para[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + "" + ddlYear.SelectedValue);
                para[2] = new SqlParameter("@ESIEmpPer", ESIEmp.Text.Trim());
                para[3] = new SqlParameter("@ESICompPer", ESIComp.Text);
                para[4] = new SqlParameter("@Amount", txtAmount.Text);
                para[5] = new SqlParameter("@HYr1FrMon", txtOhfm.Text);
                para[6] = new SqlParameter("@HYr1ToMon", txtOhtm.Text);
                para[7] = new SqlParameter("@HYr2FrMon", txtthfm.Text);
                para[8] = new SqlParameter("@HYr2ToMon", txtthtm.Text);

                result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

                if (result)
                {
                    clearControls();
                    BindGrid();
                    strQry = "";
                    strQry = "INSERT INTO T_Log(OrgId, Employeecd, MenuId, Mode, Computername) VALUES(@OrgId, @Employeecd, @MenuId, @Mode, @Computername)";

                    SqlParameter[] paraLog = new SqlParameter[5];
                    paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                    paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                    paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                    paraLog[3] = new SqlParameter("@Mode", "U");
                    paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());

                    result = SqlHelper.ExecuteNonQuery(strQry, paraLog, AppGlobal.strConnString);
                    if (result)
                    {
                        sqlTrans.Commit();
                        clearControls();
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
                    }
                }
            }
            catch(Exception ex)
            {
                sqlTrans.Rollback();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
            }
           
        }

        private void clearControls()
        {
            ddlMon.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;
            ESIEmp.Text = "";
            ESIComp.Text = "";
            txtAmount.Text = "";
            txtOhfm.Text = "";
            txtOhtm.Text = "";
            txtthfm.Text = "";
            txtthtm.Text = "";

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
                    clearControls();
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
            DataTable objDT = (DataTable)ViewState["objDTList"];

            ViewState["ID"] = objDT.Rows[i]["OrgId"].ToString();
            string m, y;
            m = objDT.Rows[i]["MonYrcd"].ToString().Substring(0, 2);
            y = objDT.Rows[i]["MonYrcd"].ToString().Substring(2, 4);

            ddlMon.SelectedValue = m;
            ddlYear.SelectedValue = y;
            ESIEmp.Text = objDT.Rows[i]["ESIEmpPer"].ToString();
            ESIComp.Text = objDT.Rows[i]["ESICompPer"].ToString();
            txtAmount.Text = objDT.Rows[i]["Amount"].ToString();
            txtOhfm.Text = objDT.Rows[i]["HYr1FrMon"].ToString();
            txtOhtm.Text = objDT.Rows[i]["HYr1ToMon"].ToString();
            txtthfm.Text = objDT.Rows[i]["HYr2FrMon"].ToString();
            txtthtm.Text = objDT.Rows[i]["HYr2ToMon"].ToString();

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
            string strQry = "SELECT Count(*) FROM M_ESIConfigure Where OrgId=" + Convert.ToInt32(Session["OrgID"]) + " and MonYrcd ='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
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
            ESIEmp.Focus();
        }


    }
}