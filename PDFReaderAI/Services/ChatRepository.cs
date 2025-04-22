using Microsoft.EntityFrameworkCore;
using PDFReaderAI.Data;
using PDFReaderAI.Models;

namespace PDFReaderAI.Services
{
    public class ChatRepository : IChatRepository
    {
        private readonly ChatDbContext context;
        private readonly ILogger<ChatRepository> logger;

        public ChatRepository(ChatDbContext context, ILogger<ChatRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        public async Task<Chat> AddChatAsync(Chat chat)
        {
   
            context.Chats.Add(chat);
            await context.SaveChangesAsync();
            logger.LogInformation("Chat added to the database: {Id}",chat.Id);
            return chat;
        }

        public async Task<bool> DeleteChatAsync(Guid id)
        {
            var chat = await context.Chats.FindAsync(id);
            if (chat == null)
            {
                logger.LogWarning("Chat with id {Id} not found", id);
                return false;
            }
            context.Chats.Remove(chat);
            await context.SaveChangesAsync();
            logger.LogInformation("Chat with id {Id} deleted", id);
            return true;
        }

        public async Task<Chat> GetChatByIdAsync(Guid id)
        {
            logger.LogInformation("Fetching chat with id {Id}", id);
            return await context.Chats.FindAsync(id);
        }

        public async Task<IEnumerable<Chat>> GetChatHistoryAsnc()
        {
            logger.LogInformation("Fetching chat history");
            return await context.Chats.ToListAsync();
        }

        public async Task<bool> UpdateChatAsync(Chat chat)
        {
            context.Chats.Update(chat);
            logger.LogInformation("Chat with id {Id} updated", chat.Id);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateChatPromptsAndResponsesAsync(Guid id, string newPrompt, string newResponse)
        {
            var chat = await context.Chats.FindAsync(id);
            if (chat == null) return false;

            // Initialize arrays if null
            var prompts = chat.Prompts != null ? chat.Prompts.ToList() : new List<string>();
            var responses = chat.Responses != null ? chat.Responses.ToList() : new List<string>();

            // Append new entries
            prompts.Add(newPrompt);
            responses.Add(newResponse);

            // Update model
            chat.Prompts = prompts.ToArray();
            chat.Responses = responses.ToArray();
            chat.TimeOfDiscussion = DateTime.UtcNow;

            context.Chats.Update(chat);
            await context.SaveChangesAsync();
            return true;
        }

    }
}
