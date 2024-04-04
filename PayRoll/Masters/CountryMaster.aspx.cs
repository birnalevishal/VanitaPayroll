using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using SqlClient;
using PayRoll.Models;
using System.Data.SqlClient;

namespace PayRoll.Masters
{
    public partial class CountryMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod]
        public static string InsertRecord(CountryModel obj)
        {
            string strQry = "";
            bool result = false;

            strQry = "SELECT Count(*) FROM M_Country Where Country=" + obj.Country;
            int nCnt = (int)SqlHelper.ExecuteScalar(strQry, AppGlobal.strConnString);
            if (nCnt > 0)
            {
               
                return "DUPLICATE";
            }

            strQry = "INSERT INTO M_Country(CountryCd, Country, IsActive) VALUES(@CountryCd, @Country, @IsActive)";
            SqlParameter[] para = new SqlParameter[3];
            para[0] = new SqlParameter("@CountryCd", SqlHelper.GetMaxID("M_Country", "CountryCd", AppGlobal.strConnString));
            para[1] = new SqlParameter("@Country", obj.Country);
            para[2] = new SqlParameter("@IsActive", obj.IsActive=="true" ? "Y" : "N");

            result = SqlHelper.ExecuteNonQuery(strQry, para, AppGlobal.strConnString);

            if (result)
            {
                return "OK";
            }

            return "NOT OK";

        }
    }
}