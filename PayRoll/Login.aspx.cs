using SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Data.SqlTypes;
using System.Web.Services;

namespace PayRoll
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
                clearControls();  
            }
            txtName.Focus();
        }
        private void BindData()
        {
            string strQry = "SELECT OrgId, Organization FROM M_Organization Where IsActive='Y' ORDER BY OrgId";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlList.DataSource = objDT;
            ddlList.DataTextField = "Organization";
            ddlList.DataValueField = "OrgId";
            ddlList.DataBind();

            strQry = "SELECT YearId, CONVERT(VARCHAR(10), Frdate, 103) +'-' +  CONVERT(VARCHAR(10), Todate, 103) as date  FROM M_FinanceYear Where IsActive='Y' ORDER BY YearId desc";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlYear.DataSource = objDT;
            ddlYear.DataTextField = "date";
            ddlYear.DataValueField = "YearId";
            ddlYear.DataBind();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Session["form16"] = "Y";
                Session["PaySlip"] = "Y";
                int OrgID = 0;

                string strQry = "SELECT * FROM M_Emp Where Employeecd='" + txtName.Text + "' and IsActive='Y'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    Session["RoleId"] = objDT.Rows[0]["UserRolecd"].ToString();
                    //if(objDT.Rows[0]["UserRolecd"].ToString()=="3" || objDT.Rows[0]["UserRolecd"]==DBNull.Value)
                    //{
                    //    OrgID = Convert.ToInt16(objDT.Rows[0]["OrgID"]);
                    //    ddlList.Attributes.Add("disabled", "disabled");
                    //}
                    //else
                    //{
                    //    OrgID = Convert.ToInt16(ddlList.SelectedValue);
                    //    ddlList.Attributes.Add("Enabled", "Enabled");
                    //}
                    
                    string strQry1 = "SELECT * FROM M_AccessOrganization Where IsActive='Y' AND OrgId='" + ddlList.SelectedValue + "' and Employeecd='" + txtName.Text + "' and yearID=" + ddlYear.SelectedValue;
                    DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
                    {
                        if (objDT1.Rows.Count == 0)
                        {
                            Response.Write("<script>alert('You Do Not Have Access To This Organisation/Year');</script>");
                            return;
                        }
                    }

                    Session["form16"] = "Y";
                    Session["PaySlip"] = "Y";

                    //string strQry2 = "SELECT isnull(form16,'N') as form16, isnull(PaySlip,'N') as PaySlip FROM M_Role Where RoleId=" + Convert.ToInt16(objDT.Rows[0]["UserRolecd"]) + " and IsActive='Y'";
                    //DataTable objDT2 = SqlHelper.ExecuteDataTable(strQry2, AppGlobal.strConnString);
                    //if (objDT2.Rows.Count == 0)
                    //{
                    //    Session["form16"] = objDT2.Rows[0]["form16"].ToString();
                    //    Session["PaySlip"] = objDT2.Rows[0]["PaySlip"].ToString();
                    //}

                    if (objDT.Rows[0]["Password"] != DBNull.Value)
                    {
                        if (objDT.Rows[0]["Password"].ToString() == txtpassword.Text)
                        {
                            //Set Session Variables
                            Session["UserName"] = txtName.Text;
                            Session["EmpName"] = objDT.Rows[0]["Employeename"].ToString();
                            Session["OrgID"] = ddlList.SelectedValue;
                            Session["OrgName"] = ddlList.SelectedItem.Text;
                            Session["YearID"] = ddlYear.SelectedValue;
                            Session["PM"] = 0;
                            Session["MM"] = 0;
                            string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
                            string myIP = GetUserIP(); //Dns.GetHostByName(hostName).AddressList[0].ToString();
                            Session["IP"] = myIP;
                            Session["RoleId"] = objDT.Rows[0]["UserRolecd"].ToString();

                            Session["RoleName"] = "Role Not Set";
                            strQry = "SELECT RoleName, isnull(form16,'N') as form16, isnull(PaySlip,'N') as PaySlip FROM M_Role WHERE RoleId=" + objDT.Rows[0]["UserRolecd"].ToString();
                            DataTable dt = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                            if(dt.Rows.Count>0)
                            {
                                Session["RoleName"] = dt.Rows[0]["RoleName"].ToString();

                                Session["form16"] = dt.Rows[0]["form16"].ToString();
                                Session["PaySlip"] = dt.Rows[0]["PaySlip"].ToString();
                            }
                            

                            //Clear Wrong Attempt Field
                            bool result = false;
                            int wrongPassAttempts = 0;
                            string strQryEmpUpdate = "UPDATE M_Emp SET wrongPassAttempts=@wrongPassAttempts WHERE OrgID=@OrgID and Employeecd=@Employeecd";
                            SqlParameter[] para = new SqlParameter[3];
                            para[0] = new SqlParameter("@wrongPassAttempts", wrongPassAttempts);
                            para[1] = new SqlParameter("@OrgID", ddlList.SelectedValue);
                            para[2] = new SqlParameter("@Employeecd", txtName.Text);
                            result = SqlHelper.ExecuteNonQuery(strQryEmpUpdate, para, AppGlobal.strConnString);

                            //Select Forms Based on Role
                            int RoleId = 0;
                            if (objDT.Rows[0]["UserRolecd"].ToString() != "")
                            {
                                RoleId = Convert.ToInt32(objDT.Rows[0]["UserRolecd"].ToString());
                            }
                            GetRoleBasedAccess(RoleId);


                            lblWarning.Visible = false;
                            Response.Redirect("Default.aspx");
                        }
                        else
                        {
                            lblWarning.Visible = true;
                            lblWarning.Text = "Wrong Username and/or Password";

                            if(Session["RoleId"].ToString()=="1") {
                                return;
                            }

                            bool result = false;
                            string strQryEmpUpdate = "UPDATE M_Emp SET wrongPassAttempts=@wrongPassAttempts WHERE OrgID=@OrgID and Employeecd=@Employeecd";
                            
                            int wrongPassAttempts = 1;
                            SqlParameter[] para = new SqlParameter[3];
                            if (objDT.Rows[0]["wrongPassAttempts"] != DBNull.Value)
                            {
                                wrongPassAttempts += Convert.ToInt16(objDT.Rows[0]["wrongPassAttempts"]);
                            }
                            para[0] = new SqlParameter("@wrongPassAttempts", wrongPassAttempts);
                            para[1] = new SqlParameter("@OrgID", ddlList.SelectedValue);
                            para[2] = new SqlParameter("@Employeecd", txtName.Text);
                            result = SqlHelper.ExecuteNonQuery(strQryEmpUpdate, para, AppGlobal.strConnString);

                            if (result)
                            {
                                strQryEmpUpdate = "select wrongPassAttempts from M_Emp where OrgID=" + OrgID + " and Employeecd='" + txtName.Text + "'";
                                DataTable objDTEmp = SqlHelper.ExecuteDataTable(strQryEmpUpdate, AppGlobal.strConnString);
                                if (Convert.ToInt16(objDTEmp.Rows[0]["wrongPassAttempts"]) >= 3)
                                {
                                    strQry = "UPDATE M_Emp SET Password=@Password WHERE OrgID=@OrgID and Employeecd=@Employeecd";
                                    SqlParameter[] para2 = new SqlParameter[3];
                                    para2[0] = new SqlParameter("@Password", DBNull.Value);
                                    para2[1] = new SqlParameter("@OrgID", ddlList.SelectedValue);
                                    para2[2] = new SqlParameter("@Employeecd", txtName.Text);
                                    result = SqlHelper.ExecuteNonQuery(strQry, para2, AppGlobal.strConnString);
                                    lblWarning.Visible = true;
                                    lblWarning.Text = "User Blocked Due To 3 Wrong Password Attempts";
                                }
                            }
                        }
                        clearControls();
                    }
                    else
                    {
                        lblWarning.Visible = true;
                        lblWarning.Text = "User Blocked Due To 3 Wrong Password Attempts";
                        clearControls();
                    }
                }
                else
                {
                    Session["form16"] = "N";
                    Session["PaySlip"] = "N";
                    strQry = "SELECT * FROM M_User Where UserCd='" + txtName.Text + "' AND Passwd='" + txtpassword.Text + "' AND IsActive='Y'";
                    objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                    if (objDT.Rows.Count > 0)
                    {
                        Session["UserName"] = txtName.Text;
                        Session["EmpName"] = objDT.Rows[0]["UserCd"];
                        Session["OrgID"] = ddlList.SelectedValue;
                        Session["OrgName"] = ddlList.SelectedItem.Text;
                        Session["YearID"] = ddlYear.SelectedValue;
                        Session["PM"] = 0;
                        Session["MM"] = 0;
                        string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
                        string myIP = GetUserIP();//Dns.GetHostByName(hostName).AddressList[0].ToString();
                        Session["IP"] = myIP;
                        Session["RoleId"] = 1;
                        Session["RoleName"] = "SUPER ADMIN";

                        Response.Redirect("Default.aspx");
                    }

                    lblWarning.Visible = true;
                    lblWarning.Text = "Wrong Username and/or Password";
                    clearControls();
                }
            }
            catch (Exception ex)
            {

            }
        }
        
        private void clearControls()
        {
            txtName.Text = "";
            txtpassword.Text = "";
            ddlList.SelectedIndex = 0;
            ddlList.Visible = true;
            txtName.Focus();
            lblWarning.Visible = false;
            lblWarning.Text = "";

            lblOrg.Visible = true;
            txtName.Text = "";
            txtpassword.Text = "";
        }

        private void GetRoleBasedAccess(int RoleId)
        {
            string parentMenuId = "", MenuId = "";
            string strQry = "SELECT DISTINCT ParentMenuId FROM M_RoleMenuAccess WHERE RoleId=" + RoleId;
            DataTable dt = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if(dt.Rows.Count>0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    parentMenuId += dr["ParentMenuId"] + ",";
                }
                parentMenuId = parentMenuId.Substring(0, parentMenuId.Length - 1);
            }

            strQry = "SELECT DISTINCT MenuId FROM M_RoleMenuAccess WHERE RoleId=" + RoleId;
            dt = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    MenuId += dr["MenuId"] + ",";
                }
                MenuId = MenuId.Substring(0, MenuId.Length - 1);
            }

            if (parentMenuId == "") { parentMenuId = "0"; }
            if (MenuId == "") { MenuId = "0"; }

            Session["PMIds"] = parentMenuId;
            Session["MMIds"] = MenuId;
        }

        private string GetUserIP()
        {
            string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return Request.ServerVariables["REMOTE_ADDR"];
        }

        protected void txtName_TextChanged(object sender, EventArgs e)
        {
            
            if (txtName.Text != "")
                {
                string strQry = "SELECT * FROM M_Emp Where Employeecd='" + txtName.Text + "' and IsActive='Y'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    Session["RoleId"] = objDT.Rows[0]["UserRolecd"].ToString();
                    if (objDT.Rows[0]["UserRolecd"].ToString() == "3" || objDT.Rows[0]["UserRolecd"] == DBNull.Value)
                    {
                        ddlList.SelectedValue = objDT.Rows[0]["OrgID"].ToString();
                        ddlList.Visible = false;
                        lblOrg.Visible = false;
                    }
                    else
                    {
                        ddlList.SelectedValue = objDT.Rows[0]["OrgID"].ToString();
                        ddlList.Visible = true;
                        lblOrg.Visible = true;
                    }
                    txtpassword.Text = "";
                    txtpassword.Focus();
                }
                else
                {
                    strQry = "SELECT * FROM M_User Where UserCd='" + txtName.Text + "' AND IsActive='Y'";
                    objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                    if (objDT.Rows.Count > 0)
                    {
                        ddlList.Visible = true;
                        lblOrg.Visible = true;
                        txtpassword.Focus();
                    }
                    else
                    {
                        ddlList.Visible = true;
                        lblOrg.Visible = true;

                        lblWarning.Visible = true;
                        lblWarning.Text = "Wrong Username ";
                        txtName.Text = "";
                        txtName.Focus();

                        txtName.Text = "";
                        txtpassword.Text = "";
                    }
                }
            }
        }
    }
}