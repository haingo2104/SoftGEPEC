using Microsoft.Data.SqlClient;
using Server.Data.Entities;
using Server.Utils;

namespace Server.Repositories {
    public class EmployeeRepository {
        private readonly string _connectionString;
        public EmployeeRepository(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("localDB")!;
        }
        public async Task<List<Employee>> GetEmployees() {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(@"
               SELECT 
                    e.id, 
                    e.lastName, 
                    e.firstName, 
                    e.entry_date, 
                    e.confirmation_date, 
                    e.service_id, 
                    e.created_by,
                    s.title AS service_title
                FROM 
                    employees AS e
                JOIN 
                    services AS s ON e.service_id = s.id
                WHERE 
                    e.deletion_date IS NULL;
            ", conn);

            using var reader = await cmd.ExecuteReaderAsync();

            var res = new List<Employee>();

            while (await reader.ReadAsync()) {
                res.Add(new Employee {
                    Id = reader["id"].ToString()!,
                    LastName = reader["lastName"].ToString()!,
                    FirstName = reader["firstName"].ToString()!,
                    Entry_date = reader["entry_date"].ToString()!,
                    Confirmation_date = reader["confirmation_date"].ToString()!,
                    Service_id = reader["service_id"].ToString()!,
                    Created_by = reader["created_by"].ToString()!,
                    ServiceTitle = reader["service_title"].ToString()!

                });
            }

            return res;
        }

        public async Task<Employee?> GetEmployeeById(long id) {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(@"
               SELECT 
                    e.id, 
                    e.lastName, 
                    e.firstName, 
                    e.entry_date, 
                    e.confirmation_date, 
                    e.service_id, 
                    e.created_by,
                    s.title AS service_title
                FROM 
                    employees AS e
                JOIN 
                    services AS s ON e.service_id = s.id
                WHERE 
                    e.id = @id AND
                    e.deletion_date IS NULL;
            ", conn);

            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync()) {
                return new Employee {
                    Id = reader["id"].ToString()!,
                    LastName = reader["lastName"].ToString()!,
                    FirstName = reader["firstName"].ToString()!,
                    Entry_date = reader["entry_date"].ToString()!,
                    Confirmation_date = reader["confirmation_date"].ToString()!,
                    Service_id = reader["service_id"].ToString()!,
                    Created_by = reader["created_by"].ToString()!,
                    ServiceTitle = reader["service_title"].ToString()!
                };
            }

            return null;
        }
        public async Task AddEmployee(EmployeeToAdd employeeToAdd, long created_by) {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(@"
                    INSERT INTO employees (id, lastName, firstName , entry_date , confirmation_date , service_id , created_by) 
                    VALUES (@id, @lastName, @firstName, @entry_date , @confirmation_date , @service_id , @created_by);", conn);

            var id = ID.GenerateID();

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@lastName", employeeToAdd.LastName);
            cmd.Parameters.AddWithValue("@firstName", employeeToAdd.FirstName);
            cmd.Parameters.AddWithValue("@entry_date", employeeToAdd.Entry_date);
            cmd.Parameters.AddWithValue("@confirmation_date", employeeToAdd.Confirmation_date);
            cmd.Parameters.AddWithValue("@service_id", employeeToAdd.Service_id);
            cmd.Parameters.AddWithValue("@created_by", created_by);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteEmployee(long employeeId, long deletedBy) {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(@"
    UPDATE employees SET deletion_date = GETUTCDATE(), deleted_by = @deleted_by
        WHERE id = @id;
    ", conn);

            cmd.Parameters.AddWithValue("@id", employeeId);
            cmd.Parameters.AddWithValue("@deleted_by", deletedBy);

            await cmd.ExecuteNonQueryAsync();
        }


        public async Task UpdateEmployee(long employeeId, EmployeeToUpdate employeeToUpdate) {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(@"
        UPDATE employees 
        SET lastName = @lastName,
            firstName = @firstName,
            entry_date = @entry_date,
            confirmation_date = @confirmation_date,
            service_id = @service_id
        WHERE id = @id
    ", conn);

            cmd.Parameters.AddWithValue("@id", employeeId);
            cmd.Parameters.AddWithValue("@lastName", employeeToUpdate.LastName);
            cmd.Parameters.AddWithValue("@firstName", employeeToUpdate.FirstName);
            cmd.Parameters.AddWithValue("@entry_date", employeeToUpdate.Entry_date);
            cmd.Parameters.AddWithValue("@confirmation_date", employeeToUpdate.Confirmation_date);
            cmd.Parameters.AddWithValue("@service_id", employeeToUpdate.Service_id);

            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<List<Employee>> GetEmployeesByService(string serviceId) {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(@"
        SELECT e.id, 
               e.lastName, 
               e.firstName, 
               e.entry_date, 
               e.confirmation_date, 
               e.service_id, 
               e.created_by,
               s.title AS service_title
        FROM employees AS e
        JOIN services AS s ON e.service_id = s.id
        WHERE e.deletion_date IS NULL
          AND s.id = @serviceId
          AND s.deletion_date IS NULL;
    ", conn);

            cmd.Parameters.AddWithValue("@serviceId", serviceId);

            using var reader = await cmd.ExecuteReaderAsync();

            var res = new List<Employee>();

            while (await reader.ReadAsync()) {
                res.Add(new Employee {
                    Id = reader["id"].ToString()!,
                    LastName = reader["lastName"].ToString()!,
                    FirstName = reader["firstName"].ToString()!,
                    Entry_date = reader["entry_date"].ToString()!,
                    Confirmation_date = reader["confirmation_date"].ToString()!,
                    Service_id = reader["service_id"].ToString()!,
                    Created_by = reader["created_by"].ToString()!,
                    ServiceTitle = reader["service_title"].ToString()!
                });
            }

            return res;
        }

        public async Task<List<Employee>> GetEmployeesByDepartment(string departmentId) {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(@"
            SELECT e.id, 
                   e.lastName, 
                   e.firstName, 
                   e.entry_date, 
                   e.confirmation_date, 
                   e.service_id, 
                   e.created_by,
                   s.title AS service_title
            FROM employees AS e
            JOIN services AS s ON e.service_id = s.id
            JOIN departments AS d ON s.department_id = d.id
            WHERE e.deletion_date IS NULL
              AND d.id = @departmentId
              AND s.deletion_date IS NULL
              AND d.deletion_date IS NULL;
        ", conn);

            cmd.Parameters.AddWithValue("@departmentId", departmentId);

            using var reader = await cmd.ExecuteReaderAsync();

            var res = new List<Employee>();

            while (await reader.ReadAsync()) {
                res.Add(new Employee {
                    Id = reader["id"].ToString()!,
                    LastName = reader["lastName"].ToString()!,
                    FirstName = reader["firstName"].ToString()!,
                    Entry_date = reader["entry_date"].ToString()!,
                    Confirmation_date = reader["confirmation_date"].ToString()!,
                    Service_id = reader["service_id"].ToString()!,
                    Created_by = reader["created_by"].ToString()!,
                    ServiceTitle = reader["service_title"].ToString()!
                });
            }

            return res;
        }

    }
}
