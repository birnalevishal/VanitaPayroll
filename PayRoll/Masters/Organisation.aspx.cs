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
using System.IO;

namespace PayRoll.Masters
{
    public partial class Organisation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
                BindGrid();
                txtName.Focus();
                EmpPhoto.ImageUrl = "~/Upload/Logo.jpg";
            }
        }

        private void BindData()
        {
            string strQry = "";
            DataTable objDT;

            strQry = "SELECT Country, CountryCd FROM M_Country ORDER BY Country";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlCountry.DataSource = objDT;
            ddlCountry.DataTextField = "Country";
            ddlCountry.DataValueField = "CountryCd";
            ddlCountry.DataBind();

            ddlCountry.Items.Insert(0, new ListItem("Select", "0"));

            strQry = "SELECT State, StateCd FROM M_State ORDER BY State";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlState.DataSource = objDT;
            ddlState.DataTextField = "State";
            ddlState.DataValueField = "StateCd";
            ddlState.DataBind();

            ddlState.Items.Insert(0, new ListItem("Select", "0"));

            strQry = "SELECT District, DistCd FROM M_District ORDER BY District";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlDistrict.DataSource = objDT;
            ddlDistrict.DataTextField = "District";
            ddlDistrict.DataValueField = "DistCd";
            ddlDistrict.DataBind();

            ddlDistrict.Items.Insert(0, new ListItem("Select", "0"));

            strQry = "SELECT Taluka, TalCd FROM M_Taluka ORDER BY Taluka";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlTaluka.DataSource = objDT;
            ddlTaluka.DataTextField = "Taluka";
            ddlTaluka.DataValueField = "TalCd";
            ddlTaluka.DataBind();

            ddlTaluka.Items.Insert(0, new ListItem("Select", "0"));

            strQry = "SELECT City, Citycd FROM M_City ORDER BY City";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlCity.DataSource = objDT;
            ddlCity.DataTextField = "City";
            ddlCity.DataValueField = "Citycd";
            ddlCity.DataBind();

            ddlCity.Items.Insert(0, new ListItem("Select", "0"));

            strQry = "SELECT BankName, BankCd FROM M_Bank ORDER BY BankName";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlBankName.DataSource = objDT;
            ddlBankName.DataTextField = "BankName";
            ddlBankName.DataValueField = "BankCd";
            ddlBankName.DataBind();

            ddlBankName.Items.Insert(0, new ListItem("Select", "0"));

            strQry = "SELECT ActType, ActTypecd FROM M_BankActType ORDER BY ActType";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlBankACType.DataSource = objDT;
            ddlBankACType.DataTextField = "ActType";
            ddlBankACType.DataValueField = "ActTypecd";
            ddlBankACType.DataBind();

            ddlBankACType.Items.Insert(0, new ListItem("Select", "0"));

            strQry = "SELECT BankBranch, BankBranchId FROM M_BankBranch ORDER BY BankBranch";
            objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlBankBranch.DataSource = objDT;
            ddlBankBranch.DataTextField = "BankBranch";
            ddlBankBranch.DataValueField = "BankBranchId";
            ddlBankBranch.DataBind();

            ddlBankBranch.Items.Insert(0, new ListItem("Select", "0"));
        }

        private void BindGrid()
        {
            string strQry = "SELECT * FROM M_Organization";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            gvList.DataSource = objDT;
            gvList.DataBind();

            ViewState["objDTList"] = objDT;
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    if (formValidation())
                    {
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
            
            strQry = @"INSERT INTO M_Organization(OrgId, Organization, Address, Citycd, Talcd, DistCd, StateCd, CountryCd, NatureOfwork, PhoneNo, MobileNo, SupervChg, BankCd, BankActNo, BankBranchId, BankActTypecd, PFNo, ESICode, PTNo, MahLabWelFund, GstNo,PanNo,Closedt, CIN, IsActive, LogoPath) 
                                        VALUES(@OrgId, @Organization, @Address, @Citycd, @Talcd, @DistCd, @StateCd, @CountryCd, @NatureOfwork, @PhoneNo, @MobileNo, @SupervChg, @BankCd, @BankActNo, @BankBranchId, @BankActTypecd, @PFNo, @ESICode, @PTNo, @MahLabWelFund, @GstNo,@PanNo,@Closedt,@CIN, @IsActive, @LogoPath)";
            int nID = SqlHelper.GetMaxID("M_Organization", "OrgId", AppGlobal.strConnString);

            SqlParameter[] para = new SqlParameter[26];
            para[0] = new SqlParameter("@OrgId", nID);
            para[1] = new SqlParameter("@Organization", txtName.Text.Trim());
            para[2] = new SqlParameter("@Address", txtAddress.Text.Trim());
            para[3] = new SqlParameter("@Citycd", ddlCity.SelectedValue);
            para[4] = new SqlParameter("@Talcd", ddlTaluka.SelectedValue);
            para[5] = new SqlParameter("@DistCd", ddlDistrict.SelectedValue);
            para[6] = new SqlParameter("@StateCd", ddlState.SelectedValue);
            para[7] = new SqlParameter("@CountryCd", ddlCountry.SelectedValue);
            para[8] = new SqlParameter("@NatureOfwork", txtNatureOfWork.Text.Trim());
            para[9] = new SqlParameter("@PhoneNo", txtPhnNo.Text);
            para[10] = new SqlParameter("@MobileNo", txtMoNo.Text);
            para[11] = new SqlParameter("@SupervChg", txtSupVChg.Text);
            para[12] = new SqlParameter("@BankCd", ddlBankName.SelectedValue);
            para[13] = new SqlParameter("@BankActNo", txtBankAcNo.Text);
            para[14] = new SqlParameter("@BankBranchId", ddlBankBranch.SelectedValue);
            para[15] = new SqlParameter("@BankActTypecd", ddlBankACType.SelectedValue);
            para[16] = new SqlParameter("@PFNo", txtPFNo.Text);
            para[17] = new SqlParameter("@ESICode", txtESIC.Text);
            para[18] = new SqlParameter("@PTNo", txtPTNo.Text);
            para[19] = new SqlParameter("@MahLabWelFund", txtWelfare.Text);
            para[20] = new SqlParameter("@GstNo", txtGSTNo.Text);
            para[21] = new SqlParameter("@PanNo", PanNo.Text);
            para[22] = new SqlParameter("@Closedt", txtCloseDt.Text.Trim()==""? (object)DBNull.Value: Convert.ToDateTime(txtCloseDt.Text.Trim()).ToString("dd MMM yyyy"));
            para[23] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");
            para[24] = new SqlParameter("@CIN", txtCIN.Text );

            if (dropzone.HasFile)
            {
                //Get Filename from fileupload control
                string filename = Path.GetFileName(dropzone.PostedFile.FileName);
                string LogoName = nID.ToString();
                string fileExtension = Path.GetExtension(dropzone.PostedFile.FileName);

                if (fileExtension == ".jpeg" || fileExtension == ".jpg" || fileExtension == ".png")
                {
                    //filename = empcode + filename;
                    //Save images into Images folder
                    dropzone.SaveAs(Server.MapPath("~/Upload/" + LogoName + fileExtension));
                    para[25] = new SqlParameter("@LogoPath", "~/Upload/" + LogoName + fileExtension);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Uploaded image not in correct format!'); ", true);
                }
            }
            else
            {
                para[25] = new SqlParameter("@LogoPath", "~/Upload/" + "Logo.jpg");
            }

            result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

            if (result)
            {
                clearControls();
                BindGrid();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
            }
        }

        private void UpdateRecord()
        {
            string strQry = "";
            bool result = false;
            int nId = Convert.ToInt32(ViewState["ID"]);

            strQry = @"UPDATE M_Organization SET Organization=@Organization, Address=@Address, Citycd=@Citycd, Talcd=@Talcd, DistCd=@DistCd, StateCd=@StateCd,
                        CountryCd=@CountryCd, NatureOfwork=@NatureOfwork, PhoneNo=@PhoneNo,MobileNo=@MobileNo, SupervChg=@SupervChg, BankCd=@BankCd, BankActNo=@BankActNo,
                        BankBranchId=@BankBranchId, BankActTypecd=@BankActTypecd, PFNo=@PFNo, ESICode=@ESICode, PTNo=@PTNo, MahLabWelFund=@MahLabWelFund, 
                        GstNo=@GstNo, Closedt=@Closedt,PanNo=@PanNo,CIN=@CIN, IsActive=@IsActive , LogoPath=@LogoPath
                       WHERE OrgId=@OrgId";
            SqlParameter[] para = new SqlParameter[26];
            para[0] = new SqlParameter("@OrgId", nId);
            para[1] = new SqlParameter("@Organization", txtName.Text.Trim());
            para[2] = new SqlParameter("@Address", txtAddress.Text.Trim());
            para[3] = new SqlParameter("@Citycd", ddlCity.SelectedValue);
            para[4] = new SqlParameter("@Talcd", ddlTaluka.SelectedValue);
            para[5] = new SqlParameter("@DistCd", ddlDistrict.SelectedValue);
            para[6] = new SqlParameter("@StateCd", ddlState.SelectedValue);
            para[7] = new SqlParameter("@CountryCd", ddlCountry.SelectedValue);
            para[8] = new SqlParameter("@NatureOfwork", txtNatureOfWork.Text.Trim());
            para[9] = new SqlParameter("@PhoneNo", txtPhnNo.Text);
            para[10] = new SqlParameter("@MobileNo", txtMoNo.Text);
            para[11] = new SqlParameter("@SupervChg", txtSupVChg.Text);
            para[12] = new SqlParameter("@BankCd", ddlBankName.SelectedValue);
            para[13] = new SqlParameter("@BankActNo", txtBankAcNo.Text);
            para[14] = new SqlParameter("@BankBranchId", ddlBankBranch.SelectedValue);
            para[15] = new SqlParameter("@BankActTypecd", ddlBankACType.SelectedValue);
            para[16] = new SqlParameter("@PFNo", txtPFNo.Text);
            para[17] = new SqlParameter("@ESICode", txtESIC.Text);
            para[18] = new SqlParameter("@PTNo", txtPTNo.Text);
            para[19] = new SqlParameter("@MahLabWelFund", txtWelfare.Text);
            para[20] = new SqlParameter("@GstNo", txtGSTNo.Text);
            para[21] = new SqlParameter("@PanNo", PanNo.Text);
            para[22] = new SqlParameter("@Closedt", txtCloseDt.Text.Trim() == "" ? (object)DBNull.Value : Convert.ToDateTime(txtCloseDt.Text.Trim()).ToString("dd MMM yyyy"));
            para[23] = new SqlParameter("@IsActive", chkIsActive.Checked ? "Y" : "N");
            para[24] = new SqlParameter("@CIN", txtCIN.Text );

            if (dropzone.HasFile)
            {
                //Get Filename from fileupload control
                string filename = Path.GetFileName(dropzone.PostedFile.FileName);
                string LogoName = nId.ToString();
                string fileExtension = Path.GetExtension(dropzone.PostedFile.FileName);

                if (fileExtension == ".jpeg" || fileExtension == ".jpg" || fileExtension == ".png")
                {
                    //filename = empcode + filename;
                    //Save images into Images folder
                    dropzone.SaveAs(Server.MapPath("~/Upload/" + LogoName + fileExtension));
                    para[25] = new SqlParameter("@LogoPath", "~/Upload/" + LogoName + fileExtension);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Uploaded image not in correct format!'); ", true);
                }
            }
            else
            {
                para[25] = new SqlParameter("@LogoPath", "~/Upload/" + EmpPhoto.ImageUrl);
            }

            result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

            if (result)
            {
                clearControls();
                BindGrid();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Saved Successfully!'); ", true);
            }
        }

        private void clearControls()
        {
            txtName.Text = "";
            txtAddress.Text = "";
            ddlCity.SelectedIndex = 0;
            ddlTaluka.SelectedIndex = 0;
            ddlDistrict.SelectedIndex = 0;
            ddlState.SelectedIndex = 0;
            ddlCountry.SelectedIndex = 0;
            txtNatureOfWork.Text = "";
            txtPhnNo.Text = "";
            txtMoNo.Text = "";
            txtSupVChg.Text = "";
            ddlBankName.SelectedIndex = 0;
            ddlBankACType.SelectedIndex = 0;
            txtBankAcNo.Text = "";
            ddlBankBranch.SelectedIndex = 0;
            txtPFNo.Text = "";
            txtESIC.Text = "";
            txtPTNo.Text = "";
            txtWelfare.Text = "";
            txtGSTNo.Text = "";
            PanNo.Text = "";
            txtCIN.Text = "";
            txtCloseDt.Text = "";
            chkIsActive.Checked = true;
            btnSave.Text = "Save";
            ViewState["ID"] = null;
            txtName.Focus();

            EmpPhoto.ImageUrl = "~/Upload/Logo.jpg";
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
                    clearControls();
                    ViewRecord(i);

                    btnSave.Text = "Update";
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void ViewRecord(int i)
        {
            DataTable objDT = (DataTable)ViewState["objDTList"];

            ViewState["ID"] = objDT.Rows[i]["OrgId"].ToString();
            txtName.Text = objDT.Rows[i]["Organization"].ToString();
            txtAddress.Text = objDT.Rows[i]["Address"].ToString();
            ddlCity.SelectedValue = objDT.Rows[i]["Citycd"] != DBNull.Value ? objDT.Rows[i]["Citycd"].ToString() : "0";
            ddlTaluka.SelectedValue = objDT.Rows[i]["Talcd"] != DBNull.Value ? objDT.Rows[i]["Talcd"].ToString() : "0";
            ddlDistrict.SelectedValue = objDT.Rows[i]["DistCd"] != DBNull.Value ? objDT.Rows[i]["DistCd"].ToString() : "0";
            ddlState.SelectedValue = objDT.Rows[i]["StateCd"] != DBNull.Value ? objDT.Rows[i]["StateCd"].ToString() : "0";
            ddlCountry.SelectedValue = objDT.Rows[i]["CountryCd"] != DBNull.Value ? objDT.Rows[i]["CountryCd"].ToString() : "0";
            txtNatureOfWork.Text = objDT.Rows[i]["NatureOfwork"].ToString();
            txtPhnNo.Text = objDT.Rows[i]["PhoneNo"].ToString();
            txtMoNo.Text = objDT.Rows[i]["MobileNo"].ToString();
            txtSupVChg.Text = objDT.Rows[i]["SupervChg"].ToString();
            ddlBankName.SelectedValue = objDT.Rows[i]["BankCd"] != DBNull.Value ? objDT.Rows[i]["BankCd"].ToString() : "0";
            txtBankAcNo.Text = objDT.Rows[i]["BankActNo"].ToString();
            ddlBankBranch.SelectedValue = objDT.Rows[i]["BankBranchId"] != DBNull.Value ? objDT.Rows[i]["BankBranchId"].ToString() : "0";
            ddlBankACType.SelectedValue = objDT.Rows[i]["BankActTypecd"] != DBNull.Value ? objDT.Rows[i]["BankActTypecd"].ToString() : "0";
            txtPFNo.Text = objDT.Rows[i]["PFNo"].ToString();
            txtESIC.Text = objDT.Rows[i]["ESICode"].ToString();
            txtPTNo.Text = objDT.Rows[i]["PTNo"].ToString();
            txtWelfare.Text = objDT.Rows[i]["MahLabWelFund"].ToString();
            txtGSTNo.Text = objDT.Rows[i]["GstNo"].ToString();
            PanNo.Text = objDT.Rows[i]["PanNo"].ToString();
            txtCloseDt.Text = objDT.Rows[i]["Closedt"] != DBNull.Value ? Convert.ToDateTime(objDT.Rows[i]["Closedt"]).ToString("dd/MM/yyyy") : "";
            chkIsActive.Checked = objDT.Rows[i]["IsActive"].ToString() == "Y" ? true : false;
            txtCIN.Text = objDT.Rows[i]["CIN"] != DBNull.Value ? objDT.Rows[i]["CIN"].ToString() : "";

            if (objDT.Rows[i]["LogoPath"] != DBNull.Value)
                EmpPhoto.ImageUrl = objDT.Rows[i]["LogoPath"].ToString();
            else
                EmpPhoto.ImageUrl = "~/Upload/Logo.jpg";

            txtName.Focus();
        }
        protected bool formValidation()
        {
            int nID = 0;
            if (ViewState["ID"] != null)
            {
                nID = Convert.ToInt32(ViewState["ID"]);
            }

            string strQry = "SELECT Count(*) FROM M_Organization Where OrgId<>" + nID + " and Organization ='" + txtName.Text.Trim() + "' ";
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('Organization is Already Exists!'); ", true);
                clearControls();
                return false;
            }


            return true;
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strQry = "SELECT State, StateCd FROM M_State where CountryCd=" + ddlCountry.SelectedValue + " ORDER BY State";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlState.DataSource = objDT;
            ddlState.DataTextField = "State";
            ddlState.DataValueField = "StateCd";
            ddlState.DataBind();

            ddlState.Items.Insert(0, new ListItem("Select", "0"));
            ddlDistrict.Items.Insert(0, new ListItem("Select", "0"));
            ddlTaluka.Items.Insert(0, new ListItem("Select", "0"));
            ddlCity.Items.Insert(0, new ListItem("Select", "0"));

            ddlState.SelectedIndex = 0;
            ddlDistrict.SelectedIndex = 0;
            ddlTaluka.SelectedIndex = 0;
            ddlCity.SelectedIndex = 0;

            ddlState.Focus();
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strQry = "SELECT District, DistCd FROM M_District where StateCd=" + ddlState.SelectedValue + " ORDER BY District";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlDistrict.DataSource = objDT;
            ddlDistrict.DataTextField = "District";
            ddlDistrict.DataValueField = "DistCd";
            ddlDistrict.DataBind();

            ddlDistrict.Items.Insert(0, new ListItem("Select", "0"));
            ddlTaluka.Items.Insert(0, new ListItem("Select", "0"));
            ddlCity.Items.Insert(0, new ListItem("Select", "0"));

            ddlDistrict.SelectedIndex = 0;
            ddlTaluka.SelectedIndex = 0;
            ddlCity.SelectedIndex = 0;

            ddlDistrict.Focus();
        }

        protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strQry = "SELECT Taluka, TalCd FROM M_Taluka where DistCd=" + ddlDistrict.SelectedValue + " ORDER BY Taluka";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlTaluka.DataSource = objDT;
            ddlTaluka.DataTextField = "Taluka";
            ddlTaluka.DataValueField = "TalCd";
            ddlTaluka.DataBind();

            ddlTaluka.Items.Insert(0, new ListItem("Select", "0"));
            ddlCity.Items.Insert(0, new ListItem("Select", "0"));

            ddlTaluka.SelectedIndex = 0;
            ddlCity.SelectedIndex = 0;

            ddlTaluka.Focus();
        }

        protected void ddlTaluka_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strQry = "SELECT City, Citycd FROM M_City where TalCd=" + ddlTaluka.SelectedValue + " ORDER BY City";
            DataTable objDT = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            ddlCity.DataSource = objDT;
            ddlCity.DataTextField = "City";
            ddlCity.DataValueField = "Citycd";
            ddlCity.DataBind();

            ddlCity.Items.Insert(0, new ListItem("Select", "0"));

            ddlCity.SelectedIndex = 0;

            ddlCity.Focus();
        }
        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
    }
}