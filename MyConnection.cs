using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace LiveDashboard.Web
{
    public class MyConnection : PersistentConnection
    {
        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            return Connection.Broadcast(data);
        }
    }
}