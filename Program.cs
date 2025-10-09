using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
public class Program
{
    public static async Task Main(string[] args)
    {
        // Set the base address of the API
        HAPIClient.WebClient.BaseAddress = new Uri(HAPIClient.ApiBaseUrl);

        try
        {
            while (true)
            {
                Console.WriteLine($"Attempting to connect to API at: {HAPIClient.ApiBaseUrl}/weatherforecast");

                // Make a GET request to the weatherforecast endpoint
                HttpResponseMessage response = await HAPIClient.WebClient.GetAsync("altitude");

                // Check if the request was successful
                response.EnsureSuccessStatusCode();

                // Read the response content as a string (JSON)
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Save the raw JSON response to a local file
                // await File.WriteAllTextAsync(Blackbox.OutputFilePath, jsonResponse);
                // Console.WriteLine($"Successfully saved API response to '{Blackbox.OutputFilePath}'.");

                // Optionally, deserialize the JSON to show parsing
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Ensures matching properties regardless of case
                };

                Console.WriteLine("\n--- Raw JSON Response ---");
                Console.WriteLine(jsonResponse);
                Console.WriteLine("-------------------------\n");
                List<HAPIClient.LiveAltitude>? forecasts = null;

                try
                {
                    // First attempt: expected shape is an array of objects matching LiveAltitude
                    forecasts = JsonSerializer.Deserialize<List<HAPIClient.LiveAltitude>>(jsonResponse, options);
                }
                catch (JsonException)
                {
                    // Fallback: the API sometimes returns a simple array of ints like [529]
                    try
                    {
                        // Try deserializing as an array of ints
                        var intList = JsonSerializer.Deserialize<List<int>>(jsonResponse, options);
                        if (intList != null)
                        {
                            forecasts = intList.Select(i => new HAPIClient.LiveAltitude(new int[] { i })).ToList();
                        }
                        else
                        {
                            // Also handle a single integer value (not in an array)
                            var singleInt = JsonSerializer.Deserialize<int?>(jsonResponse, options);
                            if (singleInt.HasValue)
                            {
                                forecasts = new List<HAPIClient.LiveAltitude> { new HAPIClient.LiveAltitude(new int[] { singleInt.Value }) };
                            }
                        }
                    }
                    catch (JsonException)
                    {
                        // Leave forecasts null; we'll report no parsed data below
                    }
                }

                if (forecasts != null && forecasts.Count > 0)
                {
                    Console.WriteLine("\n--- Parsed Altitude ---");
                    foreach (var forecast in forecasts)
                    {
                        Console.WriteLine($"Altitude: {forecast.Altitude} feet");
                    }
                    Console.WriteLine("------------------------\n");
                }
                else
                {
                    Console.WriteLine("No parsed altitude data was produced from the response.");
                }
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"\nRequest error: {e.Message}");
            Console.WriteLine("Please ensure your local API is running and the URL is correct.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"An unexpected error occurred: {e.Message}");
        }
    }
    }