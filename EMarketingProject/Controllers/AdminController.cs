using EMarketingProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using PagedList.Mvc;
using PagedList;

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
            if (Session["admin_id"]==null)
            {
                return RedirectToAction("login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Create(tbl_category cvm,HttpPostedFileBase imgFile)
        {
            string path=UploadImage(imgFile);
            if(path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded....";
            }
            else
            {
                tbl_category cat = new tbl_category();
                cat.category_Name = cvm.category_Name;
                cat.category_image = path;
                cat.category_foreginKey_admin=Convert.ToInt32(Session["admin_id"].ToString());
                db.tbl_category.Add(cat);
                db.SaveChanges();
                return RedirectToAction("Create");

            }
            return View();
        }
        public ActionResult CategoryDashboard(int?page)
        {
            int pageSize = 5, pageIndex = 1;
            pageIndex=page.HasValue ? Convert.ToInt32(page):1;
            var list=db.tbl_category.Where(x=>x.category_foreginKey_admin==1).ToList();
            IPagedList<tbl_category> stu = list.ToPagedList(pageIndex, pageSize);
            return View(stu);
        }

        //------------------code for upload image------------------------------------------
        public string UploadImage(HttpPostedFileBase file)
        {
            Random rand= new Random();
            string path = "-1";
            int random = rand.Next();
            if(file != null && file.ContentLength>0) {
                string extension=Path.GetExtension(file.FileName);
                if(extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);
                    }
                    catch(Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert(`only jpg, jpeg, png formate are accepted....`);</script>");
                }
            }
            else
            {
                Response.Write("<script>alert(`please select a file`);</script>");
                path= "-1";
            }
            return path;
        }
    }
}