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
    public partial class HamaliSalaryImpExcel : System.Web.UI.Page
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
                        dtExcelData.Columns.AddRange(new DataColumn[6] { new DataColumn("OrgId", typeof(int)),
                                                                    new DataColumn("MonYrcd", typeof(string)),
                                                                    new DataColumn("EMPCODE", typeof(string)),
                                                                    new DataColumn("Wages", typeof(decimal)),
                                                                     new DataColumn("VehicleHamali", typeof(decimal)),
                                                                    new DataColumn("TotalHamali", typeof(decimal)),
                                                    });

                        dtExcelData.Columns["OrgID"].DefaultValue = Session["OrgID"].ToString();
                        dtExcelData.Columns["MonYrcd"].DefaultValue = ddlMon.SelectedValue + ddlYear.SelectedValue;

                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT EMPCODE, Wages, VehicleHamali, TotalHamali FROM[" + sheet1 + "]", excel_con))
                        {
                            oda.Fill(dtExcelData);

                            string strQry = "SELECT * FROM T_MonthlyHamaliSalary Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and orgID=" + Convert.ToInt32(Session["OrgID"]);
                            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                            if (objDT.Rows.Count > 0)
                            {
                                string strQry1 = "delete FROM T_MonthlyHamaliSalary Where MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'and orgID=" + Convert.ToInt32(Session["OrgID"]);
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
                                sqlBulkCopy.DestinationTableName = "dbo.T_MonthlyHamaliSalary";

                                //[OPTIONAL]: Map the Excel columns with that of the database table
                                sqlBulkCopy.ColumnMappings.Add("OrgID", "OrgId");
                                sqlBulkCopy.ColumnMappings.Add("MonYrcd", "MonYrcd");
                                sqlBulkCopy.ColumnMappings.Add("EmpCode", "Employeecd");
                                sqlBulkCopy.ColumnMappings.Add("Wages", "Wages");
                                sqlBulkCopy.ColumnMappings.Add("VehicleHamali", "VehicleHamali");
                                sqlBulkCopy.ColumnMappings.Add("TotalHamali", "TotalHamali");

                                con.Open();
                                sqlBulkCopy.WriteToServer(dtExcelData);
                                con.Close();
                                
                                //Clear Grid
                                //gvAttendence.DataSource = null;
                                //gvAttendence.DataBind();
                                gvSalary.DataSource = null;
                                gvSalary.DataBind();
                                gvHamali.DataSource = null;
                                gvHamali.DataBind();

                                int lastDay = 0;
                                lastDay = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMon.SelectedValue));
                                string dt = lastDay + "/" + ddlMon.SelectedValue + "/" + ddlYear.SelectedValue;

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

                                DataTable dtHamali = new DataTable();
                                DataRow drH;
                                dtHamali.Columns.Add("Employeecd", typeof(string));
                                dtHamali.Columns.Add("Employeename", typeof(string));
                                dtHamali.Columns.Add("Attendence", typeof(string));
                                string strSalDet = "";

                                DataTable objSalDet = new DataTable();

                                int empMastCount = 0;
                                int empAttCount = 0;
                                int empSalCount = 0;
                                int empHamaliCount = 0;
                                int empSalNoApprovCount = 0;
                                string returnStr = "";

                                //Employees Active Count
                                string strQryEmp = "select count(distinct(employeecd)) as EmpMastCount from M_Emp where (leaveDate is null or leavedate>'" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "') and orgID=" + Convert.ToInt16(Session["OrgID"]) + " and isActive='Y'";
                                DataTable objDTEmp = SqlHelper.ExecuteDataTable(strQryEmp, AppGlobal.strConnString);
                                if (objDTEmp.Rows.Count > 0)
                                {
                                    empMastCount = Convert.ToInt32(objDTEmp.Rows[0]["EmpMastCount"]);
                                }

                                ////Attendence Count
                                //string strQryAtt = "select count(distinct(employeecd)) as empAttCount from T_Attendance where orgID =" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                //DataTable objDTAtt = SqlHelper.ExecuteDataTable(strQryAtt, AppGlobal.strConnString);
                                //if (objDTAtt.Rows.Count > 0)
                                //{
                                //    empAttCount = Convert.ToInt32(objDTAtt.Rows[0]["empAttCount"]);
                                //}

                                //MonthlyHamaliSalary Count
                                string strQryHamali = "select count(distinct(employeecd)) as empHamaliCount from T_MonthlyHamaliSalary where orgID =" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                DataTable objDTHamali = SqlHelper.ExecuteDataTable(strQryHamali, AppGlobal.strConnString);
                                if (objDTHamali.Rows.Count > 0)
                                {
                                    empHamaliCount = Convert.ToInt32(objDTHamali.Rows[0]["empHamaliCount"]);
                                }


                                //Employee In T_MonthlyHamaliSalary Not Found In Database
                                string strQryValidHamali = "";
                                strQryValidHamali = "select hamali.Employeecd from T_MonthlyHamaliSalary hamali where hamali.OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and hamali.MonYrcd = '" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and Employeecd  not in(select Employeecd from M_Emp emp where emp.OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and (emp.leaveDate is null or emp.leavedate>'" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "') and emp.isActive='Y' )";
                                DataTable objDTValidHamali = SqlHelper.ExecuteDataTable(strQryValidHamali, AppGlobal.strConnString);
                                if (objDTValidHamali.Rows.Count > 0)
                                {
                                    for (int i = 0; i < objDTValidHamali.Rows.Count; i++)
                                    {
                                        drH = dtHamali.NewRow();
                                        drH["Employeecd"] = objDTValidHamali.Rows[i]["Employeecd"].ToString();
                                        drH["Employeename"] = "";
                                        drH["Attendence"] = "Employee Not Found";
                                        dtHamali.Rows.Add(drH);
                                    }
                                    gvHamali.DataSource = dtHamali;
                                    gvHamali.DataBind();
                                    pnlGVList.Visible = true;
                                }


                                ////Attendance Not Found
                                string strQryValidAtt = "";
                                
                                //strQryValidAtt = "select emp.Employeename, hamali.employeecd from T_MonthlyHamaliSalary hamali inner join M_Emp emp on emp.OrgID = hamali.OrgID and emp.Employeecd = hamali.Employeecd";
                                //strQryValidAtt += " where hamali.orgID = " + Convert.ToInt16(Session["OrgID"]) + " and hamali.Employeecd not in (select Employeecd from T_Attendance att where att.orgID = " + Convert.ToInt16(Session["OrgID"]) + "  and att.MonYrCd = '" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' ) ";
                                //DataTable objDTValidAtt;
                                //objDTValidAtt = SqlHelper.ExecuteDataTable(strQryValidAtt, AppGlobal.strConnString);
                                //if (objDTValidAtt.Rows.Count > 0)
                                //{
                                //    for (int i = 0; i < objDTValidAtt.Rows.Count; i++)
                                //    {
                                //        dr = dtEmployee.NewRow();
                                //        dr["Employeecd"] = objDTValidAtt.Rows[i]["Employeecd"].ToString();
                                //        dr["Employeename"] = objDTValidAtt.Rows[i]["Employeename"].ToString();
                                //        dr["Attendence"] = "Attendence Not Found";
                                //        dtEmployee.Rows.Add(dr);
                                //    }
                                //    gvAttendence.DataSource = dtEmployee;
                                //    gvAttendence.DataBind();
                                //    pnlGVList.Visible = true;
                                //}


                                //Hamali Count > Than Employee Master
                                if (empHamaliCount > empMastCount)
                                {
                                    strQryHamali = "select hamai.Employeecd from T_MonthlyHamaliSalary hamai where hamai.OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and hamai.MonYrcd = '" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'and Employeecd  in(select Employeecd from M_Emp emp where emp.IsActive = 'N' and emp.OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and leaveDate is null or leavedate>'" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "')";
                                    objDTHamali = SqlHelper.ExecuteDataTable(strQryHamali, AppGlobal.strConnString);
                                    if (objDTHamali.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < objDTHamali.Rows.Count; i++)
                                        {
                                            drH = dtHamali.NewRow();
                                            drH["Employeecd"] = objDTHamali.Rows[i]["Employeecd"].ToString();
                                            drH["Employeename"] = "";
                                            drH["Attendence"] = "Employee Not Found";
                                            dtHamali.Rows.Add(drH);
                                        }
                                        gvHamali.DataSource = dtHamali;
                                        gvHamali.DataBind();
                                        pnlGVList.Visible = true;
                                        lblHamaliHeading.Text = "Employee Not Present In Database But Present In Hamali Salary List";
                                    }
                                }
                                

                                ////Salary Not Approved
                                string strQryValidSal = "";
                                //strQryValidSal = "select emp.Employeecd,emp.Employeename,emp.leavedate from M_Emp emp left join T_Attendance att on emp.Employeecd = att.employeecd and emp.orgID = att.orgID ";
                                //strQryValidSal += " where emp.orgID = " + Convert.ToInt16(Session["OrgID"]) + " and emp.IsActive = 'Y' and(att.monyrcd = '" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' or att.monyrcd is null) and (att.employeecd is null) and ";
                                //strQryValidSal += " (emp.leavedate is null or emp.leavedate > '" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "')";
                                //DataTable objDTValidSal = SqlHelper.ExecuteDataTable(strQryValidSal, AppGlobal.strConnString);
                                //if (objDTValidSal.Rows.Count > 0)
                                //{
                                    
                                //}

                                //strQryValidSal = " SELECT tbl.Employeecd, emp.Employeename, c.Employeecd as Salary FROM (SELECT OrgId, Employeecd, MAX(Docdate)AS DocDate FROM dbo.M_Salary";
                                //strQryValidSal += " WHERE(OrgId = " + Convert.ToInt16(Session["OrgID"]) + ") AND(Docdate <= '" + Convert.ToDateTime(dt) + "') AND(Approval = 'N') GROUP BY OrgId, Employeecd) AS tbl ";
                                //strQryValidSal += " INNER JOIN dbo.M_Salary AS c ON tbl.OrgId = c.OrgId AND tbl.Employeecd = c.Employeecd AND tbl.DocDate = c.Docdate ";
                                //strQryValidSal += " INNER JOIN dbo.M_Emp emp ON tbl.OrgId = emp.OrgId AND tbl.Employeecd = emp.Employeecd where tbl.orgID=" + Convert.ToInt16(Session["OrgID"]) + " and emp.IsActive='Y'";

                                strQryValidSal = " SELECT tbl.Employeecd, emp.Employeename, tbl.Employeecd as Salary FROM (SELECT OrgId, Employeecd, MAX(Docdate)AS DocDate FROM dbo.M_Salary";
                                strQryValidSal += " WHERE(OrgId = " + Convert.ToInt16(Session["OrgID"]) + ") AND(Docdate <= '" + Convert.ToDateTime(dt).ToString("dd MMM yyyy") + "') AND(Approval = 'N') GROUP BY OrgId, Employeecd) AS tbl ";
                                strQryValidSal += " INNER JOIN dbo.M_Emp emp ON tbl.OrgId = emp.OrgId AND tbl.Employeecd = emp.Employeecd where tbl.orgID=" + Convert.ToInt16(Session["OrgID"]) + " and emp.IsActive='Y'";

                                DataTable objDTValidSal = SqlHelper.ExecuteDataTable(strQryValidSal, AppGlobal.strConnString);
                                if (objDTValidSal.Rows.Count > 0)
                                {
                                    empSalNoApprovCount = Convert.ToInt16(objDTValidSal.Rows.Count);
                                    for (int i = 0; i < objDTValidSal.Rows.Count; i++)
                                    {
                                        drSal = dtSalary.NewRow();
                                        drSal["Employeecd"] = objDTValidSal.Rows[i]["Employeecd"].ToString();
                                        drSal["Employeename"] = objDTValidSal.Rows[i]["Employeename"].ToString();
                                        if (objDTValidSal.Rows[i]["Salary"].ToString() != "Y")
                                            drSal["Attendence"] = "Salaray Not Approved";

                                        dtSalary.Rows.Add(drSal);
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

                                //if (gvAttendence.Rows.Count > 0)
                                //{
                                //    strSalDet = "delete from T_MonthlyHamaliSalary where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                //    objSalDet = SqlHelper.ExecuteDataTable(strSalDet, AppGlobal.strConnString);

                                //    returnStr = "Hamali Salary Not Imported!!! Attendance Not Found But Deduction Present";
                                //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('" + returnStr + "'); ", true);
                                //    return;
                                //}
                                if (empSalNoApprovCount > 0)
                                {
                                    strSalDet = "delete from T_MonthlyHamaliSalary where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                    objSalDet = SqlHelper.ExecuteDataTable(strSalDet, AppGlobal.strConnString);

                                    returnStr = "Hamali Salary Not Imported!!! Salary Not Apporved Count = " + empSalNoApprovCount;
                                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('" + returnStr + "'); ", true);
                                    return;
                                }
                                if (empSalCount > 0)
                                {
                                    strSalDet = "delete from T_MonthlyHamaliSalary where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                    objSalDet = SqlHelper.ExecuteDataTable(strSalDet, AppGlobal.strConnString);

                                    returnStr = "Hamali Salary Not Imported!!! Salary Not Found Count = " + empSalCount;
                                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('" + returnStr + "'); ", true);
                                    return;
                                }
                                if (gvHamali.Rows.Count > 0 )
                                {
                                    strSalDet = "delete from T_MonthlyHamaliSalary where OrgId=" + Convert.ToInt16(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                                    objSalDet = SqlHelper.ExecuteDataTable(strSalDet, AppGlobal.strConnString);

                                    returnStr = "Hamali Salary Not Imported!!! No. Of Employees In Hamali Salary=" + gvHamali.Rows.Count + " are Not Found In Database";
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
                                    if (gvSalary.Rows.Count == 0 &&  gvHamali.Rows.Count == 0)
                                    {
                                        clearControls();
                                    }
                                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Hamali Salary Uploaded Successfully.'); ", true);
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

            //lblAttHeading.Text = "Attendence Not Found List";
            lblHamaliHeading.Text = "Employee Not Found List";

            //gvAttendence.DataSource = null;
            //gvAttendence.DataBind();
            gvSalary.DataSource = null;
            gvSalary.DataBind();

            gvHamali.DataSource = null;
            gvHamali.DataBind();
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
    }
}