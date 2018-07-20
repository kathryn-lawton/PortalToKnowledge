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
            
            AddColumn("dbo.AspNetUsers", "Role", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "InstructorId", "dbo.Instructors");
            DropForeignKey("dbo.Students", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Instructors", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Admins", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Students", new[] { "ApplicationUserId" });
            DropIndex("dbo.Students", new[] { "InstructorId" });
            DropIndex("dbo.Instructors", new[] { "ApplicationUserId" });
            DropIndex("dbo.Admins", new[] { "ApplicationUserId" });
            DropColumn("dbo.AspNetUsers", "Role");
            DropTable("dbo.Students");
            DropTable("dbo.Instructors");
            DropTable("dbo.Admins");
        }
    }
}
