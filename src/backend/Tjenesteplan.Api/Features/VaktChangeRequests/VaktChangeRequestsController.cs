using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tjenesteplan.Api.Exceptions;
using Tjenesteplan.Api.Features.VaktChangeRequests.AcceptVaktChange;
using Tjenesteplan.Api.Features.VaktChangeRequests.AddVaktChangeRequest;
using Tjenesteplan.Api.Features.VaktChangeRequests.AllRequests;
using Tjenesteplan.Api.Features.VaktChangeRequests.ReceivedRequests;
using Tjenesteplan.Api.Features.VaktChangeRequests.SentRequests;
using Tjenesteplan.Api.Features.VaktChangeRequests.UndoVaktChangeRequest;
using Tjenesteplan.Api.Features.VaktChangeRequests.VaktChangeSuggestions;
using Tjenesteplan.Api.Filters;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.VaktChangeRequests
{
    [Authorize]
    [Route("api")]
    public class VaktChangeRequestsController : Controller
    {
        private readonly AddVaktChangeRequestAction _addVaktChangeRequestAction;
        private readonly GetAllRequestsAction _getAllRequestsAction;
        private readonly GetSentRequestsAction _getSentRequestsAction;
        private readonly GetVaktChangeRequestsReceivedAction _getVaktChangeRequestsReceivedAction;
        private readonly AddVaktChangeSuggestionsAction _addVaktChangeSuggestionsAction;
        private readonly AcceptVaktChangeAction _acceptVaktChangeAction;
        private readonly UndoVaktChangeRequestAction _undoVaktChangeAction;

        public VaktChangeRequestsController(
            AddVaktChangeRequestAction addVaktChangeRequestAction,
            GetAllRequestsAction getAllRequestsAction,
            GetSentRequestsAction getSentRequestsAction,
            GetVaktChangeRequestsReceivedAction getVaktChangeRequestsReceivedAction,
            AddVaktChangeSuggestionsAction addVaktChangeSuggestionsAction,
            AcceptVaktChangeAction acceptVaktChangeAction,
            UndoVaktChangeRequestAction undoVaktChangeAction
        )
        {
            _addVaktChangeRequestAction = addVaktChangeRequestAction;
            _getAllRequestsAction = getAllRequestsAction;
            _getSentRequestsAction = getSentRequestsAction;
            _getVaktChangeRequestsReceivedAction = getVaktChangeRequestsReceivedAction;
            _addVaktChangeSuggestionsAction = addVaktChangeSuggestionsAction;
            _acceptVaktChangeAction = acceptVaktChangeAction;
            _undoVaktChangeAction = undoVaktChangeAction;
        }

        [RoleAuthorization(Role.Admin, Role.Overlege, Role.Lege)]
        [HttpPost("[controller]")]
        public async Task<IActionResult> Add([FromBody]AddVaktChangeRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _addVaktChangeRequestAction.ExecuteAsync(User.Identity.Name, model);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }


        [RoleAuthorization(Role.Admin, Role.Overlege, Role.Lege)]
        [HttpGet("tjenesteplaner/{tjenesteplanId:int}/[controller]/sent")]
        public IActionResult GetSentChangeRequests(int tjenesteplanId)
        {
            try
            {
                var requests = _getSentRequestsAction.Execute(User.Identity.Name, tjenesteplanId);
                return Ok(requests);
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorization(Role.Admin, Role.Overlege, Role.Lege)]
        [HttpGet("tjenesteplaner/{tjenesteplanId:int}/[controller]/Received")]
        public IActionResult GetVaktChangeRequestReceived(int tjenesteplanId)
        {
            try
            {
                var model = _getVaktChangeRequestsReceivedAction.Execute(User.Identity.Name, tjenesteplanId);
                return Ok(model);
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorization(Role.Admin, Role.Overlege)]
        [HttpGet("tjenesteplaner/{tjenesteplanId:int}/[controller]")]
        public IActionResult GetAllVaktChangeRequests(int tjenesteplanId)
        {
            try
            {
                var requests = _getAllRequestsAction.Execute(User.Identity.Name, tjenesteplanId);
                return Ok(requests);
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [RoleAuthorization(Role.Admin, Role.Overlege, Role.Lege)]
        [HttpDelete("tjenesteplaner/{tjenesteplanId:int}/[controller]/{vaktChangeRequestId:int}")]
        public async Task<IActionResult> UndoVaktChangeRequest(int tjenesteplanId, int vaktChangeRequestId)
        {
            try
            {
                await _undoVaktChangeAction.Execute(User.Identity.Name, vaktChangeRequestId);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [RoleAuthorization(Role.Admin, Role.Overlege, Role.Lege)]
        [HttpPut("[controller]/Received/{vaktChangeRequestReplyId:int}")]
        public async Task<IActionResult> VaktChangeSuggestions(int vaktChangeRequestReplyId, [FromBody]AddVaktChangeSugguestionsModel model)
        {
            try
            {
                await _addVaktChangeSuggestionsAction.ExecuteAsync(User.Identity.Name, vaktChangeRequestReplyId, model);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorization(Role.Admin, Role.Overlege, Role.Lege)]
        [HttpPost("[controller]/Accept")]
        public async Task<IActionResult> Accept([FromBody]AcceptVaktChangeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _acceptVaktChangeAction.ExecuteAsync(User.Identity.Name, model);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}