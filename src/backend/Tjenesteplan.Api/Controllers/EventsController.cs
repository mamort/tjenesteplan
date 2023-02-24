using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tjenesteplan.Events;

namespace Tjenesteplan.Api.Controllers
{
    [Route("api/events")]
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;
        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [AllowAnonymous]
        [HttpGet("")]
        public void Test()
        {
            _eventService.ScheduleDelayedEvent(TimeSpan.FromSeconds(30), new TestEvent() { Name = "mats" });
        }

        [AllowAnonymous]
        [HttpPost("process")]
        public void ProcessEvents()
        {
            _eventService.ProcessEvents();
        }
    }
}