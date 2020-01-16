using DataTransferObject.Comment;
using DataTransferObject.Common;
using DataTransferObject.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Comment
    {
        private readonly DataAccess.Query.CommentQ _Repository;
        private readonly DataAccess.Query.IdeaQ _RepositoryIdea;
        //-------------------------------------------------------------------------------------------------
        public Comment()
        {
            _Repository = new DataAccess.Query.CommentQ();
            _RepositoryIdea = new DataAccess.Query.IdeaQ();
        }
        //-------------------------------------------------------------------------------------------------
        public IEnumerable<IdeaCommentsDto> GetAllComments(int ideaId)
        {
            return _Repository.GetAllComments(ideaId);
        }

        //-------------------------------------------------------------------------------------------------
        public Result AddCommentToIdea(CommentDto newcomment)
        {
            if (_RepositoryIdea.IsIdeaLocked(newcomment.IdeaId))
            {
                return new Result()
                {
                    Value = false,
                    Content = "ایده توسط کمیته بررسی شده و یا وجود ندارد و موارد وابسته به آن قابل تغییر نیست"
                };
            }
            else
                return _Repository.AddCommentToIdea(newcomment);
        }

        //-------------------------------------------------------------------------------------------------
        public Result UpdateComment(CommentDto newcomment)
        {
            if (_RepositoryIdea.IsIdeaLocked(newcomment.IdeaId))
            {
                return new Result()
                {
                    Value = false,
                    Content = "ایده توسط کمیته بررسی شده و یا وجود ندارد و موارد وابسته به آن قابل تغییر نیست"
                };
            }
            else
                return _Repository.UpdateComment(newcomment);
        }
        //-------------------------------------------------------------------------------------------------
        public Result VoteToComment(VoteToCommentDto vote)
        {
            if (vote.Point < 0)
            {
                vote.Point = -1;
            }
            else if (vote.Point > 0)
            {
                vote.Point = 1;
            }
            else
            {
                vote.Point = 0;
            }

            if (_RepositoryIdea.IsIdeaLocked(_Repository.GetIdeaIdByCommentId(vote.CommentId)))
            {
                return new Result()
                {
                    Value = false,
                    Content = "ایده توسط کمیته بررسی شده و یا وجود ندارد و موارد وابسته به آن قابل تغییر نیست"
                };
            }
            else
                return _Repository.VoteToComment(vote);
        }
    }
}
