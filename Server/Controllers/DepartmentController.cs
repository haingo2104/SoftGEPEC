using Microsoft.AspNetCore.Mvc;
using Server.Data.Entities;
using Server.Repositories;

namespace Server.Controllers {
    [ApiController]
    [Route("/api/departments")]
    public class DepartmentController : ControllerBase {
        private readonly DepartmentRepository _departmentRepository;

        public DepartmentController(DepartmentRepository departmentRepository) {
            _departmentRepository = departmentRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetDepartments() {
            var departments = await _departmentRepository.GetDepartments();

            return Ok(departments);
        }

         [HttpPost("")]
        public async Task<IActionResult> AddDepartment(DepartmentToAdd departmentToAdd) {
            await _departmentRepository.AddDepartment(departmentToAdd);
            return Ok();
        }
    }
}
