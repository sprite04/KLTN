using AutoMapper;
using RealEstate.Application.DTOs;
using RealEstate.Application.IServices;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Infrastructure.Persistence.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddNotification(NotificationDto noti)
        {
            Notification notification = new Notification();
            var result = false;
            try
            {
                notification.PostID = noti.PostID;
                notification.UserID = noti.UserID;
                notification.Content = noti.Content;
                notification.Status = false;
                notification.CreatedDate = DateTime.Now;
                await _unitOfWork.Notifications.AddNotification(notification);
                await _unitOfWork.CompleteAsync();
                result = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUser(string userId)
        {
            var result = await _unitOfWork.Notifications.GetNotificationsByUser(userId);

            return result;
        }

        public async Task<bool> UpdateStatusNotifications(string userId)
        {
            var result = false;

            try
            {
                await _unitOfWork.Notifications.UpdateStatusNotifications(userId);
                await _unitOfWork.CompleteAsync();
                result = true;
            }
            catch(Exception ex)
            {

            }

            return result;
        }
    }
}
