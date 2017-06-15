using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlog.Models
{
    public class Blog
    {
        public int BlogId { get; set; }

        [Display(Name ="标题")]
        public string BlogTitle { get; set; }

        [Display(Name = "内容")]
        public string BlogContent { get; set; }

        [Display(Name = "用户")]
        [ForeignKey("Bloger")]
        public string UserId { get; set; }

        [Display(Name = "撰写时间")]
        [DisplayFormat(DataFormatString ="{0: yyyy-MM-dd HH:mm:ss}")]
        public DateTime CreatedTime { get; set; }

        [Display(Name = "暂不公开")]
        public bool IsPrivate { get; set; }

        [Display(Name = "阅读数")]
        public int ReadedTimes { get; set; }

        [Display(Name = "推荐")]
        public bool IsRecommend { get; set; }

        [Display(Name = "分类")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Reward> Rewards { get; set; }
        public virtual ICollection<Praise> Prarses { get; set; }

        [InverseProperty("Blogs")]
        public virtual ApplicationUser Bloger { get; set; }
    }

    public class Category
    {
        public int CategoryId { get; set; }

        [Display(Name ="分类名称")]
        public string CategoryName { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }
    }

    public class PrivateLetter
    {
        public int PrivateLetterId { get; set; }

        [Display(Name ="发信人")]
        [ForeignKey("FromUser")]
        public string FromUserId { get; set; }

        [Display(Name = "收信人")]
        [ForeignKey("ToUser")]
        public string ToUserId { get; set; }

        [Display(Name = "标题")]
        public string LetterTitle { get; set; }

        [Display(Name = "内容")]
        public string LetterContent { get; set; }

        [Display(Name = "发送时间")]
        [DisplayFormat(DataFormatString = "{0: yyyy-MM-dd HH:mm:ss}")]
        public DateTime SendedTime { get; set; }

        [Display(Name = "已阅")]
        public bool IsReaded { get; set; }

        [InverseProperty("ToUserLetters")]
        public virtual ApplicationUser FromUser { get; set; }

        [InverseProperty("ToMeLetters")]
        public virtual ApplicationUser ToUser { get; set; }
    }

    public class Attention
    {
        public int AttentionId { get; set; }

        [Display(Name = "关注人")]
        [ForeignKey("FromUser")]
        public string FromUserId { get; set; }

        [Display(Name = "被关注人")]
        [ForeignKey("ToUser")]
        public string ToUserId { get; set; }

        [Display(Name = "关注时间")]
        [DisplayFormat(DataFormatString = "{0: yyyy-MM-dd HH:mm:ss}")]
        public DateTime StartTime { get; set; }

        [InverseProperty("ToUserAttentions")]
        public virtual ApplicationUser FromUser { get; set; }

        [InverseProperty("ToMeAttentions")]
        public virtual ApplicationUser ToUser { get; set; }
    }

    public class Message
    {
        public int MessageId { get; set; }

        [Display(Name ="发留言人")]
        [ForeignKey("FromUser")]
        public string FromUserId { get; set; }

        [Display(Name = "收留言人")]
        [ForeignKey("ToUser")]
        public string ToUserId { get; set; }

        [Display(Name = "留言内容")]
        public string MsgContent { get; set; }

        [Display(Name = "留言时间")]
        [DisplayFormat(DataFormatString = "{0: yyyy-MM-dd HH:mm:ss}")]
        public DateTime CreatedTime { get; set; }

        [InverseProperty("ToUserMessages")]
        public virtual ApplicationUser FromUser { get; set; }

        [InverseProperty("ToMeMessages")]
        public virtual ApplicationUser ToUser { get; set; }
    }

    public class Praise
    {
        public int PraiseId { get; set; }

        [Display(Name = "点赞人")]
        [ForeignKey("FromUser")]
        public string FromUserId { get; set; }

        [Display(Name = "点赞的博客")]
        public int BlogId { get; set; }

        [Display(Name ="点赞时间")]
        [DisplayFormat(DataFormatString = "{0: yyyy-MM-dd HH:mm:ss}")]
        public DateTime PraisedTime { get; set; }

        [InverseProperty("Praises")]
        public virtual ApplicationUser FromUser { get; set; }

        public virtual Blog Blog { get; set; }
    }

    public class Comment
    {
        public int CommentId { get; set; }

        [Display(Name = "评论者")]
        [ForeignKey("FromUser")]
        public string FromUserId { get; set; }

        [Display(Name = "评论的博客")]
        public int BlogId { get; set; }

        [Display(Name = "评论内容")]
        public string CommContent { get; set; }

        [Display(Name = "评论时间")]
        [DisplayFormat(DataFormatString = "{0: yyyy-MM-dd HH:mm:ss}")]
        public DateTime CreatedTime { get; set; }

        [InverseProperty("Comments")]
        public virtual ApplicationUser FromUser { get; set; }

        public virtual Blog Blog { get; set; }
    }

    public class Reward
    {
        public int RewardId { get; set; }

        [Display(Name = "打赏人")]
        [ForeignKey("FromUser")]
        public string FromUserId { get; set; }

        [Display(Name = "打赏的博客")]
        public int BlogId { get; set; }

        [Display(Name = "赏金")]
        public int Money { get; set; }

        [Display(Name = "付款来源")]
        public string PaiedFrom { get; set; }

        [Display(Name = "付款帐号")]
        public string PaiedAccount { get; set; }

        //[Display(Name = "取款号")]
        //public int WithdrawMoneyId { get; set; }

        [Display(Name ="打赏时间")]
        [DisplayFormat(DataFormatString = "{0: yyyy-MM-dd HH:mm:ss}")]
        public DateTime RewardedTime { get; set; }

        [InverseProperty("Rewards")]
        public virtual ApplicationUser FromUser { get; set; }

        public virtual Blog Blog { get; set; }

        //public virtual  WithdrawMoney DrawMoney { get; set; }
    }


    public class Logger
    {
        public int LoggerId { get; set; }

        [Display(Name ="访问用户")]
        [ForeignKey("Accessor")]
        [DisplayFormat(NullDisplayText ="访客")]
        public string UserId { get; set; }

        [Display(Name ="客户IP地址")]
        public string IpAdrress { get; set; }

        [Display(Name ="客户访问URL")]
        public string Url { get; set; }

        [Display(Name ="访问的博客")]
        public int BlogId { get; set; }

        [Display(Name ="访问时间")]
        [DisplayFormat(DataFormatString = "{0: yyyy-MM-dd HH:mm:ss}")]
        public DateTime VisitedTime { get; set; }

        [Display(Name ="访问数")]
        public int Counter { get; set; }

        [InverseProperty("Loggers")]
        public virtual ApplicationUser Accessor { get; set; }

        public virtual Blog Blog { get; set; }

    }

    public class WithdrawMoney
    {
        public int WithdrawMoneyId { get; set; }

        [Display(Name = "取款人")]
        [ForeignKey("ToUser")]
        public string ToUserId { get; set; }

        [Display(Name = "取款总额")]
        public int Money { get; set; }

        [Display(Name = "取款去向")]
        public string PayToType { get; set; }

        [Display(Name = "取款帐号")]
        public string PayToAccount { get; set; }

        [Display(Name = "取款时间")]
        [DisplayFormat(DataFormatString = "{0: yyyy-MM-dd HH:mm:ss}")]
        public DateTime PaiedTime { get; set; }

        [InverseProperty("WithdrawMoney")]
        public virtual ApplicationUser ToUser { get; set; }

        //public virtual ICollection<Reward> Rewards { get; set; }
    }

}