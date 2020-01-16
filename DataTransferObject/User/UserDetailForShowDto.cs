using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject.User
{
    public class UserDetailForShowDto
    {
        public string Username { get; set; }
        public bool ComitteeFlag { get; set; }
        public bool AdminFlag { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public System.DateTime SaveDate { get; set; }
    }
}
