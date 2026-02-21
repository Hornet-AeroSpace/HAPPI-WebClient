using HAPPI.Shared;
using System.Net.Http.Json;

public class Program
{
    public const string version = "26.2.21A"; // Update this for each release, or automate it with a build script

    public static async Task Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"=== HAPPI GROUND CONTROL WEB SERVICE BUILD {version} ===");
        Console.ResetColor();

        // Ensure output directory exists
        Directory.CreateDirectory("logs");
        string logFile = $"logs/telemetry_{DateTime.Now:yyyyMMdd_HHmmss}.jsonl";

        using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(250)); // Poll 4 times a second
        
        while (await timer.WaitForNextTickAsync())
        {
            try
            {
                // Fetch strictly typed data
                var data = await HAPPIClient.Client.GetFromJsonAsync<TelemetryData>("telemetry");

                if (data != null)
                {
                    DisplayDashboard(data);
                    await Blackbox.LogAsync(logFile, data);
                }
            }
            catch (HttpRequestException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[!][{DateTime.Now:T}] Connection Lost to HAPPI Server...");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Critical Error: {ex.Message}");
            }
        }
    }

    static void DisplayDashboard(TelemetryData data)
    {
        Console.Clear();
        Console.WriteLine("=== HAPPI TELEMETRY DASHBOARD ===");
        Console.WriteLine($"Time:    {new DateTime(data.Timestamp):T}");
        Console.WriteLine($"State:   {data.MissionState}");
        Console.WriteLine("---------------------------------");
        Console.WriteLine($"Altitude   :    {data.Altitude:F2} m  ({data.Altitude * 3.28084:F2} ft)");
        Console.WriteLine($"Velocity   :    {data.VelocityZ:F2} m/s");
        Console.WriteLine($"Acceleratio:    {data.AccelG:F2} G");
        Console.WriteLine($"Rocket Batt:    {data.BatteryVoltageRocket:F1} V");
        Console.WriteLine($"Ground Batt:    {data.BatteryVoltageGround:F1} V");
        Console.WriteLine("---------------------------------");
        Console.WriteLine($"Pitch (X): {data.Pitch:000.0} | Roll (Y): {data.Roll:000.0} | Yaw (Z): {data.Yaw:000.0}");
    }
}