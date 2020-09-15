using HealthcareManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace HealthcareManagementSystem.Controllers
{
    public class PharmacyController : Controller
    {
        // GET: Pharmacy
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewPharmacy()
        {
            HealthCareContext db = new HealthCareContext();
            return View(db.Pharmastocks.ToList());
        }
    }
}