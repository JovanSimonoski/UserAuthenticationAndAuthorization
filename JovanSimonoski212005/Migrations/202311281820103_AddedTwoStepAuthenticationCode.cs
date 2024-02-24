namespace JovanSimonoski212005.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTwoStepAuthenticationCode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TwoFactorAuthenticationViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User_Id = c.Int(nullable: false),
                        TwoFactorAuthentication_Code = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "TwoFactorAuthenticationCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "TwoFactorAuthenticationCode");
            DropTable("dbo.TwoFactorAuthenticationViewModels");
        }
    }
}
