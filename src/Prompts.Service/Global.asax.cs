using System;
using System.Web;
using System.Web.Routing;

namespace Prompts.Service
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            (new PromptsServiceAppHost()).Init();
            RegisterRoutes(RouteTable.Routes);
        }

        void RegisterRoutes(RouteCollection routes)
        {
            routes.Add(

            new Route("{resource}.axd/{*pathInfo}", new StopRoutingHandler()));  
            routes.MapPageRoute("", "{data}", "~/WELCOME.aspx");
        }
    }
}