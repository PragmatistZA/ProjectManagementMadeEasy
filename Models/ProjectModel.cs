using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ProjectManagementMadeEasyApp.DAL;
using ProjectManagementMadeEasyApp.ViewModels;

namespace ProjectManagementMadeEasyApp.Models
{
    public class ProjectModel
    {
        public int Proj_ID { get; set; }

        [Display(Name = "Project Description:")]
        public string Proj_Desc { get; set; }

        [Display(Name = "Project Due Date:")]
        public DateTime Proj_Due { get; set; }
        public int Proj_Manager_ID { get; set; }

        [Display(Name = "Project Name:")]
        public string Project_Name { get; set; }
    }
}