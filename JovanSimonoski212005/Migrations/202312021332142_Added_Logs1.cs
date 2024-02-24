namespace JovanSimonoski212005.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Logs1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LoginAttemptLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                        Succeeded = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LoginAttemptLogs");
        }
    }
}
