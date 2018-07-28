namespace PortalToKnowledge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class duedatemig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assignments", "dueDate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assignments", "dueDate");
        }
    }
}
