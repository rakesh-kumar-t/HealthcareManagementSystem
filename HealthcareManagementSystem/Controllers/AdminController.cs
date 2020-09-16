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
        public  ActionResult ViewMember(int? id)
        {
            if (Session["Role"].ToString() == "Admin")
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
        [Authorize]
        public ActionResult EditMember(int? opno)
        {
            if(Session["Role"].ToString()=="Admin")
            {
                if(opno==null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                Member m = db.Members.Find(opno);
                if (m == null)
                {
                    return HttpNotFound();
                }
                return View(m);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize]
        public ActionResult ViewStock()
        {
            if(Session["Role"].ToString()=="Manager")
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
            if (Session["UserId"] != null && Session["Role"].ToString() == "Manager")
            {
                Session["DrugStock"] = new SelectList(db.DrugHouses, "DrugId", "Name","ExpiryDate");
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddStock(DrugHouse drug)
        {
            if (Session["Role"].ToString() == "Manager")
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
            if (Session["Role"].ToString() == "Manager")
            {
                return View(db.Pharmacies.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


    }
}