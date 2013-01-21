using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.SessionState;

namespace PhotoBeamerClone.Web
{
    /// <summary>
    /// Summary description for ByteArrayHandler
    /// </summary>
    public class ByteArrayHandler : IHttpHandler, IRequiresSessionState 
    {

        public void ProcessRequest(HttpContext context)
        {
            string base64String = HttpContext.Current.Session["ByteArray"].ToString();
            byte[] convertedFromBase64 = Convert.FromBase64String(base64String);

            context.Response.OutputStream.Write(convertedFromBase64, 0, convertedFromBase64.Length);
            context.Response.ContentType = "image/JPEG";
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