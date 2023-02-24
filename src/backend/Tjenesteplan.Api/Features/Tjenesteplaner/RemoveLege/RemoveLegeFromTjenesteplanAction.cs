using System;
using System.Linq;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.Tjenesteplaner.RemoveLege
{
    public class RemoveLegeFromTjenesteplanAction
    {
        private readonly IUserRepository _userRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;

        public RemoveLegeFromTjenesteplanAction(
            IUserRepository userRepository,
            ITjenesteplanRepository tjenesteplanRepository)
        {
            _userRepository = userRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
        }

        public void Execute(string username, int tjenesteplanId, int legeId)
        {
            var loggedInUser = _userRepository.GetUserByUsername(username);
            if (!_tjenesteplanRepository.IsUserListeforerForTjenesteplan(loggedInUser.Id, tjenesteplanId) && 
                loggedInUser.Role != Role.Admin
            )
            {
                throw new UnauthorizedAccessException("Cannot add lege to a tjenesteplan you have not created");
            }

            _userRepository.RemoveLegeFromTjenesteplan(tjenesteplanId, legeId);
            _tjenesteplanRepository.RemoveLegeFromTjenesteplanWeeK(tjenesteplanId, legeId);
        }
    }
}