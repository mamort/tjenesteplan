using System.Collections.Generic;

namespace Tjenesteplan.Domain.Repositories
{
	public interface INonCachedNotificationsRepository : INotificationRepository {}
	public interface INotificationRepository
	{
	    void AddNotification(int userId, string title, string body);

        IReadOnlyList<Notification> GetUnreadNotifications(User user);
		Notification SetNotificationRead(int id, bool isRead = true);
	}
}
