using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JovanSimonoski212005.Models
{
    public class LoginAttemptLog
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool Succeeded { get; set; }
    }
}