using SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PayRoll
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                Calendar1.SelectedDate = DateTime.Now;
                if (Session["RoleName"].ToString() == "Admin" || Session["RoleName"].ToString() == "Sub-Admin" || Session["RoleName"].ToString() == "SUPER ADMIN")
                {
                    BindGrid();
                    BindGridRetiring();
                    BindLeftEmployeePFWithdrawIntimationList();
                    lbtnRpt.Visible = true;
                }
                else
                {
                    lbtnRpt.Visible = false;
                }
            }
        }

        private void BindGrid()
        {
            string strQry = @"SELECT  Employeecd, Employeename, OrigJoindate, Period FROM udfPerformanceEvalDayCount("+ Session["OrgID"].ToString() + ") ";

            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvPerEvalList.DataSource = objDT;
            gvPerEvalList.DataBind();

            ViewState["EmpList"] = objDT;
        }

        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPerEvalList.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void lbtnRpt_Click(object sender, EventArgs e)
        {
            DataTable objDT = (DataTable)ViewState["EmpList"];
            if(objDT.Rows.Count>0)
            {
                string EmpCodes = "";
                foreach (DataRow item in objDT.Rows)
                {
                    EmpCodes += item["Employeecd"].ToString() + ",";
                }
                EmpCodes = EmpCodes.Remove(EmpCodes.Length - 1, 1);
                Response.Redirect("~/Reports/PerformanceEvaluationForm.aspx?EmpCodes=" + EmpCodes);
            }
        }

        //New Added to show Retiring Employee List: 20-06-2020
        private void BindGridRetiring()
        {
            string strQry = @"SELECT  Employeecd, Employeename, Birthdate, DaysLeft FROM udfEmpRetiringDayCount(" + Session["OrgID"].ToString() + ") ";

            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvRetiringEmpList.DataSource = objDT;
            gvRetiringEmpList.DataBind();

            //ViewState["RetiringList"] = objDT;
        }

        protected void gvRetiringEmpList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRetiringEmpList.PageIndex = e.NewPageIndex;
            BindGridRetiring();
        }

        protected void lbtnDBBackup_Click(object sender, EventArgs e)
        {
            try
            {
                // read backup folder from config file ("C:/temp/")
                var backupFolder = ConfigurationManager.AppSettings["BackupFolder"];



            }
            catch (Exception ex)
            {
                
            }
            
        }

        private void BindLeftEmployeePFWithdrawIntimationList()
        {
            string strQry = @"SELECT  Employeecd, Employeename, Leavedate, DaysFromLeftDate FROM udfLeftEmployeePFWithdrawIntimation(" + Session["OrgID"].ToString() + ") ";

            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvLeftEmployeeList.DataSource = objDT;
            gvLeftEmployeeList.DataBind();
        }
        protected void gvLeftEmployeeList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLeftEmployeeList.PageIndex = e.NewPageIndex;
            BindLeftEmployeePFWithdrawIntimationList();
        }


    }
}