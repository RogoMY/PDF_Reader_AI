using Microsoft.EntityFrameworkCore;
using PDFReaderAI.Models;

namespace PDFReaderAI.Data
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {
        }
        public DbSet<Chat> Chats { get; set; }
      
    }
}
