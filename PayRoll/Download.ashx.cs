using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace PayRoll
{
    /// <summary>
    /// Summary description for Download
    /// </summary>
    public class Download : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string file = context.Request.QueryString["file"];

            if (!string.IsNullOrEmpty(file) && File.Exists(context.Server.MapPath(file)))
            {
                context.Response.Clear();
                context.Response.ContentType = "application/octet-stream";
                //I have set the ContentType to "application/octet-stream" which cover any type of file
                context.Response.AddHeader("content-disposition", "attachment;filename=" + Path.GetFileName(file));
                context.Response.WriteFile(context.Server.MapPath(file));
                //here you can do some statistic or tracking
                //you can also implement other business request such as delete the file after download
                context.Response.End();
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("File not be found!");
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "PayRoll", "alert('File not be found!'); ", true);
                //return;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}