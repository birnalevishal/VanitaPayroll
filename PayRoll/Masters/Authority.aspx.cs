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
    public partial class Authority : System.Web.UI.Page
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

            //strQry = "SELECT Year FROM M_Year ORDER BY Year";
            //objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            //ddlYear.DataSource = objDT;
            //ddlYear.DataTextField = "Year";
            //ddlYear.DataValueField = "Year";
            //ddlYear.DataBind();
            //ddlYear.Items.Insert(0, "Select");
            
            strQry = "SELECT MenuId,Title FROM M_Menu where title in('Salary Config','Allowance Config')";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlformname.DataSource = objDT;
            ddlformname.DataTextField = "Title";
            ddlformname.DataValueField = "MenuId";
            ddlformname.DataBind();
            //ddlformname.Items.Insert(0, "Select");
            ddlformname.Items.Insert(0, new ListItem("Select", "0"));
        }

        private void BindGrid()
        {
            string strQry = @"SELECT *,M_EMP.Employeename,M_Menu.Title FROM M_Authority 
                              inner join M_EMP ON M_EMP.Employeecd = M_Authority.Employeecd
                              inner join M_Menu ON M_Menu.MenuId=M_Authority.FormCode where M_Authority.OrgID=" + Convert.ToInt16(Session["OrgID"]);
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvList.DataSource = objDT;
            gvList.DataBind();

            ViewState["grid"] = objDT;
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

        private void yearname()
        {
            //monthYear = ddlMon.SelectedValue + "" + ddlYear.SelectedValue;
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

                strQry = @"DELETE FROM M_Authority  WHERE OrgId=@OrgId AND  YearId=@YearId AND Employeecd=@Employeecd AND FormCode=@FormCode";
                SqlParameter[] para1 = new SqlParameter[4];
                para1[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
                para1[1] = new SqlParameter("@YearId", Session["YearID"].ToString());
                para1[2] = new SqlParameter("@Employeecd", txtEmpCode.Text);
                para1[3] = new SqlParameter("@FormCode", ddlformname.SelectedValue.Trim());
                result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, para1);

                strQry = @"INSERT INTO M_Authority(OrgId, YearId, Employeecd,FormCode) 
                                        VALUES(@OrgId, @YearId, @Employeecd,@FormCode)";

                SqlParameter[] para = new SqlParameter[4];
                para[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
                para[1] = new SqlParameter("@YearId", Session["YearID"].ToString());
                para[2] = new SqlParameter("@Employeecd", txtEmpCode.Text);
                para[3] = new SqlParameter("@FormCode", ddlformname.SelectedValue);

                result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, para);

                if (result)
                {
                    strQry = "INSERT INTO T_Log(OrgId, Employeecd, MenuId, Mode, Computername) VALUES(@OrgId, @Employeecd, @MenuId, @Mode, @Computername)";
                    result = false;
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
                        // clearControls();
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

            strQry = "UPDATE M_Authority SET FormCode=@FormCode WHERE OrgId=@OrgId AND YearId=@YearId AND Employeecd=@Employeecd";
            SqlParameter[] para = new SqlParameter[4];
            para[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
            para[1] = new SqlParameter("@YearId", Session["YearID"].ToString());
            para[2] = new SqlParameter("@Employeecd", txtEmpCode.Text);
            para[3] = new SqlParameter("@FormCode", ddlformname.SelectedValue);

            result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

            if (result)
            {
                strQry = "INSERT INTO T_Log(OrgId, Employeecd, MenuId, Mode, Computername) VALUES(@OrgId, @Employeecd, @MenuId, @Mode, @Computername)";
                result = false;
                SqlParameter[] paraLog = new SqlParameter[5];
                paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));

                paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                paraLog[3] = new SqlParameter("@Mode", "A");
                paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());

                result = SqlHelper.ExecuteNonQuery(strQry, paraLog, AppGlobal.strConnString);
                if (result)
                {
                    //clearControls();
                    //BindGrid();
                    fillRecords();
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
                }
            }
        }

        private void clearControls()
        {
            txtEmpCode.Text = "";
            txtEmpName.Text = "";
            ddlformname.SelectedIndex = 0;
            gvList.DataSource = null;
            gvList.DataBind();
            btnSave.Text = "Save";
        }

        private void ViewRecord(int i)
        {
            DataTable objDT = (DataTable)ViewState["objDTList"];

            var ddlYear = Session["YearID"].ToString();
            var ddlemp = objDT.Rows[i]["Employeecd"].ToString();
            var formcode = objDT.Rows[i]["FormCode"].ToString();

            string strQry = "";
            strQry = @"DELETE FROM M_Authority  WHERE OrgId=" + Convert.ToInt32(Session["OrgID"]) + "AND Employeecd=" + ddlemp + " AND  YearId=" + ddlYear + " AND FormCode=" + formcode + "";
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
                    // clearControls();
                    ViewRecord(i);
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void txtEmpCode_TextChanged(object sender, EventArgs e)
        {
            if (txtEmpCode.Text != string.Empty)
            {
                string strQry = "select * from M_Emp where Employeecd='" + txtEmpCode.Text + "'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    fillRecords();
                    if (objDT.Rows[0]["Employeename"] != DBNull.Value)
                        txtEmpName.Text = objDT.Rows[0]["Employeename"].ToString();
                    else
                    {
                        clearControls();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Employee ID Does Not Exist'); ", true);
                    clearControls();
                }
            }
            else
            {
                clearControls();
            }
        }
        protected bool formValidation()
        {
            if (txtEmpCode.Text=="")
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter Employee Code'); ", true);
                return false;
            }
            if (ddlformname.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Form'); ", true);
                return false;
            }
            return true;
        }

        protected void ddlEmpName_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void fillRecords()
        {
            var ddlYear = Session["YearID"].ToString();

            string strQry = @"SELECT *,M_EMP.Employeename,M_Menu.Title FROM M_Authority
                              inner join M_EMP ON M_EMP.Employeecd = M_Authority.Employeecd
                              inner join M_Menu ON M_Menu.MenuId = M_Authority.FormCode
                              where YearId=" + ddlYear + " AND M_EMP.Employeecd='" + txtEmpCode.Text + "' and M_Authority.OrgID=" + Convert.ToInt16(Session["OrgID"]);
            
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvList.DataSource = objDT;
            gvList.DataBind();

            ViewState["objDTList"] = objDT;
        }
    }
}