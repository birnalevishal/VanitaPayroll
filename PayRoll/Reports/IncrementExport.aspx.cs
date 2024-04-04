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
    public partial class IncrementExport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             if(!IsPostBack)
            {
               
            }
        }
        private void BindData()
        {
            //string strQry1 = "SELECT Category, Categcd FROM M_Category Where IsActive='Y' ORDER BY Category";
            //DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
            //ddlCategory.DataSource = objDT1;
            //ddlCategory.DataTextField = "Category";
            //ddlCategory.DataValueField = "Categcd";
            //ddlCategory.DataBind();

            //ddlCategory.Items.Insert(0, new ListItem("Select", "0"));

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
               
                DataTable dtEmployee = new DataTable();
                DataRow dr;
                dtEmployee.Columns.Add("SrNo", typeof(string));
                dtEmployee.Columns.Add("Employeecd", typeof(string));
                dtEmployee.Columns.Add("Employeename", typeof(string));
                dtEmployee.Columns.Add("OrgJoinDate", typeof(string));
                dtEmployee.Columns.Add("PrvGross", typeof(string));

                string strQry = " SELECT dbo.M_Emp.Employeecd, dbo.M_Emp.Employeename, dbo.M_Emp.OrigJoindate, udf_EmpSalMaster_1.Gross ";
                strQry += " FROM dbo.M_Emp LEFT OUTER JOIN dbo.udf_EmpSalMaster(" + Convert.ToInt16(Session["OrgID"]) + ",'" + Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy") + "') AS udf_EmpSalMaster_1 ON dbo.M_Emp.OrgId = udf_EmpSalMaster_1.OrgId AND";
                strQry += " dbo.M_Emp.Employeecd = udf_EmpSalMaster_1.Employeecd where dbo.M_Emp.OrgId=" + Convert.ToInt16(Session["OrgID"]);
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    for (int i = 0; i < objDT.Rows.Count; i++)
                    {
                        dr = dtEmployee.NewRow();
                        dr["SrNo"] = i + 1;
                        dr["Employeecd"] = objDT.Rows[i]["Employeecd"].ToString();
                        dr["Employeename"] = objDT.Rows[i]["Employeename"].ToString();
                        dr["OrgJoinDate"] = objDT.Rows[i]["OrigJoindate"] != DBNull.Value ? Convert.ToDateTime(objDT.Rows[i]["OrigJoindate"]).ToString("dd/MM/yyyy") : "";
                        dr["PrvGross"] = objDT.Rows[i]["Gross"] != DBNull.Value ? Convert.ToDouble(objDT.Rows[i]["Gross"]).ToString("0") : "0";
                        dtEmployee.Rows.Add(dr);
                    }
                }

           
                DataView dv = new DataView(dtEmployee);
               
               
                ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptIncrementExp.rdlc");

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
    }
}