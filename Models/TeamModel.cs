using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ProjectManagementMadeEasyApp.Authorization;
using ProjectManagementMadeEasyApp.DAL;
using ProjectManagementMadeEasyApp.ViewModels;

namespace ProjectManagementMadeEasyApp.Models
{
    public class TeamModel
    {
        public int Team_ID { get; set; }
        public int Proj_ID { get; set; }
        public string Team_Name { get; set; }
        public int Team_Leader { get; set; }

        public List<UserModel> Users { get; set; }

        public List<TeamModel> Teams { get; set; }
    }
}