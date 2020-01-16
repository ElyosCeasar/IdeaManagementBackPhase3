using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject.Comment
{
    public class VoteToCommentDto
    {
     
        public string UsernameVoter { get; set; }
        public int CommentId { get; set; }
        public int Point { get; set; }
    }
}
