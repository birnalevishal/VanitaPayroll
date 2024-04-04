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
    public partial class OrganisationList : System.Web.UI.Page
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
                ReportDS.udf_organisationListDataTable dt1 = new ReportDS.udf_organisationListDataTable();
                ReportDSTableAdapters.udf_organisationListTableAdapter dt = new PayRoll.ReportDSTableAdapters.udf_organisationListTableAdapter();
                
                dt.Fill(dt1);
                DataTable dt3 = dt1;
                DataView dv = new DataView(dt3);
               
               
                ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptOrganisationList.rdlc");

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