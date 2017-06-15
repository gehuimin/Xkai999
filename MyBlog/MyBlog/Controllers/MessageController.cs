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
    public class MessageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Message
        public ActionResult Index()
        {
            string myUserId = User.Identity.GetUserId();

            var messages = db.Messages.Where(m => m.FromUserId.Equals(myUserId))
                .Union(db.Messages.Where(m => m.ToUserId.Equals(myUserId)));

            return View(messages.OrderByDescending(o => o.CreatedTime).ToList());
        }

        // GET: Message/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: Message/Create
        public ActionResult Create(string id, string fromUserId)
        {
            ViewBag.FromUserId = fromUserId;

            var toUserList = db.Users.ToList();
            toUserList.Remove(db.Users.SingleOrDefault(u => u.Id == fromUserId));

            ViewBag.ToUserId = new SelectList(toUserList, "Id", "NickName", id);

            return View();
        }

        // POST: Message/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageId,FromUserId,ToUserId,MsgContent,CreatedTime")] Message message)
        {
            message.CreatedTime = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FromUserId = new SelectList(db.Users, "Id", "NickName", message.FromUserId);
            ViewBag.ToUserId = new SelectList(db.Users, "Id", "NickName", message.ToUserId);
            return View(message);
        }

        public JsonResult SendMessage([Bind(Include = "MessageId,FromUserId,ToUserId,MsgContent,CreatedTime")] Message message)
        {
            message.CreatedTime = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    db.Messages.Add(message);
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

        // GET: Message/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.FromUserId = new SelectList(db.Users, "Id", "NickName", message.FromUserId);
            ViewBag.ToUserId = new SelectList(db.Users, "Id", "NickName", message.ToUserId);
            return View(message);
        }

        // POST: Message/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MessageId,FromUserId,ToUserId,MsgContent,CreatedTime")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FromUserId = new SelectList(db.Users, "Id", "NickName", message.FromUserId);
            ViewBag.ToUserId = new SelectList(db.Users, "Id", "NickName", message.ToUserId);
            return View(message);
        }

        // GET: Message/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Message/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
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
