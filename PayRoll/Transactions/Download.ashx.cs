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
            string file1 = context.Request.QueryString["file"];
            string file = context.Server.MapPath(file1);

            if (!string.IsNullOrEmpty(file) && File.Exists(file))
            {
                context.Response.Clear();
                //context.Response.ContentType = "application/octet-stream";
                context.Response.ContentType = "application/vnd.ms-excel";
                
                //I have set the ContentType to "application/octet-stream" which cover any type of file
                context.Response.AddHeader("content-disposition", "attachment;filename=" + Path.GetFileName(file));
                context.Response.WriteFile(file);
                //here you can do some statistic or tracking
                //you can also implement other business request such as delete the file after download
                context.Response.End();
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("File not be found!");
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