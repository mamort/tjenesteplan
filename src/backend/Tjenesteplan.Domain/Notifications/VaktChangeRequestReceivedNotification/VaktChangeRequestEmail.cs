using System;
using Tjenesteplan.Domain.Api.Email;

namespace Tjenesteplan.Domain.Notifications.VaktChangeRequestReceivedNotification
{
    public class VaktChangeRequestEmail : EmailMessage
    {
        private readonly string _initiatorName;
        private readonly DateTime _date;
        private readonly string _url;
        public override string Title => "Forespørsel om vaktbytte";
        public override string Body => CreateEmailBody();

        public VaktChangeRequestEmail(
            string email, 
            string initiatorName, 
            DateTime date,
            string url)
            : base(email)
        {
            _initiatorName = initiatorName;
            _date = date;
            _url = url;
        }


        private string CreateEmailBody()
        {
            return "{name} ønsker å bytte vakten sin {date}. <a href=\"{url}\">Svar på forespørselen</a>"
                .Replace("{name}", _initiatorName)
                .Replace("{date}", _date.ToLongDateString())
                .Replace("{url}", _url);
        }

    }
}