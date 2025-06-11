using VisionCareCore.HealthCare.Domain.Model.Aggregates;
using VisionCareCore.HealthCare.Domain.Model.ValueObjects;

namespace VisionCareCore.HealthCare.Domain.Model.Entities;

public class MedicineTime
{
    public Guid Id { get; set; }
    public Guid MedicineId { get; set; }

    // Desayuno, almuerzo, cena, irrelevante
    public Foods Foods { get; set; }

    // Hora específica (opcional)
    public TimeOnly? SpecificTime { get; set; }

    // Intervalo de tiempo entre tomas (opcional)
    public Interval? Interval { get; set; }

    public bool IsDeleted { get; set; } = false;

    // Navegación inversa
    public Medicine Medicine { get; set; }
}