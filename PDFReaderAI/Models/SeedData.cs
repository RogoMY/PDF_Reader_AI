using Microsoft.EntityFrameworkCore;
using PDFReaderAI.Data;

namespace PDFReaderAI.Models
{
    public class SeedData
    {
        public static async Task InitializeAsync(ChatDbContext context)
        {
            if (await context.Chats.AnyAsync())
            {
                return; // DB has been seeded
            }
            var chats = new List<Chat>
            {
                new Chat
                {
                    Id = Guid.NewGuid(),
                    Title = "Chat 1",
                    Prompts = new[] { "Prompt 1", "Prompt 2" },
                    Responses = new[] { "Response 1", "Response 2" },
                    TimeOfDiscussion = DateTime.UtcNow,
                    FileName = "file1.pdf",
                    FileContent = new byte[] { 0x01, 0x02, 0x03 },
                    FileMimeType = "application/pdf"
                },
                new Chat
                {
                    Id = Guid.NewGuid(),
                    Title = "Chat 2",
                    Prompts = new[] { "Prompt 3", "Prompt 4" },
                    Responses = new[] { "Response 3", "Response 4" },
                    TimeOfDiscussion = DateTime.UtcNow,
                    FileName = "file2.pdf",
                    FileContent = new byte[] { 0x04, 0x05, 0x06 },
                    FileMimeType = "application/pdf"
                },
                new Chat
                {
                    Id = Guid.NewGuid(),
                    Title = "Chat 3",
                    Prompts = new[] { "Prompt 5", "Prompt 6" },
                    Responses = new[] { "Response 5", "Response 6" },
                    TimeOfDiscussion = DateTime.UtcNow,
                    FileName = "file3.pdf",
                    FileContent = new byte[] { 0x07, 0x08, 0x09 },
                    FileMimeType = "application/pdf"
                }
            };
            await context.Chats.AddRangeAsync(chats);
            await context.SaveChangesAsync();
        }
    }
}
