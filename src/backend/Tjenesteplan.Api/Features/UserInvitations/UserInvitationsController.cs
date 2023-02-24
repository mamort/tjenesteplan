using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tjenesteplan.Api.Configuration;
using Tjenesteplan.Api.Exceptions;
using Tjenesteplan.Api.Features.UserInvitations.RegisterInvitation;
using Tjenesteplan.Api.Filters;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Api.Email;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.UserInvitations
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserInvitationsController : Controller
    {
        private readonly RegisterInvitationAction _registerInvitationAction;
        private readonly IUserRepository _userRepository;
        private readonly IInvitationRepository _invitationRepository;

        public UserInvitationsController(
            RegisterInvitationAction registerInvitationAction,
            IUserRepository userRepository,
            IInvitationRepository invitationRepository)
        {
            _registerInvitationAction = registerInvitationAction;
            _userRepository = userRepository;
            _invitationRepository = invitationRepository;
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public IActionResult GetInvitation(Guid id)
        {
            var invitation = _invitationRepository.GetInvitation(id);
            if (invitation == null)
            {
                return NotFound();
            }

            var user = _userRepository.GetUserByUsername(invitation.Email);

            return Ok(new InvitationModel
            {
                Id = invitation.Guid,
                Email = invitation.Email,
                Firstname = user.FirstName,
                Lastname = user.LastName,
                LegeSpesialitetId = user.Spesialitet?.Id
            });
        }

        [RoleAuthorization(Role.Admin, Role.Overlege)]
        [HttpGet]
        public IActionResult GetInvitations()
        {
            var invitations = _invitationRepository.GetInvitations();
            var model = invitations.Select(i => new InvitationModel
            {
                Id = i.Guid,
                Email = i.Email
            });

            return Ok(model);
        }

        [RoleAuthorization(Role.Admin, Role.Overlege)]
        [HttpPost("")]
        public IActionResult Register([FromBody]RegisterInvitationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var guid = _registerInvitationAction.Execute(User.Identity.Name, model);
                return Ok(new {id = guid});
            }
            catch (UserAlreadyExistsException)
            {
                return new JsonResult(new { message = "User already exists." })
                {
                    StatusCode = StatusCodes.Status409Conflict
                };
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}