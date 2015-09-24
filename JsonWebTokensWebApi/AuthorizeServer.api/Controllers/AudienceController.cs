using System.Web.Http;
using AuthorizeServer.api.Models;

namespace AuthorizeServer.api.Controllers
{
    [RoutePrefix("api/audience")]
    public class AudienceController : ApiController
    {
        [Route("")]
        public IHttpActionResult Post(AudienceModel audienceModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newAudience = AudiencesStore.AddAudience(audienceModel.Name);

            return Ok(newAudience);
        }
    }
}