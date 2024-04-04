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
    public partial class EarningImpExcel : System.Web.UI.Page
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
                    string excelPath = Server.MapPath("~/Imports/") + Path.GetFileName(FUExcel.PostedFile.FileName);
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
                        dtExcelData.Columns.AddRange(new DataColumn[6] { new DataColumn("OrgID", typeof(int)),
                                                                    new DataColumn("MonYrcd", typeof(string)),
                                                                    new DataColumn("EMPCODE", typeof(string)),
                                                                    new DataColumn("ear1", typeof(decimal)),
                                                                     new DataColumn("ear2", typeof(decimal)),
                                                                    new DataColumn("ear3", typeof(decimal))
                                                });

                        dtExcelData.Columns["OrgID"].DefaultValue = Session["OrgID"].ToString();
                        dtExcelData.Columns["MonYrcd"].DefaultValue = ddlMon.SelectedValue + ddlYear.SelectedValue;

                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT EMPCODE, ear1, ear2, ear3 FROM[" + sheet1 + "]", excel_con))
                        {
                            oda.Fill(dtExcelData);

                            string strQry = "SELECT * FROM T_MonthlyEarning Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and orgID=" + Convert.ToInt32(Session["OrgID"]);
                            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                            if (objDT.Rows.Count > 0)
                            {
                                string strQry1 = "delete FROM T_MonthlyEarning Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'and orgID=" + Convert.ToInt32(Session["OrgID"]);
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
                                sqlBulkCopy.DestinationTableName = "dbo.T_MonthlyEarning";

                                //[OPTIONAL]: Map the Excel columns with that of the database table
                                //sqlBulkCopy.ColumnMappings.Add("OrgID", "OrgID");

                                con.Open();
                                sqlBulkCopy.WriteToServer(dtExcelData);
                                con.Close();

                                //string strOut = "";
                                //strOut = validation.validateEarning(Convert.ToInt32(Session["OrgId"]), ddlMon.SelectedValue + ddlYear.SelectedValue);
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
                                gvEarning.DataSource = null;
                                gvEarning.DataBind();
                                string dt = "01/" + ddlMon.SelectedValue + "/" + ddlYear.SelectedValue;

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

                                DataTable dtEarning = new DataTable();
                                DataRow drEar;
                                dtEarning.Columns.Add("Employeecd", typeof(string));
                                dtEarning.Columns.Add("Employeename", typeof(string));
                                dtEarning.Columns.Add("Attendence", typeof(string));
                                string strSalDet = "";

                                DataTable objSalEar = new DataTable();

                                int empMastCount = 0;
                                int empAttCount = 0;
                                int empSalCount = 0;
                                int empDedCount = 0;
                                int empEarCount = 0;
                                int empSalNoApprovCount = 0;

                                string returnStr = "";

                                //Employees Active Count
                                string strQryEmp = "select count(distinct(employeecd)) as EmpMastCount from M_Emp where (leaveDate is null or leavedate<'" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "') and orgID=" + Convert.ToInt16(Session["OrgID"]) + " and isActive='Y'";
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

                                //Earning Count
                                string strQryEar = "select count(distinct(employeecd)) as empEarCount from T_MonthlyEarning where orgID =" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                DataTable objDTEar = SqlHelper.ExecuteDataTable(strQryEar, AppGlobal.strConnString);
                                if (objDTEar.Rows.Count > 0)
                                {
                                    empEarCount = Convert.ToInt32(objDTEar.Rows[0]["empEarCount"]);
                                }


                                ////Earning Not Found
                                //string strQryValidEar = "";
                                //strQryValidEar = "select emp.Employeecd,emp.Employeename,emp.leavedate from M_Emp emp";
                                //strQryValidEar += " left join T_MonthlyEarning ear on emp.Employeecd = ear.employeecd and emp.orgID = ear.orgID";
                                //strQryValidEar += " where emp.orgID = " + Convert.ToInt16(Session["OrgID"]) + " and emp.IsActive='Y' and(ear.employeecd is null) or ear.monyrcd <>'" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' or emp.leaveDate is not null or emp.leavedate<'" + Convert.ToDateTime(dt) + "'";
                                //DataTable objDTValidEar = SqlHelper.ExecuteDataTable(strQryValidEar, AppGlobal.strConnString);
                                //if (objDTValidEar.Rows.Count > 0)
                                //{
                                //    for (int i = 0; i < objDTValidEar.Rows.Count; i++)
                                //    {
                                //        drEar = dtEarning.NewRow();
                                //        if (objDTValidEar.Rows[i]["leavedate"] == DBNull.Value)
                                //        {
                                //            drEar["Employeecd"] = objDTValidEar.Rows[i]["Employeecd"].ToString();
                                //            drEar["Employeename"] = objDTValidEar.Rows[i]["Employeename"].ToString();
                                //            drEar["Attendence"] = "Earning Not Found";
                                //            dtEarning.Rows.Add(drEar);
                                //        }
                                //    }
                                //    gvEarning.DataSource = dtEarning;
                                //    gvEarning.DataBind();
                                //    pnlGVList.Visible = true;
                                //}


                                //Employee In Earning Not Found In Database
                                string strQryValidEar = "";
                                strQryValidEar = "select ear.Employeecd from T_MonthlyEarning ear where ear.OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and ear.MonYrcd = '" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and Employeecd  not in(select Employeecd from M_Emp emp )";
                                DataTable objDTValidEar = SqlHelper.ExecuteDataTable(strQryValidEar, AppGlobal.strConnString);
                                if (objDTValidEar.Rows.Count > 0)
                                {
                                    for (int i = 0; i < objDTValidEar.Rows.Count; i++)
                                    {
                                        drEar = dtEarning.NewRow();
                                        drEar["Employeecd"] = objDTValidEar.Rows[i]["Employeecd"].ToString();
                                        drEar["Employeename"] = "";
                                        drEar["Attendence"] = "Employee Not Found";
                                        dtEarning.Rows.Add(drEar);
                                    }
                                    gvEarning.DataSource = dtEarning;
                                    gvEarning.DataBind();
                                    pnlGVList.Visible = true;
                                }

                                //Attendence Not Found
                                string strQryValidAtt = "";

                                strQryValidAtt = " SELECT count(emp.Employeecd) as empSalNoApprovCount FROM dbo.M_Emp AS emp";
                                strQryValidAtt += " LEFT OUTER JOIN dbo.M_Salary ON emp.Employeecd = dbo.M_Salary.Employeecd AND emp.OrgId = dbo.M_Salary.OrgId";
                                strQryValidAtt += " FULL OUTER JOIN dbo.T_Attendance AS att ON emp.Employeecd = att.Employeecd AND emp.OrgId = att.OrgId AND dbo.M_Salary.MonYrcd = att.MonYrcd WHERE(emp.OrgId = 1) AND(emp.IsActive = 'Y') AND(att.Employeecd IS NULL) AND(dbo.M_Salary.Approval = 'N') OR(att.MonYrcd <> '042019')";
                                DataTable objDTValidAtt = SqlHelper.ExecuteDataTable(strQryValidAtt, AppGlobal.strConnString);
                                if (objDTValidAtt.Rows.Count > 0)
                                {
                                    empSalNoApprovCount = Convert.ToInt32(objDTValidAtt.Rows[0]["empSalNoApprovCount"]);
                                }
                                //strQryValidAtt = "select emp.Employeecd,emp.Employeename,emp.leavedate from M_Emp emp";
                                //strQryValidAtt += " left join T_Attendance att on emp.Employeecd = att.employeecd and emp.orgID = att.orgID";
                                //strQryValidAtt += " where emp.orgID = " + Convert.ToInt16(Session["OrgID"]) + " and emp.IsActive='Y' and(att.employeecd is null) or att.monyrcd <>'" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' or emp.leaveDate is not null or emp.leavedate<'" + Convert.ToDateTime(dt) + "'";
                                strQryValidAtt = "SELECT emp.Employeecd, emp.Employeename, emp.Leavedate FROM dbo.M_Emp AS emp ";
                                strQryValidAtt += " LEFT OUTER JOIN dbo.M_Salary ON emp.Employeecd = dbo.M_Salary.Employeecd AND emp.OrgId = dbo.M_Salary.OrgId ";
                                strQryValidAtt += " FULL OUTER JOIN dbo.T_Attendance AS att ON emp.Employeecd = att.Employeecd AND emp.OrgId = att.OrgId AND dbo.M_Salary.MonYrcd = att.MonYrcd";
                                strQryValidAtt += " WHERE(emp.OrgId = " + Convert.ToInt16(Session["OrgID"]) + ") AND(emp.IsActive = 'Y') AND(att.Employeecd IS NULL) AND(dbo.M_Salary.Approval = 'Y') OR (att.MonYrcd <> '" + ddlMon.SelectedValue + ddlYear.SelectedValue + "') OR (emp.Leavedate IS NOT NULL) OR (emp.Leavedate < '" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "')";

                                objDTValidAtt = SqlHelper.ExecuteDataTable(strQryValidAtt, AppGlobal.strConnString);
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


                                //Deduction Count > Than Employee Master
                                if (empEarCount > empMastCount)
                                {
                                    strQryEar = "select ear.Employeecd from T_MonthlyEarning ear where ear.OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and ear.MonYrcd = '" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'and Employeecd  in(select Employeecd from M_Emp emp where emp.IsActive = 'N'  )";
                                    objDTEar = SqlHelper.ExecuteDataTable(strQryEar, AppGlobal.strConnString);
                                    if (objDTEar.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < objDTEar.Rows.Count; i++)
                                        {
                                            drEar = dtEarning.NewRow();
                                            drEar["Employeecd"] = objDTEar.Rows[i]["Employeecd"].ToString();
                                            drEar["Employeename"] = "";
                                            drEar["Attendence"] = "Employee Not Found";
                                            dtEarning.Rows.Add(drEar);
                                        }
                                        gvEarning.DataSource = dtEarning;
                                        gvEarning.DataBind();
                                        pnlGVList.Visible = true;
                                        lblEarHeading.Text = "Employee Not Present In Database But Present In Earning List";
                                    }
                                }

                                //Attendence Count > Than Employee Master
                                if (empAttCount > empMastCount)
                                {
                                    strQryAtt = "select att.Employeecd from T_Attendance att where att.OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and att.MonYrcd = '" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'and Employeecd  in(select Employeecd from M_Emp emp where emp.IsActive = 'N'  )";
                                    objDTAtt = SqlHelper.ExecuteDataTable(strQryAtt, AppGlobal.strConnString);
                                    if (objDTAtt.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < objDTAtt.Rows.Count; i++)
                                        {
                                            dr = dtEmployee.NewRow();
                                            dr["Employeecd"] = objDTAtt.Rows[i]["Employeecd"].ToString();
                                            dr["Employeename"] = "";
                                            dr["Attendence"] = "Employee Not Found";
                                            dtEmployee.Rows.Add(dr);
                                        }
                                        gvAttendence.DataSource = dtEmployee;
                                        gvAttendence.DataBind();
                                        pnlGVList.Visible = true;
                                        lblAttHeading.Text = "Employee Not Present In Database But Present In Attendence List";
                                    }
                                }


                                //Salary Not Approved
                                string strQryValidSal = "";
                                strQryValidSal = " SELECT tbl.Employeecd, emp.Employeename, c.Employeecd as Salary FROM (SELECT OrgId, Employeecd, MAX(Docdate)AS DocDate FROM dbo.M_Salary";
                                strQryValidSal += " WHERE(OrgId = " + Convert.ToInt16(Session["OrgID"]) + ") AND(Docdate <= '" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "') AND(Approval = 'N') GROUP BY OrgId, Employeecd) AS tbl ";
                                strQryValidSal += " INNER JOIN dbo.M_Salary AS c ON tbl.OrgId = c.OrgId AND tbl.Employeecd = c.Employeecd AND tbl.DocDate = c.Docdate ";
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

                                        strSalDet = "delete from T_Attendance where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and Employeecd='" + objDTValidSal.Rows[i]["Employeecd"].ToString() + "'";
                                        objSalEar = SqlHelper.ExecuteDataTable(strSalDet, AppGlobal.strConnString);

                                        strQryEar = "delete from T_MonthlyEarning where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and Employeecd='" + objDTValidSal.Rows[i]["Employeecd"].ToString() + "'";
                                        objSalEar = SqlHelper.ExecuteDataTable(strQryEar, AppGlobal.strConnString);
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

                                //if ((empMastCount != (empAttCount + empSalNoApprovCount) || empMastCount != empEarCount || (empAttCount + empSalNoApprovCount) != empEarCount) || (empSalCount != 0))
                                //{
                                //    strSalDet = "delete from T_MonthlyEarning where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                //    objSalEar = SqlHelper.ExecuteDataTable(strSalDet, AppGlobal.strConnString);

                                //    //if (empMastCount != empAttCount)
                                //    //{
                                //    //    returnStr = "Earning Not Imported!!! No. Of Employees In Organisation=" + empMastCount + " and No Of Employees In Attendence Master =" + empAttCount;
                                //    //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('" + returnStr + "'); ", true);
                                //    //    return;
                                //    //}

                                //    //if (empMastCount != empEarCount)
                                //    //{
                                //    //    returnStr = "Earning Not Imported!!! No. Of Employees In Organisation=" + empMastCount + " and No Of Employees In Earning =" + empEarCount;
                                //    //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('" + returnStr + "'); ", true);
                                //    //    return;
                                //    //}
                                //    //if (empAttCount != empEarCount)
                                //    //{
                                //    //    returnStr = "Earning Not Imported!!! No. Of Employees In Attendence=" + empAttCount + " and No Of Employees In Earning =" + empEarCount;
                                //    //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('" + returnStr + "'); ", true);
                                //    //    return;
                                //    //}
                                //    if (empSalCount != 0)
                                //    {
                                //        returnStr = "Earning Not Imported!!! No. Of Employees In Organisation=" + empMastCount + " and No Of Employees In Salary Master =" + (empMastCount - empSalCount);
                                //        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('" + returnStr + "'); ", true);
                                //        return;
                                //    }
                                //}
                                if (empSalCount != 0)
                                {
                                    strSalDet = "delete from T_MonthlyEarning where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                    objSalEar = SqlHelper.ExecuteDataTable(strSalDet, AppGlobal.strConnString);

                                    returnStr = "Earning Not Imported!!! No. Of Employees In Organisation=" + empMastCount + " and No Of Employees In Salary Master =" + (empMastCount - empSalCount);
                                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('" + returnStr + "'); ", true);
                                    return;
                                }

                                if (gvAttendence.Rows.Count > 0)
                                {
                                    strSalDet = "delete from T_MonthlyEarning where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                    objSalEar = SqlHelper.ExecuteDataTable(strSalDet, AppGlobal.strConnString);

                                    returnStr = "Earning Not Imported!!! No. Of Employees In Attendence=" + empAttCount + " and No Of Employees In Earning =" + empEarCount;
                                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('" + returnStr + "'); ", true);
                                    return;
                                }

                                if (gvEarning.Rows.Count > 0)
                                {
                                    strSalDet = "delete from T_MonthlyEarning where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                    objSalEar = SqlHelper.ExecuteDataTable(strSalDet, AppGlobal.strConnString);

                                    returnStr = "Earning Not Imported!!! No. Of Employees In Earning=" + gvEarning.Rows.Count + " are Not Found In Database";
                                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('" + returnStr + "'); ", true);
                                    return;
                                }

                                string strQry = "";
                                strQry = "INSERT INTO T_Log(OrgId, Employeecd, MenuId, Mode, Computername) VALUES(@OrgId, @Employeecd, @MenuId, @Mode, @Computername)";
                                bool result = false;
                                SqlParameter[] paraLog = new SqlParameter[5];
                                paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                                paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                                paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                                paraLog[3] = new SqlParameter("@Mode", "A");
                                paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());

                                result = SqlHelper.ExecuteNonQuery(strQry, paraLog, AppGlobal.strConnString);
                                if (result)
                                {
                                    sqlTrans.Commit();
                                    if (gvSalary.Rows.Count == 0 && gvAttendence.Rows.Count == 0 && gvEarning.Rows.Count == 0)
                                    {
                                        clearControls();
                                    }
                                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Earnings Uploaded Successfully.'); ", true);
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
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
            }
        }

        private void clearControls()
        {
            ddlMon.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;

            btnSave.Text = "Save";

            lblAttHeading.Text = "Attendence Not Found List";
            lblEarHeading.Text = "Earning Not Found List";

            gvAttendence.DataSource = null;
            gvAttendence.DataBind();
            gvSalary.DataSource = null;
            gvSalary.DataBind();

            gvEarning.DataSource = null;
            gvEarning.DataBind();
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

        #region[insert with orgID]
        //private void InsertRecord()
        //{
        //    try
        //    {
        //        //file upload path
        //        if (FUExcel.HasFiles)
        //        {
        //            string excelPath = Server.MapPath("~/") + Path.GetFileName(FUExcel.PostedFile.FileName);
        //            FUExcel.SaveAs(excelPath);

        //            string conString = string.Empty;
        //            conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
        //            conString = string.Format(conString, excelPath);

        //            using (OleDbConnection excel_con = new OleDbConnection(conString))
        //            {
        //                excel_con.Open();
        //                string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
        //                DataTable dtExcelData = new DataTable();

        //                //[OPTIONAL]: It is recommended as otherwise the data will be considered as String by default.
        //                dtExcelData.Columns.AddRange(new DataColumn[7] { new DataColumn("OrgID", typeof(int)),
        //                                                            new DataColumn("MonYrcd", typeof(string)),
        //                                                            new DataColumn("EMPCODE", typeof(string)),
        //                                                            new DataColumn("Advance", typeof(decimal)),
        //                                                             new DataColumn("Loan", typeof(decimal)),
        //                                                            new DataColumn("TDS", typeof(decimal)),
        //                                                            new DataColumn("TardalPathsansth", typeof(decimal)),

        //                                        });

        //                dtExcelData.Columns["MonYrcd"].DefaultValue = ddlMon.SelectedValue + ddlYear.SelectedValue;

        //                using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT OrgID,  EMPCODE, Advance, Loan, TDS, TardalPathsansth FROM[" + sheet1 + "]", excel_con))
        //                {
        //                    oda.Fill(dtExcelData);

        //                    string strQry = "SELECT OrgID, MonYrcd, Employeecd, Advance, Loan, TDS, TardalPathsansth FROM T_MonthlyDeduction Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
        //                    DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
        //                    if (objDT.Rows.Count > 0)
        //                    {
        //                        string strQry1 = "delete FROM T_MonthlyDeduction Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
        //                        DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry1, AppGlobal.strConnString);
        //                    }
        //                }
        //                excel_con.Close();

        //                string consString = ConfigurationManager.ConnectionStrings["VanitaPayrollConnectionString"].ConnectionString;
        //                using (SqlConnection con = new SqlConnection(consString))
        //                {
        //                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
        //                    {
        //                        //Set the database table name
        //                        sqlBulkCopy.DestinationTableName = "dbo.T_MonthlyDeduction";

        //                        //[OPTIONAL]: Map the Excel columns with that of the database table
        //                        //sqlBulkCopy.ColumnMappings.Add("OrgID", "OrgID");

        //                        con.Open();
        //                        sqlBulkCopy.WriteToServer(dtExcelData);
        //                        con.Close();
        //                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Deduction Uploaded Successfully.'); ", true);
        //                        return;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Select Excel File'); ", true);
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        #endregion
    }
}