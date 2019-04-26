using Microsoft.Owin;
using Owin;
using WebApplication1.Models;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(WebApplication1.Startup))]
namespace WebApplication1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            using (ApplicationDbContext c = new ApplicationDbContext())
            {
                c.Database.CommandTimeout = 600;
                c.Database.CreateIfNotExists();
                if (!c.SubModel.Any(x => x.SubscriptionModelID == 1))
                {
                    c.SubModel.Add(new SubscriptionModel() { Name = "Бесплатная", Description = "", NumberOfProj = 1, NumberOfShared = 0, SubCost = 0, SubscriptionModelID = 1 });
                    c.SaveChanges();
                }
            }
        }
    }
}
