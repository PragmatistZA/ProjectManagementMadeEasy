using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ProjectManagementMadeEasyApp.DAL;


namespace ProjectManagementMadeEasyApp.Models
{
    public class UserModel
    {
        public int User_ID { get; set; }

        public int Team_ID { get; set; }

        public string Team_Name { get; set; }

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

        public string Job_Desc { get; set; }
    }
}