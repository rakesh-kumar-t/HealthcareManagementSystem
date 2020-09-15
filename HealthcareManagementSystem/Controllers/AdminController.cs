using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HealthcareManagementSystem.Models;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace HealthcareManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private HealthCareContext db = new HealthCareContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Admin adm )
        {
          adm .Password = HomeController.encrypt(adm.Password);
            var admin = db.Admins.Where(a =>a.UserId.Equals(adm.UserId) && a.Password.Equals(adm.Password)).FirstOrDefault();
            if(admin!=null)
            {
                Session["UserId"] = admin.UserId.ToString();
                Session["Name"] = admin.Name.ToString();
                Session["Role"] = admin.Role.ToString();
                if(admin.Role=="Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid credentials");
            }
            return View(admin);
           


        }
    }
}