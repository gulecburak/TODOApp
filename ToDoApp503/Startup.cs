using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ToDoApp503.Startup))]
namespace ToDoApp503
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
