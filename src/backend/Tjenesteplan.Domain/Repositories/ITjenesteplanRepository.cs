using System.Collections.Generic;
using Tjenesteplan.Domain.Tjenesteplaner;

namespace Tjenesteplan.Domain.Repositories
{
    public interface ITjenesteplanRepository
    {
        int CreateTjenesteplan(NewTjenesteplan tjenesteplan);
        IReadOnlyList<Tjenesteplan> GetTjenesteplanerCreatedByUser(string username);
        Tjenesteplan GetTjenesteplanById(int id);
        void AssignLegeToWeek(int tjenesteplanId, int weekId, int userId);
        void RemoveLegeFromTjenesteplanWeeK(int tjenesteplanId, int legeId);
        void EditTjenesteplan(EditTjenesteplan editTjenesteplan);
        void RemoveUserFromTjenesteplaner(int userId);
        bool IsUserListeforerForTjenesteplan(int userId, int tjenesteplanId);
        IReadOnlyList<Domain.Tjenesteplan> GetTjenesteplanerForUser(int userId);
    }
}