using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace PhotoBeamerClone.Web
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var guid = Guid.NewGuid();
            HttpContext.Current.Session["InstanceGuid"] = guid.ToString();
        }
    }
}