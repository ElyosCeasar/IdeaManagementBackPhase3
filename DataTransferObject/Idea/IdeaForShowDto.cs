using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject.Idea
{
    public class IdeaForShowDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public byte StatusId { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string SaveDate { get; set; }
    }
}
