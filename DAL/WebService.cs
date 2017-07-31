using System;
using System.Globalization;
using System.Web.Mvc;
using ProjectManagementMadeEasyApp.Helpers;
using ProjectManagementMadeEasyApp.PMME_WS;

namespace ProjectManagementMadeEasyApp.DAL
{
    public class WebService
    {
        private readonly pmmeWSPortTypeClient _client = new pmmeWSPortTypeClient();

        [HttpPost]
        public string GetAllUsers()
        {
            string test = _client.getAllUsers();
            return test;
        }

        [HttpPost]
        public string GetListOfTeams()
        {
            string test = _client.getListOfTeams();
            return test;
        }

        [HttpPost]
        public string GetUser(string email)
        {
            string test = _client.getUser(email);
            return test;
        }

        //[HttpPost]
        public string VerifyUserLogin(string email, string password)
        {    
            string result = _client.verifyUserLogin(email.ToLower(), SHA1.Encode(password));
            return result;
        }

        [HttpPost]
        public string AddNewUser(string firstName, string lastName, string email, string password)
        {
            string result = _client.addNewUser(firstName, lastName, email.ToLower(), SHA1.Encode(password));
            return result;
        }

        [HttpPost]
        public string AddUserTeam(string email, string teamName)
        {
            string result = _client.userAddNewTeam(email, teamName);
            return result;
        }

        [HttpPost]
        public string UpdateUser(int userId, string title, string firstName, string lastName, string jobDesc, string email, string contact, string password)
        {
            string result = _client.editUser(Convert.ToString(userId), title, firstName, lastName, jobDesc, email, contact, SHA1.Encode(password));
            return result;
        }

        [HttpPost]
        public string GetTeam(int teamId)
        {
            string result = _client.getTeam(Convert.ToString(teamId));
            return result;
        }

        [HttpPost]
        public string TeamMembers(int teamId)
        {
            string result = _client.getTeamMembers(Convert.ToString(teamId));
            return result;
        }

        [HttpPost]
        public string TeamProjects(string teamName)
        {
            string result = _client.getTeamProjects(teamName);
            return result;
        }

        [HttpPost]
        public string AllProjects()
        {
            string result = _client.getAllProjects();
            return result;
        }

        [HttpPost]
        public string CreateProject(int projectManagerId, int teamId, string projectDesc, string projectName)
        {
            string result = _client.addNewProject(Convert.ToString(projectManagerId), Convert.ToString(teamId), projectDesc, projectName);
            return result;
        }

        [HttpPost]
        public string AllTasks()
        {
            string result = _client.getAllTasks();
            return result;
        }

        [HttpPost]
        public string ProjectTasks(string projectName)
        {
            string result = _client.getProjectTasks(projectName);
            return result;
        }

        [HttpPost]
        public string AddTask(int projectId, string projectDesc, string timeReq, string taskcreated, string taskdue)
        {
            string result = _client.addTask(Convert.ToString(projectId), projectDesc, timeReq, taskcreated, taskdue);
            return result;
        }

        [HttpPost]
        public string DeleteTask(int taskId)
        {
            string result = _client.deleteTask(Convert.ToString(taskId));
            return result;
        }

        [HttpPost]
        public string JoinTeam(int userId, int teamId)
        {
            string result = _client.joinTeam(Convert.ToString(userId), Convert.ToString(teamId));
            return result;
        }
    }
}