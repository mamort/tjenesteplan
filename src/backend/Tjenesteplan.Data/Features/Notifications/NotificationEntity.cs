using WebApi.Features.Users.Data;

namespace Tjenesteplan.Data.Features.Notifications
{
	public class NotificationEntity
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Body { get; set; }
		public bool IsRead { get; set; }
		public UserEntity User { get; set; }
		public int UserId { get; set; }
	}
}
