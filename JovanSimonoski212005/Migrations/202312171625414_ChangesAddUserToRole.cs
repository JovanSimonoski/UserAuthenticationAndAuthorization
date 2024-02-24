namespace JovanSimonoski212005.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesAddUserToRole : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AddUserToRoleViewModels", "Email", c => c.String());
            AddColumn("dbo.AddUserToRoleViewModels", "SelectedRole", c => c.String());
            DropColumn("dbo.AddUserToRoleViewModels", "UserId");
            DropColumn("dbo.AddUserToRoleViewModels", "Role");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AddUserToRoleViewModels", "Role", c => c.String());
            AddColumn("dbo.AddUserToRoleViewModels", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.AddUserToRoleViewModels", "SelectedRole");
            DropColumn("dbo.AddUserToRoleViewModels", "Email");
        }
    }
}
