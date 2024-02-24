using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JovanSimonoski212005.Models
{
    public class TwoFactorAuthenticationViewModel
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public string TwoFactorAuthentication_Code { get; set; }
    }
}