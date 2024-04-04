using SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PayRoll
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //CreateMenus(Session["PM"].ToString(), Session["MM"].ToString());

            if (!Page.IsPostBack)
            {

            }
            else
            {

            }
        }

        private void CreateMenus(string parentMenuId = "", string menuItem = "")
        {
            mnuPlaceHolder.Controls.Clear();

            DataTable dt = SqlHelper.ExecuteDataTable("SELECT * FROM M_Menu WHERE ParentMenuId=0 order by title", AppGlobal.strConnString);
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

                HtmlGenericControl span = new HtmlGenericControl("span");
                span.Attributes.Add("class", "nav-label");
                span.InnerText = item["Title"].ToString();

                a.Controls.Add(span);

                HtmlGenericControl ul = new HtmlGenericControl("ul");
                ul.Attributes.Add("class", "nav nav-second-level");

                ///string strQry = "select acc.*, menu.* from M_AccessForm acc inner join M_Menu menu on acc.menuId =menu.menuID where acc.orgID=" + Convert.ToInt16(Session["OrgID"]) + " and yearID=" + Convert.ToInt16(Session["YearID"]) + " and Employeecd='" + Session["UserName"].ToString() + "' and menu.ParentMenuId=" + item["MenuId"].ToString() + " order by title";
                string strQry = "select * from M_Menu where ParentMenuId=" + item["MenuId"].ToString() + " order by title";

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

                    if (item["MenuId"].ToString() == menuItem)
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
                DataTable dt = SqlHelper.ExecuteDataTable("SELECT * FROM M_Menu WHERE MenuId=" + menuId, AppGlobal.strConnString);
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

    }
}