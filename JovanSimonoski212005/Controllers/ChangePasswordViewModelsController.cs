using System.Data;
using System.Linq;
using System.Web.Mvc;
using JovanSimonoski212005.Models;

namespace JovanSimonoski212005.Controllers
{
    public class ChangePasswordViewModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult ChangePassword(string message)
        {
            ViewBag.Message = message;
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            model.Email = Session["UserName"].ToString();
            model.UserId = db.Users.Where(m => m.Email == model.Email).FirstOrDefault().Id;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword([Bind(Include = "Id,UserId,Email,OldPassword,NewPassword,ConfirmNewPassword")] ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.Where(m => m.Id == changePasswordViewModel.UserId).FirstOrDefault();

                string old_password = BCrypt.Net.BCrypt.HashPassword(changePasswordViewModel.OldPassword, user.Salt);
                string db_old_password = user.HashedPassword;

                if(old_password != db_old_password)
                {
                    return RedirectToAction("ChangePassword", new { message = "Wrong old password" });
                }
                else
                {
                    user.Salt = BCrypt.Net.BCrypt.GenerateSalt(12);
                    user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(changePasswordViewModel.NewPassword, user.Salt);
                    db.SaveChanges();

                    return RedirectToAction("ChangePassword", new { message = "Successfuly updated password" });
                }
            }
            else
            {
                return View(changePasswordViewModel);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
