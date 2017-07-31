using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectManagementMadeEasyApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementMadeEasyApp.ViewModels
{
    public class UsersViewModel
    {
        [Display(Name = "Member Name:")]
        public string Name { get; set; }
    }
}