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
using Microsoft.AspNet.Identity.EntityFramework;

namespace MyBlog.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comment
        public ActionResult Index()
        {
            string userId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;

            var comments = db.Comments.Include(c => c.Blog).Include(c => c.FromUser)
                .Where(c => c.FromUserId == userId)
                .Union(db.Comments.Include(c => c.Blog).Include(c => c.FromUser).Where(c => c.Blog.Bloger.Id == userId))
                .OrderBy(c => c.CreatedTime);

            return View(comments.ToList());
        }

        // GET: Comment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comment/Create
        public ActionResult Create()
        {
            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "BlogTitle");
            ViewBag.FromUserId = new SelectList(db.Users, "Id", "NickName");
            return View();
        }

        // POST: Comment/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommentId,FromUserId,BlogId,CommContent,CreatedTime")] Comment comment)
        {
            comment.CreatedTime = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Browse", "Blog", new { id = comment.BlogId });
            }

            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "BlogTitle", comment.BlogId);
            ViewBag.FromUserId = new SelectList(db.Users, "Id", "NickName", comment.FromUserId);
            return View(comment);
        }

        // GET: Comment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "BlogTitle", comment.BlogId);
            ViewBag.FromUserId = new SelectList(db.Users, "Id", "NickName", comment.FromUserId);
            return View(comment);
        }

        // POST: Comment/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentId,FromUserId,BlogId,CommContent,CreatedTime")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "BlogTitle", comment.BlogId);
            ViewBag.FromUserId = new SelectList(db.Users, "Id", "NickName", comment.FromUserId);
            return View(comment);
        }

        // GET: Comment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
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
