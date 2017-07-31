using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementMadeEasyApp.Models
{
    public class CustomPrincipalSerializeModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TeamId { get; set; }
        public string Email { get; set; }
    }
}