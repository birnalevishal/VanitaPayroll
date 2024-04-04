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
    public partial class LeaveBalance : System.Web.UI.Page
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

            strQry = "SELECT Employeecd + ' ' + Employeename As Employeename,Employeecd FROM M_Emp where OrgID=" + Convert.ToInt16(Session["OrgID"]) + " ORDER BY Employeename";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlEmpName.DataSource = objDT;
            ddlEmpName.DataTextField = "Employeename";
            ddlEmpName.DataValueField = "Employeecd";
            ddlEmpName.DataBind();
            ddlEmpName.Items.Insert(0, "Select");

            strQry = "SELECT Year  FROM M_Year Where IsActive='Y' ORDER BY Year desc";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlYear.DataSource = objDT;
            ddlYear.DataTextField = "Year";
            ddlYear.DataValueField = "Year";
            ddlYear.DataBind();

            ddlYear.Items.Insert(0, new ListItem("Select", "0000"));

        }

        private void BindGrid()
        {
            string strQry = @"SELECT *,M_EMP.Employeename FROM T_LeaveBalance 
                              inner join M_EMP ON M_EMP.Employeecd = T_LeaveBalance.Employeecd AND M_EMP.OrgId = T_LeaveBalance.OrgId";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvList.DataSource = objDT;
            gvList.DataBind();

            ViewState["grid"] = objDT;
        }

        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            SqlTransaction sqlTrans = null;
            //if(Convert.ToInt16(ddlYear.SelectedValue)!=Convert.ToInt16(DateTime.Now.Year))
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Do Not Select Previous Year. Select " + DateTime.Now.Year + "'); ", true);
            //    ddlYear.SelectedIndex = 0;
            //    return;
            //}
            if (ddlMon.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Month'); ", true);
                return;
            }
            if (ddlYear.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Year'); ", true);
                return;
            }
          
            string strQryLeaveBal = "";
            bool result = false;

            //string strQryLeaveOpn = "SELECT * FROM udf_LeaveOpening(" + Convert.ToInt16(Session["OrgID"]) + ",'" + ddlYear.SelectedValue+ddlMon.SelectedValue + "')";
            string strQryLeaveOpn = "SELECT * FROM udfopeningtransfer(" + Convert.ToInt16(Session["OrgID"]) + ",'" +  ddlMon.SelectedValue+ ddlYear.SelectedValue  + "','"+ ddlMon.SelectedValue + (Convert.ToInt32(ddlYear.SelectedValue)-1).ToString() + "')";
            DataTable objDTLeaveOpn = SqlHelper.ExecuteDataTable(strQryLeaveOpn, AppGlobal.strConnString);
            if(objDTLeaveOpn.Rows.Count>0)
            {
                try
                {
                    SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);

                    //strQryLeaveBal = "delete from T_LeaveBalance where OrgId=@OrgID and MonYrcd=@MonYrcd";
                    //SqlParameter[] paraDet = new SqlParameter[3];
                    //paraDet[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                    //paraDet[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + ddlYear.SelectedValue);
                    //SqlHelper.ExecuteTransaction(sqlCmd, strQryLeaveBal, paraDet);
                    string strQry1 = "delete FROM T_LeaveBalance Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and orgID=" + Convert.ToInt32(Session["OrgID"]);
                    DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);

                    for (int i = 0; i < objDTLeaveOpn.Rows.Count; i++)
                    {
                        strQryLeaveBal = @"INSERT INTO T_LeaveBalance(OrgId, MonYrcd, Employeecd, PL, COff)
                                    VALUES(@OrgId, @MonYrcd, @Employeecd, @PL, @COff)";
                        SqlParameter[] para = new SqlParameter[5];
                        para[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgId"]));
                        para[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + ddlYear.SelectedValue);
                        para[2] = new SqlParameter("@Employeecd", objDTLeaveOpn.Rows[i]["Employeecd"].ToString());
                        para[3] = new SqlParameter("@PL", objDTLeaveOpn.Rows[i]["Opening"].ToString());
                        para[4] = new SqlParameter("@COff", objDTLeaveOpn.Rows[i]["OpeningCoff"].ToString());
                        result = SqlHelper.ExecuteTransaction(sqlCmd, strQryLeaveBal, para);
                    }
                    if(result)
                    {
                        string strQry = "";
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
                            ddlEmpName.SelectedIndex = 0;
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

                strQry = @"DELETE FROM T_LeaveBalance  WHERE OrgId=@OrgId AND  MonYrCd=@MonYrCd AND Employeecd=@Employeecd";
                SqlParameter[] para1 = new SqlParameter[3];
                para1[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
                para1[1] = new SqlParameter("@MonYrcd",ddlMon.SelectedValue+ddlYear.SelectedValue);
                para1[2] = new SqlParameter("@Employeecd", ddlEmpName.SelectedValue.Trim());
               result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, para1);

                strQry = @"INSERT INTO T_LeaveBalance(OrgId, MonYrCd, Employeecd,COff,PL) 
                                        VALUES(@OrgId, @MonYrCd, @Employeecd,@COff,@PL)";

                SqlParameter[] para = new SqlParameter[5];
                para[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
                para[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + ddlYear.SelectedValue);
                para[2] = new SqlParameter("@Employeecd", ddlEmpName.SelectedValue.Trim());
                para[3] = new SqlParameter("@COff", COff.Text);
                para[4] = new SqlParameter("@PL", PL.Text);

                result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, para);

                if (result)
                {
                    sqlTrans.Commit();
                    // clearControls();
                    //BindGrid();
                    fillRecords();
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
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

            strQry = "UPDATE T_LeaveBalance SET COff=@COff,PL=@PL WHERE OrgId=@OrgId AND MonYrCd=@MonYrCd AND Employeecd=@Employeecd";
            SqlParameter[] para = new SqlParameter[5];
            para[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
            para[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + ddlYear.SelectedValue);
            para[2] = new SqlParameter("@Employeecd", ddlEmpName.SelectedValue.Trim());
            para[3] = new SqlParameter("@COff", COff.Text);
            para[4] = new SqlParameter("@PL", PL.Text);

            result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

            if (result)
            {
                //clearControls();
                //BindGrid();
                fillRecords();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
            }
        }

        private void clearControls()
        {
            ddlEmpName.SelectedIndex = 0;
            ddlMon.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;
            COff.Text = "";
            PL.Text = "";
            btnSave.Text = "Save";
            gvList.DataSource = null;
            gvList.DataBind();
        }

        private void ViewRecord(int i)
        {

            DataTable objDT = (DataTable)ViewState["objDTList"];
            var ddlemp = objDT.Rows[i]["Employeecd"].ToString();
            

            string strQry = "";
            strQry = @"DELETE FROM T_LeaveBalance  WHERE OrgId=" + Convert.ToInt32(Session["OrgID"]) + "AND Employeecd='" + ddlemp + "' AND  MonYrCd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);

            fillRecords();
        }
        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;
            // BindGrid();
        }
        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Show")
                {
                    int i = Convert.ToInt32(e.CommandArgument);
                    ViewRecord(i);
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
            if (ddlEmpName.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Employee Name'); ", true);
                return false;
            }
            return true;
        }

        protected void ddlEmpName_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillRecords();
        }
        protected void fillRecords()
        {
            string strQry = @"SELECT *,M_EMP.Employeename FROM T_LeaveBalance
                              inner join M_EMP ON M_EMP.Employeecd = T_LeaveBalance.Employeecd
                              where MonYrCd='" + ddlMon.SelectedValue+ddlYear.SelectedValue + "' AND M_EMP.Employeecd='" + ddlEmpName.SelectedValue + "'";

            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvList.DataSource = objDT;
            gvList.DataBind();

            COff.Text = "";
            PL.Text = "";
            if(objDT.Rows.Count>0)
            {
                COff.Text = objDT.Rows[0]["COff"].ToString();
                PL.Text = objDT.Rows[0]["PL"].ToString();
                btnSave.Text = "Update";
            }

            ViewState["objDTList"] = objDT;
        }

        protected void ddlMon_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillRecords();
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillRecords();
        }
    }
}