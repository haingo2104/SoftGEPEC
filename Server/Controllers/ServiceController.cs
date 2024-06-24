using Microsoft.AspNetCore.Mvc;
using Server.Data.Entities;
using Server.Repositories;

namespace Server.Controllers {
    [ApiController]
    [Route("/api/services")]
    public class ServiceController : ControllerBase {
        private readonly ServiceRepository _serviceRepository;

        public ServiceController(ServiceRepository serviceRepository) {
            _serviceRepository = serviceRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetServices() {
            var services = await _serviceRepository.GetServices();

            return Ok(services);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddService(ServiceToAdd serviceToAdd) {
           var currentUserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            if (currentUserId == null) {
                return Unauthorized();
            }
            await _serviceRepository.AddService(serviceToAdd, Convert.ToInt64(currentUserId));
            return Ok();
        }
        [HttpGet("by-department/{departmentId}")]
        public async Task<IActionResult> GetServicesByDepartmentId(string departmentId) {
            var services = await _serviceRepository.GetServicesByDepartmentId(departmentId);
            return Ok(services);
        }
    }
}
