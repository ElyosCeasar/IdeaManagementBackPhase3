using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObject.Auth
{
    public class UserForRegistrationDto
    {
        public string Username { get; set; }
        public string password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
     

    }
}