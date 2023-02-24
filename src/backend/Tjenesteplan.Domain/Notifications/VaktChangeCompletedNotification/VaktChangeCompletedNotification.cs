using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tjenesteplan.Domain.Api.Email;
using Tjenesteplan.Domain.Events;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Domain.Notifications.VaktChangeCompletedNotification
{
    public class VaktChangeCompletedNotification : INotificationHandler<VaktChangeCompletedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly INotificationRepository _notificationRepository;

        public VaktChangeCompletedNotification(
            IEmailService emailService,
            INotificationRepository notificationRepository
        )
        {
            _emailService = emailService;
            _notificationRepository = notificationRepository;
        }

        public Task Handle(VaktChangeCompletedEvent notification, CancellationToken cancellationToken)
        {
            NotifyRequestor(notification);
            NotifyRespondent(notification);

            return Task.CompletedTask;
        }

        private void NotifyRequestor(VaktChangeCompletedEvent vaktChangeCompleted)
        {
            var email = new VaktChangeCompletedEmail(
                email: vaktChangeCompleted.Requestor.Email,
                nameOfOtherUser: vaktChangeCompleted.Respondent.Fullname,
                dateToChange: vaktChangeCompleted.RequestDate,
                dateChangedTo: vaktChangeCompleted.ChangeDate
            );

            _emailService.Send(email);
            _notificationRepository.AddNotification(vaktChangeCompleted.Requestor.Id, email.Title, email.Body);

        }

        private void NotifyRespondent(VaktChangeCompletedEvent vaktChangeCompleted)
        {
            var email = new VaktChangeCompletedEmail(
                email: vaktChangeCompleted.Respondent.Email,
                nameOfOtherUser: vaktChangeCompleted.Requestor.Fullname,
                dateToChange: vaktChangeCompleted.ChangeDate,
                dateChangedTo: vaktChangeCompleted.RequestDate.Date
            );

            _emailService.Send(email);
            _notificationRepository.AddNotification(vaktChangeCompleted.Respondent.Id, email.Title, email.Body);
        }

    }
}