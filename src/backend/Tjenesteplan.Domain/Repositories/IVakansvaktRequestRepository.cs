using System;
using System.Collections.Generic;

namespace Tjenesteplan.Domain.Repositories
{
    public interface IVakansvaktRequestRepository
    {
        int AddVakansvaktRequest(
            int userId, 
            int tjenesteplanId, 
            DateTime date, 
            DagsplanEnum currentDagsplan,
            DagsplanEnum requestedDagsplan,
            string reason
        );

        IReadOnlyList<VakansvaktRequest> GetVakansvaktRequests(int tjenesteplanId, int userId);
        IReadOnlyList<VakansvaktRequest> GetVakansvaktRequests(int tjenesteplanId, VakansvaktRequestStatus status);
        VakansvaktRequest GetVakansvaktRequest(int vakansvaktRequestId);
        void ChangeVakansvaktRequestStatus(int vakansvaktRequestId, VakansvaktRequestStatus status);
        void AcceptVakansvakt(int vakansvaktRequestId, int userId);
        IReadOnlyList<VakansvaktRequest> GetAvailableVakansvaktRequests(int tjenesteplanId);
    }
}