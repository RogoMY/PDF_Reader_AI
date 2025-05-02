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

        [HttpPost("{id}/interact")]
        public async Task<IActionResult> InteractWithAI(
           Guid id,
           [FromBody] AIInteractionRequest request,
           [FromServices] IChatAIService chatAIService)
        {
            // Validăm cererea
            if (request == null || string.IsNullOrEmpty(request.AIModel) || string.IsNullOrEmpty(request.Prompt))
            {
                return BadRequest("Both AIModel and Prompt fields are required.");
            }

            // Obține chat-ul din baza de date
            var chat = await chatRepository.GetChatByIdAsync(id);
            if (chat == null)
            {
                return NotFound($"Chat with ID {id} not found.");
            }

            string newPrompt = request.Prompt;

            if (chat.FileContent != null && chat.FileContent.Length > 0)
            {
                switch (chat.FileMimeType)
                {
                    case "application/pdf":
                        newPrompt = $"Te rog sa imi rezumi foarte detaliat si cu explicatii tehnice toate informatiile din fisierul PDF atasat, inclusiv cu sursa si autori: {chat.FileName}.";
                        break;

                    case "image/jpeg":
                        newPrompt = $"Te rog sa analizezi imaginea JPEG atasata ({chat.FileName}) si sa imi oferi o descriere detaliata a continutului acesteia.";
                        break;
                 
                    case "text/plain":
                        newPrompt = $"Te rog sa analizezi fisierul TXT atasat ({chat.FileName}) si sa imi oferi un rezumat detaliat al continutului acestuia.";
                        break;

                    default:
                        return BadRequest($"Unsupported file type: {chat.FileMimeType}.");
                }
            }

            // Apelăm serviciul AI pentru a obține răspunsul
            var aiResponse = await chatAIService.GetResponseAsync(
                chat.Prompts,
                chat.Responses,
                newPrompt,
                chat.FileContent,
                request.AIModel
            );

            // Actualizăm prompturile și răspunsurile în baza de date
            var updated = await chatRepository.UpdateChatPromptsAndResponsesAsync(id, newPrompt, aiResponse);
            if (!updated)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update chat with AI response.");
            }

            // Returnăm răspunsul AI-ului
            return Ok(new
            {
                ChatId = id,
                NewPrompt = newPrompt,
                AIResponse = aiResponse
            });
        }






    }

}

