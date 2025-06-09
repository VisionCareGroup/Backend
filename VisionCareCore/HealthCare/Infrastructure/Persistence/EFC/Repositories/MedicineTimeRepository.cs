using Microsoft.EntityFrameworkCore;
using VisionCareCore.HealthCare.Domain.Model.Entities;
using VisionCareCore.HealthCare.Domain.Repositories;
using VisionCareCore.Shared.Infraestructure.Persistences.EFC.Configuration;
using VisionCareCore.Shared.Infraestructure.Persistences.EFC.Repositories;

namespace VisionCareCore.HealthCare.Infrastructure.Persistence.EFC.Repositories;

public class MedicineTimeRepository : BaseRepository<MedicineTime>, IMedicineTimeRepository
{
    public MedicineTimeRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<MedicineTime>> GetByMedicineIdAsync(Guid medicineId)
    {
        return await Context.Set<MedicineTime>()
            .Where(mt => mt.MedicineId == medicineId && !mt.IsDeleted)
            .ToListAsync();
    }

    public async Task<MedicineTime?> GetByIdAsync(Guid id)
    {
        return await Context.Set<MedicineTime>()
            .FirstOrDefaultAsync(mt => mt.Id == id && !mt.IsDeleted);
    }
}