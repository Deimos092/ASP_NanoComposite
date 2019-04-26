using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebApplication1.Models;
using System.Collections.Generic;
namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (ApplicationDbContext c = new ApplicationDbContext())
            {
                //c.Database.CommandTimeout = 600;
                //c.Database.CreateIfNotExists();
                //if (!c.SubModel.Any(x=>x.SubscriptionModelID==1))
                //{
                //    c.SubModel.Add(new SubscriptionModel() { Name = "Бесплатная", Description = "123", NumberOfProj = 1, NumberOfShared = 0, SubCost = 0, SubscriptionModelID = 1 });
                //    c.SaveChanges();
                //}
                //var co = new Composite();
                //Material m = new Material() { Density = 0 };
                //Material m2 = new Material() { Density = 12 };
                //co.UsedMaterials = new List<UsedMaterial>();
                //UsedMaterial usedMaterial1 = new UsedMaterial() { Material = m, Percent = 90, isMatrix = true };
                //UsedMaterial usedMaterial2 = new UsedMaterial() { Material = m2, Percent = 10, isMatrix = false, isMassPercent = true };

                //c.UsedMaterial.Add(usedMaterial1);
                //c.UsedMaterial.Add(usedMaterial2);

                //co.UsedMaterials.Add(usedMaterial1);
                //co.UsedMaterials.Add(usedMaterial2);

                //c.Composits.Add(co);
                //c.Materials.Add(m);
                //c.Materials.Add(m2);
                //Project p = new Project() { ProjectName = "123", ProjectDate = DateTime.Now };
                //c.Projects.Add(p);

                ////Material m = new Material() { Density = 1 };
                ////c.Composits.Where(k => k.CompositeID == 1).First().Materials.Add(m);
                //c.SaveChanges();
            }
            return View();
        }
        

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            using (ApplicationDbContext c = new ApplicationDbContext())
            {
                var id = User.Identity.GetUserId();
                var t = c.Users.Where(u => u.Id == id);
                ViewBag.Message = "Your contact page. " + t.First().UserName;
            }
            

            return View();
        }
    }
}