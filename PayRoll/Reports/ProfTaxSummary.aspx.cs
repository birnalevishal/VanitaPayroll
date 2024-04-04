using Microsoft.Reporting.WebForms;
using SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Text;
using System.Drawing;

namespace PayRoll.Reports
{
    public partial class ProfTaxSummary : System.Web.UI.Page
    {
        private int m_currentPageIndex;
        private IList<Stream> m_streams;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            string strQry = "SELECT Year FROM M_Year Where IsActive='Y' ORDER BY Year DESC";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlYear.DataSource = objDT;
            ddlYear.DataTextField = "Year";
            ddlYear.DataValueField = "Year";
            ddlYear.DataBind();
            ddlYear.Items.Insert(0, new ListItem("Select", "0000"));

            ddlToYear.DataSource = objDT;
            ddlToYear.DataTextField = "Year";
            ddlToYear.DataValueField = "Year";
            ddlToYear.DataBind();
            ddlToYear.Items.Insert(0, new ListItem("Select", "0000"));

            strQry = "SELECT Division, Divcd FROM M_Division Where IsActive='Y' ORDER BY Division";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlDivision.DataSource = objDT;
            ddlDivision.DataTextField = "Division";
            ddlDivision.DataValueField = "Divcd";
            ddlDivision.DataBind();
            ddlDivision.Items.Insert(0, new ListItem("All", "0"));

            strQry = "SELECT     StateCd, State FROM  M_State  WHERE (IsActive = 'Y') Order By State";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            //ddlState.DataSource = objDT;
            //ddlState.DataTextField = "State";
            //ddlState.DataValueField = "StateCd";
            //ddlState.DataBind();
            //ddlState.Items.Insert(0, new ListItem("All", "0"));

