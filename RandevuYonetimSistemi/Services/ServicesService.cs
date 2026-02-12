using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RandevuYonetimSistemi.Services
{
    public class ServicesService
    {
        private readonly string _connectionString;

        public ServicesService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddServicesAsync(int userId, List<string> services)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                foreach (var service in services)
                {
                    if (string.IsNullOrWhiteSpace(service)) continue;

                    string query = "INSERT INTO Services (UserId, ServiceName) VALUES (@uid, @name)";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@uid", userId);
                    cmd.Parameters.AddWithValue("@name", service);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task MarkSetupCompletedAsync(int userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = "UPDATE Users SET IsSetupCompleted = 1 WHERE UserId = @uid";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@uid", userId);
                await cmd.ExecuteNonQueryAsync();
            }
        }

    }
}
