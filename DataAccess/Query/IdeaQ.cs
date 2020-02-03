using DataAccess.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTransferObject.Idea;
using System.Net.Http;
using DataTransferObject.Common;

namespace DataAccess.Query
{
    public class IdeaQ
    {
        private IdeaManagmentDatabaseEntities _db;
        //-------------------------------------------------------------------------------------------------
        public IEnumerable<IdeaForShowDto> GetAllIdea()
        {
            IEnumerable<IdeaForShowDto> res = null;
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                res = _db.IDEAS.OrderByDescending(x=>x.SAVE_DATE).ToList().Select(x => new IdeaForShowDto()
                {
                    Id =x.ID,
                    Username =x.USERNAME,
                    FullName =x.USER.FIRST_NAME+" "+ x.USER.LAST_NAME,
                    SaveDate = Persia.Calendar.ConvertToPersian(x.SAVE_DATE).Simple,
                    Status =x.IDEA_STATUS.TITLE,
                    StatusId =x.STATUS_ID,
                    Title =x.TITLE,
                    TotalPoints = x.IDEA_POINTS.Sum(w => w.POINT)
                }).ToList();

  
            }
            return res;
        }
        //----------------------------------------------------------------------------------------------------------
        public IdeaDetailForShowDto GetSpecificIdea(int ideaId)
        {
            IdeaDetailForShowDto res = null; 
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var idea = _db.IDEAS.FirstOrDefault(x => x.ID == ideaId);
                if (idea != null)
                    res = new IdeaDetailForShowDto()
                    {
                        Id = idea.ID,
                        POINT = idea.IDEA_POINTS.Sum(x => x.POINT),
                        SAVE_DATE = Persia.Calendar.ConvertToPersian(idea.SAVE_DATE).Simple,
                        Status = idea.IDEA_STATUS.TITLE,
                        CurrentSituation = idea.CURRENT_SITUATION,
                        Prerequisite = idea.PREREQUISITE,
                        Advantages = idea.ADVANTAGES,
                        Steps = idea.STEPS,
                        Title=idea.TITLE,
                        Username = idea.USERNAME,
                        FullName = idea.USER.FIRST_NAME + " " + idea.USER.LAST_NAME,
                        StatusId = idea.STATUS_ID

                    };
                 
                
            }
            return res;
        }
        //-----------------------------------------------------------------------------------------------------------
        public Result SendNewIdea(NewIdeaDto newIdea)
        {
            Result result = new Result();
            using (_db = new IdeaManagmentDatabaseEntities())
            {


                IDEA idea = new IDEA()
                {
                    USERNAME = newIdea.Username,
                    TITLE = newIdea.Title,
                    SAVE_DATE = DateTime.Now,
                    ADVANTAGES=newIdea.Advantages,
                    PREREQUISITE= newIdea.Prerequisite,
                    STEPS= newIdea.Steps,
                    CURRENT_SITUATION= newIdea.CurrentSituation
                };
                _db.IDEAS.Add(idea);
                _db.SaveChanges();
                result.Value = true;
                result.Content = "ایده جدید ایجاد شد";
                
            }
            return result;
        }     
        //----------------------------------------------------------------------------------------------------------

        public List<IdeaStatusDto> GetAllIdeaStatus()
        {
            using (_db = new IdeaManagmentDatabaseEntities())
            {

                return _db.IDEA_STATUS.Select(s=>new IdeaStatusDto() { Id=s.ID,
                Title=s.TITLE}).ToList();
            }
        }
        //----------------------------------------------------------------------------------------------------------

        public IEnumerable<IdeaForShowDto> FilterSerchingIdea(FilterIdeaRequestDto searchItem)
        {
            IEnumerable<IdeaForShowDto> res=null;
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                IQueryable<IDEA> ideas = _db.IDEAS;
                if(searchItem.OnlyshowMyIdea.HasValue && searchItem.OnlyshowMyIdea.Value == true)
                {
                    ideas = ideas.Where(x => x.USERNAME == searchItem.MyUsername);
                }
                else
                {
                    if (searchItem.Username != null  && searchItem.Username.Trim().Length > 0)
                    {
                        ideas = ideas.Where(x => x.TITLE.Contains(searchItem.Username.Trim()));
                    }
                    if (searchItem.FullName != null  && searchItem.FullName.Trim().Length > 0)
                    {
                        if (searchItem.FullName.Trim().Contains(" "))
                        {
                            var firstName = searchItem.FullName.Trim().Substring(0, searchItem.FullName.Trim().IndexOf(" "));
                            var lastName = searchItem.FullName.Trim().Substring(0, searchItem.FullName.Trim().IndexOf(" "));
                            ideas = ideas.Where(u => u.USER.FIRST_NAME.Contains(firstName.Trim()) && u.USER.LAST_NAME.Contains(lastName.Trim()));

                        }
                        else
                        {
                            ideas = ideas.Where(u => u.USER.FIRST_NAME.Contains(searchItem.Username.Trim()) || u.USER.LAST_NAME.Contains(searchItem.Username.Trim()));
                        }

                    }
                }
                if (searchItem.Title != null && searchItem.Title.Trim().Length > 0)
                {
                    ideas = ideas.Where(x => x.TITLE.Contains(searchItem.Title.Trim()));
                }
                if (searchItem.StatusId.HasValue )//check shavad
                {
                    ideas = ideas.Where(x => x.STATUS_ID==searchItem.StatusId.Value);
                }
                ideas=_filterYearAndMonth(ideas,searchItem.Year,searchItem.Month);

                res = ideas.OrderByDescending(x => x.SAVE_DATE).Select(x => new IdeaForShowDto() {
                    Id = x.ID,
                    FullName = x.USER.FIRST_NAME + " " + x.USER.LAST_NAME,
                    Title = x.TITLE,
                    Status = x.IDEA_STATUS.TITLE,
                    StatusId = x.STATUS_ID,
                    Username = x.USERNAME,
                    TotalPoints = x.IDEA_POINTS.Any()? x.IDEA_POINTS.Sum(w => w.POINT):0

                }).ToList();

                foreach(var row in res)
                {
                    row.SaveDate = Persia.Calendar.ConvertToPersian(_db.IDEAS.First(z => z.ID == row.Id).SAVE_DATE).Simple;
                }
            }
            return res;
        }
        //-------------------------------------------------------------------------------------------------

        public Result DeleteIdea(int ideaId)
        {
            Result res = new Result();
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var idea = _db.IDEAS.FirstOrDefault(x => x.ID == ideaId);
                    if(idea!=null){
                    if (idea.STATUS_ID > 0)
                    {
                        res.Value = false;
                        res.Content = "برای ایده بررسی شده امکان حذف وجود ندارد";
                        return res;
                    }
                    var allpoints = _db.IDEA_POINTS.Where(x => x.IDEA_ID == ideaId);
                    var allcomments = _db.IDEA_COMMENTS.Where(x => x.IDEA_ID == ideaId);
                    if (allcomments != null && allcomments.Count() > 0)
                    {
                        var allcommentsIdes = allcomments.Select(x => x.ID);
                        var allComentPoints = _db.COMMENT_POINTS.Where(x => allcommentsIdes.Contains(x.COMMENT_ID));
                        if (allComentPoints != null && allComentPoints.Count() > 0)
                        {
                            _db.COMMENT_POINTS.RemoveRange(allComentPoints);
                        }
                        _db.IDEA_COMMENTS.RemoveRange(allcomments);
                    }
                    if (allpoints != null && allpoints.Count() > 0)
                    {
                        _db.IDEA_POINTS.RemoveRange(allpoints);
                    }
                    _db.IDEAS.Remove(idea);
                    _db.SaveChanges();
                    res.Value = true;
                    res.Content = "ایده‌ی مورد نظر حذف شد";
                }
                else
                {
                    res.Value =false;
                    res.Content ="ایده‌ی مورد نظر یافت نشد";
                }
               
            }
            return res;
      }

        //-------------------------------------------------------------------------------------------------

        public IEnumerable<int> GetYearsFromOldestIdea() 
        {
            DateTime oldestTime = DateTime.Now;//first intial
            int nowShamsiYear = Persia.Calendar.ConvertToPersian(oldestTime).ArrayType[0];
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var tempOldest = _db.IDEAS.Min(x=>x.SAVE_DATE);
                if (tempOldest != null)
                {
                    oldestTime = tempOldest;
                }
            }
            int smallestShamsiYear = Persia.Calendar.ConvertToPersian(oldestTime).ArrayType[0];
            List<int> years = new List<int>();
            for(int i = nowShamsiYear; i >= smallestShamsiYear; i--)
            {
                years.Add(i);
            }
            return years;
        }

        //----------------------------------------------------------------------------------------------------------

        public IEnumerable<IdeaForShowDto> GetAllNotDecidedIdea()
        {
            IEnumerable<IdeaForShowDto> res = null;
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                res = _db.IDEAS.Where(x=>x.STATUS_ID==0).OrderByDescending(x => x.SAVE_DATE).Select(x => new IdeaForShowDto()
                {
                    Id = x.ID,
                    Username = x.USERNAME,
                    FullName = x.USER.FIRST_NAME + " " + x.USER.LAST_NAME,

                    Status = x.IDEA_STATUS.TITLE,
                    StatusId = x.STATUS_ID,
                    Title = x.TITLE,
                    TotalPoints = x.IDEA_POINTS.Any()? x.IDEA_POINTS.Sum(w => w.POINT):0
                }).ToList();
                foreach(var row in res)
                {
                    row.SaveDate = Persia.Calendar.ConvertToPersian(_db.IDEAS.First(x => x.ID == row.Id).SAVE_DATE).Simple;
                }

            }
            return res;
        }
        //----------------------------------------------------------------------------------------------------------

        public IEnumerable<IdeaForShowDto> GetAllCurrentMontDecidedIdea()
        {
            IEnumerable<IdeaForShowDto> res = null;
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                IQueryable<IDEA> temp = _db.IDEAS.Where(x=>x.STATUS_ID>0);
                var datePersion = Persia.Calendar.ConvertToPersian(DateTime.Now);
                temp=_filterYearAndMonthForCommitteVote(temp, datePersion.ArrayType[0], datePersion.ArrayType[1]);
                res = temp.OrderByDescending(x => x.COMMITTEE_VOTE_DETAIL.SAVE_DATE).Select(x => new IdeaForShowDto()
                {
                    Id = x.ID,
                    Username = x.USERNAME,
                    FullName = x.USER.FIRST_NAME + " " + x.USER.LAST_NAME,
                    Status = x.IDEA_STATUS.TITLE,
                    StatusId = x.STATUS_ID,
                    Title = x.TITLE,
                    TotalPoints = x.IDEA_POINTS.Any()? x.IDEA_POINTS.Sum(w => w.POINT):0
                }).ToList(); 

                foreach (var row in res)
                {
                    row.SaveDate = Persia.Calendar.ConvertToPersian(_db.IDEAS.First(x => x.ID == row.Id).SAVE_DATE).Simple;
                }
            }
            return res;
        }
        //----------------------------------------------------------------------------------------------------------

        public IEnumerable<IdeaForShowDto> FilterAllNotDecidedIdea(FilterAllNotDecidedIdeaRequestDto searchItem)
        {
            IEnumerable<IdeaForShowDto> res = null;
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                IQueryable<IDEA> temp = _db.IDEAS;
                temp=_filterYearAndMonth(temp, searchItem.Year, searchItem.Month);
                res = temp.OrderByDescending(x => x.SAVE_DATE).Select(x => new IdeaForShowDto()
                {
                    Id = x.ID,
                    Username = x.USERNAME,
                    FullName = x.USER.FIRST_NAME + " " + x.USER.LAST_NAME,
             
                    Status = x.IDEA_STATUS.TITLE,
                    StatusId = x.STATUS_ID,
                    Title = x.TITLE,
                    TotalPoints = x.IDEA_POINTS.Any()? x.IDEA_POINTS.Sum(w => w.POINT):0
                }).ToList();
                foreach (var row in res)
                {
                    row.SaveDate = Persia.Calendar.ConvertToPersian(_db.IDEAS.First(x => x.ID == row.Id).SAVE_DATE).Simple;
                }

            }
            return res;
        }

        //----------------------------------------------------------------------------------------------------------

        public IEnumerable<WinnerIdeaForShowDto> FilterWinnerIdea(FilterWinnerIdeaRequestDto searchItem)
        {
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                IQueryable<SELECTED_IDEA> selectedIdeas = _db.SELECTED_IDEA;
                if (searchItem.Year.HasValue)
                {
                    selectedIdeas = selectedIdeas.Where(x => x.YEAR == searchItem.Year.Value);
                }
                if (searchItem.Month.HasValue)
                {
                    selectedIdeas = selectedIdeas.Where(x => x.MONTH == searchItem.Month.Value);
                }

                return selectedIdeas.OrderByDescending(x => x.YEAR * (x.MONTH > 9 ? 100 : 1000) + x.MONTH).ToList().Select(s => new WinnerIdeaForShowDto()
                {
                    TITLE = s.IDEA.TITLE,
                    FullName = s.IDEA.USER.FIRST_NAME + " " + s.IDEA.USER.LAST_NAME,
                    Username = s.IDEA.USERNAME,
                    AcceptDate = s.YEAR + "/" + s.MONTH + "/" + 1,
                    SaveDate = Persia.Calendar.ConvertToPersian(s.IDEA.SAVE_DATE).Simple,
                    TotalPoints = s.IDEA.IDEA_POINTS.Sum(x => x.POINT)
                });
            }
        }

        //----------------------------------------------------------------------------------------------------------

        public IEnumerable<WinnerIdeaForShowDto> GetAllWinnerIdea()
        {
            using (_db = new IdeaManagmentDatabaseEntities())
            {

                return _db.SELECTED_IDEA.OrderByDescending(x=>x.YEAR*(x.MONTH>9? 100:1000)+x.MONTH).ToList().Select(s => new WinnerIdeaForShowDto()
                {   
                    TITLE=s.IDEA.TITLE,
                    FullName=s.IDEA.USER.FIRST_NAME+" "+s.IDEA.USER.LAST_NAME,
                    Username=s.IDEA.USERNAME,
                    AcceptDate =s.YEAR+"/"+s.MONTH+"/"+1,
                    SaveDate= Persia.Calendar.ConvertToPersian(s.IDEA.SAVE_DATE).Simple,
                    TotalPoints = s.IDEA.IDEA_POINTS.Sum(x => x.POINT)
                });
            }
        }

        //----------------------------------------------------------------------------------------------------------

        public Result EditIdea(int ideaId, ChangedIdeaDto ideaForChange)
        {
            Result result = new Result();

            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var idea = _db.IDEAS.FirstOrDefault(x => x.ID == ideaId); 
                if (idea == null)
                {
                    result.Value = false;
                    result.Content = "ایده پیدا نشد";
                    return result;
                }

                idea.USERNAME = ideaForChange.Username;
                idea.TITLE = ideaForChange.Title;
                idea.PREREQUISITE = ideaForChange.Prerequisite;
                idea.STEPS = ideaForChange.Steps;
                idea.CURRENT_SITUATION = ideaForChange.CurrentSituation;
                idea.ADVANTAGES = ideaForChange.Advantages;
                idea.MODIFY_DATE = DateTime.Now;
                _db.SaveChanges();
                result.Value = true;
                result.Content = "ایده اصلاح شد";
                return result;
            }
        }

        //----------------------------------------------------------------------------------------------------------
        public Result VoteToIdea(IdeaPointDto voteDetail)
        {
            Result result = new Result();

            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var idea_point = _db.IDEA_POINTS.FirstOrDefault(x => x.IDEA_ID == voteDetail.IdeaId && x.USERNAME == voteDetail.Username);
                if (idea_point == null)
                {
                    var idea = _db.IDEAS.FirstOrDefault(x => x.ID == voteDetail.IdeaId);
                    if (idea == null)
                    {
                        result.Value = false;
                        result.Content = "ایده پیدا نشد";
                        return result;
                    }
                    var user = _db.USERS.FirstOrDefault(x => x.USERNAME == voteDetail.Username);
                    if (user == null)
                    {
                        result.Value = false;
                        result.Content = "کاربر پیدا نشد";
                        return result;
                    };
                    if (idea.USERNAME == voteDetail.Username)
                    {
                        result.Value = false;
                        result.Content = "به ایده ی خود نمی شود رای داد";
                        return result;
                    }

                    IDEA_POINTS idea_point_ = new IDEA_POINTS()
                    {
                        IDEA_ID = voteDetail.IdeaId,
                        USERNAME = voteDetail.Username,
                        SAVE_DATE = DateTime.Now,
                        POINT = voteDetail.Point
                    };

                    _db.IDEA_POINTS.Add(idea_point_);
                    _db.SaveChanges();
                    result.Value = true;
                    result.Content = "به ایده امتیاز داده شد";
                    return result;
                }

                idea_point.POINT = voteDetail.Point;
                idea_point.MODIFY_DATE = DateTime.Now;
                _db.SaveChanges();
                result.Value = true;
                result.Content = "امتیاز ایده اصلاح شد";
                return result;
            }
        }

        //-------------------------------------------------------------------------------------------------
        public IEnumerable<IdeaDto> GetIdeasTop10All()
        {
            List<IdeaDto> res = new List<IdeaDto>();
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var ideaPoint = _db.IDEA_POINTS.GroupBy(x => x.IDEA_ID).Select(y => new
                {
                    IDEA_ID = y.Key,
                    TOTAL_POINT = y.Sum(x => x.POINT)
                }).OrderByDescending(x => x.TOTAL_POINT).Take(10).ToList();

                foreach(var i in ideaPoint)
                {
                    var idea = _db.IDEAS.Single(z => z.ID == i.IDEA_ID);
            
                    res.Add(new IdeaDto()
                    {
                        Id=idea.ID,
                        Title = idea.TITLE,
                        TotalPoints = i.TOTAL_POINT,
                        FullName = idea.USER.FIRST_NAME+" "+idea.USER.LAST_NAME,
                        Username = idea.USERNAME,
                        Status = idea.IDEA_STATUS.TITLE,
                        SaveDate = Persia.Calendar.ConvertToPersian(idea.SAVE_DATE).Simple
                    });
                }
                

            }
            return res;
        }
        //-------------------------------------------------------------------------------------------------
        public IEnumerable<IdeaDto> GetIdeasTop10CurrentMonth()
        {
            List<IdeaDto> res = new List<IdeaDto>();
             //--------------------
             var now = DateTime.Now;
            var convertedToShamsi = Persia.Calendar.ConvertToPersian(now);
            int currentDayOfMonth = convertedToShamsi.ArrayType[2];
            var startDateDateMonthInMiladi = DateTime.Now.AddDays(-currentDayOfMonth + 1);

            int numberOfDaysToEndMonth = 0;
            //چک کردن ماه 31 روزه 30 روزه و29 روزه
            if (convertedToShamsi.ArrayType[1] < 7)
                numberOfDaysToEndMonth = 31 - currentDayOfMonth;/// تعداد روزی که باید بریم جلو تو ماه میلادی
            else if (convertedToShamsi.ArrayType[1] < 12)
                numberOfDaysToEndMonth = 30 - currentDayOfMonth; 
            else
                numberOfDaysToEndMonth = 29 - currentDayOfMonth; 
            var finishDateMonthInMiladi = DateTime.Now.AddDays(numberOfDaysToEndMonth);

            startDateDateMonthInMiladi = new DateTime(startDateDateMonthInMiladi.Year, startDateDateMonthInMiladi.Month, startDateDateMonthInMiladi.Day, 23, 59, 59);
            finishDateMonthInMiladi = new DateTime(finishDateMonthInMiladi.Year, finishDateMonthInMiladi.Month, finishDateMonthInMiladi.Day, 23, 59, 59);
            //--------------------
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var bestIdeas = _db.IDEA_POINTS.
                    Where(p => p.IDEA.SAVE_DATE >= startDateDateMonthInMiladi && p.IDEA.SAVE_DATE <= finishDateMonthInMiladi)
                    .GroupBy(x => x.IDEA_ID)
                    .Select(y => new
                    {
                        IDEA_ID = y.Key,
                        TOTAL_POINT = y.Sum(x => x.POINT)
                    }).OrderByDescending(x => x.TOTAL_POINT)
                .Take(10);

                foreach(var x in bestIdeas)
                {
                    var idea = _db.IDEAS.Single(i => i.ID == x.IDEA_ID);
                    res.Add(new IdeaDto()
                    {
                        Id=idea.ID,
                        Title = idea.TITLE,
                        TotalPoints = x.TOTAL_POINT,
                        FullName = idea.USER.FIRST_NAME+" "+idea.USER.LAST_NAME,
                        Username = idea.USERNAME,
                        Status = idea.IDEA_STATUS.TITLE,
                        SaveDate = Persia.Calendar.ConvertToPersian(idea.SAVE_DATE).Simple
                    });
                }

            }
            return res;
        }
        //-------------------------------------------------------------------------------------------------
        public IEnumerable<IdeaDto> GetIdeasTop10CurrentWeek()
        {
            List<IdeaDto> res = new List<IdeaDto>();
            //--------------------

            var currentDate = DateTime.Now;
            var converted = Persia.Calendar.ConvertToPersian(currentDate);
            int dayOfWeek = converted.DayOfWeek+1;
            var startDayOfWeekInMillady = DateTime.Now.AddDays(-dayOfWeek + 1);//اول
            startDayOfWeekInMillady = new DateTime(startDayOfWeekInMillady.Year, startDayOfWeekInMillady.Month, startDayOfWeekInMillady.Day, 0, 0, 0);
            var endDayOfWeekInMillady = DateTime.Now.AddDays(7 - dayOfWeek);//آخرش
            endDayOfWeekInMillady = new DateTime(endDayOfWeekInMillady.Year, endDayOfWeekInMillady.Month, endDayOfWeekInMillady.Day, 23, 59, 59);
            //--------------------
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var bestIdeas = _db.IDEA_POINTS.Where(p => p.IDEA.SAVE_DATE >= startDayOfWeekInMillady && p.IDEA.SAVE_DATE <= endDayOfWeekInMillady)
                    .GroupBy(x => x.IDEA_ID)
                    .Select(y => new
                {
                    IDEA_ID = y.Key,
                    TOTAL_POINT = y.Sum(x => x.POINT)
                }).OrderByDescending(x => x.TOTAL_POINT).Take(10);

                foreach (var x in bestIdeas)
                {
                    var idea = _db.IDEAS.Single(i => i.ID == x.IDEA_ID);
                    res.Add(new IdeaDto()
                    {
                        Id=idea.ID,
                        Title = idea.TITLE,
                        TotalPoints = x.TOTAL_POINT,
                        FullName = idea.USER.FIRST_NAME + " " + idea.USER.LAST_NAME,
                        Username = idea.USERNAME,
                        Status = idea.IDEA_STATUS.TITLE,
                        SaveDate = Persia.Calendar.ConvertToPersian(idea.SAVE_DATE).Simple
                    });
                }

            }
            return res;
        }
        //-------------------------------------------------------------------------------------------------


        public bool IsIdeaLocked(int id)
        {
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var idea = _db.IDEAS.FirstOrDefault(x => x.ID == id);
                if (idea == null)
                {
                    return true;//this true means not found
                }
                else
                {
                    return idea.STATUS_ID == 0 ? false : true;
                }
            }
        }
        //----------------------------------------------------------------------------------------------------------

        private IQueryable<IDEA> _filterYearAndMonth(IQueryable<IDEA> ideas,int? yearP,int? monthP)
        {
            if (yearP.HasValue == false && monthP.HasValue == false)
            {
                return ideas;//no change requires
            }
            if (yearP.HasValue)
            {

               var startDay =new DateTime(
                    Persia.Calendar.ConvertToGregorian(yearP.Value, 1, 1, Persia.DateType.Persian).Year,
                    Persia.Calendar.ConvertToGregorian(yearP.Value, 1, 1, Persia.DateType.Persian).Month,
                    Persia.Calendar.ConvertToGregorian(yearP.Value, 1, 1, Persia.DateType.Persian).Day,
                    0,
                    0,
                    0);
               var endDay = new DateTime(
                    Persia.Calendar.ConvertToGregorian(yearP.Value, 12, 29, Persia.DateType. Persian).Year,
                     Persia.Calendar.ConvertToGregorian(yearP.Value, 12, 29, Persia.DateType.Persian).Month,
                     Persia.Calendar.ConvertToGregorian(yearP.Value, 12, 29, Persia.DateType.Persian).Day,
                    23,
                    59,
                    59); 
                ideas = ideas.Where(x => x.SAVE_DATE >= startDay && x.SAVE_DATE <= endDay);
            }

            if (monthP.HasValue)
            {
                var yearPersionNow = Persia.Calendar.ConvertToPersian(DateTime.Now).ArrayType[0];
                IQueryable<IDEA> ideasRes = ideas.Where(x => 1 == 2);//need null IquryAble
                for (int i=1396;i<=yearPersionNow+1;i++)//1396 start software from
                {
                    IQueryable<IDEA> ideasTemp;
     
                        var startDay = new DateTime(
                  Persia.Calendar.ConvertToGregorian(i, monthP.Value, 1, Persia.DateType.Persian).Year,
                   Persia.Calendar.ConvertToGregorian(i, monthP.Value, 1, Persia.DateType.Persian).Month,
                   Persia.Calendar.ConvertToGregorian(i, monthP.Value, 1, Persia.DateType.Persian).Day,
                   0,
                   0,
                   0);
                int dayCountsInMonth;
                if (monthP.Value <= 6)
                {
                    dayCountsInMonth = 31;
                }
                else
                {
                    dayCountsInMonth = monthP.Value != 12 ? 30 : 29;
                }

                var endDay = new DateTime(
                     Persia.Calendar.ConvertToGregorian(i, monthP.Value, dayCountsInMonth, Persia.DateType.Persian).Year,
                     Persia.Calendar.ConvertToGregorian(i, monthP.Value, dayCountsInMonth, Persia.DateType.Persian).Month,
                     Persia.Calendar.ConvertToGregorian(i, monthP.Value, dayCountsInMonth, Persia.DateType.Persian).Day,
                     23,
                     59,
                     59);

          
                    ideasTemp = ideas.Where(x =>
                      x.SAVE_DATE >= startDay && x.SAVE_DATE <= endDay
                    );

                    ideasRes= ideasRes.Concat(ideasTemp);
            }
                ideas = ideasRes;
            }
            return ideas;
        }

        //----------------------------------------------------------------------------------------------------------

        private IQueryable<IDEA> _filterYearAndMonthForCommitteVote(IQueryable<IDEA> ideas, int? yearP, int? monthP)
        {
            if (yearP.HasValue == false && monthP.HasValue == false)
            {
                return ideas;//no change requires
            }
            if (yearP.HasValue)
            {

                var startDay = new DateTime(
                     Persia.Calendar.ConvertToGregorian(yearP.Value, 1, 1, Persia.DateType.Persian).Year,
                     Persia.Calendar.ConvertToGregorian(yearP.Value, 1, 1, Persia.DateType.Persian).Month,
                     Persia.Calendar.ConvertToGregorian(yearP.Value, 1, 1, Persia.DateType.Persian).Day,
                     0,
                     0,
                     0);
                var endDay = new DateTime(
                     Persia.Calendar.ConvertToGregorian(yearP.Value, 12, 29, Persia.DateType.Persian).Year,
                      Persia.Calendar.ConvertToGregorian(yearP.Value, 12, 29, Persia.DateType.Persian).Month,
                      Persia.Calendar.ConvertToGregorian(yearP.Value, 12, 29, Persia.DateType.Persian).Day,
                     23,
                     59,
                     59);
                ideas = ideas.Where(x => x.COMMITTEE_VOTE_DETAIL.SAVE_DATE >= startDay && x.COMMITTEE_VOTE_DETAIL.SAVE_DATE <= endDay);
            }

            if (monthP.HasValue)
            {
                var yearPersionNow = Persia.Calendar.ConvertToPersian(DateTime.Now).ArrayType[0];
                IQueryable<IDEA> ideasRes = ideas.Where(x => 1 == 2);//need null IquryAble
                for (int i = 1396; i <= yearPersionNow + 1; i++)//1396 start software from
                {
                    IQueryable<IDEA> ideasTemp;

                    var startDay = new DateTime(
              Persia.Calendar.ConvertToGregorian(i, monthP.Value, 1, Persia.DateType.Persian).Year,
               Persia.Calendar.ConvertToGregorian(i, monthP.Value, 1, Persia.DateType.Persian).Month,
               Persia.Calendar.ConvertToGregorian(i, monthP.Value, 1, Persia.DateType.Persian).Day,
               0,
               0,
               0);
                    int dayCountsInMonth;
                    if (monthP.Value <= 6)
                    {
                        dayCountsInMonth = 31;
                    }
                    else
                    {
                        dayCountsInMonth = monthP.Value != 12 ? 30 : 29;
                    }

                    var endDay = new DateTime(
                         Persia.Calendar.ConvertToGregorian(i, monthP.Value, dayCountsInMonth, Persia.DateType.Persian).Year,
                         Persia.Calendar.ConvertToGregorian(i, monthP.Value, dayCountsInMonth, Persia.DateType.Persian).Month,
                         Persia.Calendar.ConvertToGregorian(i, monthP.Value, dayCountsInMonth, Persia.DateType.Persian).Day,
                         23,
                         59,
                         59);


                    ideasTemp = ideas.Where(x =>
                      x.COMMITTEE_VOTE_DETAIL.SAVE_DATE >= startDay && x.COMMITTEE_VOTE_DETAIL.SAVE_DATE <= endDay
                    );

                    ideasRes = ideasRes.Concat(ideasTemp);
                }
                ideas = ideasRes;
            }
            return ideas;
        }

        //----------------------------------------------------------------------------------------------------------


    }
}

