using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObject.User
{
    public class UserForShowDto
    {
        public string Username { get; set; }
        public bool CommitteFlag { get; set; }
        public bool AdminFlag { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public System.DateTime SaveDate { get; set; }
    }
}