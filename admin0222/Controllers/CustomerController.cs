using admin0222.Extension;
using admin0222.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace admin0222.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Read()
        { //將所有會員資料顯示於清單
            var db = new dbShoppingCarEntities();
            var members = db.Customer.OrderByDescending(m => m.Id).ToList();
            return View(members);
        }

        public ActionResult Export()
        {
            var db = new dbShoppingCarEntities();//建立了一個連接的資料庫上下文物件
            var customers = db.Customer.OrderByDescending(m => m.Id).ToList();//查詢資料庫中的 Customer 資料表，獲取客戶資料，按照 Id 屬性的降序排序。將這些資料存入 customers 變數中

            List<Customer> customerList = new List<Customer>();//建立了一個空的 customerList 串列，用於存儲轉換後的客戶資料
            foreach (var customer in customers) //使用 foreach 迴圈遍歷 customers 中的每個客戶資料，將其轉換為 Customer 物件
            {
                Customer objCustomer = new Customer()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Addrress = customer.Addrress,
                    Code = customer.Code,
                    Tel = customer.Tel,
                    Date = customer.Date
                };
                customerList.Add(objCustomer); //將其加入到 customerList 串列中
            }

            var viewModel = new BigView1 //建立了一個 BigView1 物件 viewModel
            {
                Customers = customerList //轉換後的客戶資料 customerList 賦值給 Customers 屬性
            };

            var fileName = "0530_" + Guid.NewGuid().ToString() + ".xlsx";
            var guidFileName2 = viewModel.ExportExcel<BigView1>(fileName);//將 viewModel 物件匯出為 Excel 檔案
            return File(guidFileName2, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName); //使用 File() 方法回傳匯出的 Excel 檔案

        }


        
        public ActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            var db = new dbShoppingCarEntities();
            db.Customer.Add(customer); 
            //使用Add()方法新增資料後，必須使用SaveChanges()方法才會將變更的內容儲存，同理在做Edit與Delete的時候也一樣。
            db.SaveChanges();

            return RedirectToAction("Read");
            //return json()
        }

        public ActionResult Edit(int id)
        {
            var db = new dbShoppingCarEntities();
            var member = db.Customer.Where(m => m.Id == id).FirstOrDefault(); //傳回序列的第一個項目；如果找不到任何項目，則傳回預設值。
            //也可寫成下面
            //var member = db.Member.FirstOrDefault(m => m.Id == id);
            return View(member);

            //LINQ查詢Member物件中符合該唯一id的資料
        }
        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            var db = new dbShoppingCarEntities();
            var memberData = db.Customer.Where(m => m.Id == customer.Id).FirstOrDefault();
            memberData.Id = customer.Id;
            memberData.Name = customer.Name;
            memberData.Tel = customer.Tel;
            memberData.Addrress = customer.Addrress;
            memberData.Code = customer.Code;
            db.SaveChanges();
            //將修改送出後的物件屬性一一賦值回該筆資料，
            //最後記得使用SaveChanges()儲存變更

            return RedirectToAction("Read");
        }

        public ActionResult Delete(int id)
        {
            var db = new dbShoppingCarEntities();
            var member = db.Customer.Where(m => m.Id == id).FirstOrDefault();
            return View(member);
        }

        [HttpPost]
        public ActionResult Delete(Customer member)
        {
            var db = new dbShoppingCarEntities();
            var memberData = db.Customer.Where(m => m.Id == member.Id).FirstOrDefault();
            db.Customer.Remove(memberData);
            //使用Remove()來移除選擇的資料，並執行SaveChange()
            db.SaveChanges();

            return RedirectToAction("Read");
        }


    }
}