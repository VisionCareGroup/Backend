using Microsoft.EntityFrameworkCore;
using VisionCareCore.HealthCare.Domain.Model.Aggregates;
using VisionCareCore.HealthCare.Domain.Repositories;
using VisionCareCore.Shared.Infraestructure.Persistences.EFC.Configuration;
using VisionCareCore.Shared.Infraestructure.Persistences.EFC.Repositories;

namespace VisionCareCore.HealthCare.Infrastructure.Persistence.EFC.Repositories;

public class MedicineRepository : BaseRepository<Medicine>, IMedicineRepository
{
    public MedicineRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Medicine>> GetAllByUserIdAsync(Guid userId)
    {
        return await Context.Set<Medicine>()
            .Where(m => m.UserId == userId && !m.IsDeleted)
            .ToListAsync();
    }
}