using Microsoft.AspNetCore.SignalR;

namespace rig_controller.Hubs
{
    public class UiHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
        }
    }
}
