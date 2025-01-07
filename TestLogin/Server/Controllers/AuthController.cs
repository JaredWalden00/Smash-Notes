using Microsoft.AspNetCore.Mvc;
using TestLogin.Client.Services;
using TestLogin.Server.Data;

namespace TestLogin.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authRepo;
        public AuthController(IAuthService authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register([FromBody] UserRegisterDto request)
        {
			var response = await _authRepo.Register(
				new User { Username = request.Username }, request.Password
			);
			if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login(UserLoginDto request)
        {
            var response = await _authRepo.Login(request.Username, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
