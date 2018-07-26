using Microsoft.AspNetCore.Mvc;

namespace CaseKata.Controllers
{
    [Route("api/casefile")]
    [ApiController]
    public class CaseFileController : ControllerBase
    {
        //GET api/casefile/30
        [HttpGet("{id}")]
        public ActionResult<string> Get(int docketId)
        {
            return "Resource Not Found";
        }
    }
}