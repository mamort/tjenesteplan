using System;
using MediatR;

namespace Tjenesteplan.Domain.Events
{
    public class VaktChangeCompletedEvent : INotification
    {
        public User Requestor { get; }
        public User Respondent { get; }
        public DateTime RequestDate { get; }
        public DateTime ChangeDate { get; }

        public VaktChangeCompletedEvent(
            User requestor,
            User respondent,
            DateTime requestDate, 
            DateTime changeDate
        )
        {
            Requestor = requestor;
            Respondent = respondent;
            RequestDate = requestDate;
            ChangeDate = changeDate;
        }
    }
}