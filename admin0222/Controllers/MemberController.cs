using admin0222.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace admin0222.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        dbShoppingCarEntities db = new dbShoppingCarEntities();
        // GET: Member
        public ActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml", "_LayoutMember");
        }

    }
}