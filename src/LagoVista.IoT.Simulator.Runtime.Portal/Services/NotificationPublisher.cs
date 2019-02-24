﻿using LagoVista.IoT.Runtime.Core.Models.Messaging;
using LagoVista.IoT.Runtime.Core.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Runtime.Portal.Services
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
