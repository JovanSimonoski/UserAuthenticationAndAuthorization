namespace JovanSimonoski212005.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingRegistrationCodeUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "RegistrationCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "RegistrationCode");
        }
    }
}
