using Microsoft.Data.SqlClient;
using Server.Data.Entities;
using Server.Utils;

namespace Server.Repositories {
    public class UserRepository {
        private readonly string _connectionString;
        private readonly PasswordRepository _passwordRepository;
        public UserRepository(IConfiguration configuration, PasswordRepository passwordRepository) {
            _connectionString = configuration.GetConnectionString("localDB")!;
            _passwordRepository = passwordRepository;
        }


        private async Task<User?> GetUserByEmail(string email) {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(@"
                SELECT u.id 
                FROM users AS u
                WHERE u.deletion_date IS NULL
                AND u.email = @email
            ", conn);

            cmd.Parameters.AddWithValue("@email", email);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync()) {
                return new User {
                    Id = reader["id"].ToString()!
                };
            }

            return null;
        }

        public async Task<User?> FindUser(LoginCredentials loginCredentials) {
            var user = await GetUserByEmail(loginCredentials.Email);

            if (user == null) {
                return null;
            }

            var currentPassword = await _passwordRepository.GetCurrentPassword(Convert.ToInt64(user.Id));

            if (currentPassword != null) {
                if (Password.IsValidPassword(loginCredentials.Password, currentPassword)) {
                    return new User {
                        Id = user.Id,
                    };
                }
            }

            return null;
        }

        public async Task AddUser(UserToAdd userToAdd) {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(@"
                INSERT INTO users (id, email, password) 
                VALUES (@id, @email, @password);
            ", conn);

            var id = ID.GenerateID();

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@email", userToAdd.Email);
            cmd.Parameters.AddWithValue("@password", Password.HashPassword(userToAdd.Password));

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<User>> GetUsers() {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(@"
                SELECT d.id, d.email
                FROM users AS d
                WHERE d.deletion_date IS NULL;
            ", conn);

            using var reader = await cmd.ExecuteReaderAsync();

            var res = new List<User>();

            while (await reader.ReadAsync()) {
                res.Add(new User {
                    Id = reader["id"].ToString()!
                });
            }

            return res;
        }


    }
}
