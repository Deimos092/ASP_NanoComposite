using ASP_NanoComposite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using CodeFirst;

namespace ASP_NanoComposite.Controllers
{
	public class HomeController:Controller
	{
		public ActionResult Index()
		{
            using (Context cont = new Context())
            {
                /*User u = new User();
                u.SubModel = new SubscriptionModel();
                u.Login = "Test";
                cont.Users.Add(u);
                cont.SaveChanges();*/
                /*var u = cont.Users.Where(e => e.Login == "Test");
                foreach (var item in u)
                {
                    item.SubModel.NumberOfProj = 5;
                }*/
                SubscriptionModel s = new SubscriptionModel();
                s.NumberOfShared = 1;
                cont.SubModel.Add(s);
                cont.SaveChanges();
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
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}