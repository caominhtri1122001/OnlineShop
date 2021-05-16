using Model.DAO;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Model.EF;

namespace OnlineShop.Controllers
{
    public class CartController : Controller
    {
        private const string CartSession = "CartSession";
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list =new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        public JsonResult DeleteAll()
        {
            Session[CartSession] = null;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult Delete(long ID)
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(x => x.Product.ID == ID);
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart =(List<CartItem>)Session[CartSession];

            foreach(var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
            }
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        public ActionResult AddItem(long productID , int quantity)
        {
            var product = new ProductDAO().ViewDetail(productID);
            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x=>x.Product.ID == productID))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ID == productID)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }  
                else
                {
                    //Tao moi doi tuong cart item
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                //Gan vao session
                Session[CartSession] = list;
            }
            else
            {
                //Tao moi doi tuong cart item
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                //Gan vao session
                Session[CartSession] = list;
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Payment()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult Payment(string shipName,string mobile,string address ,string email)
        {
            var order = new Order();
            order.CreatedDate = DateTime.Now;
            order.ShipAddress = address;
            order.ShipName = shipName;
            order.ShipMobile = mobile;
            order.ShipEmail = email;

            try
            {
                var id = new OrderDAO().Insert(order);
                var cart = (List<CartItem>)Session[CartSession];
                var detailDAO = new OrderDetailDAO();
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.ProductID = item.Product.ID;
                    orderDetail.OrderID = id;
                    orderDetail.Price = item.Product.Price;
                    orderDetail.Quantity = item.Quantity;
                    detailDAO.Insert(orderDetail);
                }
            }
            catch(Exception ex)
            {
                return Redirect("/loi-thanh-toan");
                throw;
            }

            return Redirect("/hoan-thanh");
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}