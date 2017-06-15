using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.Entity;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(int? id, string userId)
        {
            var blogs = db.Blogs.Where(b => b.IsPrivate == false).Include(c => c.Comments).Include(r => r.Rewards).Include(cg => cg.Category);

            if (!string.IsNullOrEmpty(userId))
            {
                blogs = blogs.Where(b => b.UserId.Equals(userId));
            }

            ViewBag.Bloger = db.Users.SingleOrDefault(u => u.Id.Equals(userId));

            if (id > 0)
            {
                blogs = blogs.Where(b => b.CategoryId == id);
                ViewBag.CategoryId = id;
            }
            else
            {
                ViewBag.CategoryId = 0;
            }

            return View(blogs.OrderByDescending(o => o.CreatedTime).ToList());
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Contact()
        {

            return View();
        }
    }
}