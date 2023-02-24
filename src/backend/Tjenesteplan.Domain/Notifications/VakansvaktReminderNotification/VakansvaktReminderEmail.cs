using System;
using Tjenesteplan.Domain.Api.Email;

namespace Tjenesteplan.Domain.Notifications.VakansvaktReminderNotification
{
    public class VakansvaktReminderEmail : EmailMessage
    {
        private readonly DateTime _date;
        private readonly string _url;
        public override string Title => CreateTitle();
        public override string Body => CreateEmailBody();

        public VakansvaktReminderEmail(
            string email,
            DateTime date,
            string url
        )
            : base(email)
        {
            _date = date;
            _url = url;
        }

        private string CreateTitle()
        {
            return "Påminnelse: Vakansvakt {date} må dekkes"
                .Replace("{date}", _date.ToLongDateString());
        }

        private string CreateEmailBody()
        {
            return "Vakansvakt {date} må dekkes snart. Ta vakten <a href=\"{url}\">her</a>."
                .Replace("{date}", _date.ToLongDateString())
                .Replace("{url}", _url);
        }
    }
}