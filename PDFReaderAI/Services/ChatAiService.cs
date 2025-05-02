using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using PDFReaderAI.Services;
using UglyToad.PdfPig;

namespace PDFReaderAI.Services
{
    public interface IChatAIService
    {
        Task<string> GetResponseAsync(string[] prompts, string[] responses, string newPrompt, byte[]? fileContent = null, string? aiModel=null);
    }


    public class ChatAIService : IChatAIService
    {

        private readonly HttpClient _httpClient;

        private string? ExtractTextFromPdf(byte[] fileContent)
        {
            try
            {
                using var pdfDocument = PdfDocument.Open(fileContent);
                var text = string.Join("\n", pdfDocument.GetPages().Select(page => page.Text));
                return text;
            }
            catch
            {
                // Dacă fișierul nu este un PDF valid sau apare o eroare, returnăm null
                return null;
            }
        }

        public ChatAIService(HttpClient httpClient)
        {
            // BaseAddress trebuie setat în DI ca "http://localhost:11434"
            _httpClient = httpClient;
        }

        public async Task<string> GetResponseAsync(
           string[] prompts,
           string[] responses,
           string newPrompt,
           byte[]? fileContent = null,
           string aiModel = "neural-chat")
        {
            // Asigură că prompts și responses au aceeași dimensiune
            if (prompts.Length != responses.Length)
                throw new ArgumentException("Prompts and responses must have the same length.");

            // Construiește lista de mesaje
            var messages = new List<object>
    {
        new { role = "system", content = "You are a helpful assistant." }
    };

            for (int i = 0; i < prompts.Length; i++)
            {
                messages.Add(new { role = "user", content = prompts[i] });
                messages.Add(new { role = "assistant", content = responses[i] });
            }

            if (fileContent != null)
            {
                var pdfText = ExtractTextFromPdf(fileContent);
                if (!string.IsNullOrEmpty(pdfText))
                {
                    messages.Add(new { role = "system", content = $"The following PDF content is provided for context:\n{pdfText}" });
                }
            }

            messages.Add(new { role = "user", content = newPrompt });

            var payload = new
            {
                model = aiModel, // Folosim modelul specificat
                messages = messages,
                stream = false
            };

            var response = await _httpClient.PostAsJsonAsync("/api/chat", payload);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Loghează răspunsul pentru depanare
            Console.WriteLine("JSON Response: " + jsonResponse);

            if (string.IsNullOrEmpty(jsonResponse))
            {
                throw new InvalidOperationException("The API response is empty.");
            }

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InvalidOperationException($"API returned an error: {response.StatusCode} - {jsonResponse}");
            }

            using var doc = JsonDocument.Parse(jsonResponse);

            // Accesează direct obiectul "message" și extrage "content"
            if (doc.RootElement.TryGetProperty("message", out var message) &&
                message.TryGetProperty("content", out var contentProperty))
            {
                var content = contentProperty.GetString();
                return content ?? string.Empty;
            }

            throw new InvalidOperationException("Invalid JSON structure: Missing 'message' or 'content'.");
        }




    }
}
