namespace PortalToKnowledge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        AdminId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AdminId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Role = c.String(),
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
                "dbo.Classes",
                c => new
                    {
                        ClassId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ClassId);
            
            CreateTable(
                "dbo.Instructors",
                c => new
                    {
                        InstructorId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.InstructorId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        InstructorId = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StudentId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Instructors", t => t.InstructorId, cascadeDelete: true)
                .Index(t => t.InstructorId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Media",
                c => new
                    {
                        MediaId = c.Int(nullable: false, identity: true),
                        Link = c.String(),
                        ClassId = c.Int(nullable: false),
                        MediaTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MediaId)
                .ForeignKey("dbo.Classes", t => t.ClassId, cascadeDelete: true)
                .ForeignKey("dbo.MediaTypes", t => t.MediaTypeId, cascadeDelete: true)
                .Index(t => t.ClassId)
                .Index(t => t.MediaTypeId);
            
            CreateTable(
                "dbo.MediaTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.InstructorClasses",
                c => new
                    {
                        Instructor_InstructorId = c.Int(nullable: false),
                        Class_ClassId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Instructor_InstructorId, t.Class_ClassId })
                .ForeignKey("dbo.Instructors", t => t.Instructor_InstructorId, cascadeDelete: true)
                .ForeignKey("dbo.Classes", t => t.Class_ClassId, cascadeDelete: true)
                .Index(t => t.Instructor_InstructorId)
                .Index(t => t.Class_ClassId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Media", "MediaTypeId", "dbo.MediaTypes");
            DropForeignKey("dbo.Media", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.Students", "InstructorId", "dbo.Instructors");
            DropForeignKey("dbo.Students", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.InstructorClasses", "Class_ClassId", "dbo.Classes");
            DropForeignKey("dbo.InstructorClasses", "Instructor_InstructorId", "dbo.Instructors");
            DropForeignKey("dbo.Instructors", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Admins", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.InstructorClasses", new[] { "Class_ClassId" });
            DropIndex("dbo.InstructorClasses", new[] { "Instructor_InstructorId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Media", new[] { "MediaTypeId" });
            DropIndex("dbo.Media", new[] { "ClassId" });
            DropIndex("dbo.Students", new[] { "ApplicationUserId" });
            DropIndex("dbo.Students", new[] { "InstructorId" });
            DropIndex("dbo.Instructors", new[] { "ApplicationUserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Admins", new[] { "ApplicationUserId" });
            DropTable("dbo.InstructorClasses");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.MediaTypes");
            DropTable("dbo.Media");
            DropTable("dbo.Students");
            DropTable("dbo.Instructors");
            DropTable("dbo.Classes");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Admins");
        }
    }
}
