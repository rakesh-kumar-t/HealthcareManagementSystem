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
            if (Session["UserId"] != null &&  Session["Role"].ToString() == "Admin")
            {
                return View(db.Members.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize]
        public ActionResult AddMember()
        {
            if (Session["UserId"] != null && Session["Role"].ToString() == "Admin")
                return View();
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [Authorize]
        public ActionResult AddMember(Member member)
        {
            if (Session["UserId"] != null && Session["Role"].ToString() == "Admin")
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
        public  ActionResult ViewMember(int? id)
        {
            if (Session["UserId"] != null && Session["Role"].ToString() == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                Member mem = db.Members.Find(id);
                if (mem == null)
                {
                    return HttpNotFound();
                }
                return View(mem);
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
            if(Session["UserId"] != null && Session["Role"].ToString() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    string password = "User@123";//Default password for Every User , User can change password after login
                    user.Password = HomeController.Encrypt(password);
                    user.Confirmpassword = HomeController.Encrypt(password);
                    db.Users.Add(user);
                    db.SaveChanges();
                    ViewBag.Status = "success";
                    ViewBag.Message = "User Added Successfully";
                }    
                else
                {
                    ViewBag.Status = "danger";
                    ViewBag.Message = "Error adding the user";
                    ModelState.AddModelError("", "Invalid Data Format.");
                }
                return View(user);
            }
            else
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult EditMember(int? id)
        {
            if(Session["UserId"] != null && Session["Role"].ToString() == "Admin")
            {
                if(id==null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                Member member = db.Members.Find(id);
                if (member == null)
                {
                    return HttpNotFound();
                }
                return View(member);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //Change password will redirect to home controller common method
        [Authorize]
        public ActionResult ChangePassword()
        {
            if (Session["UserId"] != null && Session["Role"].ToString() == "Admin")
                return RedirectToAction("ChangePassword", "Home");
            else
                return RedirectToAction("Index", "Home");
        }
        
        [Authorize]
        public ActionResult ViewStock()
        {
            if(Session["UserId"] != null && Session["Role"].ToString() == "Admin")
            {
                return View(db.DrugHouses.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize]
        public ActionResult AddStock()
        {
            if (Session["UserId"] != null && Session["Role"].ToString() == "Admin")
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddStock(DrugHouse drug)
        {
            if (Session["UserId"] != null && Session["Role"].ToString() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    db.DrugHouses.Add(drug);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Data Format.");
                }
                return View(drug);
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult ViewReport()
        {
            if (Session["UserId"] != null && Session["Role"].ToString() == "Admin")
            {
                return RedirectToAction("ViewReport", "Pharmacy");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }








        //Dispose the database
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