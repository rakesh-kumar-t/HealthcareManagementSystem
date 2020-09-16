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


namespace HealthcareManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private HealthCareContext db = new HealthCareContext();
        // GET: Admin
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult AddMember()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult AddMember(Member member)
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
        public ActionResult ViewMembers()
        {
            return View(db.Members.ToList());
        }

    }
}