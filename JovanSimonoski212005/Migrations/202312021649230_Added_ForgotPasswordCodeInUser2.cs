namespace JovanSimonoski212005.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_ForgotPasswordCodeInUser2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewPasswords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User_Id = c.Int(nullable: false),
                        Password = c.String(nullable: false),
                        ConfirmPassword = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NewPasswords");
        }
    }
}
