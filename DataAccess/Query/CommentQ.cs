using DataAccess.Mapper;
using DataTransferObject.Comment;
using DataTransferObject.Common;
using DataTransferObject.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Query
{
    public class CommentQ
    {
        private IdeaManagmentDatabaseEntities _db;
        //-------------------------------------------------------------------------------------------------
        public IEnumerable<IdeaCommentsDto> GetAllComments(int ideaId)
        {
            IEnumerable<IdeaCommentsDto> res=null;
            using (_db = new IdeaManagmentDatabaseEntities())
            {


                res = _db.IDEA_COMMENTS.Where(c => c.IDEA_ID == ideaId).Select(x => new IdeaCommentsDto()
                {
                    Id = x.ID,
                    IdeaId =x.IDEA_ID,
                    Comment =x.COMMENT,
                    Points = _db.COMMENT_POINTS.Where(p => p.COMMENT_ID == x.ID).Any()? _db.COMMENT_POINTS.Where(p => p.COMMENT_ID == x.ID).Sum(z=>z.POINT):0,
                   
                    Username =x.USERNAME, 
                    FullName =x.USER.FIRST_NAME+" "+x.USER.LAST_NAME

                }).OrderByDescending(c=>c.Points).ToList();
                foreach(var row in res) {
                    row.SaveDate = Persia.Calendar.ConvertToPersian(_db.IDEA_COMMENTS.First(x => x.ID == row.Id).SAVE_DATE).Simple;
                }
            }
            return res;
        }

        //-----------------------------------------------------------------------------------------------------------
        public Result AddCommentToIdea(CommentDto newcomment)
        {
            Result result = new Result();

            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var idea = _db.IDEAS.FirstOrDefault(x => x.ID == newcomment.IdeaId);
                if (idea == null)
                {
                    result.Value = false;
                    result.Content = "ایده پیدا نشد";
                    return result;
                }

                var ideaCommentForCheck = _db.IDEA_COMMENTS.FirstOrDefault(x => x.IDEA_ID == newcomment.IdeaId && x.USERNAME == newcomment.Username);
                if (ideaCommentForCheck != null)
                {
                    result.Value = false;
                    result.Content = "قبلا شما پیشنهادی برای این ایده نوشته اید";
                    return result;
                }

                if (idea.USERNAME == newcomment.Username)
                {
                    result.Value = false;
                    result.Content = "برای ایده خود نمی شود پیشنهاد گذاشت";
                    return result;
                }
   

                IDEA_COMMENTS ideaComment = new IDEA_COMMENTS()
                {
                    COMMENT = newcomment.Comment,
                    IDEA_ID = newcomment.IdeaId,
                    SAVE_DATE = DateTime.Now,
                    USERNAME = newcomment.Username
                };
                _db.IDEA_COMMENTS.Add(ideaComment);
                _db.SaveChanges();
                result.Value = true;
                result.Content = "پیشنهاد جدید اضافه شد";
                return result;
            }
        }
        //-----------------------------------------------------------------------------------------------------------

        public IdeaCommentsDto GetSpeceficComment(int commentId)
        {
            IdeaCommentsDto res = null;
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var temp = _db.IDEA_COMMENTS.FirstOrDefault(c => c.ID == commentId);
                if (temp != null)
                {
                    res = new IdeaCommentsDto();
                    res.Id = temp.ID;
                    res.IdeaId = temp.IDEA_ID;
                    res.Comment = temp.COMMENT;
                    //don't want points
                    //res.Points = _db.COMMENT_POINTS.Where(p => p.COMMENT_ID == temp.ID).Any() ? _db.COMMENT_POINTS.Where(p => p.COMMENT_ID == x.ID).Sum(z => z.POINT) : 0,
                    res.SaveDate = Persia.Calendar.ConvertToPersian(temp.SAVE_DATE).Simple;
                    res.Username = temp.USERNAME;
                    res.FullName = temp.USER.FIRST_NAME + " " + temp.USER.LAST_NAME;
                } 
            }
            return res;
        }

        //-----------------------------------------------------------------------------------------------------------
        public Result UpdateComment(CommentDto newcomment)
        {
            Result result = new Result();

            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var idea_ = _db.IDEAS.FirstOrDefault(x => x.ID == newcomment.IdeaId);
                if (idea_ == null)
                {
                    result.Value = false;
                    result.Content = "ایده پیدا نشد";
                    return result;
                }

                var idea_comment = _db.IDEA_COMMENTS.FirstOrDefault(x => x.IDEA_ID == newcomment.IdeaId && x.USERNAME == newcomment.Username);
                if (idea_comment == null)
                {
                    result.Value = false;
                    result.Content = "قبلا شما پیشنهادی برای این ایده ننوشته اید";
                    return result;
                }

                idea_comment.COMMENT = newcomment.Comment;
                idea_comment.MODIFY_DATE = DateTime.Now;
                _db.SaveChanges();
                result.Value = true;
                result.Content = "پیشنهاد اصلاح شد";
                return result;
            }
        }
        //----------------------------------------------------------------------------------------------------------
        public Result VoteToComment(VoteToCommentDto voteDetail)
        {
            Result result = new Result();

            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var comment_point = _db.COMMENT_POINTS.FirstOrDefault(x => x.COMMENT_ID == voteDetail.CommentId && x.USERNAME == voteDetail.UsernameVoter);
                if (comment_point == null)
                {
                    var comment = _db.IDEA_COMMENTS.FirstOrDefault(x => x.ID == voteDetail.CommentId);
                    if (comment == null)
                    {
                        result.Value = false;
                        result.Content = "پیشنهادی  با این مشخصات وجود ندارد";
                        return result;
                    }
                    var user = _db.USERS.FirstOrDefault(x => x.USERNAME == voteDetail.UsernameVoter);
                    if (user == null)
                    {
                        result.Value = false;
                        result.Content = "کاربر پیدا نشد";
                        return result;
                    };
                    if (comment.USERNAME == voteDetail.UsernameVoter)
                    {
                        result.Value = false;
                        result.Content = "به پیشنهاد خود نمی توان رای داد";
                        return result;
                    }

                    COMMENT_POINTS comment_point2 = new COMMENT_POINTS()
                    {
                        COMMENT_ID = voteDetail.CommentId,
                        SAVE_DATE = DateTime.Now,
                        POINT = voteDetail.Point,
                        USERNAME= voteDetail.UsernameVoter
                    };

                    _db.COMMENT_POINTS.Add(comment_point2);
                    _db.SaveChanges();
                    result.Value = true;
                    result.Content = "به پیشنهاد امتیاز داده شد";
                    return result;
                }

                comment_point.POINT = voteDetail.Point;
                comment_point.MODIFY_DATE = DateTime.Now;
                _db.SaveChanges();
                result.Value = true;
                result.Content = "امتیاز پیشنهاداصلاح شد";
                return result;
            }
        }

        public int GetIdeaIdByCommentId(int commentId)
        {
            int res = -1;
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var comment = _db.IDEA_COMMENTS.FirstOrDefault(c=>c.ID==commentId);
                if (comment != null)
                    res = comment.IDEA_ID;
                
            }
            return res;
        }


    }
}
