using Microsoft.Data.SqlClient;
using Server.Data.Entities;
using Server.Utils;

namespace Server.Repositories {
    public class DepartmentRepository {
        private readonly string _connectionString;

        public DepartmentRepository(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("localDB")!;
        }

        public async Task<List<Department>> GetDepartments() {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(@"
                SELECT d.id, d.title
                FROM departments AS d
                WHERE d.deletion_date IS NULL;
            ", conn);

            using var reader = await cmd.ExecuteReaderAsync();

            var res = new List<Department>();

            while (await reader.ReadAsync()) {
                res.Add(new Department {
                    Id = reader["id"].ToString()!,
                    Title = reader["title"].ToString()!
                });
            }

            return res;
        }

        public async Task AddDepartment(DepartmentToAdd departmentToAdd) {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(@"
                    INSERT INTO departments (id, title, created_by) 
                    VALUES (@id, @title, @created_by);", conn);

            var id = ID.GenerateID();

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@title", departmentToAdd.Title);
            cmd.Parameters.AddWithValue("@created_by", departmentToAdd.Created_by);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}


