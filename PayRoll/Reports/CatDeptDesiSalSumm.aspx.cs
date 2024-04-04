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
    public partial class CatDeptDesiSalSumm : System.Web.UI.Page
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

            strQry = "SELECT Categcd, Category FROM M_Category Where IsActive='Y' ORDER BY Category";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlCategory.DataSource = objDT;
            ddlCategory.DataTextField = "Category";
            ddlCategory.DataValueField = "Categcd";
            ddlCategory.DataBind();

            ddlCategory.Items.Insert(0, new ListItem("All", "0"));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlMon.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Month'); ", true);
                    return;
                }
                if (ddlYear.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Year'); ", true);
                    return;
                }
                int lastDay = 0;
                lastDay = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMon.SelectedValue));
                string salaryDate = ddlYear.SelectedValue + "/" + ddlMon.SelectedValue + "/" + lastDay;

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                //Category
                if (rblType.SelectedValue == "1")   
                {
                    DataSet1.udf_CategorySalDataTable dt1 = new DataSet1.udf_CategorySalDataTable();

                    DataSet1TableAdapters.udf_CategorySalTableAdapter dt = new DataSet1TableAdapters.udf_CategorySalTableAdapter();
                    dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]), ddlMon.SelectedValue + ddlYear.SelectedValue, Convert.ToDateTime(salaryDate));
                    DataTable dt3 = dt1;
                    DataView dv = new DataView(dt3);
                    string filter = "";


                    if (ddlCategory.SelectedIndex != 0)
                    {
                        filter += " Categcd = " + ddlCategory.SelectedValue;
                        dv.RowFilter = filter;
                    }

                    ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptCategoryWiseSumm.rdlc");

                }
                //Department
                else if (rblType.SelectedValue=="2")
                {
                    DataSet1.udf_DepartmentSalDataTable dt1 = new DataSet1.udf_DepartmentSalDataTable();

                    DataSet1TableAdapters.udf_DepartmentSalTableAdapter dt = new DataSet1TableAdapters.udf_DepartmentSalTableAdapter();
                    dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]), ddlMon.SelectedValue + ddlYear.SelectedValue, Convert.ToDateTime(salaryDate));
                    DataTable dt3 = dt1;
                    DataView dv = new DataView(dt3);
                    string filter = "";


                    if (ddlCategory.SelectedIndex != 0)
                    {
                        filter += " locDepCd = " + ddlCategory.SelectedValue;
                        dv.RowFilter = filter;
                    }

                    ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptDepartmentWiseSumm.rdlc");

                }
                //Designation
                else if (rblType.SelectedValue=="3")
                {
                    DataSet1.udf_DesiSalDataTable dt1 = new DataSet1.udf_DesiSalDataTable();

                    DataSet1TableAdapters.udf_DesiSalTableAdapter dt = new DataSet1TableAdapters.udf_DesiSalTableAdapter();
                    dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]), ddlMon.SelectedValue + ddlYear.SelectedValue, Convert.ToDateTime(salaryDate));
                    DataTable dt3 = dt1;
                    DataView dv = new DataView(dt3);
                    string filter = "";


                    if (ddlCategory.SelectedIndex != 0)
                    {
                        filter += " Desigcd = " + ddlCategory.SelectedValue;
                        dv.RowFilter = filter;
                    }

                    ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptDesignationWiseSumm.rdlc");

                }

                ReportParameter[] p = new ReportParameter[3];
                p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                p[1] = new ReportParameter("MonYrCd", ddlMon.SelectedItem.Text + " " + ddlYear.SelectedItem.Text, true);
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
                lblType.Text = "Category";
                rblType.SelectedValue = "1";
                BindData();
                ddlMon.Focus();
            }
            catch(Exception ex)
            {

            }

        }

        protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strQry = "";
            DataTable objDT;
            if (rblType.SelectedValue=="1")
            {

                strQry = "SELECT Categcd, Category FROM M_Category Where IsActive='Y' ORDER BY Category";
                objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                ddlCategory.DataSource = objDT;
                ddlCategory.DataTextField = "Category";
                ddlCategory.DataValueField = "Categcd";
                ddlCategory.DataBind();

                ddlCategory.Items.Insert(0, new ListItem("All", "0"));

                lblType.Text = "Category";
            }
            else if(rblType.SelectedValue=="2")
            {
                strQry = "SELECT LocDepCd, LocationDep FROM M_LocationDep Where IsActive='Y' ORDER BY LocationDep";
                objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                ddlCategory.DataSource = objDT;
                ddlCategory.DataTextField = "LocationDep";
                ddlCategory.DataValueField = "LocDepCd";
                ddlCategory.DataBind();

                ddlCategory.Items.Insert(0, new ListItem("All", "0"));

                lblType.Text = "Department";
            }
            else if(rblType.SelectedValue=="3")
            {
                strQry = "SELECT Desigcd, Designation FROM M_Designation Where IsActive='Y' ORDER BY Designation";
                objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                ddlCategory.DataSource = objDT;
                ddlCategory.DataTextField = "Designation";
                ddlCategory.DataValueField = "Desigcd";
                ddlCategory.DataBind();

                ddlCategory.Items.Insert(0, new ListItem("All", "0"));

                lblType.Text = "Designation";
            }
            ddlCategory.Focus();
        }
    }
}