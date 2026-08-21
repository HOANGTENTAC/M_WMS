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
        static string serverIp = "192.168.40.254\\SQLEXPRESS";
        static string dbName = "ENVNDIVDB";
        static string user = "sa";
        static string pass = "t-net";
#endif
        private static readonly ConnectingString[] ERPDB_CONNECTION_STRINGS =
        {
            //new ConnectingString("HC", "DRIVER={SQL Server};server=192.168.40.52;database=ENVNDIVDB;uid=sa;pwd=t-net",
            //                           "DRIVER={SQL Server};server=192.168.40.52;database=ENVNDIVDB;uid=sa;pwd=t-net"),
            //new ConnectingString("HN", "DRIVER={SQL Server};server=192.168.40.52;database=ENHNDIVDB;uid=sa;pwd=t-net",
            //                           "DRIVER={SQL Server};server=192.168.40.52;database=ENHNDIVDB;uid=sa;pwd=t-net"),
            //new ConnectingString("TH", "DRIVER={SQL Server};server=192.168.40.52;database=ENTHDIVDB;uid=sa;pwd=t-net",
            //                           "DRIVER={SQL Server};server=192.168.40.52;database=ENTHDIVDB;uid=sa;pwd=t-net"),
            //new ConnectingString("KZ", "DRIVER={SQL Server};server=192.168.2.5;database=TEST_JPDIVDB;uid=sa;pwd=",
            //                           "DRIVER={SQL Server};server=192.168.2.5;database=TEST_JPDIVDB;uid=sa;pwd="),

            // JP
            new ConnectingString("KZ", "Data Source=192.168.2.5;Initial Catalog=JPDIVDB;User ID=cpos_user;Password=kB2Ru5;Connect Timeout=15;Trust Server Certificate=True",
                                       "Data Source=dell_stn155v;Initial Catalog=test_SHDIVDB;User ID=sa;Password=;Connect Timeout=15;Trust Server Certificate=True"),
            // SH貿易
            //new ConnectingString("02", "DRIVER={SQL Server};server=192.168.60.253;database=SHTDDIVDB;uid=cpos_user;pwd=kB2Ru5",
            //                           "DRIVER={SQL Server};server=dell_stn155v;database=test_SHTDDIVDB;uid=sa;pwd="),
            //new ConnectingString("SH", "DRIVER={SQL Server};server=192.168.60.253;database=SHDIVDB;uid=cpos_user;pwd=kB2Ru5",
            //                           "DRIVER={SQL Server};server=dell_stn155v;database=test_SHTDDIVDB;uid=sa;pwd="),
            // SH
            new ConnectingString("SH", "Data Source=192.168.60.253;Initial Catalog=SHDIVDB;User ID=cpos_user;Password=kB2Ru5;Connect Timeout=15;Trust Server Certificate=True",
                                       "Data Source=dell_stn155v;Initial Catalog=test_SHDIVDB;User ID=sa;Password=;Connect Timeout=15;Trust Server Certificate=True"),
            // HK ※調査中
            new ConnectingString("HK", "Data Source=192.168.999.999;Initial Catalog=ENTHKDIVDB;User ID=cpos_user;Password=kB2Ru5;Connect Timeout=15;Trust Server Certificate=True",
                                       "Data Source=dell_stn155v;Initial Catalog=test_SHDIVDB;User ID=sa;Password=;Connect Timeout=15;Trust Server Certificate=True"),
            // TH
            new ConnectingString("TH", "Data Source=192.168.90.54;Initial Catalog=ENTHDIVDB;User ID=cpos_user;Password=kB2Ru5;Connect Timeout=15;Trust Server Certificate=True",
                                       "Data Source=dell_stn155v;Initial Catalog=test_SHDIVDB;User ID=sa;Password=;Connect Timeout=15;Trust Server Certificate=True"),
            //// HC
            //new ConnectingString("HC", "DRIVER={SQL Server};server=192.168.40.254\\SQLEXPRESS;database=ENVNDIVDB;uid=cpos_user;pwd=kB2Ru5",
            //                           "DRIVER={SQL Server};server=dell_stn155v;database=test_SHDIVDB;uid=sa;pwd="),
            // HC
            new ConnectingString("HC", "Data Source=192.168.40.254\\SQLEXPRESS;Initial Catalog=ENVNDIVDB;User ID=sa;Password=t-net;Connect Timeout=15;Trust Server Certificate=True",
                                       "Data Source=192.168.40.52;Initial Catalog=ENVNDIVDB;User ID=sa;Password=t-net;Connect Timeout=15;Trust Server Certificate=True"),
            // QD
            new ConnectingString("QD", "Data Source=192.168.70.249;Initial Catalog=QSDIVDB;User ID=cpos_user;Password=kB2Ru5;Connect Timeout=15;Trust Server Certificate=True",
                                       "Data Source=dell_stn155v;Initial Catalog=test_SHDIVDB;User ID=sa;Password=;Connect Timeout=15;Trust Server Certificate=True"),
            // HN
            new ConnectingString("HN", "Data Source=192.168.30.250\\TNET;Initial Catalog=ENHNDIVDB;User ID=cpos_user;Password=kB2Ru5;Connect Timeout=15;Trust Server Certificate=True",
                                       "Data Source=dell_stn155v;Initial Catalog=test_ENHNDIVDB;User ID=sa;Password=;Connect Timeout=15;Trust Server Certificate=True"),
            // ID Indonesia
            new ConnectingString("ID", "Data Source=192.168.20.20\\SQLEXPRESS;Initial Catalog=ENINDONESIAPO;User ID=cpos_user;Password=kB2Ru5;Connect Timeout=15;Trust Server Certificate=True",
                                       "Data Source=192.168.20.20\\SQLEXPRESS;Initial Catalog=test_ENINDONESIAPO;User ID=cpos_user;Password=;Connect Timeout=15;Trust Server Certificate=True"),
        };
        private static string GetConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = serverIp,               // IP hoặc Domain Server của khu vực (VD: 192.168.1.10 hoặc sql.khuvuc1.com)
                InitialCatalog = dbName,       // Tên DB khu vực
                UserID = user,
                Password = pass,
                TrustServerCertificate = true,       // Bỏ qua lỗi chứng chỉ SSL nếu xài IP / SSL tự ký
                ConnectTimeout = 15                  // Thời gian chờ kết nối (giây)
            };
            return builder.ConnectionString;
        }

        // Hàm lấy dữ liệu từ DB của khu vực tương ứng
        //public async Task<DataTable> GetDataFromRegionAsync(string serverIp, string dbName, string user, string pass, string query)
        public static async Task<DataTable> GetDataFromRegionAsync(string query, string kyoten, int testFlg = 0)
        {
            DataTable dataTable = new DataTable();
            string connectionString = GetSqlDbConnectionString(kyoten, testFlg);
            //string connectionString = GetConnectionString();

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
        private static string GetSqlDbConnectionString(string kyotenCd, int testFlg = 0)
        {
            ConnectingString connecting = Array.Find(ERPDB_CONNECTION_STRINGS, c => c.KyotenCd == kyotenCd);
            return testFlg == 0 ? connecting.ProductionConnectionString : connecting.TestConnectionString;
        }
    }
    public class ConnectingString
    {
        /// <summary>
        /// 会社CD
        /// </summary>
        public string KyotenCd { get; private set; }

        /// <summary>
        /// 接続情報
        /// </summary>
        public string ProductionConnectionString { get; private set; }

        /// <summary>
        /// 接続情報（テスト用）
        /// </summary>
        public string TestConnectionString { get; private set; }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="companyCd"></param>
        /// <param name="productionString"></param>
        /// <param name="testString"></param>
        public ConnectingString(string companyCd, string productionString, string testString)
        {
            KyotenCd = companyCd;
            ProductionConnectionString = productionString;
            TestConnectionString = testString;
        }
    }
}
