namespace Tjenesteplan.Domain
{
	public class Notification
	{
		public int Id { get; }
        public int UserId { get; }
        public string Title { get; }
		public string Body { get; }
		public bool IsRead { get; }

		public Notification(int id, int userId, string title, string body, bool isRead)
		{
			Id = id;
            UserId = userId;
            Title = title;
			Body = body;
			IsRead = isRead;
		}
	}
}
