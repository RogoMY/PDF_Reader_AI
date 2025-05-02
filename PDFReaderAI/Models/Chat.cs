namespace PDFReaderAI.Models
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string[]? Prompts { get; set; }
        public string[]? Responses { get; set; }
        public DateTime TimeOfDiscussion { get; set; }
        public string? FileName { get; set; }
        public byte[]? FileContent { get; set; }
        public string? FileMimeType { get; set; }
    }
}
