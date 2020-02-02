using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using IdeaManagement.Helper.Token;
using DataTransferObject.Auth;
using DataTransferObject.Common;

namespace IdeaManagement.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuthController : ApiController
    {
        //see hear for status code https://en.wikipedia.org/wiki/List_of_HTTP_status_codes
        //or https://fa.wikipedia.org/wiki/%D9%81%D9%87%D8%B1%D8%B3%D8%AA_%DA%A9%D8%AF%D9%87%D8%A7%DB%8C_%D9%88%D8%B6%D8%B9%DB%8C%D8%AA_HTTP
        private readonly Business.Auth _business;
        private readonly Business.User _businessUser;
        //-------------------------------------------------------------------------------------------------
        public AuthController()
        {
            _business = new Business.Auth();
            _businessUser = new Business.User();
        }
        //-------------------------------------------------------------------------------------------------

        [Route("api/Auth/Registration")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage UserRegistration([FromBody]UserForRegistrationDto user)
        {
            var res = _business.Registration(user);
            if (res.Value == true)
                return Request.CreateResponse(HttpStatusCode.Created, res.Content);
            else
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, res.Content);
            }
        }
        //-------------------------------------------------------------------------------------------------

        [Route("api/Auth/login")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Login([FromBody]UserForLoginDto user)
        {
            Result res = _business.Login(user);
            if (res.Value )
            return Request.CreateResponse(HttpStatusCode.OK,
                 TokenManager.GenerateToken(user.Username,_businessUser.IsAdmin(user.Username),_businessUser.IsCommitteMember(user.Username)));
            else
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed,
     res.Content);
        }
        //-------------------------------------------------------------------------------------------------
        [Route("api/Auth/ForgetPassword")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage ForgetPassword([FromBody]ForgetPasswordDto user)
        {
            Result res = _business.ForgetPassword(user);
            if (res.Value)
                return Request.CreateResponse(HttpStatusCode.OK,
                     res.Content);
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest,
     res.Content);
        }
        //-------------------------------------------------------------------------------------------------

    }
}