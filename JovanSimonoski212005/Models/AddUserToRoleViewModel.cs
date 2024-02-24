using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JovanSimonoski212005.Models
{
    public class AddUserToRoleViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string SelectedRole { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}