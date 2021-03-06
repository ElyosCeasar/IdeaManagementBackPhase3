﻿using DataTransferObject.Comment;
using DataTransferObject.User;
using IdeaManagement.Helper.Authentication;
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
    public class CommentsController : ApiController
    {
        private readonly Business.Comment _business;
        //-------------------------------------------------------------------------------------------------
        public CommentsController()
        {
            _business = new Business.Comment();
        }
        /// <summary>
        /// تمامی نظراتی که به یک ایده داده شدن رو به ترتیب این که چه تعداد 
        /// upvote
        /// شده رو برمی گردونه
        /// </summary>
        /// <returns></returns>
        [Route("api/Comments/GetAllComments/{ideaId}")]
        [HttpGet]
        public IEnumerable<IdeaCommentsDto> GetAllComments(int ideaId)
        {
            return _business.GetAllComments(ideaId);
        }
        //-------------------------------------------------------------------------------------------------
        /// <summary>
        /// گرفتن یک ایده‌ی خاص به کمک آیدی آن
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [Route("api/Comments/GetSpeceficComment/{commentId}")]
        [HttpGet]
        public IdeaCommentsDto GetSpeceficComment(int commentId)
        {
            return _business.GetSpeceficComment(commentId);
        }
        //-------------------------------------------------------------------------------------------------
        /// <summary>
        /// کاربر بتونه به ایده نظر بده
        /// بعد از نظر کمیته راجع به ایده این بخش قفل می شه
        /// </summary>
        /// <returns></returns>
        [Route("api/Comments/AddCommentToIdea")]
        [HttpPost]
        public HttpResponseMessage AddCommentToIdea(CommentDto newcomment)
        {
            var res = _business.AddCommentToIdea(newcomment);
            if (res.Value == true)
                return Request.CreateResponse(HttpStatusCode.OK, res.Content);
            else
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, res.Content);
        }
        //-------------------------------------------------------------------------------------------------
        /// <summary>
        /// کاربر بتونه نظری که داده رو تغییر بده
            /// بعد از نظر کمیته راجع به ایده این بخش قفل می شه
        /// </summary>
        /// <returns></returns>
        [Route("api/Comments/UpdateComment")]
        [HttpPut]
        public HttpResponseMessage UpdateComment(CommentDto newcomment)
        {
            var res = _business.UpdateComment(newcomment);
            if (res.Value == true)
                return Request.CreateResponse(HttpStatusCode.OK, res.Content);
            else
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, res.Content);
        }

        //-------------------------------------------------------------------------------------------------
        /// <summary>
        /// حذف پیشنهاد
        ///  چک می کنه اگه کمیته وضعیت ایدش رو تغییر داده بود ردش کرده بود یا قبولش کرده بود دیگه نمی شه حذفش کرد
        /// </summary>
        /// <returns></returns>
        [Route("api/Comments/DeleteComment/{commentId}")]
        [HttpDelete]
        public HttpResponseMessage DeleteComment(int commentId)
        {
            var res = _business.DeleteComment(commentId);
            if (res.Value == true)
                return Request.CreateResponse(HttpStatusCode.OK, res.Content);
            else
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, res.Content);
        }
        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// رای دادن به کامنت البته اول سرچ می کنه اگر قبلا رای داده بود رایش رو آپدیت کنی
        /// voteDetail ={Ideaid,userid,isUpVote}
        /// هرکسی نباید به نظر خودش بتونه رای بده
        /// بعد از نظر کمیته راجع به ایده این بخش قفل می شه
        ///         /// </summary>
        /// <returns></returns>
        [Route("api/Comments/VoteToComment")]
        [HttpPost]
        public HttpResponseMessage VoteToComment(VoteToCommentDto voteDetail)
        {
            var res = _business.VoteToComment(voteDetail);
            if (res.Value == true)
                return Request.CreateResponse(HttpStatusCode.OK, res.Content);
            else
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, res.Content);
        }
        //-------------------------------------------------------------------------------------------------

    }
}