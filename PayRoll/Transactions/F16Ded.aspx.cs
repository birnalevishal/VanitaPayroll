using SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PayRoll.Transactions
{
    public partial class F16Ded : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
                txtEmpCode_TextChanged(sender, e);
                BindGrid();
            }
            
        }

        private void BindData()
        {
            try
            {
                //txtEmpCode.Text = Session["UserName"].ToString();
                //txtEmpName.Text = Session["EmpName"].ToString();
                if (Session["form16"].ToString() == "Y")
                {
                    txtEmpCode.Text = Session["UserName"].ToString();
                    txtEmpName.Text = Session["EmpName"].ToString();
                    txtEmpCode.ReadOnly = true;
                    if (Session["RoleId"].ToString() == "7")
                    {
                        txtEmpCode.ReadOnly = false;
                    }
                   
                }
                else
                {
                    txtEmpCode.Text = "";
                    txtEmpName.Text = "";
                    txtEmpCode.ReadOnly = false;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
            }
        }

        private void BindGrid()
        {
            try
            {
               
                if (txtEmpCode.Text != "")
                {
                    string strQry = "SELECT CommCodeAdhno FROM M_Emp WHERE Employeecd='" + txtEmpCode.Text.Trim() + "' AND OrgId=" + Session["OrgId"].ToString();
                    string aadharNo = SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString).ToString();

                    strQry = "SELECT CONVERT(Varchar, YEAR(Frdate)) + '-' + CONVERT(varchar, YEAR(Todate)) AS ACYr  FROM  M_FinanceYear WHERE YearId=" + Session["YearID"].ToString();
                    string fy = SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString).ToString();
                    string[] yrs = fy.Split('-');
                    string frDt = yrs[0] + "04";
                    string toDt = yrs[1] + "03";


                    //Get Professional Tax Amount
                    strQry = "SELECT SUM(ProfTax) AS ProfTax FROM T_MonthlySalary INNER JOIN  M_Emp ON T_MonthlySalary.OrgId = M_Emp.OrgId AND T_MonthlySalary.Employeecd = M_Emp.Employeecd WHERE(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) BETWEEN " + frDt + " AND " + toDt + ") AND (M_Emp.CommCodeAdhno = '" + aadharNo + "')";
                    string Proftax = SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString).ToString();
                    if (Proftax == "")
                        Proftax = "0";

                    //Get Standard Deduction
                    strQry = "SELECT ISNULL(StdDed,0) AS StdDed FROM  M_F16StdDedRebate WHERE Yrno=" + Session["YearID"].ToString();
                    string StdDed = SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString).ToString();
                    if (StdDed == "")
                        StdDed = "0";

                    //Get PF Amount
                    strQry = "SELECT  SUM(Provfund)+SUM(pfpension)  AS Provfund FROM T_MonthlySalary INNER JOIN  M_Emp ON T_MonthlySalary.OrgId = M_Emp.OrgId AND T_MonthlySalary.Employeecd = M_Emp.Employeecd WHERE(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) BETWEEN " + frDt + " AND " + toDt + ") AND (M_Emp.Employeecd = '" + txtEmpCode.Text + "')";
                    string Provfund = SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString).ToString();
                    if (Provfund == "")
                        Provfund = "0";

                    //Get Data from Deductions Table
                    strQry = "SELECT * FROM T_F16Ded Where OrgId=" + Session["OrgID"].ToString() + " AND YearId=" + Session["YearID"].ToString() + " AND Employeecd='" + txtEmpCode.Text + "'";
                    DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                    if (objDT.Rows.Count > 0)
                    {
                        strQry = "SELECT  dbo.M_F16Ded.Srno,dbo.M_F16Ded.SubSrno, dbo.M_F16Ded.Section, dbo.M_F16Ded.DeductionOn, CASE WHEN dbo.M_F16Ded.Srno=2 and dbo.M_F16Ded.SubSrno=1 THEN " + Proftax + " WHEN dbo.M_F16Ded.Srno=3 and dbo.M_F16Ded.SubSrno=1 THEN " + StdDed + " WHEN dbo.M_F16Ded.Srno=4 and dbo.M_F16Ded.SubSrno=1 THEN " + Provfund + " ELSE ISNULL(dbo.T_F16Ded.Amt, 0) END AS Amt ";
                        strQry += " FROM  dbo.T_F16Ded RIGHT OUTER JOIN  dbo.M_F16Ded ON dbo.T_F16Ded.Srno = dbo.M_F16Ded.Srno and dbo.T_F16Ded.SubSrno = dbo.M_F16Ded.SubSrno ";
                        string strFilter = " Where dbo.T_F16Ded.OrgId = " + Session["OrgID"].ToString() + " AND dbo.T_F16Ded.YearId = " + Session["YearID"].ToString() + " AND dbo.T_F16Ded.Employeecd = '" + txtEmpCode.Text + "'";
                        strQry = strQry + strFilter;
                    }
                    else
                    {
                        strQry = @"SELECT  dbo.M_F16Ded.Srno,dbo.M_F16Ded.SubSrno, dbo.M_F16Ded.Section, dbo.M_F16Ded.DeductionOn,  CASE WHEN dbo.M_F16Ded.Srno=2 and dbo.M_F16Ded.SubSrno=1 THEN " + Proftax + " WHEN dbo.M_F16Ded.Srno=3 and dbo.M_F16Ded.SubSrno=1 THEN " + StdDed + " WHEN dbo.M_F16Ded.Srno=4 and dbo.M_F16Ded.SubSrno=1 THEN " + Provfund + " ELSE 0 END AS Amt   FROM  dbo.M_F16Ded ";
                    }
                    objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);

                    gvList.DataSource = objDT;
                    gvList.DataBind();

                    ViewState["objDTList"] = objDT;
                }
            }
            catch (Exception ex)
            {

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
                        RebindGrid();
                        InsertRecord();
                    }
                    else if (btnSave.Text == "Update")
                    {
                        UpdateRecord();
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error! Not Saved'); ", true);
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('" + ex.ToString() + "'); ", true);
            }
        }

        private void RebindGrid()
        {
            if (ViewState["objDTList"] != null)
            {
                DataTable objDT = (DataTable)ViewState["objDTList"];
                objDT.Columns["Amt"].ReadOnly = false;

                if (objDT.Rows.Count > 0)
                {
                    for (int i = 0; i <= objDT.Rows.Count - 1; i++)
                    {
                        //extract the TextBox values
                        TextBox box1 = (TextBox)gvList.Rows[i].Cells[3].FindControl("txtAmt");
                        objDT.Rows[i]["Amt"] = box1.Text;
                    }
                    ViewState["objDTList"] = objDT;
                }
            }
        }
        private void InsertRecord()
        {
            string strQry = "";
            bool result = false;

            DataTable objDT = (DataTable)ViewState["objDTList"];
            if (objDT.Rows.Count > 0)
            {
                strQry = @"DELETE FROM T_F16Ded WHERE OrgId=@OrgId AND YearId=@YearId AND Employeecd=@Employeecd";
                SqlParameter[] para = new SqlParameter[3];
                para[0] = new SqlParameter("@OrgId", Session["OrgID"]);
                para[1] = new SqlParameter("@YearId", Session["YearID"]);
                para[2] = new SqlParameter("@Employeecd", txtEmpCode.Text);

                result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
            }
            foreach (DataRow item in objDT.Rows)
            {
                strQry = @"INSERT INTO T_F16Ded(OrgId, YearId, Employeecd, Srno, subSrNo, Heading, Amt) 
                        VALUES(@OrgId, @YearId, @Employeecd, @Srno, @subSrNo, @Heading, @Amt)";

                SqlParameter[] para = new SqlParameter[7];
                para[0] = new SqlParameter("@OrgId", Session["OrgID"]);
                para[1] = new SqlParameter("@YearId", Session["YearID"]);
                para[2] = new SqlParameter("@Employeecd", txtEmpCode.Text);
                para[3] = new SqlParameter("@Srno", item["Srno"]);
                para[4] = new SqlParameter("@subSrNo", item["SubSrno"]);
                para[5] = new SqlParameter("@Heading", item["Section"]);

                para[6] = new SqlParameter("@Amt", item["Amt"]);

                result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
            }

            if (result)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
            }

        }

        private void UpdateRecord()
        {
            string strQry = "";
            bool result = false;

            //strQry = @"UPDATE T_IncTaxInv SET  Entertainment=@Entertainment, houseloaninterest=@houseloaninterest, InstalmentPayment=@InstalmentPayment, PublicPF=@PublicPF, saleforfamily=@saleforfamily, saleforparents=@saleforparents, LIC=@LIC, TutionFee=@TutionFee, Ssy=@Ssy, Donation=@Donation, 
            //          MutualFund=@MutualFund, PostRD=@PostRD, FD=@FD, Deduction1=@Deduction1, Deduction2=@Deduction2, Deduction3=@Deduction3, Deduction4=@Deduction4, Deduction5=@Deduction5 WHERE OrgId=@OrgId AND YearId=@YearId AND Employeecd=@Employeecd";

            //SqlParameter[] para = new SqlParameter[21];
            //para[0] = new SqlParameter("@OrgId", Session["OrgID"]);
            //para[1] = new SqlParameter("@Employeecd", txtEmpCode.Text);

            //para[20] = new SqlParameter("@YearId", Session["YearID"]);

            //result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

            if (result)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clearControls();
        }

        private void clearControls()
        {

        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox tb = (TextBox)e.Row.FindControl("txtAmt");
                tb.Text = Convert.ToDouble(tb.Text).ToString();

            }
        }

        protected void txtEmpCode_TextChanged(object sender, EventArgs e)
        {
            if (txtEmpCode.Text != "")
            {
                string strQry = "SELECT Employeename  FROM M_Emp Where Employeecd='" + txtEmpCode.Text + "' and OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and IsActive='Y'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    txtEmpName.Text = objDT.Rows[0]["Employeename"].ToString();
                    BindGrid();
                }
                else
                {
                    txtEmpName.Text = "";
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Wrong Employee Code'); ", true);
                    return;
                }
            }
        }
    }
}