using System;
using System.Linq;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Actions.WeeklyTjenesteplan;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.Tjenesteplaner.GetWeeklyTjenesteplan
{
    public class GetWeeklyTjenesteplanAction
    {
        private readonly IUserRepository _userRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly WeeklyTjenesteplanAction _weeklyTjenesteplanAction;

        public GetWeeklyTjenesteplanAction(
            IUserRepository userRepository,
            ITjenesteplanRepository tjenesteplanRepository,
            WeeklyTjenesteplanAction weeklyTjenesteplanAction
        )
        {
            _userRepository = userRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
            _weeklyTjenesteplanAction = weeklyTjenesteplanAction;
        }

        public WeeklyTjenesteplan Execute(string username, int tjenesteplanId)
        {
            var user = _userRepository.GetUserByUsername(username);
            var tjenesteplan = _tjenesteplanRepository.GetTjenesteplanById(tjenesteplanId);

            if(tjenesteplan == null)
            {
                return null;
            }

            if (user.Role != Role.Admin && !_tjenesteplanRepository.IsUserListeforerForTjenesteplan(user.Id, tjenesteplanId))
            {
                throw new UnauthorizedAccessException(
                    $"Bruker {user.Id} does not have access to tjenesteplan with id {tjenesteplanId}"
                );
            }

            return _weeklyTjenesteplanAction.GetWeeklyTjenesteplan(tjenesteplanId);
        }

    }
}
