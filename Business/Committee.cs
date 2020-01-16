using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObject.Committee;
using DataTransferObject.Common;

namespace Business
{
    public class Committee
    {
        private readonly DataAccess.Query.CommitteeQ _repository;
        private readonly DataAccess.Query.UserQ _repositoryUsers;
        //-------------------------------------------------------------------------------------------------
        public Committee()
        {
            _repository = new DataAccess.Query.CommitteeQ();

        }
        //-------------------------------------------------------------------------------------------------

        public Result VoteToIdea(int ideaId, VoteDetailDto voteDetailDto)
        {
            if (voteDetailDto.Vote > 1)
            {
                voteDetailDto.Vote = 2;
            }
            else
            {
                voteDetailDto.Vote = 1;
            }
            if(_repositoryUsers.IsCommitteMember(voteDetailDto.CommitteeMemberUserName))
                return _repository.VoteToIdea(ideaId, voteDetailDto);
            else
            {
                return new Result()
                {
                    Value = false,
                    Content = "متاسفانه شما به این امکان دسترسی ندارید"
                };
            }
        }
        //-------------------------------------------------------------------------------------------------

        public Result UnVoteIdea(int ideaId, string username)
        {
            if (_repositoryUsers.IsCommitteMember(username))
                return _repository.UnVoteIdea(ideaId, username);
            else
            {
                return new Result()
                {
                    Value = false,
                    Content = "متاسفانه شما به این امکان دسترسی ندارید"
                };
            }

        } 
        //-------------------------------------------------------------------------------------------------

    }
}
