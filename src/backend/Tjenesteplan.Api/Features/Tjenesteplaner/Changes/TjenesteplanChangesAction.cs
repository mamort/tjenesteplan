using System;
using System.Collections.Generic;
using System.Linq;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.Tjenesteplaner.Changes
{
    public class TjenesteplanChangesAction
    {
        private readonly IUserRepository _userRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly ITjenesteplanChangesRepository _tjenesteplanChangesRepository;

        public TjenesteplanChangesAction(
            IUserRepository userRepository,
            ITjenesteplanRepository tjenesteplanRepository,
            ITjenesteplanChangesRepository tjenesteplanChangesRepository
        )
        {
            _userRepository = userRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
            _tjenesteplanChangesRepository = tjenesteplanChangesRepository;
        }

        public IReadOnlyList<TjenesteplanUserChangeModel> Execute(string username, int tjenesteplanId)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (!_tjenesteplanRepository.IsUserListeforerForTjenesteplan(user.Id, tjenesteplanId) &&
                user.Role != Role.Admin
            )
            {
                throw new UnauthorizedAccessException();
            }

            var changes = _tjenesteplanChangesRepository.GetTjenesteplanChanges(tjenesteplanId)
                .Where(c => c.Change.VaktChangeRequestId.HasValue || c.Change.VakansvaktRequestId.HasValue);

            return changes.Select(t => new TjenesteplanUserChangeModel
            {
                Id = t.Change.Id,
                VakansvaktRequestId = t.Change.VakansvaktRequestId,
                VaktChangeRequestId = t.Change.VaktChangeRequestId,
                Date = t.Change.Date,
                User = new User 
                {
                    Id = t.UserId, 
                    Fullname = t.Fullname
                }
            }).ToList();
        }
    }
}