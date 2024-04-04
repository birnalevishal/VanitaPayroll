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
    public partial class LWFReport : System.Web.UI.Page
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
            ddlYear.DataBind();

            ddlYear.Items.Insert(0, new ListItem("Select", "0000"));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (rblType.SelectedValue == "")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Report Type'); ", true);
                    return;
                }
                if (ddlMon.SelectedIndex==0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Month'); ", true);
                    return;
                }
                if (ddlYear.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Year'); ", true);
                    return;
                }
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                if (rblType.SelectedValue == "1")
                {
                    int lastDay = 0;
                    lastDay = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMon.SelectedValue));
                    string salaryDate = ddlYear.SelectedValue + "/" + ddlMon.SelectedValue + "/" + lastDay;

                    dsMaster.udf_LWFDataTable dt1 = new dsMaster.udf_LWFDataTable();
                    dsMasterTableAdapters.udf_LWFTableAdapter dt = new dsMasterTableAdapters.udf_LWFTableAdapter();

                    dt.Fill(dt1, ddlMon.SelectedValue + ddlYear.SelectedValue, ddlYear.SelectedValue + ddlMon.SelectedValue, Convert.ToDateTime(salaryDate), Convert.ToInt16(Session["OrgID"]));
                    DataTable dt3 = dt1;
                    DataTable dtSal = calculation();

                    DataView dv = new DataView(dt3);
                    DataView dvSal = new DataView(dtSal);

                    ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                    ReportDataSource datasourceSal = new ReportDataSource("DataSet2", dvSal.ToTable());
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.DataSources.Add(datasourceSal);

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptLWF.rdlc");
                }
                else
                {
                    dsMaster.udf_LWFDetDataTable dt1 = new dsMaster.udf_LWFDetDataTable();
                    dsMasterTableAdapters.udf_LWFDetTableAdapter dt = new dsMasterTableAdapters.udf_LWFDetTableAdapter();

                    dt.Fill(dt1, ddlMon.SelectedValue + ddlYear.SelectedValue, Convert.ToInt16(Session["OrgID"]));
                    DataTable dt3 = dt1;
                    DataView dv = new DataView(dt3);

                    ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptLWFDet.rdlc");
                }
                ReportParameter[] p = new ReportParameter[3];
                p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                p[1] = new ReportParameter("MonYrCd", ddlMon.SelectedItem.Text +" "  +  ddlYear.SelectedItem.Text, true);
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
                ddlMon.SelectedIndex = 0;
                ddlYear.SelectedIndex = 0;
                ReportViewer1.LocalReport.DataSources.Clear();
                rblType.SelectedValue = "";
                ddlMon.Focus();
            }
            catch(Exception ex)
            {

            }

        }
        protected DataTable calculation()
        {
            DataTable dtReport = new DataTable();
            dtReport.Columns.Add("countSal", typeof(long));
            dtReport.Columns.Add("LWF", typeof(double));
            dtReport.Columns.Add("LWFComp", typeof(double));
          
            DataRow dr;
            int lastDay = 0;
            lastDay = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMon.SelectedValue));
            string salaryDate = ddlYear.SelectedValue + "/" + ddlMon.SelectedValue + "/" + lastDay;
            int count = 0;
            int lwf = 0, lwfcompcontri = 0;
            string qryOpnEmp = "select sum(count) as count ,SUM(LWF) As LWF, SUM(LWFCompContri) As LWFCompContri FROM udf_LWF('" + ddlMon.SelectedValue + ddlYear.SelectedValue + "','" + ddlYear.SelectedValue + ddlMon.SelectedValue + "','" + Convert.ToDateTime(salaryDate).ToString("dd MMM yyyy") + "'," + Convert.ToInt16(Session["OrgID"]) + ")   group by LWF, LWFCompContri"; //where Expr1 ='O'
            DataTable objOpnEmp = SqlHelper.ExecuteDataTable(qryOpnEmp, AppGlobal.strConnString);
            if (objOpnEmp.Rows.Count > 0)
            {
                for(int i=0;i<objOpnEmp.Rows.Count;i++)
                {
                    count += Convert.ToInt16(objOpnEmp.Rows[i]["count"]);
                    if (Convert.ToInt16(objOpnEmp.Rows[i]["LWF"]) != 0)
                        lwf = Convert.ToInt16(objOpnEmp.Rows[i]["LWF"]);
                    if (Convert.ToInt16(objOpnEmp.Rows[i]["LWFCompContri"]) != 0)
                        lwfcompcontri = Convert.ToInt16(objOpnEmp.Rows[i]["LWFCompContri"]);

                }
                dr = dtReport.NewRow();
        
                dr["countSal"] = count;
                dr["LWF"] = lwf;
                dr["LWFComp"] = lwfcompcontri;
              
                dtReport.Rows.Add(dr);
            }
            
            return dtReport;
        }
    }
}