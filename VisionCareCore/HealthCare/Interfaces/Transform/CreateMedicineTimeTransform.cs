using VisionCareCore.HealthCare.Domain.Model.Commands;
using VisionCareCore.HealthCare.Domain.Model.ValueObjects;
using VisionCareCore.HealthCare.Interfaces.Resources;

namespace VisionCareCore.HealthCare.Interfaces.Transform;

public static class CreateMedicineTimeTransform
{
    public static CreateMedicineTimeCommand ToCommand(CreateMedicineTimeResource resource)
    {
        return new CreateMedicineTimeCommand(
            resource.MedicineId,
            (Foods)resource.Foods, 
            resource.SpecificTime,
            (Interval?)resource.Interval 
        );
    }
}