using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SqlClient;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Net;


namespace PayRoll.Reports
{
    public partial class SalarySlip : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
            
            //if((Session["RoleId"].ToString()=="3" || Session["RoleId"].ToString() == "") && (Session["form16"].ToString()=="Y" || Session["PaySlip"].ToString() == "Y"))
            if (Session["PaySlip"].ToString() == "Y")
            {
                txtEmpCode.Text = Session["UserName"].ToString();
                txtEmpCode.ReadOnly = true;
                btnSendMail.Visible = false;

                divFilter.Visible = false;
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

            strQry = "SELECT Category, Categcd FROM M_Category Where IsActive='Y' ORDER BY Category";
            DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlCategory.DataSource = objDT1;
            ddlCategory.DataTextField = "Category";
            ddlCategory.DataValueField = "Categcd";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("Select", "0"));

            strQry = "SELECT Designation, Desigcd FROM M_Designation Where IsActive='Y' ORDER BY Designation";
            objDT1 = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlDesignation.DataSource = objDT1;
            ddlDesignation.DataTextField = "Designation";
            ddlDesignation.DataValueField = "Desigcd";
            ddlDesignation.DataBind();
            ddlDesignation.Items.Insert(0, new ListItem("Select", "0"));

            strQry = "SELECT LocationDep, LocDepCd FROM M_LocationDep Where IsActive='Y' ORDER BY LocationDep";
            objDT1 = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlDepartment.DataSource = objDT1;
            ddlDepartment.DataTextField = "LocationDep";
            ddlDepartment.DataValueField = "LocDepCd";
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new ListItem("Select", "0"));

            strQry = "SELECT distinct(Employeecd), Employeename  FROM M_Emp Where IsActive='Y' and HODInchAppl='Y' ORDER BY Employeename";
            objDT1 = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlHOD.DataSource = objDT1;
            ddlHOD.DataTextField = "Employeename";
            ddlHOD.DataValueField = "Employeecd";
            ddlHOD.DataBind();
            ddlHOD.Items.Insert(0, new ListItem("Select", "0"));

            strQry = "SELECT Division, Divcd FROM M_Division Where IsActive='Y' ORDER BY Division";
            objDT1 = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlDivision.DataSource = objDT1;
            ddlDivision.DataTextField = "Division";
            ddlDivision.DataValueField = "Divcd";
            ddlDivision.DataBind();
            ddlDivision.Items.Insert(0, new ListItem("Select", "0"));

        }

        protected void btnSave_Click(object sender, EventArgs e)
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

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                //ReportDS.udfSalarySlipDataTable objdt = new ReportDS.udfSalarySlipDataTable();
                //ReportDSTableAdapters.udfSalarySlipTableAdapter obj = new ReportDSTableAdapters.udfSalarySlipTableAdapter();
                ReportDS.udfSalarySlip_NewDataTable objdt = new ReportDS.udfSalarySlip_NewDataTable();
                ReportDSTableAdapters.udfSalarySlip_NewTableAdapter obj = new ReportDSTableAdapters.udfSalarySlip_NewTableAdapter();

                ReportDS.M_AddHeadingDataTable tbl = new ReportDS.M_AddHeadingDataTable();
                ReportDSTableAdapters.M_AddHeadingTableAdapter tbladpt = new ReportDSTableAdapters.M_AddHeadingTableAdapter();
                tbladpt.Fill(tbl, Convert.ToInt32(Session["OrgId"]));

                string empCode = "0";
                if (txtEmpCode.Text != "")
                {
                    empCode = txtEmpCode.Text.Trim();
                }

                //ReportDS.udfSalarySlipDataTable objdt1 = new ReportDS.udfSalarySlipDataTable();
                ReportDS.udfSalarySlip_NewDataTable objdt1 = new ReportDS.udfSalarySlip_NewDataTable();

                string[] mnthcd = Moncd.Split(',');

                foreach (string monthcd in mnthcd)
                {
                    //Last Day of Month/Year
                    int Days = DateTime.DaysInMonth(Convert.ToInt32(monthcd.Substring(2, 4)), Convert.ToInt32(monthcd.Substring(0, 2)));
                    DateTime dt = Convert.ToDateTime(monthcd.Substring(2, 4) + "-" + monthcd.Substring(0, 2) + "-" + Days);

                    obj.Fill(objdt, Convert.ToInt32(Session["OrgId"]), Convert.ToInt32(Session["YearID"]), dt, monthcd, empCode);
                    objdt1.Merge(objdt);
                }

                DataTable dt1 = objdt1;
                //Filters
                DataView dv = new DataView(dt1);
                string filter = "";

                if (ddlCategory.SelectedIndex != 0)
                {
                    filter += " CatgCd = " + ddlCategory.SelectedValue + " AND ";
                }
                if (ddlHOD.SelectedIndex != 0)
                {
                    filter += " HodCd = " + ddlHOD.SelectedValue + " AND ";
                }
                if (ddlDepartment.SelectedIndex != 0)
                {
                    filter += " LocDeptCd=" + ddlDepartment.SelectedValue + " AND ";
                }
                if (ddlDesignation.SelectedIndex != 0)
                {
                    filter += " EmpDesgId=" + ddlDesignation.SelectedValue + " AND ";
                }
                if (ddlDivision.SelectedIndex != 0)
                {
                    filter += " Divcd=" + ddlDivision.SelectedValue + " AND ";
                }

                if (filter.Length > 0)
                {
                    filter = filter.Remove(filter.Length - 4, 3);
                    dv.RowFilter = filter;
                }

                ReportDataSource datasource = new ReportDataSource("SalarySlip", dv.ToTable());
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/SalarySlip.rdlc");
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Salary_Slip.rdlc");

                ReportParameter p1 = new ReportParameter("para1", tbl.Rows.Count > 0 ? tbl.Rows[0]["Add1Heading"].ToString() : "");
                ReportParameter p2 = new ReportParameter("para2", tbl.Rows.Count > 0 ? tbl.Rows[0]["Add2Heading"].ToString() : "");
                ReportParameter p3 = new ReportParameter("para3", tbl.Rows.Count > 0 ? tbl.Rows[0]["Add3Heading"].ToString() : "");
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });

                ReportViewer1.LocalReport.Refresh();

                ViewState["data"] = dv.ToTable();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
            }
        }

        private string GetMnthCd1()
        {
            string strMnthCd = "";
            int sMnthYrCd = Convert.ToInt32(ddlYear.SelectedValue + (ddlMnth.SelectedIndex.ToString().Length==1 ? "0" + ddlMnth.SelectedIndex.ToString() : ddlMnth.SelectedIndex.ToString()));
            int eMnthYrCd = Convert.ToInt32(ddlToYear.SelectedValue + (ddlToMnth.SelectedIndex.ToString().Length == 1 ? "0" + ddlToMnth.SelectedIndex.ToString() : ddlToMnth.SelectedIndex.ToString()));

            while (sMnthYrCd<= eMnthYrCd)
            {
                strMnthCd += "'" + sMnthYrCd.ToString().Substring(4, 2) + sMnthYrCd.ToString().Substring(0, 4) + "',";
                sMnthYrCd++;
                if(sMnthYrCd.ToString().Substring(4, 2) == "13")
                {
                    sMnthYrCd = Convert.ToInt32((sMnthYrCd.ToString().Substring(0, 4) + 1) + "01");
                }
            }

            strMnthCd = strMnthCd.Remove(strMnthCd.Length - 1, 1);

            return strMnthCd;
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
            ddlCategory.SelectedIndex = 0;
            ddlDepartment.SelectedIndex = 0;
            ddlDesignation.SelectedIndex = 0;
            ddlDivision.SelectedIndex = 0;
            ddlHOD.SelectedIndex = 0;
            txtEmpCode.Text = "";
            ReportViewer1.LocalReport.DataSources.Clear();
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

        protected void btnSendMail_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["data"];

                ReportDS.M_AddHeadingDataTable tbl = new ReportDS.M_AddHeadingDataTable();
                ReportDSTableAdapters.M_AddHeadingTableAdapter tbladpt = new ReportDSTableAdapters.M_AddHeadingTableAdapter();

                tbladpt.Fill(tbl,Convert.ToInt16(Session["OrgID"]));

                if (dt.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('No data for found'); ", true);
                    return;
                }

                string fileName = "", path = "", Moncd = "", empcode = "";

                //Generate PDFs
                foreach (DataRow dr in dt.Rows)
                {
                    DataTable temp = new ReportDS.udfSalarySlip_NewDataTable();
                    //DataTable temp = new DSSalarySlip.SalarySlipNewDataTable();
                    temp.ImportRow(dr);

                    //Generate PaySlip for each User 
                    ReportViewer2.Reset();
                    ReportViewer2.LocalReport.DataSources.Clear();
                    ReportDataSource datasource = new ReportDataSource("SalarySlip", temp);
                    ReportViewer2.LocalReport.DataSources.Add(datasource);
                    ReportViewer2.LocalReport.ReportPath = Server.MapPath("~/Reports/Salary_Slip.rdlc");

                    ReportParameter p1 = new ReportParameter("para1", tbl.Rows.Count > 0 ? tbl.Rows[0]["Add1Heading"].ToString() : "");
                    ReportParameter p2 = new ReportParameter("para2", tbl.Rows.Count > 0 ? tbl.Rows[0]["Add2Heading"].ToString() : "");
                    ReportParameter p3 = new ReportParameter("para3", tbl.Rows.Count > 0 ? tbl.Rows[0]["Add3Heading"].ToString() : "");
                    ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });


                    Moncd = dr["MonYrcd"].ToString();
                    empcode = dr["Employeecd"].ToString();
                    path = Server.MapPath("~/SalarySlips/" + Moncd);

                    //If directory not exists then create first
                    DirectoryInfo di = new DirectoryInfo(path);
                    if (!di.Exists)
                    {
                        di.Create();
                    }

                    //Create file Name 
                    fileName = empcode + "-" + Moncd + ".pdf";

                    //If file already exists for monthyecd then delete first
                    FileInfo fi = new System.IO.FileInfo(Server.MapPath("~/SalarySlips/" + Moncd + "/" + fileName));
                    if (fi.Exists) fi.Delete();

                    Warning[] warnings;
                    string[] streams;
                    string mimeType, encoding, filenameExtension;

                    byte[] bytes = ReportViewer2.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streams, out warnings);

                    FileStream fs = System.IO.File.Create(Server.MapPath("~/SalarySlips/" + Moncd + "/" + fileName));
                    fs.Write(bytes, 0, bytes.Length);

                    fs.Close();

                }

                //Send Email
                SendEmails();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('"+ ex.ToString() +"!'); ", true);
            }
        }

        private void SendEmails()
        {
            DataTable dt = (DataTable)ViewState["data"];

            DataTable objDT = SqlHelper.ExecuteDataTable("SELECT * FROM  M_EmailInfo WHERE OrgId=" + Convert.ToInt32(Session["OrgId"]) +"", AppGlobal.strConnString);

            if (objDT.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('SMTP Client Information Not Found'); ", true);
                return;
            }
            int strPort = (int)objDT.Rows[0]["port"];
            SmtpClient emailClient = new SmtpClient("" + objDT.Rows[0]["smtpClient"].ToString(), strPort);
            string strUserName = objDT.Rows[0]["username"].ToString();
            string strPasswd = objDT.Rows[0]["password"].ToString();
            string strFrom = objDT.Rows[0]["fromAddr"].ToString();
            string strCC = objDT.Rows[0]["cc"].ToString();

            emailClient.UseDefaultCredentials = false;
            emailClient.Credentials = new NetworkCredential(strUserName, strPasswd);
            //emailClient.UseDefaultCredentials = true;
            emailClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            emailClient.EnableSsl = Convert.ToBoolean(objDT.Rows[0]["ssl"]);

            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    string strTo = "";
                    strTo = dr["EmailId"].ToString();

                    if (strTo != "")
                    {
                        string Moncd = dr["MonYrcd"].ToString();
                        string empcode = dr["Employeecd"].ToString();
                        string fileName = empcode + "-" + Moncd + ".pdf";

                        string strMoncd = dr["MonYrcd"].ToString().Substring(0, 2);
                        string strMonth = MonthName(strMoncd);

                        string strSubject = "Salary Slip for the month of " + strMonth + "  " + dr["MonYrcd"].ToString().Substring(2, 4);
                        string strBody = "Please find the attachment of Salary Slip for the Month of " + strMonth + "  " + dr["MonYrcd"].ToString().Substring(2, 4);

                        MailMessage insMail = new MailMessage(strFrom, strTo, strSubject, strBody);
                        //if(strCC!="")
                        //    insMail.CC.Add(strCC);

                        //string p = Server.MapPath("~/SalarySlips/" + Moncd + "/" + fileName);
                        insMail.Attachments.Add(new Attachment(Server.MapPath("~/SalarySlips/" + Moncd + "/" + fileName)));

                        emailClient.Send(insMail);
                        insMail.Attachments.Dispose();
                    }
                }

                catch (Exception ex)
                {
                    //Release object
                    emailClient = null;
                    GC.Collect();

                    string strQry = "update M_EmailInfo set error='" + ex.ToString() + "' where Id=1";
                    SqlHelper.ExecuteNonQuery(strQry, AppGlobal.strConnString);

                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Main Not Sent ... Error !'); ", true);

                }
            }

            //Release object

            emailClient = null;
            GC.Collect();
        }

        private string MonthName(string strMoncd)
        {
            string strName = "";

            switch (strMoncd)
            {
                case "01":
                    strName = "January";
                    break;
                case "02":
                    strName = "February";
                    break;
                case "03":
                    strName = "March";
                    break;
                case "04":
                    strName = "April";
                    break;
                case "05":
                    strName = "May";
                    break;
                case "06":
                    strName = "June";
                    break;
                case "07":
                    strName = "Jul";
                    break;
                case "08":
                    strName = "August";
                    break;
                case "09":
                    strName = "September";
                    break;
                case "10":
                    strName = "October";
                    break;
                case "11":
                    strName = "November";
                    break;
                case "12":
                    strName = "December";
                    break;

            }

            return strName;
        }


    }
}