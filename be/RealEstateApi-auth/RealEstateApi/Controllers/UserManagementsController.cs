using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.DTOs;
using RealEstate.Application.Models;

namespace RealEstate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementsController : ControllerBase
    {
        private IUserService _userService;
        public UserManagementsController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("[action]")]
        //[Authorize(Roles = "Admin")]   -- comment tạm, cần xem thử có cần việc authorize ở đây hay không
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _userService.GetAllRoles();
            return Ok(result);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers([FromQuery] UserFilterParams filterParams)
        {
            var result = await _userService.GetUsers(filterParams);
            return Ok(result);
        }

        //Them search nguoi dung neu co the

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserById(string id)     // -----------------check lai dang bị
        {
            var result = await _userService.GetUserById(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromForm] UserRequest userRequest)
        {
            var result = await _userService.CreateUser(userRequest);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateAdmin([FromForm] UserRequest userRequest) //hàm này chỉ được chạy lần đầu khi app mới tạo để tạo tài khoản admin
        {
            var result = await _userService.CreateUser(userRequest);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser([FromForm] UserDto userDto)  // -----------------CAN XEM LAI O DAY 
        {
            var result = await _userService.UpdateUser(userDto);
            return Ok(result);
        }

        [HttpDelete("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LockUnlock(string userId)
        {
            var result = await _userService.LockUnlock(userId);
            return Ok(result);
        }
    }
}
