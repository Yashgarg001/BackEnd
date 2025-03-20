using ChatAppBackEnd.Data;
using ChatAppBackEnd.Model;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackEnd.Services
{
    public class ChatService
    {
        private readonly ChatAppDbContext _context;

        public ChatService(ChatAppDbContext context)
        {
            _context = context;
        }


        // Send a message
        public async Task SendMessageAsync(MessageDto messageDto)
        {
            var message = new Message
            {
                SenderId = messageDto.SenderId,
                ReceiverId = messageDto.ReceiverId,
                Content = messageDto.Content
            };
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        // Get all messages
        public async Task<List<Message>> GetMessagesAsync()
        {
            return await _context.Messages
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

        // Get messages between two users
        public async Task<List<Message>> GetMessagesBetweenUsersAsync(int senderId, int receiverId)
        {
            return await _context.Messages
                .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                            (m.SenderId == receiverId && m.ReceiverId == senderId))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

    }
}
