using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject.Idea
{
    public class IdeaDto
    {
        public string Title { get; set; }
        public int TotalPoints { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Status { get; set; } 
        public string SaveDate { get; set; }
    }
}
