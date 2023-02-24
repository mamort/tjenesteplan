using System;
using Tjenesteplan.Domain.Api.Email;

namespace Tjenesteplan.Domain.Notifications.VakansvaktRequestApprovedNotification
{
    public class VakansvaktAvailableEmail : EmailMessage
    {
        private readonly DateTime _date;
        private readonly string _url;
        public override string Title => CreateTitle();
        public override string Body => CreateEmailBody();

        public VakansvaktAvailableEmail(
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
            return "Ny vakansvakt {date} må dekkes"
                .Replace("{date}", _date.ToLongDateString());
        }

        private string CreateEmailBody()
        {
            return "Ny vakansvakt {date} må dekkes. Ta vakten <a href=\"{url}\">her</a>."
                .Replace("{date}", _date.ToLongDateString())
                .Replace("{url}", _url);
        }
    }
}