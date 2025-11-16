// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 77e8756b6b9627f9ea223b98e771245b62daa1749985943b4863e8cb1ae2846e
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.IoT.Runtime.Core.Models.Messaging;
using LagoVista.IoT.Runtime.Core.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LagoVista.IoT.Simulator.Runtime
{
    public class NotificationPublisher : INotificationPublisher
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
            Console.WriteLine($"{typeof(TEntity).Name } {entity.Id}");
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
