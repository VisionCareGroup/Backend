using VisionCareCore.HealthCare.Domain.Model.Entities;

namespace VisionCareCore.HealthCare.Domain.Model.Aggregates
{
    public class Medicine
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; }
        public string? Description { get; private set; }
        public string? SideEffects { get; private set; }
        public string? Warnings { get; private set; }
        public int? Instruccions { get; private set; } 
        public bool IsDeleted { get; private set; }

        public Guid UserId { get; private set; }

        // Relación 1:N con tiempos de recordatorio
        private readonly List<MedicineTime> _medicineTimes = new();
        public IReadOnlyCollection<MedicineTime> MedicineTimes => _medicineTimes;

        public Medicine(
            string nombre,
            string description,
            string sideEffects,
            string warnings,
            Guid userId,
            int? instruccions = null)
        {
            Id = Guid.NewGuid();
            Nombre = nombre;
            Description = description;
            SideEffects = sideEffects;
            Warnings = warnings;
            Instruccions = instruccions;
            IsDeleted = false;
            UserId = userId;
        }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }

        public void UpdateInfo(string nombre, string description, string sideEffects, string warnings, int? instruccions = null)
        {
            Nombre = nombre;
            Description = description;
            SideEffects = sideEffects;
            Warnings = warnings;
            Instruccions = instruccions;
        }

        // Métodos de dominio para manejar tiempos
        public void AddMedicineTime(MedicineTime time)
        {
            _medicineTimes.Add(time);
        }

        public void RemoveMedicineTime(Guid timeId)
        {
            var item = _medicineTimes.FirstOrDefault(mt => mt.Id == timeId);
            if (item != null)
                _medicineTimes.Remove(item);
        }
    }
}
