using System;
using System.Linq;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.Tjenesteplaner.AddLege
{
    public class AddLegeToTjenesteplanAction
    {
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly IUserRepository _userRepository;

        public AddLegeToTjenesteplanAction(
                ITjenesteplanRepository tjenesteplanRepository,
                IUserRepository userRepository
            )
        {
            _tjenesteplanRepository = tjenesteplanRepository;
            _userRepository = userRepository;
        }
        public void Execute(string username, int tjenesteplanId, AddLegeModel model)
        {
            var loggedInUser = _userRepository.GetUserByUsername(username);
            var lege = _userRepository.GetUserById(model.LegeId);
            if (lege == null)
            {
                throw new Exception($"Could not find user with id {model.LegeId}");
            }
            AddLegeToTjenesteplan(tjenesteplanId, model.LegeId, loggedInUser);
        }

        public void Execute(int loggedInUserId, int tjenesteplanId, int legeId)
        {
            var loggedInUser = _userRepository.GetUserById(loggedInUserId);
            AddLegeToTjenesteplan(tjenesteplanId, legeId, loggedInUser);
        }

        private void AddLegeToTjenesteplan(int tjenesteplanId, int legeId, User loggedInUser)
        {
            var tjenesteplan = _tjenesteplanRepository.GetTjenesteplanById(tjenesteplanId);
            if (tjenesteplan == null)
            {
                throw new Exception($"Could not find tjenesteplan with id {tjenesteplanId}");
            }
            
            if (!_tjenesteplanRepository.IsUserListeforerForTjenesteplan(loggedInUser.Id, tjenesteplanId) && 
                loggedInUser.Role != Role.Admin)
            {
                throw new UnauthorizedAccessException("Cannot add lege to a tjenesteplan you have not created");
            }

            var week = tjenesteplan.Uker.FirstOrDefault(w => w.UserId == null);

            if (week == null)
            {
                throw new Exception($"Could not available week for user with id {legeId} " +
                                    $"in tjenesteplan with id {tjenesteplanId}");
            }

            _tjenesteplanRepository.AssignLegeToWeek(tjenesteplanId, week.Id, legeId);
            _userRepository.AssignLegeToTjenesteplan(legeId, tjenesteplanId);
        }
    }
}