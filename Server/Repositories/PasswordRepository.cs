using Microsoft.Data.SqlClient;

namespace Server.Repositories {
    public class PasswordRepository {
        private readonly string _connectionString;

        public PasswordRepository(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("localDB")!;
        }

        public async Task<string?> GetCurrentPassword(long userId) {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(@"
                SELECT u.password
                FROM users AS u
                WHERE u.id = @id 
                AND u.deletion_date IS NULL 
            ", conn);

            cmd.Parameters.AddWithValue("@id", userId);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync()) {
                return reader["password"].ToString()!;
            }

            return null;
        }
    }
}