using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tjenesteplan.Api.Configuration;
using Tjenesteplan.Domain.Actions.MinTjenesteplan;
using Tjenesteplan.Domain.Api.Email;
using Tjenesteplan.Domain.Repositories;
using Tjenesteplan.Events;

namespace Tjenesteplan.Domain.Notifications.VakansvaktReminderNotification
{
    public class VakansvaktRemindersNotification : INotificationHandler<VakansvaktScheduledSoonEvent>
    {
        private readonly ILogger<VakansvaktRemindersNotification> _logger;
        private readonly IEmailService _emailService;
        private readonly INotificationRepository _notificationRepository;
        private readonly IVakansvaktRequestRepository _vakansvaktRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly CurrentTjenesteplanAction _currentTjenesteplanAction;
        private readonly CommonOptions _commonOptions;

        public VakansvaktRemindersNotification(
            ILogger<VakansvaktRemindersNotification> logger,
            IOptions<CommonOptions> commonOptions,
            IEmailService emailService,
            INotificationRepository notificationRepository,
            IVakansvaktRequestRepository vakansvaktRequestRepository,
            IUserRepository userRepository,
            CurrentTjenesteplanAction currentTjenesteplanAction
        )
        {
            _commonOptions = commonOptions.Value;
            _logger = logger;
            _emailService = emailService;
            _notificationRepository = notificationRepository;
            _vakansvaktRequestRepository = vakansvaktRequestRepository;
            _userRepository = userRepository;
            _currentTjenesteplanAction = currentTjenesteplanAction;
        }

        public Task Handle(VakansvaktScheduledSoonEvent evt, CancellationToken cancellationToken)
        {
            NotifyOtherDoctorsOfVakansvakt(evt);
            return Task.CompletedTask;
        }


        private void NotifyOtherDoctorsOfVakansvakt(VakansvaktScheduledSoonEvent evt)
        {
            var requestDate = evt.Date;
            if (requestDate < DateTime.UtcNow)
            {
                // This should not happen for this reminder
                _logger.LogError($"Vakansvakt reminder notification was not " +
                                 $"sent because request date {requestDate.ToShortDateString()} is in the past. " +
                                 $"This should not happen.");
                return;
            };

            var request = _vakansvaktRequestRepository.GetVakansvaktRequest(evt.VakansvaktRequestId);

            if (request.Status == VakansvaktRequestStatus.Cancelled ||
                request.Status == VakansvaktRequestStatus.Rejected)
            {
                _logger.LogInformation("Did not send vakansvakt reminder for " +
                                       $"request {request.Id} because it was cancelled or rejected.");
            }

            if (request.Status == VakansvaktRequestStatus.Accepted)
            {
                _logger.LogInformation("Did not send vakansvakt reminder for " +
                                       $"request {request.Id} because it was already accepted.");
            }

            // Don't include the user that sent the request
            var users = _userRepository.GetUsersByTjenesteplan(evt.TjenesteplanId)
                .Where(u => u.Id != request.RequestedByUserId);

            foreach (var user in users)
            {
                var currentTjenesteplan = _currentTjenesteplanAction.Execute(user.Id, evt.TjenesteplanId);
                if (currentTjenesteplan.IsDateAvailableForVakt(evt.Date, request.CurrentDagsplan))
                {
                    var email = new VakansvaktReminderEmail(
                        email: user.Email,
                        date: evt.Date,
                        url: $"{_commonOptions.BaseUrl}/vakansvakter/{evt.VakansvaktRequestId}"
                    );

                    _emailService.Send(email);
                    _notificationRepository.AddNotification(user.Id, email.Title, email.Body);
                }
            }
        }
    }
}