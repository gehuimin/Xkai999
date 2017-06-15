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
    public class RewardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reward
        public ActionResult Index()
        {
            string userId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;

            var rewards = db.Rewards.Include(r => r.Blog).Include(r => r.FromUser).Where(r => r.FromUserId == userId)
                .Union(db.Rewards.Include(r => r.Blog).Include(r => r.FromUser).Where(r => r.Blog.Bloger.Id == userId));

            return View(rewards.ToList());
        }

        // GET: Reward/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reward reward = db.Rewards.Find(id);
            if (reward == null)
            {
                return HttpNotFound();
            }
            return View(reward);
        }

        public JsonResult Reward([Bind(Include = "RewardId,FromUserId,BlogId,Money,PaiedFrom,PaiedAccount,RewardedTime")] Reward reward)
        {
            reward.RewardedTime = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    db.Rewards.Add(reward);
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

        // GET: Reward/Create
        public ActionResult Create()
        {
            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "BlogTitle");
            ViewBag.WithdrawMoneyId = new SelectList(db.WithdrwaMoney, "WithdrawMoneyId", "ToUserId");
            ViewBag.FromUserId = new SelectList(db.Users, "Id", "NickName");
            return View();
        }

        // POST: Reward/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RewardId,FromUserId,BlogId,Money,PaiedFrom,PaiedAccount,WithdrawMoneyId,RewardedTime")] Reward reward)
        {
            if (ModelState.IsValid)
            {
                db.Rewards.Add(reward);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "BlogTitle", reward.BlogId);
            //ViewBag.WithdrawMoneyId = new SelectList(db.WithdrwaMoney, "WithdrawMoneyId", "ToUserId", reward.WithdrawMoneyId);
            ViewBag.FromUserId = new SelectList(db.Users, "Id", "NickName", reward.FromUserId);
            return View(reward);
        }

        // GET: Reward/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reward reward = db.Rewards.Find(id);
            if (reward == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "BlogTitle", reward.BlogId);
            //ViewBag.WithdrawMoneyId = new SelectList(db.WithdrwaMoney, "WithdrawMoneyId", "ToUserId", reward.WithdrawMoneyId);
            ViewBag.FromUserId = new SelectList(db.Users, "Id", "NickName", reward.FromUserId);
            return View(reward);
        }

        // POST: Reward/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RewardId,FromUserId,BlogId,Money,PaiedFrom,PaiedAccount,WithdrawMoneyId,RewardedTime")] Reward reward)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reward).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "BlogTitle", reward.BlogId);
            //ViewBag.WithdrawMoneyId = new SelectList(db.WithdrwaMoney, "WithdrawMoneyId", "ToUserId", reward.WithdrawMoneyId);
            ViewBag.FromUserId = new SelectList(db.Users, "Id", "NickName", reward.FromUserId);
            return View(reward);
        }

        // GET: Reward/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reward reward = db.Rewards.Find(id);
            if (reward == null)
            {
                return HttpNotFound();
            }
            return View(reward);
        }

        // POST: Reward/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reward reward = db.Rewards.Find(id);
            db.Rewards.Remove(reward);
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
