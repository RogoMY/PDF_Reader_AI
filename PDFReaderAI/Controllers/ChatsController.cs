using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDFReaderAI.Models;
using PDFReaderAI.Services;

namespace PDFReaderAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IChatRepository chatRepository;
        public ChatsController(IChatRepository chatRepository)
        {
            this.chatRepository = chatRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetChatHistory()
        {
            var chats = await chatRepository.GetChatHistoryAsnc();
            return Ok(chats);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChatById(Guid id)
        {
            var chat = await chatRepository.GetChatByIdAsync(id);
            if (chat == null)
            {
                return NotFound();
            }
            return Ok(chat);
        }
        [HttpPost]
        public async Task<IActionResult> AddChat([FromBody] Chat chat)
        {
            if (chat == null)
            {
                return BadRequest("Chat cannot be null");
            }
            var addedChat = await chatRepository.AddChatAsync(chat);
            return CreatedAtAction(nameof(GetChatById), new { id = addedChat.Id }, addedChat);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChat(Guid id, [FromBody] Chat chat)
        {
            if (chat == null || chat.Id != id)
            {
                return BadRequest("Chat cannot be null and ID must match");
            }
            var updated = await chatRepository.UpdateChatAsync(chat);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChat(Guid id)
        {
            var deleted = await chatRepository.DeleteChatAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
