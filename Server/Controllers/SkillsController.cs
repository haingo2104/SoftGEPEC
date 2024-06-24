
using Microsoft.AspNetCore.Mvc;
using Server.Data.Entities;
using Server.Repositories;

namespace Server.Controllers {
    [ApiController]
    [Route("api/skills")]

    public class SkillsController : ControllerBase {
        private readonly SkillsRepository _skillsRepository;
        public SkillsController(SkillsRepository skillsRepository) {
            _skillsRepository = skillsRepository;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetSkills() {
            var skills = await _skillsRepository.GetSkills();

            return Ok(skills);
        }

        [HttpGet("grouped")]
        public async Task<IActionResult> GetSkillsGrouped() {
            var skillsByGroup = await _skillsRepository.GetSkillsByGroup();
            return Ok(skillsByGroup);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddSkills(SkillsToAdd skillsToAdd) {
            var currentUserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            if (currentUserId == null) {
                return Unauthorized();
            }
            await _skillsRepository.AddSkills(skillsToAdd, Convert.ToInt64(currentUserId));
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSkills(string id) {
            var currentUserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            if (currentUserId == null) {
                return Unauthorized();
            }

            await _skillsRepository.DeleteSkills(Convert.ToInt64(id), Convert.ToInt64(currentUserId));
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSkills(string id, SkillsToUpdate skillsToUpdate) {
            var currentUserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            if (currentUserId == null) {
                return Unauthorized();
            }

            await _skillsRepository.UpdateSkills(Convert.ToInt64(id), skillsToUpdate);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSkillsById(string id) {
            var skill = await _skillsRepository.GetSkillsById(Convert.ToInt64(id));
            if (skill == null) {
                return NotFound();
            }
            return Ok(skill);
        }
    }
}

