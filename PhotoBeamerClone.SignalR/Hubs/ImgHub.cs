using Microsoft.AspNet.SignalR;

namespace PhotoBeamerClone.SignalR.Hubs
{
    public class ImgHub : Hub
    {
        public void Create(string guid)
        {
            Groups.Add(Context.ConnectionId, guid);
        }
        
        public void ShareImage(byte[] image, string guid)
        {
            Clients.OthersInGroup(guid).ReceiveImage(image);
        }

        public void Leave(string guid)
        {
            Groups.Remove(Context.ConnectionId, guid);
        }
    }
}