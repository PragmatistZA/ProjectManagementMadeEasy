using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using ProjectManagementMadeEasyApp.DAL;
using ProjectManagementMadeEasyApp.Models;
using ProjectManagementMadeEasyApp.ViewModels;

namespace ProjectManagementMadeEasyApp.Controllers
{
    public class TeamsController : Controller
    {
        // GET: Teams
        public ActionResult Index()
        {
            return View();
        }
    }
}