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
    public class RegisterViewModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Register
        public ActionResult Register(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        // Register POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Id,Email,Password,ConfirmPassword")] RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                // Check if email is already in use
                if(db.Users.Where(m => m.Email == registerViewModel.Email).Any())
                {
                    return RedirectToAction("Register", new {message = "Email already in use"});
                }
                else
                {
                    // Create the user in the DataBase
                    User user = new User();
                    user.Email = registerViewModel.Email;
                    user.Salt = BCrypt.Net.BCrypt.GenerateSalt(12);
                    user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(registerViewModel.Password, user.Salt);
                    user.Role = "User";

                    // Just for proper working purposes
                    user.RegistrationCode_Expiration = DateTime.Now;
                    user.TwoFactorAuthenticationCode_Expiration = DateTime.Now;
                    user.ForgotPasswordCode_Expiration = DateTime.Now;

                    db.Users.Add(user);
                    db.SaveChanges();

                    SendEmailConfirmation(user.Id);

                    // Redirect to Confirm Email
                    return RedirectToAction("Confirm_Email", new { id = user.Id });
                }
            }
            else
            {
                return View(registerViewModel);
            }
        }

        // Confrim Email
        public ActionResult Confirm_Email(int id,string message)
        {
            ViewBag.Message = message;
            Confirm_Email model = new Confirm_Email();
            model.User_Id = id;
            return View(model);
        }

        // Confrim Email
        [HttpPost]
        public ActionResult Confirm_Email([Bind(Include = "Id,User_Id,Registration_Code")] Confirm_Email model)
        {
            User user = db.Users.Where(m => m.Id == model.User_Id).FirstOrDefault();

            if (user == null)
            {
                return Content("Error");
            }
            else
            {
                // Check the code from the input
                if ((user.RegistrationCode == model.Registration_Code) && (DateTime.Now < user.RegistrationCode_Expiration))
                {
                    user.RegistrationCode = null;
                    user.RegistrationCode_Expiration = DateTime.Now.AddDays(-1);
                    db.SaveChanges();

                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    Session["LoggedIn"] = "true";
                    Session["UserId"] = user.Id;
                    Session["UserName"] = user.Email;
                    Session["Role"] = user.Role;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    SendEmailConfirmation(user.Id);
                    return RedirectToAction("Confirm_Email", new { id = user.Id, message = "Invalid Code" });
                }
            }
        }

        public void SendEmailConfirmation(int id)
        {
            User user = db.Users.Where(m => m.Id ==  id).FirstOrDefault();

            user.RegistrationCode = GenerateRandomCode();
            user.RegistrationCode_Expiration = DateTime.Now.AddMinutes(5);

            db.SaveChanges();
            
            // Send Confiramtion Code on Email
            //------------------------------------------------------------------------------------------
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("", "")); // Removed for security purposes
            email.To.Add(new MailboxAddress("", "")); // Removed for security purposes

            email.Subject = "Confirm your email address";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = "This code is valid for 5 minutes.\nCode: " + user.RegistrationCode
            };
            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, false);

                smtp.Authenticate("", ""); // Removed for security purposes

                smtp.Send(email);
                smtp.Disconnect(true);
            }
            //------------------------------------------------------------------------------------------
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
