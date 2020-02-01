using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Mapper;
using DataTransferObject.Committee;
using DataTransferObject.Common;

namespace DataAccess.Query
{
    public class CommitteeQ
    {
        private IdeaManagmentDatabaseEntities _db;
        //-------------------------------------------------------------------------------------------------

        public Result VoteToIdea(int ideaId, VoteDetailDto voteDetailDto,string username)
        {
            Result res = new Result();
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                if (!_db.IDEAS.Any(x => x.ID == ideaId))
                {
                    res.Content = "ایده ی مورد نظر پیدا نشد";
                    res.Value = false;
                    return res;
                }

                if (!_db.COMMITTEE_VOTE_DETAIL.Any(x => x.IDEAS_ID == ideaId))
                {
                    var newVote = new COMMITTEE_VOTE_DETAIL()
                    {
                        COMMITTEE_MEMBER=username,
                        IDEAS_ID=ideaId,
                         PROFIT_AMOUNT=voteDetailDto.ProfitAmount,
                         SAVING_RESOURCE_AMOUNT=voteDetailDto.SavingResourceAmount,
                        SAVE_DATE = DateTime.Now
                    };
                    _db.COMMITTEE_VOTE_DETAIL.Add(newVote);
                    _db.IDEAS.First(x => x.ID == ideaId).STATUS_ID =Convert.ToByte(voteDetailDto.Vote);
                    _db.SaveChanges();
                    res.Value = true;
                    res.Content = "رای کمیته اعمال شد";
                }
                else
                {//update it is impossible do to bussines
                    var vote = _db.COMMITTEE_VOTE_DETAIL.First(x => x.IDEAS_ID == ideaId);
                    vote.PROFIT_AMOUNT = voteDetailDto.ProfitAmount;
                    vote.SAVING_RESOURCE_AMOUNT = voteDetailDto.SavingResourceAmount;
                    vote.SAVE_DATE = DateTime.Now;
                    vote.COMMITTEE_MEMBER = username;
                    _db.IDEAS.First(x => x.ID == ideaId).STATUS_ID = Convert.ToByte(voteDetailDto.Vote);
                    _db.SaveChanges();
                    res.Value = true;
                    res.Content = "رای کمیته اعمال شد";
                }
            }
            return res;
       }
        //-------------------------------------------------------------------------------------------------

        public Result UnVoteIdea(int ideaId, string username)
        {
            
            Result res = new Result();
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                if (!_db.IDEAS.Any(x => x.ID == ideaId))
                {
                    res.Content = "ایده ی مورد نظر پیدا نشد";
                    res.Value = false;
                    return res;
                }

                if (!_db.COMMITTEE_VOTE_DETAIL.Any(x => x.IDEAS_ID == ideaId))
                {
                    res.Content = "رای مورد نظر پیدا نشد";
                    res.Value = false;
                    return res;
                }
                else
                {
                    _db.IDEAS.First(x => x.ID == ideaId).STATUS_ID = 0;
                    _db.COMMITTEE_VOTE_DETAIL.Remove(_db.COMMITTEE_VOTE_DETAIL.First(x => x.IDEAS_ID == ideaId));
                    _db.SaveChanges();
                    res.Content = "ایده‌ی مورد نظر به رای داده نشده‌ها اضافه شد";
                    res.Value = true;
                }
            }
            return res;
        }
        //-------------------------------------------------------------------------------------------------
    }
}
