namespace Industrial.Monitoring.Api.Models;

public record MachineProfile(
    string Id,
    string Name,
    double Rpm,
    double Temperature,
    double Pressure
);

    

