using System;
using System.Collections.Generic;

namespace Tjenesteplan.Domain.Repositories
{
    public interface IVaktChangeRequestRepository
    {
        int AddVaktChangeRequest(int userId, int tjenesteplanId, DateTime date, DagsplanEnum dagsplan);
        IReadOnlyList<VaktChangeRequest> GetChangeRequestsCreatedByUser(int userId, int tjenesteplanId);
        IReadOnlyList<VaktChangeRequest> GetChangeRequests(int tjenesteplanId);
        VaktChangeRequest GetRequestById(int id);
        void UpdateVaktChangeRequest(int id, int chosenVaktChangeAlternativeId, VaktChangeRequestStatus status);
        void UpdateVaktChangeRequestStatus(int id, VaktChangeRequestStatus status);
        void DeleteRequest(int requestId);
    }
}