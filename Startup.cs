using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(E_Appraisal.Startup))]
namespace E_Appraisal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
