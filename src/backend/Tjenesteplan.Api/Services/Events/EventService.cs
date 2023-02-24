using System;
using System.Linq;
using System.Text;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Tjenesteplan.Api.Configuration;
using Tjenesteplan.Domain.Framework;
using Tjenesteplan.Events;

namespace Tjenesteplan.Api.Services.Events
{
    public class EventService : IEventService
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly ILogger<EventService> _logger;
        private readonly IMediator _mediatr;
        private readonly IPersistentQueueFactory _queueFactory;
        private readonly CommonOptions _config;
        public EventService(
            IDateTimeService dateTimeService,
            ILogger<EventService> logger,
            IMediator mediatr,
            IOptions<CommonOptions> eventQueueConfig,
            IPersistentQueueFactory queueFactory
        )
        {
            _dateTimeService = dateTimeService;
            _logger = logger;
            _mediatr = mediatr;
            _queueFactory = queueFactory;
            _config = eventQueueConfig.Value;
        }

        public void ScheduleDelayedEvent(TimeSpan delay, ITjenesteplanEvent evt)
        {
            _logger.LogInformation("Scheduling delayed event");
            var envelope = new EventEnvelope
            {
                Type = evt.GetType().AssemblyQualifiedName,
                Payload = JsonConvert.SerializeObject(evt)
            };

            var eventMessage = JsonConvert.SerializeObject(envelope);

            AddQueueMessage(QueueConstants.EventsQueue, delay, eventMessage);
            AddQueueMessage(QueueConstants.EventsTriggerQueue, delay, "");
        }

        public void ProcessEvents()
        {
            _logger.LogInformation("Processing events");
            var queueClient = _queueFactory.GetQueue(_config.StorageConnectionString, QueueConstants.EventsQueue);
            queueClient.CreateIfNotExists();

            if (!queueClient.Exists())
            {
                _logger.LogError("Failed to process events because events-queue does not exist.");
                return;
            }

            // Handle a maximum of 5 * 10 = 50 events
            // Will also ensure that a failed event will be retried 5 times
            // TODO: add poison queue support
            var maxReceiveCount = 5;
            while (maxReceiveCount > 0)
            {
                maxReceiveCount--;

                var messages = queueClient.ReceiveMessages(10);
                if (!messages.Any())
                {
                    break;
                }

                foreach (var message in messages)
                {
                    ProcessEventsQueueMessage(queueClient, message);
                }
            }
        }

        private void ProcessEventsQueueMessage(IPersistentQueue queueClient, IPersistentQueueMessage message)
        {
            try
            {
                var decodedMessage = Base64Decode(message.MessageText);
                var evtEnvelope = JsonConvert.DeserializeObject<EventEnvelope>(decodedMessage);
                var evtType = Type.GetType(evtEnvelope.Type);
                var evt = JsonConvert.DeserializeObject(evtEnvelope.Payload, evtType);
                _logger.LogInformation("Publishing event: " + evtEnvelope.Type);
                _mediatr.Publish(evt);
                queueClient.DeleteMessage(message.MessageId, message.PopReceipt);
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to process event", e);
            }
        }

        private void AddQueueMessage(string queueName, TimeSpan delay, string message)
        {
            var client = _queueFactory.GetQueue(_config.StorageConnectionString, queueName);
            client.CreateIfNotExists();
            client.SendMessage(
                messageText: Base64Encode(message),
                visibilityTimeout: delay
            );
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private static string Base64Decode(string base64)
        {
            return Encoding.UTF8.GetString(System.Convert.FromBase64String(base64));
        }
    }
}