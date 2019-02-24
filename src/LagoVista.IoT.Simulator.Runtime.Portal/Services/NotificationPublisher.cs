using LagoVista.IoT.Runtime.Core.Models.Messaging;
using LagoVista.IoT.Runtime.Core.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;



namespace LagoVista.IoT.Simulator.Runtime.Portal.Services
{
    public class NotificationPublisher : INotificationPublisher
    {
        IServiceCollection _serviceCollection;

        IServiceProvider _serviceProvider;

        IHubContext<NotificationHub> _hub;

        public NotificationPublisher(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public NotificationPublisher(IServiceCollection collection)
        {
            _serviceCollection = collection;
        }

        public NotificationPublisher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        private IHubContext<NotificationHub> GetHub()
        {
           // var sp = _serviceCollection.BuildServiceProvider();
           // var svc = _serviceCollection.Where(sc => sc.ServiceType == typeof(IHubContext<NotificationHub>)).FirstOrDefault();
            return _serviceProvider.GetService<IHubContext<NotificationHub>>();
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

            _hub = GetHub();
            await _hub.Clients.All.SendCoreAsync("update", new string[] { notification.Payload });
        }

        public async Task PublishAsync<TPayload>(Targets target, Channels channel, string channelId, TPayload message, NotificationVerbosity verbosity = NotificationVerbosity.Normal)
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"{channel} {channelId}");
            Console.WriteLine($"{verbosity}");
            Console.WriteLine(JsonConvert.SerializeObject(message));
            Console.WriteLine(typeof(TPayload));
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine();


            _hub = GetHub();
            await _hub.Clients.All.SendCoreAsync("update", new string[] { JsonConvert.SerializeObject(message) });
        }

        public async Task PublishAsync<TPayload>(Targets target, Channels channel, string channelId, string text, TPayload message, NotificationVerbosity verbosity = NotificationVerbosity.Normal)
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"{channel} {channelId}");
            Console.WriteLine($"{verbosity}");
            Console.WriteLine($"{text}");
            Console.WriteLine(JsonConvert.SerializeObject(message));
            Console.WriteLine(typeof(TPayload));
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine();


            _hub = GetHub();
            await _hub.Clients.All.SendCoreAsync("update", new string[] { JsonConvert.SerializeObject(message) });
        }

        public async Task PublishTextAsync(Targets target, Channels channel, string channelId, string text, NotificationVerbosity verbosity = NotificationVerbosity.Normal)
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"{channel} {channelId}");
            Console.WriteLine($"{verbosity}");
            Console.WriteLine($"{text}");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine();


            _hub = GetHub();
            await _hub.Clients.All.SendAsync("update", text);
        }
    }
}
