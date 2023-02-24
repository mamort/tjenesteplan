using System.Collections.Generic;
using System.Linq;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.MinTjenesteplan.GetMineTjenesteplaner
{
    public class GetMineTjenesteplanerAction
    {
        private readonly IUserRepository _userRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;

        public GetMineTjenesteplanerAction(
            IUserRepository userRepository,
            ITjenesteplanRepository tjenesteplanRepository
        )
        {
            _userRepository = userRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
        }

        public IReadOnlyList<MinTjenesteplanModel> Execute(string username)
        {
            var user = _userRepository.GetUserByUsername(username);
            var tjenesteplaner = _tjenesteplanRepository.GetTjenesteplanerForUser(user.Id);

            return tjenesteplaner
                .Select(t => new MinTjenesteplanModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    AvdelingId = t.AvdelingId
                }).ToList();
        }
        
    }
}