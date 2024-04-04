using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace PayRoll.Reports
{
    public partial class RelievingLetter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtEmpCode.Text == "")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Please Enter Employee Code'); ", true);
                    return;
                }
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                dsExpCert.ExpCertificateDataTable dt1 = new dsExpCert.ExpCertificateDataTable();
                dsExpCertTableAdapters.ExpCertificateTableAdapter dt = new dsExpCertTableAdapters.ExpCertificateTableAdapter();

                //Get Employee Leave Date
                string strqr = "Select Leavedate from M_Emp where (Leavedate <> null or Leavedate <> '') and Employeecd = " + txtEmpCode.Text;
                DataTable objdt = SqlHelper.ExecuteDataTable(strqr, AppGlobal.strConnString);

                if(objdt.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Employee Leave date not found'); ", true);
                    return;
                }
                if (objdt.Rows.Count > 0)
                {
                    DateTime Ldt = Convert.ToDateTime(objdt.Rows[0]["Leavedate"]);

                    //Get Employee Info from Leave Date
                    dt.Fill(dt1, Convert.ToInt32(Session["OrgID"].ToString()), Ldt, txtEmpCode.Text);
                    DataTable dt3 = dt1;
                    DataView dv1 = new DataView(dt3);
                    ReportDataSource datasource1 = new ReportDataSource("dsRelievingLetter", dv1.ToTable());
                    ReportViewer1.LocalReport.DataSources.Add(datasource1);


                    //Get Employee PF Join date
                    string strquery = "Select PFJoindate from M_Emp where (PFJoindate <> null or PFJoindate <> '') and Employeecd = '" + txtEmpCode.Text + "' and  orgid=" + Convert.ToInt16(Session["OrgID"]); 
                    DataTable objdt1 = SqlHelper.ExecuteDataTable(strquery, AppGlobal.strConnString);

                    string Bpfdt = "";
                    string Pfjdt = "";
                    string desig = "";

                    if (objdt1.Rows.Count > 0)
                    {
                        Pfjdt = Convert.ToDateTime(objdt1.Rows[0]["PFJoindate"]).ToString();
                        
                        if (Pfjdt != null)
                        {
                            Bpfdt = Convert.ToDateTime(Pfjdt).AddDays(-1).ToString();                          
                            // Gets Designation from Before PF Joining date 
                            string strq = "SELECT e.Employeecd, e.Employeename, d.Designation, ld.LocationDep FROM M_Emp AS e INNER JOIN ";
                            strq += " dbo.udfEmpConfigurationmax1(" + Convert.ToInt16(Session["OrgID"]) + ", '"+ Convert.ToDateTime(Bpfdt).ToString("dd MMM yyyy") + "', 'desg') AS u ON e.Employeecd = u.Employeecd AND e.OrgId = u.OrgId INNER JOIN ";
                            strq += " M_Designation AS d ON u.conId = d.Desigcd LEFT OUTER JOIN M_LocationDep AS ld ON e.LocDepCd = ld.LocDepCd  WHERE(e.Employeecd = "+ txtEmpCode.Text+")";
                            DataTable dtstr = SqlHelper.ExecuteDataTable(strq, AppGlobal.strConnString);
                            if(dtstr.Rows.Count > 0)
                            {
                                 desig = dtstr.Rows[0]["Designation"].ToString();
                            }
                        }
                    }
                    
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptRelievingLetter.rdlc");                   

                    ReportParameter[] p = new ReportParameter[7];
                    p[0] = new ReportParameter("OrgName", Session["OrgName"].ToString(), true);
                    p[1] = new ReportParameter("AsOnDate", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy"), true);

                    if(Bpfdt != "") 
                        p[2] = new ReportParameter("Pfdate", Convert.ToDateTime(Bpfdt).ToString("dd MMM yyyy"), true);
                    else
                        p[2] = new ReportParameter("Pfdate", "", true);

                    p[3] = new ReportParameter("desigbPF", desig, true);
                   
                    if (objdt1.Rows.Count > 0)
                    {
                        p[4] = new ReportParameter("PfOrOrigjdate", Convert.ToDateTime( Pfjdt).ToString("dd MMM yyyy"), true);
                    }
                    else
                    {
                        string strOJD = "Select OrigJoindate from M_Emp where Employeecd = '" + txtEmpCode.Text + "' and orgid=" + Convert.ToInt16(Session["OrgID"]);
                        DataTable objdt2 = SqlHelper.ExecuteDataTable(strOJD, AppGlobal.strConnString);
                        string Ojdt = "";
                        if (objdt2.Rows.Count>0)
                            Ojdt = Convert.ToDateTime(objdt2.Rows[0]["OrigJoindate"]).ToString("dd MMM yyyy");
                        
                        p[4] = new ReportParameter("PfOrOrigjdate", Convert.ToDateTime(Ojdt).ToString("dd MMM yyyy"), true);
                    }
                    string strQry = "SELECT isnull(ShortName,'') as ShortName FROM M_Organization Where OrgID=" + Session["OrgID"].ToString();
                    string shortnm = SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString).ToString();
                    p[5] = new ReportParameter("ShortName", shortnm, true);
                    p[6] = new ReportParameter("OrgID", Session["OrgID"].ToString(), true);


                    this.ReportViewer1.LocalReport.SetParameters(p);
                                        
                    ReportViewer1.ZoomMode = ZoomMode.Percent;

                    ReportViewer1.ZoomPercent = 100;

                    ReportViewer1.LocalReport.Refresh();
                }
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

        
    }
}