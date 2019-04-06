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
                cont.Users.Add(user);
                cont.SaveChanges();

                Project project = new Project() { ProjectDate = DateTime.Now, ProjectDescription = "randomDesc", ProjectName = "Test" };
                cont.Projects.Add(project);
                cont.SaveChanges();

                Material material = new Material() { Owner = user, Name = "AnyMaterial" };
                cont.Materials.Add(material);
                cont.SaveChanges();

                Material material2 = new Material() { Owner = user, Name = "123" };
                cont.Materials.Add(material2);
                cont.SaveChanges();

                ProjectMaterials projectMaterials = new ProjectMaterials() { Material = new List<Material>(), Project = project };
                projectMaterials.Material.Add(material);
                projectMaterials.Material.Add(material2);
                cont.ProjectMaterials.Add(projectMaterials);
                cont.SaveChanges();

                Share share = new Share() { Owner = user, Shared = new List<User>(), ProjectToShare = project };
                cont.Shares.Add(share);
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