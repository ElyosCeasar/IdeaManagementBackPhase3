using DataTransferObject.Committee;
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
    public class CommitteeController : ApiController
    { 
        private readonly Business.Committee _business;
        //-------------------------------------------------------------------------------------------------
        public CommitteeController()
        {
            _business = new Business.Committee(); 
        }
        //-------------------------------------------------------------------------------------------------
        /// <summary>
        /// رد شدن و تایید شدن ایده
        /// 1 تایید شده و کوچک تر از
        /// 2 و بیشتر
        /// رد شده
        /// </summary>
        /// <returns></returns>
        [Route("api/Committee/VoteToIdea/{ideaId}")]
        [HttpPost]
        public HttpResponseMessage VoteToIdea(int ideaId, [FromBody] VoteDetailDto voteDetailDto)
        {
            var res = _business.VoteToIdea(ideaId,voteDetailDto);
            if (res.Value == true)
                return Request.CreateResponse(HttpStatusCode.OK, res.Content);
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, res.Content);
        }
        //-------------------------------------------------------------------------------------------------
        /// <summary>
        /// تجدید نظر رای کمیته با حذف رای
        /// تنها در ماه جاری می توان تجدید نظر کرد
        /// </summary>
        /// <returns></returns>
        [Route("api/Committee/UnVoteIdea/{ideaId}")]
        [HttpDelete]
        public HttpResponseMessage UnVoteIdea(int ideaId ,string username)
        {
            var res = _business.UnVoteIdea(ideaId,username);
            if (res.Value == true)
                return Request.CreateResponse(HttpStatusCode.OK, res.Content);
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, res.Content);
        }
    }
}
