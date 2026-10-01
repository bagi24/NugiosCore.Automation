using System.Text;
using System.Text.Json;

namespace Api.Tests.NugiosCore;

public static class N8nWebhookReporter
{
    private static readonly HttpClient HttpClient = new();
    
    // ჩასვი შენი n8n Production Webhook URL
    private const string WebhookUrl = "https://bagrati.app.n8n.cloud/webhook/daa3fc87-1827-426a-9eb1-c8a9ecdfb2ac"; 

    public static async Task SendFailureAlertAsync(string testName, string errorMessage, string stackTrace)
    {
        var payload = new
        {
            testName = testName,
            environment = "Staging",
            status = "FAILED",
            errorMessage = errorMessage,
            stackTrace = stackTrace
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            await HttpClient.PostAsync(WebhookUrl, content);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Webhook Error] Failed to send log to n8n: {ex.Message}");
        }
    }
}