using System;
using Tjenesteplan.Domain.Api.Email;

namespace Tjenesteplan.Domain.Notifications.VakansvaktRequestReceivedNotification
{
    public class VakansvaktRequestReceivedEmail : EmailMessage
    {

        private readonly string _initiatorName;
        private readonly DateTime _date;
        private readonly string _url;
        public override string Title => "Forespørsel om vakansvakt";
        public override string Body => CreateEmailBody();

        public VakansvaktRequestReceivedEmail(
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
            return "{name} ønsker å sette vakten sin {date} vakant. <a href=\"{url}\">Godkjenn forespørselen her</a>"
                .Replace("{name}", _initiatorName)
                .Replace("{date}", _date.ToLongDateString())
                .Replace("{url}", _url);
        }

    }
}