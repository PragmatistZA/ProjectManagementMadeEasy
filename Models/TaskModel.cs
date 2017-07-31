using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ProjectManagementMadeEasyApp.DAL;

namespace ProjectManagementMadeEasyApp.Models
{
    public class TaskModel
    {
        public int Task_ID { get; set; }
        public int Project_ID { get; set; }

        [Required]
        [Display(Name = "Task Description:")]
        public string Task_Desc { get; set; }

        [Required]
        [Display(Name = "Hours Required:")]
        public string Time_Req { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Created:")]
        public DateTime Task_Created { get; set; }

        [Required(ErrorMessage = "Enter the due date.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Due By:")]
        public DateTime Task_Due { get; set; }

        [Display(Name = "Hours Spent:")]
        public string Status { get; set; }

        [Display(Name = "Completed:")]
        public bool Task_Complete { get; set; }

        public ICollection<TaskModel> Tasks { get; set; }
    }
}