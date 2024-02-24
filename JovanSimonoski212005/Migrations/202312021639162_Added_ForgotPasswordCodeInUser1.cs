namespace JovanSimonoski212005.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_ForgotPasswordCodeInUser1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ForgotPassword_ViewModel", "ForgotPasswordCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ForgotPassword_ViewModel", "ForgotPasswordCode");
        }
    }
}
