namespace JovanSimonoski212005.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Roles1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AddUserToRoleViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AddUserToRoleViewModels");
        }
    }
}
