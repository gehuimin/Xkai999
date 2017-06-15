using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyBlog.Models;

namespace MyBlog.Controllers
{
    [Authorize]
    public class PraiseController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Praise
        public ActionResult Index()
        {
            string userId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;

            var praises = db.Praises.Include(p => p.Blog).Include(p => p.FromUser).Where(x => x.FromUserId == userId)
                .Union(db.Praises.Include(p => p.Blog).Include(p => p.FromUser).Where(x => x.Blog.Bloger.Id == userId));

            return View(praises.ToList());
        }

        // GET: Praise/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Praise praise = db.Praises.Find(id);
            if (praise == null)
            {
                return HttpNotFound();
            }
            return View(praise);
        }

        public JsonResult AddPraise(string fromUserId, int blogId)
        {
            Praise praise = db.Praises.FirstOrDefault(p => p.FromUserId == fromUserId && p.BlogId == blogId);

            if(praise == null)
            {
                praise = new Praise { FromUserId = fromUserId, BlogId = blogId, PraisedTime = DateTime.Now };
                try
                {
                    db.Praises.Add(praise);
                    db.SaveChanges();

                    return Json(new { result = true });
                }
                catch
                {
                    return Json(new { result = false });
                }
            }else
            {
                return Json(new { result = true });
            }
        }

        // GET: Praise/Create
        public ActionResult Create()
        {
            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "BlogTitle");
            ViewBag.FromUserId = new SelectList(db.Users, "Id", "NickName");
            return View();
        }

        // POST: Praise/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PraiseId,FromUserId,BlogId,PraisedTime")] Praise praise)
        {
            if (ModelState.IsValid)
            {
                db.Praises.Add(praise);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "BlogTitle", praise.BlogId);
            ViewBag.FromUserId = new SelectList(db.Users, "Id", "NickName", praise.FromUserId);
            return View(praise);
        }

        // GET: Praise/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Praise praise = db.Praises.Find(id);
            if (praise == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "BlogTitle", praise.BlogId);
            ViewBag.FromUserId = new SelectList(db.Users, "Id", "NickName", praise.FromUserId);
            return View(praise);
        }

        // POST: Praise/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PraiseId,FromUserId,BlogId,PraisedTime")] Praise praise)
        {
            if (ModelState.IsValid)
            {
                db.Entry(praise).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "BlogTitle", praise.BlogId);
            ViewBag.FromUserId = new SelectList(db.Users, "Id", "NickName", praise.FromUserId);
            return View(praise);
        }

        // GET: Praise/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Praise praise = db.Praises.Find(id);
            if (praise == null)
            {
                return HttpNotFound();
            }
            return View(praise);
        }

        // POST: Praise/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Praise praise = db.Praises.Find(id);
            db.Praises.Remove(praise);
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
    }
}
