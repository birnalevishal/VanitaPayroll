using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SqlClient;
using System.IO;
using System.Text;

namespace PayRoll.Reports
{
    public partial class PfECRRegister : System.Web.UI.Page
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
                if(ddlMon.SelectedIndex==0)
                {
                    //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Month'); ", true);
                    return;
                }
                if (ddlYear.SelectedIndex == 0)
                {
                    //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Year'); ", true);
                    return;
                }
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                dsRegister.udf_pfECRRegisterDataTable dt1 = new dsRegister.udf_pfECRRegisterDataTable();

                dsRegisterTableAdapters.udf_pfECRRegisterTableAdapter dt = new dsRegisterTableAdapters.udf_pfECRRegisterTableAdapter();
                dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]), ddlMon.SelectedValue+ddlYear.SelectedValue);
                DataTable dt3 = dt1;
                DataView dv = new DataView(dt3);
                string filter = "";
                
                if (ddlMon.SelectedIndex != 0 && ddlYear.SelectedIndex!=0)
                {
                    filter += " MonYrcd = " + ddlMon.SelectedValue +ddlYear.SelectedValue ;
                    dv.RowFilter = filter;
                }
                
                ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptPFECRegister.rdlc");

                ReportParameter[] p = new ReportParameter[6];
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

                string ac1 = "", ac2 = "", acmin ="";
                string strPfConfig = "select * from M_PFConfiguration WHERE RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) = (SELECT  max(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) as MonYrcd  FROM dbo.M_PFConfiguration WHERE RIGHT(MonYrcd, 4) +LEFT(MonYrcd, 2) <= '" + ddlYear.SelectedValue + ddlMon.SelectedValue + "' and orgid = " + Convert.ToInt32(Session["OrgID"]) + " ) AND(OrgId = " + Convert.ToInt32(Session["OrgID"]) + ")";
                DataTable objPFConfig = SqlHelper.ExecuteDataTable(strPfConfig, AppGlobal.strConnString);
                if (objPFConfig.Rows.Count > 0)
                {
                    ac1 = Convert.ToDouble(objPFConfig.Rows[0]["AdminCharge1"]).ToString("0.0");
                    ac2 = Convert.ToDouble(objPFConfig.Rows[0]["AdminCharge2"]).ToString("0.0");
                    acmin = Convert.ToDouble(objPFConfig.Rows[0]["AdminCharge1min"]).ToString("0.0");
                }
                p[3] = new ReportParameter("ac1", ac1, true);
                p[4] = new ReportParameter("ac2", ac2, true);
                p[5] = new ReportParameter("acmin", acmin, true);
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

                ddlMon.Focus();
            }
            catch(Exception ex)
            {

            }

        }

        protected void btnCSV_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlMon.SelectedIndex == 0)
                {
                    //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Month'); ", true);
                    return;
                }
                if (ddlYear.SelectedIndex == 0)
                {
                    //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Year'); ", true);
                    return;
                }

                string strcsv = "";
                dsRegister.udf_pfECRRegisterDataTable dt1 = new dsRegister.udf_pfECRRegisterDataTable();

                dsRegisterTableAdapters.udf_pfECRRegisterTableAdapter dt = new dsRegisterTableAdapters.udf_pfECRRegisterTableAdapter();
                dt.Fill(dt1, Convert.ToInt16(Session["OrgID"]), ddlMon.SelectedValue + ddlYear.SelectedValue);
                DataTable dt3 = dt1;
                DataView dv = new DataView(dt3);
                if (dt3.Rows.Count > 0)
                {
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        strcsv += dt3.Rows[i]["UANNo"].ToString() + "#~#";
                        strcsv += dt3.Rows[i]["employeeName"].ToString() + "#~#";
                        strcsv += Convert.ToInt64(dt3.Rows[i]["gross"]).ToString("0") + "#~#";
                        strcsv += Convert.ToInt64(dt3.Rows[i]["BasicDA"]).ToString("0") + "#~#";
                        strcsv += Convert.ToInt64(dt3.Rows[i]["EPSWages"]).ToString("0") + "#~#";
                        strcsv += Convert.ToInt64(dt3.Rows[i]["EdliWages"]).ToString("0") + "#~#";
                        strcsv += Convert.ToInt64(dt3.Rows[i]["provFund"]).ToString("0") + "#~#";
                        strcsv += Convert.ToDouble(dt3.Rows[i]["pension"]).ToString("0") + "#~#";
                        strcsv += Convert.ToDouble(dt3.Rows[i]["fund"]).ToString("0") + "#~#";
                        
                        //strcsv += "#~#";
                        strcsv += dt3.Rows[i]["NCPDays"].ToString() + "#~#";
                        strcsv += "\n";

                    }

                    strcsv = strcsv.Replace("\n", Environment.NewLine);

                    string fileName = Server.MapPath("~/CSV/") + Path.GetFileName(ddlMon.SelectedValue + ddlYear.SelectedValue + ".txt");
                   
                    // Check if file already exists. If yes, delete it.     
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                    // Create a new file     
                    using (FileStream fs = File.Create(fileName))
                    {
                        // Add some text to file    
                        Byte[] title = new UTF8Encoding(true).GetBytes(strcsv);
                        fs.Write(title, 0, title.Length);
                        byte[] author = new UTF8Encoding(true).GetBytes(strcsv);
                        fs.Write(author, 0, author.Length);
                    }

                    // Open the stream and read it back.    
                    using (StreamReader sr = File.OpenText(fileName))
                    {
                        string s = "";
                        while ((s = sr.ReadLine()) != null)
                        {
                            Console.WriteLine(s);
                        }
                    }

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + ddlMon.SelectedValue+ddlYear.SelectedValue + ".txt");
                    Response.Charset = "";
                    Response.ContentType = "application/text";
                    Response.Output.Write(strcsv);
                    Response.Flush();
                    Response.End();
                    //HttpContext.Current.ApplicationInstance.CompleteRequest();

                    //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('File Generated Successfully'); ", true);
                    return;
                }
              
            }
            catch (Exception ex)
            {

            }
        }
    }
}