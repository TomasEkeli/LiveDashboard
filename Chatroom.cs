using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace LiveDashboard.Web
{
    public class Chatroom : PersistentConnection
    {
        protected override Task OnConnected(IRequest request, string connectionId)
        {
            Connection.Send(connectionId, "Welcome to Chatting. Your name is <strong>" + connectionId + "</strong>");

            return Connection.Broadcast(connectionId + " has joined us");
        }

        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            return Connection.Broadcast(connectionId + ": " + data);
        }
    }
}