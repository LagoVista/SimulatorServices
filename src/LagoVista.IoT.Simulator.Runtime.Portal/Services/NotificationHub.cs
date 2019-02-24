using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Runtime.Portal.Services
{
    public class NotificationHub : Hub
    {
        static int _hubCount = 0;

        public static NotificationHub Current { get; set; }

        public NotificationHub()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Gen => " + Guid.NewGuid());
            Console.ResetColor();

            Current = this;

            _hubCount++;
        }

        public async Task Send(string message)
        {
            await Clients.All.SendAsync("Send", message);
        }

        public Task NotifyAsync(String msg)
        {
           return Clients.All.SendAsync("update", new string[] { msg });
        }

        public override Task OnConnectedAsync()
        {
            var cid = Context.ConnectionId;

            return base.OnConnectedAsync();
        }


    }
}
