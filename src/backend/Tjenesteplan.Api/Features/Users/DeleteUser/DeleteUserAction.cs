using Tjenesteplan.Domain.Repositories;
using WebApi.Features.Users.Data;

namespace Tjenesteplan.Api.Features.Users.DeleteUser
{
    public class DeleteUserAction
    {
        private readonly IUserRepository _userRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly IVaktChangeRequestRepository _vaktChangeRequestRepository;
        private readonly IVaktChangeRequestRepliesRepository _vaktChangeRequestRepliesRepository;

        public DeleteUserAction(
            IUserRepository userRepository,
            ITjenesteplanRepository tjenesteplanRepository,
            IVaktChangeRequestRepository vaktChangeRequestRepository,
            IVaktChangeRequestRepliesRepository vaktChangeRequestRepliesRepository
        )
        {
            _userRepository = userRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
            _vaktChangeRequestRepository = vaktChangeRequestRepository;
            _vaktChangeRequestRepliesRepository = vaktChangeRequestRepliesRepository;
        }

        public void Delete(int id)
        {
            var tjenesteplaner = _tjenesteplanRepository.GetTjenesteplanerForUser(id);
            _tjenesteplanRepository.RemoveUserFromTjenesteplaner(id);

            foreach (var tjenesteplan in tjenesteplaner)
            {
                var requests = _vaktChangeRequestRepository.GetChangeRequestsCreatedByUser(id, tjenesteplan.Id);

                foreach (var request in requests)
                {
                    _vaktChangeRequestRepliesRepository.DeleteVaktChangeReplies(request.Id);
                    _vaktChangeRequestRepository.DeleteRequest(request.Id);
                }
            }

            _vaktChangeRequestRepliesRepository.DeleteVaktChangeRepliesForUser(id);

            // Her mangler det litt. Vi har slettet forespørslene om vaktbytte opprettet av brukeren og svarene han fikk
            // Men ikke svarene han har gitt til andre

            _userRepository.DeleteUser(id);
        }

    }
}