using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MovieImdb.Startup))]
namespace MovieImdb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
