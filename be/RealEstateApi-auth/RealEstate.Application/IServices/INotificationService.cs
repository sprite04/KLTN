using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.IServices
{
    public interface INotificationService
    {
        public Task<IEnumerable<Notification>> GetNotificationsByUser(string userId);
        public Task<bool> AddNotification(NotificationDto noti);
        public Task<bool> UpdateStatusNotifications(string userId); //chuyển sang trạng thái đã đọc cho tất cả các thông báo của người dùng, cần check lại xem được không

    }
}
