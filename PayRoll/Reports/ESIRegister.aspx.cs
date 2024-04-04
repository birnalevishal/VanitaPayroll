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
    public partial class ESIRegister : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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

            strQry = "SELECT    RegId, Regname FROM M_Regional";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlregional.DataSource = objDT;
            ddlregional.DataTextField = "Regname";
            ddlregional.DataValueField = "RegId";
            ddlregional.DataBind();

            ddlregional.Items.Insert(0, new ListItem("Select", "0"));            
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
                dsRegister.udf_ESIRegisterDataTable dt1 = new dsRegister.udf_ESIRegisterDataTable();

                dsRegisterTableAdapters.udf_ESIRegisterTableAdapter dt = new dsRegisterTableAdapters.udf_ESIRegisterTableAdapter();
                dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]), ddlMon.SelectedValue + ddlYear.SelectedValue);
                DataTable dt3 = dt1;
                DataView dv = new DataView(dt3);
                string filter = "";

                if (ddlMon.SelectedIndex != 0 && ddlYear.SelectedIndex != 0)
                {
                    filter += " MonYrcd = " + ddlMon.SelectedValue + ddlYear.SelectedValue + " and ";
                }
                if (ddlregional.SelectedIndex > 0)
                {
                    filter += "Regid=" + ddlregional.SelectedValue + " and ";
                }
                string s = "",txt="";
                foreach (ListItem item in chklbSubReginaol.Items)
                {                    
                   if(item.Selected == true)
                    {
                        s += item.Value + ",";
                        txt += item.Text.Trim() + ",";
                    }
                }
                if(s!="")
                {
                    filter+="SubRegid in(" + s.Remove(s.Length - 1, 1) + ") and ";
                }                

                if (filter != "")
                {
                    dv.RowFilter = filter.Remove(filter.Length - 4, 3);
                }


                ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptESIRegister.rdlc");

                ReportParameter[] p = new ReportParameter[5];
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
                if (ddlregional.SelectedIndex != 0)
                {
                    p[3] = new ReportParameter("Regional","Regional" + " :" + ddlregional.SelectedItem.Text +"",true);
                }
                else
                {
                    p[3] = new ReportParameter("Regional", "", true);
                }
                if (txt!="")
                {
                    p[4] = new ReportParameter("SubRegional", "Sub-Regional" + " :" + txt.Remove(txt.Length-1,1) + "", true);
                }
                else
                {
                    p[4] = new ReportParameter("SubRegional", "", true);
                }

                //-----------------------------------------------------
                //string strESIConfig = "select top(1) * FROM dbo.M_ESIConfigure AS M_ESIConfigure1 WHERE (convert(int, RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) <=" + ddlYear.SelectedValue + ddlMon.SelectedValue + ") and orgid=" + Convert.ToInt32(Session["OrgID"]) + " order by convert(int, RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) desc";
                //DataTable objESIConfig = SqlHelper.ExecuteDataTable(strESIConfig, AppGlobal.strConnString);
                //if(objESIConfig.Rows.Count>0)
                //{
                //    p[3] = new ReportParameter("ESIEmp", objESIConfig.Rows[0]["ESIEmpPer"].ToString(), true);
                //    p[4] = new ReportParameter("ESIComp", objESIConfig.Rows[0]["ESICompPer"].ToString(), true);
                //}
                this.ReportViewer1.LocalReport.SetParameters(p);

                ReportViewer1.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error'); ", true);
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

                ddlMon.Focus();
            }
            catch (Exception ex)
            {

            }

        }

        protected void ddlregional_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlregional.SelectedIndex > 0)
            {
                string str = "SELECT SubRegId, SubRegName FROM M_SubRegional WHERE(RegId = '" + ddlregional.SelectedValue + "')";
                DataTable dt = SqlHelper.ExecuteDataTable(str, AppGlobal.strConnString);
                chklbSubReginaol.DataSource = dt;
                chklbSubReginaol.DataTextField = "SubRegName";
                chklbSubReginaol.DataValueField = "SubRegId";
                chklbSubReginaol.DataBind();                
            }
            else
            {                
                chklbSubReginaol.Items.Clear();               
            }
            chkAll.Checked = false;
        }   

        protected void chklbSubReginaol_SelectedIndexChanged1(object sender, EventArgs e)
        {
           
        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {            
            if(chkAll.Checked==true)
            {
                foreach (ListItem item in chklbSubReginaol.Items)
                {
                    item.Selected = true;
                }
            }
            else
            {
                foreach (ListItem item in chklbSubReginaol.Items)
                {
                    item.Selected = false;
                }
            }           
        }
    }
}