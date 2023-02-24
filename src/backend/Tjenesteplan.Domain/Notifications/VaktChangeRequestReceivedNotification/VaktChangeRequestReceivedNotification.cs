using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Tjenesteplan.Api.Configuration;
using Tjenesteplan.Domain.Api.Email;
using Tjenesteplan.Domain.Events;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Domain.Notifications.VaktChangeRequestReceivedNotification
{
    public class VaktChangeRequestReceivedNotification : INotificationHandler<VaktChangeRequestReceivedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly INotificationRepository _notificationRepository;
        private readonly CommonOptions _commonOptions;

        public VaktChangeRequestReceivedNotification(
            IOptions<CommonOptions> commonOptions,
            IEmailService emailService,
            INotificationRepository notificationRepository
        )
        {
            _commonOptions = commonOptions.Value;
            _emailService = emailService;
            _notificationRepository = notificationRepository;
        }

        public Task Handle(VaktChangeRequestReceivedEvent vaktChangeRequestReceived, CancellationToken cancellationToken)
        {
            var email = new VaktChangeRequestEmail(
                email: vaktChangeRequestReceived.Receiver.Email,
                initiatorName: vaktChangeRequestReceived.Requestor.Fullname,
                date: vaktChangeRequestReceived.Date,
                url: $"{_commonOptions.BaseUrl}/minetjenesteplaner/{vaktChangeRequestReceived.TjenesteplanId}/vaktbytter/{vaktChangeRequestReceived.VaktChangeRequestId}"
            );

            _emailService.Send(email);
            _notificationRepository.AddNotification(vaktChangeRequestReceived.Receiver.Id, email.Title, email.Body);

            return Task.CompletedTask;
        }
    }
}