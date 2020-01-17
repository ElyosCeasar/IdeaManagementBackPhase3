using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObject.User
{
    public class UserForProfileDto
    {
        public string Username { get; set; }
        public bool CommitteeFlag { get; set; }
        public bool AdminFlag { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SaveDate { get; set; }
    }
}