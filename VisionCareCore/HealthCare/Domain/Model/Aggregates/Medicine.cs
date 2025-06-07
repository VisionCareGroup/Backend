namespace VisionCareCore.HealthCare.Domain.Model.Aggregates;

public class Medicine
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; }
    public string Description { get; private set; }
    public string SideEffects { get; private set; }
    public string Warnings { get; private set; }
    public bool IsDeleted { get; private set; }


    public Guid UserId { get; private set; }
    public Guid MedicineTimeId { get; private set; }


    public Medicine(string nombre, string description, string sideEffects, string warnings, Guid userId, Guid medicineTimeId)
    {
        Id = Guid.NewGuid();
        Nombre = nombre;
        Description = description;
        SideEffects = sideEffects;
        Warnings = warnings;
        IsDeleted = false;
        UserId = userId;
        MedicineTimeId = medicineTimeId;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }

    public void UpdateInfo(string nombre, string description, string sideEffects, string warnings)
    {
        Nombre = nombre;
        Description = description;
        SideEffects = sideEffects;
        Warnings = warnings;
    }
}