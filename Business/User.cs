using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObject.Common;
using DataTransferObject.User;

namespace Business
{
    public class User
    {
        private readonly DataAccess.Query.UserQ _Repository;
        //-------------------------------------------------------------------------------------------------
        public User()
        {
            _Repository = new DataAccess.Query.UserQ();
        }
        //-------------------------------------------------------------------------------------------------
        public IEnumerable<UserForShowDto> GetAllUsers()
        {
            return _Repository.GetAllUsers();
        }
        //-------------------------------------------------------------------------------------------------

        public UserForProfileDto GetUserProfile(string username)
        {
            return _Repository.GetUserProfile(username);
        }
        //-------------------------------------------------------------------------------------------------

        public Result PutUserProfile(ProfileForUpdateDto newProfile)
        {
            return _Repository.PutUserProfile(newProfile);
        }
        //-------------------------------------------------------------------------------------------------
        public Result ChangeCommitteFlag(string username, int value)
        {

            return _Repository.ChangeCommitteFlag(username,  value);
        }

        public bool IsAdmin(string username)
        {
            return _Repository.IsAdmin(username);
        }

        public bool IsCommitteMember(string username)
        {
            return _Repository.IsCommitteMember(username);
        }


        public IEnumerable<UserShowingTop10Dto> GetTop10IdeaMaker()
        {
            return _Repository.GetTop10IdeaMaker();
        }

        public IEnumerable<UserShowingTop10Dto> GetTop10CommentMaker()
        {
            return _Repository.GetTop10CommentMaker();
        }

        public IEnumerable<UserForShowDto> FilterSerchingUsers(FilterUserRequestDto searchItem)
        {
            return _Repository.FilterSerchingUsers(searchItem);
        }
    }
}
