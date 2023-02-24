using System;
using System.Collections.Generic;

namespace Tjenesteplan.Domain.Repositories
{
    public interface ITjenesteplanChangesRepository
    {
        IReadOnlyList<TjenesteplanChange> GetTjenesteplanChanges(int tjenesteplanId, int userId);
        IReadOnlyList<TjenesteplanUserChange> GetTjenesteplanChanges(int tjenesteplanId);
        int AddTjenesteplanChange(int userId, int tjenesteplanId, DateTime date, DagsplanEnum dagsplan);
        int AddTjenesteplanVakansvaktChange(int userId, int tjenesteplanId, int vakansvaktRequestId, DateTime date, DagsplanEnum dagsplan);
        int AddTjenesteplanChange(int userId, int tjenesteplanId, int vaktChangeRequestId, DateTime date, DagsplanEnum dagsplan);
        void UndoTjenesteplanVakansvaktChange(int tjenesteplanId, int vakansvaktRequestId);
        void UndoTjenesteplanVaktChange(int tjenesteplanId, int vaktChangeRequestId);
    }
}