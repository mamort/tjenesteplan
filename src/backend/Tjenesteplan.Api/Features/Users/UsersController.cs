using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tjenesteplan.Api.Exceptions;
using Tjenesteplan.Api.Features.Users.AddUserToAvdeling;
using Tjenesteplan.Api.Features.Users.AutheticatedUser;
using Tjenesteplan.Api.Features.Users.DeleteUser;
using Tjenesteplan.Api.Features.Users.NewPassword;
using Tjenesteplan.Api.Features.Users.RegisterUser;
using Tjenesteplan.Api.Features.Users.RemoveLegeFromAvdeling;
using Tjenesteplan.Api.Features.Users.ResetPassword;
using Tjenesteplan.Api.Features.Users.UpdateUser;
using Tjenesteplan.Api.Features.Users.ViewUser;
using Tjenesteplan.Api.Filters;
using Tjenesteplan.Domain;
using WebApi.Features.Users.RegisterUser;
using WebApi.Features.Users.UpdateUser;

namespace Tjenesteplan.Api.Features.Users
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly AuthenticateUserAction _authenticateAction;
        private readonly RegisterUserAction _registerUserAction;
        private readonly UpdateUserAction _updateUserAction;
        private readonly DeleteUserAction _deleteUserAction;
        private readonly GetAllUsersAction _getAllUsersAction;
        private readonly GetLegerAction _getLegerAction;
        private readonly GetUserAction _getUserAction;
        private readonly ResetPasswordAction _resetPasswordAction;
        private readonly NewPasswordAction _newPasswordAction;
        private readonly RemoveLegeFromAvdelingAction _removeLegeFromAvdelingAction;
        private readonly AddUserToAvdelingAction _addUserToAvdelingAction;

        public UsersController(
            AuthenticateUserAction authenticateAction,
            RegisterUserAction registerUserAction,
            UpdateUserAction updateUserAction,
            DeleteUserAction deleteUserAction,
            GetAllUsersAction getAllUsersAction,
            GetLegerAction getLegerAction,
            GetUserAction getUserAction,
            ResetPasswordAction resetPasswordAction,
            NewPasswordAction newPasswordAction,
            RemoveLegeFromAvdelingAction removeLegeFromAvdelingAction,
            AddUserToAvdelingAction addUserToAvdelingAction
        )
        {
            _authenticateAction = authenticateAction;
            _registerUserAction = registerUserAction;
            _updateUserAction = updateUserAction;
            _deleteUserAction = deleteUserAction;
            _getAllUsersAction = getAllUsersAction;
            _getLegerAction = getLegerAction;
            _getUserAction = getUserAction;
            _resetPasswordAction = resetPasswordAction;
            _newPasswordAction = newPasswordAction;
            _removeLegeFromAvdelingAction = removeLegeFromAvdelingAction;
            _addUserToAvdelingAction = addUserToAvdelingAction;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]RegisterUserModel registerUserModel)
        {
            var authenticatedUser = _authenticateAction.Authenticate(registerUserModel.Username, registerUserModel.Password);

            if (authenticatedUser == null)
            {
                return BadRequest("Username or password is incorrect");
            }

            return Ok(authenticatedUser);
        }

        [RoleAuthorization(Role.Admin, Role.Overlege, Role.Lege)]
        [HttpGet("current")]
        public IActionResult GetCurrentUser()
        {
            var username = User.Identity.Name;
            var user = _getUserAction.GetUser(username);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("current/resetpassword")]
        public IActionResult ResetPassword([FromBody]ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _resetPasswordAction.Execute(model);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("current/newPassword")]
        public IActionResult NewPassword([FromBody]NewPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _newPasswordAction.Execute(model);

            return Ok();
        }

        
        [AllowAnonymous]
        [HttpPost("register/{id}")]
        public IActionResult Register(Guid id, [FromBody]RegisterUserModel registerUserModel)
        {
            try
            {
                var legeSpesialitet = LegeSpesialitet.Spesialiteter.FirstOrDefault(s => s.Id == registerUserModel.LegeSpesialitetId);
                if (legeSpesialitet == null)
                {
                    return BadRequest($"Cannot find legespesialitet with id {registerUserModel.LegeSpesialitetId}");
                }

                // save 
                var userId = _registerUserAction.Execute(
                    invitationId: id,
                    newUser: new NewUser(
                        firstName: registerUserModel.FirstName,
                        lastName: registerUserModel.LastName,
                        username: registerUserModel.Username,
                        legeSpesialitet: legeSpesialitet,
                        password: registerUserModel.Password,
                        role: registerUserModel.Role
                    )
                );

                return Ok(new { id = userId });
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorization(Role.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _getAllUsersAction.GetAllUsers();
            return Ok(users);
        }

        [RoleAuthorization(Role.Admin, Role.Overlege, Role.Lege)]
        [HttpGet("leger")]
        public IActionResult GetLeger()
        {
            var leger = _getLegerAction.GetLegerInSameAvdeling(User.Identity.Name);
            return Ok(leger);
        }

        [RoleAuthorization(Role.Admin, Role.Overlege, Role.Lege)]
        [HttpGet("alle-leger")]
        public IActionResult GetAlleLeger()
        {
            var leger = _getLegerAction.GetAlleLeger(User.Identity.Name);
            return Ok(leger);
        }

        [RoleAuthorization(Role.Admin, Role.Overlege, Role.Lege)]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var user = _getUserAction.GetUser(User.Identity.Name, id);
                return Ok(user);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [RoleAuthorization(Role.Admin)]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]UpdateUserModel updatedUserModel)
        {
            try 
            {
                _updateUserAction.Update(
                    new UpdatedUser(
                        id: id,
                        firstName: updatedUserModel.FirstName,
                        lastName: updatedUserModel.LastName,
                        username: updatedUserModel.Username,
                        password: updatedUserModel.Password
                    )
                );

                return Ok();
            } 
            catch(AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [RoleAuthorization(Role.Admin, Role.Overlege)]
        [HttpPost("avdelinger/{avdelingId:int}/leger")]
        public IActionResult AddToAvdeling(int avdelingId, [FromBody] AddUserToAvdelingModel model)
        {
            _addUserToAvdelingAction.Execute(User.Identity.Name, avdelingId, model.UserId);
            return Ok();
        }

        [RoleAuthorization(Role.Admin, Role.Overlege)]
        [HttpDelete("avdelinger/{avdelingId:int}/leger/{legeId:int}")]
        public IActionResult DeleteFromAvdeling(int avdelingId, int legeId)
        {
            _removeLegeFromAvdelingAction.Execute(User.Identity.Name, avdelingId, legeId);
            return Ok();
        }

        [RoleAuthorization(Role.Admin)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _deleteUserAction.Delete(id);
            return Ok();
        }
    }
}
