using HealthcareManagementSystem.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;


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
        [Authorize]
        public ActionResult AddStock()
        {
            if (Session["UserId"] != null && Session["Role"].ToString() == "Manager")
            {
                return View(db.DrugHouses.Where(d=>d.StockLeft>0).OrderBy(exp=>exp.ExpiryDate).ToList());
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddStock(int? DrugId,int? Itemno)
        {
            if (Session["UserId"] != null && Session["Role"].ToString() == "Manager")
            {
                if (DrugId==null || Itemno==null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                var Drug = db.DrugHouses.Find(DrugId);
                if (Drug == null)
                {
                    ViewBag.Status = "danger";
                    ViewBag.Message = "Drug Not found, Might have removed from stockhouse";
                    return HttpNotFound();
                }
                else
                {
                    var pharmacy = db.Pharmastocks.Where(medicine => medicine.DrugId == DrugId).FirstOrDefault();
                    if (pharmacy != null)
                    {
                        if (pharmacy.Stockleft > 0)
                        {
                            ViewBag.Status = "danger";
                            ViewBag.Message = "Stock cant be added when old stock remains";
                        }
                        else
                        {
                            pharmacy.Stockleft = pharmacy.Stockleft + (int)Itemno;
                            pharmacy.Expiry = Drug.ExpiryDate;
                            pharmacy.Dateadded = DateTime.Now;
                            db.Entry(pharmacy).State = EntityState.Modified;
                            db.SaveChanges();
                            Drug.StockLeft = Drug.StockLeft - (int)Itemno;
                            db.Entry(Drug).State = EntityState.Modified;
                            db.SaveChanges();
                            ViewBag.Status = "success";
                            ViewBag.Message = "Stock Updated successfully";
                        }
                    }
                    else
                    {
                        Pharmastock pharm = new Pharmastock();
                        pharm.DrugId=Drug.DrugId;
                        pharm.Stockleft = (int)Itemno;
                        pharm.Expiry = Drug.ExpiryDate;
                        pharm.Dateadded = DateTime.Now;
                        db.Pharmastocks.Add(pharm);
                        db.SaveChanges();
                        Drug.StockLeft = Drug.StockLeft - (int)Itemno;
                        db.Entry(Drug).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.Status = "success";
                        ViewBag.Message = "New drug added successfully";
                    }
                }
                return View(db.DrugHouses.Where(d => d.StockLeft > 0).OrderBy(exp => exp.ExpiryDate).ToList());
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult ViewReport()
        {
            if (Session["UserId"] != null && Session["Role"].ToString() == "Manager"||Session["Role"].ToString() == "Admin")
            {
                return View(db.Pharmacies.ToList());
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