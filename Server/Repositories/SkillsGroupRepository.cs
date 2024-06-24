using Microsoft.Data.SqlClient;
using Server.Data.Entities;
using Server.Utils;

namespace Server.Repositories {
    public class SkillsGroupRepository {
        private readonly string _connectionString;

        public SkillsGroupRepository(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("localDB")!;
        }

        public async Task<List<SkillsGroup>> GetSkillsGroup() {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(@"
                SELECT s.id, s.title, s.created_by
                FROM skills_groups AS s
                WHERE s.deletion_date IS NULL;
            ", conn);

            using var reader = await cmd.ExecuteReaderAsync();

            var res = new List<SkillsGroup>();

            while (await reader.ReadAsync()) {
                res.Add(new SkillsGroup {
                    Id = reader["id"].ToString()!,
                    Title = reader["title"].ToString()!,
                    Created_by = reader["created_by"].ToString()!,
                });
            }

            return res;
        }

        public async Task AddSkillsGroup(SkillsGroupToAdd skillsGroupToAdd, long created_by) {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(@"
            INSERT INTO skills_groups (id, title, created_by) 
            VALUES (@id, @title, @created_by);", conn);

            var id = ID.GenerateID();

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@title", skillsGroupToAdd.Title);
            cmd.Parameters.AddWithValue("@created_by", created_by);

            await cmd.ExecuteNonQueryAsync();

        }


        public async Task UpdateSkillsGroup(long skillsGroupId, SkillsGroupToUpdate skillsGroupToUpdate) {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(@"
        UPDATE skills_groups 
        SET title = @title
        WHERE id = @id
    ", conn);

            cmd.Parameters.AddWithValue("@id", skillsGroupId);
            cmd.Parameters.AddWithValue("@title", skillsGroupToUpdate.Title);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteSkillsGroup(long skillsGroupId, long deletedBy) {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(@"
    UPDATE skills_groups SET deletion_date = GETUTCDATE(), deleted_by = @deleted_by
        WHERE id = @id;
    ", conn);

            cmd.Parameters.AddWithValue("@id", skillsGroupId);
            cmd.Parameters.AddWithValue("@deleted_by", deletedBy);

            await cmd.ExecuteNonQueryAsync();
        }

         public async Task<SkillsGroup?> GetSkillsGroupById(long id) {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(@"
               SELECT 
                    s.id, 
                    s.title, 
                    s.created_by
                FROM 
                    skills_groups AS s
                WHERE 
                    s.id = @id AND
                    s.deletion_date IS NULL;
            ", conn);

            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync()) {
                return new SkillsGroup {
                    Id = reader["id"].ToString()!,
                    Title = reader["title"].ToString()!,
                    Created_by = reader["created_by"].ToString()!,
                };
            }

            return null;
        }
    }
}

