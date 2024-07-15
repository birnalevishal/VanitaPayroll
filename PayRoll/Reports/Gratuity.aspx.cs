using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SqlClient;

namespace PayRoll.Reports
{
    public partial class Gratuity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             if(!IsPostBack)
            {
                BindData();
            }
        }
        private void BindData()
        {
            string strQry = "SELECT Year  FROM M_Year Where IsActive='Y' ORDER BY Year desc";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlYear.DataSource = objDT;
            ddlYear.DataTextField = "Year";
            ddlYear.DataValueField = "Year";

            ddlToYear.DataSource = objDT;
            ddlToYear.DataTextField = "Year";
            ddlToYear.DataValueField = "Year";

            ddlYear.DataBind();
            ddlToYear.DataBind();

            ddlYear.Items.Insert(0, new ListItem("Select", "0000"));
            ddlToYear.Items.Insert(0, new ListItem("Select", "0000"));

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                DataSet1.udf_GratuityDataTable dt1 = new DataSet1.udf_GratuityDataTable();
                DataSet1TableAdapters.udf_GratuityTableAdapter dt = new DataSet1TableAdapters.udf_GratuityTableAdapter();
                string monYrCd = "";
                monYrCd = Convert.ToInt16(Convert.ToDateTime(DateTime.Now).ToString("MM")).ToString("00");
                monYrCd += Convert.ToInt16(Convert.ToDateTime(DateTime.Now).ToString("yyyy")).ToString("00");

                string fromDate = "01/" + ddlMon.SelectedValue + "/" + ddlYear.SelectedValue;
                int lastDay = 0;
                lastDay = DateTime.DaysInMonth(Convert.ToInt32(ddlToYear.SelectedValue), Convert.ToInt32(ddlToMonth.SelectedValue));
                string toDate = lastDay +"/" + ddlToMonth.SelectedValue + "/" + ddlToYear.SelectedValue;

                // dt.Fill(dt1,Convert.ToInt16(Session["OrgID"]), Convert.ToDateTime(DateTime.Now), Convert.ToDateTime(fromDate),Convert.ToDateTime(toDate));
                 
                DataTable dt3 = gratuity();
                DataView dv = new DataView(dt3);
               
               
                ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptGratuity.rdlc");

                ReportParameter[] p = new ReportParameter[3];
                p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                p[1] = new ReportParameter("asOnDate", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy"), true);
                //--- To Display Logo -----------------------------------
                ReportViewer1.LocalReport.EnableExternalImages = true;
                string strqry = "select LogoPath from M_Organization where OrgId=" + Convert.ToInt32(Session["OrgID"]);
                DataTable objDTP = SqlHelper.ExecuteDataTable(strqry, AppGlobal.strConnString);
                string path = "";

                if (objDTP.Rows[0]["LogoPath"] != DBNull.Value)
                    path = new Uri(Server.MapPath(objDTP.Rows[0]["LogoPath"].ToString())).AbsoluteUri;
                else
                    path = new Uri(Server.MapPath("~/Upload/Logo.png")).AbsoluteUri;

                p[2] = new ReportParameter("LogoPath", path, true);
                //-----------------------------------------------------
                this.ReportViewer1.LocalReport.SetParameters(p);


                ReportViewer1.LocalReport.Refresh();
            }
            catch(Exception ex)
            {

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
               // txtEmpName.Text = "";
                
                ReportViewer1.LocalReport.DataSources.Clear();
                ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:txtVisible();", true);

            }
            catch(Exception ex)
            {

            }

        }
        protected void ddlMon_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlToMonth.SelectedValue = ddlMon.SelectedValue;
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlToYear.SelectedValue = ddlYear.SelectedValue;
        }

        protected DataTable gratuity()
        {
            int ageFlag = 0;
            int leaveFlag = 0;
            string fromDate = "01/" + ddlMon.SelectedValue + "/" + ddlYear.SelectedValue;
            int lastDay = 0;
            lastDay = DateTime.DaysInMonth(Convert.ToInt32(ddlToYear.SelectedValue), Convert.ToInt32(ddlToMonth.SelectedValue));
            string toDate = lastDay + "/" + ddlToMonth.SelectedValue + "/" + ddlToYear.SelectedValue;


            DataTable dtReport = new DataTable();
            dtReport.Columns.Add("LIC_PolicyNo", typeof(string));
            dtReport.Columns.Add("LIC_Id", typeof(string));
            dtReport.Columns.Add("Employeecd", typeof(string));
            dtReport.Columns.Add("Category", typeof(string));
            dtReport.Columns.Add("Birthdate", typeof(string));
            dtReport.Columns.Add("OrigJoindate", typeof(string));
            dtReport.Columns.Add("BasicDA", typeof(string));
            dtReport.Columns.Add("Freequency", typeof(string));
            dtReport.Columns.Add("EmployeeName", typeof(string));
            dtReport.Columns.Add("Gender", typeof(string));
            dtReport.Columns.Add("Gratuitydate", typeof(string));
          
            DataRow dr;

            string strQry = "";
            DataTable objDT;
            strQry = " SELECT M_Gender.Gender, M_Emp_1.Birthdate, M_Emp_1.DatofJoin, M_Emp_1.OrigJoindate, M_Category.Category, M_Emp_1.Employeecd, M_Emp_1.Employeename, cat.conId AS catid, M_Emp_1.Gratuitydate, udf_EmpSalMaster_1.BasicDA, (SELECT years FROM dbo.udfTimeSpan(M_Emp_1.Birthdate, '" + Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy") + "') AS udfTimeSpan_6) AS Age, M_Emp_1.LIC_PolicyNo, M_Emp_1.LIC_Id, g.Amount ";
            strQry += " FROM M_Emp AS M_Emp_1 LEFT OUTER JOIN dbo.udfEmployeesalarymax(" + Convert.ToInt16(Session["OrgID"])  + ",'"+ ddlToYear.SelectedValue+ ddlToMonth.SelectedValue + "') AS udf_EmpSalMaster_1 ON M_Emp_1.Employeecd = udf_EmpSalMaster_1.Employeecd AND M_Emp_1.OrgId = udf_EmpSalMaster_1.OrgId ";
            strQry += " LEFT OUTER JOIN M_Gender ON M_Emp_1.Gendercd = M_Gender.Gendercd ";
            strQry += " LEFT OUTER JOIN dbo.udfEmpConfigurationmax1(" + Convert.ToInt16(Session["OrgID"]) + ",'" + Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy") + "', 'cat') AS cat ON M_Emp_1.OrgId = cat.OrgId AND M_Emp_1.Employeecd = cat.Employeecd";
            strQry += " LEFT OUTER JOIN M_Category ON cat.conId = M_Category.Categcd";
            strQry += " LEFT OUTER JOIN udfGratuitymax(" + Convert.ToInt16(Session["OrgID"]) + ",'" + ddlYear.SelectedValue + ddlMon.SelectedValue + "') AS g ON M_Emp_1.OrgId = g.OrgID AND M_Emp_1.Employeecd = g.Employeecd ";
            strQry += " WHERE(M_Emp_1.OrgId = " + Convert.ToInt16(Session["OrgID"]) + ") and M_Emp_1.isActive = 'Y' and (M_Emp_1.Leavedate is null) and (SELECT years FROM dbo.udfTimeSpan(M_Emp_1.Birthdate, '" + Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy") + "') AS udfTimeSpan_6) < 58 and M_Emp_1.OrigJoindate<'" + Convert.ToDateTime(toDate).ToString("dd MMM yyyy") + "' and cat.conId <> 3";
         
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if(objDT.Rows.Count>0)
            {
                for(int i=0;i<objDT.Rows.Count;i++)
                {
                    dr = dtReport.NewRow();
                    dr["LIC_PolicyNo"] = objDT.Rows[i]["LIC_PolicyNo"].ToString();
                    dr["LIC_Id"] = objDT.Rows[i]["LIC_Id"].ToString();
                    dr["Employeecd"] = objDT.Rows[i]["Employeecd"].ToString();
                    dr["Category"] = objDT.Rows[i]["Category"].ToString();
                    if (objDT.Rows[i]["Birthdate"] != DBNull.Value)
                        dr["Birthdate"] = Convert.ToDateTime(objDT.Rows[i]["Birthdate"]).ToString("dd/MM/yyyy");

                    dr["OrigJoindate"] = Convert.ToDateTime(objDT.Rows[i]["OrigJoindate"]).ToString("dd/MM/yyyy"); 
                    dr["BasicDA"] = objDT.Rows[i]["BasicDA"].ToString();
                    dr["Freequency"] = "M";
                    dr["EmployeeName"] = objDT.Rows[i]["Employeename"].ToString();
                    dr["Gender"] = objDT.Rows[i]["Gender"].ToString();
                    if (objDT.Rows[i]["Gratuitydate"] != DBNull.Value)
                        dr["Gratuitydate"] = Convert.ToDateTime(objDT.Rows[i]["Gratuitydate"]).ToString("dd/MM/yyyy");

                    dtReport.Rows.Add(dr);
                }
            }

            dr = dtReport.NewRow();
            dr["LIC_PolicyNo"] = "Completed Age of 58 years";
            dtReport.Rows.Add(dr);

            //strQry = "";
            strQry = " SELECT M_Gender.Gender, M_Emp_1.Birthdate, M_Emp_1.DatofJoin, M_Emp_1.OrigJoindate, M_Category.Category, M_Emp_1.Employeecd, M_Emp_1.Employeename, cat.conId AS catid, M_Emp_1.Gratuitydate, udf_EmpSalMaster_1.BasicDA, (SELECT years FROM dbo.udfTimeSpan(M_Emp_1.Birthdate, '" + Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy") + "') AS udfTimeSpan_6) AS Age, M_Emp_1.LIC_PolicyNo, M_Emp_1.LIC_Id, g.Amount ";
            strQry += " FROM M_Emp AS M_Emp_1 LEFT OUTER JOIN dbo.udfEmployeesalarymax(" + Convert.ToInt16(Session["OrgID"]) + ",'" + ddlToYear.SelectedValue + ddlToMonth.SelectedValue + "') AS udf_EmpSalMaster_1 ON M_Emp_1.Employeecd = udf_EmpSalMaster_1.Employeecd AND M_Emp_1.OrgId = udf_EmpSalMaster_1.OrgId ";
            strQry += " LEFT OUTER JOIN M_Gender ON M_Emp_1.Gendercd = M_Gender.Gendercd ";
            strQry += " LEFT OUTER JOIN dbo.udfEmpConfigurationmax1(" + Convert.ToInt16(Session["OrgID"]) + ",'" + Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy") + "', 'cat') AS cat ON M_Emp_1.OrgId = cat.OrgId AND M_Emp_1.Employeecd = cat.Employeecd";
            strQry += " LEFT OUTER JOIN M_Category ON cat.conId = M_Category.Categcd";
            strQry += " LEFT OUTER JOIN udfGratuitymax(" + Convert.ToInt16(Session["OrgID"]) + ",'" + ddlYear.SelectedValue + ddlMon.SelectedValue + "') AS g ON M_Emp_1.OrgId = g.OrgID AND M_Emp_1.Employeecd = g.Employeecd ";
            strQry += " WHERE(M_Emp_1.OrgId = " + Convert.ToInt16(Session["OrgID"]) + ") and M_Emp_1.isActive = 'Y' and M_Emp_1.Leavedate is null and (SELECT years FROM dbo.udfTimeSpan(M_Emp_1.Birthdate, '" + Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy") + "') AS udfTimeSpan_6) >= 58 and M_Emp_1.OrigJoindate<'" + Convert.ToDateTime(toDate).ToString("dd MMM yyyy") + "'and cat.conId <> 3";            

            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (objDT.Rows.Count > 0)
            {
                for (int i = 0; i < objDT.Rows.Count; i++)
                {
                    dr = dtReport.NewRow();
                    dr["LIC_PolicyNo"] = objDT.Rows[i]["LIC_PolicyNo"].ToString();
                    dr["LIC_Id"] = objDT.Rows[i]["LIC_Id"].ToString();
                    dr["Employeecd"] = objDT.Rows[i]["Employeecd"].ToString();
                    dr["Category"] = objDT.Rows[i]["Category"].ToString();
                    if (objDT.Rows[i]["Birthdate"] != DBNull.Value)
                        dr["Birthdate"] = Convert.ToDateTime(objDT.Rows[i]["Birthdate"]).ToString("dd/MM/yyyy");

                    dr["OrigJoindate"] = Convert.ToDateTime(objDT.Rows[i]["OrigJoindate"]).ToString("dd/MM/yyyy");
                    dr["BasicDA"] = objDT.Rows[i]["BasicDA"].ToString();
                    dr["Freequency"] = "M";
                    dr["EmployeeName"] = objDT.Rows[i]["Employeename"].ToString();
                    dr["Gender"] = objDT.Rows[i]["Gender"].ToString();
                    if (objDT.Rows[i]["Gratuitydate"] != DBNull.Value)
                        dr["Gratuitydate"] = Convert.ToDateTime(objDT.Rows[i]["Gratuitydate"]).ToString("dd/MM/yyyy");

                    dtReport.Rows.Add(dr);
                }
            }

            dr = dtReport.NewRow();
            dr["LIC_PolicyNo"] = "Employees left during the year";
            dtReport.Rows.Add(dr);

            strQry = "";

            strQry = " SELECT M_Gender.Gender, M_Emp_1.Birthdate, M_Emp_1.DatofJoin, M_Emp_1.OrigJoindate, M_Category.Category, M_Emp_1.Employeecd, M_Emp_1.Employeename, cat.conId AS catid, M_Emp_1.Gratuitydate, udf_EmpSalMaster_1.BasicDA, (SELECT years FROM dbo.udfTimeSpan(M_Emp_1.Birthdate, '" + Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy") + "') AS udfTimeSpan_6) AS Age, M_Emp_1.LIC_PolicyNo, M_Emp_1.LIC_Id, g.Amount ";
            strQry += " FROM M_Emp AS M_Emp_1 LEFT OUTER JOIN dbo.udfEmployeesalarymax(" + Convert.ToInt16(Session["OrgID"]) + ",'" + ddlToYear.SelectedValue + ddlToMonth.SelectedValue + "') AS udf_EmpSalMaster_1 ON M_Emp_1.Employeecd = udf_EmpSalMaster_1.Employeecd AND M_Emp_1.OrgId = udf_EmpSalMaster_1.OrgId ";
            strQry += " LEFT OUTER JOIN M_Gender ON M_Emp_1.Gendercd = M_Gender.Gendercd ";
            strQry += " LEFT OUTER JOIN dbo.udfEmpConfigurationmax1(" + Convert.ToInt16(Session["OrgID"]) + ",'" + Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy") + "', 'cat') AS cat ON M_Emp_1.OrgId = cat.OrgId AND M_Emp_1.Employeecd = cat.Employeecd";
            strQry += " LEFT OUTER JOIN M_Category ON cat.conId = M_Category.Categcd";
            strQry += " LEFT OUTER JOIN udfGratuitymax(" + Convert.ToInt16(Session["OrgID"]) + ",'" + ddlYear.SelectedValue + ddlMon.SelectedValue + "') AS g ON M_Emp_1.OrgId = g.OrgID AND M_Emp_1.Employeecd = g.Employeecd ";
            strQry += " WHERE(M_Emp_1.OrgId = " + Convert.ToInt16(Session["OrgID"]) + ") and M_Emp_1.isActive = 'N' and M_Emp_1.OrigJoindate < '" + Convert.ToDateTime(fromDate).ToString("dd MMM yyyy") + "'  and M_Emp_1.Leavedate is not null and M_Emp_1.Leavedate>='" + Convert.ToDateTime(fromDate).ToString("dd MMM yyyy") + "' and M_Emp_1.Leavedate<='" + Convert.ToDateTime(toDate).ToString("dd MMM yyyy") + "' and M_Emp_1.OrigJoindate<'" + Convert.ToDateTime(toDate).ToString("dd MMM yyyy") + "'and cat.conId <> 3";

            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (objDT.Rows.Count > 0)
            {
                for (int i = 0; i < objDT.Rows.Count; i++)
                {
                    dr = dtReport.NewRow();
                    dr["LIC_PolicyNo"] = objDT.Rows[i]["LIC_PolicyNo"].ToString();
                    dr["LIC_Id"] = objDT.Rows[i]["LIC_Id"].ToString();
                    dr["Employeecd"] = objDT.Rows[i]["Employeecd"].ToString();
                    dr["Category"] = objDT.Rows[i]["Category"].ToString();
                    if (objDT.Rows[i]["Birthdate"] != DBNull.Value)
                        dr["Birthdate"] = Convert.ToDateTime(objDT.Rows[i]["Birthdate"]).ToString("dd/MM/yyyy");

                    dr["OrigJoindate"] = Convert.ToDateTime(objDT.Rows[i]["OrigJoindate"]).ToString("dd/MM/yyyy");
                    dr["BasicDA"] = objDT.Rows[i]["BasicDA"].ToString();
                    dr["Freequency"] = "M";
                    dr["EmployeeName"] = objDT.Rows[i]["Employeename"].ToString();
                    dr["Gender"] = objDT.Rows[i]["Gender"].ToString();
                    if (objDT.Rows[i]["Gratuitydate"] != DBNull.Value)
                        dr["Gratuitydate"] = Convert.ToDateTime(objDT.Rows[i]["Gratuitydate"]).ToString("dd/MM/yyyy");

                    dtReport.Rows.Add(dr);
                }
            }
            return dtReport;
        }

        public int getAge(DateTime Dob)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1;
            DateTime PastYearDate = Dob.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
            int Hours = Now.Subtract(PastYearDate).Hours;
            int Minutes = Now.Subtract(PastYearDate).Minutes;
            int Seconds = Now.Subtract(PastYearDate).Seconds;
            if (Years == 58 && (Months > 0 || Days > 0))
            {
                Years += 1;
            }

            //return String.Format("Age: {0} Year(s) {1} Month(s) {2} Day(s) {3} Hour(s) {4} Second(s)",
            //Years, Months, Days, Hours, Seconds);
            return Years;
        }
    }
}