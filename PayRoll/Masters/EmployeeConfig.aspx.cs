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
    public partial class EmployeeConfig : System.Web.UI.Page
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
            txtEmpCode.Focus();
        }

        private void BindData()
        {
            string strQry1 = "SELECT Category, Categcd FROM M_Category Where IsActive='Y' ORDER BY Category";
            DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            ddlCategory.DataSource = objDT1;
            ddlCategory.DataTextField = "Category";
            ddlCategory.DataValueField = "Categcd";
            ddlCategory.DataBind();

            ddlCategory.Items.Insert(0, new ListItem("Select", "0"));

            strQry1 = "SELECT Designation, Desigcd FROM M_Designation Where IsActive='Y' ORDER BY Designation";
            objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            ddlDesignation.DataSource = objDT1;
            ddlDesignation.DataTextField = "Designation";
            ddlDesignation.DataValueField = "Desigcd";
            ddlDesignation.DataBind();

            ddlDesignation.Items.Insert(0, new ListItem("Select", "0"));

            strQry1 = "SELECT LocationDep, LocDepCd FROM M_LocationDep Where IsActive='Y' ORDER BY LocationDep";
            objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            ddlDepartment.DataSource = objDT1;
            ddlDepartment.DataTextField = "LocationDep";
            ddlDepartment.DataValueField = "LocDepCd";
            ddlDepartment.DataBind();

            ddlDepartment.Items.Insert(0, new ListItem("Select", "0"));

            //strQry1 = "SELECT Employeename, Employeecd FROM M_Emp Where IsActive='Y' and HODInchAppl='Y' ORDER BY Employeename";
            //objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            //ddlHOD.DataSource = objDT1;
            //ddlHOD.DataTextField = "Employeename";
            //ddlHOD.DataValueField = "Employeecd";
            //ddlHOD.DataBind();

            //ddlHOD.Items.Insert(0, new ListItem("Select", "0"));

            strQry1 = "SELECT Division, Divcd FROM M_Division Where IsActive='Y' ORDER BY Division";
            objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            ddlDivision.DataSource = objDT1;
            ddlDivision.DataTextField = "Division";
            ddlDivision.DataValueField = "Divcd";
            ddlDivision.DataBind();

            ddlDivision.Items.Insert(0, new ListItem("Select", "0"));

            strQry1 = "SELECT Skill, Skillcd FROM M_Skill Where IsActive='Y' ORDER BY [Skill]";
            objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            ddlSkill.DataSource = objDT1;
            ddlSkill.DataTextField = "Skill";
            ddlSkill.DataValueField = "Skillcd";
            ddlSkill.DataBind();

            ddlSkill.Items.Insert(0, new ListItem("Select", "0"));

            strQry1 = "SELECT Status, Stacd FROM M_Status Where IsActive='Y' ORDER BY [Status]";
            objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            ddlStatus.DataSource = objDT1;
            ddlStatus.DataTextField = "Status";
            ddlStatus.DataValueField = "Stacd";
            ddlStatus.DataBind();

            ddlStatus.Items.Insert(0, new ListItem("Select", "0"));
        }

        private void BindGrid()
        {
             string strQry = @"select empCon.Employeecd as Employeecd, Docdate, div.Division as Divcd, loc.LocationDep as LocDepCd,  desi.Designation as Desigcd , emp.Employeename as HodInchEmpcd , cat.Category as Categcd, skill.Skill as Skillcd, sts.Status as Stacd 
                                from M_EmpConfiguration empCon
                                left outer join M_Division div   on empCon.Divcd = div.Divcd
                                left outer join M_LocationDep loc   on empCon.LocDepCd = loc.LocDepCd
                                left outer join M_Designation desi  on empCon.Desigcd = desi.Desigcd
                                left outer join M_Emp emp   on empCon.HodInchEmpcd = emp.Employeecd
                                left outer join M_Category cat   on empCon.Categcd = cat.Categcd
                                left outer join M_Skill skill on empCon.Skillcd = skill.Skillcd
                                left outer join M_Status sts   on empCon.Stacd = sts.Stacd
                                where empCon.Employeecd ='" + txtEmpCode.Text + "' and empCon.OrgId=" + Convert.ToInt32(Session["OrgID"]) + " order by Docdate desc";

            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvList.DataSource = objDT;
            gvList.DataBind();

            ViewState["objDTList"] = objDT;
        }

        protected void txtEmpCode_TextChanged(object sender, EventArgs e)
        {
            if (txtEmpCode.Text != string.Empty)
            {
                string strQry = "select * from M_Emp where Employeecd='" + txtEmpCode.Text + "' and OrgId=" + Convert.ToInt16(Session["OrgID"]);
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    BindGrid();
                    if (objDT.Rows[0]["Employeename"] != DBNull.Value)
                    {
                        txtEmpName.Text = objDT.Rows[0]["Employeename"].ToString();
                        txtDocDate.Focus();
                    }
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    if (formValidation())
                    {
                        string monthYear = "";

                        int month = Convert.ToDateTime(txtDocDate.Text).Month;
                        int year = Convert.ToDateTime(txtDocDate.Text).Year;

                        monthYear =  year + month.ToString("00") ;

                        string strQry = "select * from T_SalaryLock where OrgId=" + Convert.ToInt16(Session["orgID"]) + " and RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)>='" + monthYear + "' and Lock='Y'";
                        DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                        if (objDT.Rows.Count > 0)
                        {
                            clearControls();
                            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Salary Already Processed, Cant Modify Now.'); ", true);
                            return;
                        }
                       
                        if (btnSave.Text == "Save")
                        {
                            InsertRecord();
                        }
                        else if (btnSave.Text == "Update")
                        {
                            strQry = "select max(docdate) as DocDate from M_EMPConfiguration where orgid=" + Convert.ToInt16(Session["orgID"]) + " and Employeecd='" + txtEmpCode.Text + "'";
                            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                            if (objDT.Rows.Count > 0)
                            {
                                if(objDT.Rows[0]["DocDate"]!=DBNull.Value)
                                {
                                    if(Convert.ToDateTime(txtDocDate.Text)> Convert.ToDateTime(objDT.Rows[0]["DocDate"]))
                                    {
                                        clearControls();
                                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Cant Modify Document Date'); ", true);
                                        return;
                                    }
                                }
                            }

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
            try
            {
                string strQry = "";
                bool result = false;

                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);

                //strQry = "SELECT Count(*) FROM M_District Where District='" + txtName.Text.Trim() + "'";
                //int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
                //if (nCnt > 0)
                //{
                //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('District Name already exists!'); ", true);
                //    clearControls();
                //    return;
                //}

                strQry = @"INSERT INTO M_EmpConfiguration(OrgId, Employeecd, Docdate, MonYrcd, Divcd, LocDepCd, Desigcd, HodInchEmpcd, Categcd, Skillcd, Stacd) 
                                             VALUES(@OrgId, @Employeecd, @Docdate, @MonYrcd, @Divcd, @LocDepCd, @Desigcd, @HodInchEmpcd, @Categcd, @Skillcd, @Stacd)";

                SqlParameter[] para = new SqlParameter[11];
                para[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                para[1] = new SqlParameter("@Employeecd", txtEmpCode.Text);

                para[2] = new SqlParameter("@Docdate", Convert.ToDateTime(txtDocDate.Text).ToString("dd MMM yyyy"));
                para[3] = new SqlParameter("@Divcd", ddlDivision.SelectedValue);

                string monthYear = "";
                int month = Convert.ToDateTime(txtDocDate.Text).Month;
                int year = Convert.ToDateTime(txtDocDate.Text).Year;

                monthYear = month.ToString("00") + year;

                para[4] = new SqlParameter("@MonYrcd", monthYear);
                para[5] = new SqlParameter("@LocDepCd", ddlDepartment.SelectedValue);
                para[6] = new SqlParameter("@Desigcd", ddlDesignation.SelectedValue);
                para[7] = new SqlParameter("@HodInchEmpcd", txtHODEmpCode.Text);
                para[8] = new SqlParameter("@Categcd", ddlCategory.SelectedValue);
                para[9] = new SqlParameter("@Skillcd", ddlSkill.SelectedValue);
                para[10] = new SqlParameter("@Stacd", ddlStatus.SelectedValue);

                result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

                ////Update ConfigInfo into Master
                //string strQryEmp = @"select Divcd, LocDepCd,  Desigcd , HodInchEmpcd , Categcd, Skillcd, Stacd from M_Emp where  OrgId=" + Convert.ToInt32(Session["orgID"]) + " and Employeecd='" + txtEmpCode.Text + "'";
                //DataTable objDT = SqlHelper.ExecuteDataTable(strQryEmp, AppGlobal.strConnString);
                //if(objDT.Rows.Count>0)
                //{

                //}
                //strQry = "UPDATE M_EmpConfiguration SET Divcd=@Divcd, LocDepCd=@LocDepCd, Desigcd=@Desigcd, HodInchEmpcd=@HodInchEmpcd, Categcd=@Categcd, Skillcd=@Skillcd, Stacd=@Stacd WHERE OrgId=" + Convert.ToInt32(Session["orgID"]) + " and Employeecd='" + txtEmpCode.Text + "'";
                //SqlParameter[] para1 = new SqlParameter[9];

                //para[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                //para[1] = new SqlParameter("@Employeecd", txtEmpCode.Text);
                //para[2] = new SqlParameter("@Divcd", ddlDivision.SelectedValue);
                //para[3] = new SqlParameter("@LocDepCd", ddlDepartment.SelectedValue);
                //para[4] = new SqlParameter("@Desigcd", ddlDesignation.SelectedValue);
                //para[5] = new SqlParameter("@HodInchEmpcd", ddlHOD.SelectedValue);
                //para[6] = new SqlParameter("@Categcd", ddlCategory.SelectedValue);
                //para[7] = new SqlParameter("@Skillcd", ddlSkill.SelectedValue);
                //para[8] = new SqlParameter("@Stacd", ddlStatus.SelectedValue);

                //result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

                if (result)
                {
                    ViewState["EmpCode"] = txtEmpCode.Text;
                    ViewState["EmpName"] = txtEmpName.Text;

                   
                    txtEmpCode.Text = ViewState["EmpCode"].ToString();
                    txtEmpName.Text = ViewState["EmpName"].ToString();
                    BindGrid();

                    strQry = "";
                    strQry = "INSERT INTO T_Log(OrgId,docdate, Employeecd, MenuId, Mode, Computername) VALUES(@OrgId,@docdate, @Employeecd, @MenuId, @Mode, @Computername)";

                    SqlParameter[] paraLog = new SqlParameter[6];
                    paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                    paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                    paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                    paraLog[3] = new SqlParameter("@Mode", "A");
                    paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());
                    paraLog[5] = new SqlParameter("@docdate", Convert.ToDateTime(txtDocDate.Text).ToString("dd MMM yyyy"));

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
                int nId = Convert.ToInt32(ViewState["ID"]);

                string monthYear = "";

                int month = Convert.ToDateTime(txtDocDate.Text).Month;
                int year = Convert.ToDateTime(txtDocDate.Text).Year;

                monthYear = month.ToString("00") + year;

                strQry = "UPDATE M_EmpConfiguration SET Docdate=@Docdate, MonYrcd=@MonYrcd, Divcd=@Divcd, LocDepCd=@LocDepCd, Desigcd=@Desigcd, HodInchEmpcd=@HodInchEmpcd, Categcd=@Categcd, Skillcd=@Skillcd, Stacd=@Stacd WHERE OrgId=" + Convert.ToInt32(Session["orgID"]) + " and Employeecd='" + txtEmpCode.Text + "' and docDate = '" + Convert.ToDateTime(ViewState["DocDate"].ToString()).ToString("dd MMM yyyy") + "'";
                SqlParameter[] para = new SqlParameter[11];
                //para[0] = new SqlParameter("@OrgId", nId);
                //para[1] = new SqlParameter("@Employeecd", txtEmpCode.Text);
                //para[2] = new SqlParameter("@Docdate", Convert.ToDateTime(txtDocDate.Text));
                //para[3] = new SqlParameter("@Divcd", ddlDivision.SelectedValue);
                //para[4] = new SqlParameter("@MonYrcd", "");
                //para[5] = new SqlParameter("@LocDepCd", ddlDepartment.SelectedValue);
                //para[6] = new SqlParameter("@Desigcd", ddlDesignation.SelectedValue);
                //para[7] = new SqlParameter("@HodInchEmpcd", ddlHOD.SelectedValue);
                //para[8] = new SqlParameter("@Categcd", ddlCategory.SelectedValue);
                //para[9] = new SqlParameter("@Skillcd", ddlSkill.SelectedValue);
                //para[10] = new SqlParameter("@Stacd", ddlStatus.SelectedValue);

                para[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                para[1] = new SqlParameter("@Employeecd", txtEmpCode.Text);
                para[2] = new SqlParameter("@Docdate", Convert.ToDateTime(txtDocDate.Text).ToString("dd MMM yyyy"));
                para[3] = new SqlParameter("@Divcd", ddlDivision.SelectedValue);
                para[4] = new SqlParameter("@MonYrcd", monthYear);
                para[5] = new SqlParameter("@LocDepCd", ddlDepartment.SelectedValue);
                para[6] = new SqlParameter("@Desigcd", ddlDesignation.SelectedValue);
                para[7] = new SqlParameter("@HodInchEmpcd", txtHODEmpCode.Text);
                para[8] = new SqlParameter("@Categcd", ddlCategory.SelectedValue);
                para[9] = new SqlParameter("@Skillcd", ddlSkill.SelectedValue);
                para[10] = new SqlParameter("@Stacd", ddlStatus.SelectedValue);

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
            txtDocDate.Text = "";
            ddlDivision.SelectedIndex = 0;
            ddlDepartment.SelectedIndex = 0;
            ddlDesignation.SelectedIndex = 0;
            txtHODEmpCode.Text = "";
            txtHODEmpName.Text = "";
            ddlCategory.SelectedIndex = 0;
            ddlSkill.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;

            btnSave.Text = "Save";
            txtEmpCode.ReadOnly = false;
            txtEmpCode.Focus();

            gvList.DataSource = null;
            gvList.DataBind();
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
                    ViewRecord(i);
                    btnSave.Text = "Update";
                    txtEmpCode.Focus();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ViewRecord(int i)
        {
            GridViewRow row = gvList.Rows[i];

            string dt = row.Cells[1].Text; 
            string hodName= row.Cells[5].Text;
            string strQry = "SELECT * FROM M_EmpConfiguration Where orgID=" + Convert.ToInt32(Session["OrgID"]) + " and Employeecd=" + txtEmpCode.Text + "  and Docdate='" + Convert.ToDateTime(dt).ToString("dd MMM yyyy")  + "'";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (objDT.Rows.Count > 0)
            {
                txtDocDate.Text = Convert.ToDateTime(objDT.Rows[0]["Docdate"]).ToString("dd/MM/yyyy");
                ViewState["DocDate"]= Convert.ToDateTime(objDT.Rows[0]["Docdate"]).ToString("dd/MM/yyyy");
                ddlDivision.SelectedValue = objDT.Rows[0]["Divcd"] != DBNull.Value ? objDT.Rows[0]["Divcd"].ToString() : "0";
                ddlDepartment.SelectedValue = objDT.Rows[0]["LocDepCd"] != DBNull.Value ? objDT.Rows[0]["LocDepCd"].ToString() : "0";
                ddlDesignation.SelectedValue = objDT.Rows[0]["Desigcd"] != DBNull.Value ? objDT.Rows[0]["Desigcd"].ToString() : "0";
                txtHODEmpCode.Text = objDT.Rows[0]["HodInchEmpcd"] != DBNull.Value ? objDT.Rows[0]["HodInchEmpcd"].ToString() : "0";
                txtHODEmpName.Text = hodName;
                ddlCategory.SelectedValue = objDT.Rows[0]["Categcd"] != DBNull.Value ? objDT.Rows[0]["Categcd"].ToString() : "0";
                ddlSkill.SelectedValue = objDT.Rows[0]["Skillcd"] != DBNull.Value ? objDT.Rows[0]["Skillcd"].ToString() : "0";
                ddlStatus.SelectedValue = objDT.Rows[0]["Stacd"] != DBNull.Value ? objDT.Rows[0]["Stacd"].ToString() : "0";

            }
            txtEmpCode.ReadOnly = true;
            btnSave.Text = "Update";
        }

        protected bool formValidation()
        {
            if(ddlCategory.SelectedIndex==0 && ddlDepartment.SelectedIndex==0 && ddlDesignation.SelectedIndex==0 && ddlDivision.SelectedIndex==0 && txtHODEmpCode.Text=="" && ddlSkill.SelectedIndex==0 && ddlStatus.SelectedIndex==0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Atleast One From Above'); ", true);
                return false;
            }
            if (btnSave.Text == "Save")
            {
                string strQry = "SELECT Count(*) FROM M_EmpConfiguration Where OrgId=" + Convert.ToInt32(Session["OrgID"]) + " and Employeecd=" + txtEmpCode.Text + " and Docdate='" + Convert.ToDateTime(txtDocDate.Text.Trim()).ToString("dd MMM yyyy") + "'";
                int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
                if (nCnt > 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Record Already Exists!'); ", true);
                    //clearControls();
                    return false;
                }
            }
            return true;
        }

        protected void txtHODEmpCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string strQry = "";

                if (txtHODEmpCode.Text != "")
                {
                    strQry = "SELECT Employeecd,Employeename FROM M_Emp Where  Employeecd='" + txtHODEmpCode.Text + "'";
                    DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                    if (objDT1.Rows.Count==0)
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Wrong HOD Employee Code'); ", true);
                        txtHODEmpName.Text = "";
                        txtHODEmpCode.Text = "";
                        return;
                    }
                    txtHODEmpName.Text = objDT1.Rows[0]["EmployeeName"].ToString();
                    btnSave.Focus();
                }
                else
                {
                    txtHODEmpName.Text = "";
                }
                btnSave.Focus();
            }
            catch (Exception ex)
            {

            }
        }
    }
}