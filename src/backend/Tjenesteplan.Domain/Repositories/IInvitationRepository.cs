using System;
using System.Collections.Generic;

namespace Tjenesteplan.Domain.Repositories
{
    public interface IInvitationRepository
    {
        Invitation GetInvitation(Guid guid);
        IReadOnlyList<Invitation> GetInvitations();
        Guid AddInvitation(int avdelingId, string email);
        Invitation GetInvitationByEmail(string modelEmail);
        void DeleteInvitation(Guid invitationId);
    }
}