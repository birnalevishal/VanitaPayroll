using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PayRoll
{
    public class AppGlobal
    {
        public static string strConnString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["VanitaPayrollConnectionString"].ToString();
            }
            set
            {

            }
        }

    }
}