using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using Newtonsoft.Json;
using System.Dynamic;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class ProjectsTestController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProjectsTest
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).First();
            var temp = db.Projects.Where(u => u.Owner.Id==user.Id).ToList();
            return View(temp);
        }

        // GET: ProjectsTest/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: ProjectsTest/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProjectsTest/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectID,ProjectName,ProjectDescription,ProjectDate")] Project project)
        {
            if (ModelState.IsValid)
            {
                    var name = User.Identity.Name;
                    project.Owner = db.Users.Where(u => u.UserName == name).First();
                    project.ProjectDate = DateTime.Now;
                    db.Projects.Add(project);
                    db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: ProjectsTest/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: ProjectsTest/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectID,ProjectName,ProjectDescription")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: ProjectsTest/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: ProjectsTest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ProjectContent(int id)
        {
            Project project = db.Projects.Find(id);
            return View(project);
        }
        /// <summary>
        /// для перехода на страницу создания композита
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="compositeId"></param>
        /// <returns></returns>
        public ActionResult AddComposite(int projectId, int? compositeId)
        {
            Composite composite;
            ViewBag.ProjectId = projectId;
            if (compositeId.HasValue)
            {
                
                composite = db.Composits.Where(c => c.CompositeID == compositeId.Value).First();
                composite.UsedMaterials = db.UsedMaterial.Where(c => c.Composite_.CompositeID == compositeId.Value).ToList();
            }
            else
            {
                composite = new Composite();
                composite.UsedMaterials = new List<UsedMaterial>();
            }
            
            return View(composite);
        }

        /// <summary>
        /// Класс для временного размещения данных с json
        /// </summary>
        public class temp
        {
            public int id;
            public decimal percent;
            public bool isMassPercent;
            public bool isMatrix;
        }
        /// <summary>
        /// Добавление композита
        /// </summary>
        /// <param name="composite"></param>
        /// <param name="json"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddComposite(Composite composite, string json, int projectId)
        {
            List<temp> list = JsonConvert.DeserializeObject<List<temp>>(json);
            foreach (var item in db.UsedMaterial.Where(m => m.Composite_.CompositeID == composite.CompositeID))
            {
                db.UsedMaterial.Remove(item);
            }
            composite.UsedMaterials = new List<UsedMaterial>();
            foreach (var item in list)
            {
                UsedMaterial m = new UsedMaterial();
                m.isMassPercent = item.isMassPercent;
                m.isMatrix = item.isMatrix;
                m.Percent = item.percent / 100;
                m.Material = db.Materials.Where(a => a.MaterialID == item.id).First();
                m.Composite_ = composite;
                db.UsedMaterial.Add(m);
                composite.UsedMaterials.Add(m);
            }
            db.Entry(composite).State = EntityState.Modified;
            Project p = db.Projects.Where(m => m.ProjectID == projectId).First();
            if (p.UsedComposits.Where(m=>m.CompositeID==composite.CompositeID).Count()==0)
            {
                p.UsedComposits.Add(composite);
            }
            if (composite.CompositeID==0)
            {
                db.Composits.Add(composite);
            }
            db.SaveChanges();          
            return RedirectToAction("ProjectContent", new { id = projectId });
        }
        /// <summary>
        /// Переход на страницу управления материалами
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageMaterials(string ownerID)
        {
            dynamic mymodel = new ExpandoObject();
            User usr = db.Users.Where(u => u.Id == ownerID).First();
            var mats = db.Materials.Where(m => m.Owner.Id == ownerID);
            mymodel.User = usr;
            mymodel.Materials = mats;
            return View(mymodel);
        }
        /// <summary>
        /// Сюда стучится ajax для того чтобы удалить материал
        /// </summary>
        /// <param name="MaterialID"></param>
        /// <returns></returns>
        public JsonResult DeleteMaterial(int MaterialID)
        {
            Material mt = db.Materials.Where(m => m.MaterialID == MaterialID).First();
            var usedMts = db.UsedMaterial.Where(um => um.Material.MaterialID == mt.MaterialID);
            //удаление из UsedMaterials чтобы не было ошибки
            if(usedMts.Count() != 0)
                foreach (var uMt in usedMts)
                    db.UsedMaterial.Remove(uMt);
            db.Materials.Remove(mt);
            db.SaveChanges();
            return Json(mt.Name, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Добавление материала через ajax
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public JsonResult AddMaterial(Material material, string uId)
        {
            Material tmp = material;
            User tmp_u = db.Users.Where(u => u.Id == uId).First();
            tmp.Owner = tmp_u;
            db.Materials.Add(tmp);
            db.SaveChanges();
            int newId = db.Materials.Max(m => m.MaterialID);
            return Json(newId, JsonRequestBehavior.AllowGet);
        }
    }
}
