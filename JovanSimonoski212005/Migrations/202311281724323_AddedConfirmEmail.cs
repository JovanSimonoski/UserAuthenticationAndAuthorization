namespace JovanSimonoski212005.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedConfirmEmail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Confirm_Email",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User_Id = c.Int(nullable: false),
                        Registration_Code = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Confirm_Email");
        }
    }
}
