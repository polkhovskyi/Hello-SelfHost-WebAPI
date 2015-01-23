using System.Collections.Generic;
using System.Web.Http;
using Hello_SelfHost_WebAPI.Attributes;

namespace Hello_SelfHost_WebAPI.Controllers
{
    [MembershipHttpAuthorize]
    public class ValuesController : ApiController
    {
        // GET api/values 
        public IEnumerable<string> Get()
        {
            return new[] {"value1", "value2"};
        }

        public string Get(int id)
        {
            return "value";
        }
    }
}