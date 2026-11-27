namespace Industrial.Monitoring.Api.Models
{
    public static class MachineCatalog
    {
        public static readonly MachineProfile[] All =
        {
            new MachineProfile("1", "Machine 1", 1000, 55, 5.2),
            new MachineProfile("2", "Machine 2", 1200, 60, 6.4),
            new MachineProfile("3", "Machine 3", 900, 48, 4.8)
        };
    }
}
