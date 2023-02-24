using System;
using Tjenesteplan.Domain.Api.Email;

namespace Tjenesteplan.Domain.Notifications.VakansvaktRequestAcceptedNotification
{
    public class VakansvaktRequestAcceptedEmail : EmailMessage
    {
        private readonly DateTime _date;
        private readonly string _acceptorName;
        public override string Title => "Vakansvakt tildelt";
        public override string Body => CreateEmailBody();

        public VakansvaktRequestAcceptedEmail(
            string email,
            DateTime date,
            string acceptorName
        )
            : base(email)
        {
            _date = date;
            _acceptorName = acceptorName;
        }


        private string CreateEmailBody()
        {
            return "Vakansvakt {date} er tildelt {acceptorName}."
                .Replace("{date}", _date.ToLongDateString())
                .Replace("{acceptorName}", _acceptorName);
        }

    }
}