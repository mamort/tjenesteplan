using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Tjenesteplan.Api.Filters;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Events;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.Notifications
{
    [Route("api/notifications")]
    public class NotificationsController : BaseController
    {
	    private readonly TelemetryClient _telemetryClient;
	    private readonly INotificationRepository _notificationRepository;
	    private readonly IUserRepository _userRepository;

	    public NotificationsController(TelemetryClient telemetryClient, INotificationRepository notificationRepository, IUserRepository userRepository)
	    {
		    _telemetryClient = telemetryClient;
		    _notificationRepository = notificationRepository;
		    _userRepository = userRepository;
	    }

        [RoleAuthorization(Role.Admin, Role.Overlege, Role.Lege)]
        [Route("")]
        [HttpGet]
        public IActionResult Get()
        {
	        var user = _userRepository.GetUserByUsername(HttpContext.User.Identity.Name);
            return Ok(_notificationRepository.GetUnreadNotifications(user).ToList());
        }

        [RoleAuthorization(Role.Admin, Role.Overlege, Role.Lege)]
        [Route("{id:int}/Read")]
        [HttpPut]
        public IActionResult SetNotificationRead(int id)
        {
	        var notification =_notificationRepository.SetNotificationRead(id);

	        if (notification != null)
	        {
		        return Ok(id);

	        }

	        _telemetryClient.TrackEvent(
		        TelemetryEvent.Notification.UnableToSetNotificationRead,
		        new Dictionary<string, string> {{"id", $"{id}"}});

	        return BadRequest($"Unable to mark notification with id: '{id}' as read");
        }
    }
}