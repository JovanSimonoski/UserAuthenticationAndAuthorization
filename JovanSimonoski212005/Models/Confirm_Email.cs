using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JovanSimonoski212005.Models
{
    public class Confirm_Email
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public string Registration_Code { get; set; }
    }
}