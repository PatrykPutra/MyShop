using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Entities;
using MyShop.Models;
using MyShop.Services;

namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserInfoController : ControllerBase
    {
        private readonly IUserContextService _userContextService;
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;
        public UserInfoController(IUserContextService userContextService, IUserServices userServices, IMapper mapper)
        {
            _userContextService = userContextService;
            _userServices = userServices;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetUserInfoAsync()
        {
            try
            {
                User user = await _userServices.GetAsync(_userContextService.GetUserId());
                UserInfoDto userInfoDto = _mapper.Map<UserInfoDto>(user);
                return Ok(userInfoDto);
            }
            catch (Exception)
            {
                return Ok(new UserInfoDto { UserName = "quest", IsAdmin = false, Email = "undefined" });
            }
  
        }
    }
}
