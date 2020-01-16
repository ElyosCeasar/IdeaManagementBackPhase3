using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject.Comment
{
    public class CommentDto
    {
        public string Username { get; set; }
        public int IdeaId { get; set; }
        public string Comment { get; set; }

    }
}
