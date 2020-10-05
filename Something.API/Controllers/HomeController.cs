using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Something.Security;

namespace Something.API.Controllers
{
    [Authorize]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ISomethingUserManager userManager;
        public HomeController(ISomethingUserManager userManager)
        {
            this.userManager = userManager;
        }

        [AllowAnonymous]
        [Route("home/authenticate")]
        public ActionResult Authenticate()
        {
            var token = userManager.GetUserToken();
            return Ok(new { access_token = token});
        }
    }
}
