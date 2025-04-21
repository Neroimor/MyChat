using ChatApi.DTO.UserDTO;
using ChatApi.Services.RegisterServices.Interface;
using ChatApi.Services.RegisterServices.Realization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace ChatApi.Controllers.ControllerRegister
{
    [Route("/api/[controller]")]
    public class RegisterController : ControllerBase
    {
        public readonly IRegistrServices _registrServices;

        public RegisterController(RegistrServices registeredServices)
        {
            _registrServices = registeredServices;
        }

        [HttpPost("reg")]
        public async Task<IActionResult> RegistrationAsync([FromBody] UserRegistration userRegistration)
        {
            var result = await _registrServices.RegistrationAsync(userRegistration);

            if (!result._isSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLogin userLogin)
        {

            var result = await _registrServices.LoginAsync(userLogin);

            if (!result._isSuccess && result.notFound)
            {
                return NotFound(result);
            }
            else if(!result._isSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
