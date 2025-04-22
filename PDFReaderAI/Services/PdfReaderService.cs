using UglyToad.PdfPig;

namespace PDFReaderAI.Services
{
    public interface IPdfReaderService
    {
        string ReadPdfContent(byte[] fileContent);
    }

    public class PdfReaderService : IPdfReaderService
    {
        public string ReadPdfContent(byte[] fileContent)
        {
            using var pdfDocument = PdfDocument.Open(fileContent);
            var text = string.Join("\n", pdfDocument.GetPages().Select(page => page.Text));
            return text;
        }
    }
}
