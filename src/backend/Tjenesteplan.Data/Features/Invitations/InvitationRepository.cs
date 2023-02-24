using System;
using System.Collections.Generic;
using System.Linq;
using Tjenesteplan.Data.Contexts;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Data.Features.Invitations
{
    public class InvitationRepository : IInvitationRepository
    {
        private readonly DataContext _context;

        public InvitationRepository(DataContext context)
        {
            _context = context;
        }

        public Guid AddInvitation(int avdelingId, string email)
        {
            var guid = Guid.NewGuid();
            _context.Invitations.Add(new InvitationEntity
            {
                Guid = guid,
                AvdelingId = avdelingId,
                Email = email,
                Role = Role.Lege
            });

            _context.SaveChanges();

            return guid;
        }

        public IReadOnlyList<Invitation> GetInvitations()
        {
            return _context.Invitations
                .Select(i => new Invitation(i.Guid, i.Email, i.Role))
                .ToList();
        }

        public Invitation GetInvitation(Guid guid)
        {
            var invitation =  _context.Invitations.FirstOrDefault(i => i.Guid == guid);
            if (invitation == null)
            {
                return null;
            }

            return new Invitation(invitation.Guid, invitation.Email, invitation.Role);
        }

        public Invitation GetInvitationByEmail(string email)
        {
            var invitation = _context.Invitations.FirstOrDefault(i => i.Email == email);
            if (invitation == null)
            {
                return null;
            }

            return new Invitation(invitation.Guid, invitation.Email, invitation.Role);
        }

        public void DeleteInvitation(Guid invitationId)
        {
            var invitation = _context.Invitations.FirstOrDefault(i => i.Guid == invitationId);
            if (invitation != null)
            {
                _context.Invitations.Remove(invitation);
                _context.SaveChanges();
            }
        }
    }
}