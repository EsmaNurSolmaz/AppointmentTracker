using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RandevuYonetimSistemi.Services
{
    public class SectorsService
    {
        private readonly string _connectionString;

        public SectorsService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddSectorsAsync(int userId, List<string> sectors)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                foreach (var sector in sectors)
                {
                    if (string.IsNullOrWhiteSpace(sector)) continue;

                    string query = "INSERT INTO Sectors (UserId, SectorName) VALUES (@uid, @name)";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@uid", userId);
                    cmd.Parameters.AddWithValue("@name", sector);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
