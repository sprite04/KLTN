using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.IServices;
using RealEstate.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private IReportService _reportService;
        private IAuthService _authService;
        public ReportsController(IReportService reportService, IAuthService authService)
        {
            _reportService = reportService;
            _authService = authService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetReports(int ?page, int ? size, bool? status) //lấy ra những bài báo cáo chưa được duyệt
        {
            var result = await _reportService.GetReports(page, size, status);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetReportByPost(int id, int ? page, int ? size, bool status) 
        {
            var result = await _reportService.GetReportsByPost(id, page, size, status);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReport(ReportDto report)
        {
            var creatorID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id").Value;
            if (string.IsNullOrEmpty(creatorID))
                return StatusCode(StatusCodes.Status404NotFound);
            var result = await _reportService.AddReport(report);
            return Ok(result);
        }
    }
}
