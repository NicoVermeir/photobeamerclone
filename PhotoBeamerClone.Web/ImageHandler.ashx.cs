using System;
using System.Net;
using System.Web;
using System.Web.SessionState;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace PhotoBeamerClone.Web
{
    /// <summary>
    /// Summary description for ImageHandler
    /// </summary>
    public class ImageHandler : IHttpHandler, IRequiresSessionState 
    {
        public void ProcessRequest(HttpContext context)
        {
            var guid = HttpContext.Current.Session["InstanceGuid"];

            string url = string.Format(@"http://api.qrserver.com/v1/create-qr-code/?size=300x300&data={0}", guid);

            WebClient client = new WebClient();
            var bytes = client.DownloadData(new Uri(url));

            context.Response.OutputStream.Write(bytes, 0, bytes.Length);
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