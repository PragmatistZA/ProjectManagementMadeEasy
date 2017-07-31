using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectManagementMadeEasyApp.ViewModels
{
    public class ProfileViewModel
    {
        [Display(Name = "Title:")]
        public string Title { get; set; }

        [Display(Name = "First Name:")]
        public string First_Name { get; set; }

        [Display(Name = "Last Name:")]
        public string Last_Name { get; set; }

        [Display(Name = "Email:")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password:")]
        public string Password { get; set; }

        public string Contact { get; set; }

        [Display(Name = "Job Description:")]
        public string Job_Desc { get; set; }
    }
}