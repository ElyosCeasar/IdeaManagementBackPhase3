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
    public class LottoryController : ApiController
    {

        //این قسمت در فاز نهایی که دیتابیس اصلی وارد شد ساخته می شود
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        //-------------------------------------------------------------------------------------------------

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }
        //-------------------------------------------------------------------------------------------------

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }
        //-------------------------------------------------------------------------------------------------

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }
        //-------------------------------------------------------------------------------------------------

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        //-------------------------------------------------------------------------------------------------

    }
}