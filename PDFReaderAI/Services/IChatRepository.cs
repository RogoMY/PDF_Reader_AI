using PDFReaderAI.Models;

namespace PDFReaderAI.Services
{
    public interface IChatRepository
    {
        Task<IEnumerable<Chat>> GetChatHistoryAsnc();
        Task<Chat> GetChatByIdAsync(Guid id);
        Task<Chat> AddChatAsync(Chat chat);
        Task<bool> DeleteChatAsync(Guid id);
        Task<bool> UpdateChatAsync(Chat chat);
    }
}
