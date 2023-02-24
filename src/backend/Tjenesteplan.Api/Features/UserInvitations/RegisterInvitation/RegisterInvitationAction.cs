using System;
using System.Transactions;
using Microsoft.Extensions.Options;
using Tjenesteplan.Api.Configuration;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Api.Email;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.UserInvitations.RegisterInvitation
{
    public class RegisterInvitationAction
    {
        private readonly IUserRepository _userRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IEmailService _emailService;
        private readonly CommonOptions _commonOptions;

        public RegisterInvitationAction(
            IOptions<CommonOptions> commonOptions,
            IUserRepository userRepository,
            IInvitationRepository invitationRepository,
            IEmailService emailService
        )
        {
            _commonOptions = commonOptions.Value;
            _userRepository = userRepository;
            _invitationRepository = invitationRepository;
            _emailService = emailService;
        }

        public Guid Execute(string username, RegisterInvitationModel model)
        {
            var email = model.Email.ToLower().Trim();
            var invitation = _invitationRepository.GetInvitationByEmail(email);
            var user = _userRepository.GetUserByUsername(username);

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            var invitedUser = _userRepository.GetUserByUsername(email);
            if (invitedUser != null)
            {
                throw new UserAlreadyExistsException();
            }

            using (var transaction = _userRepository.BeginTransaction())
            {
                var userId = _userRepository.CreateUser(
                    username: email,
                    firstName: model.Firstname,
                    lastName: model.Lastname,
                    passwordHash: null,
                    passwordSalt: null,
                    role: Role.Lege
                );

                _userRepository.AddUserToAvdeling(userId, model.AvdelingId);

                var guid = invitation?.Guid ?? _invitationRepository.AddInvitation(model.AvdelingId, email);

                _emailService.Send(
                    new InvitationEmail(
                        email: email,
                        url: $"{_commonOptions.BaseUrl}/register/{guid}"
                    )
                );

                transaction.Commit();

                return guid;
            }
        }
    }
}