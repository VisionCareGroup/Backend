namespace VisionCareCore.HealthCare.Interfaces.Resources;

public class CreateMedicineTimeResource
{
    public Guid MedicineId { get; set; }
    public int Foods { get; set; } // Enum (Breakfast = 0, etc.)
    public TimeOnly? SpecificTime { get; set; }
    public int? Interval { get; set; } // Enum (Every4Hours = 0, etc.)
}