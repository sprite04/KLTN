using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.IServices;
using RealEstate.Domain.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace RealEstate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportProcessingsController : ControllerBase
    {
        private IReportProcessingService _reportProcessingService;
        private IAuthService _authService;
        public ReportProcessingsController(IReportProcessingService reportProcessingService, IAuthService authService)
        {
            _reportProcessingService = reportProcessingService;
            _authService = authService;
        }

        [HttpPut("[action]")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateStatusReportProcessing(int id, int statusId) //lấy ra danh sách những bài post đến hạn giải trình
        {
            var result = await _reportProcessingService.UpdateStatusReportProcessing(id, statusId);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetReportProcessings(int? page, int? size) //lấy ra những bài đến hạn xử lý
        {
            var result = await _reportProcessingService.GetReportProcessings(page, size); 
            return Ok(result);
        }

        [HttpPut("[action]")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> BlockAllPosts() //khoá tất cả các bài viết đến hạn giải trình nhưng chưa thực hiên
        {
            var result = await _reportProcessingService.BlockAllPosts();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetReportProcessingByPost(int id) //lấy ra report processing của bài đăng, trả về statusId
        {
            var result = await _reportProcessingService.GetReportProcessingByPost(id);
            return Ok(result);
        }
    }
}
