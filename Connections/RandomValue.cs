using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace LiveDashboard.Web.Connections
{
    public class RandomValue : PersistentConnection
    {
        static Thread backgroundThread;
        static int connections;
        static bool keepGoing;
        Notifier notifier;

        protected override Task OnConnected(IRequest request, string connectionId)
        {
            keepGoing = true;
            notifier = new Notifier();

            if (backgroundThread == null)
            {
                backgroundThread = new Thread(() =>
                {
                    while (keepGoing)
                    {
                        notifier.Notify();
                        Thread.Sleep(1000);
                    }

                });

                backgroundThread.Start();
            }

            var groupName = GetStringValue(request, "group") ?? "default";
            var minValue = GetValue(request, "min") ?? 0;
            var maxValue = GetValue(request, "max") ?? 100;

            Groups.Add(connectionId, groupName);
            notifier.AddGroup(new Group
            {
                Name = groupName,
                Min = minValue,
                Max = maxValue
            });

            connections += 1;

            return base.OnConnected(request, connectionId);
        }

        static string GetStringValue(IRequest request, string keyName)
        {
            return request.QueryString.AllKeys.Contains(keyName)
                       ? request.QueryString[keyName]
                       : null;
        }

        static int? GetValue(IRequest request, string keyName)
        {
            int minValue;
            if (request.QueryString.AllKeys.Contains(keyName) && int.TryParse(request.QueryString[keyName], out minValue))
            {
                return minValue;
            }
            return null;
        }

        protected override Task OnDisconnected(IRequest request, string connectionId)
        {
            connections--;
            if (connections < 1)
            {
                keepGoing = false;
                backgroundThread = null;
            }
            return base.OnDisconnected(request, connectionId);
        }
    }
}