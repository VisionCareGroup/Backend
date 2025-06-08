namespace VisionCareCore.HealthCare.Interfaces.Resources;

public class CreateMedicineResource
{
    public string Nombre { get; set; }
    public string Description { get; set; }
    public string SideEffects { get; set; }
    public string Warnings { get; set; }
    public Guid UserId { get; set; }
    public int? Instruccions { get; set; }
}