using IdeaManagement.Helper.Authentication;
using DataTransferObject.User;
using DataTransferObject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IdeaManagement.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    //[BasicAuthentication]
    public class UsersController : ApiController
    {
        private readonly Business.User _business;
        //-------------------------------------------------------------------------------------------------
        public UsersController()
        {
            _business = new Business.User();
        }
        //-------------------------------------------------------------------------------------------------

        [Route("api/User/GetAllUsers")]
        [HttpGet]
        public IEnumerable<UserForShowDto> GetAllUsers()
        {
            IEnumerable<UserForShowDto> res = _business.GetAllUsers();
            return res;
        }
        //-------------------------------------------------------------------------------------------------

        [Route("api/User/GetUserProfile/{username}")]
        [HttpGet]
        public UserForProfileDto GetUserProfile(string username)
        {
            return _business.GetUserProfile(username);
        }
        //-------------------------------------------------------------------------------------------------

        [Route("api/User/PutUserProfile")]
        [HttpPut]
        public HttpResponseMessage PutUserProfile( [FromBody]ProfileForUpdateDto newProfile)
        {
            var queryGenerator = new Business.User();
            Result res = queryGenerator.PutUserProfile(newProfile);
            if (res.Value == true)
                return Request.CreateResponse(HttpStatusCode.OK, res.Content);
            else
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, res.Content);
            }
        }
        //-------------------------------------------------------------------------------------------------
        [Route("api/User/IsAdmin/{username}")]
        [HttpGet]
        public bool IsAdmin(string username)
        {
            return _business.IsAdmin(username);
        }
        //-------------------------------------------------------------------------------------------------
        [Route("api/User/IsCommitteMember/{username}")]
        [HttpGet]
        public bool IsCommitteMember(string username)
        {
            return _business.IsCommitteMember(username);
        }
        //-------------------------------------------------------------------------------------------------
        /// <summary>
        /// تغییر فلگ کمیته با توجه به مفدار ارسالی
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>

        [Route("api/User/ChangeCommitteFlags")]
        [HttpPut]
        public HttpResponseMessage ChangeCommitteFlag(UserChangeCommitteeFlagDto info)
        {
            Result res = _business.ChangeCommitteFlag(info.Username,info.Value);
            if (res.Value == true)
                return Request.CreateResponse(HttpStatusCode.OK, res.Content);
            else
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, res.Content);
        }

        //-------------------------------------------------------------------------------------------------
        /// <summary>
        /// از
        /// get
        /// می توان استفاده کرد ولی من ترجیحم این بود که در بدنه در خواست ارسال شود
        /// فیلتر کردن بر اساس 
        /// 1- نامکامل(نام و نام خانوادگی)
        /// 2-نام کاربری
        /// 3-عضو کمیته بودن یا نبودن مقدار 0 یا 1 یا 2 یا 3 می گیرد 0 به معنای کاربر معمولی 1 به معنای عضو کمیته 2 به معنای ادمین 3 به معنای همه
        /// بدیه ی است که می تواند بعضی از موارد نال یا خالی باشد
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [Route("api/User/FilterSerching")]
        [HttpPost]
        public IEnumerable<UserForShowDto> FilterSerchingUsers(FilterUserRequestDto searchItem)
        {
            return _business.FilterSerchingUsers(searchItem);
        }
        //-------------------------------------------------------------------------------------------------
        /// <summary>
        /// به متد بالا نگاه کنید در بخش 3 ی آن ما یک دراپ داون داریم
        /// برای آن یک مقدار و یک تایتل برگردانید
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [Route("api/User/GetPositionType")]
        [HttpGet]
        public Array GetDropDownUserType()
        {
            var Items =new[] 
            {
                new { title = "کاربر معمولی", value = 0 },
                new { title = "عضو کمیته", value = 1 },
                new { title = "ادمین", value = 2 },
                new { title = "همه ی موارد", value = 3 }
            };

            return Items;
        }
        //-------------------------------------------------------------------------------------------------    
        /// <summary>
        ///ده نفر برتر ایده پرداز براساس مجموع رای های مثبت و منفی
        ///شامل
    ///  <th nzWidth = "20%" > نام و نام خانوادگی طراح</th>
    /// <th nzWidth = "20%" > نام کاربری</th>
    /// <th nzWidth = "20%" > تعداد ایده ها</th>
    /// <th nzWidth = "20%" > مجموع امتیاز ها</th>
    /// <th nzWidth = "13%" > تعداد ایده های برنده</th>
        /// </summary>
        /// <returns></returns>
        [Route("api/User/GetTop10IdeaMaker")]
        [HttpGet]
        public IEnumerable<UserShowingTop10Dto> GetTop10IdeaMaker()
        {

            return _business.GetTop10IdeaMaker();
        }
        //-------------------------------------------------------------------------------------------------    
        /// <summary>
        ///ده انفر برتر پیشنهاد پرداز براساس مجموع رای های مثبت و منفی
        ///شامل
    /// < th nzWidth="23%">نام و نام خانوادگی طراح</th>
    /// <th nzWidth = "23%" > نام کاربری</th>
    /// <th nzWidth = "24%" > تعداد پیشنهادها</th>
    /// <th nzWidth = "20%" > مجموع امتیاز ها</th>
        /// </summary>
        /// <returns></returns>
        [Route("api/User/GetTop10CommentMaker")]
        [HttpGet]
        public IEnumerable<UserShowingTop10Dto> GetTop10CommentMaker()
        {
            return _business.GetTop10CommentMaker();
        }

    }
}