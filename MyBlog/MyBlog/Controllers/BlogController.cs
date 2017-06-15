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
    public class BlogController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Blog
        [Authorize]
        public ActionResult Index()
        {
            string myUserId = User.Identity.GetUserId();
            var blogs = db.Blogs
                .Where(b => b.UserId.Equals(myUserId))
                .Include(b => b.Category).OrderByDescending(o => o.CreatedTime);

            return View(blogs.ToList());
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult RecommendList()
        {
            var blogs = db.Blogs.Where(b => b.IsPrivate == false).OrderByDescending(o => o.CreatedTime);

            return View(blogs.ToList());
        }


        // GET: Blog/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        public ActionResult Browse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }

            blog.ReadedTimes++;
            db.SaveChanges();

            Logger logger = new Logger
            {
                UserId = User != null ? User.Identity.GetUserId() : null,
                BlogId = id.Value,
                Url = System.Web.HttpContext.Current.Request.Url.ToString(),
                IpAdrress = System.Web.HttpContext.Current.Request.UserHostAddress,
                Counter = 1,
                VisitedTime = DateTime.Now
            };

            db.Loggers.Add(logger);
            db.SaveChanges();

            return View(blog);
        }

        // GET: Blog/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Blog/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Create([Bind(Include = "BlogId,BlogTitle,BlogContent,UserId,CreatedTime,IsPrivate,ReadedTimes,IsRecommend,CategoryId")] Blog blog)
        {
            blog.UserId = User.Identity.GetUserId();
            blog.CreatedTime = DateTime.Now;
            blog.ReadedTimes = 0;
            blog.IsRecommend = false;

            if (ModelState.IsValid)
            {
                db.Blogs.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", blog.CategoryId);
            return View(blog);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Recommend(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Blog blog = db.Blogs.Find(id);

            if (blog == null)
            {
                return HttpNotFound();
            }

            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize(Roles = "Administrator")]
        public ActionResult Recommend([Bind(Include = "BlogId,BlogTitle,BlogContent,UserId,CreatedTime,IsPrivate,ReadedTimes,IsRecommend,CategoryId")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RecommendList");
            }
            return View(blog);
        }


        // GET: Blog/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", blog.CategoryId);
            return View(blog);
        }

        // POST: Blog/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Edit([Bind(Include = "BlogId,BlogTitle,BlogContent,UserId,CreatedTime,IsPrivate,ReadedTimes,IsRecommend,CategoryId")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", blog.CategoryId);
            return View(blog);
        }

        // GET: Blog/Delete/5
        [Authorize(Roles ="Register,Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Register,Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Blog blog = db.Blogs.Find(id);
            db.Blogs.Remove(blog);
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
