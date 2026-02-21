using System.Text.Json;
using HAPPI.Shared;

public static class Blackbox
{
    public static async Task LogAsync(string filepath, TelemetryData data)
    {
        string jsonLine = JsonSerializer.Serialize(data) + Environment.NewLine;
        await File.AppendAllTextAsync(filepath, jsonLine);
    }
}