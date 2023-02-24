using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Tjenesteplan.Api.Configuration;
using Tjenesteplan.Domain.Actions.MinTjenesteplan;
using Tjenesteplan.Domain.Api.Email;
using Tjenesteplan.Domain.Events;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Domain.Notifications.VakansvaktRequestApprovedNotification
{
    public class VakansvaktRequestApprovedNotification : INotificationHandler<VakansvaktRequestApprovedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly INotificationRepository _notificationRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly IUserRepository _userRepository;
        private readonly CurrentTjenesteplanAction _currentTjenesteplanAction;
        private readonly CommonOptions _commonOptions;

        public VakansvaktRequestApprovedNotification(
            IOptions<CommonOptions> commonOptions,
            IEmailService emailService,
            INotificationRepository notificationRepository,
            ITjenesteplanRepository tjenesteplanRepository,
            IUserRepository userRepository,
            CurrentTjenesteplanAction currentTjenesteplanAction
        )
        {
            _commonOptions = commonOptions.Value;
            _emailService = emailService;
            _notificationRepository = notificationRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
            _userRepository = userRepository;
            _currentTjenesteplanAction = currentTjenesteplanAction;
        }

        public Task Handle(VakansvaktRequestApprovedEvent vakansvaktRequestApprovedEvt, CancellationToken cancellationToken)
        {
            var tjenesteplan = _tjenesteplanRepository.GetTjenesteplanById(vakansvaktRequestApprovedEvt.TjenesteplanId);

            if (!tjenesteplan.ListeførerId.HasValue)
            {
                throw new Exception($"Tjenesteplan with id {tjenesteplan.Id} is missing listefører");
            }

            NotifyRequestorOfApprovedVakansvaktRequest(vakansvaktRequestApprovedEvt);
            NotifyOtherDoctorsOfNewVakansvakt(vakansvaktRequestApprovedEvt);


            return Task.CompletedTask;
        }

        private void NotifyRequestorOfApprovedVakansvaktRequest(VakansvaktRequestApprovedEvent vakansvaktRequestApprovedEvt)
        {
            var email = new VakansvaktRequestApprovedEmail(
                email: vakansvaktRequestApprovedEvt.Requestor.Email,
                date: vakansvaktRequestApprovedEvt.Date
            );

            _emailService.Send(email);
            _notificationRepository.AddNotification(vakansvaktRequestApprovedEvt.Requestor.Id, email.Title, email.Body);
        }

        private void NotifyOtherDoctorsOfNewVakansvakt(VakansvaktRequestApprovedEvent vakansvaktRequestApprovedEvt)
        {
            var requestDate = vakansvaktRequestApprovedEvt.Date;
            // If date is in the past or is more than one week from now => dont send notifications to other users
            if (requestDate < DateTime.UtcNow || requestDate >= DateTime.UtcNow.AddDays(7)) return;

            // Don't include the user that sent the request
            var users = _userRepository.GetUsersByTjenesteplan(vakansvaktRequestApprovedEvt.TjenesteplanId)
                .Where(u => u.Id != vakansvaktRequestApprovedEvt.Requestor.Id);

            foreach (var user in users)
            {
                var currentTjenesteplan = _currentTjenesteplanAction.Execute(user.Id, vakansvaktRequestApprovedEvt.TjenesteplanId);
                if (currentTjenesteplan.IsDateAvailableForVakt(vakansvaktRequestApprovedEvt.Date, vakansvaktRequestApprovedEvt.Dagsplan))
                {
                    var email = new VakansvaktAvailableEmail(
                        email: user.Email,
                        date: vakansvaktRequestApprovedEvt.Date,
                        url: $"{_commonOptions.BaseUrl}/vakansvakter/{vakansvaktRequestApprovedEvt.VakansvaktRequestId}"
                    );

                    _emailService.Send(email);
                    _notificationRepository.AddNotification(user.Id, email.Title, email.Body);
                }
            }
        }

    }
}