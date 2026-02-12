namespace RandevuYonetimSistemi.Helpers
{
    public static class ConnectionStringConfig
    {
        public static string ConnectionString { get; } =
            @"Server=localhost;Database=AppointmentTrackingSystem;Trusted_Connection=True;TrustServerCertificate=True;";
    }
}
