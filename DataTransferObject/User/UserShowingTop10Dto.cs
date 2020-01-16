using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject.User
{
    public class UserShowingTop10Dto
    {
        public string FullName { set; get; }
        public string UserName { set; get; }
        public int Count { set; get; }
        public int PointsCount { set; get; }
    }
}
