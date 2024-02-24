using JovanSimonoski212005.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JovanSimonoski212005.Controllers
{
    public class AddUserToRoleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AddUserToRole
        public ActionResult AddUserToRole()
        {
            if ((string)Session["Role"] != "Admin")
            {
                return Content("Unathorized Access!");
            }
            AddUserToRoleViewModel model = new AddUserToRoleViewModel();
            model.Roles = new List<string> {"Admin", "Moderator", "User" };
            
            return View(model);
        }
        [HttpPost]
        public ActionResult AddUserToRole(AddUserToRoleViewModel model)
        {
            if ((string)Session["Role"] != "Admin")
            {
                return Content("Unathorized Access!");
            }
            User user = db.Users.Where(m => m.Email == model.Email).FirstOrDefault();
            if (user == null)
            {
                return Content("Invalid Email");
            }
            user.Role = model.SelectedRole;
            db.SaveChanges();
            return RedirectToAction("Index","Home");
        }
    }
}