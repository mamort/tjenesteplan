using System;
using System.Linq;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.Tjenesteplaner.ChangeTjenesteplanDateForLege
{
    public class ChangeTjenesteplanDateForLegeAction
    {
        private readonly IUserRepository _userRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly ITjenesteplanChangesRepository _tjenesteplanChangesRepository;

        public ChangeTjenesteplanDateForLegeAction(
            IUserRepository userRepository,
            ITjenesteplanRepository tjenesteplanRepository,
            ITjenesteplanChangesRepository tjenesteplanChangesRepository
        )
        {
            _userRepository = userRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
            _tjenesteplanChangesRepository = tjenesteplanChangesRepository;
        }

        public void Execute(
            string username,
            int tjenesteplanId, 
            int userId, 
            DateTime date, 
            DagsplanEnum dagsplan
        )
        {
            var user = _userRepository.GetUserByUsername(username);

            if (!_tjenesteplanRepository.IsUserListeforerForTjenesteplan(user.Id, tjenesteplanId) && 
                user.Role != Role.Admin)
            {
                throw new UnauthorizedAccessException();
            }

            _tjenesteplanChangesRepository.AddTjenesteplanChange(
                tjenesteplanId: tjenesteplanId,
                userId: userId,
                date: date,
                dagsplan: dagsplan
            );
        }
    }
}