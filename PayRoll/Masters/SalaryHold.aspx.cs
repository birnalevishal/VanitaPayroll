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
    public partial class SalaryHold : System.Web.UI.Page
    {
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

            //strQry = "SELECT Employeename,Employeecd FROM M_Emp ORDER BY Employeename";
            //objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            //ddlEmpName.DataSource = objDT;
            //ddlEmpName.DataTextField = "Employeename";
            //ddlEmpName.DataValueField = "Employeecd";
            //ddlEmpName.DataBind();
            //ddlEmpName.Items.Insert(0, "Select");
        }

        private void BindGrid()
        {
            string strQry = "SELECT *,DATENAME(MONTH, DateAdd( month , CONVERT(INT,LEFT(T_SalaryHold.MonYrcd,2)) , -1 ) -1 ) as monthname,right(T_SalaryHold.MonYrcd,4) as year FROM T_SalaryHold inner join M_EMP ON M_EMP.Employeecd = T_SalaryHold.Employeecd where T_SalaryHold.orgId=" + Convert.ToInt16(Session["OrgID"])  + " order by right(T_SalaryHold.MonYrcd, 4) + left(T_SalaryHold.MonYrcd, 2) DESC";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvList.DataSource = objDT;
            gvList.DataBind();

            ViewState["objDTList"] = objDT;
        }
        //private void GetData()
        //{
        //    if (ddlMon.SelectedIndex != 0 && ddlYear.SelectedIndex != 0)
        //    {
        //        string strQry = "SELECT * FROM T_SalaryHold where OrgID=" + Convert.ToInt32(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
        //        DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
        //        if (objDT.Rows.Count > 0)
        //        {
        //            Hold.Checked = objDT.Rows[0]["Hold"].ToString() == "Y" ? true : false;

        //        }
        //    }
        //}

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

                strQry = @"DELETE FROM T_SalaryHold  WHERE OrgId=@OrgId AND  MonYrcd=@MonYrcd AND Employeecd=@Employeecd ";
                SqlParameter[] para1 = new SqlParameter[3];
                para1[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
                para1[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + "" + ddlYear.SelectedValue);
                para1[2] = new SqlParameter("@Employeecd", txtEmpCode.Text);
                result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, para1);

                strQry = @"INSERT INTO T_SalaryHold(OrgId, MonYrcd, Employeecd,remark, Hold) 
                                        VALUES(@OrgId, @MonYrcd, @Employeecd,@remark, @Hold)";

                SqlParameter[] para = new SqlParameter[5];
                para[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
                para[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + "" + ddlYear.SelectedValue);
                para[2] = new SqlParameter("@Employeecd", txtEmpCode.Text);
                para[3] = new SqlParameter("@Hold", Hold.Checked ? "Y" : "N");
                para[4] = new SqlParameter("@Remark", txtRemark.Text);

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
                        //BindGrid();
                        fillRecords();
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
            string strQry = "";
            bool result = false;
            int nId = Convert.ToInt32(ViewState["CID"]);

            strQry = "UPDATE T_SalaryHold SET Hold=@Hold, remark=@remark WHERE OrgId=@OrgId AND MonYrcd=@MonYrcd AND Employeecd=@Employeecd";
            SqlParameter[] para = new SqlParameter[5];
            para[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
            para[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + "" + ddlYear.SelectedValue);
            para[2] = new SqlParameter("@Employeecd", txtEmpCode.Text);
            para[3] = new SqlParameter("@Hold", Hold.Checked ? "Y" : "N");
            para[4] = new SqlParameter("@Remark", txtRemark.Text);
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
                    clearControls();
                    //BindGrid();
                    fillRecords();
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
                }
            }
        }

        private void clearControls()
        {
            ddlMon.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;
            txtEmpCode.Text = "";
            txtEmpName.Text = "";
            txtRemark.Text = "";
            Hold.Checked = false;
            btnSave.Text = "Save";
        }

        private void ViewRecord(int i)
        {
            Hold.Checked = false;
            
            ddlMon.SelectedIndex = 0;ddlYear.SelectedIndex = 0;

            DataTable objDT = (DataTable)ViewState["objDTList"];

            ViewState["ID"] = objDT.Rows[i]["OrgId"].ToString();
            string m, y;
            m = objDT.Rows[i]["MonYrcd"].ToString().Substring(0, 2);
            y = objDT.Rows[i]["MonYrcd"].ToString().Substring(2, 4);

            ddlMon.SelectedValue = m;
            ddlYear.SelectedValue = y;

            Hold.Checked = objDT.Rows[i]["Hold"].ToString() == "Y" ? true : false;
            txtEmpCode.Text = objDT.Rows[i]["Employeecd"].ToString();
            txtEmpName.Text = objDT.Rows[i]["Employeename"].ToString();
            txtRemark.Text = objDT.Rows[i]["remark"].ToString();

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
            return true;
        }
        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            yearname();
            //GetData();
            ddlMon.Focus();
            fillRecords();
        }

        protected void ddlMon_SelectedIndexChanged(object sender, EventArgs e)
        {
            yearname();
            // GetData();
            Hold.Focus();
            fillRecords();
        }

        protected void ddlYear_SelectedIndexChanged1(object sender, EventArgs e)
        {
            fillRecords();
        }
        protected void fillRecords()
        {
            string strQry = "SELECT *,DATENAME(MONTH, DateAdd( month , CONVERT(INT,LEFT(T_SalaryHold.MonYrcd,2)) , -1 ) -1 ) as monthname,right(T_SalaryHold.MonYrcd,4) as year FROM T_SalaryHold inner join M_EMP ON M_EMP.Employeecd = T_SalaryHold.Employeecd where T_SalaryHold.orgId=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrCd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' order by right(T_SalaryHold.MonYrcd, 4) + left(T_SalaryHold.MonYrcd, 2) DESC";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvList.DataSource = objDT;
            gvList.DataBind();

            ViewState["objDTList"] = objDT;
        }

        protected void txtEmpCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtEmpCode.Text != "")
                {
                    string strQry = "select * from M_Emp where OrgId=" + Convert.ToInt32(Session["OrgID"]) + " and Employeecd='" + txtEmpCode.Text + "'";
                    DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                    if (objDT.Rows.Count > 0)
                    {
                        txtEmpName.Text = objDT.Rows[0]["Employeename"].ToString();
                        txtRemark.Focus();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Wrong Employee Code'); ", true);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}