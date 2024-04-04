using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SqlClient;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace PayRoll.Reports
{
    public partial class EmpBonus : System.Web.UI.Page
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

            strQry = "SELECT BankCd, BankName FROM M_Bank Where IsActive='Y' ORDER BY BankName";
            DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlBank.DataSource = objDT1;
            ddlBank.DataTextField = "BankName";
            ddlBank.DataValueField = "BankCd";
            ddlBank.DataBind();
            ddlBank.Items.Insert(0, new ListItem("Select", "0"));


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

                ReportDS.udfEmpBonusExpDataTable tbl = new ReportDS.udfEmpBonusExpDataTable();
                ReportDS.udfEmpBonusExpDataTable tbl1 = new ReportDS.udfEmpBonusExpDataTable();
                ReportDSTableAdapters.udfEmpBonusExpTableAdapter objDA = new ReportDSTableAdapters.udfEmpBonusExpTableAdapter();

                ReportDS.udfEmpBonusExp_ContractDataTable tblCont = new ReportDS.udfEmpBonusExp_ContractDataTable();
                ReportDS.udfEmpBonusExp_ContractDataTable tblCont1 = new ReportDS.udfEmpBonusExp_ContractDataTable();
                ReportDSTableAdapters.udfEmpBonusExp_ContractTableAdapter objDACont = new ReportDSTableAdapters.udfEmpBonusExp_ContractTableAdapter();


                //-----------------------For Left Employees--------------------------------------------------------------
                ReportDS.udfEmpBonusExp_LeftEmpDataTable tbllft = new ReportDS.udfEmpBonusExp_LeftEmpDataTable();
                ReportDS.udfEmpBonusExp_LeftEmpDataTable tbllft1 = new ReportDS.udfEmpBonusExp_LeftEmpDataTable();
                ReportDSTableAdapters.udfEmpBonusExp_LeftEmpTableAdapter objlftDA = new ReportDSTableAdapters.udfEmpBonusExp_LeftEmpTableAdapter();

                ReportDS.udfEmpBonusExp_Contract_LeftEmpDataTable tbllftCont = new ReportDS.udfEmpBonusExp_Contract_LeftEmpDataTable();
                ReportDS.udfEmpBonusExp_Contract_LeftEmpDataTable tbllftCont1 = new ReportDS.udfEmpBonusExp_Contract_LeftEmpDataTable();
                ReportDSTableAdapters.udfEmpBonusExp_Contract_LeftEmpTableAdapter objlftDACont = new ReportDSTableAdapters.udfEmpBonusExp_Contract_LeftEmpTableAdapter();
                //-------------------------------------------------------------------------------------------------------
                

                string[] mnthcd = Moncd.Split(',');
                string yrMonCd = "";
                //Last Day of Month/Year
                int Days = DateTime.DaysInMonth(Convert.ToInt32(ddlToYear.SelectedValue), Convert.ToInt32(ddlToMnth.SelectedValue));
                DateTime dt = Convert.ToDateTime(Convert.ToInt32(ddlToYear.SelectedValue) + "-" + Convert.ToInt32(ddlToMnth.SelectedValue) + "-" + Days);

                DataView dv;

                if (rblType.SelectedValue == "1")
                {
                    foreach (string monthcd in mnthcd)
                    {
                        yrMonCd = monthcd.Substring(2, 4) + monthcd.Substring(0, 2);

                        if(lftEmp.Checked == true)
                        {
                            objlftDA.Fill(tbllft, Convert.ToInt32(Session["OrgId"]), monthcd, yrMonCd, dt);
                            tbllft1.Merge(tbllft);
                        }
                        else
                        {
                            objDA.Fill(tbl, Convert.ToInt32(Session["OrgId"]), monthcd, yrMonCd, dt);
                            tbl1.Merge(tbl);
                        }                        
                    }

                    if (lftEmp.Checked == true)
                    {
                        dv = new DataView(tbllft1);
                    }
                    else
                    {
                        dv = new DataView(tbl1);
                    }
                        
                    string filter = "";

                    if (ddlBank.SelectedIndex != 0)
                    {
                        filter += " BankCd = '" + ddlBank.SelectedValue + "' AND ";
                    }
                    if (txtEmpCode.Text.Trim() != "")
                    {
                        filter += " Employeecd = '" + txtEmpCode.Text.Trim() + "' AND ";
                    }

                    if (filter.Length > 0)
                    {
                        filter = filter.Remove(filter.Length - 4, 3);
                        dv.RowFilter = filter;
                    }

                    ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                    ReportViewer1.LocalReport.DataSources.Add(datasource);

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptEmpBonusExport.rdlc");
                }
                else
                {
                    foreach (string monthcd in mnthcd)
                    {
                        yrMonCd = monthcd.Substring(2, 4) + monthcd.Substring(0, 2);

                        if (lftEmp.Checked == true) //
                        {
                            objlftDACont.Fill(tbllftCont, Convert.ToInt32(Session["OrgId"]), monthcd, yrMonCd, dt);
                            tbllftCont1.Merge(tbllftCont);
                        }
                        else
                        {                            
                            objDACont.Fill(tblCont, Convert.ToInt32(Session["OrgId"]), monthcd, yrMonCd, dt);
                            tblCont1.Merge(tblCont);
                        }
                    }

                    if(lftEmp.Checked == true)
                    {
                        dv = new DataView(tbllftCont1);
                    }
                    else
                    {
                        dv = new DataView(tblCont1);
                    }
                    
                    string filter = "";

                    if (ddlBank.SelectedIndex != 0)
                    {
                        filter += " BankCd = '" + ddlBank.SelectedValue + "' AND ";
                    }
                    if (txtEmpCode.Text.Trim() != "")
                    {
                        filter += " Employeecd = '" + txtEmpCode.Text.Trim() + "' AND ";
                    }

                    if (filter.Length > 0)
                    {
                        filter = filter.Remove(filter.Length - 4, 3);
                        dv.RowFilter = filter;
                    }

                    ReportDataSource datasource = new ReportDataSource("DataSet1", dv.ToTable());
                    ReportViewer1.LocalReport.DataSources.Add(datasource);

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptEmpBonusContractExport.rdlc");
                }

                //objDA.Fill(tbl, Convert.ToInt32(Session["OrgId"]), Moncd);

                ReportParameter p1 = new ReportParameter("OrgName", Session["OrgName"].ToString());
                ReportParameter p2 = new ReportParameter("FrMoncd", ddlMnth.SelectedItem.ToString() + " " + ddlYear.SelectedValue.ToString());
                ReportParameter p3 = new ReportParameter("ToMoncd", ddlToMnth.SelectedItem.ToString() + " " + ddlToYear.SelectedValue.ToString());

                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });

                ReportViewer1.LocalReport.Refresh();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            rblType.SelectedValue = "1";
        }
    }
}