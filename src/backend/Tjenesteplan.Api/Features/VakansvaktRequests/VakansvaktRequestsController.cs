using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tjenesteplan.Api.Exceptions;
using Tjenesteplan.Api.Features.VakansvaktRequests.AcceptVakansvaktRequest;
using Tjenesteplan.Api.Features.VakansvaktRequests.AddVakansvaktRequest;
using Tjenesteplan.Api.Features.VakansvaktRequests.ApproveVakansvaktRequest;
using Tjenesteplan.Api.Features.VakansvaktRequests.GetAvailableVakansvaktRequests;
using Tjenesteplan.Api.Features.VakansvaktRequests.GetVakansvaktRequest;
using Tjenesteplan.Api.Features.VakansvaktRequests.GetVakansvaktRequests;
using Tjenesteplan.Api.Features.VakansvaktRequests.RejectVakansvaktRequest;
using Tjenesteplan.Api.Features.VakansvaktRequests.UndoVakansvaktRequest;
using Tjenesteplan.Api.Filters;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.VakansvaktRequests
{
    [Authorize]
    [Route("api")]
    public class VakansvaktRequestsController : Controller
    {
        private readonly AddVakansvaktRequestAction _addVakansvaktRequestAction;
        private readonly GetVakansvaktRequestsAction _getVakansvaktRequestsAction;
        private readonly GetAvailableVakansvaktRequestsAction _getAvailableVakansvaktRequestsAction;
        private readonly GetVakansvaktRequestAction _getVakansvaktRequestAction;
        private readonly ApproveVakansvaktRequestAction _approveVakansvaktRequestAction;
        private readonly RejectVakansvaktRequestAction _rejectVakansvaktRequestAction;
        private readonly AcceptVakansvaktRequestAction _acceptVakansvaktRequestAction;
        private readonly UndoVakansvaktRequestAction _undoVakansvaktRequestAction;

        public VakansvaktRequestsController(
            AddVakansvaktRequestAction addVakansvaktRequestAction,
            GetVakansvaktRequestsAction getVakansvaktRequestsAction,
            GetAvailableVakansvaktRequestsAction getAvailableVakansvaktRequestsAction,
            GetVakansvaktRequestAction getVakansvaktRequestAction,
            ApproveVakansvaktRequestAction approveVakansvaktRequestAction,
            RejectVakansvaktRequestAction rejectVakansvaktRequestAction,
            AcceptVakansvaktRequestAction acceptVakansvaktRequestAction,
            UndoVakansvaktRequestAction undoVakansvaktRequestAction
        )
        {
            _addVakansvaktRequestAction = addVakansvaktRequestAction;
            _getVakansvaktRequestsAction = getVakansvaktRequestsAction;
            _getAvailableVakansvaktRequestsAction = getAvailableVakansvaktRequestsAction;
            _getVakansvaktRequestAction = getVakansvaktRequestAction;
            _approveVakansvaktRequestAction = approveVakansvaktRequestAction;
            _rejectVakansvaktRequestAction = rejectVakansvaktRequestAction;
            _acceptVakansvaktRequestAction = acceptVakansvaktRequestAction;
            _undoVakansvaktRequestAction = undoVakansvaktRequestAction;
        }

        [RoleAuthorization(Role.Admin, Role.Overlege, Role.Lege)]
        [HttpPost("[controller]")]
        public async Task<IActionResult> Add([FromBody]AddVakansvaktRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _addVakansvaktRequestAction.ExecuteAsync(User.Identity.Name, model);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [RoleAuthorization(Role.Admin, Role.Overlege, Role.Lege)]
        [HttpGet("tjenesteplaner/{tjenesteplanId:int}/[controller]")]
        public IActionResult GetMyVakansvaktRequests(int tjenesteplanId)
        {
            try
            {
                var requests = _getVakansvaktRequestsAction.GetVakansvaktRequests(User.Identity.Name, tjenesteplanId);
                return Ok(requests);
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorization(Role.Admin, Role.Overlege)]
        [HttpGet("tjenesteplaner/{tjenesteplanId:int}/[controller]/approved")]
        public IActionResult GetApprovedVakansvaktRequests(int tjenesteplanId)
        {
            try
            {
                var approvedRequests = _getVakansvaktRequestsAction.GetApprovedVakansvaktRequests(User.Identity.Name, tjenesteplanId);
                var acceptedRequests = _getVakansvaktRequestsAction.GetAcceptedVakansvaktRequests(User.Identity.Name, tjenesteplanId);
                var requests = approvedRequests.Concat(acceptedRequests);
                return Ok(requests);
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorization(Role.Admin, Role.Overlege)]
        [HttpGet("tjenesteplaner/{tjenesteplanId:int}/[controller]/unapproved")]
        public IActionResult GetUnApprovedVakansvaktRequests(int tjenesteplanId)
        {
            try
            {
                var requests = _getVakansvaktRequestsAction.GetUnapprovedVakansvaktRequests(User.Identity.Name, tjenesteplanId);
                return Ok(requests);
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [RoleAuthorization(Role.Admin, Role.Overlege, Role.Lege)]
        [HttpGet("Tjenesteplaner/{tjenesteplanId}/[controller]/Available")]
        public IActionResult GetAvailableVakansvaktRequests(int tjenesteplanId)
        {
            try
            {
                var requests = _getAvailableVakansvaktRequestsAction.Execute(User.Identity.Name, tjenesteplanId);
                return Ok(requests);
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorization(Role.Admin, Role.Overlege, Role.Lege)]
        [HttpGet("[controller]/{vakansvaktRequestId}")]
        public IActionResult GetVakansvaktRequest(int vakansvaktRequestId)
        {
            try
            {
                var request = _getVakansvaktRequestAction.Execute(User.Identity.Name, vakansvaktRequestId);
                if (request == null)
                {
                    return NotFound();
                }

                return Ok(request);
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorization(Role.Admin, Role.Overlege)]
        [HttpPost("[controller]/{vakansvaktRequestId}/Approve")]
        public IActionResult ApproveVakansvaktRequest(int vakansvaktRequestId)
        {
            try
            {
                _approveVakansvaktRequestAction.ExecuteAsync(User.Identity.Name, vakansvaktRequestId);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorization(Role.Admin, Role.Overlege)]
        [HttpPost("[controller]/{vakansvaktRequestId}/Reject")]
        public IActionResult RejectVakansvaktRequest(int vakansvaktRequestId)
        {
            try
            {
                _rejectVakansvaktRequestAction.ExecuteAsync(User.Identity.Name, vakansvaktRequestId);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorization(Role.Admin, Role.Overlege, Role.Lege)]
        [HttpPost("[controller]/{vakansvaktRequestId}/Accept")]
        public IActionResult AcceptVakansvaktRequest(int vakansvaktRequestId)
        {
            try
            {
                _acceptVakansvaktRequestAction.ExecuteAsync(User.Identity.Name, vakansvaktRequestId);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorization(Role.Admin, Role.Overlege)]
        [HttpDelete("[controller]/{vakansvaktRequestId}")]
        public IActionResult UndoVakansvaktRequest(int vakansvaktRequestId)
        {
            try
            {
                _undoVakansvaktRequestAction.ExecuteAsync(User.Identity.Name, vakansvaktRequestId);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}