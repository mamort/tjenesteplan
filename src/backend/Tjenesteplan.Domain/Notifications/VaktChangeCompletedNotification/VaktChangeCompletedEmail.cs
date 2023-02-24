using System;
using Tjenesteplan.Domain.Api.Email;

namespace Tjenesteplan.Domain.Notifications.VaktChangeCompletedNotification
{
    public class VaktChangeCompletedEmail : EmailMessage
    {
        private readonly string _nameOfOtherUser;
        private readonly DateTime _dateToChange;
        private readonly DateTime _dateChangedTo;
        public override string Title => "Vaktbytte fullført";
        public override string Body => CreateEmailBody();

        public VaktChangeCompletedEmail(
            string email, 
            string nameOfOtherUser, 
            DateTime dateToChange,
            DateTime dateChangedTo)
            : base(email)
        {
            _nameOfOtherUser = nameOfOtherUser;
            _dateToChange = dateToChange;
            _dateChangedTo = dateChangedTo;
        }


        private string CreateEmailBody()
        {
            return "Du har byttet vakten din {dateToChange} med {name}. Din nye vakt er {dateChangedTo}."
                .Replace("{name}", _nameOfOtherUser)
                .Replace("{dateToChange}", _dateToChange.ToLongDateString())
                .Replace("{dateChangedTo}", _dateChangedTo.ToLongDateString());
        }

    }
}