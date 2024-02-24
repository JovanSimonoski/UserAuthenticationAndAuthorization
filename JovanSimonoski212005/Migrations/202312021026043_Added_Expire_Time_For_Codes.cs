namespace JovanSimonoski212005.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Expire_Time_For_Codes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "RegistrationCode_Expiration", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "TwoFactorAuthenticationCode_Expiration", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "TwoFactorAuthenticationCode_Expiration");
            DropColumn("dbo.Users", "RegistrationCode_Expiration");
        }
    }
}
