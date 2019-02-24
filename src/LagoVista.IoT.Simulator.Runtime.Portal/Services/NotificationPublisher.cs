using LagoVista.Core.Models;
using LagoVista.IoT.Runtime.Core.Models.Messaging;
using LagoVista.IoT.Runtime.Core.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Threading.Tasks;



namespace LagoVista.IoT.Simulator.Runtime.Portal.Services
{
    public class NotificationPublisher : INotificationPublisher
    {
        IServiceProvider _serviceProvider;

        static JsonSerializerSettings _camelCaseSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

        public NotificationPublisher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }        

        public async Task PublishAsync(Targets target, Notification notification, NotificationVerbosity verbosity = NotificationVerbosity.Normal)
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"{notification.Channel} {notification.ChannelId}");
            Console.WriteLine($"{verbosity}");
            Console.WriteLine($"{notification.PayloadType}");
            Console.WriteLine($"{notification.Payload}");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine();

            notification.Verbosity = EntityHeader<NotificationVerbosity>.Create(verbosity);

            var _hub = _serviceProvider.GetService<IHubContext<NotificationHub>>();
            await _hub.Clients.All.SendAsync("notification", JsonConvert.SerializeObject(notification));
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

            var notification = new Notification()
            {
                 Channel =  EntityHeader<Channels>.Create(channel),
                 ChannelId = channelId,
                 PayloadType = typeof(TPayload).Name,
                 Payload = JsonConvert.SerializeObject(message)
            };

            return PublishAsync(target, notification);
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

            var notification = new Notification()
            {
                Channel = EntityHeader<Channels>.Create(channel),
                ChannelId = channelId,
                Title = text,
                PayloadType = typeof(TPayload).Name,
                Payload = JsonConvert.SerializeObject(message, _camelCaseSettings)
            };

            return PublishAsync(target, notification);
        }

        public Task PublishTextAsync(Targets target, Channels channel, string channelId, string text, NotificationVerbosity verbosity = NotificationVerbosity.Normal)
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"{channel} {channelId}");
            Console.WriteLine($"{verbosity}");
            Console.WriteLine($"{text}");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine();

            var notification = new Notification()
            {
                Channel = EntityHeader<Channels>.Create(channel),
                ChannelId = channelId,
                Title = text,
                PayloadType = "text"
            };

            return PublishAsync(target, notification);
        }
    }
}
