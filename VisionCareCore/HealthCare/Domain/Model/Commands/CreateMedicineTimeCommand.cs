using VisionCareCore.HealthCare.Domain.Model.ValueObjects;

namespace VisionCareCore.HealthCare.Domain.Model.Commands;

public record CreateMedicineTimeCommand(
    Guid MedicineId,
    Foods Foods,
    TimeOnly? SpecificTime,
    Interval? Interval
);