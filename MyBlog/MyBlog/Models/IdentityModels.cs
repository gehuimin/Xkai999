using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace MyBlog.Models
{
    // 可以通过向 ApplicationUser 类添加更多属性来为用户添加配置文件数据。若要了解详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=317594。
    public class ApplicationUser : IdentityUser
    {
        public string NickName { get; set; }
        public string MobilePhone { get; set; }
        public string PersonalIdentity { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Praise> Praises { get; set; }

        public virtual ICollection<PrivateLetter> ToUserLetters { get; set; }
        public virtual ICollection<PrivateLetter> ToMeLetters { get; set; }

        public virtual ICollection<Attention> ToUserAttentions { get; set; }
        public virtual ICollection<Attention> ToMeAttentions { get; set; }

        public virtual ICollection<Message> ToUserMessages { get; set; }
        public virtual ICollection<Message> ToMeMessages { get; set; }

        public virtual ICollection<Logger> Loggers { get; set; }

        public virtual ICollection<Reward> Rewards { get; set; }

        public virtual ICollection<WithdrawMoney> WithdrawMoney { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<PrivateLetter> PrivateLetters { get; set; }
        public DbSet<Attention> Attentions { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Praise> Praises { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<Logger> Loggers { get; set; }
        public DbSet<WithdrawMoney> WithdrwaMoney { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}