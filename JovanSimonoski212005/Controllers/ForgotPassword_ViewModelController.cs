using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using JovanSimonoski212005.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNet.Identity;
using MimeKit;

namespace JovanSimonoski212005.Controllers
{
    public class ForgotPassword_ViewModelController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult ForgotPassword(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword([Bind(Include = "Id,Email,ForgotPasswordCode")] ForgotPassword_ViewModel forgotPassword_ViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.Where(m => m.Email == forgotPassword_ViewModel.Email).FirstOrDefault();
                return RedirectToAction("ForgotPassword_InsertCode",new {id = user.Id});
            }
            else
            {
                return View();
            }
        }

        public ActionResult ForgotPassword_InsertCode(int id, string message)
        {
            SendEmailConfirmation(id);
            ViewBag.Message = message;

            ForgotPassword_ViewModel model = new ForgotPassword_ViewModel();
            model.Email = db.Users.Where(m => m.Id == id).FirstOrDefault().Email;

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword_InsertCode([Bind(Include = "Id,Email,ForgotPasswordCode")] ForgotPassword_ViewModel forgotPassword_ViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.Where(m => m.Email == forgotPassword_ViewModel.Email).FirstOrDefault();

                if (user == null)
                {
                    return Content("Error");
                }
                else
                {
                    if((user.ForgotPasswordCode == forgotPassword_ViewModel.ForgotPasswordCode) && (DateTime.Now < user.ForgotPasswordCode_Expiration))
                    {
                        user.ForgotPasswordCode = null;
                        user.ForgotPasswordCode_Expiration = DateTime.Now.AddDays(-1);
                        db.SaveChanges();
                        return RedirectToAction("NewPassword", new {id = user.Id});
                    }
                    else
                    {
                        return RedirectToAction("ForgotPassword_InsertCode", new { id = user.Id, message = "Invalid code" });
                    }
                }
            }
            else
            {
                return View();
            }
        }

        public ActionResult NewPassword(int id, string message)
        {
            ViewBag.Message = message;
            NewPassword newPassword = new NewPassword();
            newPassword.User_Id = id;
            return View(newPassword);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewPassword([Bind(Include = "Id,User_Id,Password,ConfirmPassword")] NewPassword newPassword_viewModel )
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.Where(m => m.Id == newPassword_viewModel.User_Id).First();
                user.Salt = BCrypt.Net.BCrypt.GenerateSalt(12);
                user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword_viewModel.Password, user.Salt);
                db.SaveChanges();

                return RedirectToAction("NewPassword", new {id=user.Id, message = "Successfuly updated password"});
            }
            else
            {
                return View();
            }
        }

        public void SendEmailConfirmation(int id)
        {
            User user = db.Users.Where(m => m.Id ==  id).FirstOrDefault();

            user.ForgotPasswordCode = GenerateRandomCode();
            user.ForgotPasswordCode_Expiration = DateTime.Now.AddMinutes(5);

            db.SaveChanges();
            
            // Send Confiramtion Code on Email
            //------------------------------------------------------------------------------------------
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("", "")); // Removed for security purposes
            email.To.Add(new MailboxAddress("", "")); // Removed for security purposes

            email.Subject = "Reset your password";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = "This code is valid for 5 minutes.\nCode: " + user.ForgotPasswordCode
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
