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
    [Authorize(Roles ="Administrator")]
    public class LoggerController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Logger
        public ActionResult Index()
        {
            var logs = db.Loggers.Include(x => x.Blog).Include(y => y.Accessor);
            return View(logs.ToList());
        }

    }
}