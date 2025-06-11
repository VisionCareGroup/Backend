using VisionCareCore.HealthCare.Domain.Model.Commands;
using VisionCareCore.HealthCare.Domain.Model.Entities;
using VisionCareCore.HealthCare.Domain.Repositories;
using VisionCareCore.HealthCare.Domain.Services;
using VisionCareCore.Shared.Domain.Repositories;

namespace VisionCareCore.HealthCare.Application.Internal.CommandServices;

public class MedicineTimeCommandService : IMedicineTimeCommandService
{
    private readonly IMedicineTimeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public MedicineTimeCommandService(IMedicineTimeRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateMedicineTimeCommand command)
    {
        var entity = new MedicineTime
        {
            Id = Guid.NewGuid(),
            MedicineId = command.MedicineId,
            Foods = command.Foods,
            SpecificTime = command.SpecificTime,
            Interval = command.Interval
        };

        await _repository.AddAsync(entity);
        await _unitOfWork.CompleteAsync();

        return entity.Id;
    }

    public async Task SoftDelete(Guid id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null) throw new Exception("MedicineTime not found");

        item.IsDeleted = true;
        await _repository.UpdateAsync(item);
        await _unitOfWork.CompleteAsync();
    }
}