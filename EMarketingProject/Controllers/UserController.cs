using EMarketingProject.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace EMarketingProject.Controllers
{
    public class UserController : Controller
    {
        db_MarketingEntities db = new db_MarketingEntities();
        // GET: User
        public ActionResult Index(int?page)
        {
            int pageSize = 5, pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_category.Where(x => x.category_foreginKey_admin > 0).OrderByDescending(x => x.category_id).ToList();
            IPagedList<tbl_category> stu = list.ToPagedList(pageIndex, pageSize);
            return View(stu);
        }
        [HttpGet]
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(tbl_user uvm)
        {
            tbl_user usr = db.tbl_user.Where(x => x.user_Name == uvm.user_Name && x.user_password == uvm.user_password).SingleOrDefault();
            if (usr != null)
            {
                Session["user_id"] = usr.user_id.ToString();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "Invalid Username or Password";
            }
            return View();
        }
    }
}