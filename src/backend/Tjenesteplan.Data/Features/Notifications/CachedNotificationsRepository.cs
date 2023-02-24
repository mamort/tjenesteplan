using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Data.Features.Notifications
{
    public class CachedNotificationsRepository : INotificationRepository
    {
        private string NotificationsCacheKey(int id) => $"unread-notifications-{id}";
        private readonly IMemoryCache _memoryCache;
        private readonly INonCachedNotificationsRepository _notificationsRepository;

        public CachedNotificationsRepository(
            IMemoryCache memoryCache,
            INonCachedNotificationsRepository notificationsRepository
        )
        {
            _memoryCache = memoryCache;
            _notificationsRepository = notificationsRepository;
        }

        public void AddNotification(int userId, string title, string body)
        {
            _notificationsRepository.AddNotification(userId, title, body);
            _memoryCache.Remove(NotificationsCacheKey(userId));
        }

        public IReadOnlyList<Notification> GetUnreadNotifications(User user)
        {
            return _memoryCache.GetOrCreate(NotificationsCacheKey(user.Id), entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return _notificationsRepository.GetUnreadNotifications(user);
            });
        }

        public Notification SetNotificationRead(int id, bool isRead = true)
        {
            var notification = _notificationsRepository.SetNotificationRead(id, isRead);
            _memoryCache.Remove(NotificationsCacheKey(notification.UserId));
            return notification;
        }
    }
}