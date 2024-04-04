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
    public partial class SalaryLock : System.Web.UI.Page
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
                //BindGrid();
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
            ddlYear.Items.Insert(0, "Select");

        }

        private void BindGrid()
        {
            string strQry = "SELECT *,DATENAME(MONTH, DateAdd( month , CONVERT(INT,LEFT(T_SalaryLock.MonYrcd,2)) , -1 ) -1 ) as monthname,right(T_SalaryLock.MonYrcd,4) as year FROM T_SalaryLock  order by right(T_SalaryLock.MonYrcd, 4) + left(T_SalaryLock.MonYrcd, 2) DESC";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
           // gvList.DataSource = objDT;
           // gvList.DataBind();

            ViewState["objDTList"] = objDT;
        }
        private void GetData()
        {
            if (ddlMon.SelectedIndex != 0 && ddlYear.SelectedIndex != 0)
            {
                string strQry = "SELECT * FROM T_SalaryLock where OrgID=" + Convert.ToInt32(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    LOCK.Checked = objDT.Rows[0]["Lock"].ToString() == "Y" ? true : false;
                }
            }
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
          
            bool result = false;
            try
            {
                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);
                string strQry = "";
                strQry = @"DELETE FROM T_SalaryLock  WHERE OrgId=@OrgId AND  MonYrcd=@MonYrcd ";
                SqlParameter[] para1 = new SqlParameter[2];
                para1[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
                para1[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + "" + ddlYear.SelectedValue);
                result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, para1);

                strQry = @"INSERT INTO T_SalaryLock(OrgId, MonYrcd,Lock) 
                                        VALUES(@OrgId, @MonYrcd,@Lock)";

                SqlParameter[] para = new SqlParameter[3];
                para[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
                para[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + "" + ddlYear.SelectedValue);
                para[2] = new SqlParameter("@Lock", LOCK.Checked ? "Y" : "N");

                result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, para);

                if (result)
                {
                    strQry = "";
                    strQry = "INSERT INTO T_Log(OrgId,MonthYrcd, Employeecd, MenuId, Mode, Computername) VALUES(@OrgId,@MonthYrcd, @Employeecd, @MenuId, @Mode, @Computername)";

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
        private void UpdateRecord()
        {
            try
            {
                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);

                string strQry = "";
                bool result = false;
                int nId = Convert.ToInt32(ViewState["CID"]);

                strQry = "UPDATE T_SalaryLock SET Lock=@Lock WHERE OrgId=@OrgId AND MonYrcd=@MonYrcd";
                SqlParameter[] para = new SqlParameter[3];
                para[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
                para[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + "" + ddlYear.SelectedValue);
                para[2] = new SqlParameter("@Lock", LOCK.Checked ? "Y" : "N");

                result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

                if (result)
                {
                    strQry = "";
                    strQry = "INSERT INTO T_Log(OrgId,MonthYrcd, Employeecd, MenuId, Mode, Computername) VALUES(@OrgId,@MonthYrcd, @Employeecd, @MenuId, @Mode, @Computername)";

                    SqlParameter[] paraLog = new SqlParameter[6];
                    paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                    paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                    paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                    paraLog[3] = new SqlParameter("@Mode", "U");
                    paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());
                    paraLog[5] = new SqlParameter("@MonthYrcd", ddlMon.SelectedValue + ddlYear.SelectedValue);
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
            LOCK.Checked = false;
            btnSave.Text = "Save";
        }



        //private void ViewRecord(int i)
        //{
        //    LOCK.Checked = false;
        //    ddlMon.SelectedIndex = 0; ddlYear.SelectedIndex = 0;

        //    DataTable objDT = (DataTable)ViewState["objDTList"];

        //    ViewState["ID"] = objDT.Rows[i]["OrgId"].ToString();
        //    string m, y;
        //    m = objDT.Rows[i]["MonYrcd"].ToString().Substring(0, 2);
        //    y = objDT.Rows[i]["MonYrcd"].ToString().Substring(2, 4);

        //    ddlMon.SelectedValue = m;
        //    ddlYear.SelectedValue = y;

        //    LOCK.Checked = objDT.Rows[i]["Lock"].ToString() == "Y" ? true : false;


        //}
        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvList.PageIndex = e.NewPageIndex;
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
                    //ViewRecord(i);

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


            return true;
        }


        protected void ddlYear_SelectedIndexChanged1(object sender, EventArgs e)
        {
            // fillRecords();
            GetData();
        }

        protected void ddlMon_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetData();
        }

        protected void btnProcessCoff_Click(object sender, EventArgs e)
        {

            string strQry = "";
            bool result = false;
            
            strQry = "SELECT * FROM T_SalaryLock WHERE OrgId="+ Session["OrgID"].ToString() + " AND MonYrcd="+ ddlMon.SelectedValue + "" + ddlYear.SelectedValue;
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if(objDT.Rows.Count > 0)
            {
                if(objDT.Rows[0]["COffProcessed"].ToString()=="" || objDT.Rows[0]["COffProcessed"].ToString() == "N")
                {
                    
                }
            }
           

            //strQry = "UPDATE T_SalaryLock SET COffProcessed=@COffProcessed WHERE OrgId=@OrgId AND MonYrcd=@MonYrcd";
            //SqlParameter[] para = new SqlParameter[3];
            //para[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
            //para[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + "" + ddlYear.SelectedValue);
            //para[2] = new SqlParameter("@COffProcessed",  "Y");

            //result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

        }
        //protected void fillRecords()
        //{
        //    string strQry = "SELECT *,DATENAME(MONTH, DateAdd( month , CONVERT(INT,LEFT(T_SalaryLock.MonYrcd,2)) , -1 ) -1 ) as monthname,right(T_SalaryLock.MonYrcd,4) as year FROM T_SalaryLock  where MonYrCd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' order by right(T_SalaryLock.MonYrcd, 4) + left(T_SalaryLock.MonYrcd, 2) DESC";
        //    DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
        //    gvList.DataSource = objDT;
        //    gvList.DataBind();

        //    ViewState["objDTList"] = objDT;
        //}
    }
}