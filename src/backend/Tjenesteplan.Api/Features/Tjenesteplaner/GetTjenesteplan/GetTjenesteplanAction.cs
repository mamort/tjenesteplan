using System;
using System.Linq;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.Tjenesteplaner.GetTjenesteplan
{
    public class GetTjenesteplanAction
    {
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly IUserRepository _userRepository;

        public GetTjenesteplanAction(
            ITjenesteplanRepository tjenesteplanRepository,
            IUserRepository userRepository)
        {
            _tjenesteplanRepository = tjenesteplanRepository;
            _userRepository = userRepository;
        }

        public TjenesteplanInfo Execute(string username, int tjenesteplanId)
        {
            var tjenesteplan = _tjenesteplanRepository.GetTjenesteplanById(tjenesteplanId);
            var leger = _userRepository.GetUsersByTjenesteplan(tjenesteplan.Id);

            var user = _userRepository.GetUserByUsername(username);
            if (!_tjenesteplanRepository.IsUserListeforerForTjenesteplan(user.Id, tjenesteplanId) && 
                user.Role != Role.Admin
            )
            {
                throw new UnauthorizedAccessException();
            }

            var tjenesteplanInfo = new TjenesteplanInfo
            {
                Id = tjenesteplan.Id,
                Name = tjenesteplan.Name,
                StartDate = tjenesteplan.Start,
                NumberOfWeeks = tjenesteplan.Uker.Count,
                IsFull = leger.Count >= tjenesteplan.Uker.Count,
                Weeks = tjenesteplan.Uker.Select(w => new WeekInfo()
                {
                    Id = w.Id,
                    Lege = leger.Select(l => new LegeInfo
                    {
                        Id = l.Id,
                        Firstname = l.FirstName,
                        Lastname = l.LastName,
                        Username = l.Username
                    })
                        .FirstOrDefault(l => l.Id == w.UserId),
                    Days = w.Dager.Select(d => new DayInfo
                    {
                        Date = d.Date,
                        Dagsplan = d.Dagsplan
                    }).ToList()
                }).ToList(),
                Leger = leger.Select(l => new LegeInfo
                {
                    Id = l.Id,
                    Firstname = l.FirstName,
                    Lastname = l.LastName,
                    Username = l.Username
                }).ToList()

            };

            return tjenesteplanInfo;
        }

    }
}