            chkState.DataSource = objDT;
            chkState.DataTextField = "State";
            chkState.DataValueField = "StateCd";
            chkState.DataBind();
            chkState.Items.Insert(0, new ListItem("All", "0"));
            chkState.SelectedIndex = 0;
        }
        protected void ddlMnth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMnth.SelectedIndex > 0)
            {
                ddlToMnth.SelectedIndex = ddlMnth.SelectedIndex;
            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlYear.SelectedIndex > 0)
            {
                ddlToYear.SelectedIndex = ddlYear.SelectedIndex;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlMnth.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Month'); ", true);
                    return;
                }
                if (ddlYear.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Year'); ", true);
                    return;
                }

                //Get Mnth Code for Selected Month & Year
                string Moncd = ddlMnth.SelectedValue + ddlYear.SelectedValue;

                if (ddlMnth.SelectedValue == ddlToMnth.SelectedValue && ddlYear.SelectedValue == ddlToYear.SelectedValue)
                {
                    Moncd = ddlMnth.SelectedValue + ddlYear.SelectedValue;
                }
                else if (Convert.ToInt32(ddlToYear.SelectedValue) < Convert.ToInt32(ddlYear.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select To Year Greater than or equal to From Year'); ", true);
                    return;
                }
                else
                {
                    Moncd = GetMnthCd();
                }
                string statechecked = "";
                foreach (ListItem st in chkState.Items)
                {
                    if (st.Selected)
                    {
                        statechecked = "1";
                    }
                }
                if(statechecked=="")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select State'); ", true);
                    return;
                }

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                ReportDS.udf_ProfTaxSummaryDataTable objdt = new ReportDS.udf_ProfTaxSummaryDataTable();
                ReportDSTableAdapters.udf_ProfTaxSummaryTableAdapter obj = new ReportDSTableAdapters.udf_ProfTaxSummaryTableAdapter();

                ReportDS.udf_ProfTaxSummaryDataTable objdt1 = new ReportDS.udf_ProfTaxSummaryDataTable();

                string[] mnthcd = Moncd.Split(',');

                foreach (string monthcd in mnthcd)
                {
                    //Last Day of Month/Year
                    int Days = DateTime.DaysInMonth(Convert.ToInt32(monthcd.Substring(2, 4)), Convert.ToInt32(monthcd.Substring(0, 2)));
                    DateTime dt = Convert.ToDateTime(monthcd.Substring(2, 4) + "-" + monthcd.Substring(0, 2) + "-" + Days);

                    obj.Fill(objdt1, Convert.ToInt32(Session["OrgId"]), monthcd, dt);
                    objdt.Merge(objdt1);
                }
                    
                DataTable dt1 = objdt;

                if (dt1.Rows.Count > 0) { btnPrint.Enabled = true; } else { btnPrint.Enabled = false; }
                //Filters
                DataView dv = new DataView(dt1);
                string filter = "";

                if (ddlDivision.SelectedIndex != 0)
                {
                    filter += " Divcd=" + ddlDivision.SelectedValue + " AND ";
                }
                if (chkState.SelectedIndex != 0)
                {
                    foreach(ListItem st in chkState.Items)
                    {
                        if (st.Selected)
                        {
                            filter += " WStatecd=" + st.Value+ " or ";
                        }
                    }
                    //filter += " WStatecd=" + ddlState.SelectedValue + " AND ";
                    
                }
                
                if (filter.Length > 0)
                {
                    //filter = filter.Remove(filter.Length - 3, 2);
                    filter = filter.Remove(filter.Length - 4, 3);
                    dv.RowFilter = filter;
                }

                ViewState["objDT"] = dv.ToTable();

                ReportDataSource datasource = new ReportDataSource("ProfTaxSumm", dv.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/ProfTaxSummary.rdlc");

                ReportParameter p = new ReportParameter("OrgName", Session["OrgName"].ToString());
                ReportParameter p1 = new ReportParameter("FrMoncd", ddlMnth.SelectedItem.ToString()+ " " + ddlYear.SelectedValue.ToString());
                ReportParameter p2 = new ReportParameter("ToMoncd", ddlToMnth.SelectedItem.ToString() + " " + ddlToYear.SelectedValue.ToString());
                ReportParameter p3 = new ReportParameter("Division", ddlDivision.SelectedItem.ToString());
                ReportParameter p4 = new ReportParameter("State", chkState.SelectedItem.ToString());

                //--- To Display Logo -----------------------------------
                ReportViewer1.LocalReport.EnableExternalImages = true;
                string strqry = "select LogoPath from M_Organization where OrgId=" + Convert.ToInt32(Session["OrgID"]);
                DataTable objDTP = SqlHelper.ExecuteDataTable(strqry, AppGlobal.strConnString);
                string path = "";

                if (objDTP.Rows[0]["LogoPath"] != DBNull.Value)
                    path = new Uri(Server.MapPath(objDTP.Rows[0]["LogoPath"].ToString())).AbsoluteUri;
                else
                    path = new Uri(Server.MapPath("~/Upload/Logo.png")).AbsoluteUri;

                ReportParameter p5 = new ReportParameter("LogoPath", path, true);
                //-----------------------------------------------------

                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p, p1, p2, p3, p4,p5 });

                ReportViewer1.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
            }
        }

        private string GetMnthCd()
        {
            string strMnthCd = "";
            int sMnth = ddlMnth.SelectedIndex;
            int sYr = Convert.ToInt32(ddlYear.SelectedValue);
            int eMnth = ddlToMnth.SelectedIndex;
            int eYr = Convert.ToInt32(ddlToYear.SelectedValue);
            int nMnthCnt = 0;
            int nYrCnt = 0;
            int Y = 1;
            int M = 1;

            if (sMnth < eMnth)
            {
                nMnthCnt = (eMnth - sMnth) + 1;
            }
            else
            {
                if (sMnth == eMnth)
                {
                    nMnthCnt = 13;
                }
                else
                {
                    nMnthCnt = (12 - (sMnth - eMnth)) + 1;
                }
            }

            if (sYr < eYr)
            {
                nYrCnt = eYr - sYr;
                if (nYrCnt > 1)
                {
                    nMnthCnt = nMnthCnt + ((nYrCnt - 1) * 12);
                }
            }

            if (sYr == eYr)
            {
                nYrCnt = 1;
            }

            while (Y <= nYrCnt)
            {
                while (M <= nMnthCnt)
                {
                    if (sMnth < 10)
                    {
                        strMnthCd += "0" + sMnth + sYr + ",";
                    }
                    else
                    {
                        strMnthCd += "" + sMnth + sYr + ",";
                    }
                    if (sMnth == 12)
                    {
                        sMnth = 1;
                        sYr += 1;
                        M += 1;
                        break;
                    }
                    else
                    {
                        sMnth += 1;
                        M += 1;
                    }
                }
                if (M > nMnthCnt)
                {
                    Y += 1;
                }
            }

            strMnthCd = strMnthCd.Remove(strMnthCd.Length - 1, 1);

            return strMnthCd;
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ddlMnth.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;
            ddlToMnth.SelectedIndex = 0;
            ddlToYear.SelectedIndex = 0;
           
            ddlDivision.SelectedIndex = 0;
            //ddlState.SelectedIndex = 0;
            chkState.SelectedIndex = 0;
            ReportViewer1.LocalReport.DataSources.Clear();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            LocalReport report = new LocalReport();
            report.ReportPath = Server.MapPath("~/Reports/ProfTaxSummary.rdlc");
            report.DataSources.Add(new ReportDataSource("ProfTaxSumm", LoadSalesData()));
            Export(report);
            m_currentPageIndex = 0;
            Print();


        }

        private DataTable LoadSalesData()
        {
            DataTable dt = (DataTable)ViewState["objDT"];
            return dt;
        }

        // Routine to provide to the report renderer, in order to
        //    save an image for each page of the report.
        private Stream CreateStream(string name,string fileNameExtension, Encoding encoding,string mimeType, bool willSeek)
        {
            Stream stream = new FileStream(name + "." + fileNameExtension, FileMode.Create);
            m_streams.Add(stream);
            return stream;
        }
        

        // Export the given report as an EMF (Enhanced Metafile) file.
        private void Export(LocalReport report)
        {
            string deviceInfo =
              @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>11in</PageWidth>
                <PageHeight>8.5in</PageHeight>
                <MarginTop>0.5in</MarginTop>
                <MarginLeft>0.5in</MarginLeft>
                <MarginRight>0.5in</MarginRight>
                <MarginBottom>0.5in</MarginBottom>
            </DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream, out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }

        // Handler for PrintPageEvents
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new  Metafile(m_streams[m_currentPageIndex]);

            ev.Graphics.DrawImage(pageImage, 0, 0);

            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        private void Print()
        {
            const string printerName = "Microsoft Office Document Image Writer";

            if (m_streams == null || m_streams.Count == 0)
                return;

            PrintDocument printDoc = new PrintDocument();
            printDoc.PrinterSettings.PrinterName = printerName;
            if (!printDoc.PrinterSettings.IsValid)
            {
                string msg = String.Format("Can't find printer \"{0}\".",
                    printerName);
                Console.WriteLine(msg);
                return;
            }
            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
            printDoc.Print();
        }
               
    }
}