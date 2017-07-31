using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using MySql.Data.MySqlClient;
using System.Xml;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Security;
using Microsoft.Ajax.Utilities;
using ProjectManagementMadeEasyApp.DAL;
using ProjectManagementMadeEasyApp.Models;
using Newtonsoft.Json;
using ProjectManagementMadeEasyApp.Authorization;
using ProjectManagementMadeEasyApp.ViewModels;

namespace ProjectManagementMadeEasyApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly WebService _ws = new WebService();

        // GET: /User/
        public ActionResult Index()
        {
            return RedirectToAction("Login", "Users");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (IsAuthenticated(viewModel.Email, viewModel.Password))
                {
                    var jsonContent = _ws.GetUser(viewModel.Email);
                    var user = JsonConvert.DeserializeObject<UserModel[]>(jsonContent,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel
                    {
                        Id = user[0].User_ID,
                        FirstName = user[0].First_Name,
                        LastName = user[0].Last_Name,
                        Email = user[0].Email,
                        TeamId = user[0].Team_ID
                    };

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    string userData = serializer.Serialize(serializeModel);

                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                        1,
                        user[0].Email,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(15),
                        false,
                        userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);

                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Error = "Incorrect details.";
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel newUser)
        {
            if (RegisterUser(newUser.First_Name, newUser.Last_Name, newUser.Email, newUser.Password))
            {
                var jsonContent = _ws.GetUser(newUser.Email);
                var viewModel = JsonConvert.DeserializeObject<UserModel[]>(jsonContent,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel
                {
                    Id = viewModel[0].User_ID,
                    FirstName = viewModel[0].First_Name,
                    LastName = viewModel[0].Last_Name,
                    Email = viewModel[0].Email,
                    TeamId = viewModel[0].Team_ID
                };



                JavaScriptSerializer serializer = new JavaScriptSerializer();

                string userData = serializer.Serialize(serializeModel);

                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1,
                    newUser.Email,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(15),
                    false,
                    userData);

                string encTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                Response.Cookies.Add(faCookie);

                return RedirectToAction("Index", "Home");

                //Login(viewModel);
            }
            ViewBag.Error = "An error has occurred.";
            return View();
        }

        [HttpGet]
        public ActionResult Profile()
        {
            UserModel user = new UserModel();
            CustomPrincipal customPrincipal = (CustomPrincipal)User;
            if (customPrincipal != null) user = GetUser(customPrincipal.Email);
            ProfileViewModel profileView = new ProfileViewModel
            {
                Email = user.Email,
                Contact = user.Contact,
                First_Name = user.First_Name,
                Job_Desc = user.Job_Desc,
                Last_Name = user.Last_Name,
                Password = user.Password,
                Title = user.Title
            };
            return View(profileView);
        }

        [HttpPost]
        public ActionResult Profile(UserModel user)
        {
            CustomPrincipal customPrincipal = (CustomPrincipal) User;
            if (customPrincipal != null) user.User_ID = customPrincipal.Id;
            if (UpdateProfile(user.User_ID, user.Title, user.First_Name, user.Last_Name, user.Job_Desc, user.Email,
                user.Contact, user.Password))
            {
                return View();
            }
            return View();
        }

        public bool IsAuthenticated(string email, string password)
        {
            string result = _ws.VerifyUserLogin(email, password);
            switch (result)
            {
                case "Correct":
                    return true;
                case "Not Found":
                    return false;
                case "Incorrrect":
                    return false;
            }
            return false;
        }

        public bool UpdateProfile(int userId, string title, string firstName, string lastName, string jobDesc, string email, string contact, string password)
        {
            string result = _ws.UpdateUser(userId, title, firstName, lastName, jobDesc, email, contact, password);
            return result != "Fail";
        }

        public bool RegisterUser(string firstName, string lastName, string email, string password)
        {
            string result = _ws.AddNewUser(firstName, lastName, email, password);
            switch (result)
            {
                case "Success":
                    return true;
                case "Exists":
                    return false;
                case "Fail":
                    return false;
            }
            return false;
        }

        public UserModel GetUser(string email)
        {
            var jsonContent = _ws.GetUser(email);
            var user = JsonConvert.DeserializeObject<UserModel[]>(jsonContent,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return user[0];
        }
    }
}