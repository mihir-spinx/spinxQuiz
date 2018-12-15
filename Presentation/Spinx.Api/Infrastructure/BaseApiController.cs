using System.Web.Http;

namespace Spinx.Api.Infrastructure
{
    public class BaseApiController : ApiController
    {
        protected IHttpActionResult Result(dynamic entity)
        {
            return entity == null ? NotFound() : (IHttpActionResult) Ok(entity);
        }
    }
}