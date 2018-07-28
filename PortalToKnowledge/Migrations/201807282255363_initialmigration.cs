namespace PortalToKnowledge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialmigration : DbMigration
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
                "dbo.Assignments",
                c => new
                    {
                        AssignmentId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DueDate = c.String(),
                        Link = c.String(),
                        MediaTypeId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AssignmentId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.MediaTypes", t => t.MediaTypeId, cascadeDelete: true)
                .Index(t => t.MediaTypeId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        InstructorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CourseId)
                .ForeignKey("dbo.Instructors", t => t.InstructorId, cascadeDelete: true)
                .Index(t => t.InstructorId);
            
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
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StudentId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Progresses",
                c => new
                    {
                        ProgressId = c.Int(nullable: false, identity: true),
                        Status = c.Boolean(nullable: false),
                        StudentId = c.Int(nullable: false),
                        AssignmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProgressId)
                .ForeignKey("dbo.Assignments", t => t.AssignmentId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.AssignmentId);
            
            CreateTable(
                "dbo.MediaTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CityId);
            
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        ResourceId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StreetAddress = c.String(),
                        CityId = c.Int(nullable: false),
                        StateId = c.Int(nullable: false),
                        ZipcodeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ResourceId)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.States", t => t.StateId, cascadeDelete: true)
                .ForeignKey("dbo.Zipcodes", t => t.ZipcodeId, cascadeDelete: true)
                .Index(t => t.CityId)
                .Index(t => t.StateId)
                .Index(t => t.ZipcodeId);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        StateId = c.Int(nullable: false, identity: true),
                        Abbreviation = c.String(),
                    })
                .PrimaryKey(t => t.StateId);
            
            CreateTable(
                "dbo.Zipcodes",
                c => new
                    {
                        ZipcodeId = c.Int(nullable: false, identity: true),
                        Zip = c.String(),
                    })
                .PrimaryKey(t => t.ZipcodeId);
            
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
                "dbo.StudentCourses",
                c => new
                    {
                        Student_StudentId = c.Int(nullable: false),
                        Course_CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_StudentId, t.Course_CourseId })
                .ForeignKey("dbo.Students", t => t.Student_StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.Course_CourseId, cascadeDelete: true)
                .Index(t => t.Student_StudentId)
                .Index(t => t.Course_CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Resources", "ZipcodeId", "dbo.Zipcodes");
            DropForeignKey("dbo.Resources", "StateId", "dbo.States");
            DropForeignKey("dbo.Resources", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Assignments", "MediaTypeId", "dbo.MediaTypes");
            DropForeignKey("dbo.Assignments", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Progresses", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Progresses", "AssignmentId", "dbo.Assignments");
            DropForeignKey("dbo.StudentCourses", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.StudentCourses", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.Students", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "InstructorId", "dbo.Instructors");
            DropForeignKey("dbo.Instructors", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Admins", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.StudentCourses", new[] { "Course_CourseId" });
            DropIndex("dbo.StudentCourses", new[] { "Student_StudentId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Resources", new[] { "ZipcodeId" });
            DropIndex("dbo.Resources", new[] { "StateId" });
            DropIndex("dbo.Resources", new[] { "CityId" });
            DropIndex("dbo.Progresses", new[] { "AssignmentId" });
            DropIndex("dbo.Progresses", new[] { "StudentId" });
            DropIndex("dbo.Students", new[] { "ApplicationUserId" });
            DropIndex("dbo.Instructors", new[] { "ApplicationUserId" });
            DropIndex("dbo.Courses", new[] { "InstructorId" });
            DropIndex("dbo.Assignments", new[] { "CourseId" });
            DropIndex("dbo.Assignments", new[] { "MediaTypeId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Admins", new[] { "ApplicationUserId" });
            DropTable("dbo.StudentCourses");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Zipcodes");
            DropTable("dbo.States");
            DropTable("dbo.Resources");
            DropTable("dbo.Cities");
            DropTable("dbo.MediaTypes");
            DropTable("dbo.Progresses");
            DropTable("dbo.Students");
            DropTable("dbo.Instructors");
            DropTable("dbo.Courses");
            DropTable("dbo.Assignments");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Admins");
        }
    }
}
