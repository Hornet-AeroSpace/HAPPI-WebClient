using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

// This program connects to the Hornet Aerospace and Propulsion's HAPI API,
// retrieves rocket flight data, and saves it into a local JSON file.
// It assumes the API is running at https://localhost:5029.

public class HAPIClient
{
    private static readonly HttpClient client;
    public static string ApiBaseUrl = "http://10.3.141.1:5097";

    // Static constructor to initialize the HttpClient with a handler that bypasses SSL validation.
    static HAPIClient()
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        client = new HttpClient(handler);
    }

    // Define a record to match the structure of the API's response
    public record LiveAltitude(int[] altitudeIn)
    {
        public int Altitude => altitudeIn.Length > 0 ? altitudeIn[0] : 0;
    }

    public static HttpClient WebClient => client;
}
