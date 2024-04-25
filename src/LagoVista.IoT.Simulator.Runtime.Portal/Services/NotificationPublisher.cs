using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.IoT.Runtime.Core.Models.Messaging;
using LagoVista.IoT.Runtime.Core.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;



namespace LagoVista.IoT.Simulator.Runtime.Portal.Services
{
    public class NotificationPublisher : INotificationPublisher
    {
        IServiceProvider _serviceProvider;

        readonly JsonSerializerSettings _camelCaseSettings = new Newtonsoft.Json.JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        public NotificationPublisher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }        

        void ConsoleWriteLine(string msg = "")
        {

        }

        public async Task PublishAsync(Targets target, Notification notification, NotificationVerbosity verbosity = NotificationVerbosity.Normal)
        {
            ConsoleWriteLine("--------------------------------------------------------------");
            ConsoleWriteLine($"{notification.Channel} {notification.ChannelId}");
            ConsoleWriteLine($"{verbosity}");
            ConsoleWriteLine($"{notification.PayloadType}");
            ConsoleWriteLine($"{notification.Payload}");
            ConsoleWriteLine("--------------------------------------------------------------");
            ConsoleWriteLine();

            notification.Verbosity = EntityHeader<NotificationVerbosity>.Create(verbosity);

            var _hub = _serviceProvider.GetService<IHubContext<NotificationHub>>();
            await _hub.Clients.All.SendAsync("notification", JsonConvert.SerializeObject(notification, _camelCaseSettings));
        }

        public Task PublishAsync<TPayload>(Targets target, Channels channel, string channelId, TPayload message, NotificationVerbosity verbosity = NotificationVerbosity.Normal)
        {
            ConsoleWriteLine("--------------------------------------------------------------");
            ConsoleWriteLine($"{channel} {channelId}");
            ConsoleWriteLine($"{verbosity}");
            ConsoleWriteLine(JsonConvert.SerializeObject(message));
            ConsoleWriteLine(typeof(TPayload).ToString());
            ConsoleWriteLine("--------------------------------------------------------------");
            ConsoleWriteLine();

            var notification = new Notification()
            {
                 Channel =  EntityHeader<Channels>.Create(channel),
                 ChannelId = channelId,
                 PayloadType = typeof(TPayload).Name,
                 Payload = JsonConvert.SerializeObject(message, _camelCaseSettings)
            };

            return PublishAsync(target, notification);
        }

        public Task PublishAsync<TEntity>(Targets target, TEntity entity, NotificationVerbosity verbosity = NotificationVerbosity.Normal) where TEntity : IIDEntity
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"{typeof(TEntity).Name} {entity.Id}");
            Console.WriteLine($"{verbosity}");
            ConsoleWriteLine("--------------------------------------------------------------");
            ConsoleWriteLine();

            var notification = new Notification()
            {
                Channel = EntityHeader<Channels>.Create(Channels.Entity),
                ChannelId = entity.Id,
                PayloadType = typeof(TEntity).Name,
                Payload = JsonConvert.SerializeObject(entity, _camelCaseSettings)
            };

            return PublishAsync(target, notification);
        }

            public Task PublishAsync<TPayload>(Targets target, Channels channel, string channelId, string text, TPayload message, NotificationVerbosity verbosity = NotificationVerbosity.Normal)
        {
            ConsoleWriteLine("--------------------------------------------------------------");
            ConsoleWriteLine($"{channel} {channelId}");
            ConsoleWriteLine($"{verbosity}");
            ConsoleWriteLine($"{text}");
            ConsoleWriteLine(JsonConvert.SerializeObject(message));
            ConsoleWriteLine(typeof(TPayload).ToString());
            ConsoleWriteLine("--------------------------------------------------------------");
            ConsoleWriteLine();

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
            ConsoleWriteLine("--------------------------------------------------------------");
            ConsoleWriteLine($"{channel} {channelId}");
            ConsoleWriteLine($"{verbosity}");
            ConsoleWriteLine($"{text}");
            ConsoleWriteLine("--------------------------------------------------------------");
            ConsoleWriteLine();

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
