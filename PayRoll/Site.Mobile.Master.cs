using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SqlClient;
using System.Data;
using System.Configuration;

namespace PayRoll
{
    public partial class Site_Mobile : System.Web.UI.MasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session.Keys.Count == 0)
            {
                Response.Redirect("~/Login.aspx");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

            }

            string PM = Session["PM"].ToString();
            string MM = Session["MM"].ToString();

            CreateMenus(PM, MM);
            CreateReportsMenus();
            CreateMasterMenus();
        }

        private void CreateMenus(string parentMenuId = "", string menuItem = "")
        {
            string RoleId = Session["RoleId"].ToString();

            mnuPlaceHolder.Controls.Clear();

            DataTable dt;
            if (RoleId == "1")
            {
                dt = SqlHelper.ExecuteDataTable("SELECT * FROM M_Menu WHERE ParentMenuId=0  AND IsActive='Y' order by seqNo", AppGlobal.strConnString);
            }
            else
            {
                string PMIds = Session["PMIds"].ToString();
                dt = SqlHelper.ExecuteDataTable("SELECT * FROM M_Menu WHERE ParentMenuId=0 AND IsActive='Y' AND MenuId IN(" + PMIds + ") order by seqNo", AppGlobal.strConnString);
            }

            int i = 1;
            foreach (DataRow item in dt.Rows)
            {
                HtmlGenericControl li = new HtmlGenericControl("li");

                if (item["MenuId"].ToString() == parentMenuId)
                {
                    li.Attributes.Add("class", "active");
                }

                LinkButton a = new LinkButton();
                //a.PostBackUrl = item["Url"].ToString();
                a.ID = "c" + i;
                a.Command += btnMenuItem_Click;
                a.CommandArgument = item["MenuId"].ToString();

                //if(item["iconclass"].ToString()!="")
                //{
                //    HtmlGenericControl ii = new HtmlGenericControl("i");
                //    ii.Attributes.Add("class", item["iconclass"].ToString());
                //    a.Controls.Add(ii);
                //}

                HtmlGenericControl span = new HtmlGenericControl("span");
                span.Attributes.Add("class", "nav-label");
                span.InnerText = item["Title"].ToString();

                a.Controls.Add(span);

                HtmlGenericControl ul = new HtmlGenericControl("ul");
                ul.Attributes.Add("class", "nav nav-second-level");

                string strQry = "";
                if (RoleId == "1")
                {
                    strQry = "select * from M_Menu where  IsActive='Y' AND ParentMenuId=" + item["MenuId"].ToString() + " order by seqNo";
                }
                else
                {
                    string MMIds = Session["MMIds"].ToString();
                    strQry = "select * from M_Menu where IsActive='Y' AND ParentMenuId=" + item["MenuId"].ToString() + "AND MenuId IN(" + MMIds + ") order by seqNo";
                }

                DataTable subMenu = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
                if (subMenu.Rows.Count > 0)
                {
                    HtmlGenericControl span1 = new HtmlGenericControl("span");
                    span1.Attributes.Add("class", "fa arrow");
                    a.Controls.Add(span1);

                    ul.Controls.Clear();
                }

                li.Controls.Add(a);

                foreach (DataRow subitem in subMenu.Rows)
                {
                    i++;

                    HtmlGenericControl subli = new HtmlGenericControl("li");

                    if (subitem["MenuId"].ToString() == menuItem)
                    {
                        subli.Attributes.Add("class", "active");
                    }

                    LinkButton suba = new LinkButton();
                    suba.Text = subitem["Title"].ToString();
                    //suba.PostBackUrl = subitem["Url"].ToString();
                    suba.ID = "c" + i;
                    suba.Command += btnMenuItem_Click;
                    suba.CommandArgument = subitem["MenuId"].ToString();

                    subli.Controls.Add(suba);
                    ul.Controls.Add(subli);
                }

                if (subMenu.Rows.Count > 0)
                {
                    li.Controls.Add(ul);
                }

                mnuPlaceHolder.Controls.Add(li);
                i++;
            }
        }

        protected void btnMenuItem_Click(object sender, CommandEventArgs e)
        {
            try
            {
                LinkButton lb = (LinkButton)sender;
                string menuId = e.CommandArgument.ToString();
                DataTable dt = SqlHelper.ExecuteDataTable("SELECT * FROM M_Menu WHERE IsActive='Y' AND MenuId=" + menuId, AppGlobal.strConnString);
                if (dt.Rows.Count > 0)
                {
                    Session["PM"] = dt.Rows[0]["ParentMenuId"].ToString();
                    Session["MM"] = dt.Rows[0]["MenuId"].ToString();
                    Response.Redirect(dt.Rows[0]["Url"].ToString());
                }


            }
            catch (Exception ex)
            {

            }
        }

        protected void lbtnLogOut_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            Response.Redirect("~/Login.aspx");
        }

        private void CreateReportsMenus(string parentMenuId = "", string menuItem = "")
        {
            string RoleId = Session["RoleId"].ToString();
            string strQry = "";

            if (RoleId == "1")
            {
                strQry = "select * from M_Menu where  IsActive='Y' AND ParentMenuId IN(4,11) order by Title";
            }
            else
            {
                string MMIds = Session["MMIds"].ToString();
                strQry = "select * from M_Menu where IsActive='Y' AND ParentMenuId IN(4,11) AND MenuId IN(" + MMIds + ") order by Title";
            }

            DataTable menus = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (menus.Rows.Count > 0)
            {
                int i = 0;
                HtmlGenericControl tr = new HtmlGenericControl("tr");
                foreach (DataRow item in menus.Rows)
                {
                    if (i != 0 && i % 4 == 0)
                    {
                        rptMenuPlaceHolder.Controls.Add(tr);
                        tr = new HtmlGenericControl("tr");
                    }

                    HtmlGenericControl td = new HtmlGenericControl("td");

                    LinkButton a = new LinkButton();
                    a.PostBackUrl = item["Url"].ToString();

                    HtmlGenericControl h5 = new HtmlGenericControl("h5");
                    h5.InnerText = item["Title"].ToString();

                    a.Controls.Add(h5);
                    td.Controls.Add(a);
                    tr.Controls.Add(td);

                    i++;
                }
                rptMenuPlaceHolder.Controls.Add(tr);
            }
        }

        private void CreateMasterMenus(string parentMenuId = "", string menuItem = "")
        {
            string RoleId = Session["RoleId"].ToString();
            string strQry = "";

            if (RoleId == "1")
            {
                strQry = "select * from M_Menu where  IsActive='Y' AND ParentMenuId IN(2,9) order by Title";
            }
            else
            {
                string MMIds = Session["MMIds"].ToString();
                strQry = "select * from M_Menu where IsActive='Y' AND ParentMenuId IN(2,9) AND MenuId IN(" + MMIds + ") order by Title";
            }

            DataTable menus = SqlHelper.ExecuteDataTable(strQry, AppGlobal.strConnString);
            if (menus.Rows.Count > 0)
            {
                int i = 0;
                HtmlGenericControl tr = new HtmlGenericControl("tr");
                foreach (DataRow item in menus.Rows)
                {
                    if (i != 0 && i % 3 == 0)
                    {
                        masterMenuPlaceHolder.Controls.Add(tr);
                        tr = new HtmlGenericControl("tr");
                    }

                    HtmlGenericControl td = new HtmlGenericControl("td");

                    LinkButton a = new LinkButton();
                    a.PostBackUrl = item["Url"].ToString();

                    HtmlGenericControl h5 = new HtmlGenericControl("h5");
                    h5.InnerText = item["Title"].ToString();

                    a.Controls.Add(h5);
                    td.Controls.Add(a);
                    tr.Controls.Add(td);

                    i++;
                }
                masterMenuPlaceHolder.Controls.Add(tr);
            }
        }
    }
}