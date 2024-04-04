using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SqlClient;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Configuration;

namespace PayRoll.Masters
{
    public partial class DeductionImpExcel : System.Web.UI.Page
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
                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);

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
                    string extension = Path.GetExtension(FUExcel.PostedFile.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(FUExcel.PostedFile.FileName);
                    string strConcat = DateTime.Now.ToString("ddMMyyyy_HHmmss");

                    string excelPath = Server.MapPath("~/Imports/" + fileName + "_" + strConcat + extension);
                    FUExcel.SaveAs(excelPath);

                    string conString = string.Empty;
                    conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
                    conString = string.Format(conString, excelPath);

                    using (OleDbConnection excel_con = new OleDbConnection(conString))
                    {
                        excel_con.Open();
                        string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                        DataTable dtExcelData = new DataTable();

                        //[OPTIONAL]: It is recommended as otherwise the data will be considered as String by default.
                        dtExcelData.Columns.AddRange(new DataColumn[8] {new DataColumn("OrgID", typeof(int)),
                                                                        new DataColumn("MonYrcd", typeof(string)),
                                                                        new DataColumn("EMPCODE", typeof(string)),
                                                                        new DataColumn("Advance", typeof(decimal)),
                                                                        new DataColumn("Loan", typeof(decimal)),
                                                                        new DataColumn("TDS", typeof(decimal)),
                                                                        new DataColumn("TardalPathsansth", typeof(decimal)),
                                                                        new DataColumn("EntertainmentCost", typeof(decimal)),
                                                                    

                                                });

                        dtExcelData.Columns["OrgID"].DefaultValue = Session["OrgID"].ToString();
                        dtExcelData.Columns["MonYrcd"].DefaultValue = ddlMon.SelectedValue + ddlYear.SelectedValue;

                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT EMPCODE, Advance, Loan, TDS, TardalPathsansth, EntertainmentCost FROM[" + sheet1 + "]", excel_con))
                        {
                            oda.Fill(dtExcelData);

                            string strQry = "SELECT * FROM T_MonthlyDeduction Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and orgID=" + Convert.ToInt32(Session["OrgID"]);
                            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                            if (objDT.Rows.Count > 0)
                            {
                                string strQry1 = "delete FROM T_MonthlyDeduction Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'and orgID=" + Convert.ToInt32(Session["OrgID"]);
                                DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
                            }
                        }
                        excel_con.Close();

                        string consString = ConfigurationManager.ConnectionStrings["VanitaPayrollConnectionString"].ConnectionString;
                        using (SqlConnection con = new SqlConnection(consString))
                        {
                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                            {
                                //Set the database table name
                                sqlBulkCopy.DestinationTableName = "dbo.T_MonthlyDeduction";

                                //[OPTIONAL]: Map the Excel columns with that of the database table
                                sqlBulkCopy.ColumnMappings.Add("OrgID", "OrgId");
                                sqlBulkCopy.ColumnMappings.Add("MonYrcd", "MonYrcd");
                                sqlBulkCopy.ColumnMappings.Add("EMPCODE", "Employeecd");
                                sqlBulkCopy.ColumnMappings.Add("Advance", "Advance");
                                sqlBulkCopy.ColumnMappings.Add("Loan", "Loan");
                                sqlBulkCopy.ColumnMappings.Add("TDS", "TDS");
                                sqlBulkCopy.ColumnMappings.Add("TardalPathsansth", "TardalPathsansth");
                                sqlBulkCopy.ColumnMappings.Add("EntertainmentCost", "Ded1");

                                con.Open();
                                sqlBulkCopy.WriteToServer(dtExcelData);
                                con.Close();

                                //string strOut = "";
                                //strOut = validation.validateDeduction(Convert.ToInt32(Session["OrgId"]), ddlMon.SelectedValue + ddlYear.SelectedValue);
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
                                gvDeduction.DataSource = null;
                                gvDeduction.DataBind();

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

                                DataTable dtDeduction = new DataTable();
                                DataRow drDed;
                                dtDeduction.Columns.Add("Employeecd", typeof(string));
                                dtDeduction.Columns.Add("Employeename", typeof(string));
                                dtDeduction.Columns.Add("Attendence", typeof(string));
                                string strSalDet = "";

                                DataTable objSalDet = new DataTable();

                                int empMastCount = 0;
                                int empAttCount = 0;
                                int empSalCount = 0;
                                int empDedCount = 0;
                                int empSalNoApprovCount = 0;
                                string returnStr = "";

                                //Employees Active Count
                                string strQryEmp = "select count(distinct(employeecd)) as EmpMastCount from M_Emp where (leaveDate is null or leavedate>'" + Convert.ToDateTime(firstDate).ToString("dd MMM yyyy") + "') AND DatofJoin <'" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "' and orgID=" + Convert.ToInt16(Session["OrgID"]) + " and isActive='Y'";
                                DataTable objDTEmp = SqlHelper.ExecuteDataTable(strQryEmp, AppGlobal.strConnString);
                                if (objDTEmp.Rows.Count > 0)
                                {
                                    empMastCount = Convert.ToInt32(objDTEmp.Rows[0]["EmpMastCount"]);
                                }

                                //Attendence Count
                                string strQryAtt = "select count(distinct(employeecd)) as empAttCount from T_Attendance where orgID =" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                DataTable objDTAtt = SqlHelper.ExecuteDataTable(strQryAtt, AppGlobal.strConnString);
                                if (objDTAtt.Rows.Count > 0)
                                {
                                    empAttCount = Convert.ToInt32(objDTAtt.Rows[0]["empAttCount"]);
                                }

                                //Deduction Count
                                string strQryDed = "select count(distinct(employeecd)) as empDedCount from T_MonthlyDeduction where orgID =" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                DataTable objDTDed = SqlHelper.ExecuteDataTable(strQryDed, AppGlobal.strConnString);
                                if (objDTDed.Rows.Count > 0)
                                {
                                    empDedCount = Convert.ToInt32(objDTDed.Rows[0]["empDedCount"]);
                                }


                                //Employee In Deduction Not Found In Database
                                string strQryValidDed = "";
                                strQryValidDed = "select ded.Employeecd from T_MonthlyDeduction ded where ded.OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and ded.MonYrcd = '" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and Employeecd  not in(select Employeecd from M_Emp emp where emp.OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and (emp.leaveDate is null or emp.leavedate>='" + Convert.ToDateTime(firstDate).ToString("dd MMM yyyy") + "') AND DatofJoin <'" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "' and emp.isActive='Y' )";
                                DataTable objDTValidDed = SqlHelper.ExecuteDataTable(strQryValidDed, AppGlobal.strConnString);
                                if (objDTValidDed.Rows.Count > 0)
                                {
                                    for (int i = 0; i < objDTValidDed.Rows.Count; i++)
                                    {
                                        drDed = dtDeduction.NewRow();
                                        drDed["Employeecd"] = objDTValidDed.Rows[i]["Employeecd"].ToString();
                                        drDed["Employeename"] = "";
                                        drDed["Attendence"] = "Employee Not Found";
                                        dtDeduction.Rows.Add(drDed);
                                    }
                                    gvDeduction.DataSource = dtDeduction;
                                    gvDeduction.DataBind();
                                    pnlGVList.Visible = true;
                                }


                                //Attendance Not Found
                                string strQryValidAtt = "";

                                //strQryValidAtt = "select emp.Employeename, ded.employeecd from T_MonthlyDeduction ded inner join M_Emp emp on emp.OrgID = ded.OrgID and emp.Employeecd = ded.Employeecd";
                                //strQryValidAtt += " where ded.orgID = " + Convert.ToInt16(Session["OrgID"]) + " and ded.Employeecd not in (select Employeecd from T_Attendance att where att.orgID = " + Convert.ToInt16(Session["OrgID"]) + "  and att.MonYrCd = '" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' ) ";
                                strQryValidAtt =  " SELECT emp.Employeename, ded.Employeecd, udfEmpConfigurationmax1_1.conId FROM dbo.T_MonthlyDeduction AS ded INNER JOIN dbo.M_Emp AS emp ON emp.OrgId = ded.OrgId AND emp.Employeecd = ded.Employeecd INNER JOIN dbo.udfEmpConfigurationmax1("+ Convert.ToInt16(Session["OrgID"]) + ",'" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "', 'desg') AS udfEmpConfigurationmax1_1 ON emp.OrgId = udfEmpConfigurationmax1_1.OrgId AND ";
                                strQryValidAtt += " emp.Employeecd = udfEmpConfigurationmax1_1.Employeecd WHERE(ded.OrgId = " + Convert.ToInt16(Session["OrgID"]) + " ) and ded.MonYrcd ='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' AND(ded.Employeecd NOT IN (SELECT Employeecd FROM dbo.T_Attendance AS att WHERE(OrgId = " + Convert.ToInt16(Session["OrgID"]) + " ) AND(MonYrcd = '" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'))) AND(udfEmpConfigurationmax1_1.conId <> 21)";
                                DataTable objDTValidAtt;
                                objDTValidAtt = SqlHelper.ExecuteDataTable(strQryValidAtt, AppGlobal.strConnString);
                                if (objDTValidAtt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < objDTValidAtt.Rows.Count; i++)
                                    {
                                        dr = dtEmployee.NewRow();
                                        dr["Employeecd"] = objDTValidAtt.Rows[i]["Employeecd"].ToString();
                                        dr["Employeename"] = objDTValidAtt.Rows[i]["Employeename"].ToString();
                                        dr["Attendence"] = "Attendence Not Found";
                                        dtEmployee.Rows.Add(dr);
                                    }
                                    gvAttendence.DataSource = dtEmployee;
                                    gvAttendence.DataBind();
                                    pnlGVList.Visible = true;
                                }


                                //Deduction Count > Than Employee Master
                                if (empDedCount > empMastCount)
                                {
                                    strQryDed = "select ded.Employeecd from T_MonthlyDeduction ded where ded.OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and ded.MonYrcd = '" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'and Employeecd  in(select Employeecd from M_Emp emp where emp.IsActive = 'N' and emp.OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and leaveDate is null or leavedate>='" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "')";
                                    objDTDed = SqlHelper.ExecuteDataTable(strQryDed, AppGlobal.strConnString);
                                    if (objDTDed.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < objDTDed.Rows.Count; i++)
                                        {
                                            drDed = dtDeduction.NewRow();
                                            drDed["Employeecd"] = objDTDed.Rows[i]["Employeecd"].ToString();
                                            drDed["Employeename"] = "";
                                            drDed["Attendence"] = "Employee Not Found";
                                            dtDeduction.Rows.Add(drDed);
                                        }
                                        gvDeduction.DataSource = dtDeduction;
                                        gvDeduction.DataBind();
                                        pnlGVList.Visible = true;
                                        lblDedHeading.Text = "Employee Not Present In Database But Present In Deduction List";
                                    }
                                }



                                //Salary Not Approved
                                string strQryValidSal = "";
                                strQryValidSal = "select emp.Employeecd,emp.Employeename,emp.leavedate from M_Emp emp left join T_Attendance att on emp.Employeecd = att.employeecd and emp.orgID = att.orgID ";
                                strQryValidSal += " where emp.orgID = " + Convert.ToInt16(Session["OrgID"]) + " and emp.IsActive = 'Y' and(att.monyrcd = '" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' or att.monyrcd is null) and (att.employeecd is null) and ";
                                strQryValidSal += " (emp.leavedate is null or emp.leavedate > '" + Convert.ToDateTime(firstDate).ToString("dd MMM yyyy") + "') and DatofJoin < '" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "'";
                                DataTable objDTValidSal = SqlHelper.ExecuteDataTable(strQryValidSal, AppGlobal.strConnString);
                                if (objDTValidAtt.Rows.Count > 0)
                                {
                                    empSalNoApprovCount = Convert.ToInt32(objDTValidAtt.Rows[0]["empSalNoApprovCount"]);
                                }

                                //strQryValidSal = " SELECT tbl.Employeecd, emp.Employeename, c.Employeecd as Salary FROM (SELECT OrgId, Employeecd, MAX(Docdate)AS DocDate FROM dbo.M_Salary";
                                //strQryValidSal += " WHERE(OrgId = " + Convert.ToInt16(Session["OrgID"]) + ") AND(Docdate <= '" + Convert.ToDateTime(dt) + "') AND(Approval = 'N') GROUP BY OrgId, Employeecd) AS tbl ";
                                //strQryValidSal += " INNER JOIN dbo.M_Salary AS c ON tbl.OrgId = c.OrgId AND tbl.Employeecd = c.Employeecd AND tbl.DocDate = c.Docdate ";
                                //strQryValidSal += " INNER JOIN dbo.M_Emp emp ON tbl.OrgId = emp.OrgId AND tbl.Employeecd = emp.Employeecd where tbl.orgID=" + Convert.ToInt16(Session["OrgID"]) + " and emp.IsActive='Y'";

                                strQryValidSal = " SELECT tbl.Employeecd, emp.Employeename, tbl.Employeecd as Salary FROM (SELECT OrgId, Employeecd, MAX(Docdate)AS DocDate FROM dbo.M_Salary";
                                strQryValidSal += " WHERE(OrgId = " + Convert.ToInt16(Session["OrgID"]) + ") AND(Docdate <= '" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "') AND(Approval = 'N') GROUP BY OrgId, Employeecd) AS tbl ";
                                strQryValidSal += " INNER JOIN dbo.M_Emp emp ON tbl.OrgId = emp.OrgId AND tbl.Employeecd = emp.Employeecd where tbl.orgID=" + Convert.ToInt16(Session["OrgID"]) + " and emp.IsActive='Y'";

                                objDTValidSal = SqlHelper.ExecuteDataTable(strQryValidSal, AppGlobal.strConnString);
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

                                        //strSalDet = "delete from T_Attendance where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and Employeecd='" + objDTValidSal.Rows[i]["Employeecd"].ToString() + "'";
                                        //objSalDet = SqlHelper.ExecuteDataTable(strSalDet, AppGlobal.strConnString);

                                        strQryDed = "delete from T_MonthlyDeduction where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and Employeecd='" + objDTValidSal.Rows[i]["Employeecd"].ToString() + "'";
                                        objSalDet = SqlHelper.ExecuteDataTable(strQryDed, AppGlobal.strConnString);
                                    }
                                    gvSalary.DataSource = dtSalary;
                                    gvSalary.DataBind();
                                    pnlGVList.Visible = true;
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
                                //if ((empMastCount !=( empAttCount+empSalNoApprovCount) || empMastCount  != empDedCount || (empAttCount+empSalNoApprovCount) != empDedCount ) || (empSalCount != 0))
                                //{
                                //    strSalDet = "delete from T_MonthlyDeduction where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                //    objSalDet = SqlHelper.ExecuteDataTable(strSalDet, AppGlobal.strConnString);

                                //    //if (empMastCount != empAttCount)
                                //    //{
                                //    //    returnStr = "Deduction Not Imported!!! No. Of Employees In Organisation=" + empMastCount + " and No Of Employees In Attendence Master =" + empAttCount;
                                //    //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('" + returnStr + "'); ", true);
                                //    //    return;
                                //    //}

                                //    //if (empMastCount != empDedCount)
                                //    //{
                                //    //    returnStr = "Deduction Not Imported!!! No. Of Employees In Organisation=" + empMastCount + " and No Of Employees In Deduction Master =" + empDedCount;
                                //    //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('" + returnStr + "'); ", true);
                                //    //    return;
                                //    //}
                                //    //if (empAttCount != empDedCount)
                                //    //{
                                //    //    returnStr = "Deduction Not Imported!!! No. Of Employees In Attendence=" + empAttCount + " and No Of Employees In Deduction Master =" + empDedCount;
                                //    //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('" + returnStr + "'); ", true);
                                //    //    return;
                                //    //}
                                //    if (empSalCount != 0)
                                //    {
                                //        returnStr = "Deduction Not Imported!!! No. Of Employees In Organisation=" + empMastCount + " and No Of Employees In Salary Master =" + (empMastCount - empSalCount);
                                //        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('" + returnStr + "'); ", true);
                                //        return;
                                //    }                                   
                                //}

                                //if (empSalCount != 0)
                                //{
                                //    strSalDet = "delete from T_MonthlyDeduction where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                //    objSalDet = SqlHelper.ExecuteDataTable(strSalDet, AppGlobal.strConnString);

                                //    returnStr = "Deduction Not Imported!!! No. Of Employees In Organisation=" + empMastCount + " and No Of Employees In Salary Master =" + (empMastCount - empSalCount);
                                //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('" + returnStr + "'); ", true);
                                //    return;
                                //}

                                if (gvAttendence.Rows.Count > 0)
                                {
                                    strSalDet = "delete from T_MonthlyDeduction where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                    objSalDet = SqlHelper.ExecuteDataTable(strSalDet, AppGlobal.strConnString);

                                    returnStr = "Deduction Not Imported!!! Attendance Not Found But Deduction Present";
                                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('" + returnStr + "'); ", true);
                                    return;
                                }
                                if (gvDeduction.Rows.Count > 0)
                                {
                                    strSalDet = "delete from T_MonthlyDeduction where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                    objSalDet = SqlHelper.ExecuteDataTable(strSalDet, AppGlobal.strConnString);

                                    returnStr = "Deduction Not Imported!!! No. Of Employees In Deduction=" + gvDeduction.Rows.Count + " are Not Found In Database";
                                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('" + returnStr + "'); ", true);
                                    return;
                                }

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
                                    if (gvSalary.Rows.Count == 0 && gvAttendence.Rows.Count == 0 && gvDeduction.Rows.Count == 0)
                                    {
                                        clearControls();
                                    }
                                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Deduction Uploaded Successfully.'); ", true);
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
                string error = "Error : " + ex.ToString();
               
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('" + error + "'); ", true);

            }
        }

        private void clearControls()
        {
            ddlMon.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;

            btnSave.Text = "Save";

            lblAttHeading.Text = "Attendence Not Found List";
            lblDedHeading.Text = "Employee Not Found List";

            gvAttendence.DataSource = null;
            gvAttendence.DataBind();
            gvSalary.DataSource = null;
            gvSalary.DataBind();

            gvDeduction.DataSource = null;
            gvDeduction.DataBind();
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
                string strQry = "select * from T_MonthlyDeduction where OrgId=" + Convert.ToInt16(Session["orgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    lblMsg.Text = "Deduction Already Imported";
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
    }
}