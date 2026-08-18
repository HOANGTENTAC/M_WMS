using Microsoft.Data.SqlClient;
using System.Data;

namespace M_WMS.Services
{
    public class SqlServerService
    {
#if !DEBUG
        string serverIp = "192.168.40.52";
        string dbName = "ENVNDIVDB";
        string user = "sa";
        string pass = "t-net";
#else
        string serverIp = "192.168.40.254\\SQLEXPRESS";
        string dbName = "ENVNDIVDB";
        string user = "sa";
        string pass = "t-net";
#endif
        private string GetConnectionString(string serverIp, string databaseName, string user, string password)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = serverIp,               // IP hoặc Domain Server của khu vực (VD: 192.168.1.10 hoặc sql.khuvuc1.com)
                InitialCatalog = databaseName,       // Tên DB khu vực
                UserID = user,
                Password = password,
                TrustServerCertificate = true,       // Bỏ qua lỗi chứng chỉ SSL nếu xài IP / SSL tự ký
                ConnectTimeout = 15                  // Thời gian chờ kết nối (giây)
            };
            return builder.ConnectionString;
        }

        // Hàm lấy dữ liệu từ DB của khu vực tương ứng
        //public async Task<DataTable> GetDataFromRegionAsync(string serverIp, string dbName, string user, string pass, string query)
        public async Task<DataTable> GetDataFromRegionAsync(string query)
        {
            DataTable dataTable = new DataTable();
            string connectionString = GetConnectionString(serverIp, dbName, user, pass);

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi kết nối (Timeout, sai IP, nghẽn mạng...)
                System.Diagnostics.Debug.WriteLine($"Lỗi kết nối SQL Khu vực: {ex.Message}");
                throw;
            }

            return dataTable;
        }
    }
}
