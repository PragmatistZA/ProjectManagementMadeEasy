using ProjectManagementMadeEasyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.WebPages;
using System.Xml;
using ProjectManagementMadeEasyApp.DAL;


namespace ProjectManagementMadeEasyApp.Controllers
{
    public partial class HomeController : Controller
    {

        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult About()
        {           
            return View();
        }

        public virtual ActionResult Contact()
        {
            return View();
        }
    }
}