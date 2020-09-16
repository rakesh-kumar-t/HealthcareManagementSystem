using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HealthcareManagementSystem.Models;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace HealthcareManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private HealthCareContext db = new HealthCareContext();
        // GET: Admin
        [Authorize]
        public ActionResult Index()
        {
            if (Session["Role"].ToString() == "Admin")
                return View();
            else
                return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult AddMember()
        {
            if (Session["Role"].ToString() == "Admin")
                return View();
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [Authorize]
        public ActionResult AddMember(Member member)
        {
            if (Session["Role"].ToString() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    db.Members.Add(member);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Data Formats");
                }
                return View(member);
            }
            else
                return RedirectToAction("Index", "Home");

        }
        [Authorize]
        public ActionResult ViewMembers()
        {
            if (Session["Role"].ToString() == "Admin")
            {
                return View(db.Members.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [Authorize]
        public ActionResult NewUser()
        {
            if (Session["UserId"]!=null&&Session["Role"].ToString() == "Admin")
            {
                Session["Rolelist"] = new SelectList(db.Roles,"RoleId","RoleName");
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [Authorize]
        [HttpPost]
        public ActionResult NewUser([Bind(Include ="UserId,Name,RoleId")]User user)
        {
            if(Session["Role"].ToString()=="Admin")
            {
                if (ModelState.IsValid)
                {
                    string password = "User@123";
                    user.Password = HomeController.encrypt(password);
                    user.Confirmpassword = HomeController.encrypt(password);
                    db.Users.Add(user);
                    db.SaveChanges();
                }    
                else
                {
                    ModelState.AddModelError("", "Invalid Data Format.");
                }
                return View(user);
            }
            else
            return RedirectToAction("Index", "Home");
        }

    }
}