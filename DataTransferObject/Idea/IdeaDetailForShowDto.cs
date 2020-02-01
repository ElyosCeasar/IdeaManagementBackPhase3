using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject.Idea
{
    public class IdeaDetailForShowDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public byte StatusId { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string CurrentSituation { get; set; }
        public string Prerequisite { get; set; }
        public string Steps { get; set; }
        public string Advantages { get; set; }
        public string SAVE_DATE { get; set; }
        public int POINT { get; set; }

    }
}
