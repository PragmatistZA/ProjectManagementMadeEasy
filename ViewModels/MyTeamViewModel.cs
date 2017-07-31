using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ProjectManagementMadeEasyApp.DAL;
using ProjectManagementMadeEasyApp.Models;

namespace ProjectManagementMadeEasyApp.ViewModels
{
    public class MyTeamViewModel
    {
        [Display(Name="Team Name:")]
        public string Team_Name { get; set; }

        [Display(Name = "Team Leader:")]
        public string Team_Leader { get; set; }

        public string Email { get; set; }

        [Display(Name = "Member Name:")]
        public List<UserModel> Users { get; set; }


    }
}