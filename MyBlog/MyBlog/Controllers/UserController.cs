using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyBlog.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MyBlog.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: User
        public ActionResult Index()
        {
            List<SetAdminRoleViewModel> userList = new List<SetAdminRoleViewModel>();

            List<ApplicationUser> appUserList = db.Users.Include(u => u.Roles).ToList();

            string adminRoleId = db.Roles.FirstOrDefault(r => r.Name == "Administrator").Id;

            foreach (var user in appUserList)
            {
                SetAdminRoleViewModel userViewModel = new SetAdminRoleViewModel();
                userViewModel.UserName = user.UserName;
                userViewModel.NickName = user.NickName;
                userViewModel.IsAdmin = false;

                foreach(var r in user.Roles)
                {
                    if (r.RoleId == adminRoleId)
                    {
                        userViewModel.IsAdmin = true;
                    }
                }
                userList.Add(userViewModel);
            }

            return View(userList);
        }

        // GET: User/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        public ActionResult SetRole(string username)
        {
            IdentityRole adminRole = db.Roles.FirstOrDefault(r => r.Name == "Administrator");
            ApplicationUser user = db.Users.Include(r=>r.Roles).FirstOrDefault(u => u.UserName == username);

            SetAdminRoleViewModel viewModel = new SetAdminRoleViewModel();

            viewModel.UserName = username;
            viewModel.NickName = user.NickName;
            viewModel.IsAdmin = false;

            foreach(var r in user.Roles)
            {
                if(r.RoleId==adminRole.Id)
                {
                    viewModel.IsAdmin = true;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetRole(SetAdminRoleViewModel viewModel)
        {
            ApplicationUser user = db.Users.Include(u=>u.Roles).FirstOrDefault(u => u.UserName == viewModel.UserName);
            IdentityRole adminRole = db.Roles.Include(r => r.Users).FirstOrDefault(r => r.Name == "Administrator");

            if(viewModel.IsAdmin)
            {
                bool f1 = false;
                foreach(var r in user.Roles)
                {
                    if (r.RoleId == adminRole.Id && r.UserId == user.Id)
                    {
                        f1 = true;
                    }
                }
                if (f1 == false)
                {
                    user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = adminRole.Id });
                }

                db.SaveChanges();

                bool f2 = false;
                adminRole = db.Roles.Include(r => r.Users).FirstOrDefault(r => r.Name == "Administrator");
                foreach (var u in adminRole.Users)
                {
                    if(u.RoleId==adminRole.Id && u.UserId==user.Id)
                    {
                        f2 = true;
                    }
                }
                if (f2 == false)
                {
                    adminRole.Users.Add(new IdentityUserRole { UserId = user.Id, RoleId = adminRole.Id });
                }
            }
            else
            {
                IdentityUserRole userrole = null;
                foreach (var r in user.Roles)
                {
                    if (r.RoleId == adminRole.Id && r.UserId == user.Id)
                    {
                        userrole = r;
                    }
                }
                if (userrole != null)
                {
                    user.Roles.Remove(userrole);
                }

                db.SaveChanges();

                userrole = null;
                adminRole = db.Roles.Include(r => r.Users).FirstOrDefault(r => r.Name == "Administrator");
                foreach (var u in adminRole.Users)
                {
                    if (u.RoleId == adminRole.Id && u.UserId == user.Id)
                    {
                        userrole = u;
                    }
                }
                if(userrole!=null)
                {
                    adminRole.Users.Remove(userrole);
                }
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }


        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NickName,MobilePhone,PersonalIdentity,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(applicationUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }

        // GET: User/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: User/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NickName,MobilePhone,PersonalIdentity,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // GET: User/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
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
