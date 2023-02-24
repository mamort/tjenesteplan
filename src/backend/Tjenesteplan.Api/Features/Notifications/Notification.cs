namespace Tjenesteplan.Api.Features.Notifications
{
    public class Notification
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public bool IsRead { get; set; }
    }
}