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

namespace PayRoll.Transactions
{
    public partial class AdvProcess : System.Web.UI.Page
    {
        SqlConnection sqlConn = null;
        SqlCommand sqlCmd = null;
        SqlTransaction sqlTrans = null;

        string empName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                clearControls();
            }
        }

        private void BindData()
        {
            string strQry = "SELECT Year  FROM M_Year Where IsActive='Y' ORDER BY Year DESC";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlYear.DataSource = objDT;
            ddlYear.DataTextField = "Year";
            ddlYear.DataValueField = "Year";
            ddlYear.DataBind();

            ddlYear.Items.Insert(0, new ListItem("Select", "00"));

            strQry = "SELECT Employeename,Employeecd FROM M_Emp where OrgID=" + Convert.ToInt16(Session["OrgID"]) + " ORDER BY Employeename";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlEmpName.DataSource = objDT;
            ddlEmpName.DataTextField = "Employeename";
            ddlEmpName.DataValueField = "Employeecd";
            ddlEmpName.DataBind();
            ddlEmpName.Items.Insert(0, "Select");

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    if (ddlMon.SelectedIndex != 0 || ddlMon.SelectedIndex != 0)
                    {
                        AdvanceProcess(ddlMon.SelectedValue, ddlYear.SelectedValue, "", "2");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Month and Year '); ", true);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void AdvanceProcess(string month, string year, string empCode, string opr)
        {
            try
            {
                SqlConnection sqlConn = null;
                SqlCommand sqlCmd = null;
                SqlTransaction sqlTrans = null;
                string strQry = "";
                DataTable objDT;
                bool result = false;

                if (opr == "2")
                {
                    strQry = "select * from T_SalaryLock where OrgId=" + Convert.ToInt16(Session["orgID"]) + " and RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)>='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "' and Lock='Y'";
                    objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                    if (objDT.Rows.Count == 0)
                    {
                        clearControls();
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Salary not Locked for selected Month & Year. Please process after salary Lock'); ", true);
                        return;
                    }
                }

                //Get data from T_MonthlySalary table
                strQry = "SELECT OrgId, Docdate, MonYrcd, Employeecd, Advance FROM  T_MonthlySalary WHERE (Advance > 0) AND OrgId=" + Convert.ToInt16(Session["orgID"]) + " and RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "'";
                objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                foreach (DataRow item in objDT.Rows)
                {
                    //Check for Advance entry in T_EmpAdvanceHdr
                    strQry = "SELECT TOP(1) * FROM  dbo.T_EmpAdvanceHdr WHERE (OrgId = "+ Convert.ToInt16(Session["orgID"]) + ") AND (Employeecd = '" + item["Employeecd"].ToString() + "') AND (AdvApproved = 1) AND (AdvBalance > 0) ";
                    DataTable objDTHdr = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                    if (objDTHdr.Rows.Count > 0)
                    {
                        //Check for Advance entry in T_EmpAdvanceDtl
                        strQry = "SELECT TOP(1) * FROM  dbo.T_EmpAdvanceDtl WHERE(OrgId = " + Convert.ToInt16(Session["orgID"]) + ") AND (Employeecd = '" + item["Employeecd"].ToString() + "') AND (MonYrcd = '"+ item["MonYrcd"].ToString() + "') AND Advid=" + objDTHdr.Rows[0]["AdvId"].ToString();
                        DataTable objDTDtl = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                        if (objDTDtl.Rows.Count > 0)
                        {
                            strQry = @"UPDATE T_EmpAdvanceDtl SET PayAmount=@PayAmount, PayDt=@PayDt, Paid='Y' 
                                                WHERE OrgId=@OrgId AND Employeecd=@Employeecd AND AdvId=@AdvId AND MonYrcd=@MonYrcd";

                            SqlParameter[] para = new SqlParameter[6];
                            para[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                            para[1] = new SqlParameter("@Employeecd", item["Employeecd"].ToString());
                            para[2] = new SqlParameter("@AdvId", objDTHdr.Rows[0]["AdvId"].ToString());
                            para[3] = new SqlParameter("@MonYrcd", item["MonYrcd"].ToString());
                            para[4] = new SqlParameter("@PayAmount", item["Advance"].ToString());
                            para[5] = new SqlParameter("@PayDt", DateTime.Now.ToString("dd MMM yyyy"));

                            result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                        }
                        else
                        {
                            strQry = @"SELECT ISNULL(MAX(AdvPaySrNo),0) As AdvPaySrNo FROM T_EmpAdvanceDtl WHERE AdvId="+ objDTHdr.Rows[0]["AdvId"].ToString();
                            int nSrNo = Convert.ToInt32(SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString));

                            strQry = @"INSERT INTO T_EmpAdvanceDtl (OrgId, Employeecd, AdvId, AdvPaySrNo, MonYrcd, PayAmount, PayDt, Paid) 
                                        VALUES(@OrgId, @Employeecd, @AdvId, @AdvPaySrNo, @MonYrcd, @PayAmount, @PayDt, @Paid)";

                            SqlParameter[] para = new SqlParameter[8];
                            para[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                            para[1] = new SqlParameter("@Employeecd", item["Employeecd"].ToString());
                            para[2] = new SqlParameter("@AdvId", objDTHdr.Rows[0]["AdvId"].ToString());
                            para[3] = new SqlParameter("@AdvPaySrNo", nSrNo);
                            para[4] = new SqlParameter("@MonYrcd", item["MonYrcd"].ToString());
                            para[5] = new SqlParameter("@PayAmount", item["Advance"].ToString());
                            para[6] = new SqlParameter("@PayDt", DateTime.Now.ToString("dd MMM yyyy"));
                            para[7] = new SqlParameter("@Paid", "Y");

                            result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                        }


                        //Update Balannce Amount
                        double AdvanceAmount = Convert.ToDouble(objDTHdr.Rows[0]["AdvAmount"]);
                        strQry = @"SELECT ISNULL(SUM(PayAmount),0) As PayAmount FROM T_EmpAdvanceDtl WHERE Paid='Y' AND AdvId=" + objDTHdr.Rows[0]["AdvId"].ToString();
                        double nPayAmount = Convert.ToDouble(SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString));

                        strQry = @"UPDATE T_EmpAdvanceHdr SET AdvBalance=@AdvBalance 
                                                WHERE OrgId=@OrgId AND Employeecd=@Employeecd AND AdvId=@AdvId";

                        SqlParameter[] para1 = new SqlParameter[4];
                        para1[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                        para1[1] = new SqlParameter("@Employeecd", item["Employeecd"].ToString());
                        para1[2] = new SqlParameter("@AdvId", objDTHdr.Rows[0]["AdvId"].ToString());
                        para1[3] = new SqlParameter("@AdvBalance", AdvanceAmount - nPayAmount);

                        result = SqlHelper.ExecuteNonQuery(strQry, para1, AppGlobal.strConnString);

                    }
                }

                if (result)
                {
                    clearControls();
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Advance Processed Successfully!'); ", true);
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

        private void clearControls()
        {
            BindData();
            ddlMon.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;
            txtEmpCode.Text = "";
            ddlEmpName.SelectedIndex = 0;

            btnSave.Visible = true;
        }

        protected void txtEmpCode_TextChanged(object sender, EventArgs e)
        {
            if (txtEmpCode.Text != "")
            {
                string strQry = "SELECT Employeename  FROM M_Emp Where Employeecd='" + txtEmpCode.Text + "' and OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and IsActive='Y'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    ddlEmpName.SelectedValue = txtEmpCode.Text;
                }
                else
                {
                    ddlEmpName.SelectedIndex = 0;
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Wrong Employee Code'); ", true);
                    return;
                }
            }
        }

        protected void ddlEmpName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEmpName.SelectedIndex != 0)
            {
                txtEmpCode.Text = ddlEmpName.SelectedValue.ToString();
            }
        }

        protected void btnGetData_Click(object sender, EventArgs e)
        {

        }


    }
}