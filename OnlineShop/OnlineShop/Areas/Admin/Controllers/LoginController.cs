﻿using Model.DAO;
using OnlineShop.Areas.Admin.Models;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        public  ActionResult Login(LoginModel model )
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDAO();
                var result = dao.Login(model.UserName, model.Password);
                if (result)
                {
                    var user = dao.getByID(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;

                    Session.Add(CommonConstants.USER_SESSION,userSession);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Dang nhap khong thanh cong ");
                }
            }
            return View("Index");
        }
    }
}