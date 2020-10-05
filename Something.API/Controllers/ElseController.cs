using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Something.Application;

namespace Something.API.Controllers
{
    [Authorize]
    [ApiController]
    public class ElseController : ControllerBase
    {
        private readonly ISomethingElseCreateInteractor createInteractor;
        private readonly ISomethingElseReadInteractor readInteractor;
        private readonly ISomethingElseUpdateInteractor updateInteractor;
        private readonly ISomethingElseDeleteInteractor deleteInteractor;

        public ElseController(ISomethingElseCreateInteractor createInteractor, ISomethingElseReadInteractor readInteractor, ISomethingElseUpdateInteractor updateInteractor, ISomethingElseDeleteInteractor deleteInteractor)
        {
            this.createInteractor = createInteractor;
            this.readInteractor = readInteractor;
            this.updateInteractor = updateInteractor;
            this.deleteInteractor = deleteInteractor;
        }
        [HttpPost]
        [Route("api/thingselse")]
        public ActionResult CreateElse([FromForm] string name, [FromForm] string[] othername)
        {
            if (name.Length < 1)
                return GetAllSomethingElseIncludeSomething();

            createInteractor.CreateSomethingElse(name, othername);
            return GetAllSomethingElseIncludeSomething();
        }
        [HttpPut]
        [Route("api/thingselse/{id}")]
        public ActionResult Put(int id, [FromForm] string othername)
        {
            if (id < 1)
                return GetAllSomethingElseIncludeSomething();

            updateInteractor.UpdateSomethingElseAddSomething(id, othername);
            return GetAllSomethingElseIncludeSomething();
        }
        [HttpDelete]
        [Route("api/thingselse/{else_id}/{something_id}")]
        public ActionResult Delete(int else_id, int something_id)
        {
            if (else_id < 1 || something_id < 1)
                return GetAllSomethingElseIncludeSomething();

            updateInteractor.UpdateSomethingElseDeleteSomething(else_id, something_id);
            return GetAllSomethingElseIncludeSomething();
        }
        [HttpDelete]
        [Route("api/thingselse/{else_id}")]
        public ActionResult Delete(int else_id)
        {
            if (else_id < 1)
                return GetAllSomethingElseIncludeSomething();

            deleteInteractor.DeleteSomethingElse(else_id);
            return GetAllSomethingElseIncludeSomething();
        }
        [HttpGet]
        [Route("api/thingselse")]
        public ActionResult GetElseList()
        {
            return GetAllSomethingElseIncludeSomething();
        }
        private ActionResult GetAllSomethingElseIncludeSomething()
        {
            var result = readInteractor.GetSomethingElseIncludingSomethingsList();
            return Ok(result);
        }
    }
}
