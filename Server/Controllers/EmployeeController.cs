using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Data.Entities;
using Server.Repositories;

namespace Server.Controllers {
    [ApiController]
    [Route("/api/employees")]
    public class EmployeeController : ControllerBase {
        private readonly EmployeeRepository _employeeRepository;

        public EmployeeController(EmployeeRepository employeeRepository) {
            _employeeRepository = employeeRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetServices() {
            var employees = await _employeeRepository.GetEmployees();

            return Ok(employees);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddEmployee(EmployeeToAdd employeeToAdd) {
            var currentUserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            if (currentUserId == null) {
                return Unauthorized();
            }
            await _employeeRepository.AddEmployee(employeeToAdd, Convert.ToInt64(currentUserId));
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id) {
            var currentUserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            if (currentUserId == null) {
                return Unauthorized();
            }

            await _employeeRepository.DeleteEmployee(Convert.ToInt64(id), Convert.ToInt64(currentUserId));
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(string id, EmployeeToUpdate employeeToUpdate) {
            var currentUserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            if (currentUserId == null) {
                return Unauthorized();
            }

            await _employeeRepository.UpdateEmployee(Convert.ToInt64(id), employeeToUpdate);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(string id) {
            var employee = await _employeeRepository.GetEmployeeById(Convert.ToInt64(id));
            if (employee == null) {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpGet("by-service/{serviceId}")]
        public async Task<IActionResult> GetEmployeeByServiceId(string serviceId) {
            var employees = await _employeeRepository.GetEmployeesByService(serviceId);
            return Ok(employees);
        }

        [HttpGet("by-department/{departmentId}")]
        public async Task<IActionResult> GetEmployeesByDepartmentId(string departmentId)
        {
            var employees = await _employeeRepository.GetEmployeesByDepartment(departmentId);
            return Ok(employees);
        }

    }
}
