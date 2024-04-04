using Microsoft.Reporting.WebForms;
using SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace PayRoll.Reports
{
    public partial class F16Report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();

                if (Session["RoleId"].ToString() == "1")
                {
                    txtEmpCode.ReadOnly = false;
                }
            }
        }

        private void BindData()
        {
            //string qryEmpRole = "SELECT Employeename FROM dbo.M_Emp WHERE (Employeecd ='" + Session["UserName"].ToString() + "' ) AND(OrgId = " + Convert.ToInt16(Session["OrgID"]) + ") AND(IsActive = 'Y') ";
            //qryEmpRole += " union ";
            //qryEmpRole += " SELECT RoleName FROM dbo.M_Role WHERE(RoleId = " + Session["RoleId"].ToString() + " ) AND(form16 = 'Y') AND(IsActive = 'Y')";

            string qryEmpRole = "SELECT OrgID,Employeename, UserRolecd FROM dbo.M_Emp WHERE (Employeecd = '" + Session["UserName"].ToString() + "') AND(OrgId = " + Convert.ToInt16(Session["OrgID"]) +") AND(IsActive = 'Y') OR (Employeecd ='" + Session["UserName"].ToString() + "') AND(UserRolecd = 7)";
            DataTable dtEmpRole = SqlHelper.ExecuteDataTable(qryEmpRole, AppGlobal.strConnString);
            if(dtEmpRole.Rows.Count>0)
            {
                if(dtEmpRole.Rows[0]["UserRolecd"].ToString()=="7" && dtEmpRole.Rows[0]["OrgID"].ToString() == Session["OrgID"].ToString())
                {
                    txtEmpCode.Text = Session["UserName"].ToString();
                    txtEmpName.Text = Session["EmpName"].ToString();

                    txtEmpCode.ReadOnly = false;
                }
                else if(dtEmpRole.Rows[0]["UserRolecd"].ToString() == "7" && dtEmpRole.Rows[0]["OrgID"].ToString()!= Session["OrgID"].ToString())
                {
                    txtEmpCode.ReadOnly = false;
                }
                else if (dtEmpRole.Rows[0]["UserRolecd"].ToString() != "7" && dtEmpRole.Rows[0]["OrgID"].ToString() == Session["OrgID"].ToString())
                {
                    txtEmpCode.ReadOnly = true;
                    txtEmpCode.Text = Session["UserName"].ToString();
                    txtEmpName.Text = Session["EmpName"].ToString();
                }
                else
                {
                    txtEmpCode.ReadOnly = true;
                }
            }
            else
            {
                txtEmpCode.ReadOnly = true;
            }


            string strQry = "SELECT YearId, CONVERT(Varchar, YEAR(Frdate)) + '-' + CONVERT(varchar, YEAR(Todate)) AS ACYr  FROM  M_FinanceYear ORDER BY YearId DESC";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlYear.DataSource = objDT;
            ddlYear.DataTextField = "ACYr";
            ddlYear.DataValueField = "YearId";
            ddlYear.DataBind();

            ddlYear.SelectedValue = Session["YearID"].ToString();
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtEmpCode.Text.Trim()=="")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter Employee Code'); ", true);
                    return;
                }

                string strQry = "SELECT CommCodeAdhno FROM M_Emp WHERE Employeecd='" + txtEmpCode.Text.Trim() + "' AND OrgId="+ Session["OrgId"].ToString();
                string aadharNo = SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString).ToString();

                strQry = "SELECT EmployeeName FROM M_Emp WHERE Employeecd='" + txtEmpCode.Text.Trim() + "' AND OrgId=" + Session["OrgId"].ToString();
                txtEmpName.Text = SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString).ToString();

                string[] yrs = ddlYear.SelectedItem.ToString().Split('-');
                string frDt = yrs[0] + "04";
                string toDt = yrs[1] + "03";
                
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                  
                //Hosue Property 7a
                DSF16.HousePropertyAmtDataTable house = new DSF16.HousePropertyAmtDataTable();
                DSF16TableAdapters.HousePropertyAmtTableAdapter houseDed = new DSF16TableAdapters.HousePropertyAmtTableAdapter();
                houseDed.Fill(house, Convert.ToInt16(Session["OrgID"]), Convert.ToInt32(Session["YearID"]), txtEmpCode.Text.Trim());
                DataTable dtHouse = house;
                if (dtHouse.Rows.Count == 0) { dtHouse.Rows.Add(); }
                ReportDataSource HousePropertyAmt = new ReportDataSource("HousePropertyAmt", dtHouse);
                ReportViewer1.LocalReport.DataSources.Add(HousePropertyAmt);

                //HouseRent
                DSF16.HouseRentDataTable houseRentDt = new DSF16.HouseRentDataTable();
                DSF16TableAdapters.HouseRentTableAdapter houseRentA = new DSF16TableAdapters.HouseRentTableAdapter();
                houseRentA.Fill(houseRentDt, Convert.ToInt16(Session["OrgID"]), txtEmpCode.Text.Trim(), frDt, toDt);
                DataTable dtHouseRent = houseRentDt;
                if (dtHouseRent.Rows.Count == 0) { dtHouseRent.Rows.Add(); }
                ReportDataSource HouseRent = new ReportDataSource("HouseRent", dtHouseRent);
                ReportViewer1.LocalReport.DataSources.Add(HouseRent);

                //Ten_80D
                DSF16.Ten_80DDataTable ten_80dDT = new DSF16.Ten_80DDataTable();
                DSF16TableAdapters.Ten_80DTableAdapter ten_80d = new DSF16TableAdapters.Ten_80DTableAdapter();
                ten_80d.Fill(ten_80dDT, Convert.ToInt16(Session["OrgID"]), Convert.ToInt32(Session["YearID"]), txtEmpCode.Text.Trim());
                DataTable dtTen_80D = ten_80dDT;
                if (dtTen_80D.Rows.Count == 0) { dtTen_80D.Rows.Add(); }
                ReportDataSource Ten_80D = new ReportDataSource("Ten_80D", dtTen_80D);
                ReportViewer1.LocalReport.DataSources.Add(Ten_80D);

                //eighty_G
                DSF16.eighty_GDataTable eighty_G_DT = new DSF16.eighty_GDataTable();
                DSF16TableAdapters.eighty_GTableAdapter eighty_GD = new DSF16TableAdapters.eighty_GTableAdapter();
                eighty_GD.Fill(eighty_G_DT, Convert.ToInt16(Session["OrgID"]), Convert.ToInt32(Session["YearID"]), txtEmpCode.Text.Trim());
                DataTable DT_eighty_G_DT = eighty_G_DT;
                if (DT_eighty_G_DT.Rows.Count == 0) { DT_eighty_G_DT.Rows.Add(); }
                ReportDataSource eighty_G = new ReportDataSource("eighty_G", DT_eighty_G_DT);
                ReportViewer1.LocalReport.DataSources.Add(eighty_G);

                //Ten_D (10B)
                DSF16.Ten_BDataTable tenBDt = new DSF16.Ten_BDataTable();
                DSF16TableAdapters.Ten_BTableAdapter tenBTA = new DSF16TableAdapters.Ten_BTableAdapter();
                tenBTA.Fill(tenBDt, Convert.ToInt16(Session["OrgID"]), Convert.ToInt32(Session["YearID"]), txtEmpCode.Text.Trim());
                DataTable dtTenB = tenBDt;
                if (dtTenB.Rows.Count == 0) { dtTenB.Rows.Add(); }
                ReportDataSource Ten_B = new ReportDataSource("Ten_B", dtTenB);
                ReportViewer1.LocalReport.DataSources.Add(Ten_B);

                //Ten_D (10C)
                DSF16.Ten_CDataTable tenCDt = new DSF16.Ten_CDataTable();
                DSF16TableAdapters.Ten_CTableAdapter tenCTA = new DSF16TableAdapters.Ten_CTableAdapter();
                tenCTA.Fill(tenCDt, Convert.ToInt16(Session["OrgID"]), Convert.ToInt32(Session["YearID"]), txtEmpCode.Text.Trim());
                DataTable dtTenC = tenCDt;
                if (dtTenC.Rows.Count == 0) { dtTenC.Rows.Add(); }
                ReportDataSource Ten_C = new ReportDataSource("Ten_C", dtTenC);
                ReportViewer1.LocalReport.DataSources.Add(Ten_C);

                //Ten_D (10D)
                DSF16.Ten_DDataTable tenDDt = new DSF16.Ten_DDataTable();
                DSF16TableAdapters.Ten_DTableAdapter tenDTA = new DSF16TableAdapters.Ten_DTableAdapter();
                tenDTA.Fill(tenDDt, txtEmpCode.Text.Trim(), Convert.ToInt16(Session["OrgID"]), Convert.ToInt32(Session["YearID"]));
                DataTable dtTenD = tenDDt;
                if (dtTenD.Rows.Count == 0) { dtTenD.Rows.Add(); }
                ReportDataSource Ten_D = new ReportDataSource("Ten_D", dtTenD);
                ReportViewer1.LocalReport.DataSources.Add(Ten_D);

                //Ten_D (10E)
                DSF16.Ten_EDataTable tenEDt = new DSF16.Ten_EDataTable();
                DSF16TableAdapters.Ten_ETableAdapter tenETA = new DSF16TableAdapters.Ten_ETableAdapter();
                tenETA.Fill(tenEDt, Convert.ToInt16(Session["OrgID"]), Convert.ToInt32(Session["YearID"]), txtEmpCode.Text.Trim());
                DataTable dtTenE= tenEDt;
                if (dtTenE.Rows.Count == 0) { dtTenE.Rows.Add(); }
                ReportDataSource Ten_E = new ReportDataSource("Ten_E", dtTenE);
                ReportViewer1.LocalReport.DataSources.Add(Ten_E);

                //Ten_D (10F)
                DSF16.Ten_FDataTable tenFDt = new DSF16.Ten_FDataTable();
                DSF16TableAdapters.Ten_FTableAdapter tenFTA = new DSF16TableAdapters.Ten_FTableAdapter();
                tenFTA.Fill(tenFDt, Convert.ToInt16(Session["OrgID"]), Convert.ToInt32(Session["YearID"]), txtEmpCode.Text.Trim());
                DataTable dtTenF = tenFDt;
                if (dtTenF.Rows.Count == 0) { dtTenF.Rows.Add(); }
                ReportDataSource Ten_F = new ReportDataSource("Ten_F", dtTenF);
                ReportViewer1.LocalReport.DataSources.Add(Ten_F);

                //Ten_D (10H)
                DSF16.Ten_HDataTable tenHDt = new DSF16.Ten_HDataTable();
                DSF16TableAdapters.Ten_HTableAdapter tenHTA = new DSF16TableAdapters.Ten_HTableAdapter();
                tenHTA.Fill(tenHDt, Convert.ToInt16(Session["OrgID"]), Convert.ToInt32(Session["YearID"]), txtEmpCode.Text.Trim());
                DataTable dtTenH = tenHDt;
                if (dtTenH.Rows.Count == 0) { dtTenH.Rows.Add(); }
                ReportDataSource Ten_H = new ReportDataSource("Ten_H", dtTenH);
                ReportViewer1.LocalReport.DataSources.Add(Ten_H);

                //Ten_D (10J)
                DSF16.Ten_JDataTable tenJDt = new DSF16.Ten_JDataTable();
                DSF16TableAdapters.Ten_JTableAdapter tenJTA = new DSF16TableAdapters.Ten_JTableAdapter();
                tenJTA.Fill(tenJDt, Convert.ToInt16(Session["OrgID"]), Convert.ToInt32(Session["YearID"]), txtEmpCode.Text.Trim());
                DataTable dtTenJ = tenJDt;
                if (dtTenJ.Rows.Count == 0) { dtTenJ.Rows.Add(); }
                ReportDataSource Ten_J = new ReportDataSource("Ten_J", dtTenJ);
                ReportViewer1.LocalReport.DataSources.Add(Ten_J);

                //Ten_D (10M)
                DSF16.Ten_MDataTable tenMDt = new DSF16.Ten_MDataTable();
                DSF16TableAdapters.Ten_MTableAdapter tenMTA = new DSF16TableAdapters.Ten_MTableAdapter();
                tenMTA.Fill(tenMDt, Convert.ToInt16(Session["OrgID"]), Convert.ToInt32(Session["YearID"]), txtEmpCode.Text.Trim());
                DataTable dtTenM = tenMDt;
                if (dtTenM.Rows.Count == 0) { dtTenM.Rows.Add(); }
                ReportDataSource Ten_M = new ReportDataSource("Ten_M", dtTenM);
                ReportViewer1.LocalReport.DataSources.Add(Ten_M);

                //Ten_D (10N)
                DSF16.Ten_NDataTable tenNDt = new DSF16.Ten_NDataTable();
                DSF16TableAdapters.Ten_NTableAdapter tenNTA = new DSF16TableAdapters.Ten_NTableAdapter();
                tenNTA.Fill(tenNDt, Convert.ToInt16(Session["OrgID"]), Convert.ToInt32(Session["YearID"]), txtEmpCode.Text.Trim());
                DataTable dtTenN = tenNDt;
                if (dtTenN.Rows.Count == 0) { dtTenN.Rows.Add(); }
                ReportDataSource Ten_N = new ReportDataSource("Ten_N", dtTenN);
                ReportViewer1.LocalReport.DataSources.Add(Ten_N);

                //Deductions
                DSF16.DeductionsDataTable ded = new DSF16.DeductionsDataTable();
                DSF16TableAdapters.DeductionsTableAdapter dedadpt = new DSF16TableAdapters.DeductionsTableAdapter();

                dedadpt.Fill(ded, txtEmpCode.Text.Trim(), Convert.ToInt32(Session["OrgId"]), Convert.ToInt32(Session["YearID"]));
                DataTable dtded = ded;
                if (dtded.Rows.Count == 0) { dtded.Rows.Add(); }
                ReportDataSource datasource = new ReportDataSource("Deductions", dtded);
                ReportViewer1.LocalReport.DataSources.Add(datasource);

                //Deduction by Section
                DSF16.DeductionsBySectionsDataTable dedbySec = new DSF16.DeductionsBySectionsDataTable();
                DSF16TableAdapters.DeductionsBySectionsTableAdapter dedsecadpt = new DSF16TableAdapters.DeductionsBySectionsTableAdapter();

                dedsecadpt.Fill(dedbySec,  Convert.ToInt32(Session["OrgId"]), Convert.ToInt32(Session["YearID"]), txtEmpCode.Text.Trim());
                DataTable dtdedbysec = dedbySec;
                if (dtdedbysec.Rows.Count == 0) { dtdedbysec.Rows.Add(); }
                ReportDataSource datasource1 = new ReportDataSource("DedBySec", dtdedbysec);
                ReportViewer1.LocalReport.DataSources.Add(datasource1);

                //Gross Salary (Gross+Bonus)
                DSF16.GrossSalaryDataTable grossSal = new DSF16.GrossSalaryDataTable();
                DSF16TableAdapters.GrossSalaryTableAdapter grossSalAdpt = new DSF16TableAdapters.GrossSalaryTableAdapter();

                grossSalAdpt.Fill(grossSal,  aadharNo, frDt, toDt);
                DataTable dtgrossSal = grossSal;
                if (dtgrossSal.Rows.Count == 0) { dtgrossSal.Rows.Add(); }
                ReportDataSource datasource2 = new ReportDataSource("GrossSal", dtgrossSal);
                ReportViewer1.LocalReport.DataSources.Add(datasource2);

                //PAN No
                strQry = "SELECT isnull(PANNo,'') as PAN FROM M_Emp WHERE CommCodeAdhno='" + aadharNo + "'";
                string panNo= (string)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
               
                //get Emp Age
                strQry = "SELECT Birthdate FROM M_Emp WHERE CommCodeAdhno='"+ aadharNo+"'";
                DateTime bdate = (DateTime)SqlHelper.ExecuteScalar(strQry,AppGlobal.strConnString);
                int age = getAge(bdate);

                //Income Tax Slab Percentage
                DSF16.T_IncTaxDataTable IncTax = new DSF16.T_IncTaxDataTable();
                DSF16TableAdapters.T_IncTaxTableAdapter IncTaxAdpt = new DSF16TableAdapters.T_IncTaxTableAdapter();

                IncTaxAdpt.Fill(IncTax, Convert.ToByte(Session["YearID"]), age);
                DataTable dtIncTax = IncTax;
                if (dtIncTax.Rows.Count == 0) { dtIncTax.Rows.Add(); }
                ReportDataSource datasource3 = new ReportDataSource("IncTax", dtIncTax);
                ReportViewer1.LocalReport.DataSources.Add(datasource3);

                //Rebate Amt and Limit
                double rebate = 0, rebatelimit = 0;
                strQry = "SELECT  Rebate, RebateLimit FROM M_F16StdDedRebate WHERE Yrno="+ Convert.ToInt32(Session["YearID"]);
                DataTable dtrebet = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if(dtrebet.Rows.Count>0)
                {
                    rebate = Convert.ToDouble(dtrebet.Rows[0]["Rebate"]);
                    rebatelimit = Convert.ToDouble(dtrebet.Rows[0]["RebateLimit"]);
                }

                //Report Data

                string strQryYr = "SELECT YearId, CONVERT(Varchar, YEAR(Frdate)+1) + '-' + CONVERT(varchar, YEAR(Todate)+1) AS ACYr  FROM  M_FinanceYear ORDER BY YearId DESC";
                DataTable objDTYr = SqlHelper.ExecuteDataTable(strQryYr, AppGlobal.strConnString);
                
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Form16.rdlc");

                ReportParameter p = new ReportParameter("OrgName", Session["OrgName"].ToString());
                ReportParameter p1 = new ReportParameter("Yr", objDTYr.Rows[0]["ACYr"].ToString());
                ReportParameter p2 = new ReportParameter("EmpNm", txtEmpName.Text);
                ReportParameter p3 = new ReportParameter("rebate", rebate.ToString());
                ReportParameter p4 = new ReportParameter("rebatelimit", rebatelimit.ToString());
                ReportParameter p5 = new ReportParameter("PAN", panNo);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p, p1, p2, p3, p4, p5 });

                ReportViewer1.LocalReport.Refresh();


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
            }
        }

        private string GetMnthCd1()
        {
            string[] strYear = ddlYear.SelectedItem.ToString().Split('-');
             
            string strMnthCd = "";
            int sMnthYrCd = Convert.ToInt32(strYear[0] + "04");
            int eMnthYrCd = Convert.ToInt32(strYear[1] + "03");

            while (sMnthYrCd <= eMnthYrCd)
            {
                strMnthCd += "" + sMnthYrCd.ToString().Substring(4, 2) + sMnthYrCd.ToString().Substring(0, 4) + ",";
                sMnthYrCd++;
                if (sMnthYrCd.ToString().Substring(4, 2) == "13")
                {
                    sMnthYrCd = Convert.ToInt32((Convert.ToInt32(sMnthYrCd.ToString().Substring(0, 4)) + 1).ToString() + "01");
                }
            }

            strMnthCd = strMnthCd.Remove(strMnthCd.Length - 1, 1);

            return strMnthCd;
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string qryEmpRole = "SELECT OrgID,Employeename, UserRolecd FROM dbo.M_Emp WHERE (Employeecd = '" + Session["UserName"].ToString() + "') AND(OrgId = " + Convert.ToInt16(Session["OrgID"]) + ") AND(IsActive = 'Y') OR (Employeecd ='" + Session["UserName"].ToString() + "') AND(UserRolecd = 7)";
            DataTable dtEmpRole = SqlHelper.ExecuteDataTable(qryEmpRole, AppGlobal.strConnString);
            if (dtEmpRole.Rows.Count > 0)
            {
                if (dtEmpRole.Rows[0]["UserRolecd"].ToString() == "7" && dtEmpRole.Rows[0]["OrgID"].ToString() == Session["OrgID"].ToString())
                {
                    txtEmpCode.Text = Session["UserName"].ToString();
                    txtEmpName.Text = Session["EmpName"].ToString();

                    txtEmpCode.ReadOnly = false;
                }
                else if (dtEmpRole.Rows[0]["UserRolecd"].ToString() == "7" && dtEmpRole.Rows[0]["OrgID"].ToString() != Session["OrgID"].ToString())
                {
                    txtEmpCode.ReadOnly = false;
                }
                else if (dtEmpRole.Rows[0]["UserRolecd"].ToString() != "7" && dtEmpRole.Rows[0]["OrgID"].ToString() == Session["OrgID"].ToString())
                {
                    txtEmpCode.ReadOnly = true;
                    txtEmpCode.Text = Session["UserName"].ToString();
                    txtEmpName.Text = Session["EmpName"].ToString();
                }
                else
                {
                    txtEmpCode.ReadOnly = true;
                }
            }
            else
            {
                txtEmpCode.ReadOnly = true;
            }
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.Refresh();
        }

        private int getAge(DateTime Dob)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1;
            DateTime PastYearDate = Dob.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
            int Hours = Now.Subtract(PastYearDate).Hours;
            int Minutes = Now.Subtract(PastYearDate).Minutes;
            int Seconds = Now.Subtract(PastYearDate).Seconds;
            if (Years == 58 && (Months > 0 || Days > 0))
            {
                Years += 1;
            }

            //return String.Format("Age: {0} Year(s) {1} Month(s) {2} Day(s) {3} Hour(s) {4} Second(s)",
            //Years, Months, Days, Hours, Seconds);
            return Years;
        }

        protected void txtEmpCode_TextChanged(object sender, EventArgs e)
        {
            if (txtEmpCode.Text != "")
            {
                string strQry = "SELECT Employeename  FROM M_Emp Where Employeecd='" + txtEmpCode.Text + "' and OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and IsActive='Y'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    txtEmpName.Text = objDT.Rows[0]["Employeename"].ToString();
                    btnShow_Click(sender, e);
                    btnShow.Focus();
                }
                else
                {
                    txtEmpName.Text = "";
                    txtEmpCode.Text = "";
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Wrong Employee Code'); ", true);
                    return;
                }
            }
        }

    }
}