using IdeaManagement.Helper.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using DataTransferObject.Idea;
using DataTransferObject.Common;


namespace IdeaManagement.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    //[BasicAuthentication]
    public class IdeasController : ApiController
    {
        private readonly Business.Idea _business;
        //-------------------------------------------------------------------------------------------------
        public IdeasController()
        {
            _business = new Business.Idea();
        }
        //-------------------------------------------------------------------------------------------------
        /// <summary>
        /// به ترتیب معکوس زمان ورود ایده ها رو برمی گردونه
        /// نام کاربر  
        /// تاریخ
        /// وضعیت
        /// تایتل
        /// </summary>
        /// <returns></returns>
        [Route("api/Idea/GetAllIdea")]
        [HttpPut]
        public IEnumerable<IdeaForShowDto> GetAllIdea()
        {
            return _business.GetAllIdea();
        }
        //-------------------------------------------------------------------------------------------------
        /// <summary>
        /// یک ایده ی خاص رو با تمام ویپگی هاش و اینکه کی فرستادش ( نام کاربریش)
        /// ...تاریخ و وضعیت و
        /// و اکثر جزئیات قابل نمایشش
        /// /// </summary>
        /// <returns></returns>
        [Route("api/Idea/GetSpecificIdea/{ideaId}")]
        [HttpGet]
        public IdeaDetailForShowDto GetSpecificIdea(int ideaId)
        {
            return _business.GetSpecificIdea(ideaId);
        }
        //-------------------------------------------------------------------------------------------------
        /// <summary>
        /// وضعیت های ممکن برای یک ایده را برای استفاده در یو آی می دهد
        /// /// </summary>
        /// <returns></returns>
        [Route("api/Idea/GetAllIdeaStatus")]
        [HttpGet]
        public List<IdeaStatusDto> GetAllIdeaStatus()
        {
            return _business.GetAllIdeaStatus();
        }
        //-------------------------------------------------------------------------------------------------
        /// <summary>
        /// ثبت ایده ی جدید
        /// </summary>
        /// <returns></returns>
        [Route("api/Idea/SendNewIdea")]
        [HttpPost]
        public HttpResponseMessage SendNewIdea(NewIdeaDto idea)
        {
            var res = _business.SendNewIdea(idea);
            if (res.Value == true)
                return Request.CreateResponse(HttpStatusCode.OK, res.Content);
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, res.Content);
        }
        //-------------------------------------------------------------------------------------------------
        /// <summary>
        /// تغییر ایده
        ///  چک می کنه اگه کمیته وضعیتش رو تغییر داده بود ردش کرده بود یا قبولش کرده بود دیگه نمی شه تغییرش بده
        /// </summary>
        /// <returns></returns>
        [Route("api/Idea/EditIdea/{Ideaid}")]
        [HttpPut]
        public HttpResponseMessage EditIdea(int ideaid, ChangedIdeaDto idea)
        {
            var res = _business.EditIdea(ideaid, idea);
            if (res.Value == true)
                return Request.CreateResponse(HttpStatusCode.OK, res.Content);
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, res.Content);
        }
        //-------------------------------------------------------------------------------------------------
        /// <summary>
        ///رای دادن به کامنت البته اول باید سرچ کنه اگر قبلا رای داده بود رایش رو آپدیت کنه
        /// هرکسی نباید به نظر خودش بتونه رای بده
        /// بعد از نظر کمیته راجع به ایده این بخش قفل می شه
        ///  </summary>
        /// <returns></returns>
        [Route("api/Idea/VoteToIdea")]
        [HttpPost]
        public HttpResponseMessage VoteToIdea(IdeaPointDto voteDetail)
        {
            var res = _business.VoteToIdea(voteDetail);
            if (res.Value == true)
                return Request.CreateResponse(HttpStatusCode.OK, res.Content);
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, res.Content);
        }
        //-------------------------------------------------------------------------------------------------
        /// <summary>
        ///ده ایده ی برتر در کل ایده ها بر اساس تعداد رای های مثبت منهای تعداد رای های منفی مطرح شده
        ///شامل
      ///<th nzWidth="28%">موضوع</th>
      ///<th nzWidth="10%"> امتیاز </ th >
      ///<th nzWidth="18%">نام و نام خانوادگی طراح</th>
      ///<th nzWidth="18%"> نام کاربری</th>
      ///<th nzWidth="9%" > وضعیت </ th >
      ///<th nzWidth="10%">تاریخ مطرح شدن</th>
        /// </summary>
        /// <returns></returns>
        [Route("api/Idea/GetIdeasTop10All")]
        [HttpGet]
        public IEnumerable<IdeaDto> GetIdeasTop10All()
        {
            return _business.GetIdeasTop10All();
        }
        //-------------------------------------------------------------------------------------------------
        /// <summary>
        ///ده ایده ی برتر ماه ایده ها بر اساس تعداد رای های مثبت منهای تعداد رای های منفی مطرح شده
        ///شامل
        ///< th nzWidth="28%">موضوع</th>
        ///<th nzWidth = "10%" > امتیاز </ th >
        ///< th nzWidth="18%">نام و نام خانوادگی طراح</th>
        ///<th nzWidth = "18%" > نام کاربری</th>
        ///<th nzWidth = "9%" > وضعیت </ th >
        ///< th nzWidth="10%">تاریخ مطرح شدن</th>
        /// </summary>
        /// <returns></returns>
        [Route("api/Idea/GetIdeasTop10CurrentMonth")]
        [HttpGet]
        public IEnumerable<IdeaDto> GetIdeasTop10CurrentMonth()
        {
            return _business.GetIdeasTop10CurrentMonth();
        }
        //-------------------------------------------------------------------------------------------------    
        /// <summary>
        ///ده ایده ی برتر هفته ایده ها بر اساس تعداد رای های مثبت منهای تعدا رای های منفی مطرح شده
        ///شامل
        ///< th nzWidth="28%">موضوع</th>
        ///<th nzWidth = "10%" > امتیاز </ th >
        ///< th nzWidth="18%">نام و نام خانوادگی طراح</th>
        ///<th nzWidth = "18%" > نام کاربری</th>
        ///<th nzWidth = "9%" > وضعیت </ th >
        ///< th nzWidth="10%">تاریخ مطرح شدن</th>
        /// </summary>
        /// <returns></returns>
        [Route("api/Idea/GetIdeasTop10CurrentWeek")]
        [HttpGet]
        public IEnumerable<IdeaDto> GetIdeasTop10CurrentWeek()
        {
            return _business.GetIdeasTop10CurrentWeek();
        }
        //-------------------------------------------------------------------------------------------------
        /// <summary>
        /// از
        /// get
        /// می توان استفاده کرد ولی من ترجیحم این بود که در بدنه در خواست ارسال شود
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [Route("api/Idea/FilterSerchingIdea")]
        [HttpPost]
        public IEnumerable<IdeaForShowDto> FilterSerchingIdea(FilterIdeaRequestDto searchItem)
        {
            return  _business.FilterSerchingIdea(searchItem);
        }

        //-------------------------------------------------------------------------------------------------
        [Route("api/Idea/GetAllWinnerIdea")]
        [HttpGet]
        public IEnumerable<WinnerIdeaForShowDto> GetAllWinnerIdea()
        {
            return _business.GetAllWinnerIdea();
        }
        //-------------------------------------------------------------------------------------------------
        [Route("api/Idea/FilterWinnerIdea")]
        [HttpPost]
        public IEnumerable<WinnerIdeaForShowDto> FilterWinnerIdea(FilterWinnerIdeaRequestDto searchItem)
                    {
            return _business.FilterWinnerIdea(searchItem);
        }

        //-------------------------------------------------------------------------------------------------
        [Route("api/Idea/GetAllNotDecidedIdea")]
        [HttpGet]
        public IEnumerable<IdeaForShowDto> GetAllNotDecidedIdea()
        {
            return _business.GetAllNotDecidedIdea();
        }
        //-------------------------------------------------------------------------------------------------   
        [Route("api/Idea/GetAllCurrentMontDecidedIdea")]
        [HttpGet]
        public IEnumerable<IdeaForShowDto> GetAllCurrentMontDecidedIdea()
        {
            return _business.GetAllCurrentMontDecidedIdea();
        }
        //-------------------------------------------------------------------------------------------------   
        [Route("api/Idea/FilterAllNotDecidedIdea")]
        [HttpGet]
        public IEnumerable<IdeaForShowDto> FilterAllNotDecidedIdea(FilterAllNotDecidedIdeaRequestDto searchItem)
        {
            return _business.FilterAllNotDecidedIdea(searchItem);
        }

    }
}