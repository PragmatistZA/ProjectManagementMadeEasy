using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using ProjectManagementMadeEasyApp.Models;
using ProjectManagementMadeEasyApp.Authorization;
using ProjectManagementMadeEasyApp.DAL;
using ProjectManagementMadeEasyApp.ViewModels;
using Newtonsoft.Json;

namespace ProjectManagementMadeEasyApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly WebService _ws = new WebService();

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                CustomPrincipal customPrincipal = (CustomPrincipal) User;
                if (customPrincipal != null && customPrincipal.TeamId != 0)
                {
                    TeamModel team = GetTeam(((CustomPrincipal) User).TeamId);

                    ProjectModel projectView = TeamProjects(team.Team_ID);

                    ViewBag.Team = "You are in the team: " + team.Team_Name;

                    if (projectView.Project_Name != null)
                    {
                        ViewBag.Project = "Your current project is: " + projectView.Project_Name;
                    }
                    else
                    {
                        ViewBag.Project = "You have no current projects.";
                    }
                }
                else
                {
                    ViewBag.Team = "You are not part of a team! Join a team or create your own.";
                    ViewBag.Project = "You have no current projects.";
                }
            
                return View();
            }
            return RedirectToAction("Login", "Users");
        }

        [HttpGet]
        public ActionResult MyTeam()
        {
            if (User.Identity.IsAuthenticated)
            {
                CustomPrincipal customPrincipal = (CustomPrincipal) User;
                if (customPrincipal == null || (customPrincipal.TeamId == 0))
                    return RedirectToAction("MyTeamRegister");
                TeamModel team = GetTeam(((CustomPrincipal) User).TeamId);

                var userList = TeamMembers(((CustomPrincipal) User).TeamId);

                team.Users = userList;

                MyTeamViewModel teamView = new MyTeamViewModel
                {
                    Team_Name = team.Team_Name,
                    Users = team.Users
                };

                foreach (UserModel user in userList)
                {
                    if (user.User_ID == team.Team_Leader)
                    {
                        teamView.Team_Leader = user.First_Name + " " + user.Last_Name;
                    }
                }

                for (int i = 0; i < userList.Count; i++)
                {
                    UserModel user = userList[i];
                    if (user.User_ID == team.Team_Leader)
                    {
                        teamView.Users.Remove(user);
                    }
                }

                ViewBag.Team_Name = teamView.Team_Name;
                ViewBag.Team_Leader = teamView.Team_Leader;

                return View(teamView.Users);
            }

            return RedirectToAction("MyTeamRegister");
        }

        [HttpGet]
        public ActionResult MyTeamRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MyTeamRegister(UserModel user)
        {
            CustomPrincipal customPrincipal = (CustomPrincipal) User;
            if (customPrincipal != null) user.Email = customPrincipal.Email;
            if (RegisterTeam(user.Email, user.Team_Name))
            {
                user = GetUser(user.Email);
                user.Team_ID = GetUserTeamID(user.Email);
                updateLogin(user);
                return RedirectToAction("MyTeam","Dashboard");
            }

            return View("MyTeamRegister");
        }

        [HttpGet]
        public ActionResult MyProject()
        {
            CustomPrincipal customPrincipal = (CustomPrincipal) User;
            if (customPrincipal == null || (customPrincipal.TeamId == 0)) return RedirectToAction("MyTeamRegister");
 
            ProjectModel userProject = TeamProjects(customPrincipal.TeamId);
            if (userProject.Proj_ID != 0)
            {
                return View(userProject);
            }
            return RedirectToAction("MyProjectCreate");
        }

        [HttpPost]
        public ActionResult MyProject(UserModel user)
        {
            CustomPrincipal customPrincipal = (CustomPrincipal) User;
            if (customPrincipal == null || customPrincipal.TeamId == 0) return RedirectToAction("MyTeamRegister");
            TeamModel team = GetTeam(((CustomPrincipal) User).TeamId);

            ProjectModel projectView = TeamProjects(team.Team_ID);

            if (projectView.Proj_ID != 0)
            {
                return View(projectView);
            }

            return RedirectToAction("MyTeamRegister");
        }

        [HttpGet]
        public ActionResult MyProjectCreate()
        {
            return View("MyProjectCreate");
        }

        [HttpPost]
        public ActionResult MyProjectCreate(ProjectModel projectView)
        {
            CustomPrincipal customPrincipal = (CustomPrincipal) User;
            if (customPrincipal != null && CreateNewProject(customPrincipal.Id, customPrincipal.TeamId, projectView.Proj_Desc, projectView.Project_Name) == "Success")
            {
                ViewBag.Result = "Project created successfully!";
                return RedirectToAction("MyProject", "Dashboard");
            }
            ViewBag.Result = "Failed to create Project.";
            return View("Index");
        }

        [HttpGet]
        public ActionResult MyTasks()
        {
            CustomPrincipal customPrincipal = (CustomPrincipal) User;
            if (customPrincipal == null || customPrincipal.TeamId == 0) return RedirectToAction("MyTeamRegister");
            var tasks = AllTasks(((CustomPrincipal) User).TeamId);

            return View("MyTasks", tasks);
        }

        [HttpGet]
        public ActionResult RemoveTask(TaskModel task)
        {
            DeleteTask(task.Task_ID);
            return RedirectToAction("MyTasks");
        }

        [HttpGet]
        public ActionResult FindTeam()
        {
            List<UserModel> users = GetAllUsers();
            List<TeamModel> teams = new List<TeamModel>();
            List<Int32> idList = new List<Int32>();
            idList.Add(0);
            foreach (var user in users)
            {
                if (!idList.Contains(user.Team_ID))
                {
                    idList.Add(user.Team_ID);
                    teams.Add(GetTeam(user.Team_ID));
                }
            }

            return View(teams);
        }

        [HttpGet]
        public ActionResult JoinTeam(TeamModel team)
        {
            CustomPrincipal customPrincipal = (CustomPrincipal)User;
            UserModel user = GetUser(customPrincipal.Email);
            
            if (AddUserToTeam(user.User_ID, team.Team_ID))
            {
                user.Team_ID = team.Team_ID;
                updateLogin(user);
                return RedirectToAction("MyTeam");
            }
            
            return View("MyTeam");
        }

        [HttpGet]
        public ActionResult AddTask(UserModel user)
        {
            CustomPrincipal customPrincipal = (CustomPrincipal) User;
            if (customPrincipal != null && customPrincipal.TeamId != 0)
            {
                return View("AddTask");
            }

            return View();
        }

        [HttpPost]
        public ActionResult AddTask(TaskModel task)
        {
            if (!ModelState.IsValid) return View("AddTask");
            CustomPrincipal customPrincipal = (CustomPrincipal) User;
            UserModel user = GetUser(customPrincipal.Email);
            TeamModel team = GetTeam(user.Team_ID);
            task.Project_ID = team.Proj_ID;
            if (AddTask(task.Project_ID, task.Task_Desc, task.Time_Req, task.Task_Due))
            {
                return RedirectToAction("MyTasks");
            }

            return View("AddTask");
        }

        public ProjectModel TeamProjects(int teamId)
        {
            ProjectModel projectView = new ProjectModel();

            var jsonContentTeam = _ws.GetTeam(teamId);
            if (jsonContentTeam != "[]")
            {
                var team = JsonConvert.DeserializeObject<TeamModel[]>(jsonContentTeam,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                var jsonContentProjects = _ws.AllProjects();
                var projects = JsonConvert.DeserializeObject<ProjectModel[]>(jsonContentProjects,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                foreach (ProjectModel project in projects)
                {
                    if (team[0].Proj_ID == project.Proj_ID)
                    {
                        projectView = project;
                    }
                }
                return projectView;
            }
            ProjectModel tempProject = new ProjectModel();
            return tempProject;
        }

        public string CreateNewProject(int projectManagerId, int teamId, string projectDesc, string projectName)
        {
            string result = _ws.CreateProject(projectManagerId, teamId, projectDesc, projectName);

            return result;
        }

        public List<TaskModel> ProjectTasks(string projectName)
        {
            var jsonContentProjects = _ws.ProjectTasks(projectName);
            var tasks = JsonConvert.DeserializeObject<TaskModel[]>(jsonContentProjects, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var taskList = tasks.ToList();

            return taskList;
        }

        public List<TaskModel> AllTasks(int teamId)
        {
            ProjectModel projectView = new ProjectModel();

            var jsonContentTeam = _ws.GetTeam(teamId);
            var team = JsonConvert.DeserializeObject<TeamModel[]>(jsonContentTeam, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            var jsonContentProjects = _ws.AllProjects();
            var projects = JsonConvert.DeserializeObject<ProjectModel[]>(jsonContentProjects, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            foreach (ProjectModel project in projects)
            {
                if (team[0].Proj_ID == project.Proj_ID)
                {
                    projectView = project;
                }
            }

            var jsonContentTasks = _ws.AllTasks();
            var tasks = JsonConvert.DeserializeObject<TaskModel[]>(jsonContentTasks, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var taskList = new List<TaskModel>();
            foreach (TaskModel task in tasks)
            {
                if (projectView.Proj_ID == task.Project_ID)
                {
                    taskList.Add(task);
                }
            }

            return taskList;
        }

        public bool AddTask(int projectId, string projectDesc, string timeReq, DateTime taskDue)
        {
            string taskDueDate = taskDue.ToString("yyyy-MM-dd");
            DateTime taskCreated = DateTime.Today;
            string taskCreatedDate = taskCreated.ToString("yyyy-MM-dd");
            string result = _ws.AddTask(projectId, projectDesc, timeReq, taskCreatedDate, taskDueDate);

            switch (result)
            {
                case "fail":
                    return false;
                case "Fail":
                    return false;
            }
            return true;
        }

        public TeamModel UserTeam(int userId)
        {
            TeamViewModel teamList = TeamList();
            TeamModel userTeam = new TeamModel();
            foreach (TeamModel team in teamList.Teams)
            {
                foreach (UserModel users in team.Users)
                {
                    if (userId == users.User_ID)
                    {
                        userTeam = team;
                    }
                }
            }

            return userTeam;
        }

        public TeamViewModel TeamList()
        {
            var jsonContent = _ws.GetListOfTeams();
            var team = JsonConvert.DeserializeObject<TeamModel[]>(jsonContent,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            List<TeamModel> teamList = team.ToList();
            TeamViewModel teams = new TeamViewModel() { Teams = teamList };
            return teams;
        }

        public List<TeamModel> AllTeams()
        {
            var jsonContent = _ws.GetListOfTeams();
            var team = JsonConvert.DeserializeObject<TeamModel[]>(jsonContent,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            List<TeamModel> teamList = team.ToList();
            return teamList;
        }

        public TeamModel GetTeam(int teamId)
        {
            var jsonContent = _ws.GetTeam(teamId);
            if (jsonContent != "[]")
            {
                var team = JsonConvert.DeserializeObject<TeamModel[]>(jsonContent,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                return team[0];
            }
            TeamModel teamTemp = new TeamModel();
            return teamTemp;
        }

        public List<UserModel> TeamMembers(int teamId)
        {
            var jsonContent = _ws.TeamMembers(teamId);
            var users = JsonConvert.DeserializeObject<UserModel[]>(jsonContent,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var userList = users.ToList();
            return userList;
        }

        public bool RegisterTeam(string email, string teamName)
        {
            string result = _ws.AddUserTeam(email, teamName);
            switch (result)
            {
                case "Success":
                    return true;
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

        public int GetUserTeamID(string email)
        {
            UserModel user = GetUser(email);
            return user.Team_ID;
        }

        public void updateLogin(UserModel user)
        {
            CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel
            {
                Id = user.User_ID,
                FirstName = user.First_Name,
                LastName = user.Last_Name,
                Email = user.Email,
                TeamId = user.Team_ID
            };

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string userData = serializer.Serialize(serializeModel);

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                1,
                user.Email,
                DateTime.Now,
                DateTime.Now.AddMinutes(15),
                false,
                userData);

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(faCookie);
        }

        public bool DeleteTask(int taskId)
        {
            string result = _ws.DeleteTask(taskId);
            switch (result)
            {
                case "Success":
                    return true;
                case "Fail":
                    return false;
            }
            return false;
        }

        public List<UserModel> GetAllUsers()
        {
            var jsonContent = _ws.GetAllUsers();
            var users = JsonConvert.DeserializeObject<UserModel[]>(jsonContent,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var userList = users.ToList();
            return userList;
        }

        public bool AddUserToTeam(int userId, int teamId)
        {
            string result = _ws.JoinTeam(userId, teamId);
            switch (result)
            {
                case "Success":
                    return true;
                case "Fail":
                    return false;
            }
            return false;
        }
    }
}