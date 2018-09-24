using Microsoft.AspNetCore.SignalR;
namespace Finwin.Web
{
    public class TransactionHub : Hub
    {
        public async void SendData(string data)
        {
            await Clients.All.SendAsync("ReceiveData", data);
        }
    }
}
