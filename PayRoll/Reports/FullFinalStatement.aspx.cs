using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SqlClient;

namespace PayRoll.Reports
{
    public partial class FullFinalStatement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                txtEmpCode.Focus();
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtReport = new DataTable();
                if (txtEmpCode.Text == "")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Please Enter Employee Code'); ", true);
                    return;
                }

                string strQry = "SELECT * FROM M_Emp where OrgID=" + Convert.ToInt32(Session["OrgID"]) + " and Employeecd ='" + txtEmpCode.Text + "'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);

                string monYrCd = "";

                if(objDT.Rows.Count>0)
                {
                    if(objDT.Rows[0]["Leavedate"]!=DBNull.Value)
                    {
                        string strQrySalHold = "SELECT * FROM T_SalaryHold where OrgID=" + Convert.ToInt32(Session["OrgID"]) + " and Employeecd='" + txtEmpCode.Text + "' and Hold='Y'";
                        DataTable objDTSalHold = SqlHelper.ExecuteDataTable(strQrySalHold, AppGlobal.strConnString);
                        if(objDTSalHold.Rows.Count>0)
                        {
                           for(int i=0;i<objDTSalHold.Rows.Count;i++)
                            {
                                if (monYrCd == "")
                                    monYrCd = objDTSalHold.Rows[i]["MonYrCd"].ToString();
                                else
                                    monYrCd = monYrCd + "," + objDTSalHold.Rows[i]["MonYrCd"].ToString();
                            }
                            dtReport = fullFinalStat(txtEmpCode.Text, monYrCd, objDT.Rows[0]["Leavedate"].ToString());
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Salary Not Hold For This Employee'); ", true);
                            return;
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Wrong Employee Code'); ", true);
                    return;
                }
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                
                DataView dv = new DataView(dtReport);

                ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());

                ReportViewer1.LocalReport.DataSources.Add(datasource);

                //if (dt3.Rows.Count == 0)
                //{
                //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Employee Code does not exist'); ", true);
                //    ReportViewer1.LocalReport.DataSources.Clear();
                //    return;
                //}

                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptFullFinalStat.rdlc");

                ReportParameter[] p = new ReportParameter[2];
                p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                //--- To Display Logo -----------------------------------
                ReportViewer1.LocalReport.EnableExternalImages = true;
                string strqry = "select LogoPath from M_Organization where OrgId=" + Convert.ToInt32(Session["OrgID"]);
                DataTable objDTP = SqlHelper.ExecuteDataTable(strqry, AppGlobal.strConnString);
                string path = "";

                if (objDTP.Rows[0]["LogoPath"] != DBNull.Value)
                    path = new Uri(Server.MapPath(objDTP.Rows[0]["LogoPath"].ToString())).AbsoluteUri;
                else
                    path = new Uri(Server.MapPath("~/Upload/Logo.png")).AbsoluteUri;

                p[1] = new ReportParameter("LogoPath", path, true);
                //-----------------------------------------------------


                this.ReportViewer1.LocalReport.SetParameters(p);
                ReportViewer1.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                txtEmpCode.Text = "";                
                ReportViewer1.LocalReport.DataSources.Clear();
                txtEmpCode.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected DataTable fullFinalStat(string employeecd, string monYrCd, string leaveDate)
        {
            DataRow dr;
            DataTable dt = new DataTable();
            try
            {
                string PendingSalMonth = "", PresentDays="";
                double Basic = 0, HRA = 0, Conveyance = 0, Educational = 0, Medical = 0, Washing = 0, Uniform = 0, Canteen = 0, insentive=0, EarTotal = 0;
                double PF = 0, ProfTax = 0, LWF = 0, Advance = 0, Patsanstha = 0, CompAsset = 0, TDS = 0, ESI = 0, Entertainment=0, DedTotal = 0;
                string leaveMonthYr = Convert.ToInt16(Convert.ToDateTime(leaveDate).Year).ToString("0000") + Convert.ToInt16(Convert.ToDateTime(leaveDate).Month).ToString("00");

                dt.Columns.Add("OrgID", typeof(string));
                dt.Columns.Add("OrgName", typeof(string));
                dt.Columns.Add("EmpCode", typeof(string));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("DOJ", typeof(string));
                dt.Columns.Add("ServicePeriod", typeof(string));
                dt.Columns.Add("Department", typeof(string));
                dt.Columns.Add("LeaveDate", typeof(string));
                dt.Columns.Add("StdGrossSal", typeof(double));

                dt.Columns.Add("PendingSalMonth", typeof(string));
                dt.Columns.Add("PresentDays", typeof(string));
                dt.Columns.Add("Basic", typeof(double));
                dt.Columns.Add("HRA", typeof(double));
                dt.Columns.Add("Conveyance", typeof(double));
                dt.Columns.Add("Educational", typeof(double));
                dt.Columns.Add("Medical", typeof(double));
                dt.Columns.Add("Washing", typeof(double));
                dt.Columns.Add("Uniform", typeof(double));
                dt.Columns.Add("Canteen", typeof(double));
                dt.Columns.Add("Insentive", typeof(double));
                dt.Columns.Add("EarTotal", typeof(double));

                dt.Columns.Add("PF", typeof(double));
                dt.Columns.Add("ProfTax", typeof(double));
                dt.Columns.Add("LWF", typeof(double));
                dt.Columns.Add("Advance", typeof(double));
                dt.Columns.Add("Patsanstha", typeof(double));
                dt.Columns.Add("CompAsset", typeof(double));
                dt.Columns.Add("TDS", typeof(double));
                dt.Columns.Add("ESI", typeof(double));
                dt.Columns.Add("Entertainment", typeof(double));
                dt.Columns.Add("DedTotal", typeof(double));

                dt.Columns.Add("Gratuity15", typeof(double));
                dt.Columns.Add("GratuityYears", typeof(double));
                dt.Columns.Add("GratuityTotal", typeof(double));
                dt.Columns.Add("GratuityScheme", typeof(double));
                dt.Columns.Add("GratuitySchRemark", typeof(double));

                dt.Columns.Add("BonasPeriod", typeof(string));
                dt.Columns.Add("BonasPeriodRemark", typeof(string));
                dt.Columns.Add("BonasGrossSal", typeof(double));
                dt.Columns.Add("BonusPayble", typeof(double));

                dt.Columns.Add("LeaveBalance", typeof(double));
                dt.Columns.Add("LeaveAmtPayable", typeof(double));

                dt.Columns.Add("SalaryAdvance", typeof(double));
                dt.Columns.Add("SalAdvPatSanstha", typeof(double));

                dt.Columns.Add("TotalPayble", typeof(double));
                dt.Columns.Add("TotalReceivable", typeof(double));
                dt.Columns.Add("NetPay", typeof(double));

                dt.Columns.Add("Head1", typeof(string));
                dt.Columns.Add("Head2", typeof(string));
                dt.Columns.Add("Head3", typeof(string));
                dt.Columns.Add("Head4", typeof(string));
                dt.Columns.Add("Head5", typeof(string));

                //string strQrySal = "SELECT dbo.M_Emp.Employeename, dbo.M_Emp.Employeecd, dbo.M_Emp.OrigJoindate, dbo.M_Emp.PFJoindate, dbo.M_Emp.Leavedate, dbo.T_MonthlySalary.MonYrcd, ";
                //strQrySal += " dbo.T_MonthlySalary.PresentDay, isnull(dbo.T_MonthlySalary.BasicDA,0) as BasicDA, isnull(dbo.T_MonthlySalary.HRA,0) as HRA, isnull(dbo.T_MonthlySalary.Conveyance,0) as Conveyance, isnull(dbo.T_MonthlySalary.Education,0) as Education, ";
                //strQrySal += " isnull(dbo.T_MonthlySalary.Medical,0) as Medical, isnull(dbo.T_MonthlySalary.Washing,0) as Washing, isnull(dbo.T_MonthlySalary.Uniform,0) as Uniform, isnull(dbo.T_MonthlySalary.Canteen,0) as Canteen,isnull(dbo.T_MonthlySalary.Incentive,0) as Incentive, ";
                //strQrySal += " isnull(dbo.T_MonthlySalary.TardalPathsasnth,0) as TardalPathsasnth, isnull(dbo.T_MonthlySalary.TDS,0) as TDS, isnull(dbo.T_MonthlySalary.Advance,0) as Advance, isnull(dbo.T_MonthlySalary.Loan,0) as Loan, isnull(dbo.T_MonthlySalary.ProfTax,0) as ProfTax, ";
                //strQrySal += " dbo.T_MonthlySalary.Provfund+dbo.T_MonthlySalary.pfpension as Provfund, isnull(dbo.T_MonthlySalary.ESIEmpContribution,0) as ESIEmpContribution, isnull(dbo.T_MonthlySalary.LWF,0) as LWF,isnull(dbo.T_MonthlySalary.Ded1,0) as Entertainment, isnull(dbo.T_MonthlySalary.Deduction,0) as Deduction,";
                //strQrySal += " isnull(dbo.T_MonthlySalary.Gross,0) as Gross, ";
                //strQrySal += " (select years from udfTimeSpan(dbo.M_Emp.OrigJoindate, dbo.M_Emp.Leavedate) )as Years,";
                //strQrySal += " (select months from udfTimeSpan(dbo.M_Emp.OrigJoindate, dbo.M_Emp.Leavedate) ) as months,";
                //strQrySal += " (select days from udfTimeSpan(dbo.M_Emp.OrigJoindate, dbo.M_Emp.Leavedate) ) as days , isnull(dbo.T_Gratuity.Amount,0) as GratuityImpAmount ";
                //strQrySal += " FROM dbo.T_MonthlySalary LEFT OUTER JOIN dbo.T_Gratuity ON dbo.T_MonthlySalary.OrgId = dbo.T_Gratuity.OrgID AND dbo.T_MonthlySalary.Employeecd = dbo.T_Gratuity.Employeecd AND";
                //strQrySal += " dbo.T_MonthlySalary.MonYrcd = dbo.T_Gratuity.MonYrCd RIGHT OUTER JOIN dbo.M_Emp ON dbo.T_MonthlySalary.OrgId = dbo.M_Emp.OrgId AND dbo.T_MonthlySalary.Employeecd = dbo.M_Emp.Employeecd";
                //strQrySal += " where dbo.M_Emp.orgid = " + Convert.ToInt32(Session["OrgID"]) + " and dbo.M_Emp.Employeecd='" + txtEmpCode.Text + "' and dbo.T_MonthlySalary.MonYrCd in ("+  monYrCd + ") order by dbo.T_MonthlySalary.docdate desc";


                string strQrySal = " SELECT TOP (100) PERCENT dbo.M_Emp.Employeename, dbo.M_Emp.Employeecd, dbo.M_Emp.OrigJoindate, dbo.M_Emp.PFJoindate, dbo.M_Emp.Leavedate, dbo.T_MonthlySalary.MonYrcd, ";
                strQrySal += " dbo.T_MonthlySalary.PayableDay as PresentDay, ISNULL(dbo.T_MonthlySalary.BasicDA, 0) AS BasicDA, ISNULL(dbo.T_MonthlySalary.HRA, 0) AS HRA, ISNULL(dbo.T_MonthlySalary.Conveyance, 0) AS Conveyance, ";
                strQrySal += " ISNULL(dbo.T_MonthlySalary.Education, 0) AS Education, ISNULL(dbo.T_MonthlySalary.Medical, 0) AS Medical, ISNULL(dbo.T_MonthlySalary.Washing, 0) AS Washing, ISNULL(dbo.T_MonthlySalary.Uniform, 0) ";
                strQrySal += " AS Uniform, ISNULL(dbo.T_MonthlySalary.Canteen, 0) AS Canteen, (ISNULL(dbo.T_MonthlySalary.Incentive, 0) +ISNULL(dbo.T_MonthlySalary.Add1, 0)) AS Incentive, ISNULL(dbo.T_MonthlySalary.TardalPathsasnth, 0) AS TardalPathsasnth,";
                strQrySal += " ISNULL(dbo.T_MonthlySalary.TDS, 0) AS TDS, ISNULL(dbo.T_MonthlySalary.Advance, 0) AS Advance, ISNULL(dbo.T_MonthlySalary.Loan, 0) AS Loan, ISNULL(dbo.T_MonthlySalary.ProfTax, 0) AS ProfTax,";
                strQrySal += " dbo.T_MonthlySalary.Provfund + dbo.T_MonthlySalary.pfpension AS Provfund, ISNULL(dbo.T_MonthlySalary.ESIEmpContribution, 0) AS ESIEmpContribution, ISNULL(dbo.T_MonthlySalary.LWF, 0) AS LWF,";
                strQrySal += " ISNULL(dbo.T_MonthlySalary.Ded1, 0) AS Entertainment, ISNULL(dbo.T_MonthlySalary.Deduction, 0) AS Deduction, ISNULL(dbo.T_MonthlySalary.Gross, 0) AS Gross,";
                strQrySal += " (SELECT years FROM dbo.udfTimeSpan(dbo.M_Emp.OrigJoindate, dbo.M_Emp.Leavedate) AS udfTimeSpan_3) AS Years,";
                strQrySal += " (SELECT months FROM dbo.udfTimeSpan(dbo.M_Emp.OrigJoindate, dbo.M_Emp.Leavedate) AS udfTimeSpan_2) AS months,";
                strQrySal += " (SELECT days FROM dbo.udfTimeSpan(dbo.M_Emp.OrigJoindate, dbo.M_Emp.Leavedate) AS udfTimeSpan_1) AS days, ISNULL(dbo.T_Gratuity.Amount, 0) AS GratuityImpAmount,";
                strQrySal += " udfEmployeesalarymax_1.BasicDA AS stdBasicDA ";
                strQrySal += " FROM dbo.T_MonthlySalary LEFT OUTER JOIN ";
                strQrySal += " dbo.udfEmployeesalarymax("+  Convert.ToInt32(Session["OrgID"]) +"," + leaveMonthYr + ") AS udfEmployeesalarymax_1 ON dbo.T_MonthlySalary.OrgId = udfEmployeesalarymax_1.OrgId AND";
                strQrySal += " dbo.T_MonthlySalary.Employeecd = udfEmployeesalarymax_1.Employeecd LEFT OUTER JOIN ";
                strQrySal += " dbo.T_Gratuity ON dbo.T_MonthlySalary.OrgId = dbo.T_Gratuity.OrgID AND dbo.T_MonthlySalary.Employeecd = dbo.T_Gratuity.Employeecd AND ";
                strQrySal += " dbo.T_MonthlySalary.MonYrcd = dbo.T_Gratuity.MonYrCd RIGHT OUTER JOIN ";
                strQrySal += " dbo.M_Emp ON dbo.T_MonthlySalary.OrgId = dbo.M_Emp.OrgId AND dbo.T_MonthlySalary.Employeecd = dbo.M_Emp.Employeecd ";
                strQrySal += " WHERE(dbo.M_Emp.OrgId = " + Convert.ToInt32(Session["OrgID"]) + ") AND(dbo.M_Emp.Employeecd = '" + txtEmpCode.Text + "') AND(dbo.T_MonthlySalary.MonYrcd IN(" + monYrCd + ")) ";
                strQrySal += " ORDER BY dbo.T_MonthlySalary.Docdate DESC";


                DataTable objDTSal = SqlHelper.ExecuteDataTable(strQrySal, AppGlobal.strConnString);
                if (objDTSal.Rows.Count > 0)
                {
                    string joinMonYr = "", leaveMonYr = "";

                    dr = dt.NewRow();
                    
                    dr["OrgID"] = Session["OrgID"].ToString();
                    dr["OrgName"] = Session["OrgName"].ToString();
                    dr["EmpCode"] = objDTSal.Rows[0]["Employeecd"].ToString();
                    dr["Name"] = objDTSal.Rows[0]["Employeename"].ToString();

                    //if (objDTSal.Rows[0]["PFJoindate"] != DBNull.Value)
                    //{
                    //    dr["DOJ"] = Convert.ToDateTime(objDTSal.Rows[0]["PFJoindate"]).ToString("dd/MM/yyyy");
                    //}
                    //else
                    //{
                    //    dr["DOJ"] = Convert.ToDateTime(objDTSal.Rows[0]["OrigJoindate"]).ToString("dd/MM/yyyy");
                    //}

                    dr["DOJ"] = Convert.ToDateTime(objDTSal.Rows[0]["OrigJoindate"]).ToString("dd/MM/yyyy");

                    dr["LeaveDate"] = Convert.ToDateTime(objDTSal.Rows[0]["Leavedate"]).ToString("dd/MM/yyyy");

                    joinMonYr = "01"+ Convert.ToInt16(Convert.ToDateTime(objDTSal.Rows[0]["Leavedate"]).Year).ToString("0000");
                    leaveMonYr = Convert.ToInt16(Convert.ToDateTime(objDTSal.Rows[0]["Leavedate"]).Month).ToString("00") + Convert.ToInt16(Convert.ToDateTime(objDTSal.Rows[0]["Leavedate"]).Year).ToString("0000");

                    for (int i=0;i<objDTSal.Rows.Count;i++)
                    {
                        if(PendingSalMonth=="")
                            PendingSalMonth= objDTSal.Rows[i]["MonYrcd"].ToString().Substring(0, 2) + "/" + objDTSal.Rows[i]["MonYrcd"].ToString().Substring(2, 4);
                         else
                            PendingSalMonth = PendingSalMonth + "-" +  objDTSal.Rows[i]["MonYrcd"].ToString().Substring(0, 2) + "/" + objDTSal.Rows[i]["MonYrcd"].ToString().Substring(2, 4);

                        if(PresentDays=="")
                            PresentDays = objDTSal.Rows[i]["PresentDay"].ToString();
                        else
                            PresentDays = PresentDays + "," +objDTSal.Rows[i]["PresentDay"].ToString();

                        Basic = Basic + Convert.ToDouble(objDTSal.Rows[i]["BasicDA"]);
                        HRA = HRA + Convert.ToDouble(objDTSal.Rows[i]["HRA"]);
                        Conveyance = Conveyance + Convert.ToDouble(objDTSal.Rows[i]["Conveyance"]);
                        Educational = Educational + Convert.ToDouble(objDTSal.Rows[i]["Education"]);
                        Medical = Medical + Convert.ToDouble(objDTSal.Rows[i]["Medical"]);
                        Washing = Washing + Convert.ToDouble(objDTSal.Rows[i]["Washing"]);
                        Uniform = Uniform + Convert.ToDouble(objDTSal.Rows[i]["Uniform"]);
                        Canteen = Canteen + Convert.ToDouble(objDTSal.Rows[i]["Canteen"]);
                        insentive = insentive + Convert.ToDouble(objDTSal.Rows[i]["Incentive"]);
                        EarTotal = EarTotal + Convert.ToDouble(objDTSal.Rows[i]["Gross"]);

                        PF = PF + Convert.ToDouble(objDTSal.Rows[i]["Provfund"]);
                        ProfTax = ProfTax + Convert.ToDouble(objDTSal.Rows[i]["ProfTax"]);
                        LWF = LWF + Convert.ToDouble(objDTSal.Rows[i]["LWF"]);
                        Advance = Advance + Convert.ToDouble(objDTSal.Rows[i]["Advance"]);
                        Patsanstha = Patsanstha + Convert.ToDouble(objDTSal.Rows[i]["TardalPathsasnth"]);
                        //  dr["CompAsset"] = objDTSal.Rows[0]["Canteen"].ToString();
                        TDS = TDS + Convert.ToDouble(objDTSal.Rows[i]["TDS"]);
                        ESI = ESI + Convert.ToDouble(objDTSal.Rows[i]["ESIEmpContribution"]);
                        Entertainment = Entertainment + Convert.ToDouble(objDTSal.Rows[i]["Entertainment"]);
                        DedTotal = DedTotal + Convert.ToDouble(objDTSal.Rows[i]["Deduction"]);
                    }


                    dr["PendingSalMonth"] = PendingSalMonth;
                    dr["PresentDays"] =PresentDays;
                    dr["Basic"] = Convert.ToDouble(Basic).ToString();
                    dr["HRA"] = Convert.ToDouble(HRA).ToString();
                    dr["Conveyance"] = Convert.ToDouble(Conveyance).ToString();
                    dr["Educational"] = Convert.ToDouble(Educational).ToString();
                    dr["Medical"] = Convert.ToDouble(Medical).ToString();
                    dr["Washing"] = Convert.ToDouble(Washing).ToString();
                    dr["Uniform"] = Convert.ToDouble(Uniform).ToString();
                    dr["Canteen"] = Convert.ToDouble(Canteen).ToString();
                    dr["Insentive"] = Convert.ToDouble(insentive).ToString();
                    dr["EarTotal"] = Convert.ToDouble(EarTotal).ToString();

                    dr["PF"] = Convert.ToDouble(PF).ToString();
                    dr["ProfTax"] = Convert.ToDouble(ProfTax).ToString();
                    dr["LWF"] = Convert.ToDouble(LWF).ToString();
                    dr["Advance"] = Convert.ToDouble(Advance).ToString();
                    dr["Patsanstha"] = Convert.ToDouble(Patsanstha).ToString();
                    //  dr["CompAsset"] = objDTSal.Rows[0]["Canteen"].ToString();
                    dr["TDS"] = Convert.ToDouble(TDS).ToString();
                    dr["ESI"] = Convert.ToDouble(ESI).ToString();
                    dr["Entertainment"] = Convert.ToDouble(Entertainment).ToString();
                    dr["DedTotal"] = Convert.ToDouble(DedTotal).ToString();


                    dr["ServicePeriod"] = objDTSal.Rows[0]["Years"].ToString() + " Years " + objDTSal.Rows[0]["Months"].ToString() + " Months " + objDTSal.Rows[0]["days"].ToString() + " Days " ;

                    string qryEmpConfig = "SELECT dbo.M_LocationDep.LocationDep FROM dbo.M_LocationDep INNER JOIN dbo.udfEmpConfigurationmax1(" + Convert.ToInt32(Session["OrgID"]) +",'" + Convert.ToDateTime(objDTSal.Rows[0]["Leavedate"]).ToString("dd MMM yyyy") + "','dept') AS udfEmpConfigurationmax1_1 ON";
                    qryEmpConfig += " dbo.M_LocationDep.LocDepCd = udfEmpConfigurationmax1_1.conId LEFT OUTER JOIN dbo.M_Emp ON udfEmpConfigurationmax1_1.OrgId = dbo.M_Emp.OrgId AND udfEmpConfigurationmax1_1.Employeecd = dbo.M_Emp.Employeecd";
                    qryEmpConfig += " WHERE(dbo.M_Emp.OrgID=" + Convert.ToInt32(Session["OrgID"]) + " and  dbo.M_Emp.Employeecd = '" + txtEmpCode.Text + "')";
                    DataTable objDTEmpConfig = SqlHelper.ExecuteDataTable(qryEmpConfig, AppGlobal.strConnString);
                    if (objDTEmpConfig.Rows.Count > 0)
                    {
                        dr["Department"] = objDTEmpConfig.Rows[0]["LocationDep"].ToString();
                    }

                    string qrySalConfig = "SELECT isnull(udf_EmpSalMaster_1.Gross,0) as Gross FROM dbo.M_Emp LEFT OUTER JOIN dbo.udf_EmpSalMaster(" + Convert.ToInt32(Session["OrgID"]) + ",'" + Convert.ToDateTime(objDTSal.Rows[0]["Leavedate"]).ToString("dd MMM yyyy") + "') AS udf_EmpSalMaster_1 ON dbo.M_Emp.OrgId = udf_EmpSalMaster_1.OrgId AND  dbo.M_Emp.Employeecd = udf_EmpSalMaster_1.Employeecd";
                    qrySalConfig += " WHERE(dbo.M_Emp.OrgID=" + Convert.ToInt32(Session["OrgID"]) + " and  dbo.M_Emp.Employeecd = '" + txtEmpCode.Text + "')";
                    DataTable objDTSalConfig = SqlHelper.ExecuteDataTable(qrySalConfig, AppGlobal.strConnString);
                    if (objDTSalConfig.Rows.Count > 0)
                    {
                        dr["StdGrossSal"] = Convert.ToDouble(objDTSalConfig.Rows[0]["Gross"]).ToString();
                    }

                    if (Convert.ToInt16(objDTSal.Rows[0]["Years"]) >= 5)
                    {
                        int monthCount = 0;

                        if(Convert.ToInt32(objDTSal.Rows[0]["Months"].ToString()) > 5 )
                        {
                            monthCount = 1;
                        }

                        dr["Gratuity15"] = Math.Round(Convert.ToDouble(objDTSal.Rows[0]["stdBasicDA"]) / 26 * 15);
                        //dr["GratuityYears"] = objDTSal.Rows[0]["Years"].ToString();
                        dr["GratuityYears"] = (Convert.ToDouble(objDTSal.Rows[0]["Years"].ToString())) + monthCount;
                        //dr["GratuityTotal"] = Math.Round((Convert.ToDouble(objDTSal.Rows[0]["stdBasicDA"]) / 26 * 15) * Convert.ToInt16(objDTSal.Rows[0]["Years"]));
                        dr["GratuityTotal"] = Math.Round( Math.Round(Convert.ToDouble(objDTSal.Rows[0]["stdBasicDA"]) / 26 * 15) * (Convert.ToDouble(objDTSal.Rows[0]["Years"]) + monthCount ));
                        dr["GratuityScheme"] = Convert.ToDouble(objDTSal.Rows[0]["GratuityImpAmount"]).ToString();
                    }

                    string qryRptHead = "SELECT * FROM M_FullFinalHeader where OrgID=" + Convert.ToInt32(Session["OrgID"]);
                    DataTable objDTRptHead = SqlHelper.ExecuteDataTable(qryRptHead, AppGlobal.strConnString);
                    if (objDTRptHead.Rows.Count > 0)
                    {
                        dr["Head1"] = objDTRptHead.Rows[0]["Head1"].ToString();
                        dr["Head2"] = objDTRptHead.Rows[0]["Head2"].ToString();
                        dr["Head3"] = objDTRptHead.Rows[0]["Head3"].ToString();
                        dr["Head4"] = objDTRptHead.Rows[0]["Head4"].ToString();
                        dr["Head5"] = objDTRptHead.Rows[0]["Head5"].ToString();
                    }


                    int year = 0;
                    string salaryFrom= "", salaryTo = "";

                    string bonusLeaveMon = Convert.ToInt16(Convert.ToInt16(leaveMonthYr.Substring(0, 4)) - 1).ToString("0000") + leaveMonthYr.Substring(4, 2);



                    //string qryCheckBonus = "select * from dbo.T_Bonus where OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and employeecd='" + employeecd + "' and '" + bonusLeaveMon + "' between right(MonYrCd,4)+ left(MonYrCd,2) and right(ToMonYrCd,4)+ left(ToMonYrCd,2)";
                    //DataTable objDTCheckBonus = SqlHelper.ExecuteDataTable(qryCheckBonus, AppGlobal.strConnString);
                    //if(objDTCheckBonus.Rows.Count>0)
                    //{
                    //    //salaryFrom = leaveMonthYr.Substring(0, 4) + "04";
                    //    //salaryTo = leaveMonthYr;
                    //}
                    //else
                    //{
                    //    salaryFrom =  Convert.ToInt16(Convert.ToInt16(leaveMonthYr.Substring(0, 4)) - 1).ToString("0000") + "04" ;
                    //    salaryTo = leaveMonthYr;
                    //}

                    
                    //------- Modified above code on 02/01/2021 As per discussed with Gade Mam (Display Bonus Calculation on Current Financial Year)
                    int janfebmar = 0;
                    janfebmar = Convert.ToInt32(leaveMonthYr.Substring(4, 2));
                    if (janfebmar >= 1 && janfebmar <= 3)
                    {
                        salaryFrom = Convert.ToInt16(Convert.ToInt16(leaveMonthYr.Substring(0, 4)) - 1).ToString("0000") + "04";
                        salaryTo = leaveMonthYr;
                    }
                    else
                    {
                        salaryFrom = leaveMonthYr.Substring(0, 4) + "04";
                        salaryTo = leaveMonthYr;
                    }
                    


                    string bonusRemark = "";
                    double bonus = 0;

                    //if (Convert.ToInt32(Convert.ToDateTime(DateTime.Now).Month) > 10)
                    //    year = Convert.ToInt32(Convert.ToDateTime(DateTime.Now).Year) ;
                    //else
                    //    year = Convert.ToInt32(Convert.ToDateTime(DateTime.Now).Year) - 1;

                    //string qryBonus = " SELECT dbo.T_MonthlySalary.Gross, isnull(dbo.T_MonthlySalary.Incentive,0) as Incentive, isnull(desi.conId,0) as desiCD FROM dbo.T_MonthlySalary LEFT OUTER JOIN dbo.udfEmpConfigurationmax1(" + Convert.ToInt32(Session["OrgID"]) + ",'" + Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy") + "', 'desg') AS desi ";
                    //qryBonus += " ON dbo.T_MonthlySalary.OrgId = desi.OrgId AND dbo.T_MonthlySalary.Employeecd = desi.Employeecd ";
                    //qryBonus += " WHERE(dbo.T_MonthlySalary.OrgId =" + Convert.ToInt32(Session["OrgID"]) + ") AND(dbo.T_MonthlySalary.Employeecd = '"+ txtEmpCode.Text+ "') AND(dbo.T_MonthlySalary.Docdate > '" + Convert.ToDateTime("01-10-" + year).ToString("dd MMM yyyy") + "')";

                    string qryBonus = " SELECT dbo.T_MonthlySalary.Gross, isnull(dbo.T_MonthlySalary.Incentive,0) as Incentive, isnull(desi.conId,0) as desiCD FROM dbo.T_MonthlySalary LEFT OUTER JOIN dbo.udfEmpConfigurationmax1(" + Convert.ToInt32(Session["OrgID"]) + ",'" + Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy") + "', 'desg') AS desi ";
                    qryBonus += " ON dbo.T_MonthlySalary.OrgId = desi.OrgId AND dbo.T_MonthlySalary.Employeecd = desi.Employeecd ";
                    qryBonus += " WHERE(dbo.T_MonthlySalary.OrgId =" + Convert.ToInt32(Session["OrgID"]) + ") AND(dbo.T_MonthlySalary.Employeecd = '" + txtEmpCode.Text + "') AND ( right(dbo.T_MonthlySalary.MonYrCd,4) +  left(dbo.T_MonthlySalary.MonYrCd,2) between '" + salaryFrom + "' and '" + salaryTo + "')";

                    DataTable objDTBonus = SqlHelper.ExecuteDataTable(qryBonus, AppGlobal.strConnString);
                    if (objDTBonus.Rows.Count > 0)
                    {
                        for(int i=0;i<objDTBonus.Rows.Count;i++)
                        {
                            //if (Convert.ToInt16(objDTBonus.Rows[i]["desiCD"]) == 34)
                            if (Convert.ToInt16(objDTBonus.Rows[i]["desiCD"]) == 34 || Convert.ToInt16(objDTBonus.Rows[i]["desiCD"]) == 21)//As per email loading and unloading workers bonus calculated with incentive deduction
                            {
                                if (i == 0)
                                    bonusRemark = Convert.ToDouble(Convert.ToDouble(objDTBonus.Rows[i]["Gross"]) - Convert.ToDouble(objDTBonus.Rows[i]["Incentive"])).ToString();
                                else
                                    bonusRemark += " + " + Convert.ToDouble(Convert.ToDouble(objDTBonus.Rows[i]["Gross"]) - Convert.ToDouble(objDTBonus.Rows[i]["Incentive"])).ToString();

                                bonus += Convert.ToDouble(objDTBonus.Rows[i]["Gross"]) - Convert.ToDouble(objDTBonus.Rows[i]["Incentive"]);
                            }
                            else
                            {
                                if (i == 0)
                                    bonusRemark = Convert.ToDouble(objDTBonus.Rows[i]["Gross"]).ToString();
                                else
                                    bonusRemark += " + " + Convert.ToDouble(objDTBonus.Rows[i]["Gross"]).ToString();

                                bonus += Convert.ToDouble(objDTBonus.Rows[i]["Gross"]);
                            }
                        }

                        //if(Convert.ToInt32(Convert.ToDateTime(objDTSal.Rows[0]["Leavedate"]).ToString("yyyy")) >12)
                        //    dr["BonasPeriod"] = "Apr -" + (Convert.ToInt32(Convert.ToDateTime(objDTSal.Rows[0]["Leavedate"]).ToString("yyyy"))-1) + " To " + Convert.ToDateTime(objDTSal.Rows[0]["Leavedate"]).ToString("MMM") + "-" + Convert.ToDateTime(objDTSal.Rows[0]["Leavedate"]).ToString("yyyy");
                        //else
                        //    dr["BonasPeriod"] = "Apr -" + Convert.ToInt32(Convert.ToDateTime(objDTSal.Rows[0]["Leavedate"]).ToString("yyyy")) + " To " + Convert.ToDateTime(objDTSal.Rows[0]["Leavedate"]).ToString("MMM") + "-" + Convert.ToDateTime(objDTSal.Rows[0]["Leavedate"]).ToString("yyyy");

                        dr["BonasPeriod"] = "Apr -" + (Convert.ToInt32(salaryFrom.Substring(0, 4))) + " To " + Convert.ToDateTime(objDTSal.Rows[0]["Leavedate"]).ToString("MMM") + "-" + Convert.ToDateTime(objDTSal.Rows[0]["Leavedate"]).ToString("yyyy");

                        dr["BonasPeriodRemark"] = bonusRemark;
                        dr["BonasGrossSal"] = bonus.ToString();
                        dr["BonusPayble"] = Math.Round(bonus * 8.33 / 100).ToString();
                    }

                    // Remaining Salary Advance
                    double advanceSanAmt = 0;
                    double advanceMonthlyDed = 0;
                   
                    string qryAdvance = "SELECT * FROM T_Advance where OrgID=" + Convert.ToInt32(Session["OrgID"]) + " and Employeecd='" + txtEmpCode.Text + "'";
                    DataTable objDTAdvance = SqlHelper.ExecuteDataTable(qryAdvance, AppGlobal.strConnString);
                    if (objDTAdvance.Rows.Count > 0)
                    {
                        advanceSanAmt = Convert.ToDouble(objDTAdvance.Rows[0]["AdvAmount"]);
                        string qryAdvanceMonthly = "select sum(advance) as advance from T_MonthlySalary where OrgID=" + Convert.ToInt32(Session["OrgID"]) + " and Employeecd='" + txtEmpCode.Text + "' and docDate>='" + Convert.ToDateTime(objDTAdvance.Rows[0]["SanctionDate"]).ToString("dd MMM yyyy") + "'";
                        DataTable objDTAdvanceMonthly = SqlHelper.ExecuteDataTable(qryAdvanceMonthly, AppGlobal.strConnString);
                        if (objDTAdvanceMonthly.Rows.Count > 0)
                        {
                            advanceMonthlyDed = Convert.ToDouble(objDTAdvanceMonthly.Rows[0]["advance"]);
                            dr["SalaryAdvance"] = advanceSanAmt - advanceMonthlyDed;
                        }
                    }

                    //string qryAdvance = "SELECT OrgId, Employeecd, SUM(Advamount) AS Advamount, DeductionFrom FROM dbo.T_Advance WHERE (OrgId =" + Convert.ToInt32(Session["OrgID"]) + " and Employeecd='" + txtEmpCode.Text  + "') GROUP BY OrgId, Employeecd, DeductionFrom ";
                    //DataTable objDTAdvance= SqlHelper.ExecuteDataTable(qryAdvance, AppGlobal.strConnString);
                    //if (objDTAdvance.Rows.Count > 0)
                    //{
                    //    advanceSanAmt = Convert.ToDouble(objDTAdvance.Rows[0]["Advamount"]);
                    //    string qryAdvanceMonthly = "select sum(advance) as advance from T_MonthlySalary where OrgID=" + Convert.ToInt32(Session["OrgID"]) + " and Employeecd='" + txtEmpCode.Text + "'";
                    //    DataTable objDTAdvanceMonthly = SqlHelper.ExecuteDataTable(qryAdvanceMonthly, AppGlobal.strConnString);
                    //    if (objDTAdvanceMonthly.Rows.Count > 0)
                    //    {
                    //        advanceMonthlyDed = Convert.ToDouble(objDTAdvanceMonthly.Rows[0]["advance"]);
                    //        dr["SalaryAdvance"] = advanceSanAmt - advanceMonthlyDed;
                    //    }
                    //}

                    double leaveBal = 0, LeaveAmtPayable=0;
                    string qryLeaveBal = "select t.Employeecd, sum(t.OpnPL-t.AttPL) as ClosePL from (";
                    qryLeaveBal += " SELECT T_LeaveBalance.employeecd, T_LeaveBalance.PL AS OpnPL, T_LeaveBalance.COff AS OpnCOff, 0 as AttPL, 0 as AttCoff, 0 as ClosePL, 0 as CloseCofff  FROM T_LeaveBalance where(T_LeaveBalance.OrgId = "+ Convert.ToInt16(Session["OrgID"]) + ") AND(LEFT(T_LeaveBalance.MonYrcd, 2) = LEFT('" + joinMonYr  + "', 2)) AND(RIGHT(T_LeaveBalance.MonYrcd, 4) = RIGHT('"+ joinMonYr + "', 4)) ";
                    qryLeaveBal += " Union all ";
                    qryLeaveBal += " select T_Attendance.employeecd, 0 as OpnPL, 0 as OpnCOff, T_Attendance.PL AS AttPL, T_Attendance.COff AS AttCOff, 0 as ClosePL, 0 as CloseCofff  FROM T_Attendance where(T_Attendance.OrgId = " + Convert.ToInt16(Session["OrgID"]) + ") AND((LEFT(T_Attendance.MonYrcd, 2) >= LEFT('" + joinMonYr + "', 2)) AND(RIGHT(T_Attendance.MonYrcd, 4) >= RIGHT('" + joinMonYr + "', 4)) and(LEFT(T_Attendance.MonYrcd, 2) <= LEFT('" + leaveMonYr + "', 2)) AND(RIGHT(T_Attendance.MonYrcd, 4) <= RIGHT('" + leaveMonYr + "', 4)))) as t ";
                    qryLeaveBal += " where t.Employeecd ='" + txtEmpCode.Text + "' group by t.Employeecd ";
                    DataTable dtLeaveBal = SqlHelper.ExecuteDataTable(qryLeaveBal, AppGlobal.strConnString);
                    if(dtLeaveBal.Rows.Count>0)
                    {
                        leaveBal = Convert.ToDouble(dtLeaveBal.Rows[0]["ClosePL"]);
                        LeaveAmtPayable = (Convert.ToDouble(objDTSalConfig.Rows[0]["Gross"]) / 26) * leaveBal;
                    }

                    dr["LeaveBalance"] = leaveBal;
                    dr["LeaveAmtPayable"] = LeaveAmtPayable;

                    dt.Rows.Add(dr);
                }

                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }
        }
    }
}