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
    public class AttentionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Attention
        public ActionResult Index()
        {
            string userId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;

            var attentions = db.Attentions.Include(a => a.FromUser).Include(a => a.ToUser)
                .Where(a => a.FromUserId == userId)
                .Union(db.Attentions.Include(a => a.FromUser).Include(a => a.ToUser).Where(a => a.ToUserId == userId));

            return View(attentions.ToList());
        }

        // GET: Attention/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attention attention = db.Attentions.Find(id);
            if (attention == null)
            {
                return HttpNotFound();
            }
            return View(attention);
        }


        public JsonResult AddAttention(string fromUserId, string toUserId)
        {
            Attention attention = db.Attentions.FirstOrDefault(a => a.FromUserId == fromUserId && a.ToUserId == toUserId);

            if(attention == null)
            {
                attention = new Attention { FromUserId = fromUserId, ToUserId = toUserId, StartTime = DateTime.Now };
                try
                {
                    db.Attentions.Add(attention);
                    db.SaveChanges();

                    return Json(new { result = true });
                }
                catch
                {
                    return Json(new { result = false });
                }
            }
            else
            {
                return Json(new { result = true });
            }
        }

        public JsonResult RemoveAttention(string fromUserId, string toUserId)
        {
            Attention attention = db.Attentions.FirstOrDefault(a => a.FromUserId == fromUserId && a.ToUserId == toUserId);

            if(attention != null)
            {
                try
                {
                    db.Attentions.Remove(attention);
                    db.SaveChanges();

                    return Json(new { result = true });
                }
                catch
                {
                    return Json(new { result = false });
                }
            }
            else
            {
                return Json(new { result = true });
            }
        }


        // GET: Attention/Create
        public ActionResult Create()
        {
            ViewBag.FromUserId = new SelectList(db.Users, "Id", "NickName");
            ViewBag.ToUserId = new SelectList(db.Users, "Id", "NickName");
            return View();
        }

        // POST: Attention/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AttentionId,FromUserId,ToUserId,StartTime")] Attention attention)
        {
            if (ModelState.IsValid)
            {
                db.Attentions.Add(attention);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FromUserId = new SelectList(db.Users, "Id", "NickName", attention.FromUserId);
            ViewBag.ToUserId = new SelectList(db.Users, "Id", "NickName", attention.ToUserId);
            return View(attention);
        }

        // GET: Attention/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attention attention = db.Attentions.Find(id);
            if (attention == null)
            {
                return HttpNotFound();
            }
            ViewBag.FromUserId = new SelectList(db.Users, "Id", "NickName", attention.FromUserId);
            ViewBag.ToUserId = new SelectList(db.Users, "Id", "NickName", attention.ToUserId);
            return View(attention);
        }

        // POST: Attention/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AttentionId,FromUserId,ToUserId,StartTime")] Attention attention)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attention).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FromUserId = new SelectList(db.Users, "Id", "NickName", attention.FromUserId);
            ViewBag.ToUserId = new SelectList(db.Users, "Id", "NickName", attention.ToUserId);
            return View(attention);
        }

        // GET: Attention/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attention attention = db.Attentions.Find(id);
            if (attention == null)
            {
                return HttpNotFound();
            }
            return View(attention);
        }

        // POST: Attention/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Attention attention = db.Attentions.Find(id);
            db.Attentions.Remove(attention);
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
