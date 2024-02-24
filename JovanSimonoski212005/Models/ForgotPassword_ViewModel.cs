using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JovanSimonoski212005.Models
{
    public class ForgotPassword_ViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string ForgotPasswordCode { get; set; }
    }
}