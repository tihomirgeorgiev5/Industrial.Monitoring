namespace Industrial.Monitoring.Api.Models;

public record TelemetryMessage(

     string MachineId,
     string Name,
     double Rpm,
     double Temperature,
     double Pressure,
     DateTime Timestamp
     );


