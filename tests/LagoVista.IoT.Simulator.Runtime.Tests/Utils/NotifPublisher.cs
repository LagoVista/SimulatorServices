// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: fca6be0eb2495afcb8bff625ea7eaad0573e7b862f710723f85216ac52b8d48b
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.IoT.Runtime.Core.Models.Messaging;
using LagoVista.IoT.Runtime.Core.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Runtime.Tests.Utils
{
    public class NotifPublisher : INotificationPublisher
    {
        public Task PublishAsync(Targets target, Notification notification, NotificationVerbosity verbosity = NotificationVerbosity.Normal)
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"{notification.Channel} {notification.ChannelId}");
            Console.WriteLine($"{verbosity}");
            Console.WriteLine($"{notification.PayloadType}");
            Console.WriteLine($"{notification.Payload}");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine();

            return Task.FromResult(default(object));
        }

        public Task PublishAsync<TPayload>(Targets target, Channels channel, string channelId, TPayload message, NotificationVerbosity verbosity = NotificationVerbosity.Normal)
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"{channel} {channelId}");
            Console.WriteLine($"{verbosity}");
            Console.WriteLine(JsonConvert.SerializeObject(message));
            Console.WriteLine(typeof(TPayload));
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine();

            return Task.FromResult(default(object));
        }

        public Task PublishAsync<TPayload>(Targets target, Channels channel, string channelId, string text, TPayload message, NotificationVerbosity verbosity = NotificationVerbosity.Normal)
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"{channel} {channelId}");
            Console.WriteLine($"{verbosity}");
            Console.WriteLine($"{text}");
            Console.WriteLine(JsonConvert.SerializeObject(message));
            Console.WriteLine(typeof(TPayload));
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine();

            return Task.FromResult(default(object));
        }

        public Task PublishAsync<TEntity>(Targets target, TEntity entity, NotificationVerbosity verbosity = NotificationVerbosity.Normal) where TEntity : IIDEntity
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"{typeof(TEntity).Name} {entity.Id}");
            Console.WriteLine($"{verbosity}");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine();

            return Task.FromResult(default(object));
        }


        public Task PublishTextAsync(Targets target, Channels channel, string channelId, string text, NotificationVerbosity verbosity = NotificationVerbosity.Normal)
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"{channel} {channelId}");
            Console.WriteLine($"{verbosity}");
            Console.WriteLine($"{text}");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine();

            return Task.FromResult(default(object));
        }
    }
}
