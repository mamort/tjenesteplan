using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Tjenesteplan.Api.Configuration;
using Tjenesteplan.Domain.Api.Email;
using Tjenesteplan.Domain.Events;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Domain.Notifications.VakansvaktRequestAcceptedNotification
{
    public class VakansvaktRequestAcceptedNotification : INotificationHandler<VakansvaktRequestAcceptedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly INotificationRepository _notificationRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly IUserRepository _userRepository;
        private readonly CommonOptions _commonOptions;

        public VakansvaktRequestAcceptedNotification(
            IOptions<CommonOptions> commonOptions,
            IEmailService emailService,
            INotificationRepository notificationRepository,
            ITjenesteplanRepository tjenesteplanRepository,
            IUserRepository userRepository
        )
        {
            _commonOptions = commonOptions.Value;
            _emailService = emailService;
            _notificationRepository = notificationRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
            _userRepository = userRepository;
        }

        public Task Handle(VakansvaktRequestAcceptedEvent vakansvaktRequestApprovedEvt, CancellationToken cancellationToken)
        {
            var tjenesteplan = _tjenesteplanRepository.GetTjenesteplanById(vakansvaktRequestApprovedEvt.TjenesteplanId);

            if (!tjenesteplan.ListeførerId.HasValue)
            {
                throw new Exception($"Tjenesteplan with id {tjenesteplan.Id} is missing listefører");
            }

            var listefører = _userRepository.GetUserById(tjenesteplan.ListeførerId.Value);

            var email = new VakansvaktRequestAcceptedEmail(
                email: listefører.Email,
                date: vakansvaktRequestApprovedEvt.Date,
                acceptorName:vakansvaktRequestApprovedEvt.Acceptor.Fullname
            );

            _emailService.Send(email);
            _notificationRepository.AddNotification(listefører.Id, email.Title, email.Body);

            return Task.CompletedTask;
        }
    }
}