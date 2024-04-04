using Microsoft.Reporting.WebForms;
using SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PayRoll.Reports
{
    public partial class M7 : System.Web.UI.Page
    {
        private DateTime dt;

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

            strQry = "SELECT LocationDep, LocDepCd FROM M_LocationDep Where IsActive='Y' ORDER BY LocationDep";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlDepartment.DataSource = objDT;
            ddlDepartment.DataTextField = "LocationDep";
            ddlDepartment.DataValueField = "LocDepCd";
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new ListItem("All", "0"));

            //ddlDepartment1.DataSource = objDT;
            //ddlDepartment1.DataTextField = "LocationDep";
            //ddlDepartment1.DataValueField = "LocDepCd";
            //ddlDepartment1.DataBind();
 

            strQry = "SELECT Division, Divcd FROM M_Division Where IsActive='Y' ORDER BY Division";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlDivision.DataSource = objDT;
            ddlDivision.DataTextField = "Division";
            ddlDivision.DataValueField = "Divcd";
            ddlDivision.DataBind();
            ddlDivision.Items.Insert(0, new ListItem("All", "0"));
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string depts = "";
                //foreach (ListItem item in ddlDepartment1.Items)
                //{
                //    if(item.Selected)
                //    {
                //        if(depts=="")
                //        {
                //            depts += item.Value;
                //        }
                //        else
                //        {
                //            depts += "," + item.Value;
                //        }
                //    }
                //}


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

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                //Get Mnth Code for Selected Month & Year
                string Moncd = GetMnthCd();
                string[] arr = Moncd.Split(',');
                string strQry = "";
                string sCateg = "";
                string sCategcd = "";
                string filter = "";

                strQry = "DELETE FROM M7ReportData WHERE OrgId=" + Convert.ToInt32(Session["OrgId"]) + "";// UserCd = '" + Session["UserName"].ToString() + "'
                bool bResult = SqlHelper.ExecuteNonQuery(strQry, AppGlobal.strConnString);

                if (ddlDivision.SelectedIndex == 0 && ddlDepartment.SelectedIndex == 0)
                {
                    ReportDSTableAdapters.udfM7GrossSalTableAdapter obj =new ReportDSTableAdapters.udfM7GrossSalTableAdapter();
                    ReportDS.udfM7GrossSalDataTable objdt =new ReportDS.udfM7GrossSalDataTable();

                    foreach (string monthcd in arr)
                    {
                        //Last Day of Month/Year
                        int Days = DateTime.DaysInMonth(Convert.ToInt32(monthcd.Substring(2, 4)), Convert.ToInt32(monthcd.Substring(0, 2)));
                        DateTime dt = Convert.ToDateTime(monthcd.Substring(2, 4) + "-" + monthcd.Substring(0, 2) + "-" + Days);

                        obj.Fill(objdt, Convert.ToInt32(Session["OrgId"]), monthcd, dt);

                        if (objdt.Rows.Count > 0)
                        {
                            strQry = "INSERT INTO M7ReportData(OrgId, MonYrcd, WorkerCnt, WorkerGross, ManagerCnt, ManagerGross, OtherCnt, OtherGross, UserCd) VALUES(@OrgId, @MonYrcd, @WorkerCnt, @WorkerGross, @ManagerCnt, @ManagerGross, @OtherCnt, @OtherGross, @UserCd)";
                            SqlParameter[] para = new SqlParameter[9];
                            para[0] = new SqlParameter("@OrgId", Convert.ToInt32(objdt.Rows[0]["OrgId"]));
                            para[1] = new SqlParameter("@MonYrcd", objdt.Rows[0]["MonYrcd"].ToString());
                            para[2] = new SqlParameter("@WorkerCnt", Convert.ToInt32(objdt.Rows[0]["Worker"]));
                            para[3] = new SqlParameter("@WorkerGross", Convert.ToDecimal(objdt.Rows[0]["WorkerGross"]));
                            para[4] = new SqlParameter("@ManagerCnt", Convert.ToInt32(objdt.Rows[0]["Manager"]));
                            para[5] = new SqlParameter("@ManagerGross", Convert.ToDecimal(objdt.Rows[0]["ManagerGross"]));
                            para[6] = new SqlParameter("@OtherCnt", Convert.ToInt32(objdt.Rows[0]["Other"]));
                            para[7] = new SqlParameter("@OtherGross", Convert.ToDecimal(objdt.Rows[0]["OtherGross"]));
                            para[8] = new SqlParameter("@UserCd", Session["UserName"]);
                            bResult = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                        }
                    }
                    ReportDS.udfM7DataTable dt1 = new ReportDS.udfM7DataTable();
                    ReportDSTableAdapters.udfM7TableAdapter obj1 = new ReportDSTableAdapters.udfM7TableAdapter();

                    obj1.Fill(dt1, Convert.ToInt32(Session["OrgId"]), Moncd);

                    DataTable dtt = objdt;
                    //Filters
                    DataView dv = new DataView(dt1);
                    if (filter.Length > 0)
                    {
                        filter = filter.Remove(filter.Length - 4, 3);
                        dv.RowFilter = filter;
                    }

                    
                    ReportDataSource datasource = new ReportDataSource("M7Report", dv.ToTable());
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/M7Report.rdlc");
                    

                }
                else if ((ddlDivision.SelectedIndex != 0 && ddlDepartment.SelectedIndex == 0) || (ddlDivision.SelectedIndex == 0 && ddlDepartment.SelectedIndex != 0))
                {
                    DateTime dt11=new DateTime();

                    ReportDSTableAdapters.udfM7GrossSalcategTableAdapter obj = new ReportDSTableAdapters.udfM7GrossSalcategTableAdapter();
                    ReportDS.udfM7GrossSalcategDataTable objdt = new ReportDS.udfM7GrossSalcategDataTable();

                    foreach (string monthcd in arr)
                    {
                        //Last Day of Month/Year
                        //int Days = DateTime.DaysInMonth(Convert.ToInt32(monthcd.Substring(2, 4)), Convert.ToInt32(monthcd.Substring(0, 2)));
                        //DateTime dt = Convert.ToDateTime(monthcd.Substring(2, 4) + "-" + monthcd.Substring(0, 2) + "-" + Days);
                        int Days = DateTime.DaysInMonth(Convert.ToInt32(ddlToYear.Text ), Convert.ToInt32(ddlToMnth.SelectedValue  ));
                        DateTime dt = Convert.ToDateTime(ddlToYear.Text + "-" + ddlToMnth.SelectedValue + "-" + Days);
                        if (ddlDivision.SelectedIndex != 0)
                        {
                            sCateg = "div";
                            sCategcd = ddlDivision.SelectedValue;
                        }
                        else 
                        {
                            sCateg = "dept";
                            sCategcd = ddlDepartment.SelectedValue;
                        }
                        obj.Fill(objdt, Convert.ToInt32(Session["OrgId"]), monthcd, dt, sCateg, sCategcd);

                        if (objdt.Rows.Count > 0)
                        {
                            strQry = "INSERT INTO M7ReportData(OrgId, MonYrcd, WorkerCnt, WorkerGross, ManagerCnt, ManagerGross, OtherCnt, OtherGross, UserCd) VALUES(@OrgId, @MonYrcd, @WorkerCnt, @WorkerGross, @ManagerCnt, @ManagerGross, @OtherCnt, @OtherGross, @UserCd)";
                            SqlParameter[] para = new SqlParameter[9];
                            para[0] = new SqlParameter("@OrgId", Convert.ToInt32(objdt.Rows[0]["OrgId"]));
                            para[1] = new SqlParameter("@MonYrcd", objdt.Rows[0]["MonYrcd"].ToString());
                            para[2] = new SqlParameter("@WorkerCnt", Convert.ToInt32(objdt.Rows[0]["Worker"]));
                            para[3] = new SqlParameter("@WorkerGross", Convert.ToDecimal(objdt.Rows[0]["WorkerGross"]));
                            para[4] = new SqlParameter("@ManagerCnt", Convert.ToInt32(objdt.Rows[0]["Manager"]));
                            para[5] = new SqlParameter("@ManagerGross", Convert.ToDecimal(objdt.Rows[0]["ManagerGross"]));
                            para[6] = new SqlParameter("@OtherCnt", Convert.ToInt32(objdt.Rows[0]["Other"]));
                            para[7] = new SqlParameter("@OtherGross", Convert.ToDecimal(objdt.Rows[0]["OtherGross"]));
                            para[8] = new SqlParameter("@UserCd", Session["UserName"]);
                            bResult = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                        }

                        dt11 = dt;
                    }

                    ReportDS.udfM7categDataTable dt1 = new ReportDS.udfM7categDataTable();
                    ReportDSTableAdapters.udfM7categTableAdapter obj1 = new ReportDSTableAdapters.udfM7categTableAdapter();

                    obj1.Fill(dt1, Convert.ToInt32(Session["OrgId"]), Moncd, dt11, sCateg, sCategcd);

                    DataTable dtt = objdt;
                    //Filters
                    DataView dv = new DataView(dt1);
                    if (filter.Length > 0)
                    {
                        filter = filter.Remove(filter.Length - 4, 3);
                        dv.RowFilter = filter;
                    }


                    ReportDataSource datasource = new ReportDataSource("M7Report", dv.ToTable());
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/M7Report.rdlc");

                }
                else
                {
                    DateTime dt11 = new DateTime();

                    ReportDSTableAdapters.udfM7GrossSaldivLocTableAdapter obj = new ReportDSTableAdapters.udfM7GrossSaldivLocTableAdapter();
                    ReportDS.udfM7GrossSaldivLocDataTable objdt = new ReportDS.udfM7GrossSaldivLocDataTable();
                    foreach (string monthcd in arr)
                    {
                        //Last Day of Month/Year
                        int Days = DateTime.DaysInMonth(Convert.ToInt32(ddlToYear.Text), Convert.ToInt32(ddlToMnth.SelectedValue));
                        DateTime dt = Convert.ToDateTime(ddlToYear.Text + "-" + ddlToMnth.SelectedValue + "-" + Days);

                        obj.Fill(objdt, Convert.ToInt32(Session["OrgId"]), monthcd, dt , "div",ddlDivision.SelectedValue, "dept", ddlDepartment.SelectedValue);

                        if (objdt.Rows.Count > 0)
                        {
                            strQry = "INSERT INTO M7ReportData(OrgId, MonYrcd, WorkerCnt, WorkerGross, ManagerCnt, ManagerGross, OtherCnt, OtherGross, UserCd) VALUES(@OrgId, @MonYrcd, @WorkerCnt, @WorkerGross, @ManagerCnt, @ManagerGross, @OtherCnt, @OtherGross, @UserCd)";
                            SqlParameter[] para = new SqlParameter[9];
                            para[0] = new SqlParameter("@OrgId", Convert.ToInt32(objdt.Rows[0]["OrgId"]));
                            para[1] = new SqlParameter("@MonYrcd", objdt.Rows[0]["MonYrcd"].ToString());
                            para[2] = new SqlParameter("@WorkerCnt", Convert.ToInt32(objdt.Rows[0]["Worker"]));
                            para[3] = new SqlParameter("@WorkerGross", Convert.ToDecimal(objdt.Rows[0]["WorkerGross"]));
                            para[4] = new SqlParameter("@ManagerCnt", Convert.ToInt32(objdt.Rows[0]["Manager"]));
                            para[5] = new SqlParameter("@ManagerGross", Convert.ToDecimal(objdt.Rows[0]["ManagerGross"]));
                            para[6] = new SqlParameter("@OtherCnt", Convert.ToInt32(objdt.Rows[0]["Other"]));
                            para[7] = new SqlParameter("@OtherGross", Convert.ToDecimal(objdt.Rows[0]["OtherGross"]));
                            para[8] = new SqlParameter("@UserCd", Session["UserName"]);
                            bResult = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);
                        }
                        dt11 = dt;
                    }
                    ReportDS.udfM7divLocDataTable dt1 = new ReportDS.udfM7divLocDataTable();
                    ReportDSTableAdapters.udfM7divLocTableAdapter obj1 = new ReportDSTableAdapters.udfM7divLocTableAdapter();

                    obj1.Fill(dt1, Convert.ToInt32(Session["OrgId"]), Moncd, dt11, "div", ddlDivision.SelectedValue, "dept", ddlDepartment.SelectedValue);

                    DataTable dtt = objdt;
                    //Filters
                    DataView dv = new DataView(dt1);
                    if (filter.Length > 0)
                    {
                        filter = filter.Remove(filter.Length - 4, 3);
                        dv.RowFilter = filter;
                    }


                    ReportDataSource datasource = new ReportDataSource("M7Report", dv.ToTable());
                    ReportViewer1.LocalReport.DataSources.Add(datasource);
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/M7Report.rdlc");

                }



               

             

              
               

                             
               




                //ReportParameter p = new ReportParameter("OrgName", Session["OrgName"].ToString());
                //ReportParameter p1 = new ReportParameter("FrMoncd", ddlMnth.SelectedItem.ToString() + " " + ddlYear.SelectedValue.ToString());
                //ReportParameter p2 = new ReportParameter("ToMoncd", ddlToMnth.SelectedItem.ToString() + " " + ddlToYear.SelectedValue.ToString());
               
                //ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p, p1, p2 });

                ReportViewer1.LocalReport.Refresh();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ddlMnth.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;
            ddlToMnth.SelectedIndex = 0;
            ddlToYear.SelectedIndex = 0;
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


    }
}