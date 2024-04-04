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
    public partial class EmployeeTransfer : System.Web.UI.Page
    {
        SqlConnection sqlConn = null;
        SqlCommand sqlCmd = null;
        SqlTransaction sqlTrans = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
            ddlPrvOrg.Focus();
        }

        private void BindData()
        {
            string strQry1 = "SELECT Organization, OrgId FROM M_Organization Where IsActive='Y' ORDER BY OrgId";
            DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            ddlPrvOrg.DataSource = objDT1;
            ddlPrvOrg.DataTextField = "Organization";
            ddlPrvOrg.DataValueField = "OrgId";
            ddlPrvOrg.DataBind();

            ddlPrvOrg.Items.Insert(0, new ListItem("Select", "0"));

            strQry1 = "SELECT Organization, OrgId FROM M_Organization Where IsActive='Y' ORDER BY OrgId";
            objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            ddlNewOrg.DataSource = objDT1;
            ddlNewOrg.DataTextField = "Organization";
            ddlNewOrg.DataValueField = "OrgId";
            ddlNewOrg.DataBind();

            ddlNewOrg.Items.Insert(0, new ListItem("Select", "0"));
        }

        private void BindGrid()
        {
            string strQry = @"select empCon.Employeecd as Employeecd, CONVERT(VARCHAR(10), empCon.Docdate, 103) as Docdate, div.Division as Divcd, loc.LocationDep as LocDepCd,  desi.Designation as Desigcd , emp.Employeename as HodInchEmpcd , cat.Category as Categcd, skill.Skill as Skillcd, sts.Status as Stacd 
                                from M_EmpConfiguration empCon
                                left outer join M_Division div   on empCon.Divcd = div.Divcd
                                left outer join M_LocationDep loc   on empCon.LocDepCd = loc.LocDepCd
                                left outer join M_Designation desi  on empCon.Desigcd = desi.Desigcd
                                left outer join M_Emp emp   on empCon.HodInchEmpcd = emp.Employeecd
                                left outer join M_Category cat   on empCon.Categcd = cat.Categcd
                                left outer join M_Skill skill on empCon.Skillcd = skill.Skillcd
                                left outer join M_Status sts   on empCon.Stacd = sts.Stacd
                                where empCon.Employeecd ='" + txtEmpCode.Text + "' and empCon.OrgId=" + Convert.ToInt32(Session["OrgID"]) + " order by Docdate ";

            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);

            ViewState["objDTList"] = objDT;
        }

        protected void txtEmpCode_TextChanged(object sender, EventArgs e)
        {
            if (ddlPrvOrg.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Previous Oraganisation'); ", true);
                clearControls();
            }
            if (txtEmpCode.Text != string.Empty)
            {
                string strQry = "select * from M_Emp where Employeecd='" + txtEmpCode.Text + "' and OrgId=" + Convert.ToInt16(ddlPrvOrg.SelectedValue);
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    if (objDT.Rows[0]["Employeename"] != DBNull.Value)
                    {
                        txtEmpName.Text = objDT.Rows[0]["Employeename"].ToString();
                        ddlNewOrg.Focus();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Employee Code Does Not Exist'); ", true);
                    txtEmpCode.Text = "";
                }
            }
        }
        protected void txtEmpCodeNew_TextChanged(object sender, EventArgs e)
        {
            if (ddlNewOrg.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select New Oraganisation'); ", true);
                clearControls();
            }
            if (txtEmpCodeNew.Text != string.Empty)
            {
                string strQry = "select * from M_Emp where Employeecd='" + txtEmpCodeNew.Text + "' and OrgId=" + Convert.ToInt16(ddlNewOrg.SelectedValue);
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    if (objDT.Rows[0]["Employeename"] != DBNull.Value)
                    {
                        //txtEmpNameNew.Text = objDT.Rows[0]["Employeename"].ToString();
                    }
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Employee Code Already Exist'); ", true);
                    txtEmpCodeNew.Text = "";
                }
                else
                {
                    btnSave.Focus();
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

        private void InsertRecord()
        {
            try
            {
                string strQry = "";
                bool result = false;

                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);

                strQry = "update M_Emp set OrgId=@OrgId, Employeecd=@Employeecd where OrgId=" + Convert.ToInt16(ddlPrvOrg.SelectedValue) + " and Employeecd='" + txtEmpCode.Text + "'";

                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter("@OrgId", Convert.ToInt16(ddlNewOrg.SelectedValue));
                para[1] = new SqlParameter("@Employeecd", txtEmpCodeNew.Text);

                //result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, para);
                if (!result)
                {
                    sqlTrans.Rollback();
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
                }

                //Employee Configuration
                string qryExist = "select * from M_EmpConfiguration where OrgId=" + Convert.ToInt16(ddlPrvOrg.SelectedValue) + " and Employeecd='" + txtEmpCode.Text + "'";
                DataTable dtExist = SqlHelper.ExecuteDataTable(qryExist, AppGlobal.strConnString);
                if (dtExist.Rows.Count > 0)
                {
                    strQry = "update M_EmpConfiguration set OrgId=@OrgId, Employeecd=@Employeecd where OrgId=" + Convert.ToInt16(ddlPrvOrg.SelectedValue) + " and Employeecd='" + txtEmpCode.Text + "'";

                    para = new SqlParameter[2];
                    para[0] = new SqlParameter("@OrgId", Convert.ToInt16(ddlNewOrg.SelectedValue));
                    para[1] = new SqlParameter("@Employeecd", txtEmpCodeNew.Text);

                    //result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                    result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, para);
                    if (!result)
                    {
                        sqlTrans.Rollback();
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
                    }
                }


                //Salary Configuration
                qryExist = "select * from M_Salary where OrgId=" + Convert.ToInt16(ddlPrvOrg.SelectedValue) + " and Employeecd='" + txtEmpCode.Text + "'";
                dtExist = SqlHelper.ExecuteDataTable(qryExist, AppGlobal.strConnString);
                if (dtExist.Rows.Count > 0)
                {
                    strQry = "update M_Salary set OrgId=@OrgId, Employeecd=@Employeecd where OrgId=" + Convert.ToInt16(ddlPrvOrg.SelectedValue) + " and Employeecd='" + txtEmpCode.Text + "'";

                    para = new SqlParameter[2];
                    para[0] = new SqlParameter("@OrgId", Convert.ToInt16(ddlNewOrg.SelectedValue));
                    para[1] = new SqlParameter("@Employeecd", txtEmpCodeNew.Text);

                    //result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                    result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, para);
                    if (!result)
                    {
                        sqlTrans.Rollback();
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
                    }
                }


                //Allowance Configuration
                qryExist = "select * from M_AllowanceConfig where OrgId=" + Convert.ToInt16(ddlPrvOrg.SelectedValue) + " and Employeecd='" + txtEmpCode.Text + "'";
                dtExist = SqlHelper.ExecuteDataTable(qryExist, AppGlobal.strConnString);
                if (dtExist.Rows.Count > 0)
                {
                    strQry = "update M_AllowanceConfig set OrgId=@OrgId, Employeecd=@Employeecd where OrgId=" + Convert.ToInt16(ddlPrvOrg.SelectedValue) + " and Employeecd='" + txtEmpCode.Text + "'";

                    para = new SqlParameter[2];
                    para[0] = new SqlParameter("@OrgId", Convert.ToInt16(ddlNewOrg.SelectedValue));
                    para[1] = new SqlParameter("@Employeecd", txtEmpCodeNew.Text);

                    //result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                    result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, para);
                    if (!result)
                    {
                        sqlTrans.Rollback();
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
                    }
                }


                //Leave Balance
                qryExist = "select * from T_LeaveBalance where OrgId=" + Convert.ToInt16(ddlPrvOrg.SelectedValue) + " and Employeecd='" + txtEmpCode.Text + "'";
                dtExist = SqlHelper.ExecuteDataTable(qryExist, AppGlobal.strConnString);
                if (dtExist.Rows.Count > 0)
                {
                    strQry = "update T_LeaveBalance set OrgId=@OrgId, Employeecd=@Employeecd where OrgId=" + Convert.ToInt16(ddlPrvOrg.SelectedValue) + " and Employeecd='" + txtEmpCode.Text + "'";

                    para = new SqlParameter[2];
                    para[0] = new SqlParameter("@OrgId", Convert.ToInt16(ddlNewOrg.SelectedValue));
                    para[1] = new SqlParameter("@Employeecd", txtEmpCodeNew.Text);

                    //result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                    result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, para);
                    if (!result)
                    {
                        sqlTrans.Rollback();
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
                    }
                }

                strQry = "";
                strQry = "INSERT INTO T_Log(OrgId,docdate, Employeecd, MenuId, Mode, Computername) VALUES(@OrgId,@docdate, @Employeecd, @MenuId, @Mode, @Computername)";

                SqlParameter[] paraLog = new SqlParameter[6];
                paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                paraLog[3] = new SqlParameter("@Mode", "M");
                paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());
                paraLog[5] = new SqlParameter("@docdate", Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy"));
                result = SqlHelper.ExecuteNonQuery(strQry, paraLog, AppGlobal.strConnString);
                if (result)
                {
                    sqlTrans.Commit();
                    clearControls();
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
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
            txtEmpCode.Text = "";
            txtEmpName.Text = "";
            txtEmpCodeNew.Text = "";
            txtEmpNameNew.Text = "";
            ddlPrvOrg.SelectedIndex = 0;
            ddlNewOrg.SelectedIndex = 0;

            btnSave.Text = "Save";
            ddlPrvOrg.Focus();
        }

        protected bool formValidation()
        {
            if (ddlPrvOrg.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Previous Oraganisation'); ", true);
                return false;
            }
            if (ddlNewOrg.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select New Oraganisation'); ", true);
                return false;
            }
            if (txtEmpCode.Text == "")
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter Previous Employee Code'); ", true);
                return false;
            }
            if (txtEmpCodeNew.Text == "")
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter New Employee Code'); ", true);
                return false;
            }

            string strQry = "select * from T_MonthlySalary where Employeecd='" + txtEmpCode.Text + "' and OrgId=" + Convert.ToInt16(ddlPrvOrg.SelectedValue);
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (objDT.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Employee Salary Already Processed, Cant Modify Now.'); ", true);
                return false;
            }
            return true;
        }
    }
}