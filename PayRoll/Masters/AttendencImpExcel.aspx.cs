using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SqlClient;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Configuration;
using System.Windows.Input;

namespace PayRoll.Masters
{
    public partial class AttendencImpExcel : System.Web.UI.Page
    {
        SqlConnection sqlConn = null;
        SqlCommand sqlCmd = null;
        SqlTransaction sqlTrans = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            if (!Page.IsPostBack)
            {
                BindData();
                ddlMon.Focus();
                clearControls();
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

            ddlYear.Items.Insert(0, new ListItem("Select", "00"));

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    if (formValidation())
                    {
                        string strQry = "select * from T_SalaryLock where OrgId=" + Convert.ToInt16(Session["orgID"]) + " and RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)>='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "' and Lock='Y'";
                        DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                        if (objDT.Rows.Count > 0)
                        {
                            clearControls();
                            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Salary Already Processed, Cant Modify Now.'); ", true);
                            return;
                        }

                        if (btnSave.Text == "Save")
                        {
                            InsertRecord();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                clearControls();
            }
            catch (Exception ex)
            {

            }
        }

        private void InsertRecord()
        {
            try
            {
                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);

                //file upload path
                if (FUExcel.HasFiles)
                {
                    //File name seperation
                    string extension = Path.GetExtension(FUExcel.PostedFile.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(FUExcel.PostedFile.FileName);
                    string strConcat = DateTime.Now.ToString("ddMMyyyy_HHmmss");
                    
                    //Create New file name for saving file on sever
                    string excelPath = Server.MapPath("~/Imports/" + fileName + "_" + strConcat + extension);
                    FUExcel.SaveAs(excelPath);

                    string conString = string.Empty;
                    conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
                    conString = string.Format(conString, excelPath);

                    //Read excel file
                    using (OleDbConnection excel_con = new OleDbConnection(conString))
                    {
                        excel_con.Open();
                        string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                        DataTable dtExcelData = new DataTable();

                        //Data Table to store the information from excel file
                        dtExcelData.Columns.AddRange(new DataColumn[11] {
                                                                    new DataColumn("OrgID", typeof(int)),
                                                                    new DataColumn("MonYrcd", typeof(string)),
                                                                    new DataColumn("EMPCODE", typeof(string)),
                                                                    new DataColumn("H", typeof(float)),
                                                                    new DataColumn("PH", typeof(float)),
                                                                    new DataColumn("COff", typeof(float)),
                                                                    new DataColumn("A", typeof(float)),
                                                                    new DataColumn("L", typeof(float)),
                                                                    new DataColumn("P", typeof(float)),
                                                                    new DataColumn("SNACKCOUNT", typeof(float)),
                                                                    new DataColumn("OT", typeof(float)),
                                                                });

                        dtExcelData.Columns["OrgID"].DefaultValue = Session["OrgID"].ToString();
                        dtExcelData.Columns["MonYrcd"].DefaultValue = ddlMon.SelectedValue + ddlYear.SelectedValue;

                        //Read from Sheet
                        //using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT EMPCODE, Weeklyoff, Payholiday, COff, Absent, PL, PresentDay, SNACKCOUNT, OT  FROM[" + sheet1 + "]", excel_con))
                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT EMPCODE, H, PH, COff, A, L, P, SNACKCOUNT, OT  FROM[" + sheet1 + "]", excel_con))
                        {
                            oda.Fill(dtExcelData);

                            //Delete existing data for Selected Month/Year
                            string strQry = "SELECT * FROM T_Attendance Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and orgID=" + Convert.ToInt32(Session["OrgID"]);
                            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                            if (objDT.Rows.Count > 0)
                            {
                                string strQry1 = "delete FROM T_Attendance Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and orgID=" + Convert.ToInt32(Session["OrgID"]);
                                DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
                            }
                        }
                        excel_con.Close();

                        //Bulk copy data from excel to SQL 
                        string consString = ConfigurationManager.ConnectionStrings["VanitaPayrollConnectionString"].ConnectionString;
                        using (SqlConnection con = new SqlConnection(consString))
                        {
                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                            {
                                //Set the database table name
                                sqlBulkCopy.DestinationTableName = "dbo.T_Attendance";

                                //[OPTIONAL]: Map the Excel columns with that of the database table
                                sqlBulkCopy.ColumnMappings.Add("OrgID", "OrgId");
                                sqlBulkCopy.ColumnMappings.Add("MonYrcd", "MonYrcd");
                                sqlBulkCopy.ColumnMappings.Add("EMPCODE", "Employeecd");
                                sqlBulkCopy.ColumnMappings.Add("H", "Weeklyoff");
                                sqlBulkCopy.ColumnMappings.Add("PH", "Payholiday");

                                sqlBulkCopy.ColumnMappings.Add("COff", "COff");
                                sqlBulkCopy.ColumnMappings.Add("A", "Absent");
                                sqlBulkCopy.ColumnMappings.Add("L", "PL");
                                sqlBulkCopy.ColumnMappings.Add("P", "PresentDay");
                                sqlBulkCopy.ColumnMappings.Add("SNACKCOUNT", "SNACKCOUNT");
                                sqlBulkCopy.ColumnMappings.Add("OT", "OT");

                                con.Open();
                                sqlBulkCopy.WriteToServer(dtExcelData);
                                con.Close();

                                ////Validation class for validateAttendence
                                //string strOut = "";
                                //strOut = validation.validateAttendence(Convert.ToInt32(Session["OrgId"]), ddlMon.SelectedValue + ddlYear.SelectedValue);
                                //if (strOut.Substring(0, 1) != "1")
                                //{
                                //    sqlTrans.Rollback();
                                //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('" + strOut + "'); ", true);
                                //    return;
                                //}

                                //Clear Grid
                                gvAttendence.DataSource = null;
                                gvAttendence.DataBind();
                                gvSalary.DataSource = null;
                                gvSalary.DataBind();
                                gvConfiguration.DataSource = null;
                                gvConfiguration.DataBind();

                                int lastDay = 0;
                                lastDay = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMon.SelectedValue));
                                string dt = lastDay + "/" + ddlMon.SelectedValue + "/" + ddlYear.SelectedValue;
                                string firstDate = "01/" + ddlMon.SelectedValue + "/" + ddlYear.SelectedValue;

                                DataTable dtEmployee = new DataTable();
                                DataRow dr;
                                dtEmployee.Columns.Add("Employeecd", typeof(string));
                                dtEmployee.Columns.Add("Employeename", typeof(string));
                                dtEmployee.Columns.Add("Attendence", typeof(string));

                                DataTable dtSalary = new DataTable();
                                DataRow drSal;
                                dtSalary.Columns.Add("Employeecd", typeof(string));
                                dtSalary.Columns.Add("Employeename", typeof(string));
                                dtSalary.Columns.Add("Attendence", typeof(string));

                                DataTable dtConfiguration = new DataTable();
                                DataRow drConfiguration;
                                dtConfiguration.Columns.Add("Employeecd", typeof(string));
                                dtConfiguration.Columns.Add("Employeename", typeof(string));
                                dtConfiguration.Columns.Add("Attendence", typeof(string));

                                DataTable dtAllowConfiguration = new DataTable();
                                DataRow drAllowConfiguration;
                                dtAllowConfiguration.Columns.Add("Employeecd", typeof(string));
                                dtAllowConfiguration.Columns.Add("Employeename", typeof(string));
                                dtAllowConfiguration.Columns.Add("Attendence", typeof(string));

                                string strSalDet = "";

                                DataTable objSalDet = new DataTable();

                                int empMastCount = 0;
                                int empAttCount = 0;
                                int empSalCount = 0;
                                string returnStr = "";

                                //Active Employees Count
                                //string strQryEmp = "select count(distinct(employeecd)) as EmpMastCount from M_Emp where (leaveDate is null or leavedate>'" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "') and orgID=" + Convert.ToInt16(Session["OrgID"]) + " and isActive='Y'";
                                string strQryEmp = " SELECT COUNT(DISTINCT dbo.M_Emp.Employeecd) AS EmpMastCount FROM dbo.M_Emp LEFT OUTER JOIN dbo.udfEmpConfigurationmax1(" + Convert.ToInt16(Session["OrgID"]) + ",'" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "', 'desg') AS udfEmpConfigurationmax1_1 ON dbo.M_Emp.OrgId = udfEmpConfigurationmax1_1.OrgId AND ";
                                strQryEmp += " dbo.M_Emp.Employeecd = udfEmpConfigurationmax1_1.Employeecd ";
                                //strQryEmp += " WHERE(dbo.M_Emp.Leavedate IS NULL) AND(dbo.M_Emp.OrgId = " + Convert.ToInt16(Session["OrgID"]) + ") AND(dbo.M_Emp.IsActive = 'Y') AND(udfEmpConfigurationmax1_1.conId <> 21) OR (dbo.M_Emp.Leavedate IS NULL) AND(dbo.M_Emp.OrgId = " + Convert.ToInt16(Session["OrgID"]) + ") AND(udfEmpConfigurationmax1_1.conId <> 21) AND(dbo.M_Emp.Leavedate > '" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "')";
                                strQryEmp += " WHERE (dbo.M_Emp.OrgId = " + Convert.ToInt16(Session["OrgID"]) + ") AND (dbo.M_Emp.IsActive = 'Y') AND ((dbo.M_Emp.Leavedate IS NULL) OR (dbo.M_Emp.Leavedate >= '" + Convert.ToDateTime(firstDate).ToString("dd MMM yyyy") + "')) AND DatofJoin <='" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "' AND(udfEmpConfigurationmax1_1.conId <> 21) ";

                                DataTable objDTEmp = SqlHelper.ExecuteDataTable(strQryEmp, AppGlobal.strConnString);
                                if (objDTEmp.Rows.Count > 0)
                                {
                                    empMastCount = Convert.ToInt32(objDTEmp.Rows[0]["EmpMastCount"]);
                                }

                                //Attendence Import Count
                                string strQryAtt = "select count(distinct(employeecd)) as empAttCount from T_Attendance where orgID =" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                DataTable objDTAtt = SqlHelper.ExecuteDataTable(strQryAtt, AppGlobal.strConnString);
                                if (objDTAtt.Rows.Count > 0)
                                {
                                    empAttCount = Convert.ToInt32(objDTAtt.Rows[0]["empAttCount"]);
                                }

                                //Attendence Not Found
                                string strQryValidAtt = "";


                                strQryValidAtt = " SELECT emp.Employeecd, emp.Employeename, emp.Leavedate, udfEmpConfigurationmax1_1.conId FROM dbo.M_Emp AS emp LEFT OUTER JOIN dbo.udfEmpConfigurationmax1(" + Convert.ToInt16(Session["OrgID"]) + ",'" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "', 'desg') AS udfEmpConfigurationmax1_1 ON emp.OrgId = udfEmpConfigurationmax1_1.OrgId AND ";
                                strQryValidAtt += " emp.Employeecd = udfEmpConfigurationmax1_1.Employeecd LEFT OUTER JOIN udfAttendance(" + Convert.ToInt16(Session["OrgID"]) + " ,'" + ddlMon.SelectedValue + ddlYear.SelectedValue + "') AS att ON emp.Employeecd = att.Employeecd AND emp.OrgId = att.OrgId WHERE(emp.OrgId = " + Convert.ToInt16(Session["OrgID"]) + ") AND(emp.IsActive = 'Y') AND(att.Employeecd IS NULL) AND(emp.Leavedate IS NULL OR ";
                                // strQryValidAtt += " emp.Employeecd = udfEmpConfigurationmax1_1.Employeecd LEFT OUTER JOIN dbo.T_Attendance AS att ON emp.Employeecd = att.Employeecd AND emp.OrgId = att.OrgId WHERE(emp.OrgId = " + Convert.ToInt16(Session["OrgID"]) + ") AND(emp.IsActive = 'Y') AND(att.MonYrcd = '" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' OR att.MonYrcd IS NULL) AND(att.Employeecd IS NULL) AND(emp.Leavedate IS NULL OR ";
                                strQryValidAtt += " emp.Leavedate >= '" + Convert.ToDateTime(firstDate).ToString("dd MMM yyyy") + "') AND DatofJoin <='" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "' AND(udfEmpConfigurationmax1_1.conId <> 21)";
                                DataTable objDTValidAtt = SqlHelper.ExecuteDataTable(strQryValidAtt, AppGlobal.strConnString);
                                if (objDTValidAtt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < objDTValidAtt.Rows.Count; i++)
                                    {
                                        dr = dtEmployee.NewRow();
                                        if (objDTValidAtt.Rows[i]["leavedate"] == DBNull.Value)
                                        {
                                            dr["Employeecd"] = objDTValidAtt.Rows[i]["Employeecd"].ToString();
                                            dr["Employeename"] = objDTValidAtt.Rows[i]["Employeename"].ToString();
                                            dr["Attendence"] = "Attendence Not Found";
                                            dtEmployee.Rows.Add(dr);
                                        }
                                    }
                                    gvAttendence.DataSource = dtEmployee;
                                    gvAttendence.DataBind();
                                    pnlGVList.Visible = true;
                                }


                                //Attendence Count > Than Employee Master
                                strQryAtt = " select att.Employeecd from T_Attendance att where att.OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and att.MonYrcd = '" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'and Employeecd not in(select Employeecd from udfempconfigure(" + Convert.ToInt16(Session["OrgID"]) + ",'" + Convert.ToDateTime(firstDate).ToString("dd MMM yyyy") + "','" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "' ) ) ";
                                //strQryAtt = " select att.Employeecd from T_Attendance att where att.OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and att.MonYrcd = '" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'and Employeecd not in(select Employeecd from M_Emp emp where emp.OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and emp.IsActive = 'Y' and (leaveDate is null or leavedate>='" + Convert.ToDateTime(firstDate).ToString("dd MMM yyyy") + "') AND DatofJoin <='" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "'  )";
                                objDTAtt = SqlHelper.ExecuteDataTable(strQryAtt, AppGlobal.strConnString);
                                if (objDTAtt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < objDTAtt.Rows.Count; i++)
                                    {
                                        dr = dtEmployee.NewRow();
                                        dr["Employeecd"] = objDTAtt.Rows[i]["Employeecd"].ToString();
                                        dr["Employeename"] = "";
                                        dr["Attendence"] = "Employee/Configuration Not Found";
                                        dtEmployee.Rows.Add(dr);
                                    }
                                    gvAttendence.DataSource = dtEmployee;
                                    gvAttendence.DataBind();
                                    pnlGVList.Visible = true;
                                    lblAttHeading.Text = "Employee/Configuration Not Present In Database But Present In Attendence List";
                                }

                                //Salary Not Approved
                                string strQryValidSal = "";
                                strQryValidSal = " SELECT tbl.Employeecd, emp.Employeename, tbl.Employeecd as Salary FROM (SELECT OrgId, Employeecd, MAX(Docdate)AS DocDate FROM dbo.M_Salary";
                                strQryValidSal += " WHERE(OrgId = " + Convert.ToInt16(Session["OrgID"]) + ") AND(Docdate <= '" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "') AND(Approval = 'N') GROUP BY OrgId, Employeecd) AS tbl ";
                                //strQryValidSal += " INNER JOIN dbo.M_Salary AS c ON tbl.OrgId = c.OrgId AND tbl.Employeecd = c.Employeecd AND tbl.DocDate = c.Docdate ";
                                strQryValidSal += " INNER JOIN dbo.M_Emp emp ON tbl.OrgId = emp.OrgId AND tbl.Employeecd = emp.Employeecd where tbl.orgID=" + Convert.ToInt16(Session["OrgID"]) + " and emp.IsActive='Y'";
                                DataTable objDTValidSal = SqlHelper.ExecuteDataTable(strQryValidSal, AppGlobal.strConnString);
                                if (objDTValidSal.Rows.Count > 0)
                                {
                                    for (int i = 0; i < objDTValidSal.Rows.Count; i++)
                                    {
                                        drSal = dtSalary.NewRow();
                                        drSal["Employeecd"] = objDTValidSal.Rows[i]["Employeecd"].ToString();
                                        drSal["Employeename"] = objDTValidSal.Rows[i]["Employeename"].ToString();
                                        if (objDTValidSal.Rows[i]["Salary"].ToString() != "Y")
                                            drSal["Attendence"] = "Salaray Not Approved";

                                        dtSalary.Rows.Add(drSal);

                                        strSalDet = "delete from T_Attendance where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                        objSalDet = SqlHelper.ExecuteDataTable(strSalDet, AppGlobal.strConnString);
                                    }
                                    gvSalary.DataSource = dtSalary;
                                    gvSalary.DataBind();
                                    pnlGVList.Visible = true;
                                    return;
                                }


                                //Salary Not Exist
                                string strQryValidSalExt = "";
                                strQryValidSalExt = "  select emp.employeecd, emp.Employeename from M_emp emp where  orgID=" + Convert.ToInt16(Session["OrgID"]) + " and emp.Employeecd not in (select Employeecd from M_salary sal where sal.orgID=" + Convert.ToInt16(Session["OrgID"]) + ") order by emp.employeecd";
                                DataTable objDTValidSalExt = SqlHelper.ExecuteDataTable(strQryValidSalExt, AppGlobal.strConnString);
                                if (objDTValidSalExt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < objDTValidSalExt.Rows.Count; i++)
                                    {
                                        drSal = dtSalary.NewRow();
                                        drSal["Employeecd"] = objDTValidSalExt.Rows[i]["Employeecd"].ToString();
                                        drSal["Employeename"] = objDTValidSalExt.Rows[i]["Employeename"].ToString();
                                        drSal["Attendence"] = "Salaray Not Found";
                                        dtSalary.Rows.Add(drSal);
                                    }
                                    gvSalary.DataSource = dtSalary;
                                    gvSalary.DataBind();
                                    pnlGVList.Visible = true;
                                    empSalCount = objDTValidSalExt.Rows.Count;
                                }

                                //if employee count and attendnce count not same delete attendence
                                if ((empMastCount != empAttCount) || (empSalCount != 0) || gvAttendence.Rows.Count > 0)
                                {
                                    if (empSalCount != 0)
                                    {
                                        returnStr = "Attendence Not Imported!!! No. Of Employees In Organisation=" + empMastCount + " and No Of Employees In Salary Master =" + (empMastCount - empSalCount);
                                    }
                                    else
                                    {
                                        returnStr = "Attendence Not Imported!!! No. Of Employees In Organisation=" + empMastCount + " and No Of Employees In Attendence Excel=" + empAttCount;
                                    }
                                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('" + returnStr + "'); ", true);
                                    strSalDet = "delete from T_Attendance where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                    objSalDet = SqlHelper.ExecuteDataTable(strSalDet, AppGlobal.strConnString);
                                    return;
                                }

                                //Employee Configuration
                                string strQyValidAtt = "";

                                strQyValidAtt = " SELECT emp.Employeecd, emp.Employeename, emp.Leavedate, udfEmpConfigurationmax1_1.conId FROM dbo.M_Emp AS emp LEFT OUTER JOIN dbo.udfEmpConfigurationmax1(" + Convert.ToInt16(Session["OrgID"]) + ",'" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "', 'desg') AS udfEmpConfigurationmax1_1 ON emp.OrgId = udfEmpConfigurationmax1_1.OrgId AND ";
                                strQyValidAtt += " emp.Employeecd = udfEmpConfigurationmax1_1.Employeecd LEFT OUTER JOIN dbo.T_Attendance AS att ON emp.Employeecd = att.Employeecd AND emp.OrgId = att.OrgId WHERE(emp.OrgId = " + Convert.ToInt16(Session["OrgID"]) + ") AND(emp.IsActive = 'Y') AND(att.MonYrcd = '" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' OR att.MonYrcd IS NULL) AND (emp.Leavedate IS NULL OR ";
                                strQyValidAtt += " emp.Leavedate >= '" + Convert.ToDateTime(firstDate).ToString("dd MMM yyyy") + "') AND DatofJoin <='" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "' AND(ISNULL(udfEmpConfigurationmax1_1.conId, 0) = 0)";
                                DataTable objDTValidAtte = SqlHelper.ExecuteDataTable(strQyValidAtt, AppGlobal.strConnString);
                                if (objDTValidAtte.Rows.Count > 0)
                                {
                                    for (int i = 0; i < objDTValidAtte.Rows.Count; i++)
                                    {
                                        dr = dtConfiguration.NewRow();
                                        if (objDTValidAtte.Rows[i]["leavedate"] == DBNull.Value)
                                        {
                                            dr["Employeecd"] = objDTValidAtte.Rows[i]["Employeecd"].ToString();
                                            dr["Employeename"] = objDTValidAtte.Rows[i]["Employeename"].ToString();
                                            dr["Attendence"] = "Employee Configuration Not Found";
                                            dtConfiguration.Rows.Add(dr);
                                        }
                                    }
                                    strSalDet = "delete from T_Attendance where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                    objSalDet = SqlHelper.ExecuteDataTable(strSalDet, AppGlobal.strConnString);
                                    gvConfiguration.DataSource = dtConfiguration;
                                    gvConfiguration.DataBind();
                                    pnlGVList.Visible = true;
                                    return;
                                }


                                //Employee Allowance Configuration
                                string strQuryValidAtt = "";

                                strQuryValidAtt = " SELECT Employeecd, Employeename, Leavedate FROM udfAllowance(" + Convert.ToInt16(Session["OrgID"]) + ",'" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "', 'div')  ";
                                strQuryValidAtt += " WHERE (IsActive = 'Y')  AND (Leavedate IS NULL OR ";
                                strQuryValidAtt += " Leavedate >= '" + Convert.ToDateTime(firstDate).ToString("dd MMM yyyy") + "') AND DatofJoin <='" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "' ";
                                DataTable objDTValidAll = SqlHelper.ExecuteDataTable(strQuryValidAtt, AppGlobal.strConnString);
                                if (objDTValidAll.Rows.Count > 0)
                                {
                                    for (int i = 0; i < objDTValidAll.Rows.Count; i++)
                                    {
                                        dr = dtAllowConfiguration.NewRow();
                                        if (objDTValidAll.Rows[i]["leavedate"] == DBNull.Value)
                                        {
                                            dr["Employeecd"] = objDTValidAll.Rows[i]["Employeecd"].ToString();
                                            dr["Employeename"] = objDTValidAll.Rows[i]["Employeename"].ToString();
                                            dr["Attendence"] = "Employee Allowance Configuration Not Approved or Blank";
                                            dtAllowConfiguration.Rows.Add(dr);
                                        }
                                    }
                                    strSalDet = "delete from T_Attendance where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                    objSalDet = SqlHelper.ExecuteDataTable(strSalDet, AppGlobal.strConnString);
                                    gvAllowance.DataSource = dtAllowConfiguration;
                                    gvAllowance.DataBind();
                                    pnlGVList.Visible = true;
                                    return;
                                }


                                //Insert Log
                                string strQry = "";
                                strQry = "INSERT INTO T_Log(OrgId,MonthYrcd, Employeecd, MenuId, Mode, Computername) VALUES(@OrgId,@MonthYrcd, @Employeecd, @MenuId, @Mode, @Computername)";
                                bool result = false;
                                SqlParameter[] paraLog = new SqlParameter[6];
                                paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));

