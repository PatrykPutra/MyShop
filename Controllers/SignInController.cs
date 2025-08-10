using Microsoft.AspNetCore.Mvc;
using MyShop.Models;
using MyShop.Services;

namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignInController : ControllerBase
    {
        private IUserServices _userServices;
        private ILoginServices _loginServices;
        public SignInController(IUserServices userServices, ILoginServices loginServices)
        {
            _userServices = userServices;
            _loginServices = loginServices;
        }

        [HttpPost("NewUser")]
        public async Task<IActionResult> RegisterAsync([FromBody] CreateUserDto userDto)
        {
           
            await _userServices.CreateAsync(userDto);
            return Ok("New user has been registered.");
            
        }

        [HttpPost("User")]
        public async Task<IActionResult> LoginAsync([FromBody] CredentialsDto credentials)
        {
            
            string responce = await _loginServices.Login(credentials);
            return Ok(responce);
    
        }
    }
}
