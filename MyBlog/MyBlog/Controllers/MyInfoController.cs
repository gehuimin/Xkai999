using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MyBlog.Models;
using System.Net;
using System.Data.Entity;

namespace MyBlog.Controllers
{
    [Authorize]
    public class MyInfoController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            MyInfoViewModel myInfo = new MyInfoViewModel { Id=user.Id, NickName = user.NickName, MobilePhone = user.MobilePhone, PersonalIdentity = user.PersonalIdentity };

            return View(myInfo);
        }

        // GET: MyInfo
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            MyInfoViewModel myInfo = new MyInfoViewModel { Id =user.Id, NickName = user.NickName, MobilePhone = user.MobilePhone, PersonalIdentity = user.PersonalIdentity };

            return View(myInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MyInfoViewModel myInfo)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = db.Users.Find(myInfo.Id);

                user.NickName = myInfo.NickName;
                user.MobilePhone = myInfo.MobilePhone;
                user.PersonalIdentity = myInfo.PersonalIdentity;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "MyInfo", new { id = myInfo.Id });
            }
            return View(myInfo);
        }
    }
}