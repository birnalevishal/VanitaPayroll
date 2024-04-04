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
    public partial class Password : System.Web.UI.Page
    {
        public string monthYear = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if(Convert.ToInt32(Session["RoleId"])==1)
                {
                    Emp.Text = "";
                    BtnReset.Visible = true;
                }
                else
                {
                    BtnReset.Visible = false;
                    Emp.Attributes.Add("ReadOnly", "ReadOnly");
                    Emp.Text = Session["UserName"].ToString();
                    Emp_TextChanged(null, null);

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
                        //InsertRecord();
                        UpdateRecord();
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

        //private void InsertRecord()
        //{
        //    SqlConnection sqlConn = null;
        //    SqlCommand sqlCmd = null;
        //    SqlTransaction sqlTrans = null;
        //    bool result = false;
        //    try
        //    {
        //        SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);
        //        string strQry = "";
        //        strQry = @"DELETE FROM M_EMP  WHERE OrgId=@OrgId AND  MonYrcd=@MonYrcd ";
        //        SqlParameter[] para1 = new SqlParameter[2];
        //        para1[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
        //        para1[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + "" + ddlYear.SelectedValue);
        //        result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, para1);

        //        strQry = @"INSERT INTO M_EMP(Employeecd,Password) 
        //                                VALUES(@Employeecd, @Password)";

        //        SqlParameter[] para = new SqlParameter[3];
        //        para[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
        //        para[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + "" + ddlYear.SelectedValue);
        //        para[2] = new SqlParameter("@Lock", LOCK.Checked ? "Y" : "N");

        //        result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, para);

        //        if (result)
        //        {
        //            sqlTrans.Commit();
        //            clearControls();
        //           ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        sqlTrans.Rollback();
        //        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
        //    }
        //}
        private void UpdateRecord()
        {
            string strQry = "";
            bool result = false;
            int nId = Convert.ToInt32(ViewState["CID"]);

            if (Emp.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Employee Code cannot be left blank'); ", true);
                return;
            }

            if (passwordChange.Text.Trim() == "") {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('New password cannot be left blank'); ", true);
                return;
            }

            strQry = "UPDATE M_EMP SET Password=@Password WHERE OrgId=@OrgId AND Employeecd=@Employeecd";
            SqlParameter[] para = new SqlParameter[3];
            para[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
            para[1] = new SqlParameter("@Employeecd", Emp.Text);
            para[2] = new SqlParameter("@Password", passwordChange.Text);

            result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

            if (result)
            {
                clearControls();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
            }
        }
        private void clearControls()
        {
            Emp.Text = "";
            passwordChange.Text = "";
            txtEmpName.Text = "";
            btnSave.Text = "Save";
        }
        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Show")
                {
                    int i = Convert.ToInt32(e.CommandArgument);
                    clearControls();
                    btnSave.Text = "Update";
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlYear_SelectedIndexChanged1(object sender, EventArgs e)
        {
            // fillRecords();
            //GetData();
        }

        protected void BtnReset_Click(object sender, EventArgs e)
        {
            string strQry = "";
            bool result = false;
            int nId = Convert.ToInt32(ViewState["CID"]);
            string passward = "";
            string strQry1 = "update M_Emp set password= RIGHT('0' + RTRIM(day(birthdate)), 2)+RIGHT('0' + RTRIM(month(birthdate)), 2)+RIGHT('0' + RTRIM(year(birthdate)), 2) +Employeecd ";
            //DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            //if (objDT.Rows.Count> 0)
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Wrong Employee Code'); ", true);
            //    return;
            //}
            //if (txtBirthdate.Text != "")
            //{
            //    int day = Convert.ToDateTime(txtBirthdate.Text).Day;
            //    int month = Convert.ToDateTime(txtBirthdate.Text).Month;
            //    int year = Convert.ToDateTime(txtBirthdate.Text).Year;
            //    string y = year.ToString();
            //    passward = Convert.ToInt16(day).ToString("00") + Convert.ToInt16(month).ToString("00") + y.Substring(y.Length - 2) + txtEmpCode.Text;

            //}

            //strQry = "UPDATE M_EMP SET Password=NULL WHERE OrgId=@OrgId AND Employeecd=@Employeecd";
            //SqlParameter[] para = new SqlParameter[3];
            //para[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
            //para[1] = new SqlParameter("@Employeecd", Emp.Text);
            //para[2] = new SqlParameter("@Password", passwordChange.Text);

            result = SqlHelper.ExecuteNonQuery(strQry1, AppGlobal.strConnString);

            if (result)
            {
                clearControls();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Reset Successfully!'); ", true);
            }
        }

        protected void Emp_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Emp.Text != "")
                {
                    string strQry = "select * from M_Emp where Employeecd='" + Emp.Text + "' and OrgId=" + Convert.ToInt16(Session["OrgID"]);
                    DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                    if (objDT.Rows.Count > 0)
                    {
                        if (objDT.Rows[0]["Employeename"] != DBNull.Value)
                            txtEmpName.Text = objDT.Rows[0]["Employeename"].ToString();
                        //BtnReset.Focus();
                        passwordChange.Focus();
                    }
                    else
                    {
                        Emp.Text = "";
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