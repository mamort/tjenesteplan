using System.Collections.Generic;
using System.Linq;
using Tjenesteplan.Data.Contexts;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Data.Features.Notifications
{
	public class NotificationRepository : INonCachedNotificationsRepository
	{
		private readonly DataContext _datacontext;

		public NotificationRepository(DataContext datacontext)
		{
			_datacontext = datacontext;
		}

	    public void AddNotification(int userId, string title, string body)
	    {
	        _datacontext.Notifications.Add(new NotificationEntity()
	        {
                UserId = userId,
                Title = title,
                Body = body
	        });

	        _datacontext.SaveChanges();

	    }

		public IReadOnlyList<Notification> GetUnreadNotifications(User user)
		{
			return _datacontext.Notifications
				.Where(entity => entity.UserId == user.Id && !entity.IsRead)
                .OrderByDescending(e => e.Id)
				.Select(entity => CreateNotification(entity))
				.ToList();

		}

        public Notification SetNotificationRead(int id, bool isRead = true)
		{
			var notificationToUpdate = _datacontext.Notifications.FirstOrDefault(entity => entity.Id == id);

			if (notificationToUpdate == null)
			{
				return null;
			}

			notificationToUpdate.IsRead = isRead;

			_datacontext.SaveChanges();

			return CreateNotification(notificationToUpdate);
		}

        private static Notification CreateNotification(NotificationEntity entity)
        {
            return new Notification(entity.Id, entity.UserId, entity.Title, entity.Body, entity.IsRead);
        }
	}
}