                                paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                                paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                                paraLog[3] = new SqlParameter("@Mode", "A");
                                paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());
                                paraLog[5] = new SqlParameter("@MonthYrcd", ddlMon.SelectedValue + ddlYear.SelectedValue);
                                result = SqlHelper.ExecuteNonQuery(strQry, paraLog, AppGlobal.strConnString);
                                if (result)
                                {
                                    sqlTrans.Commit();
                                    if (gvSalary.Rows.Count == 0 && gvAttendence.Rows.Count == 0)
                                    {
                                        clearControls();
                                    }

                                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Attendence Uploaded Successfully.'); ", true);
                                    return;
                                }
                            }
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Excel File'); ", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                lblMsg.Text = ex.ToString();
            }
        }
        protected void gvAttendence_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAttendence.PageIndex = e.NewPageIndex;

        }

        private void clearControls()
        {
            ddlMon.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;
            ddlMon.Focus();
            lblAttHeading.Text = "Attendence Not Found List";
            btnSave.Text = "Save";

            gvAttendence.DataSource = null;
            gvAttendence.DataBind();
            gvSalary.DataSource = null;
            gvSalary.DataBind();

            lblMsg.Text = "";
        }

        protected bool formValidation()
        {
            int nID = 0;
            if (ddlMon.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Month'); ", true);
                return false;
            }
            if (ddlYear.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Year'); ", true);
                return false;
            }
            //if(!FUExcel.HasFile)
            //{
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Excel File'); ", true);
            //    return false;
            //}
            return true;
        }

        protected void exist()
        {
            if (ddlMon.SelectedValue != "00" && ddlYear.SelectedValue != "00")
            {
                string strQry = "select * from T_Attendance where OrgId=" + Convert.ToInt16(Session["orgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    lblMsg.Text = "Attendance Already Imported";
                }
                else
                {
                    lblMsg.Text = "";
                }
            }
            else
            {
                lblMsg.Text = "";
            }
        }

        protected void ddlMon_SelectedIndexChanged(object sender, EventArgs e)
        {
            exist();
            ddlYear.Focus();
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            exist();
            FUExcel.Focus();
        }

        protected void gvAttendence_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}