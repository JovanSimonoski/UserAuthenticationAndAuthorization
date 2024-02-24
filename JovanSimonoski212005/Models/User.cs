using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JovanSimonoski212005.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string HashedPassword { get; set; }
        public string Role { get; set; }
        public string RegistrationCode { get; set; }
        public DateTime RegistrationCode_Expiration { get; set; }
        public string TwoFactorAuthenticationCode { get; set; }
        public DateTime TwoFactorAuthenticationCode_Expiration { get; set; }
        public string ForgotPasswordCode { get; set; }
        public DateTime ForgotPasswordCode_Expiration { get; set; }
    }
}