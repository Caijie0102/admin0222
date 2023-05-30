using admin0222.Models;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace admin0222.Controllers
{
    //static responseFormats=["json"];

    [Authorize]
  
    public class MemberController : Controller
    {
        dbShoppingCarEntities db = new dbShoppingCarEntities();
        private const string COOKIE_NAME = "myCookie"; // 在這裡定義 COOKIE_NAME

        // GET: Member
        public ActionResult Index()
        {

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }

            string userId = null;
            string password = null;


            // 檢查是否存在 Cookie，如果存在則讀取cookie中的用戶ID和密碼
            if (Request.Cookies[COOKIE_NAME] != null)
            { 
                userId = Request.Cookies[COOKIE_NAME]["UserId"];
                password = Request.Cookies[COOKIE_NAME]["Password"];
            }
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(password))
            {
                // 如果不存在持久 Cookie，則使用 Session
                ViewBag.Message = Session["Welcome"];

                // 如果cookie中沒有用戶ID或密碼，重新導向到登錄頁面
                return RedirectToAction("Login", "Home");
            }
            else
            {    // 如果存在持久 Cookie，則使用 Cookie //根據用戶ID和密碼查詢 Member 資料
                var member = db.table_Member.Where(m => m.UserId == userId && m.Password == password).FirstOrDefault();
                if (member == null)
                { // 如果找不到 Member，則刪除 Cookie / 如果沒有查詢到符合條件的 Member 資料，重新導向到登錄頁面
                    Response.Cookies[COOKIE_NAME]?.Expires.AddDays(-1);
                    ViewBag.Message = Session["Welcome"];
                    return RedirectToAction("Login", "Home");
                }
                else
                { ViewBag.Message = $"{member.Name} 您好"; }


            }
            return View("~/Views/Home/Index.cshtml", "_LayoutMember");




        }

    }
}