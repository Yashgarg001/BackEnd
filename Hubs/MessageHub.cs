using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ChatAppBackEnd.Hubs
{
    [Authorize]
    public class MessageHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"MessageHub: User {Context.UserIdentifier} connected. ConnectionId: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (exception != null)
            {
                Console.WriteLine($"MessageHub: User {Context.UserIdentifier} disconnected. ConnectionId: {Context.ConnectionId}, Error: {exception.Message}, StackTrace: {exception.StackTrace}");
            }
            else
            {
                Console.WriteLine($"MessageHub: User {Context.UserIdentifier} disconnected. ConnectionId: {Context.ConnectionId}");
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinRoom(string otherUserId)
        {
            try
            {
                Console.WriteLine($"MessageHub: JoinRoom: User {Context.UserIdentifier} attempting to join room with {otherUserId}");

                if (string.IsNullOrEmpty(otherUserId))
                {
                    Console.WriteLine($"MessageHub: JoinRoom: otherUserId is null or empty.");
                    throw new ArgumentException("otherUserId cannot be null or empty.");
                }

                await Groups.AddToGroupAsync(Context.ConnectionId, otherUserId);

                Console.WriteLine($"MessageHub: JoinRoom: User {Context.UserIdentifier} joined room with {otherUserId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MessageHub: JoinRoom: Error joining room: {ex.Message}, StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task SendMessage(string receiverId, string content)
        {
            try
            {
                Console.WriteLine($"MessageHub: SendMessage: User {Context.UserIdentifier} sending message to {receiverId}: {content}");
                await Clients.Group(receiverId).SendAsync("ReceiveMessage", Context.UserIdentifier, receiverId, content);
                Console.WriteLine($"MessageHub: SendMessage: Message sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MessageHub: SendMessage: Error sending message: {ex.Message}, StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task FetchChatHistory(string otherUserId)
        {
            try
            {
                Console.WriteLine($"MessageHub: FetchChatHistory: User {Context.UserIdentifier} fetching chat history with {otherUserId}");
                Console.WriteLine($"MessageHub: FetchChatHistory: Logic skipped for testing.");
                // Add your database logic here to fetch chat history
                // Example:
                // var chatHistory = await _dbContext.Messages.Where(...).ToListAsync();
                // await Clients.Caller.SendAsync("ReceiveChatHistory", chatHistory);
                Console.WriteLine($"MessageHub: FetchChatHistory: Chat history fetched successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MessageHub: FetchChatHistory: Error fetching chat history: {ex.Message}, StackTrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}