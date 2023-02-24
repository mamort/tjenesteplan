using System;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.Users.RemoveLegeFromAvdeling
{
    public class RemoveLegeFromAvdelingAction
    {
        private readonly IUserRepository _userRepository;
        private readonly IAvdelingRepository _avdelingRepository;

        public RemoveLegeFromAvdelingAction(
            IUserRepository userRepository, 
            IAvdelingRepository avdelingRepository
        )
        {
            _userRepository = userRepository;
            _avdelingRepository = avdelingRepository;
        }

        public void Execute(string username, int avdelingId, int legeId)
        {
            var user = _userRepository.GetUserByUsername(username);
            var avdeling = _avdelingRepository.GetAvdeling(avdelingId);

            if (user.Id != avdeling.ListeforerId && user.Role != Role.Admin)
            {
                throw new UnauthorizedAccessException();
            }

            _userRepository.RemoveUserFromAvdeling(avdelingId, legeId);
        }
    }
}