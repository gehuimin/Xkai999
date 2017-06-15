using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class MyFilterController : Controller
    {
        // GET: MyFilter
        public ActionResult Index()
        {
            return View();
        }
    }
}