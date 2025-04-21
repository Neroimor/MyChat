using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Controllers.ControllerRegister
{
    [Route("/api/[controller]")]
    public class RegisterController : ControllerBase
    {
        [HttpGet("reg")]
        public IActionResult Registration()
        {
            return Ok("Registration");
        }
    }
}
