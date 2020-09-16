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
        private HealthCareContext db = new HealthCareContext();

        // GET: Pharmacy
        [Authorize]
        public ActionResult Index()
        {
            if (Session["UserId"] != null && Session["Role"].ToString() == "Manager")
                return View(db.Pharmastocks.ToList());
            else
                return RedirectToAction("Index", "Home");
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