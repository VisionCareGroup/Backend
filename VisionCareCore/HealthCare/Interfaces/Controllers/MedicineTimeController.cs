using Microsoft.AspNetCore.Mvc;
using VisionCareCore.HealthCare.Domain.Services;
using VisionCareCore.HealthCare.Interfaces.Resources;
using VisionCareCore.HealthCare.Interfaces.Transform;

namespace VisionCareCore.HealthCare.Interfaces.Controllers;

[ApiController]
[Route("api/medicine-time")]
public class MedicineTimeController : ControllerBase
{
    private readonly IMedicineTimeCommandService _commandService;
    private readonly IMedicineTimeQueryService _queryService;

    public MedicineTimeController(
        IMedicineTimeCommandService commandService,
        IMedicineTimeQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

   
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMedicineTimeResource resource)
    {
        var command = CreateMedicineTimeTransform.ToCommand(resource);
        var id = await _commandService.Handle(command);
        return Ok(new { id });
    }

    
    [HttpGet("by-medicine/{medicineId}")]
    public async Task<IActionResult> GetByMedicineId(Guid medicineId)
    {
        var result = await _queryService.GetAllByMedicineIdAsync(medicineId);
        return Ok(result);
    }

   
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _queryService.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

 
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _commandService.SoftDelete(id);
        return NoContent();
    }
}