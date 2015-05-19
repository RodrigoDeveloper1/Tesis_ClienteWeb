using Owin;
using System.Web.Mvc;
using Tesis_ClienteWeb.App_Start;

namespace Tesis_ClienteWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Tesis_ClienteWeb.App_Start.Startup.ConfigureAuth(app);
        }
    }
}