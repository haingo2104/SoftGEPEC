using Microsoft.Data.SqlClient;
using Server.Data.Entities;
using Server.Utils;

namespace Server.Repositories {
    public class ServiceRepository {
        private readonly string _connectionString;
        public ServiceRepository(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("localDB")!;
        }
        public async Task<List<Service>> GetServices() {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(@"
                SELECT s.id, s.title, s.department_id, d.title AS department_title, s.created_by
                FROM services AS s
                JOIN departments AS d ON s.department_id = d.id
                WHERE s.deletion_date IS NULL;
            ", conn);

            using var reader = await cmd.ExecuteReaderAsync();

            var res = new List<Service>();

            while (await reader.ReadAsync()) {
                res.Add(new Service {
                    Id = reader["id"].ToString()!,
                    Title = reader["title"].ToString()!,
                    Department_id = reader["department_id"].ToString()!,
                    Created_by = reader["created_by"].ToString()!,
                    DepartmentTitle = reader["department_title"].ToString()!
                });
            }

            return res;
        }
        public async Task<List<Service>> GetServicesByDepartmentId(string departmentId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(@"
                SELECT s.id, s.title, s.department_id, d.title AS department_title, s.created_by
                FROM services AS s
                JOIN departments AS d ON s.department_id = d.id
                WHERE s.department_id = @DepartmentId AND s.deletion_date IS NULL;
            ", conn);

            cmd.Parameters.AddWithValue("@DepartmentId", departmentId);

            using var reader = await cmd.ExecuteReaderAsync();

            var res = new List<Service>();

            while (await reader.ReadAsync())
            {
                res.Add(new Service
                {
                    Id = reader["id"].ToString()!,
                    Title = reader["title"].ToString()!,
                    Department_id = reader["department_id"].ToString()!,
                    Created_by = reader["created_by"].ToString()!,
                    DepartmentTitle = reader["department_title"].ToString()!
                });
            }

            return res;
        }
        public async Task AddService(ServiceToAdd serviceToAdd , long created_by) {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(@"
                    INSERT INTO services (id, title, department_id , created_by) 
                    VALUES (@id, @title, @department_id, @created_by);", conn);

            var id = ID.GenerateID();

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@title", serviceToAdd.Title);
            cmd.Parameters.AddWithValue("@department_id", serviceToAdd.Department_id);
            cmd.Parameters.AddWithValue("@created_by", created_by);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}