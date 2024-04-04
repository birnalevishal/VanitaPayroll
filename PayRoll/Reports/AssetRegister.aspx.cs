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
    public partial class AssetRegister : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             if(!IsPostBack)
            {
                btnCancel_Click(sender, e);
             
                if(rblType.SelectedValue=="1")
                    txtDate.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
            }
        }

        private void BindData()
        {
            string strQry = "SELECT Employeename,Employeecd FROM M_Emp where OrgID=" + Convert.ToInt16(Session["OrgID"]) +  " ORDER BY Employeename";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
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
                if(rblType.SelectedValue=="")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Report Type'); ", true);
                    return;
                }
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                if (rblType.SelectedValue == "1")
                {
                    //if (txtDate.Text == "")
                    //{
                    //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter From Date'); ", true);
                    //    return;
                    //}
                    if(txtEmpCode.Text=="")
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter Employee Code'); ", true);
                        return;
                    }
                    dsRegister.udf_EmpForAssetReportDataTable dt1 = new dsRegister.udf_EmpForAssetReportDataTable();
                    dsRegisterTableAdapters.udf_EmpForAssetReportTableAdapter dt = new dsRegisterTableAdapters.udf_EmpForAssetReportTableAdapter();

                    dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]), DateTime.Now, txtEmpCode.Text);
                    DataTable dt3 = dt1;
                    DataView dv = new DataView(dt3);

                    ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptAssetRegister.rdlc");

                    ReportParameter[] p = new ReportParameter[3];
                    p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                    p[1] = new ReportParameter("asOnDate", txtDate.Text, true);
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
                }
                else
                {
                    if(txtDate.Text=="")
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter From Date'); ", true);
                        return;
                    }
                    if (txtToDate.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter To Date'); ", true);
                        return;
                    }
                    dsRegister.udf_EmpForAssetDetReportDataTable dt1 = new dsRegister.udf_EmpForAssetDetReportDataTable();
                    dsRegisterTableAdapters.udf_EmpForAssetDetReportTableAdapter dt = new dsRegisterTableAdapters.udf_EmpForAssetDetReportTableAdapter();

                    dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]), DateTime.Now, Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txtToDate.Text));
                    DataTable dt3 = dt1;
                    DataView dv = new DataView(dt3);

                    ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptAssetRegisterDet.rdlc");

                    ReportParameter[] p = new ReportParameter[3];
                    p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                    p[1] = new ReportParameter("asOnDate", "From " + txtDate.Text + " To " + txtToDate.Text, true);
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
                }
                ReportViewer1.LocalReport.Refresh();
            }
            catch(Exception ex)
            {

            }
        }

        protected void ddlEmpName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEmpName.SelectedIndex != 0)
            {
                txtEmpCode.Text = ddlEmpName.SelectedValue.ToString();
                btnSave.Focus();
            }
        }

        protected void txtEmpCode_TextChanged(object sender, EventArgs e)
        {
            if (txtEmpCode.Text != "")
            {
                string strQry = "SELECT Employeename FROM M_EMP Where OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and Employeecd='" + txtEmpCode.Text + "'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    if (objDT.Rows[0]["Employeename"] != DBNull.Value)
                        ddlEmpName.SelectedItem.Text = objDT.Rows[0]["Employeename"].ToString();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Wrong Employee Code'); ", true);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                BindData();
                txtDate.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
                txtEmpCode.Text = "";
                lblEmpCode.Visible = true;
                lblIssueDate.Text = "Issue Date";
                lblEmpName.Text = "Employee Name";
                lblIssueDate.Visible = false;
                txtDate.Visible = false;
                txtEmpCode.Visible = true;
                ddlEmpName.SelectedIndex = 0;
                txtToDate.Visible = false;
                lblToDate.Visible = false;

                RFVtxtEmpCode.Enabled = true;
                rblType.SelectedValue = "1";
                ReportViewer1.LocalReport.DataSources.Clear();
                ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:txtVisible();", true);
            }
            catch(Exception ex)
            {

            }
        }

        protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDate.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
            txtToDate.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
            
            txtEmpCode.Text = "";

            if (rblType.SelectedValue=="1")
            {
                txtDate.Visible = false;
                txtEmpCode.Visible = true;
                ddlEmpName.Visible = true;
                lblEmpName.Visible = true;
                RFVtxtEmpCode.Enabled = true;


                lblIssueDate.Text = "Issue Date";
                lblIssueDate.Visible = false;
                lblEmpCode.Visible = true;
                txtToDate.Visible = false;
                lblToDate.Visible = false;  
            }
            else
            {
                txtDate.Visible = true;
                txtToDate.Visible = true;
                lblToDate.Visible = true;
                lblIssueDate.Text = "From Date";
                lblIssueDate.Visible = true;
                RFVtxtEmpCode.Enabled = false;

                ddlEmpName.Visible = false;
                lblEmpName.Visible = false;
                txtEmpCode.Visible = false;
                lblEmpCode.Visible = false;
            }
            ReportViewer1.LocalReport.DataSources.Clear();
        }   
    }
}