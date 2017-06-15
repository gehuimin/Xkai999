namespace MyBlog.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MyBlog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MyBlog.Models.ApplicationDbContext context)
        {

            context.Roles.AddOrUpdate(
                r => r.Name,
                new IdentityRole { Name = "Register" },
                new IdentityRole { Name = "Administrator" }
            );

            context.Users.AddOrUpdate(
                u => u.UserName,
                new ApplicationUser {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "admin@ycjdgz.cn",
                    PasswordHash = (new PasswordHasher()).HashPassword("Abc123$"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Email = "admin@ycjdgz.cn",
                    NickName = "管理员",
                    MobilePhone = "12312345678",
                    PersonalIdentity = "320902199910121234"
                }
            );

            context.SaveChanges();

            ApplicationUser admin = context.Users.FirstOrDefault(u => u.UserName == "admin@ycjdgz.cn");

            List<string> adminRoleIds = new List<string>(); //管理员的角色 Id 集合
            foreach(var r in admin.Roles)
            {
                adminRoleIds.Add(r.RoleId);
            }

            foreach (var r in context.Roles)
            {
                if (!adminRoleIds.Contains(r.Id))
                {
                    admin.Roles.Add(new IdentityUserRole { RoleId = r.Id, UserId = admin.Id });
                }
            }

            context.Categories.AddOrUpdate(
                n => n.CategoryName,
                new Category { CategoryName = "编程语言" },
                new Category { CategoryName = "数据库及开发技术" },
                new Category { CategoryName = "服务器端开发与管理" },
                new Category { CategoryName = "移动开发" },
                new Category { CategoryName = "前端开发" },
                new Category { CategoryName = "图像与多媒体" },
                new Category { CategoryName = "系统运维" },
                new Category { CategoryName = "云计算" },
                new Category { CategoryName = "游戏开发" },
                new Category { CategoryName = "大数据" }
            );

        }
    }
}
