using EMarketingProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMarketingProject.Controllers
{
    public class AdminController : Controller
    {
        db_MarketingEntities db=new db_MarketingEntities();
        // GET: Admin
        [HttpGet]
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(tbl_admin avm)
        {
            tbl_admin ad=db.tbl_admin.Where(x=>x.admin_userName == avm.admin_userName && x.admin_password==avm.admin_password).SingleOrDefault();
            if(ad!=null)
            {
                Session["admin_id"]=ad.admin_id.ToString();
                return RedirectToAction("Create");
            }
            else
            {
                ViewBag.error = "Invalid Username or Password";
            }
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}