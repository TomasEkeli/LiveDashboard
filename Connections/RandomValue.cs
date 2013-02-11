using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace LiveDashboard.Web.Connections
{
    public class RandomValue : PersistentConnection
    {
        static Thread thread;
        static int connections;

        protected override Task OnConnected(IRequest request, string connectionId)
        {
            if (thread == null)
            {
                thread = new Thread(() =>
                {
                    while (true)
                    {
                        var notifier = new Notifier();
                        notifier.Notify();
                        Thread.Sleep(1000);
                    }

                });

                thread.Start();
            }

            connections = 1;

            return base.OnConnected(request, connectionId);
        }

        protected override Task OnDisconnected(IRequest request, string connectionId)
        {
            connections--;
            if (connections < 1)
            {
                thread.Abort();
                thread = null;
            }
            return base.OnDisconnected(request, connectionId);
        }
    }


    public class Notifier
    {
        public void Notify()
        {
            var number = new Random().Next(0, 100);

            var context = GlobalHost.ConnectionManager.GetConnectionContext<RandomValue>();
            context.Connection.Broadcast(number);
        }
    }
}