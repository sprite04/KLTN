using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Persistence.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(RealEstateDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<Notification> AddNotification(Notification notification)
        {
            var result = await _dbSet.AddAsync(notification);
            return result.Entity;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUser(string userId)
        {
            var result = await _dbSet.Where(x => x.UserID.Equals(userId)).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Notification>> UpdateStatusNotifications(string userId)
        {
            var result = await _dbSet.Where(x => x.UserID == userId && x.Status != true).ToListAsync();

            foreach (var value in result)
            {
                value.Status = true;
            }

            return result;
        }
    }
}
