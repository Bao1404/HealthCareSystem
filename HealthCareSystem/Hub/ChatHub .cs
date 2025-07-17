// Hubs/ChatHub.cs
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    public async Task SendMessage(string conversationId, string senderId, string message)
    {
        await Clients.Group(conversationId).SendAsync("ReceiveMessage", senderId, message);
    }
        
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var conversationId = httpContext?.Request.Query["conversationId"];

        if (!string.IsNullOrEmpty(conversationId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var httpContext = Context.GetHttpContext();
        var conversationId = httpContext?.Request.Query["conversationId"];

        if (!string.IsNullOrEmpty(conversationId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}
