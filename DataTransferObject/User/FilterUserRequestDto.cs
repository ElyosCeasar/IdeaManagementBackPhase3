using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject.User
{
    public class FilterUserRequestDto
    {
        public string FullName { set; get; }
        public string Username { set; get; }
        public int RoleValue { set; get; }
    }
}
