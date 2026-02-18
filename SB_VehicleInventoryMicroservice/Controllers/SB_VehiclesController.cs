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
    }
}
