namespace JovanSimonoski212005.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class corrections : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChangePasswordViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Email = c.String(nullable: false),
                        OldPassword = c.String(nullable: false),
                        NewPassword = c.String(nullable: false),
                        ConfirmNewPassword = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ChangePasswordViewModels");
        }
    }
}
