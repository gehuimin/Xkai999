namespace MyBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attentions",
                c => new
                    {
                        AttentionId = c.Int(nullable: false, identity: true),
                        FromUserId = c.String(maxLength: 128),
                        ToUserId = c.String(maxLength: 128),
                        StartTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AttentionId)
                .ForeignKey("dbo.AspNetUsers", t => t.FromUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.ToUserId)
                .Index(t => t.FromUserId)
                .Index(t => t.ToUserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        NickName = c.String(),
                        MobilePhone = c.String(),
                        PersonalIdentity = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        BlogId = c.Int(nullable: false, identity: true),
                        BlogTitle = c.String(),
                        BlogContent = c.String(),
                        UserId = c.String(maxLength: 128),
                        CreatedTime = c.DateTime(nullable: false),
                        IsPrivate = c.Boolean(nullable: false),
                        ReadedTimes = c.Int(nullable: false),
                        IsRecommend = c.Boolean(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BlogId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        FromUserId = c.String(maxLength: 128),
                        BlogId = c.Int(nullable: false),
                        CommContent = c.String(),
                        CreatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Blogs", t => t.BlogId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.FromUserId)
                .Index(t => t.FromUserId)
                .Index(t => t.BlogId);
            
            CreateTable(
                "dbo.Praises",
                c => new
                    {
                        PraiseId = c.Int(nullable: false, identity: true),
                        FromUserId = c.String(maxLength: 128),
                        BlogId = c.Int(nullable: false),
                        PraisedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PraiseId)
                .ForeignKey("dbo.Blogs", t => t.BlogId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.FromUserId)
                .Index(t => t.FromUserId)
                .Index(t => t.BlogId);
            
            CreateTable(
                "dbo.Rewards",
                c => new
                    {
                        RewardId = c.Int(nullable: false, identity: true),
                        FromUserId = c.String(maxLength: 128),
                        BlogId = c.Int(nullable: false),
                        Money = c.Int(nullable: false),
                        PaiedFrom = c.String(),
                        PaiedAccount = c.String(),
                        RewardedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RewardId)
                .ForeignKey("dbo.Blogs", t => t.BlogId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.FromUserId)
                .Index(t => t.FromUserId)
                .Index(t => t.BlogId);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Loggers",
                c => new
                    {
                        LoggerId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        IpAdrress = c.String(),
                        Url = c.String(),
                        BlogId = c.Int(nullable: false),
                        VisitedTime = c.DateTime(nullable: false),
                        Counter = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LoggerId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Blogs", t => t.BlogId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.BlogId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.PrivateLetters",
                c => new
                    {
                        PrivateLetterId = c.Int(nullable: false, identity: true),
                        FromUserId = c.String(maxLength: 128),
                        ToUserId = c.String(maxLength: 128),
                        LetterTitle = c.String(),
                        LetterContent = c.String(),
                        SendedTime = c.DateTime(nullable: false),
                        IsReaded = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PrivateLetterId)
                .ForeignKey("dbo.AspNetUsers", t => t.FromUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.ToUserId)
                .Index(t => t.FromUserId)
                .Index(t => t.ToUserId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        FromUserId = c.String(maxLength: 128),
                        ToUserId = c.String(maxLength: 128),
                        MsgContent = c.String(),
                        CreatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.AspNetUsers", t => t.FromUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.ToUserId)
                .Index(t => t.FromUserId)
                .Index(t => t.ToUserId);
            
            CreateTable(
                "dbo.WithdrawMoneys",
                c => new
                    {
                        WithdrawMoneyId = c.Int(nullable: false, identity: true),
                        ToUserId = c.String(maxLength: 128),
                        Money = c.Int(nullable: false),
                        PayToType = c.String(),
                        PayToAccount = c.String(),
                        PaiedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.WithdrawMoneyId)
                .ForeignKey("dbo.AspNetUsers", t => t.ToUserId)
                .Index(t => t.ToUserId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Attentions", "ToUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Attentions", "FromUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.WithdrawMoneys", "ToUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "ToUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "FromUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PrivateLetters", "ToUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PrivateLetters", "FromUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Loggers", "BlogId", "dbo.Blogs");
            DropForeignKey("dbo.Loggers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Rewards", "FromUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Rewards", "BlogId", "dbo.Blogs");
            DropForeignKey("dbo.Praises", "FromUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Praises", "BlogId", "dbo.Blogs");
            DropForeignKey("dbo.Comments", "FromUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "BlogId", "dbo.Blogs");
            DropForeignKey("dbo.Blogs", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Blogs", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.WithdrawMoneys", new[] { "ToUserId" });
            DropIndex("dbo.Messages", new[] { "ToUserId" });
            DropIndex("dbo.Messages", new[] { "FromUserId" });
            DropIndex("dbo.PrivateLetters", new[] { "ToUserId" });
            DropIndex("dbo.PrivateLetters", new[] { "FromUserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Loggers", new[] { "BlogId" });
            DropIndex("dbo.Loggers", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Rewards", new[] { "BlogId" });
            DropIndex("dbo.Rewards", new[] { "FromUserId" });
            DropIndex("dbo.Praises", new[] { "BlogId" });
            DropIndex("dbo.Praises", new[] { "FromUserId" });
            DropIndex("dbo.Comments", new[] { "BlogId" });
            DropIndex("dbo.Comments", new[] { "FromUserId" });
            DropIndex("dbo.Blogs", new[] { "CategoryId" });
            DropIndex("dbo.Blogs", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Attentions", new[] { "ToUserId" });
            DropIndex("dbo.Attentions", new[] { "FromUserId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.WithdrawMoneys");
            DropTable("dbo.Messages");
            DropTable("dbo.PrivateLetters");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Loggers");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Rewards");
            DropTable("dbo.Praises");
            DropTable("dbo.Comments");
            DropTable("dbo.Categories");
            DropTable("dbo.Blogs");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Attentions");
        }
    }
}
