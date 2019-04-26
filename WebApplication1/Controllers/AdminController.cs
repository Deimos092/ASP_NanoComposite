using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using Newtonsoft.Json;
namespace WebApplication1.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            using (WebApplication1.Models.ApplicationDbContext c = new WebApplication1.Models.ApplicationDbContext())
            {
                dynamic subs = new ExpandoObject();
                subs.x = new List<string>();
                subs.y = new List<int>();
                foreach (var item in c.SubModel.ToList())
                {
                    var count = c.Users.Count(z => z.SubModel.SubscriptionModelID == item.SubscriptionModelID);
                    subs.x.Add(item.Name);
                    subs.y.Add(count);
                }
                dynamic subsDate = new ExpandoObject();
                subsDate.x = new List<string>();
                subsDate.y = new List<int>();
                foreach (var item in c.Users.ToList())
                {
                    int i = subsDate.x.IndexOf(item.SubscriptionStartDate.Date.ToShortDateString());
                    if (i==-1)
                    {
                        subsDate.x.Add(item.SubscriptionStartDate.Date.ToShortDateString());
                        subsDate.y.Add(1);
                    }
                    else
                    {
                        subsDate.y[i] += 1;
                    }
                    
                }
                ViewBag.Subs = JsonConvert.SerializeObject(subs);
                ViewBag.SubsDate = JsonConvert.SerializeObject(subsDate);
            }
            return View();
        }
    }
}