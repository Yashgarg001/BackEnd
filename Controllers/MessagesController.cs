using ChatAppBackEnd.Model;
using ChatAppBackEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ChatAppBackEnd.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly ChatService _chatService;

        public MessagesController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage(MessageDto messageDto)
        {
            await _chatService.SendMessageAsync(messageDto);
            return Ok(new { message = "Message sent!" });
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _chatService.GetMessagesAsync();
            return Ok(messages);
        }

        [HttpGet("getBetweenUsers")]
        public async Task<IActionResult> GetMessagesBetweenUsers(int senderId, int receiverId)
        {
            var messages = await _chatService.GetMessagesBetweenUsersAsync(senderId, receiverId);
            return Ok(messages);
        }
    }
}

