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

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //using (ApplicationDbContext c = new ApplicationDbContext())
            //{
            //    c.Database.CommandTimeout = 600;
            //    c.Database.CreateIfNotExists();
            //}
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