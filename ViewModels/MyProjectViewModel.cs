using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectManagementMadeEasyApp.ViewModels
{
    public class MyProjectViewModel
    {
        [Display(Name = "Project Name:")]
        public string Proj_Name { get; set; }

        [Display(Name="Project Description:")]
        public string Proj_Desc { get; set; }

        [Display(Name = "Project Due Date:")]
        public string Proj_Due { get; set; }

        [Display(Name = "Project Manager:")]
        public string Manager_Name { get; set; }
    }
}