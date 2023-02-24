using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Tjenesteplan.Api.Configuration;
using Tjenesteplan.Api.Features.Tjenesteplaner.AddLege;
using Tjenesteplan.Api.Features.Tjenesteplaner.EditTjenesteplan;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Api.Email;
using Tjenesteplan.Domain.Repositories;
using Tjenesteplan.Domain.Tjenesteplaner;

namespace Tjenesteplan.Api.Features.Tjenesteplaner.CreateTjenesteplan
{
    public class CreateTjenesteplanAction
    {
        private readonly CommonOptions _commonOptions;
        private readonly IUserRepository _userRepository;
        private readonly IAvdelingRepository _avdelingRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly IEmailService _emailService;
        private readonly AddLegeToTjenesteplanAction _addLegeToTjenesteplanAction;

        public CreateTjenesteplanAction(
            IOptions<CommonOptions> commonOptions,
            IUserRepository userRepository,
            IAvdelingRepository avdelingRepository,
            ITjenesteplanRepository tjenesteplanRepository,
            IEmailService emailService,
            AddLegeToTjenesteplanAction addLegeToTjenesteplanAction
        )
        {
            _commonOptions = commonOptions.Value;
            _userRepository = userRepository;
            _avdelingRepository = avdelingRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
            _emailService = emailService;
            _addLegeToTjenesteplanAction = addLegeToTjenesteplanAction;
        }

        public async Task<int> ExecuteAsync(string username, CreateTjenesteplanModel model)
        {
            var user = _userRepository.GetUserByUsername(username);
            var avdeling = _avdelingRepository.GetAvdeling(model.AvdelingId);

            if (avdeling.ListeforerId != user.Id && user.Role != Role.Admin)
            {
                throw new UnauthorizedAccessException($"User with id {user.Id} is not listefører for avdeling with id {model.AvdelingId}");
            }

            var tjenesteplanId = _tjenesteplanRepository.CreateTjenesteplan(new NewTjenesteplan(
                avdelingId: model.AvdelingId,
                userId: user.Id,
                name: model.Name,
                startDate: model.StartDate,
                weeks: model.Weeks.Select(w =>
                    new NewTjenesteUke(w.Days.Select(d =>
                        new NewTjenesteDag(d.Date.Date, d.Dagsplan)
                    ).ToList())
                ).ToList()
            ));

            await AssignLegerToTjenesteplanAsync(user, model, tjenesteplanId);

            return tjenesteplanId;
        }

        private Task AssignLegerToTjenesteplanAsync(User user, CreateTjenesteplanModel model, int tjenesteplanId)
        {
            var emailTasks = new List<Task>();
            foreach (var week in model.Weeks)
            {
                if (week.LegeId != null)
                {
                    var lege = _userRepository.GetUserById(week.LegeId.Value);
                    if (lege != null)
                    {
                        _addLegeToTjenesteplanAction.Execute(user.Id, tjenesteplanId, lege.Id);

                        var emailTask = _emailService.SendAsync(new NewTjenesteplanAssignedEmail(
                            email: lege.Email,
                            url: _commonOptions.BaseUrl
                        ));

                        emailTasks.Add(emailTask);
                    }
                }
            }

            return Task.WhenAll(emailTasks);
        }

    }
}