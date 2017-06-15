using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyBlog.Models;

using Microsoft.AspNet.Identity;

namespace MyBlog.Controllers
{
    [Authorize]
    public class PrivateLetterController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PrivateLetter
        public ActionResult Index()
        {
            string myUserId = User.Identity.GetUserId();

            var letters = db.PrivateLetters.Where(l => l.FromUserId.Equals(myUserId))
                .Union(db.PrivateLetters.Where(l => l.ToUserId.Equals(myUserId)));

            return View(letters.OrderByDescending(o => o.SendedTime).ToList());
        }

        // GET: PrivateLetter/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrivateLetter privateLetter = db.PrivateLetters.Find(id);
            if (privateLetter == null)
            {
                return HttpNotFound();
            }

            privateLetter.IsReaded = true;
            db.SaveChanges();

            return View(privateLetter);
        }

        // GET: PrivateLetter/Create
        public ActionResult Create(string id, string fromUserId )
        {
            var usersList = db.Users.ToList();

            //删除列表中的登录者自己
            usersList.Remove(db.Users.SingleOrDefault(u => u.Id == fromUserId));

            ViewBag.ToUserId = new SelectList(usersList, "Id", "NickName", id);

            ViewBag.FromUserId = fromUserId;

            return View();
        }

        // POST: PrivateLetter/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PrivateLetterId,FromUserId,ToUserId,LetterTitle,LetterContent,SendedTime,IsReaded")] PrivateLetter privateLetter)
        {
            privateLetter.SendedTime = DateTime.Now;
            privateLetter.IsReaded = false;

            if (ModelState.IsValid)
            {
                db.PrivateLetters.Add(privateLetter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(privateLetter);
        }

        public JsonResult SendLetter([Bind(Include = "PrivateLetterId,FromUserId,ToUserId,LetterTitle,LetterContent,SendedTime,IsReaded")] PrivateLetter privateLetter)
        {
            privateLetter.SendedTime = DateTime.Now;
            privateLetter.IsReaded = false;

            if (ModelState.IsValid)
            {
                try
                {
                    db.PrivateLetters.Add(privateLetter);
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
                return Json(new { result = false });
            }
        }

        // GET: PrivateLetter/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrivateLetter privateLetter = db.PrivateLetters.Find(id);
            if (privateLetter == null)
            {
                return HttpNotFound();
            }
            return View(privateLetter);
        }

        // POST: PrivateLetter/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PrivateLetterId,FromUserId,ToUserId,LetterTitle,LetterContent,SendedTime,IsReaded")] PrivateLetter privateLetter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(privateLetter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(privateLetter);
        }

        // GET: PrivateLetter/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrivateLetter privateLetter = db.PrivateLetters.Find(id);
            if (privateLetter == null)
            {
                return HttpNotFound();
            }
            return View(privateLetter);
        }

        // POST: PrivateLetter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PrivateLetter privateLetter = db.PrivateLetters.Find(id);
            db.PrivateLetters.Remove(privateLetter);
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
