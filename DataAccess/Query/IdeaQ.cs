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
                    SaveDate = Persia.Calendar.ConvertToPersian(x.SAVE_DATE).Persian,
                    Status =x.IDEA_STATUS.TITLE,
                    StatusId =x.STATUS_ID,
                    Title =x.TITLE,
                    TotalPoints = x.IDEA_POINTS.Sum(w => w.POINT)
                });

  
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
                        SAVE_DATE = idea.SAVE_DATE,
                        Status = idea.IDEA_STATUS.TITLE,
                        CurrentSituation = idea.CURRENT_SITUATION,
                        Prerequisite = idea.PREREQUISITE,
                        Advantages = idea.ADVANTAGES,
                        Steps = idea.STEPS,
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
                return result;
            }
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
                    if (searchItem.Username != null && searchItem.Username.Trim().Length > 0)
                    {
                        ideas = ideas.Where(x => x.TITLE.Contains(searchItem.Username.Trim()));
                    }
                    if (searchItem.FullName != null && searchItem.FullName.Trim().Length > 0)
                    {
                        if (searchItem.FullName.Trim().Contains(" "))
                        {
                            var firstName = searchItem.FullName.Trim().Substring(0, searchItem.FullName.Trim().IndexOf(" "));
                            var lastName = searchItem.FullName.Trim().Substring(0, searchItem.FullName.Trim().IndexOf(" "));
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
                _filterYearAndMonth(ideas,searchItem.Year,searchItem.Month);

                res = ideas.OrderByDescending(x => x.SAVE_DATE).ToList().Select(x => new IdeaForShowDto() {
                    Id=x.ID,
                    FullName=x.USER.FIRST_NAME+" "+x.USER.LAST_NAME,
                    Title=x.TITLE,
                    Status=x.IDEA_STATUS.TITLE,
                    SaveDate= Persia.Calendar.ConvertToPersian(x.SAVE_DATE).Persian,
                    StatusId=x.STATUS_ID,
                    Username=x.USERNAME,
                    TotalPoints = x.IDEA_POINTS.Sum(w => w.POINT)

                });
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
                    SaveDate = Persia.Calendar.ConvertToPersian(s.IDEA.SAVE_DATE).Persian,
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
                    SaveDate= Persia.Calendar.ConvertToPersian(s.IDEA.SAVE_DATE).Persian,
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
                        TITLE = idea.TITLE,
                        TotalPoints = i.TOTAL_POINT,
                        FullName = idea.USER.FIRST_NAME+" "+idea.USER.LAST_NAME,
                        Username = idea.USERNAME,
                        Status = idea.IDEA_STATUS.TITLE,
                        SaveDate = Persia.Calendar.ConvertToPersian(idea.SAVE_DATE).Persian
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
                        TITLE = idea.TITLE,
                        TotalPoints = x.TOTAL_POINT,
                        FullName = idea.USER.FIRST_NAME+" "+idea.USER.LAST_NAME,
                        Username = idea.USERNAME,
                        Status = idea.IDEA_STATUS.TITLE,
                        SaveDate = Persia.Calendar.ConvertToPersian(idea.SAVE_DATE).Persian
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
                        TITLE = idea.TITLE,
                        TotalPoints = x.TOTAL_POINT,
                        FullName = idea.USER.FIRST_NAME + " " + idea.USER.LAST_NAME,
                        Username = idea.USERNAME,
                        Status = idea.IDEA_STATUS.TITLE,
                        SaveDate = Persia.Calendar.ConvertToPersian(idea.SAVE_DATE).Persian
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

        private void _filterYearAndMonth(IQueryable<IDEA> ideas,int? year,int? month)
        {
            if (year.HasValue == false && month.HasValue == false)
            {
                return;//no change requires
            }
            if (year.HasValue)
            {

               var startDay =new DateTime(
                    Persia.Calendar.ConvertToGregorian(year.Value, 1, 1, Persia.DateType.Persian).Year,
                    Persia.Calendar.ConvertToGregorian(year.Value, 1, 1, Persia.DateType.Persian).Month,
                    Persia.Calendar.ConvertToGregorian(year.Value, 1, 1, Persia.DateType.Persian).Day,
                    0,
                    0,
                    0);
               var endDay = new DateTime(
                    Persia.Calendar.ConvertToGregorian(year.Value, 12, 29, Persia.DateType.Persian).Year,
                     Persia.Calendar.ConvertToGregorian(year.Value, 12, 29, Persia.DateType.Persian).Month,
                     Persia.Calendar.ConvertToGregorian(year.Value, 12, 29, Persia.DateType.Persian).Day,
                    23,
                    59,
                    59); 
                ideas = ideas.Where(x => x.SAVE_DATE >= startDay && x.SAVE_DATE <= endDay);
            }

            if (month.HasValue)
            {
                var startDay = new DateTime(
                  2020,
                   Persia.Calendar.ConvertToGregorian(1398, month.Value, 1, Persia.DateType.Persian).Month,
                   Persia.Calendar.ConvertToGregorian(1398, month.Value,1, Persia.DateType.Persian).Day,
                   0,
                   0,
                   0);
                int dayCountsInMonth;
                if (month.Value <= 6)
                {
                    dayCountsInMonth = 31;
                }
                else
                {
                    dayCountsInMonth = month.Value!=12?30:29;
                }

                var endDay = new DateTime(
                     2020,
                     Persia.Calendar.ConvertToGregorian(1398, month.Value, dayCountsInMonth, Persia.DateType.Persian).Month,
                     Persia.Calendar.ConvertToGregorian(1398, month.Value, dayCountsInMonth, Persia.DateType.Persian).Day,
                     23,
                     59,
                     59);
                if (month.Value != 9)//دی استثنا هست چون قسمت دوم از قسمت اول کوچک تر می شه
                {
                    ideas = ideas.Where(x =>
                      x.SAVE_DATE.DayOfYear >= startDay.DayOfYear && x.SAVE_DATE.DayOfYear <= endDay.DayOfYear
                    );
                }
                else
                {
                    var part1 = ideas.Where(x =>
                       x.SAVE_DATE.Month == 12 && x.SAVE_DATE.Day>= 22&& x.SAVE_DATE.Day <= 31
                    );
                    var part2 = ideas.Where(x =>
   x.SAVE_DATE.Month == 1 && x.SAVE_DATE.Day >= 1 && x.SAVE_DATE.Day <= 20
);
                    ideas = part1.Concat(part2);
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------


    }
}

