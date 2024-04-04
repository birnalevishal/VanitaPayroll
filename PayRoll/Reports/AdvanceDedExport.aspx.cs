﻿using Microsoft.Reporting.WebForms;
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
    public partial class AdvanceDedExport : System.Web.UI.Page
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
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                string strQry = " SELECT * FROM udf_AdvanceExport(" + Convert.ToInt16(Session["OrgID"]) + ",'" + ddlMon.SelectedValue + ddlYear.SelectedValue + "')"; 
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);

                DataView dv = new DataView(objDT);
                              
                ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptAdvanceDedExport.rdlc");

                ReportParameter[] p = new ReportParameter[3];
                p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                p[1] = new ReportParameter("MonYrCd", ddlMon.SelectedItem.Text + " " + ddlYear.SelectedValue, true);
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
                ReportViewer1.LocalReport.DataSources.Clear();
                ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:txtVisible();", true);
            }
            catch(Exception ex)
            {

            }
        }
    }
}