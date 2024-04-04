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
    public partial class AssetTransanction : System.Web.UI.Page
    {
        SqlConnection sqlConn = null;
        SqlCommand sqlCmd = null;
        SqlTransaction sqlTrans = null;

        public string monthYear = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                clearControls();
                SetInitialRow();
                
            }
        }
        private void BindData()
        {
            string strQry = "SELECT Employeename,Employeecd FROM M_Emp where OrgID=" + Convert.ToInt16(Session["OrgID"]) + " ORDER BY Employeename";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlEmpName.DataSource = objDT;
            ddlEmpName.DataTextField = "Employeename";
            ddlEmpName.DataValueField = "Employeecd";
            ddlEmpName.DataBind();
            ddlEmpName.Items.Insert(0, "Select");

        }


        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlItem = (e.Row.FindControl("ddlItem") as DropDownList);

                string strQry = "SELECT Item, ItemCd FROM M_Item Where IsActive='Y' ORDER BY Item";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                ddlItem.DataSource = objDT;
                ddlItem.DataTextField = "Item";
                ddlItem.DataValueField = "ItemCd";
                ddlItem.DataBind();

                string selectedCity = DataBinder.Eval(e.Row.DataItem, "Item").ToString();
                ddlItem.Items.FindByValue(selectedCity).Selected = true;
            }
        }

        private void SetInitialRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("Item", typeof(string)));
            dt.Columns.Add(new DataColumn("MaterialDetails", typeof(string)));
            dt.Columns.Add(new DataColumn("Quantity", typeof(string)));
            dt.Columns.Add(new DataColumn("IssueDate", typeof(string)));
            dt.Columns.Add(new DataColumn("Amount", typeof(string)));
            dt.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt.Columns.Add(new DataColumn("HandOverDate", typeof(string)));
            
            dr = dt.NewRow();

            dr["SrNo"] = 1;
            dr["Item"] = "1";
            dr["MaterialDetails"] = string.Empty;
            dr["Quantity"] = string.Empty;
            dr["IssueDate"] = string.Empty;
            dr["Amount"] = "0";
            dr["Remark"] = string.Empty;
            dr["HandOverDate"] = string.Empty;

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
                    foreach (System.Data.DataColumn col in dtCurrentTable.Columns) col.ReadOnly = false;
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        DropDownList ddlItem = (DropDownList)Gridview1.Rows[rowIndex].Cells[1].FindControl("ddlItem");
                        Label lblItem = (Label)Gridview1.Rows[rowIndex].Cells[1].FindControl("lblItem");
                        TextBox txtDescription = (TextBox)Gridview1.Rows[rowIndex].Cells[2].FindControl("txtDescription");
                        TextBox txtQuantity = (TextBox)Gridview1.Rows[rowIndex].Cells[3].FindControl("txtQuantity");
                        TextBox txtAmount = (TextBox)Gridview1.Rows[rowIndex].Cells[4].FindControl("txtAmount");
                        TextBox txtRemark = (TextBox)Gridview1.Rows[rowIndex].Cells[5].FindControl("txtRemark");
                        TextBox txtIssueDate = (TextBox)Gridview1.Rows[rowIndex].Cells[6].FindControl("txtIssueDate");
                        TextBox txtHandOverDt = (TextBox)Gridview1.Rows[rowIndex].Cells[7].FindControl("txtHandOverDate");

                        if (strAction == "")
                        {
                            if (txtDescription.Text == "" || txtQuantity.Text == "" || txtIssueDate.Text == "")
                            {
                                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Add All Values!'); ", true);
                                return;
                            }
                            drCurrentRow = dtCurrentTable.NewRow();
                            drCurrentRow["SrNo"] = i + 1;
                            drCurrentRow["Item"] = "1";
                           
                        }
                        dtCurrentTable.Rows[i - 1]["Item"] = ddlItem.SelectedValue;
                        dtCurrentTable.Rows[i - 1]["Remark"] = txtRemark.Text;
                        dtCurrentTable.Rows[i - 1]["MaterialDetails"] = txtDescription.Text;
                        dtCurrentTable.Rows[i - 1]["Quantity"] = txtQuantity.Text;
                        if(txtIssueDate.Text!="")
                            dtCurrentTable.Rows[i - 1]["IssueDate"] = txtIssueDate.Text;
                        else
                            dtCurrentTable.Rows[i - 1]["IssueDate"] = (object)DBNull.Value;

                        dtCurrentTable.Rows[i - 1]["Amount"] = txtAmount.Text;
                        if(txtHandOverDt.Text!="")
                            dtCurrentTable.Rows[i - 1]["HandOverDate"] = txtHandOverDt.Text;
                        else
                            dtCurrentTable.Rows[i - 1]["HandOverDate"] = (object)DBNull.Value;

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
                        DropDownList ddlItem = (DropDownList)Gridview1.Rows[rowIndex].Cells[1].FindControl("ddlItem");
                        Label lblItem = (Label)Gridview1.Rows[rowIndex].Cells[1].FindControl("lblItem");
                        TextBox txtDescription = (TextBox)Gridview1.Rows[rowIndex].Cells[2].FindControl("txtDescription");
                        TextBox txtQuantity = (TextBox)Gridview1.Rows[rowIndex].Cells[3].FindControl("txtQuantity");
                        TextBox txtAmount = (TextBox)Gridview1.Rows[rowIndex].Cells[4].FindControl("txtAmount");
                        TextBox txtRemark = (TextBox)Gridview1.Rows[rowIndex].Cells[5].FindControl("txtRemark");
                        TextBox txtIssueDate = (TextBox)Gridview1.Rows[rowIndex].Cells[6].FindControl("txtIssueDate");
                        TextBox txtHandOverDt = (TextBox)Gridview1.Rows[rowIndex].Cells[7].FindControl("txtHandOverDate");

                        ddlItem.SelectedValue = dt.Rows[i]["Item"].ToString();
                        txtDescription.Text = dt.Rows[i]["MaterialDetails"].ToString();
                        txtQuantity.Text = dt.Rows[i]["Quantity"].ToString();
                        if (dt.Rows[i]["IssueDate"] != DBNull.Value)
                            txtIssueDate.Text = dt.Rows[i]["IssueDate"].ToString();
                       
                        txtAmount.Text = dt.Rows[i]["Amount"].ToString();
                        txtRemark.Text = dt.Rows[i]["Remark"].ToString();
                        if(dt.Rows[i]["HandOverDate"]!=DBNull.Value)
                            txtHandOverDt.Text= dt.Rows[i]["HandOverDate"].ToString();
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

        private void InsertRecord()
        {
            bool result = false;
            try
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                string strQry = "";
                SqlHelper.BeginTransaction(out sqlConn, out sqlCmd, out sqlTrans, AppGlobal.strConnString);

                string strQry1 = "delete  from T_Asset where OrgId=@OrgID and Employeecd=@EmpCode";
                //string strQry1 = "delete  from T_Asset where OrgId=" + Convert.ToInt32(Session["OrgId"]) + " and IssueDate='" + txtIssueDate.Text + "' and Employeecd='" + txtEmpCode.Text + "'";

                SqlParameter[] paraDet = new SqlParameter[2];
                paraDet[0] = new SqlParameter("@OrgID", Session["OrgId"].ToString());
                paraDet[1] = new SqlParameter("@EmpCode", txtEmpCode.Text);
               
                result = SqlHelper.ExecuteNonQuery(strQry1, paraDet, AppGlobal.strConnString);

                foreach (DataRow item in dtCurrentTable.Rows)
                {
                    if (item["MaterialDetails"].ToString() == "")
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter Material Descreiption'); ", true);
                        return;
                    }
                    if (item["Quantity"].ToString() == ""|| item["Quantity"].ToString() == "0")
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter Quantity'); ", true);
                        return;
                    }
                    if (item["IssueDate"].ToString() == "" )
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Enter Issue Date'); ", true);
                        return;
                    }

                    strQry = @"INSERT INTO T_Asset(OrgId, Employeecd,IssueDate, SrNo,Item, MaterialDetails, Quantity,Amount, Remark,HandOverDate) 
                                        VALUES(@OrgId, @Employeecd,@IssueDate, @SrNo,@Item, @MaterialDetails, @Quantity,@Amount, @Remark,@HandOverDate)";

                    SqlParameter[] para = new SqlParameter[10];
                    para[0] = new SqlParameter("@OrgId", Session["OrgId"].ToString());
                    para[1] = new SqlParameter("@Employeecd", txtEmpCode.Text);

                    para[2] = new SqlParameter("@Srno", item["SrNo"].ToString());
                    para[3] = new SqlParameter("@IssueDate", Convert.ToDateTime(item["IssueDate"]).ToString("dd MMM yyyy"));
                    
                    para[4] = new SqlParameter("@MaterialDetails", item["MaterialDetails"].ToString());
                    para[5] = new SqlParameter("@Quantity", item["Quantity"].ToString());
                    para[6] = new SqlParameter("@Amount", item["Amount"].ToString());
                    para[7] = new SqlParameter("@Remark", item["Remark"].ToString());
                    para[8] = new SqlParameter("@Item", item["Item"].ToString());
                    if(item["HandOverDate"].ToString()!="")
                        para[9] = new SqlParameter("@HandOverDate", Convert.ToDateTime(item["HandOverDate"]).ToString("dd MMM yyyy"));
                    else
                        para[9] = new SqlParameter("@HandOverDate", (object)DBNull.Value);
                    result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, para);
                }

                if (result)
                {
                    strQry = "";
                    strQry = "INSERT INTO T_Log(OrgId,docdate, Employeecd, MenuId, Mode, Computername) VALUES(@OrgId,@docdate, @Employeecd, @MenuId, @Mode, @Computername)";

                    SqlParameter[] paraLog = new SqlParameter[6];
                    paraLog[0] = new SqlParameter("@OrgId", Convert.ToInt32(Session["OrgID"]));
                    paraLog[1] = new SqlParameter("@Employeecd", Session["UserName"].ToString());
                    paraLog[2] = new SqlParameter("@MenuId", Convert.ToInt32(Session["MM"]));
                    paraLog[3] = new SqlParameter("@Mode", "A");
                    paraLog[4] = new SqlParameter("@Computername", Session["IP"].ToString());
                    paraLog[5] = new SqlParameter("@docdate", Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy"));

                    result = SqlHelper.ExecuteTransaction(sqlCmd, strQry, paraLog);
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
            SetInitialRow();
            BindData();
            txtEmpCode.Text = "";
            ddlEmpName.SelectedIndex = 0;
            txtDesignation.Text = "";
            txtDepartment.Text = "";
            txtMobileNo.Text = "";
            txtEmailID.Text = "";
            txtDrivingLicence.Text = "";
            txtPermenentAdd.Text = "";
            txtWorkingAdd.Text = "";
           // txtIssueDate.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
            
            btnSave.Text = "Save";
        }

        protected void txtEmpCode_TextChanged(object sender, EventArgs e)
        {
            if (txtEmpCode.Text != "")
            {
                string strQry = "SELECT Employeename FROM M_EMP Where OrgID=" + Convert.ToInt16(Session["OrgID"]) + " and Employeecd='" + txtEmpCode.Text + "'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    showEmpDetails();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Wrong Employee Code'); ", true);
                }
            }
        }
        protected void showEmpDetails()
        {
           
                string wAddress = "", pAddress = "";
                string strQry = "select *  from udf_EmpForAsset(" + Convert.ToInt32(Session["OrgId"]) + ",'" + DateTime.Now.ToString("dd MMM yyyy") + "','" + txtEmpCode.Text + "')";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    if (objDT.Rows[0]["Employeename"] != DBNull.Value)
                        ddlEmpName.SelectedItem.Text = objDT.Rows[0]["Employeename"].ToString();
                    if (objDT.Rows[0]["MobNo"] != DBNull.Value)
                        txtMobileNo.Text = objDT.Rows[0]["MobNo"].ToString();
                    if (objDT.Rows[0]["DrivingLicence"] != DBNull.Value)
                        txtDrivingLicence.Text = objDT.Rows[0]["DrivingLicence"].ToString();
                    if (objDT.Rows[0]["Designation"] != DBNull.Value)
                        txtDesignation.Text = objDT.Rows[0]["Designation"].ToString();
                    if (objDT.Rows[0]["LocationDep"] != DBNull.Value)
                        txtDepartment.Text = objDT.Rows[0]["LocationDep"].ToString();


                    if (objDT.Rows[0]["WAddre"] != DBNull.Value)
                        wAddress = objDT.Rows[0]["WAddre"].ToString();
                    if (objDT.Rows[0]["WTaluka"] != DBNull.Value)
                        wAddress = wAddress + " Taluka- " + objDT.Rows[0]["WTaluka"].ToString();
                    if (objDT.Rows[0]["WDistrict"] != DBNull.Value)
                        wAddress = wAddress + " District- " + objDT.Rows[0]["WDistrict"].ToString();
                    if (objDT.Rows[0]["wstate"] != DBNull.Value)
                        wAddress = wAddress + " State- " + objDT.Rows[0]["wstate"].ToString();

                    if (objDT.Rows[0]["wPincode"] != DBNull.Value)
                        wAddress = wAddress + " Pincode- " + objDT.Rows[0]["wPincode"].ToString();

                    if (objDT.Rows[0]["PAddre"] != DBNull.Value)
                        pAddress = objDT.Rows[0]["PAddre"].ToString();
                    if (objDT.Rows[0]["PCity"] != DBNull.Value)
                        pAddress = pAddress + " " + objDT.Rows[0]["PCity"].ToString();
                    if (objDT.Rows[0]["pTaluka"] != DBNull.Value)
                        pAddress = pAddress + " Taluka- " + objDT.Rows[0]["pTaluka"].ToString();
                    if (objDT.Rows[0]["PDistrict"] != DBNull.Value)
                        pAddress = pAddress + " District- " + objDT.Rows[0]["PDistrict"].ToString();
                    if (objDT.Rows[0]["pState"] != DBNull.Value)
                        pAddress = pAddress + " State- " + objDT.Rows[0]["pState"].ToString();
                    if (objDT.Rows[0]["pPincode"] != DBNull.Value)
                        pAddress = pAddress + " Pincode- " + objDT.Rows[0]["pPincode"].ToString();

                    txtPermenentAdd.Text = pAddress;
                    txtWorkingAdd.Text = wAddress;
                }
            
            FillGrid();
        }
        protected void ddlEmpName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEmpName.SelectedIndex != 0)
            {
                txtEmpCode.Text = ddlEmpName.SelectedValue.ToString();
                showEmpDetails();
            }
        }

        protected void txtIssueDate_TextChanged(object sender, EventArgs e)
        {
            FillGrid();
        }

        protected void FillGrid()
        {
            if (txtEmpCode.Text != "" )
            {
                //string strQry = "select SrNo,CONVERT(VARCHAR(10), IssueDate, 103) as IssueDate,T_Asset.Item,M_Item.Item, MaterialDetails,Quantity,amount,Remark,CONVERT(VARCHAR(10), HandOverDate, 103) as HandOverDate from T_Asset inner join M_Item on T_Asset.Item=M_Item.Itemcd  where OrgId=" + Convert.ToInt32(Session["OrgId"]) + " and IssueDate='" + Convert.ToDateTime(txtIssueDate.Text).ToString("dd MMM yyyy") + "' and Employeecd='" + txtEmpCode.Text + "'";
                string strQry = "select SrNo,CONVERT(VARCHAR(10), IssueDate, 103) as IssueDate,T_Asset.Item,M_Item.Item, MaterialDetails,Quantity,amount,Remark,CONVERT(VARCHAR(10), HandOverDate, 103) as HandOverDate from T_Asset inner join M_Item on T_Asset.Item=M_Item.Itemcd  where OrgId=" + Convert.ToInt32(Session["OrgId"]) + " and Employeecd='" + txtEmpCode.Text + "'";
                DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (objDT.Rows.Count > 0)
                {
                    Gridview1.DataSource = objDT;
                    Gridview1.DataBind();
                    ViewState["CurrentTable"] = objDT;
                }
                else
                {
                    SetInitialRow();
                }
            }
        }

        protected void lbtnRemove_Click(object sender, EventArgs e)
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            ((DataTable)ViewState["CurrentTable"]).Rows.RemoveAt(gvrow.RowIndex);

            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("Item", typeof(string)));
            dt.Columns.Add(new DataColumn("MaterialDetails", typeof(string)));
            dt.Columns.Add(new DataColumn("Quantity", typeof(string)));
            dt.Columns.Add(new DataColumn("IssueDate", typeof(string)));
            dt.Columns.Add(new DataColumn("Amount", typeof(string)));
            dt.Columns.Add(new DataColumn("Remark", typeof(string))); 
            dt.Columns.Add(new DataColumn("HandOverDate", typeof(string))); 

            int rowIndex = 0;
            DataRow drCurrentRow = null;
            string strAction = "";
            //for (int i = ((DataTable)ViewState["CurrentTable"]).Rows.Count - 1; i >= 0; i--)
            //{
            //    DataRow dr = ((DataTable)ViewState["CurrentTable"]).Rows[i];
            //    if ((int)dr["SrNo"] == gvrow.RowIndex)
            //        dr.Delete();
            //}

            for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
            {
                DropDownList ddlItem = (DropDownList)Gridview1.Rows[rowIndex].Cells[1].FindControl("ddlItem");
                TextBox txtDescription = (TextBox)Gridview1.Rows[rowIndex].Cells[2].FindControl("txtDescription");
                TextBox txtQuantity = (TextBox)Gridview1.Rows[rowIndex].Cells[3].FindControl("txtQuantity");
                TextBox txtAmount = (TextBox)Gridview1.Rows[rowIndex].Cells[4].FindControl("txtAmount");
                TextBox txtRemark = (TextBox)Gridview1.Rows[rowIndex].Cells[5].FindControl("txtRemark");
                TextBox txtIssueDate = (TextBox)Gridview1.Rows[rowIndex].Cells[6].FindControl("txtIssueDate");
                TextBox txtHandOverDate = (TextBox)Gridview1.Rows[rowIndex].Cells[7].FindControl("txtHandOverDate");
                
                drCurrentRow = dt.NewRow();
                drCurrentRow["SrNo"] = i;
                drCurrentRow["Item"] = ddlItem.SelectedValue;
                drCurrentRow["MaterialDetails"] = txtDescription.Text;
                drCurrentRow["Quantity"] = txtQuantity.Text;
                // dtCurrentTable.Rows[i - 1]["IssueDate"] = txtIssueDate.Text;
                drCurrentRow["Amount"] = txtAmount.Text;
                drCurrentRow["Remark"] = txtRemark.Text;
                drCurrentRow["HandOverDate"] = txtHandOverDate.Text;
                dt.Rows.Add(drCurrentRow);
                rowIndex++;
            }
            
            ViewState["CurrentTable"] = dt;
            Gridview1.DataSource = dt;
            Gridview1.DataBind();
        }
    }
}