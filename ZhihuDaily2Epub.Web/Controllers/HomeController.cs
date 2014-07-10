using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZhihuDaily2Epub.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Zhihu()
        {
            ViewBag.Files =Directory.GetFiles(Server.MapPath("/epub"),"*.epub");
            return View();
        }

    }
}
