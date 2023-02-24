using System;
using Tjenesteplan.Domain.Api.Email;

namespace Tjenesteplan.Domain.Notifications.VakansvaktRequestApprovedNotification
{
    public class VakansvaktRequestApprovedEmail : EmailMessage
    {
        private readonly DateTime _date;
        public override string Title => "Forespørsel om vakansvakt godkjent";
        public override string Body => CreateEmailBody();

        public VakansvaktRequestApprovedEmail(
            string email,
            DateTime date)
            : base(email)
        {
            _date = date;
        }


        private string CreateEmailBody()
        {
            return "Forespørselen om vakansvakt {date} er godkjent av listefører."
                .Replace("{date}", _date.ToLongDateString());
        }

    }
}