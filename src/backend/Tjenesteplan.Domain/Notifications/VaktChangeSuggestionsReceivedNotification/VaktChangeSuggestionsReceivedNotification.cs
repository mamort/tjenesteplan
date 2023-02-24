using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Tjenesteplan.Api.Configuration;
using Tjenesteplan.Domain.Api.Email;
using Tjenesteplan.Domain.Events;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Domain.Notifications.VaktChangeSuggestionsReceivedNotification
{
    public class VaktChangeSuggestionsReceivedNotification : INotificationHandler<VaktChangeSuggestionsReceivedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly INotificationRepository _notificationRepository;
        private readonly CommonOptions _commonOptions;

        public VaktChangeSuggestionsReceivedNotification(
            IOptions<CommonOptions> commonOptions,
            IEmailService emailService,
            INotificationRepository notificationRepository
        )
        {
            _commonOptions = commonOptions.Value;
            _emailService = emailService;
            _notificationRepository = notificationRepository;
        }

        public Task Handle(VaktChangeSuggestionsReceivedEvent vaktChangeSuggestionsReceived, CancellationToken cancellationToken)
        {
            var email = new VaktChangeSuggestionsAvailableEmail(
                email: vaktChangeSuggestionsReceived.VaktChangeRequestor.Email,
                respondentName: vaktChangeSuggestionsReceived.VaktChangeRespondent.Fullname,
                date: vaktChangeSuggestionsReceived.Date,
                url: $"{_commonOptions.BaseUrl}/minetjenesteplaner/{vaktChangeSuggestionsReceived.TjenesteplanId}/vaktbytter/{vaktChangeSuggestionsReceived.VaktChangeRequestId}/aksepter"
            );

            _emailService.Send(email);
            _notificationRepository.AddNotification(
                vaktChangeSuggestionsReceived.VaktChangeRequestor.Id, 
                email.Title, 
                email.Body
            );
            return Task.CompletedTask;
        }
    }
}