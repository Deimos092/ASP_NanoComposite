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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string name = User.Identity.Name;
            var user = db.Users.Where(x => x.UserName == name).First();
            if (user.SubModel.SubscriptionModelID!=1 && user.SubscriptionEndDate<DateTime.Now)
            {
                user.SubscriptionStartDate = DateTime.Now;
                user.SubModel = db.SubModel.Where(x => x.SubscriptionModelID == 1).First();
                db.SaveChanges();
            }
            base.OnActionExecuting(filterContext);
        }

        // GET: ProjectsTest
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).First();
            var temp = db.Projects.Where(u => u.Owner.Id==user.Id || db.Shares.Any(s=>s.Shared.Id==user.Id && u.ProjectID==s.ProjectToShare.ProjectID)).ToList();
            ViewBag.User = user;
            return View(temp);
        }
        public bool isInProject(int projectID, bool edit)
        {
            if (db.Projects.Any(x=>x.Owner.UserName== User.Identity.Name && x.ProjectID==projectID))
            {
                return true;
            }
            if (edit)
            {
                if (db.Shares.Any(x => x.ProjectToShare.ProjectID == projectID && x.Shared.UserName == User.Identity.Name && x.isWrite))
                {
                    return true;
                }
            }
            else
            {
                if (db.Shares.Any(x => x.ProjectToShare.ProjectID == projectID && x.Shared.UserName == User.Identity.Name))
                {
                    return true;
                }
            }
            return false;
        }
        public bool isMaterialOwner(int materialId)
        {
            if (db.Materials.Any(x => x.MaterialID == materialId && x.Owner.UserName == User.Identity.Name))
            {
                return true;
            }
            return false;
        }
        public bool isVerified(int compId, bool edit)
        {
            var t = db.Projects.Where(p => p.UsedComposits.Any(x => x.CompositeID == compId)).First();
            if (edit)
            {
                if (db.Composits.Any(x => x.CompositeID == compId && db.Shares.Any(z => z.ProjectToShare.ProjectID == t.ProjectID && z.Shared.UserName == User.Identity.Name && z.isWrite)))
                {
                    return true;
                }
            }
            else
            {
                if (db.Composits.Any(x => x.CompositeID == compId && db.Shares.Any(z => z.ProjectToShare.ProjectID == t.ProjectID && z.Shared.UserName == User.Identity.Name)))
                {
                    return true;
                }
            }
            return false;
        }
        // GET: ProjectsTest/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!isInProject(id.Value,false)) return RedirectToAction("Index");
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
                if (project.Owner.SubModel.NumberOfProj>=db.Projects.Where(x=>x.Owner.Id==project.Owner.Id).Count())
                {
                    return RedirectToAction("Index");
                }
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
            if (!isInProject(id.Value,true)) return RedirectToAction("Index");
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
        public ActionResult Edit([Bind(Include = "ProjectID,ProjectName,ProjectDescription,ProjectDate")] Project project)
        {
            if (project.ProjectName==null)
            {
                project.ProjectName = "Без названия";
            }
            if (!isInProject(project.ProjectID,true)) return RedirectToAction("Index");
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
            if (!isInProject(id.Value,true)) return RedirectToAction("Index");
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
            if (!isInProject(id,true)) return RedirectToAction("Index");
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
            if(!isInProject(id,false)) return RedirectToAction("Index");
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
            if (!isInProject(projectId,true)) return RedirectToAction("Index");
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
            if (!isInProject(projectId,true)) return RedirectToAction("Index");
            List<temp> list = JsonConvert.DeserializeObject<List<temp>>(json);
            Composite comp;
            if (composite.CompositeID == 0)
            {
                comp = new Composite();
                comp.UsedMaterials = new List<UsedMaterial>();
            }
            else
            {
                comp = db.Composits.Where(x => x.CompositeID == composite.CompositeID).First();
            }
            comp.Elasticity = composite.Elasticity;
            comp.FactorKogezia = composite.FactorKogezia;
            comp.Hardness = composite.Hardness;
            comp.Name = composite.Name;
            comp.Porosity = composite.Porosity;
            comp.Strength = composite.Strength;
            comp.ThermalConduct = composite.ThermalConduct;
            foreach (var item in list)
            {
                UsedMaterial m;
                var ml = comp.UsedMaterials.Where(x => x.Material.MaterialID == item.id);
                if (ml.Count() != 0)
                {
                    m = ml.First();
                    m.isMassPercent = item.isMassPercent;
                    m.isMatrix = item.isMatrix;
                    m.Percent = item.percent;
                    m.Material = db.Materials.Where(a => a.MaterialID == item.id).First();
                    m.Composite_ = comp;
                    db.Entry(m).State = EntityState.Modified;
                }
                else
                {
                    m = new UsedMaterial();
                    m.isMassPercent = item.isMassPercent;
                    m.isMatrix = item.isMatrix;
                    m.Percent = item.percent;
                    m.Material = db.Materials.Where(a => a.MaterialID == item.id).First();
                    m.Composite_ = comp;
                    db.UsedMaterial.Add(m);
                    comp.UsedMaterials.Add(m);
                }
            }
            for (int i = 0; i < comp.UsedMaterials.Count; i++)
            {
                if (!list.Any(x=>x.id== comp.UsedMaterials.ElementAt(i).Material.MaterialID))
                {
                    db.UsedMaterial.Remove(comp.UsedMaterials.ElementAt(i));
                    i--;
                }
            }


            db.Entry(comp).State = EntityState.Modified;
            Project p = db.Projects.Where(m => m.ProjectID == projectId).First();
            if (p.UsedComposits.Where(m=>m.CompositeID== comp.CompositeID).Count()==0)
            {
                p.UsedComposits.Add(comp);
            }
            if (comp.CompositeID==0)
            {
                db.Composits.Add(comp);
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
            if (!isMaterialOwner(MaterialID)) return Json("", JsonRequestBehavior.AllowGet);
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
        public JsonResult AddMaterial(Material material, string uId, int idToEdit)
        {
            if (idToEdit == -1)
            {
                Material tmp = material;
                User tmp_u = db.Users.Where(u => u.Id == uId).First();
                tmp.Owner = tmp_u;
                db.Materials.Add(tmp);
                db.SaveChanges();
                int newId = db.Materials.Max(m => m.MaterialID);
                return Json(newId, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (!isMaterialOwner(idToEdit)) return Json("", JsonRequestBehavior.AllowGet);
                Material toEdit = db.Materials.Where(m => m.MaterialID == idToEdit).First();
                toEdit.Name = material.Name;
                toEdit.Hardness = material.Hardness;
                toEdit.Elasticity = material.Elasticity;
                toEdit.StrengthBeyond = material.StrengthBeyond;
                toEdit.Density = material.Density;
                toEdit.HeatCapacity = material.HeatCapacity;
                toEdit.ThermalConduct = material.ThermalConduct;
                toEdit.ThermalExpansion = material.ThermalExpansion;
                db.SaveChanges();
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Сюда стучится ajax для того чтобы удалить композит
        /// </summary>
        /// <param name="CompositeID"></param>
        /// <returns></returns>
        public JsonResult DeleteComposite(int CompositeID)
        {
            if (!isVerified(CompositeID, true)) return Json("", JsonRequestBehavior.AllowGet);
            Composite comp = db.Composits.Where(m => m.CompositeID == CompositeID).First();
            var used = db.UsedMaterial.Where(u => u.Composite_.CompositeID == comp.CompositeID);
            foreach (var uMt in used)
                db.UsedMaterial.Remove(uMt);
            db.Composits.Remove(comp);
            db.SaveChanges();
            return Json(comp.Name, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Проверка на существование пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JsonResult CheckForUser(string user, string owner)
        {
            var usr = db.Users.Where(u => u.Email == user && u.EmailConfirmed == true && u.Id != owner);
            if (usr.Count() > 0)
                return Json(true, JsonRequestBehavior.AllowGet);
            else return Json(false, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Добавление юзера в проект
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isWrite"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public JsonResult AddUserInProject(string user,bool isWrite , int project)
        {
            if (!isInProject(project,true)) return Json(false, JsonRequestBehavior.AllowGet);

            var Project = db.Projects.Where(p => p.ProjectID == project).First();
            if (Project.Owner.SubModel.NumberOfShared<=Project.SharedTo.Count)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            dynamic ret = new ExpandoObject();
            //
            //нужно для проверок на повторения, и на создателя проекта
            var usr = db.Users.Where(u => u.Email == user && u.EmailConfirmed == true && u.Id != Project.Owner.Id).ToList();
            var inShare = Project.SharedTo.Where(s => s.Shared.Email == user).ToList();
            //
            if (usr.Count() > 0 && inShare.Count() < 1)
            {
                Share nShare = new Share();
                nShare.Shared = usr.ElementAt(0);
                nShare.ProjectToShare = Project;
                nShare.isWrite = isWrite;
                nShare.isRead = true;
                Project.SharedTo.Add(nShare);
                db.SaveChanges();

                ret.res = true;
                ret.usr = usr.ElementAt(0).Id;
                ret.newShId = db.Shares.Max(s => s.ShareID);
                ret.isE = isWrite;
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ret.res = false;
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// удаление пользователя из бд
        /// </summary>
        /// <param name="user">id юзера</param>
        /// <param name="projectID">id проекта</param>
        /// <returns></returns>
        //сделать обработчик на то что просто так нажали кнопку delete
        public JsonResult DeleteUserFromProject(string user, int projectID)
        {
            if (!isInProject(projectID,true)) return Json(false, JsonRequestBehavior.AllowGet);
            var duser = db.Users.Where(u => u.Id == user).First();
            var project = db.Projects.Where(p => p.ProjectID == projectID).First();
            var share = db.Shares.Where(s => s.Shared.Id == duser.Id && s.ProjectToShare.ProjectID == project.ProjectID).First();
            project.SharedTo.Remove(share);
            db.Shares.Remove(share);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CalculateAll(int id)
        {
            Composite TmpComposite = db.Composits.Where(x => x.CompositeID == id).First(); 
            string[] props = new string[7] { "Elasticity", "ThermalExpansion", "Hardness", "StrengthBeyond", "Density", "HeatCapacity", "ThermalConduct" };
            string[] rus = new string[7] { "E - Модуль упругости, МПа", "α - коэф.термического расширения", "HB - Твердость, МПа", "σ^2 - Предел прочности, МПа", "p - Плотность, кг/м^3", "Cp - Теплоемкость, Дж/кг*К", "λ - Теплопроводность, Вт/м*град" };
            dynamic ret = new ExpandoObject();
            ret.props = new ExpandoObject[props.Length];
            for (int i = 0; i < props.Length; i++)
            {
                ret.props[i] = new ExpandoObject();
                ret.props[i].name = props[i];
                ret.props[i].x = new decimal[101];
                ret.props[i].y = new decimal[101];
                ret.props[i].rus = rus[i];
                for (int j = 0; j <= 100; j++)
                {
                    ret.props[i].x[j] = j;
                    ret.props[i].y[j] = CalculateParamsForData(props[i], j/100M, TmpComposite);
                }
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Calculate(int id, decimal porosity)
        {
            Composite TmpComposite = db.Composits.Where(x => x.CompositeID == id).First();
            string[] props = new string[7] { "Elasticity", "ThermalExpansion", "Hardness", "StrengthBeyond", "Density", "HeatCapacity", "ThermalConduct" };
            dynamic ret = new ExpandoObject();
            ret.props = props;
            ret.values = new decimal[7];
            for (int i = 0; i < props.Length; i++)
            {
                ret.values[i] = CalculateParamsForData(props[i], porosity/100M, TmpComposite);
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        public decimal CalculateParamsForData(string KeyNames, decimal porosity, Composite TmpComposite)
        {
            Dictionary<string, decimal> CalcResult = new Dictionary<string, decimal>();
            CalcResult[KeyNames] = 0;
            switch (KeyNames)
            {
                case "Elasticity":
                    foreach (UsedMaterial material in TmpComposite.UsedMaterials)
                        CalcResult[KeyNames] = CalcPercent(material.Percent * 0.01M, KeyNames, CalcResult, material.Material.Elasticity);
                    return CalcResult[KeyNames] * (decimal)Math.Exp((double)-TmpComposite.Elasticity * (double)porosity);
                case "ThermalExpansion":
                    decimal tempDivision = 0;
                    foreach (UsedMaterial material in TmpComposite.UsedMaterials)
                    {
                        CalcResult[KeyNames] = CalcPercent(material.Percent * 0.01M, KeyNames, CalcResult, material.Material.ThermalExpansion, material.Material.ThermalConduct);
                        tempDivision += material.Percent * material.Material.ThermalConduct;
                    }
                    CalcResult[KeyNames] /= tempDivision;
                    var temp = Math.Pow(1.0 - (double)porosity, 1.0 / 3.0);
                    var temp2 = (decimal)temp * 10;
                    return CalcResult[KeyNames] * temp2;
                case "Hardness":
                    foreach (UsedMaterial material in TmpComposite.UsedMaterials)
                        CalcResult[KeyNames] = CalcPercent(material.Percent * 0.01M, KeyNames, CalcResult, material.Material.Hardness);
                    return CalcResult[KeyNames] * (decimal)Math.Exp((double)-TmpComposite.Hardness * (double)porosity);
                case "StrengthBeyond":
                    foreach (UsedMaterial material in TmpComposite.UsedMaterials)
                        CalcResult[KeyNames] = CalcPercent(material.Percent * 0.01M, KeyNames, CalcResult, material.Material.StrengthBeyond);
                    if (TmpComposite.UsedMaterials.Count == 4)
                    {
                        var tempMatrix = TmpComposite.UsedMaterials.Where(x => x.isMatrix).First();
                        CalcResult[KeyNames] = tempMatrix.Material.StrengthBeyond *
                                                (decimal)(1.0 - 1.21 * (double)Math.Pow(1.0 - (double)tempMatrix.Percent / 100.0, 1.0 / 3.0))
                                                * TmpComposite.FactorKogezia;
                    }
                    return CalcResult[KeyNames] * (decimal)Math.Exp((double)-TmpComposite.Strength * (double)porosity);
                case "Density":
                    foreach (UsedMaterial material in TmpComposite.UsedMaterials)
                        CalcResult[KeyNames] = CalcPercent(material.Percent * 0.01M, KeyNames, CalcResult, material.Material.Density);
                    return CalcResult[KeyNames] * (1 - porosity);
                case "HeatCapacity":
                    decimal tempForthermalCapacity = 1;
                    foreach (UsedMaterial material in TmpComposite.UsedMaterials)
                    {
                        CalcResult[KeyNames] = CalcPercent(material.Percent * 0.01M, KeyNames, CalcResult, material.Material.HeatCapacity);
                        if (material.isMatrix) tempForthermalCapacity *= 0.2M * material.Percent * 0.01M;
                        else tempForthermalCapacity *= material.Percent * 0.01M;
                    }
                    return CalcResult[KeyNames] *= (1.0M + tempForthermalCapacity);
                case "ThermalConduct":
                    foreach (UsedMaterial material in TmpComposite.UsedMaterials)
                        CalcResult[KeyNames] = CalcPercent(material.Percent * 0.01M, KeyNames, CalcResult, material.Material.ThermalConduct);
                    return CalcResult[KeyNames] * (decimal)Math.Exp((double)-TmpComposite.ThermalConduct * (double)porosity) * 10;
                default:
                    break;
            }
            return 0.0M;
        }
        private decimal CalcPercent(decimal percent, string KeyNames, Dictionary<string, decimal> CalcResult, params decimal[] property)
        {
            if (KeyNames == "ThermalExpansion")
                return CalcResult[KeyNames] + property[0] * percent * property[1];//Для него немного по другому считается поэтому условие
            else
                return CalcResult[KeyNames] + property[0] * percent;
        }
    }
}
