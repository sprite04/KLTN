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
    public class NotificationsController : ControllerBase
    {
        private INotificationService _notificationService;
        private IAuthService _authService;

        public NotificationsController(INotificationService notificationService, IAuthService authService)
        {
            _notificationService = notificationService;
            _authService = authService;
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetNotificationsByUser()
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id");
            string userID = null;
            if (user != null)
                userID = user.Value;

            if (userID == null)
                return NotFound();

            var result = await _notificationService.GetNotificationsByUser(userID);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AddNotification(NotificationDto notification) //admin, staff tạo thông báo
        {
            var result = await _notificationService.AddNotification(notification);
            return Ok(result);
        }

        [HttpPut("[action]")]
        [Authorize]
        public async Task<IActionResult> UpdateStatusNotifications()
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id");
            string userID = null;
            if (user != null)
                userID = user.Value;

            if (userID == null)
                return NotFound();

            var result = await _notificationService.UpdateStatusNotifications(userID);
            return Ok(result);
        }
    }
}
