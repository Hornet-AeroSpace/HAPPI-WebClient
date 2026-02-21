namespace HAPPI.Shared;

// An identical version of this exists in both HAPPI-Server and HAPPI-Client, please keep them in sync.
public record TelemetryData(
    long Timestamp,          // Unix Tick
    string MissionState,     // Current mission status.  E.g. "Pad Idle", "Ascent", "Landed"
    double Altitude,         // Altitude            (meters)
    double VelocityZ,        // Vertical Velocity   (m/s)
    float AccelG,            // G-Force 
    float Pitch,             // X Orientation       (degrees)
    float Roll,              // Y Orientation       (degrees)
    float Yaw,               // Z Orientation       (degrees)
    double BatteryVoltageRocket,    // Battery Voltage for the rocket
    double BatteryVoltageGround    // Battery Voltage for the ground station
);