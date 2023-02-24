using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Tjenesteplan.Api.Configuration;
using Tjenesteplan.Api.Services.Email;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Api;
using Tjenesteplan.Domain.Api.Email;
using Tjenesteplan.Domain.Repositories;
using Tjenesteplan.Domain.Tjenesteplaner;

namespace Tjenesteplan.Api.Features.Tjenesteplaner.EditTjenesteplan
{
    public class EditTjenesteplanAction
    {
        private readonly IUserRepository _userRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly IEmailService _emailService;
        private CommonOptions _commonOptions;

        public EditTjenesteplanAction(
            IOptions<CommonOptions> commonOptions,
            IUserRepository userRepository,
            ITjenesteplanRepository tjenesteplanRepository,
            IEmailService emailService)
        {
            _commonOptions = commonOptions.Value;
            _userRepository = userRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
            _emailService = emailService;
        }

        public void Execute(string username, EditTjenesteplanModel model)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (!_tjenesteplanRepository.IsUserListeforerForTjenesteplan(user.Id, model.Id) && 
                user.Role != Role.Admin
            )
            {
                throw new UnauthorizedAccessException("Cannot edit a tjenesteplan you have not created");
            }
            
            var legerInTjenesteplan = _userRepository.GetUsersByTjenesteplan(model.Id);
            _tjenesteplanRepository.EditTjenesteplan(new Domain.Tjenesteplaner.EditTjenesteplan(
                id: model.Id,
                userId: user.Id,
                name: model.Name,
                startDate: model.StartDate,
                weeks: model.Weeks.Select(w =>
                    new EditTjenesteUke(w.Id, w.LegeId, w.Days.Select(d =>
                        new EditTjenesteDag(d.Date.Date, d.Dagsplan)
                    ).ToList())
                ).ToList()
            ));

            AssignLegerToTjenesteplan(model);
            RemoveLegerNoLongerInTjenesteplan(model, legerInTjenesteplan);
        }

        private void AssignLegerToTjenesteplan(EditTjenesteplanModel model)
        {
            foreach (var week in model.Weeks)
            {
                if (week.LegeId != null)
                {
                    var tjenesteplaner = _tjenesteplanRepository.GetTjenesteplanerForUser(week.LegeId.Value);
                    if (!tjenesteplaner.Any(t => t.Id != model.Id))
                    {
                        var lege = _userRepository.GetUserById(week.LegeId.Value);
                        _emailService.Send(new NewTjenesteplanAssignedEmail(
                            email: lege.Email,
                            url: _commonOptions.BaseUrl
                        ));
                    }

                    // If lege not assigned to Tjenesteplan
                    if (!tjenesteplaner.Any(t => t.Id == model.Id))
                    {
                        _userRepository.AssignLegeToTjenesteplan(week.LegeId.Value, model.Id);
                    }
                }
            }
        }

        private void RemoveLegerNoLongerInTjenesteplan(EditTjenesteplanModel model, IReadOnlyList<User> legerInTjenesteplan)
        {
            var legerNoLongerInTjenesteplan = legerInTjenesteplan.Where(l => !model.Weeks.Any(w => w.LegeId == l.Id)).ToList();
            foreach (var lege in legerNoLongerInTjenesteplan)
            {
                _userRepository.RemoveLegeFromTjenesteplan(model.Id, lege.Id);
            }
        }
    }
}