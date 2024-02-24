namespace JovanSimonoski212005.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_ForgotPasswordCodeInUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ForgotPassword_ViewModel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "ForgotPasswordCode", c => c.String());
            AddColumn("dbo.Users", "ForgotPasswordCode_Expiration", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ForgotPasswordCode_Expiration");
            DropColumn("dbo.Users", "ForgotPasswordCode");
            DropTable("dbo.ForgotPassword_ViewModel");
        }
    }
}
