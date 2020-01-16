using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject.User
{
    public class IdeaCommentsDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public int IdeaId { get; set; }
        public string Comment { get; set; }
        public System.DateTime SaveDate { get; set; }

        public int Points { get; set; }
    }
}
