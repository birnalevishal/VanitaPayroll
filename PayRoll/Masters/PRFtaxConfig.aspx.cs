using SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlTypes;

namespace PayRoll.Masters
{
    public partial class PRFtaxConfig : System.Web.UI.Page
    {
        SqlConnection sqlConn = null;
        SqlCommand sqlCmd = null;
        SqlTransaction sqlTrans = null;

        public string monthYear = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
                //BindGrid();
                SetInitialRow();
            }
        }

        private void BindData()
        {
            string strQry = "";
            DataTable objDT;

            strQry = "SELECT Year FROM M_Year ORDER BY Year desc";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlYear.DataSource = objDT;
            ddlYear.DataTextField = "Year";
            ddlYear.DataValueField = "Year";
            ddlYear.DataBind();

            strQry = "SELECT State,StateCd FROM M_State ORDER BY State";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlState.DataSource = objDT;
            ddlState.DataTextField = "State";
            ddlState.DataValueField = "StateCd";
            ddlState.DataBind();

            strQry = "SELECT Gender,Gendercd FROM M_Gender ORDER BY Gender";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlGender.DataSource = objDT;
            ddlGender.DataTextField = "Gender";
            ddlGender.DataValueField = "Gendercd";
            ddlGender.DataBind();
        }
        private void BindGrid()
        {
            string strQry = "SELECT *,DATENAME(MONTH, DateAdd( month , CONVERT(INT,LEFT(MonYrcd,2)) , -1 ) -1 ) as monthname,right(MonYrcd,4) as year FROM M_PFConfiguration";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            Gridview1.DataSource = null;
            Gridview1.DataBind();

            ViewState["objDTList"] = objDT;
        }
        private void GetData()
        {
            if (ddlMon.SelectedIndex != 0 && ddlYear.SelectedIndex != 0)
            {
                string strQry = "SELECT * FROM M_ProfTaxConfiguration where OrgID=" + Convert.ToInt32(Session["OrgID"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    //frmmount.Text = objDT.Rows[0]["FrAmount"].ToString();
                    //toamount.Text = objDT.Rows[0]["ToAmount"].ToString();
                    //amount.Text = objDT.Rows[0]["TaxAmount"].ToString();

                }
            }
        }
        private void SetInitialRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("FrAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("ToAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("TaxAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("Mon", typeof(string)));
            dt.Columns.Add(new DataColumn("MonAmount", typeof(string)));
            dr = dt.NewRow();
            dr["SrNo"] = 1;
            dr["FrAmount"] = string.Empty;
            dr["ToAmount"] = string.Empty;
            dr["TaxAmount"] = string.Empty;
            dr["Mon"] = "00";
            dr["MonAmount"] = string.Empty;

            dt.Rows.Add(dr);
            //dr = dt.NewRow();
            //Store the DataTable in ViewState

            ViewState["CurrentTable"] = dt;
            Gridview1.DataSource = dt;
            Gridview1.DataBind();

        }

        private void AddNewRowToGrid(string strAction = "")
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        TextBox box1 = (TextBox)Gridview1.Rows[rowIndex].Cells[1].FindControl("TextBox1");
                        TextBox box2 = (TextBox)Gridview1.Rows[rowIndex].Cells[2].FindControl("TextBox2");
                        TextBox box3 = (TextBox)Gridview1.Rows[rowIndex].Cells[3].FindControl("TextBox3");
                        DropDownList ddlmonth = (DropDownList)Gridview1.Rows[rowIndex].Cells[4].FindControl("Month");
                        TextBox box4 = (TextBox)Gridview1.Rows[rowIndex].Cells[5].FindControl("TextBox4");

                        if (strAction == "")
                        {
                            if (box1.Text == "" || box2.Text == "" || box3.Text == "")
                            {
                                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Add All Values!'); ", true);
                                return;
                            }
                            drCurrentRow = dtCurrentTable.NewRow();
                            drCurrentRow["SrNo"] = i + 1;
                            drCurrentRow["Mon"] = "00";
                        }
                        dtCurrentTable.Rows[i - 1]["FrAmount"] = box1.Text;
                        dtCurrentTable.Rows[i - 1]["ToAmount"] = box2.Text;
                        dtCurrentTable.Rows[i - 1]["TaxAmount"] = box3.Text;
                        dtCurrentTable.Rows[i - 1]["Mon"] = ddlmonth.SelectedValue;
                        dtCurrentTable.Rows[i - 1]["MonAmount"] = box4.Text;

                        rowIndex++;
                    }

                    if (strAction == "")
                    {
                        dtCurrentTable.Rows.Add(drCurrentRow);
                    }
                    ViewState["CurrentTable"] = dtCurrentTable;
                    Gridview1.DataSource = dtCurrentTable;
                    Gridview1.DataBind();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }
            SetPreviousData();
            //Set Previous Data on Postbacks
            
        }
        private void SetPreviousData()
        {
            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TextBox box1 = (TextBox)Gridview1.Rows[rowIndex].Cells[1].FindControl("TextBox1");
                        TextBox box2 = (TextBox)Gridview1.Rows[rowIndex].Cells[2].FindControl("TextBox2");
                        TextBox box3 = (TextBox)Gridview1.Rows[rowIndex].Cells[3].FindControl("TextBox3");
                        DropDownList ddlmonth = (DropDownList)Gridview1.Rows[rowIndex].Cells[4].FindControl("Month");
                        TextBox box4 = (TextBox)Gridview1.Rows[rowIndex].Cells[5].FindControl("TextBox4");

                        box1.Text = dt.Rows[i]["FrAmount"].ToString();
                        box2.Text = dt.Rows[i]["ToAmount"].ToString();
                        box3.Text = dt.Rows[i]["TaxAmount"].ToString();
                        ddlmonth.SelectedValue =Convert.ToInt16(dt.Rows[i]["Mon"]).ToString("00");
                        box4.Text = dt.Rows[i]["MonAmount"].ToString();
                        rowIndex++;
                    }
                }
            }
        }
        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            AddNewRowToGrid();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    if (btnSave.Text == "Save")
                    {
                        AddNewRowToGrid("Save");
                        InsertRecord();
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

        private void yearname()
        {
            monthYear = ddlMon.SelectedValue + "" + ddlYear.SelectedValue;
        }
        private void InsertRecord()
        {
            bool result = false;

            try
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                string strQry = "";
                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);

                strQry = "select * from T_SalaryLock where OrgId=" + Convert.ToInt16(Session["orgID"]) + " and RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)>='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "' and Lock='Y'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    clearControls();
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Salary Already Processed, Cant Modify Now.'); ", true);
                    return;
                }

                strQry = @"DELETE FROM M_ProfTaxConfiguration WHERE OrgId=@OrgId AND  MonYrcd=@MonYrcd AND Statecd=@Statecd AND Gendercd=@Gendercd ";
                SqlParameter[] para1 = new SqlParameter[4];
                para1[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
                para1[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + "" + ddlYear.SelectedValue);
                para1[2] = new SqlParameter("@Statecd", ddlState.SelectedValue);
                para1[3] = new SqlParameter("@Gendercd", ddlGender.SelectedValue);
                //para1[4] = new SqlParameter("@Srno", ddlGender.SelectedValue);

                result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, para1);


                foreach (DataRow item in dtCurrentTable.Rows)
                {
                    
                    strQry = @"INSERT INTO M_ProfTaxConfiguration(OrgId, MonYrcd, Statecd, Gendercd, Srno, FrAmount, ToAmount, TaxAmount,Mon,MonAmount) 
                                        VALUES(@OrgId, @MonYrcd, @Statecd, @Gendercd, @Srno, @FrAmount, @ToAmount, @TaxAmount,@Mon,@MonAmount)";

                    SqlParameter[] para = new SqlParameter[10];
                    para[0] = new SqlParameter("@OrgId", Session["OrgID"].ToString());
                    para[1] = new SqlParameter("@MonYrcd", ddlMon.SelectedValue + "" + ddlYear.SelectedValue);
                    para[2] = new SqlParameter("@Statecd", ddlState.SelectedValue);
                    para[3] = new SqlParameter("@Gendercd", ddlGender.SelectedValue);
                    para[4] = new SqlParameter("@Srno", item["SrNo"].ToString());
                    para[5] = new SqlParameter("@FrAmount", item["FrAmount"].ToString());
                    para[6] = new SqlParameter("@ToAmount", item["ToAmount"].ToString());
                    para[7] = new SqlParameter("@TaxAmount", item["TaxAmount"].ToString());
                    para[8] = new SqlParameter("@Mon", item["Mon"].ToString());
                    para[9] = new SqlParameter("@MonAmount", item["MonAmount"].ToString());

                    result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, para);
                }

                if (result)
                {
                    sqlTrans.Commit();
                    clearControls();
                    BindGrid();
                    strQry = "";
                    strQry = "INSERT INTO T_Log(OrgId,MonthYrcd, Employeecd, MenuId, Mode, Computername) VALUES(@OrgId,@MonthYrcd, @Employeecd, @MenuId, @Mode, @Computername)";

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
                        clearControls();
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
        
        private void clearControls()
        {
            ddlMon.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;
            ddlState.SelectedIndex = 0;
            ddlGender.SelectedIndex = 0;
            SetInitialRow();
            btnSave.Text = "Save";
        }

        private void ViewRecord(int i)
        {
            DataTable objDT = (DataTable)ViewState["objDTList"];

            ViewState["ID"] = objDT.Rows[i]["OrgId"].ToString();
            string m, y;
            m = objDT.Rows[i]["MonYrcd"].ToString().Substring(0, 2);
            y = objDT.Rows[i]["MonYrcd"].ToString().Substring(2, 4);

            ddlMon.SelectedValue = m;
            ddlYear.SelectedValue = y;

            // frmmount.Text = objDT.Rows[i]["FrAmount"].ToString();
            // toamount.Text = objDT.Rows[i]["ToAmount"].ToString();
            //amount.Text = objDT.Rows[i]["TaxAmount"].ToString();

        }

        protected bool formValidation()
        {
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

            string strQry = "SELECT Count(*) FROM M_PFConfiguration Where OrgId=" + Convert.ToInt32(Session["OrgID"]) + " and MonYrcd ='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "'";
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Organization is Already Exists!'); ", true);
                clearControls();
                return false;
            }
            return true;
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            yearname();
            // GetData();
            ddlMon.Focus();
            ViewRecordN();
        }

        protected void ddlMon_SelectedIndexChanged(object sender, EventArgs e)
        {
            yearname();
            //GetData();
            // PF.Focus();
            ViewRecordN();
        }

        protected void ddlGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewRecordN();
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewRecordN();
        }

        private void ViewRecordN()
        {
            int rowIndex = 0;
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;

            //string strQry = "select Srno,FrAmount,ToAmount, TaxAmount,Mon,MonAmount from M_ProfTaxConfiguration where OrgId=" + Convert.ToInt32(Session["OrgId"]) + " and MonYrcd='" + ddlMon.SelectedValue + ddlYear.SelectedValue + "' and Statecd=" + Convert.ToInt32(ddlState.SelectedValue) + " and Gendercd=" + Convert.ToInt32(ddlGender.SelectedValue);
            string strQry = "select Srno,FrAmount,ToAmount, TaxAmount,Mon,MonAmount from M_ProfTaxConfiguration WHERE (RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2) = (SELECT  max(RIGHT(MonYrcd, 4) + LEFT(MonYrcd, 2)) AS Expr1 FROM dbo.M_ProfTaxConfiguration WHERE(RIGHT(MonYrcd, 4) +LEFT(MonYrcd, 2) <='" + ddlYear.SelectedValue + ddlMon.SelectedValue + "') AND(OrgId =" + Convert.ToInt32(Session["OrgID"]) + ") and statecd= " + Convert.ToInt32(ddlState.SelectedValue) + " and gendercd=" + Convert.ToInt32(ddlGender.SelectedValue) + " )) AND(OrgId = " + Convert.ToInt32(Session["OrgID"]) + ") and statecd= " + Convert.ToInt32(ddlState.SelectedValue) + " and gendercd=" + Convert.ToInt32(ddlGender.SelectedValue);
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (objDT.Rows.Count > 0)
            {

                //TextBox box1;
                //TextBox box2;
                //TextBox box3;
                //DropDownList ddlmonth;
                //TextBox box4;
                //for (int i = 0; i < objDT.Rows.Count; i++)
                //{
                //    box1 = (TextBox)Gridview1.Rows[rowIndex].Cells[1].FindControl("TextBox1");
                //    box2 = (TextBox)Gridview1.Rows[rowIndex].Cells[2].FindControl("TextBox2");
                //    box3 = (TextBox)Gridview1.Rows[rowIndex].Cells[3].FindControl("TextBox3");
                //    ddlmonth = (DropDownList)Gridview1.Rows[rowIndex].Cells[4].FindControl("Month");
                //    box4 = (TextBox)Gridview1.Rows[rowIndex].Cells[5].FindControl("TextBox4");


                //    box1.Text = objDT.Rows[i]["FrAmount"].ToString();
                //    box2.Text = objDT.Rows[i]["ToAmount"].ToString();
                //    box3.Text = objDT.Rows[i]["TaxAmount"].ToString();
                //    ddlmonth.SelectedValue = objDT.Rows[i]["Mon"].ToString();
                //    box4.Text = objDT.Rows[i]["MonAmount"].ToString();

                //    rowIndex++;
                //    Gridview1.DataSource = objDT;
                //    Gridview1.DataBind();

                //}
                ViewState["CurrentTable"] = objDT;
                Gridview1.DataSource = objDT;
                Gridview1.DataBind();

            }
            else
            {
                SetInitialRow();
                //Gridview1.DataSource = (DataTable)ViewState["CurrentTable"];
                //Gridview1.DataBind();
            }
        }

        //protected void lbtnRemove_Click(object sender, EventArgs e)
        //{
        //    LinkButton lnkbtn = sender as LinkButton;
        //    GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
        //    ((DataTable)ViewState["CurrentTable"]).Rows.RemoveAt(gvrow.RowIndex);

        //    DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
        //    dt.Columns.Add(new DataColumn("FrAmount", typeof(string)));
        //    dt.Columns.Add(new DataColumn("ToAmount", typeof(string)));
        //    dt.Columns.Add(new DataColumn("TaxAmount", typeof(string)));
        //    dt.Columns.Add(new DataColumn("Mon", typeof(string)));
        //    dt.Columns.Add(new DataColumn("MonAmount", typeof(string)));
            
        //    int rowIndex = 0;
        //    DataRow drCurrentRow = null;
        //    string strAction = "";
        //    //for (int i = ((DataTable)ViewState["CurrentTable"]).Rows.Count - 1; i >= 0; i--)
        //    //{
        //    //    DataRow dr = ((DataTable)ViewState["CurrentTable"]).Rows[i];
        //    //    if ((int)dr["SrNo"] == gvrow.RowIndex)
        //    //        dr.Delete();
        //    //}

        //    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
        //    {
                
        //        TextBox FrAmount = (TextBox)Gridview1.Rows[rowIndex].Cells[2].FindControl("FrAmount");
        //        TextBox ToAmount = (TextBox)Gridview1.Rows[rowIndex].Cells[3].FindControl("ToAmount");
        //        TextBox TaxAmount = (TextBox)Gridview1.Rows[rowIndex].Cells[3].FindControl("TaxAmount");
        //        DropDownList Mon = (DropDownList)Gridview1.Rows[rowIndex].Cells[4].FindControl("Mon");
        //        TextBox MonAmount = (TextBox)Gridview1.Rows[rowIndex].Cells[5].FindControl("MonAmount");
               
        //        drCurrentRow = dt.NewRow();
        //        drCurrentRow["SrNo"] = i;
        //        drCurrentRow["FrAmount"] = FrAmount.Text;
        //        drCurrentRow["ToAmount"] = ToAmount.Text;
        //        drCurrentRow["Mon"] = ddlMon.SelectedValue;
        //        drCurrentRow["MonAmount"] = MonAmount.Text;
              
        //        dt.Rows.Add(drCurrentRow);
        //        rowIndex++;
        //    }

        //    ViewState["CurrentTable"] = dt;
        //    Gridview1.DataSource = dt;
        //    Gridview1.DataBind();
        //}
    }
}