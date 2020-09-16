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
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult AddMember()
        {
            return View();
        }
        
    }
}