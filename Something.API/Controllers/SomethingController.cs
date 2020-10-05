using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Something.Application;

namespace Something.API.Controllers
{
    [Authorize]
    [ApiController]
    public class SomethingController : ControllerBase
    {

        private readonly ISomethingCreateInteractor createInteractor;
        private readonly ISomethingReadInteractor readInteractor;

        public SomethingController(ISomethingCreateInteractor createInteractor, ISomethingReadInteractor readInteractor)
        {
            this.createInteractor = createInteractor;
            this.readInteractor = readInteractor;
        }

        [HttpPost]
        [Route("api/things")]
        public ActionResult Create([FromForm] string name)
        {
            if (name.Length < 1)
                return GetAll();

            createInteractor.CreateSomething(name);
            return GetAll();
        }

        [HttpGet]
        [Route("api/things")]
        public ActionResult GetList()
        {
            return GetAll();
        }
        private ActionResult GetAll()
        {
            var result = readInteractor.GetSomethingList();
            return Ok(result);
        }
    }
}
