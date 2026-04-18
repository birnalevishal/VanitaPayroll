using SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PayRoll.Transactions
{
    public partial class EmpAdvance : System.Web.UI.Page
    {
        SqlConnection sqlConn = null;
        SqlCommand sqlCmd = null;
        SqlTransaction sqlTrans = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
                clearControls();
            }
            txtEmpCode.Focus();
        }

        private void clearControls()
        {
            btnSave.Text = "Save";

            txtEmpCode.Text = "";
            txtEmpName.Text = "";
            txtDocDate.Text = "";
            ddlAdvType.SelectedIndex = 0;
            txtAdvAmt.Text = "";
            txtBalAmt.Text = "";
            txtDedAmt.Text = "";
            chkApproved.Checked = false;
            ddlMon.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;
            txtApproveDt.Text = "";

            string strQry = "select * from M_Authority where OrgId=" + Convert.ToInt32(Session["OrgID"]) + " and Employeecd='" + Session["UserName"].ToString() + "' and FormCode=132";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (objDT.Rows.Count > 0)
            {
                divApprove.Visible = true;
                divApproveDt.Visible = true;
            }
            else
            {
                if (Session["UserName"].ToString().ToLower() == "admin")
                {
                    divApprove.Visible = true;
                    divApproveDt.Visible = true;
                }
                else
                {
                    divApprove.Visible = false;
                    divApproveDt.Visible = false;
                }
            }


        }
        private void BindData()
        {
            string strQry = "SELECT * FROM  M_AdvanceType WHERE Active='Y' Order By AdvTypeId";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlAdvType.DataSource = objDT;
            ddlAdvType.DataTextField = "AdvName";
            ddlAdvType.DataValueField = "AdvTypeId";
            ddlAdvType.DataBind();
            ddlAdvType.Items.Insert(0, new ListItem("Select", "0"));

            strQry = "SELECT Year  FROM M_Year Where IsActive='Y' ORDER BY Year desc";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
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
                        string monthYear = "";

                        int month = Convert.ToDateTime(txtDocDate.Text).Month;
                        int year = Convert.ToDateTime(txtDocDate.Text).Year;
                        monthYear = year + month.ToString("00");

                        if (btnSave.Text == "Save")
                        {
                            InsertRecord();
                        }
                        else if (btnSave.Text == "Update")
                        {
                            UpdateRecord();
                            btnSave.Text = "Save";
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
            string strQry = "";
            bool result = false;
            try
            {
                int nID = SqlHelper.GetMaxID("T_EmpAdvanceHdr", "AdvId", AppGlobal.strConnString);

                strQry = @"INSERT INTO T_EmpAdvanceHdr(OrgId, Employeecd, AdvId, AdvTypeId, AdvDt, AdvAmount, AdvApproved, AdvBalance, DedAmount, DedMonYrcd, LastUpdatedBy, LastUpdatedDt) 
                                             VALUES(@OrgId, @Employeecd, @AdvId, @AdvTypeId, @AdvDt, @AdvAmount, @AdvApproved, @AdvBalance, @DedAmount, @DedMonYrcd, @LastUpdatedBy, @LastUpdatedDt)";

                SqlParameter[] para = new SqlParameter[13];
                para[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                para[1] = new SqlParameter("@Employeecd", txtEmpCode.Text);
                para[2] = new SqlParameter("@AdvId", nID);
                para[3] = new SqlParameter("@AdvTypeId", ddlAdvType.SelectedValue);
                para[4] = new SqlParameter("@AdvDt", Convert.ToDateTime(txtDocDate.Text).ToString("dd MMM yyyy"));
                para[5] = new SqlParameter("@AdvAmount", txtAdvAmt.Text != "" ? txtAdvAmt.Text : "0");
                para[6] = new SqlParameter("@AdvApproved", chkApproved.Checked);
                para[7] = new SqlParameter("@ApproveDt", txtApproveDt.Text.Trim() != "" ? Convert.ToDateTime(txtApproveDt.Text).ToString("dd MMM yyyy") : (object)DBNull.Value);
                para[8] = new SqlParameter("@AdvBalance", txtAdvAmt.Text != "" ? txtAdvAmt.Text : "0");
                para[9] = new SqlParameter("@DedAmount", txtDedAmt.Text != "" ? txtDedAmt.Text : "0");
                para[10] = new SqlParameter("@DedMonYrcd", ddlMon.SelectedValue + ddlYear.SelectedValue);
                para[11] = new SqlParameter("@LastUpdatedBy", Session["UserName"].ToString());
                para[12] = new SqlParameter("@LastUpdatedDt", DateTime.Now.ToString("dd MMM yyyy"));

                result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

                if (result)
                {
                    double AdvAmt = txtAdvAmt.Text != "" ? Convert.ToDouble(txtAdvAmt.Text) : 0;
                    double DedAmt = txtDedAmt.Text != "" ? Convert.ToDouble(txtDedAmt.Text) : 0;
                    double BalAmt = txtAdvAmt.Text != "" ? Convert.ToDouble(txtAdvAmt.Text) : 0;
                    string dedMnthYr = ddlMon.SelectedValue + ddlYear.SelectedValue;

                    if (DedAmt != 0)
                    {
                        int nSrNo = 1;

                        while (BalAmt > 0)
                        {
                            if (BalAmt >= DedAmt)
                            {
                                strQry = @"INSERT INTO T_EmpAdvanceDtl(OrgId, MonYrcd, Employeecd, AdvId, AdvPaySrNo, PayAmount, PayDt,  Paid) 
                                             VALUES(@OrgId, @MonYrcd, @Employeecd, @AdvId, @AdvPaySrNo, @PayAmount, @PayDt,  @Paid)";

                                SqlParameter[] para1 = new SqlParameter[8];
                                para1[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                                para1[1] = new SqlParameter("@MonYrcd", dedMnthYr);
                                para1[2] = new SqlParameter("@Employeecd", txtEmpCode.Text);
                                para1[3] = new SqlParameter("@AdvId", nID);
                                para1[4] = new SqlParameter("@AdvPaySrNo", nSrNo);
                                para1[5] = new SqlParameter("@PayAmount", DedAmt);
                                para1[6] = new SqlParameter("@PayDt", DBNull.Value);
                                para1[7] = new SqlParameter("@Paid", "N");

                                result = SqlHelper.ExecuteNonQuery(strQry, para1, AppGlobal.strConnString);
                            }
                            else
                            {
                                strQry = @"INSERT INTO T_EmpAdvanceDtl(OrgId, MonYrcd, Employeecd, AdvId, AdvPaySrNo, PayAmount, PayDt,  Paid) 
                                             VALUES(@OrgId, @MonYrcd, @Employeecd, @AdvId, @AdvPaySrNo, @PayAmount, @PayDt,  @Paid)";

                                SqlParameter[] para1 = new SqlParameter[8];
                                para1[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                                para1[1] = new SqlParameter("@MonYrcd", dedMnthYr);
                                para1[2] = new SqlParameter("@Employeecd", txtEmpCode.Text);
                                para1[3] = new SqlParameter("@AdvId", nID);
                                para1[4] = new SqlParameter("@AdvPaySrNo", nSrNo);
                                para1[5] = new SqlParameter("@PayAmount", BalAmt);
                                para1[6] = new SqlParameter("@PayDt", DBNull.Value);
                                para1[7] = new SqlParameter("@Paid", "N");

                                result = SqlHelper.ExecuteNonQuery(strQry, para1, AppGlobal.strConnString);
                            }
                            //Counters
                            nSrNo++;
                           
                            //Calcualte MnthYrcd
                            int Mnth = Convert.ToInt32(dedMnthYr.Substring(0, 2));
                            int Yr = Convert.ToInt32(dedMnthYr.Substring(2, 4));
                            Mnth++;
                            if (Mnth > 12)
                            {
                                Mnth = 1;
                                Yr = Yr + 1;
                            }
                            dedMnthYr = Mnth.ToString("00") + Yr.ToString();

                            //Balance Amount
                            BalAmt = BalAmt - DedAmt;
                        }

                       
                    }

                }

                if (result)
                {
                    strQry = "";
                    strQry = "INSERT INTO T_Log(OrgId,docdate, Employeecd, MenuId, Mode, Computername, amount) VALUES(@OrgId,@docdate, @Employeecd, @MenuId, @Mode, @Computername,@amount)";

                    SqlParameter[] paraLog = new SqlParameter[7];
                    paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                    paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                    paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                    paraLog[3] = new SqlParameter("@Mode", "A");
                    paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());
                    paraLog[5] = new SqlParameter("@amount", txtAdvAmt.Text != "" ? txtAdvAmt.Text : "0");
                    paraLog[6] = new SqlParameter("@docdate", Convert.ToDateTime(txtDocDate.Text).ToString("dd MMM yyyy"));

                    result = SqlHelper.ExecuteNonQuery(strQry, paraLog, AppGlobal.strConnString);
                    if (result)
                    {
                        txtEmpCodeSearch.Text = txtEmpCode.Text;
                        clearControls();
                        txtEmpCode_TextChanged(null, null);
                        BindGrid();
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
                    }
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
            }

        }

        private void UpdateRecord()
        {
            try
            {
                string strQry = "";
                bool result = false;
                int nID = Convert.ToInt32(ViewState["nID"]);

                strQry = @"UPDATE T_EmpAdvanceHdr SET AdvTypeId=@AdvTypeId, AdvDt=@AdvDt, AdvAmount=@AdvAmount, AdvApproved=@AdvApproved, ApproveDt=@ApproveDt, AdvBalance=@AdvBalance, 
                                                        DedAmount=@DedAmount, DedMonYrcd=@DedMonYrcd, LastUpdatedBy=@LastUpdatedBy, LastUpdatedDt=@LastUpdatedDt 
                                                WHERE OrgId=@OrgId AND Employeecd=@Employeecd AND AdvId=@AdvId";

                SqlParameter[] para = new SqlParameter[13];
                para[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                para[1] = new SqlParameter("@Employeecd", txtEmpCode.Text);
                para[2] = new SqlParameter("@AdvId", nID);
                para[3] = new SqlParameter("@AdvTypeId", ddlAdvType.SelectedValue);
                para[4] = new SqlParameter("@AdvDt", Convert.ToDateTime(txtDocDate.Text).ToString("dd MMM yyyy"));
                para[5] = new SqlParameter("@AdvAmount", txtAdvAmt.Text != "" ? txtAdvAmt.Text : "0");
                para[6] = new SqlParameter("@AdvApproved", chkApproved.Checked);
                para[7] = new SqlParameter("@ApproveDt", txtApproveDt.Text.Trim() != "" ? Convert.ToDateTime(txtApproveDt.Text).ToString("dd MMM yyyy") : (object)DBNull.Value);
                para[8] = new SqlParameter("@AdvBalance", txtAdvAmt.Text != "" ? txtAdvAmt.Text : "0");
                para[9] = new SqlParameter("@DedAmount", txtDedAmt.Text != "" ? txtDedAmt.Text : "0");
                para[10] = new SqlParameter("@DedMonYrcd", ddlMon.SelectedValue + ddlYear.SelectedValue);
                para[11] = new SqlParameter("@LastUpdatedBy", Session["UserName"].ToString());
                para[12] = new SqlParameter("@LastUpdatedDt", DateTime.Now.ToString("dd MMM yyyy"));

                result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

                if (result)
                {
                    strQry = @"DELETE FROM T_EmpAdvanceDtl WHERE OrgId=@OrgId AND Employeecd=@Employeecd AND AdvId=@AdvId";

                    SqlParameter[] paraDel = new SqlParameter[3];
                    paraDel[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                    paraDel[1] = new SqlParameter("@Employeecd", txtEmpCode.Text);
                    paraDel[2] = new SqlParameter("@AdvId", nID);
                    result = SqlHelper.ExecuteNonQuery(strQry, paraDel, AppGlobal.strConnString);

                    double AdvAmt = txtAdvAmt.Text != "" ? Convert.ToDouble(txtAdvAmt.Text) : 0;
                    double DedAmt = txtDedAmt.Text != "" ? Convert.ToDouble(txtDedAmt.Text) : 0;
                    double BalAmt = txtAdvAmt.Text != "" ? Convert.ToDouble(txtAdvAmt.Text) : 0;
                    string dedMnthYr = ddlMon.SelectedValue + ddlYear.SelectedValue;

                    if (DedAmt != 0)
                    {
                        int nSrNo = 1;

                        while (BalAmt > 0)
                        {
                            if (BalAmt >= DedAmt)
                            {
                                strQry = @"INSERT INTO T_EmpAdvanceDtl(OrgId, MonYrcd, Employeecd, AdvId, AdvPaySrNo, PayAmount, PayDt,  Paid) 
                                             VALUES(@OrgId, @MonYrcd, @Employeecd, @AdvId, @AdvPaySrNo, @PayAmount, @PayDt,  @Paid)";

                                SqlParameter[] para1 = new SqlParameter[8];
                                para1[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                                para1[1] = new SqlParameter("@MonYrcd", dedMnthYr);
                                para1[2] = new SqlParameter("@Employeecd", txtEmpCode.Text);
                                para1[3] = new SqlParameter("@AdvId", nID);
                                para1[4] = new SqlParameter("@AdvPaySrNo", nSrNo);
                                para1[5] = new SqlParameter("@PayAmount", DedAmt);
                                para1[6] = new SqlParameter("@PayDt", DBNull.Value);
                                para1[7] = new SqlParameter("@Paid", "N");

                                result = SqlHelper.ExecuteNonQuery(strQry, para1, AppGlobal.strConnString);
                            }
                            else
                            {
                                strQry = @"INSERT INTO T_EmpAdvanceDtl(OrgId, MonYrcd, Employeecd, AdvId, AdvPaySrNo, PayAmount, PayDt,  Paid) 
                                             VALUES(@OrgId, @MonYrcd, @Employeecd, @AdvId, @AdvPaySrNo, @PayAmount, @PayDt,  @Paid)";

                                SqlParameter[] para1 = new SqlParameter[8];
                                para1[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                                para1[1] = new SqlParameter("@MonYrcd", dedMnthYr);
                                para1[2] = new SqlParameter("@Employeecd", txtEmpCode.Text);
                                para1[3] = new SqlParameter("@AdvId", nID);
                                para1[4] = new SqlParameter("@AdvPaySrNo", nSrNo);
                                para1[5] = new SqlParameter("@PayAmount", BalAmt);
                                para1[6] = new SqlParameter("@PayDt", DBNull.Value);
                                para1[7] = new SqlParameter("@Paid", "N");

                                result = SqlHelper.ExecuteNonQuery(strQry, para1, AppGlobal.strConnString);
                            }
                            //Counters
                            nSrNo++;

                            //Calcualte MnthYrcd
                            int Mnth = Convert.ToInt32(dedMnthYr.Substring(0, 2));
                            int Yr = Convert.ToInt32(dedMnthYr.Substring(2, 4));
                            Mnth++;
                            if (Mnth > 12)
                            {
                                Mnth = 1;
                                Yr = Yr + 1;
                            }
                            dedMnthYr = Mnth.ToString("00") + Yr.ToString();

                            //Balance Amount
                            BalAmt = BalAmt - DedAmt;
                        }
                    }
                }


                if (result)
                {
                    strQry = "";
                    strQry = "INSERT INTO T_Log(OrgId, docdate, Employeecd, MenuId, Mode, Computername, amount) VALUES(@OrgId,@docdate, @Employeecd, @MenuId, @Mode, @Computername,@amount)";

                    SqlParameter[] paraLog = new SqlParameter[7];
                    paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                    paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                    paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                    paraLog[3] = new SqlParameter("@Mode", "U");
                    paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());
                    paraLog[5] = new SqlParameter("@amount", txtAdvAmt.Text != "" ? txtAdvAmt.Text : "0");
                    paraLog[6] = new SqlParameter("@docdate", Convert.ToDateTime(txtDocDate.Text).ToString("dd MMM yyyy"));

                    result = SqlHelper.ExecuteNonQuery(strQry, paraLog, AppGlobal.strConnString);
                    if (result)
                    {
                        clearControls();
                        BindGrid();
                        //Remove readonly attributes
                        txtEmpCode.Attributes.Remove("readonly");
                        ddlAdvType.Attributes.Remove("disabled");
                        txtDocDate.Attributes.Remove("disabled");
                        txtAdvAmt.Attributes.Remove("readonly");

                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
                    }
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Error!'); ", true);
            }
        }

        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Show")
                {
                    int i = Convert.ToInt32(e.CommandArgument);
                    ViewRecord(i);
                    btnSave.Text = "Update";
                    txtEmpCode.Focus();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ViewRecord(int nId)
        {
            string strQry = "SELECT * FROM T_EmpAdvanceHdr Where orgID=" + Convert.ToInt32(Session["OrgID"]) + " and AdvId=" + nId;
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (objDT.Rows.Count > 0)
            {
                txtEmpCode.Text = objDT.Rows[0]["Employeecd"].ToString();
                txtEmpCode_TextChanged(null, null);
                ViewState["nID"] = objDT.Rows[0]["AdvId"].ToString();
                ddlAdvType.SelectedValue = objDT.Rows[0]["AdvTypeId"].ToString();
                txtDocDate.Text = Convert.ToDateTime(objDT.Rows[0]["AdvDt"]).ToString("dd/MM/yyyy");
                txtAdvAmt.Text = objDT.Rows[0]["AdvAmount"] != DBNull.Value ? Convert.ToDouble(objDT.Rows[0]["AdvAmount"]).ToString("0.00") : "0";
                txtBalAmt.Text = objDT.Rows[0]["AdvBalance"] != DBNull.Value ? Convert.ToDouble(objDT.Rows[0]["AdvBalance"]).ToString("0.00") : "0";
                txtDedAmt.Text = objDT.Rows[0]["DedAmount"] != DBNull.Value ? Convert.ToDouble(objDT.Rows[0]["DedAmount"]).ToString("0.00") : "0";
                chkApproved.Checked = objDT.Rows[0]["AdvApproved"].ToString() == "1" ? true : false;
                txtApproveDt.Text = objDT.Rows[0]["ApproveDt"].ToString() != "" ? Convert.ToDateTime(objDT.Rows[0]["ApproveDt"]).ToString("dd/MM/yyyy") : "";
                ddlMon.SelectedValue = objDT.Rows[0]["DedMonYrcd"].ToString().Substring(0, 2);
                ddlYear.SelectedValue = objDT.Rows[0]["DedMonYrcd"].ToString().Substring(2, 4);

                if (chkApproved.Checked)
                {
                    txtEmpCode.Attributes.Add("readonly", "readonly");
                    ddlAdvType.Attributes.Add("disabled", "disabled");
                    txtDocDate.Attributes.Add("disabled", "disabled");
                    txtAdvAmt.Attributes.Add("readonly", "readonly");
                }
            }
        }

        protected bool formValidation()
        {
            if (btnSave.Text == "Save")
            {
                string strQry = "SELECT Count(*) FROM M_AllowanceConfig Where OrgId=" + Convert.ToInt32(Session["OrgID"]) + " and Employeecd=" + txtEmpCode.Text + " and Docdate='" + Convert.ToDateTime(txtDocDate.Text.Trim()).ToString("dd MMM yyyy") + "'";
                int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
                if (nCnt > 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Record Already Exists!'); ", true);
                    //clearControls();
                    return false;
                }
            }
            return true;
        }

        protected void txtEmpCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int nCnt = 0;
                string strQry = "";

                if (txtEmpCode.Text != "")
                {
                    strQry = "SELECT Employeecd,Employeename FROM M_Emp Where OrgId='" + Convert.ToInt32(Session["OrgID"]) + "' and Employeecd='" + txtEmpCode.Text + "'";
                    DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                    if (objDT1.Rows.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Wrong Employee Code'); ", true);
                        clearControls();
                        return;
                    }
                    txtEmpName.Text = objDT1.Rows[0]["EmployeeName"].ToString();
                }

                if (txtEmpCodeSearch.Text != "")
                {
                    strQry = "SELECT Employeecd,Employeename FROM M_Emp Where OrgId='" + Convert.ToInt32(Session["OrgID"]) + "' and Employeecd='" + txtEmpCodeSearch.Text + "'";
                    DataTable objDT1 = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                    if (objDT1.Rows.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Wrong Employee Code'); ", true);
                        clearControls();
                        return;
                    }
                    txtEmpNameSearch.Text = objDT1.Rows[0]["EmployeeName"].ToString();
                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
        private void BindGrid()
        {
            string strQry = @"SELECT  Hdr.OrgId, Hdr.Employeecd, emp.Employeename, Hdr.AdvId, Hdr.AdvTypeId, AdvType.AdvName, Hdr.AdvDt, Hdr.AdvAmount, CASE WHEN Hdr.AdvApproved=1 THEN 'Y' ELSE 'N' END As AdvApproved, Hdr.ApproveDt, Hdr.AdvBalance, Hdr.DedAmount, Hdr.DedMonYrcd
                              FROM  dbo.T_EmpAdvanceHdr AS Hdr LEFT OUTER JOIN dbo.M_Emp AS emp ON Hdr.Employeecd = emp.Employeecd AND Hdr.OrgId = emp.OrgId LEFT OUTER JOIN
                              dbo.M_AdvanceType AS AdvType ON Hdr.AdvTypeId = AdvType.AdvTypeId where Hdr.Employeecd ='" + txtEmpCodeSearch.Text + "' and Hdr.OrgId=" + Convert.ToInt32(Session["OrgID"]) + " order by AdvDt desc";

            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvList.DataSource = objDT;
            gvList.DataBind();

            ViewState["objDTList"] = objDT;
        }
    }
}