using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject.Idea
{
    public  class FilterIdeaRequestDto
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public byte? StatusId { get; set; }
        public string Title { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public bool? OnlyshowMyIdea { get; set; }
    }
}
