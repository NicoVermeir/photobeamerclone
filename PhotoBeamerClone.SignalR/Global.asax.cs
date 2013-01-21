using System;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;

namespace PhotoBeamerClone.SignalR
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AuthConfig.RegisterOpenAuth();

            GlobalHost.HubPipeline.EnableAutoRejoiningGroups();

            // Register the default hubs route: ~/signalr/hubs
            RouteTable.Routes.MapHubs();
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }
    }
}
