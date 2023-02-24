using System;
using System.Collections.Generic;
using System.Linq;
using Tjenesteplan.Api.Exceptions;
using Tjenesteplan.Api.Services.PasswordHash;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;
using WebApi.Features.Users.RegisterUser;

namespace Tjenesteplan.Api.Features.Users.RegisterUser
{
    public class RegisterUserAction
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashService _passwordHashService;

        public RegisterUserAction(
            IInvitationRepository invitationRepository,
            IUserRepository userRepository,
            IPasswordHashService passwordHashService)
        {
            _invitationRepository = invitationRepository;
            _userRepository = userRepository;
            _passwordHashService = passwordHashService;
        }

        public int Execute(Guid invitationId, NewUser newUser)
        {
            var invitation = _invitationRepository.GetInvitation(invitationId);
            if (invitation == null)
            {
                throw new AppException("An invitation with this id does not exist.");
            }

            var invitedUser = new NewUser(
                firstName: newUser.FirstName,
                lastName: newUser.LastName,
                username: invitation.Email,
                legeSpesialitet: newUser.LegeSpesialitet,
                password: newUser.Password,
                role: invitation.Role
            );

            var userId = Execute(invitedUser);

            _invitationRepository.DeleteInvitation(invitationId);

            return userId;
        }

        public int Execute(NewUser newUser)
        {
            if (string.IsNullOrWhiteSpace(newUser.Password))
            {
                throw new AppException("Password is required");
            }

            var hashSalt = _passwordHashService.CreatePasswordHash(newUser.Password);

            var existingUser = _userRepository.GetUserByUsername(newUser.Username);
            if (existingUser != null)
            {
                _userRepository.UpdateUser(
                    id: existingUser.Id,
                    firstname: newUser.FirstName,
                    lastname: newUser.LastName,
                    spesialitet: newUser.LegeSpesialitet,
                    username: existingUser.Username,
                    passwordHash: hashSalt.PasswordHash,
                    passwordSalt: hashSalt.PasswordSalt
                );
            }

            return existingUser.Id;
        }
        
    }
}