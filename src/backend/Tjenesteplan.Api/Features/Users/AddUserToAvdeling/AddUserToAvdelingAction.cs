using System;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.Users.AddUserToAvdeling
{
    public class AddUserToAvdelingAction
    {
        private readonly IAvdelingRepository _avdelingRepository;
        private readonly IUserRepository _userRepository;

        public AddUserToAvdelingAction(IAvdelingRepository avdelingRepository, IUserRepository userRepository)
        {
            _avdelingRepository = avdelingRepository;
            _userRepository = userRepository;
        }

        public void Execute(string username, int avdelingId, int userId)
        {
            var user = _userRepository.GetUserByUsername(username);
            var avdeling = _avdelingRepository.GetAvdeling(avdelingId);

            if (user.Id != avdeling.ListeforerId && user.Role != Role.Admin)
            {
                throw new UnauthorizedAccessException();
            }

            _userRepository.AddUserToAvdeling(userId: userId, avdelingId: avdelingId);
        }
    }
}