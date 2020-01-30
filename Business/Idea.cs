using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObject.Common;
using DataTransferObject.Idea;


namespace Business
{
    public class Idea
    {
        private readonly DataAccess.Query.IdeaQ _Repository;
        //-------------------------------------------------------------------------------------------------
        public Idea()
        {
            _Repository = new DataAccess.Query.IdeaQ();
        }
        //-------------------------------------------------------------------------------------------------
        public IEnumerable<IdeaForShowDto> GetAllIdea()
        {
            return _Repository.GetAllIdea();
        }
        //-------------------------------------------------------------------------------------------------
        public IdeaDetailForShowDto GetSpecificIdea(int ideaId)
        {
            return _Repository.GetSpecificIdea(ideaId);
        }
        //-------------------------------------------------------------------------------------------------
        public Result SendNewIdea(NewIdeaDto idea)
        {
            Result result = new Result();
            if (
                idea.Advantages==null ||
                idea.CurrentSituation== null ||
                idea.Prerequisite== null ||
                idea.Steps== null ||
                idea.Title== null ||
                idea.Username== null
                )
            {
                result.Value = false;
                result.Content = "همه‌ی بخش‌های اجباری پر نشده است";
                return result;
            }
            return _Repository.SendNewIdea(idea);
        }
        //-------------------------------------------------------------------------------------------------
        public Result EditIdea(int ideaId, ChangedIdeaDto idea)
        {
            if (_Repository.IsIdeaLocked(ideaId))
            {
                return new Result()
                {
                    Value = false,
                    Content = "ایده توسط کمیته بررسی شده و یا وجود ندارد و قابل تغییر نیست"
                };
            }else
            return _Repository.EditIdea(ideaId, idea);
        }

        //-------------------------------------------------------------------------------------------------
        public Result VoteToIdea(IdeaPointDto vote)
        {
            if (vote.Point < 0)
            {
                vote.Point = -1;
            }else if (vote.Point > 0)
            {
                vote.Point = 1;
            }
            else
            {
                vote.Point = 0;
            }
            if (_Repository.IsIdeaLocked(vote.IdeaId))
            {
                
                return new Result()
                {
                    Value = false,
                    Content = "ایده توسط کمیته بررسی شده و یا وجود ندارد و قابل رای دهی نیست"
                };
            }
            else
                return _Repository.VoteToIdea(vote);
        }

        //-------------------------------------------------------------------------------------------------

        public List<IdeaStatusDto> GetAllIdeaStatus()
        {
            return _Repository.GetAllIdeaStatus();
        }

        //-------------------------------------------------------------------------------------------------
        public IEnumerable<IdeaDto> GetIdeasTop10All()
        {
            return _Repository.GetIdeasTop10All();
        }
        
        //-------------------------------------------------------------------------------------------------
        public IEnumerable<IdeaDto> GetIdeasTop10CurrentMonth()
        {
            return _Repository.GetIdeasTop10CurrentMonth();
        }
        
        //-------------------------------------------------------------------------------------------------
        public IEnumerable<IdeaDto> GetIdeasTop10CurrentWeek()
        {
            return _Repository.GetIdeasTop10CurrentWeek();
        }

        public IEnumerable<IdeaForShowDto> FilterSerchingIdea(FilterIdeaRequestDto searchItem)
        {
            return _Repository.FilterSerchingIdea(searchItem);
        }
        //-------------------------------------------------------------------------------------------------

        public IEnumerable<WinnerIdeaForShowDto> GetAllWinnerIdea()
        {
            return _Repository.GetAllWinnerIdea();
        }
        //-------------------------------------------------------------------------------------------------

        public IEnumerable<WinnerIdeaForShowDto> FilterWinnerIdea(FilterWinnerIdeaRequestDto searchItem)
        {
            return _Repository.FilterWinnerIdea(searchItem);
        }
        //-------------------------------------------------------------------------------------------------

        public IEnumerable<IdeaForShowDto> FilterAllNotDecidedIdea(FilterAllNotDecidedIdeaRequestDto searchItem)
        {
            return _Repository.FilterAllNotDecidedIdea(searchItem);
        }
        //-------------------------------------------------------------------------------------------------

        public IEnumerable<IdeaForShowDto> GetAllCurrentMontDecidedIdea()
        {
            return _Repository.GetAllCurrentMontDecidedIdea();
        }
        //-------------------------------------------------------------------------------------------------

        public IEnumerable<IdeaForShowDto> GetAllNotDecidedIdea()
        {
            return _Repository.GetAllNotDecidedIdea();
        }
        //-------------------------------------------------------------------------------------------------

        public IEnumerable<int> GetYearsFromOldestIdea()
        {
            return _Repository.GetYearsFromOldestIdea();
        }
        //-------------------------------------------------------------------------------------------------

    }
}
