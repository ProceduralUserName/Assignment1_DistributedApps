using Microsoft.AspNetCore.Mvc;
using SB_Application.DTOs;
using SB_Application.Services;
using SB_Domain.Exceptions;

namespace SB_VehicleInventoryMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SB_VehiclesController : ControllerBase
    {
        private readonly SB_VehicleService _vehicleService;

        public SB_VehiclesController(SB_VehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SB_VehicleDto>>> GetAll()
        {
            var vehicles = await _vehicleService.GetAllVehiclesAsync();
            return Ok(vehicles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SB_VehicleDto>> GetById(Guid id)
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicle == null)
                return NotFound();

            return Ok(vehicle);
        }

        [HttpPost]
        public async Task<ActionResult<SB_VehicleDto>> Create(SB_CreateVehicleDto dto)
        {
            var vehicle = await _vehicleService.CreateVehicleAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult<SB_VehicleDto>> UpdateStatus(Guid id, SB_UpdateVehicleStatusDto dto)
        {
            try
            {
                var vehicle = await _vehicleService.UpdateVehicleStatusAsync(id, dto);
                return Ok(vehicle);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (SB_InvalidVehicleStateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _vehicleService.DeleteVehicleAsync(id);
            return NoContent();
        }
    }
}
