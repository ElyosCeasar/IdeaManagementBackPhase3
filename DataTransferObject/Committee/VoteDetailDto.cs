using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject.Committee
{
    public class VoteDetailDto
    {
        public int IdeaId { get; set; }
        public string CommitteeMemberUserName { get; set; }
        public Nullable<long> ProfitAmount { get; set; }
        public Nullable<long> SavingResourceAmount { get; set; }

        public int Vote { get; set; }
    }
}
