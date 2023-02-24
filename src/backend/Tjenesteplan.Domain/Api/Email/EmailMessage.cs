namespace Tjenesteplan.Domain.Api.Email
{
    public class EmailMessage
    {
        public string Email { get; }
        public virtual string Title { get; }
        public virtual string Body { get; }

        public EmailMessage(string email, string title, string body)
        {
            Email = email;
            Title = title;
            Body = body;
        }

        protected EmailMessage(string email)
        {
            Email = email;
        }
    }
}