using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Infrastructure.Persistence.Repositories
{
    public interface INotificationRepository
    {
        Task<Notification> AddNotification(Notification notification);
        Task<IEnumerable<Notification>> GetNotificationsByUser(string userId);
        Task<IEnumerable<Notification>> UpdateStatusNotifications(string userId);
    }
}