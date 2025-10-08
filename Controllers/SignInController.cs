using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Models;
using MyShop.Services;

namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class SignInController : ControllerBase
    {
        private IUserServices _userServices;
        private ILoginServices _loginServices;
        private ILogoutServices _logoutServices;
        public SignInController(IUserServices userServices, ILoginServices loginServices, ILogoutServices logoutServices)
        {
            _userServices = userServices;
            _loginServices = loginServices;
            _logoutServices = logoutServices;
        }

        [HttpPost("NewUser")]
        public async Task<IActionResult> RegisterAsync([FromBody] CreateUserDto userDto)
        {
           
            await _userServices.CreateAsync(userDto);
            return Ok("New user has been registered.");  
        }

        [HttpPost("User")]
        public async Task<IActionResult> Login([FromBody] CredentialsDto credentials)
        {
            string token = await _loginServices.GenerateJwt(credentials);
            return Ok(new TokenDto(token));
    
        }
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            string token = await _logoutServices.GenerateJwt();
            return Ok(new TokenDto(token));

        }
    }
}
