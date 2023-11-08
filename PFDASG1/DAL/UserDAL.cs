using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using PFDASG1.Models;

namespace PFDASG1.DAL
{
    public class UserDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public UserDAL()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("OCBCConnectionString");
            conn = new SqlConnection(strConn);
        }

        public User Login(string access_code, string Pin)
        {
            if (access_code == null || Pin == null)
            {
                return null;
            }
            User user = new User();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Users WHERE Access_Code = @access_code AND Pin = @Pin";
            cmd.Parameters.AddWithValue("@Access_Code", access_code);
            cmd.Parameters.AddWithValue("@Pin", Pin);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    user.Userid = reader.GetInt32(0);
                    user.Name = reader.GetString(1);
                    user.AccessCode = reader.GetString(2);
                    user.phoneNumber = reader.GetString(3);
                    user.Pin = reader.GetString(4);
                    

                    reader.Close();
                    conn.Close();
                    return user;
                }
            }
            reader.Close();
            conn.Close();
            return null;
        }
    }
}
