using Microsoft.AspNetCore.SignalR;

namespace WebSocket.Hubs;

// sql de saxlasaq uname ve image db de qalacaq connection id ramda qalr list kimi cunki deyisir her reloadda
class User
{
    public string Username { get; set; }
    public string ImageUrl { get; set; }
    public string ConnectionId { get; set; }
    public User(string id)
    {
        ConnectionId = id;
    }
}
public class ChatHub : Hub
{
    static List<User> _users = new List<User>();
    public override Task OnConnectedAsync() //if joined new user in hub
    {
        _users.Add(new User(Context.ConnectionId));

        return base.OnConnectedAsync();
    }
    public override Task OnDisconnectedAsync(Exception? exception) //if user exit in hub
    {
        _users.Remove(FindUser(Context.ConnectionId));
        Clients.All.SendAsync("GetUsers", _users.Where(u => u.Username != null).Select(u => new
        {
            Username = u.Username,
            ImageUrl = u.ImageUrl,
        }));
        return base.OnDisconnectedAsync(exception);
    }

    public async Task Register(string username, string imageUrl)
    {
        var user = FindUser(Context.ConnectionId);
        user.Username = username;
        user.ImageUrl = imageUrl;
        Clients.All.SendAsync("GetUsers", _users.Where(u => u.Username != null).Select(u => new
        {
            Username = u.Username,
            ImageUrl = u.ImageUrl,
        }));
        //await Clients.All.SendAsync("NewUser", username, imageUrl);
    }
    public async Task SendMessage(string username, string message)
    {
        var user = FindUser(Context.ConnectionId);
        await Clients.All.SendAsync("RecievedMessage", user.Username, message);

    }

    User FindUser(string id)
    {
        return _users.Find(u => u.ConnectionId == id);
    }

}
