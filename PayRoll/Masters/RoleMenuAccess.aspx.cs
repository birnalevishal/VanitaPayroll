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
    public partial class RoleMenuAccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
                BindList();
            }
        }

        private void BindData()
        {
            string strQry = "SELECT RoleId, RoleName FROM M_Role Where IsActive='Y' ORDER BY RoleId";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlList.DataSource = objDT;
            ddlList.DataTextField = "RoleName";
            ddlList.DataValueField = "RoleId";
            ddlList.DataBind();

            strQry = "SELECT MenuId, Description FROM M_Menu Where ParentMenuId IN(3) ORDER BY Description";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            chkTranList.DataSource = objDT;
            chkTranList.DataTextField = "Description";
            chkTranList.DataValueField = "MenuId";
            chkTranList.DataBind();

            strQry = "SELECT MenuId, Description FROM M_Menu Where ParentMenuId IN(4) ORDER BY Description";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            chkSalReportsList.DataSource = objDT;
            chkSalReportsList.DataTextField = "Description";
            chkSalReportsList.DataValueField = "MenuId";
            chkSalReportsList.DataBind();

            strQry = "SELECT MenuId, Description FROM M_Menu Where ParentMenuId IN(11) ORDER BY Description";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            chkEmpRptList.DataSource = objDT;
            chkEmpRptList.DataTextField = "Description";
            chkEmpRptList.DataValueField = "MenuId";
            chkEmpRptList.DataBind();

            strQry = "SELECT MenuId, Description FROM M_Menu Where ParentMenuId IN(9) ORDER BY Description";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            chkEmpMasterList.DataSource = objDT;
            chkEmpMasterList.DataTextField = "Description";
            chkEmpMasterList.DataValueField = "MenuId";
            chkEmpMasterList.DataBind();

            strQry = "SELECT MenuId, Description FROM M_Menu Where ParentMenuId IN(2) ORDER BY Description";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            chkCommMastList.DataSource = objDT;
            chkCommMastList.DataTextField = "Description";
            chkCommMastList.DataValueField = "MenuId";
            chkCommMastList.DataBind();

            strQry = "SELECT MenuId, Description FROM M_Menu Where ParentMenuId IN(5) ORDER BY Description";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            chkUtilList.DataSource = objDT;
            chkUtilList.DataTextField = "Description";
            chkUtilList.DataValueField = "MenuId";
            chkUtilList.DataBind();

            strQry = "SELECT MenuId, Description FROM M_Menu Where ParentMenuId IN(6) ORDER BY Description";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            chkConfigList.DataSource = objDT;
            chkConfigList.DataTextField = "Description";
            chkConfigList.DataValueField = "MenuId";
            chkConfigList.DataBind();

            strQry = "SELECT MenuId, Description FROM M_Menu Where ParentMenuId IN(7) ORDER BY Description";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            chkIncrList.DataSource = objDT;
            chkIncrList.DataTextField = "Description";
            chkIncrList.DataValueField = "MenuId";
            chkIncrList.DataBind();

            strQry = "SELECT MenuId, Description FROM M_Menu Where ParentMenuId IN(8) ORDER BY Description";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            chkBonusList.DataSource = objDT;
            chkBonusList.DataTextField = "Description";
            chkBonusList.DataValueField = "MenuId";
            chkBonusList.DataBind();

            strQry = "SELECT MenuId, Description FROM M_Menu Where ParentMenuId IN(10) ORDER BY Description";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            chkAccessList.DataSource = objDT;
            chkAccessList.DataTextField = "Description";
            chkAccessList.DataValueField = "MenuId";
            chkAccessList.DataBind();

            strQry = "SELECT MenuId, Description FROM M_Menu Where ParentMenuId IN(98) ORDER BY Description";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            chkEmpLifeCycleList.DataSource = objDT;
            chkEmpLifeCycleList.DataTextField = "Description";
            chkEmpLifeCycleList.DataValueField = "MenuId";
            chkEmpLifeCycleList.DataBind();

            strQry = "SELECT MenuId, Description FROM M_Menu Where ParentMenuId IN(108) ORDER BY Description";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            chkInsentiveList.DataSource = objDT;
            chkInsentiveList.DataTextField = "Description";
            chkInsentiveList.DataValueField = "MenuId";
            chkInsentiveList.DataBind();

            strQry = "SELECT MenuId, Description FROM M_Menu Where ParentMenuId IN(123) ORDER BY Description";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            chkSalaryProcessList.DataSource = objDT;
            chkSalaryProcessList.DataTextField = "Description";
            chkSalaryProcessList.DataValueField = "MenuId";
            chkSalaryProcessList.DataBind();

            strQry = "SELECT MenuId, Description FROM M_Menu Where ParentMenuId IN(124) ORDER BY Description";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            chkLegalList.DataSource = objDT;
            chkLegalList.DataTextField = "Description";
            chkLegalList.DataValueField = "MenuId";
            chkLegalList.DataBind();


        }

        private void BindList()
        {
            ClearList();

            string strQry = "SELECT DISTINCT MenuId FROM M_RoleMenuAccess Where RoleId=" + ddlList.SelectedValue;
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);

            foreach (DataRow dr in objDT.Rows)
            {
                foreach (ListItem ls in chkTranList.Items)
                {
                    if (ls.Value == dr["MenuId"].ToString())
                    {
                        ls.Selected = true;
                    }
                }

                foreach (ListItem ls in chkSalReportsList.Items)
                {
                    if (ls.Value == dr["MenuId"].ToString())
                    {
                        ls.Selected = true;
                    }
                }

                foreach (ListItem ls in chkEmpRptList.Items)
                {
                    if (ls.Value == dr["MenuId"].ToString())
                    {
                        ls.Selected = true;
                    }
                }

                foreach (ListItem ls in chkEmpMasterList.Items)
                {
                    if (ls.Value == dr["MenuId"].ToString())
                    {
                        ls.Selected = true;
                    }
                }

                foreach (ListItem ls in chkCommMastList.Items)
                {
                    if (ls.Value == dr["MenuId"].ToString())
                    {
                        ls.Selected = true;
                    }
                }

                foreach (ListItem ls in chkUtilList.Items)
                {
                    if (ls.Value == dr["MenuId"].ToString())
                    {
                        ls.Selected = true;
                    }
                }

                foreach (ListItem ls in chkConfigList.Items)
                {
                    if (ls.Value == dr["MenuId"].ToString())
                    {
                        ls.Selected = true;
                    }
                }

                foreach (ListItem ls in chkIncrList.Items)
                {
                    if (ls.Value == dr["MenuId"].ToString())
                    {
                        ls.Selected = true;
                    }
                }

                foreach (ListItem ls in chkBonusList.Items)
                {
                    if (ls.Value == dr["MenuId"].ToString())
                    {
                        ls.Selected = true;
                    }
                }

                foreach (ListItem ls in chkAccessList.Items)
                {
                    if (ls.Value == dr["MenuId"].ToString())
                    {
                        ls.Selected = true;
                    }
                }

                foreach (ListItem ls in chkEmpLifeCycleList.Items)
                {
                    if (ls.Value == dr["MenuId"].ToString())
                    {
                        ls.Selected = true;
                    }
                }

                foreach (ListItem ls in chkLegalList.Items)
                {
                    if (ls.Value == dr["MenuId"].ToString())
                    {
                        ls.Selected = true;
                    }
                }

                foreach (ListItem ls in chkSalaryProcessList.Items)
                {
                    if (ls.Value == dr["MenuId"].ToString())
                    {
                        ls.Selected = true;
                    }
                }

                foreach (ListItem ls in chkInsentiveList.Items)
                {
                    if (ls.Value == dr["MenuId"].ToString())
                    {
                        ls.Selected = true;
                    }
                }
            }
        }

        private void ClearList()
        {
            chkTranFormsAll.Checked = false;
            chkSalReportsAll.Checked = false;
            chkEmpRptAll.Checked = false;
            chkEmpMastersAll.Checked = false;
            chkCommMasterAll.Checked = false;
            chkUtilAll.Checked = false;
            chkConfigAll.Checked = false;
            chkIncrAll.Checked = false;
            chkBonusAll.Checked = false;
            chkAccessAll.Checked = false;
            chkEmpLifeCycleAll.Checked = false;
            chkInsentiveAll.Checked = false;
            chkLegalAll.Checked = false;
            chkSalaryProcessAll.Checked = false;


            foreach (ListItem ls in chkEmpRptList.Items)
            {
                ls.Selected = false;
            }
            foreach (ListItem ls in chkEmpMasterList.Items)
            {
                ls.Selected = false;
            }
            foreach (ListItem ls in chkCommMastList.Items)
            {
                ls.Selected = false;
            }
            foreach (ListItem ls in chkConfigList.Items)
            {
                ls.Selected = false;
            }
            foreach (ListItem ls in chkTranList.Items)
            {
                ls.Selected = false;
            }
            foreach (ListItem ls in chkSalaryProcessList.Items)
            {
                ls.Selected = false;
            }
            foreach (ListItem ls in chkSalReportsList.Items)
            {
                ls.Selected = false;
            }
            foreach (ListItem ls in chkLegalList.Items)
            {
                ls.Selected = false;
            }
            foreach (ListItem ls in chkEmpLifeCycleList.Items)
            {
                ls.Selected = false;
            }
            foreach (ListItem ls in chkBonusList.Items)
            {
                ls.Selected = false;
            }
            foreach (ListItem ls in chkInsentiveList.Items)
            {
                ls.Selected = false;
            }
            foreach (ListItem ls in chkIncrList.Items)
            {
                ls.Selected = false;
            }
            foreach (ListItem ls in chkAccessList.Items)
            {
                ls.Selected = false;
            }
            foreach (ListItem ls in chkUtilList.Items)
            {
                ls.Selected = false;
            }
        }

        private int getParentMenuId(string MenuId)
        {
            int pMenuId = 0;
            string strQry = "SELECT ParentMenuId FROM M_Menu Where MenuId=" + MenuId;
            pMenuId = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);

            return pMenuId;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string mode = "A";

                string strQry = "";
                string strQryExist= "select * FROM M_RoleMenuAccess WHERE RoleId=" + ddlList.SelectedValue;
                DataTable dtExist = SqlHelper.ExecuteDataTable(strQryExist, AppGlobal.strConnString);
                if(dtExist.Rows.Count>0)
                {
                    mode = "U";
                }

                int icount = 0;
                icount = getCount();
                
                if (icount > 0)
                {
                    strQry = "DELETE FROM M_RoleMenuAccess WHERE RoleId=" + ddlList.SelectedValue;
                    bool result = SqlHelper.ExecuteNonQuery(strQry, AppGlobal.strConnString);
                }

                //Transaction 
                foreach (ListItem ls in chkTranList.Items)
                {
                    if (ls.Selected && ls.Text != "Select All")
                    {
                        strQry = "INSERT INTO M_RoleMenuAccess(RoleId, MenuId, ParentMenuId) VALUES(@RoleId, @MenuId, @ParentMenuId)";
                        SqlParameter[] para = new SqlParameter[3];
                        para[0] = new SqlParameter("@RoleId", ddlList.SelectedValue);
                        para[1] = new SqlParameter("@MenuId", ls.Value);
                        para[2] = new SqlParameter("@ParentMenuId", getParentMenuId(ls.Value));

                        bool result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                    }
                }

                //Salary Reports
                foreach (ListItem ls in chkSalReportsList.Items)
                {
                    if (ls.Selected)
                    {
                        strQry = "INSERT INTO M_RoleMenuAccess(RoleId, MenuId, ParentMenuId) VALUES(@RoleId, @MenuId, @ParentMenuId)";
                        SqlParameter[] para = new SqlParameter[3];
                        para[0] = new SqlParameter("@RoleId", ddlList.SelectedValue);
                        para[1] = new SqlParameter("@MenuId", ls.Value);
                        para[2] = new SqlParameter("@ParentMenuId", getParentMenuId(ls.Value));

                        bool result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                    }
                }

                //Employee Reports
                foreach (ListItem ls in chkEmpRptList.Items)
                {
                    if (ls.Selected)
                    {
                        strQry = "INSERT INTO M_RoleMenuAccess(RoleId, MenuId, ParentMenuId) VALUES(@RoleId, @MenuId, @ParentMenuId)";
                        SqlParameter[] para = new SqlParameter[3];
                        para[0] = new SqlParameter("@RoleId", ddlList.SelectedValue);
                        para[1] = new SqlParameter("@MenuId", ls.Value);
                        para[2] = new SqlParameter("@ParentMenuId", getParentMenuId(ls.Value));

                        bool result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                    }
                }

                //Employee Master
                foreach (ListItem ls in chkEmpMasterList.Items)
                {
                    if (ls.Selected && ls.Text != "Select All")
                    {
                        strQry = "INSERT INTO M_RoleMenuAccess(RoleId, MenuId, ParentMenuId) VALUES(@RoleId, @MenuId, @ParentMenuId)";
                        SqlParameter[] para = new SqlParameter[3];
                        para[0] = new SqlParameter("@RoleId", ddlList.SelectedValue);
                        para[1] = new SqlParameter("@MenuId", ls.Value);
                        para[2] = new SqlParameter("@ParentMenuId", getParentMenuId(ls.Value));

                        bool result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                    }
                }

                //Common Master
                foreach (ListItem ls in chkCommMastList.Items)
                {
                    if (ls.Selected && ls.Text != "Select All")
                    {
                        strQry = "INSERT INTO M_RoleMenuAccess(RoleId, MenuId, ParentMenuId) VALUES(@RoleId, @MenuId, @ParentMenuId)";
                        SqlParameter[] para = new SqlParameter[3];
                        para[0] = new SqlParameter("@RoleId", ddlList.SelectedValue);
                        para[1] = new SqlParameter("@MenuId", ls.Value);
                        para[2] = new SqlParameter("@ParentMenuId", getParentMenuId(ls.Value));

                        bool result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                    }
                }

                //Utility
                foreach (ListItem ls in chkUtilList.Items)
                {
                    if (ls.Selected && ls.Text != "Select All")
                    {
                        strQry = "INSERT INTO M_RoleMenuAccess(RoleId, MenuId, ParentMenuId) VALUES(@RoleId, @MenuId, @ParentMenuId)";
                        SqlParameter[] para = new SqlParameter[3];
                        para[0] = new SqlParameter("@RoleId", ddlList.SelectedValue);
                        para[1] = new SqlParameter("@MenuId", ls.Value);
                        para[2] = new SqlParameter("@ParentMenuId", getParentMenuId(ls.Value));

                        bool result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                    }
                }

                //Configuration
                foreach (ListItem ls in chkConfigList.Items)
                {
                    if (ls.Selected && ls.Text != "Select All")
                    {
                        strQry = "INSERT INTO M_RoleMenuAccess(RoleId, MenuId, ParentMenuId) VALUES(@RoleId, @MenuId, @ParentMenuId)";
                        SqlParameter[] para = new SqlParameter[3];
                        para[0] = new SqlParameter("@RoleId", ddlList.SelectedValue);
                        para[1] = new SqlParameter("@MenuId", ls.Value);
                        para[2] = new SqlParameter("@ParentMenuId", getParentMenuId(ls.Value));

                        bool result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                    }
                }

                //Increment
                foreach (ListItem ls in chkIncrList.Items)
                {
                    if (ls.Selected && ls.Text != "Select All")
                    {
                        strQry = "INSERT INTO M_RoleMenuAccess(RoleId, MenuId, ParentMenuId) VALUES(@RoleId, @MenuId, @ParentMenuId)";
                        SqlParameter[] para = new SqlParameter[3];
                        para[0] = new SqlParameter("@RoleId", ddlList.SelectedValue);
                        para[1] = new SqlParameter("@MenuId", ls.Value);
                        para[2] = new SqlParameter("@ParentMenuId", getParentMenuId(ls.Value));

                        bool result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                    }
                }

                //Bonus
                foreach (ListItem ls in chkBonusList.Items)
                {
                    if (ls.Selected && ls.Text != "Select All")
                    {
                        strQry = "INSERT INTO M_RoleMenuAccess(RoleId, MenuId, ParentMenuId) VALUES(@RoleId, @MenuId, @ParentMenuId)";
                        SqlParameter[] para = new SqlParameter[3];
                        para[0] = new SqlParameter("@RoleId", ddlList.SelectedValue);
                        para[1] = new SqlParameter("@MenuId", ls.Value);
                        para[2] = new SqlParameter("@ParentMenuId", getParentMenuId(ls.Value));

                        bool result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                    }
                }

                //Access
                foreach (ListItem ls in chkAccessList.Items)
                {
                    if (ls.Selected && ls.Text != "Select All")
                    {
                        strQry = "INSERT INTO M_RoleMenuAccess(RoleId, MenuId, ParentMenuId) VALUES(@RoleId, @MenuId, @ParentMenuId)";
                        SqlParameter[] para = new SqlParameter[3];
                        para[0] = new SqlParameter("@RoleId", ddlList.SelectedValue);
                        para[1] = new SqlParameter("@MenuId", ls.Value);
                        para[2] = new SqlParameter("@ParentMenuId", getParentMenuId(ls.Value));

                        bool result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                    }
                }

                //Employee life cycle
                foreach (ListItem ls in chkEmpLifeCycleList.Items)
                {
                    if (ls.Selected && ls.Text != "Select All")
                    {
                        strQry = "INSERT INTO M_RoleMenuAccess(RoleId, MenuId, ParentMenuId) VALUES(@RoleId, @MenuId, @ParentMenuId)";
                        SqlParameter[] para = new SqlParameter[3];
                        para[0] = new SqlParameter("@RoleId", ddlList.SelectedValue);
                        para[1] = new SqlParameter("@MenuId", ls.Value);
                        para[2] = new SqlParameter("@ParentMenuId", getParentMenuId(ls.Value));

                        bool result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                    }
                }

                //salary process
                foreach (ListItem ls in chkSalaryProcessList.Items)
                {
                    if (ls.Selected && ls.Text != "Select All")
                    {
                        strQry = "INSERT INTO M_RoleMenuAccess(RoleId, MenuId, ParentMenuId) VALUES(@RoleId, @MenuId, @ParentMenuId)";
                        SqlParameter[] para = new SqlParameter[3];
                        para[0] = new SqlParameter("@RoleId", ddlList.SelectedValue);
                        para[1] = new SqlParameter("@MenuId", ls.Value);
                        para[2] = new SqlParameter("@ParentMenuId", getParentMenuId(ls.Value));

                        bool result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                    }
                }

                //legal
                foreach (ListItem ls in chkLegalList.Items)
                {
                    if (ls.Selected && ls.Text != "Select All")
                    {
                        strQry = "INSERT INTO M_RoleMenuAccess(RoleId, MenuId, ParentMenuId) VALUES(@RoleId, @MenuId, @ParentMenuId)";
                        SqlParameter[] para = new SqlParameter[3];
                        para[0] = new SqlParameter("@RoleId", ddlList.SelectedValue);
                        para[1] = new SqlParameter("@MenuId", ls.Value);
                        para[2] = new SqlParameter("@ParentMenuId", getParentMenuId(ls.Value));

                        bool result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                    }
                }

                //insentive
                foreach (ListItem ls in chkInsentiveList.Items)
                {
                    if (ls.Selected && ls.Text != "Select All")
                    {
                        strQry = "INSERT INTO M_RoleMenuAccess(RoleId, MenuId, ParentMenuId) VALUES(@RoleId, @MenuId, @ParentMenuId)";
                        SqlParameter[] para = new SqlParameter[3];
                        para[0] = new SqlParameter("@RoleId", ddlList.SelectedValue);
                        para[1] = new SqlParameter("@MenuId", ls.Value);
                        para[2] = new SqlParameter("@ParentMenuId", getParentMenuId(ls.Value));

                        bool result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                    }
                }

                string strQryLog = "";
                strQryLog = "INSERT INTO T_Log(OrgId, Employeecd, MenuId, Mode, Computername) VALUES(@OrgId, @Employeecd, @MenuId, @Mode, @Computername)";
                bool resultLog = false;
                SqlParameter[] paraLog = new SqlParameter[5];
                paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                paraLog[3] = new SqlParameter("@Mode", mode);
                paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());

                resultLog = SqlHelper.ExecuteNonQuery(strQryLog, paraLog, AppGlobal.strConnString);
                if (resultLog)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
            }
        }
        protected void ddlList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindList();
        }
        protected void chkTranFormsAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTranFormsAll.Checked)
            {
                foreach (ListItem ls in chkTranList.Items)
                {
                    ls.Selected = true;
                }
            }
            else
            {
                foreach (ListItem ls in chkTranList.Items)
                {
                    ls.Selected = false;
                }
            }
        }

        protected void chkSalReportsAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSalReportsAll.Checked)
            {
                foreach (ListItem ls in chkSalReportsList.Items)
                {
                    ls.Selected = true;
                }
            }
            else
            {
                foreach (ListItem ls in chkSalReportsList.Items)
                {
                    ls.Selected = false;
                }
            }
        }

        protected void chkEmpRptAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEmpRptAll.Checked)
            {
                foreach (ListItem ls in chkEmpRptList.Items)
                {
                    ls.Selected = true;
                }
            }
            else
            {
                foreach (ListItem ls in chkEmpRptList.Items)
                {
                    ls.Selected = false;
                }
            }
        }

        protected void chkEmpMastersAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEmpMastersAll.Checked)
            {
                foreach (ListItem ls in chkEmpMasterList.Items)
                {
                    ls.Selected = true;
                }
            }
            else
            {
                foreach (ListItem ls in chkEmpMasterList.Items)
                {
                    ls.Selected = false;
                }
            }
        }

        protected void chkCommMasterAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCommMasterAll.Checked)
            {
                foreach (ListItem ls in chkCommMastList.Items)
                {
                    ls.Selected = true;
                }
            }
            else
            {
                foreach (ListItem ls in chkCommMastList.Items)
                {
                    ls.Selected = false;
                }
            }
        }

        protected void chkUtilAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUtilAll.Checked)
            {
                foreach (ListItem ls in chkUtilList.Items)
                {
                    ls.Selected = true;
                }
            }
            else
            {
                foreach (ListItem ls in chkUtilList.Items)
                {
                    ls.Selected = false;
                }
            }
        }

        protected void chkConfigAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkConfigAll.Checked)
            {
                foreach (ListItem ls in chkConfigList.Items)
                {
                    ls.Selected = true;
                }
            }
            else
            {
                foreach (ListItem ls in chkConfigList.Items)
                {
                    ls.Selected = false;
                }
            }
        }

        protected void chkIncrAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIncrAll.Checked)
            {
                foreach (ListItem ls in chkIncrList.Items)
                {
                    ls.Selected = true;
                }
            }
            else
            {
                foreach (ListItem ls in chkIncrList.Items)
                {
                    ls.Selected = false;
                }
            }
        }

        protected void chkBonusAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBonusAll.Checked)
            {
                foreach (ListItem ls in chkBonusList.Items)
                {
                    ls.Selected = true;
                }
            }
            else
            {
                foreach (ListItem ls in chkBonusList.Items)
                {
                    ls.Selected = false;
                }
            }
        }

        protected void chkAccessAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAccessAll.Checked)
            {
                foreach (ListItem ls in chkAccessList.Items)
                {
                    ls.Selected = true;
                }
            }
            else
            {
                foreach (ListItem ls in chkAccessList.Items)
                {
                    ls.Selected = false;
                }
            }
            
        }
        
        protected void chkSalaryProcessAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSalaryProcessAll.Checked)
            {
                foreach (ListItem ls in chkSalaryProcessList.Items)
                {
                    ls.Selected = true;
                }
            }
            else
            {
                foreach (ListItem ls in chkSalaryProcessList.Items)
                {
                    ls.Selected = false;
                }
            }
        }

        protected void chkLegalAll_CheckedChanged(object sender, EventArgs e)
        {
            
            if (chkLegalAll.Checked)
            {
                foreach (ListItem ls in chkLegalList.Items)
                {
                    ls.Selected = true;
                }
            }
            else
            {
                foreach (ListItem ls in chkLegalList.Items)
                {
                    ls.Selected = false;
                }
            }
        }

        protected void chkInsentiveAll_CheckedChanged(object sender, EventArgs e)
        {
            
            if (chkInsentiveAll.Checked)
            {
                foreach (ListItem ls in chkInsentiveList.Items)
                {
                    ls.Selected = true;
                }
            }
            else
            {
                foreach (ListItem ls in chkInsentiveList.Items)
                {
                    ls.Selected = false;
                }
            }
        }

        private int getCount()
        {
            int icount = 0;
            foreach (ListItem lItem in chkTranList.Items)
            {
                if (lItem.Selected == true)
                {
                    icount++;
                }
            }

            foreach (ListItem lItem in chkSalReportsList.Items)
            {
                if (lItem.Selected == true)
                {
                    icount++;
                }
            }

            foreach (ListItem lItem in chkEmpRptList.Items)
            {
                if (lItem.Selected == true)
                {
                    icount++;
                }
            }

            foreach (ListItem lItem in chkEmpMasterList.Items)
            {
                if (lItem.Selected == true)
                {
                    icount++;
                }
            }

            foreach (ListItem lItem in chkCommMastList.Items)
            {
                if (lItem.Selected == true)
                {
                    icount++;
                }
            }

            foreach (ListItem lItem in chkUtilList.Items)
            {
                if (lItem.Selected == true)
                {
                    icount++;
                }
            }

            foreach (ListItem lItem in chkConfigList.Items)
            {
                if (lItem.Selected == true)
                {
                    icount++;
                }
            }

            foreach (ListItem lItem in chkIncrList.Items)
            {
                if (lItem.Selected == true)
                {
                    icount++;
                }
            }

            foreach (ListItem lItem in chkBonusList.Items)
            {
                if (lItem.Selected == true)
                {
                    icount++;
                }
            }

            foreach (ListItem lItem in chkAccessList.Items)
            {
                if (lItem.Selected == true)
                {
                    icount++;
                }
            }

            foreach (ListItem lItem in chkSalaryProcessList.Items)
            {
                if (lItem.Selected == true)
                {
                    icount++;
                }
            }

            foreach (ListItem lItem in chkLegalList.Items)
            {
                if (lItem.Selected == true)
                {
                    icount++;
                }
            }

            foreach (ListItem lItem in chkInsentiveList.Items)
            {
                if (lItem.Selected == true)
                {
                    icount++;
                }
            }

            return icount;
        }

        protected void chkEmpLifeCycleAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEmpLifeCycleAll.Checked)
            {
                foreach (ListItem ls in chkEmpLifeCycleList.Items)
                {
                    ls.Selected = true;
                }
            }
            else
            {
                foreach (ListItem ls in chkEmpLifeCycleList.Items)
                {
                    ls.Selected = false;
                }
            }
        }
    }
}