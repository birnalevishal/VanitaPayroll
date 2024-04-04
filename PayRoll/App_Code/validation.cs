using System;
using System.Data;
using SqlClient;

namespace PayRoll.App_Code
{
    public class validation
    {
        public static string validateAttendence(int orgID, string monYrCd)
        {
            int empMastCount = 0;
            int empAttCount = 0;
            int empSalCount = 0;
            string returnStr = "";
            string dt = "01/" + monYrCd.Substring(0, 2) + "/" + monYrCd.Substring(2, 4);
            
            string strQry = "select count(distinct(employeecd)) as EmpMastCount from M_Emp where (leaveDate is null or leavedate<'" + Convert.ToDateTime(dt) + "') and orgID=" + orgID  + " and isActive='Y'";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (objDT.Rows.Count > 0)
            {
                empMastCount = Convert.ToInt32(objDT.Rows[0]["EmpMastCount"]);
            }

            strQry = "select count(distinct(employeecd)) as empAttCount from T_Attendance where orgID =" + orgID + " and MonYrcd='" + monYrCd + "'";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (objDT.Rows.Count > 0)
            {
                empAttCount = Convert.ToInt32(objDT.Rows[0]["empAttCount"]);
            }

            if(empMastCount!=empAttCount)
            {
                returnStr = "No. Of Employees In Organisation=" + empMastCount + " and No Of Employees In Attendence Excel=" + empAttCount;
                return returnStr;
            }

            strQry = " SELECT count(distinct(c.Employeecd)) as empSalCount FROM(SELECT OrgId, Employeecd, MAX(Docdate) AS DocDate FROM M_Salary WHERE(OrgId =" + orgID + ") AND(Docdate <= '" + Convert.ToDateTime(dt) + "') and IsActive = 'Y' and Approval = 'Y' GROUP BY OrgId, Employeecd) AS tbl";
            strQry +=" INNER JOIN M_Salary AS c ON tbl.OrgId = c.OrgId AND tbl.Employeecd = c.Employeecd AND tbl.DocDate = c.Docdate";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (objDT.Rows.Count > 0)
            {
                empSalCount = Convert.ToInt32(objDT.Rows[0]["empSalCount"]);
            }

            if (empMastCount != empSalCount)
            {
                returnStr = "No. Of Employees In Organisation=" + empMastCount + " and No Of Salary Approved Employees =" + empSalCount;
                return returnStr;
            }

            returnStr = "1Attendence Uploaded Successfully";
            return returnStr;
        }

        public static string validateDeduction(int orgID, string monYrCd)
        {
            int empMastCount = 0;
            int empDedCount = 0;
            int empSalCount = 0;
            string returnStr = "";
            string dt = "01/" + monYrCd.Substring(0, 2) + "/" + monYrCd.Substring(2, 4);

            string strQry = "select count(distinct(employeecd)) as EmpMastCount from M_Emp where (leaveDate is null or leavedate<'" + Convert.ToDateTime(dt) + "') and orgID=" + orgID + " and isActive='Y'";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (objDT.Rows.Count > 0)
            {
                empMastCount = Convert.ToInt32(objDT.Rows[0]["EmpMastCount"]);
            }

            strQry = "select count(distinct(employeecd)) as empDedCount from T_MonthlyDeduction where orgID =" + orgID + " and MonYrcd='" + monYrCd + "'";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (objDT.Rows.Count > 0)
            {
                empDedCount = Convert.ToInt32(objDT.Rows[0]["empDedCount"]);
            }

            if (empMastCount != empDedCount)
            {
                returnStr = "No. Of Employees In Organisation=" + empMastCount + " and No Of Employees In Deduction Excel=" + empDedCount;
                return returnStr;
            }

            //strQry = " SELECT count(distinct(c.Employeecd)) as empSalCount FROM(SELECT OrgId, Employeecd, MAX(Docdate) AS DocDate FROM M_Salary WHERE(OrgId =" + orgID + ") AND(Docdate <= '" + Convert.ToDateTime(dt) + "') and IsActive = 'Y' and Approval = 'Y' GROUP BY OrgId, Employeecd) AS tbl";
            //strQry += " INNER JOIN M_Salary AS c ON tbl.OrgId = c.OrgId AND tbl.Employeecd = c.Employeecd AND tbl.DocDate = c.Docdate";
            //objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            //if (objDT.Rows.Count > 0)
            //{
            //    empSalCount = Convert.ToInt32(objDT.Rows[0]["empSalCount"]);
            //}

            //if (empMastCount != empSalCount)
            //{
            //    returnStr = "No. Of Employees In Organisation=" + empMastCount + " and No Of Salary Approved Employees =" + empSalCount;
            //    return returnStr;
            //}

            returnStr = "1Deduction Uploaded Successfully";
            return returnStr;
        }

        public static string validateEarning(int orgID, string monYrCd)
        {
            int empMastCount = 0;
            int empEarningCount = 0;
            int empSalCount = 0;
            string returnStr = "";
            string dt = "01/" + monYrCd.Substring(0, 2) + "/" + monYrCd.Substring(2, 4);

            string strQry = "select count(distinct(employeecd)) as EmpMastCount from M_Emp where (leaveDate is null or leavedate<'" + Convert.ToDateTime(dt) + "') and orgID=" + orgID + " and isActive='Y'";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (objDT.Rows.Count > 0)
            {
                empMastCount = Convert.ToInt32(objDT.Rows[0]["EmpMastCount"]);
            }

            strQry = "select count(distinct(employeecd)) as empEarningCount from T_MonthlyEarning where orgID =" + orgID + " and MonYrcd='" + monYrCd + "'";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (objDT.Rows.Count > 0)
            {
                empEarningCount = Convert.ToInt32(objDT.Rows[0]["empEarningCount"]);
            }

            if (empMastCount != empEarningCount)
            {
                returnStr = "No. Of Employees In Organisation=" + empMastCount + " and No Of Employees In Deduction Excel=" + empEarningCount;
                return returnStr;
            }

            //strQry = " SELECT count(distinct(c.Employeecd)) as empSalCount FROM(SELECT OrgId, Employeecd, MAX(Docdate) AS DocDate FROM M_Salary WHERE(OrgId =" + orgID + ") AND(Docdate <= '" + Convert.ToDateTime(dt) + "') and IsActive = 'Y' and Approval = 'Y' GROUP BY OrgId, Employeecd) AS tbl";
            //strQry += " INNER JOIN M_Salary AS c ON tbl.OrgId = c.OrgId AND tbl.Employeecd = c.Employeecd AND tbl.DocDate = c.Docdate";
            //objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            //if (objDT.Rows.Count > 0)
            //{
            //    empSalCount = Convert.ToInt32(objDT.Rows[0]["empSalCount"]);
            //}

            //if (empMastCount != empSalCount)
            //{
            //    returnStr = "No. Of Employees In Organisation=" + empMastCount + " and No Of Salary Approved Employees =" + empSalCount;
            //    return returnStr;
            //}

            returnStr = "1Earning Uploaded Successfully";
            return returnStr;
        }
    }
}