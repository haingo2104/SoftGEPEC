using Microsoft.AspNetCore.Mvc;
using Server.Data.Entities;
using Server.Repositories;

namespace Server.Controllers {
    [ApiController]
    [Route("api/skillsGroup")]

    public class SkillsGroupController : ControllerBase {
        private readonly SkillsGroupRepository _skillsGroupRepository;

        public SkillsGroupController(SkillsGroupRepository skillsGroupRepository) {
            _skillsGroupRepository = skillsGroupRepository;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetSkillsGroup() {
            var skillsGroups = await _skillsGroupRepository.GetSkillsGroup();

            return Ok(skillsGroups);
        }
        // [HttpPost("")]
        // public async Task<IActionResult> AddSkillsGroup(SkillsGroupToAdd skillsGroupToAdd) {
        //     var currentUserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
        //     if (currentUserId == null) {
        //         return Unauthorized();
        //     }
        //     await _skillsGroupRepository.AddSkillsGroup(skillsGroupToAdd,Convert.ToInt64(currentUserId));
        //     return Ok();
        // }
        [HttpPost("")]
        public async Task<IActionResult> AddSkillsGroup(SkillsGroupToAdd skillsGroupToAdd) {
            var currentUserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            if (currentUserId == null) {
                return Unauthorized();
            }
            await _skillsGroupRepository.AddSkillsGroup(skillsGroupToAdd, Convert.ToInt64(currentUserId));
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSkillsGroup(string id, SkillsGroupToUpdate skillsGroupToUpdate) {
            var currentUserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            if (currentUserId == null) {
                return Unauthorized();
            }

            await _skillsGroupRepository.UpdateSkillsGroup(Convert.ToInt64(id), skillsGroupToUpdate);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSkillsGroup(string id) {
            var currentUserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            if (currentUserId == null) {
                return Unauthorized();
            }

            await _skillsGroupRepository.DeleteSkillsGroup(Convert.ToInt64(id), Convert.ToInt64(currentUserId));
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSkillsGroupById(string id) {
            var skillsGroup = await _skillsGroupRepository.GetSkillsGroupById(Convert.ToInt64(id));
            if (skillsGroup == null) {
                return NotFound();
            }
            return Ok(skillsGroup);
        }
    }
}