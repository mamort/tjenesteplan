using System;
using Tjenesteplan.Domain.Api.Email;

namespace Tjenesteplan.Domain.Notifications.VaktChangeSuggestionsReceivedNotification
{
    public class VaktChangeSuggestionsAvailableEmail : EmailMessage
    {
        private readonly string _respondentName;
        private readonly DateTime _date;
        private readonly string _url;
        public override string Title => "Forslag til vaktbytte mottatt";
        public override string Body => CreateEmailBody();

        public VaktChangeSuggestionsAvailableEmail(
            string email,
            string respondentName,
            DateTime date,
            string url)
            : base(email)
        {
            _respondentName = respondentName;
            _date = date;
            _url = url;
        }


        private string CreateEmailBody()
        {
            return "{name} har sendt inn forslag for å bytte vakten din {date}. <a href=\"{url}\">Godta vaktbytte</a>"
                .Replace("{name}", _respondentName)
                .Replace("{date}", _date.ToLongDateString())
                .Replace("{url}", _url);
        }
    }
}