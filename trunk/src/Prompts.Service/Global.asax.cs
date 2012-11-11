using System;
using System.Web;

namespace Prompts.Service
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            (new PromptsServiceAppHost()).Init();
        }
    }
}