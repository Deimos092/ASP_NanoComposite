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
                //SubscriptionModel s = new SubscriptionModel();
                //s.NumberOfShared = 1;
                //cont.SubModel.Add(s);

                User user = new User() { Login = "Test", SubModel = new SubscriptionModel() };
                User user2 = new User() { Login = "Test222", SubModel = new SubscriptionModel() };
                Project project = new Project() { ProjectDate = DateTime.Now, ProjectDescription = "randomDesc", ProjectName = "Test", SharedTo = new List<Share>(), UsedMaterials = new List<Material>() };
                Material material = new Material() { Owner = user, Name = "AnyMaterial" };
                Material material2 = new Material() { Owner = user, Name = "123" };

                Share share = new Share() { ProjectToShare = project, Shared = user };
                Share share2 = new Share() { ProjectToShare = project, Shared = user2 };

                project.SharedTo.Add(share);
                project.SharedTo.Add(share2);

                project.UsedMaterials.Add(material);
                project.UsedMaterials.Add(material2);

                cont.Users.Add(user);
                cont.Users.Add(user2);
                cont.Projects.Add(project);
                cont.Shares.Add(share);
                cont.Shares.Add(share2);
                cont.Materials.Add(material);
                cont.Materials.Add(material2);
                cont.SaveChanges();

                //var proj = cont.Projects.First();
                //var t = proj.UsedMaterials;
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