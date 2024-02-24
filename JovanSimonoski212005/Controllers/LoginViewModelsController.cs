using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using JovanSimonoski212005.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace JovanSimonoski212005.Controllers
{
    public class LoginViewModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //Login
        public ActionResult Login(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Id,Email,Password")] LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                User user_db = db.Users.Where(m => m.Email == loginViewModel.Email).FirstOrDefault();

                // User doesn't exist - Return 'Wrong username or password' for security reasons
                if (user_db == null)
                {
                    return RedirectToAction("Login", new { message = "Wrong username or password" });
                }
                else
                {
                    // Hash the password from the input
                    string hashed_password_login = BCrypt.Net.BCrypt.HashPassword(loginViewModel.Password, user_db.Salt);
                    string hashed_password_db = user_db.HashedPassword;

                    // Check if the passwords match
                    if (hashed_password_login == hashed_password_db)
                    {
                        // If true, the user has not confirmed the email yet
                        if (user_db.RegistrationCode != null)
                        {
                            RegisterViewModelsController registerViewModelsController = new RegisterViewModelsController();
                            registerViewModelsController.SendEmailConfirmation(user_db.Id);

                            return RedirectToAction("Confirm_Email", "RegisterViewModels", new { id = user_db.Id, message = "You haven't confirmed your email yet" });
                        }
                        else
                        {
                            SendTwoFactorAuthenticationCode(user_db.Id);

                            return RedirectToAction("TwoFactorAuthentication", new { id = user_db.Id });
                        }
                    }
                    else
                    {
                        // Add log
                        LoginAttemptLog log = new LoginAttemptLog();
                        log.Email = user_db.Email;
                        log.TimeStamp = DateTime.Now;
                        log.Succeeded = false;
                        db.Logs.Add(log);
                        db.SaveChanges();

                        return RedirectToAction("Login", new { message = "Wrong username or password" });
                    }
                }
            }
            else 
            {
                return View(loginViewModel);
            }
        }

        public ActionResult TwoFactorAuthentication(int id,string message)
        {
            ViewBag.Message = message;
            TwoFactorAuthenticationViewModel model = new TwoFactorAuthenticationViewModel();
            model.User_Id = id;
            return View(model);
        }

        [HttpPost]
        public ActionResult TwoFactorAuthentication([Bind(Include = "Id,User_Id,TwoFactorAuthentication_Code")] TwoFactorAuthenticationViewModel model)
        {
            User user = db.Users.Where(m => m.Id == model.User_Id).FirstOrDefault();
            if (user == null)
            {
                return Content("Error");
            }
            else
            {
                // Check if the code is correct
                if ((user.TwoFactorAuthenticationCode == model.TwoFactorAuthentication_Code) && (DateTime.Now < user.TwoFactorAuthenticationCode_Expiration))
                {
                    user.TwoFactorAuthenticationCode = null;
                    user.TwoFactorAuthenticationCode_Expiration = DateTime.Now.AddDays(-1);
                    db.SaveChanges();

                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    Session["LoggedIn"] = "true";
                    Session["UserId"] = user.Id;
                    Session["UserName"] = user.Email;
                    Session["Role"] = user.Role;

                    // Add log
                    LoginAttemptLog log = new LoginAttemptLog();
                    log.Email = user.Email;
                    log.TimeStamp = DateTime.Now;
                    log.Succeeded = true;
                    db.Logs.Add(log);
                    db.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    SendTwoFactorAuthenticationCode(user.Id);

                    return RedirectToAction("TwoFactorAuthentication", new { id = user.Id, message = "Invalid code" });
                }
            }

            
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SetAuthCookie(null,false);
            Session["LoggedIn"] = "false";

            Session["UserId"] = null;
            Session["UserName"] = null;
            Session["Role"] = null;

            return RedirectToAction("Index","Home");
        }

        public void SendTwoFactorAuthenticationCode(int id)
        {
            User user = db.Users.Where(m => m.Id == id).FirstOrDefault();

            user.TwoFactorAuthenticationCode = GenerateRandomCode();
            user.TwoFactorAuthenticationCode_Expiration = DateTime.Now.AddMinutes(5);

            db.SaveChanges();

            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("", "")); // Removed for security purposes
            email.To.Add(new MailboxAddress("", "")); // Removed for security purposes

            email.Subject = "Two Factor Authentication";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = "This code is valid for 5 minutes.\nCode: " + user.TwoFactorAuthenticationCode
            };
            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                smtp.Authenticate("", ""); // Removed for security purposes

                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }

        public string GenerateRandomCode()
        {
            int codeLength = 5;

            StringBuilder codeBuilder = new StringBuilder();

            Random random = new Random();

            for (int i = 0; i < codeLength; i++)
            {
                codeBuilder.Append(random.Next(10));
            }

            return codeBuilder.ToString();
        }
    }
}
