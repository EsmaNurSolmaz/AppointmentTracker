using RandevuYonetimSistemi.Models;

public class AppointmentModel
{
    public int AppointmentId { get; set; }

    public int CreatedByUserId { get; set; }
    public int CustomerId { get; set; }

    public DateTime AppointmentDate { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
    public string? Service { get; set; }
    public DateTime CreatedAt { get; set; }

    public CustomerModel? Customer { get; set; }
}
