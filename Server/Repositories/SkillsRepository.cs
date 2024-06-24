using Microsoft.Data.SqlClient;
using Server.Data.Entities;
using Server.Utils;

namespace Server.Repositories {
    public class SkillsRepository {
        private readonly string _connectionString;
        public SkillsRepository(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("localDB")!;
        }
        public async Task<List<Skills>> GetSkills() {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(@"
        SELECT s.id, s.title, s.skills_group_id, g.title AS skillsGroupTitle, s.created_by
        FROM skills AS s
        JOIN skills_groups AS g ON s.skills_group_id = g.id
        WHERE s.deletion_date IS NULL;
    ", conn);

            using var reader = await cmd.ExecuteReaderAsync();

            var res = new List<Skills>();

            while (await reader.ReadAsync()) {
                res.Add(new Skills {
                    Id = reader["id"].ToString()!,
                    Title = reader["title"].ToString()!,
                    Skills_group_id = reader["skills_group_id"].ToString()!,
                    Created_by = reader["created_by"].ToString()!,
                    SkillsGroupTitle = reader["skillsGroupTitle"].ToString()!
                });
            }

            return res;
        }
        public async Task AddSkills(SkillsToAdd skillsToAdd, long created_by) {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(@"
        INSERT INTO skills (id,title, skills_group_id, created_by)
        VALUES (@id, @title, @skills_group_id, @created_by);
    ", conn);
            var id = ID.GenerateID();
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@title", skillsToAdd.Title);
            cmd.Parameters.AddWithValue("@skills_group_id", skillsToAdd.Skills_group_id);
            cmd.Parameters.AddWithValue("@created_by", created_by);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteSkills(long skillsId, long deletedBy) {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(@"
    UPDATE skills SET deletion_date = GETUTCDATE(), deleted_by = @deleted_by
        WHERE id = @id;
    ", conn);

            cmd.Parameters.AddWithValue("@id", skillsId);
            cmd.Parameters.AddWithValue("@deleted_by", deletedBy);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateSkills(long skillsId, SkillsToUpdate skillsToUpdate) {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(@"
        UPDATE skills 
        SET title = @title,
            skills_group_id = @skills_group_id
        WHERE id = @id
    ", conn);

            cmd.Parameters.AddWithValue("@id", skillsId);
            cmd.Parameters.AddWithValue("@title", skillsToUpdate.Title);
            cmd.Parameters.AddWithValue("@skills_group_id", skillsToUpdate.Skills_group_id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Dictionary<string, List<Skills>>> GetSkillsByGroup() {
            var skillsByGroup = new Dictionary<string, List<Skills>>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(@"
                SELECT s.id, s.title, s.skills_group_id, g.title AS skillsGroupTitle, s.created_by
                FROM skills AS s
                JOIN skills_groups AS g ON s.skills_group_id = g.id
                WHERE s.deletion_date IS NULL;
            ", conn);

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync()) {
                var skill = new Skills {
                    Id = reader["id"].ToString()!,
                    Title = reader["title"].ToString()!,
                    Skills_group_id = reader["skills_group_id"].ToString()!,
                    Created_by = reader["created_by"].ToString()!,
                    SkillsGroupTitle = reader["skillsGroupTitle"].ToString()!
                };

                if (!skillsByGroup.ContainsKey(skill.Skills_group_id)) {
                    skillsByGroup[skill.Skills_group_id] = new List<Skills>();
                }

                skillsByGroup[skill.Skills_group_id].Add(skill);
            }

            return skillsByGroup;
        }
        public async Task<Skills?> GetSkillsById(long id) {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(@"
               SELECT 
                    s.id, 
                    s.title, 
                    s.skills_group_id,
                    g.title AS skillsGroupTitle,
                    s.created_by
                FROM 
                    skills AS s
                JOIN skills_groups AS g ON s.skills_group_id = g.id
                WHERE 
                    s.id = @id AND
                    s.deletion_date IS NULL;
            ", conn);

            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync()) {
                return new Skills {
                    Id = reader["id"].ToString()!,
                    Title = reader["title"].ToString()!,
                    Skills_group_id = reader["skills_group_id"].ToString()!,
                    Created_by = reader["created_by"].ToString()!,
                    SkillsGroupTitle = reader["skillsGroupTitle"].ToString()!
                };
            }

            return null;
        }

    }
}