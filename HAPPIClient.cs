public static class HAPPIClient
{
    public static string ApiBaseUrl = "http://localhost:5097"; // Update IP as needed
    public static readonly HttpClient Client;

    static HAPPIClient()
    {
        var handler = new HttpClientHandler();
        // Very insecure, but idgaf. Katie hired me to get this working, not to do pen testing. -Port
        handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        
        Client = new HttpClient(handler)
        {
            BaseAddress = new Uri(ApiBaseUrl),
            Timeout = TimeSpan.FromSeconds(2) // Fail fast if radio is down
        };
    }
}