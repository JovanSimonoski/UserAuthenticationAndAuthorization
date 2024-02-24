using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace JovanSimonoski212005.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<LoginAttemptLog> Logs { get; set; }

        public System.Data.Entity.DbSet<JovanSimonoski212005.Models.RegisterViewModel> RegisterViewModels { get; set; }

        public System.Data.Entity.DbSet<JovanSimonoski212005.Models.LoginViewModel> LoginViewModels { get; set; }

        public System.Data.Entity.DbSet<JovanSimonoski212005.Models.Confirm_Email> Confirm_Email { get; set; }

        public System.Data.Entity.DbSet<JovanSimonoski212005.Models.TwoFactorAuthenticationViewModel> TwoFactorAuthenticationViewModels { get; set; }

        public System.Data.Entity.DbSet<JovanSimonoski212005.Models.ChangePasswordViewModel> ChangePasswordViewModels { get; set; }

        public System.Data.Entity.DbSet<JovanSimonoski212005.Models.ForgotPassword_ViewModel> ForgotPassword_ViewModel { get; set; }

        public System.Data.Entity.DbSet<JovanSimonoski212005.Models.NewPassword> NewPasswords { get; set; }
        public System.Data.Entity.DbSet<JovanSimonoski212005.Models.AddUserToRoleViewModel> AddUserToRoleViewModels { get; set; }

    }
}