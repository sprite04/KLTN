using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.IServices;
using RealEstate.Application.Models;
using RealEstate.Application.Models.Request;
using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RealEstate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        
        private IAuthService _authService;
        public AuthsController(IAuthService authService, UserManager<User> userManager)
        {
            _authService = authService;
        }


        // GET: api/<AuthsController>
        [HttpGet]
        public async Task<IActionResult> VerifyPhoneNumber(string phone)
        {
            var result = await _authService.VerifyPhoneNumber(phone);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.Login(request);
                if (result != null)
                {
                    return Ok(result);
                }    
            }
            Response response = new Response();
            response.Succeeded = false;
            response.Errors = "Số điện thoại hoặc mật khẩu không chính xác. Vui lòng kiểm tra lại.";
            return Ok(response);
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id").Value;
                var result = await _authService.Logout(id);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
            
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePassword)
        {
            try
            {
                var username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value;
                if (ModelState.IsValid)
                {
                    var result = await _authService.ChangePassword(changePassword, username);
                    if (result == null)
                        return NotFound();
                    return Ok(result);
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> ChangeInfo([FromForm] ChangeInfoRequest changeInfo)
        {
            try
            {
                var username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value;
                if (ModelState.IsValid)
                {
                    var result = await _authService.ChangeInfo(changeInfo, username);
                    if (result == null)
                        return NotFound();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.Refresh(refreshRequest);
                if (result != null)
                {
                    return Ok(result);
                }
            }
            Response response = new Response();
            response.Succeeded = false;
            response.Errors = "Vui lòng đăng nhập lại.";
            return Ok(response);
        }

        // POST api/<AuthsController>
        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.Register(request);
                return Ok(result);
            }
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> Info()
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id");
            string userID = null;
            if (user != null)
                userID = user.Value;

            if (userID==null)
                return StatusCode(StatusCodes.Status404NotFound);

            var result = await _authService.GetUserById(userID);
            return Ok(result);
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPassword(ResetPassword request)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.ResetPassword(request);
                return Ok(result);
            }
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendCodeResetPassword(string phone)
        {
            var result = await _authService.SendCodeResetPassword(phone);
            return Ok(result);
        }
    }
}


