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

        public bool Login(string accesscode, string pin, out string Name, out int id)
        {
            Name = "null";
            id = 0;
            bool authenticated = false;
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement 
            cmd.CommandText = @"SELECT * FROM Users";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end
            while (reader.Read())
            {
                // Convert email address to lowercase for comparison
                // Password comparison is case-sensitive
                if ((reader.GetString(2) == accesscode) && (reader.GetString(4) == pin))
                {
                    authenticated = true;
                    Name = reader.GetString(1);
                    id = reader.GetInt32(0);
                    break; // Exit the while loop
                }
            }
            return authenticated;
        }



        //public User Login(string accessCode, string Pin, out string Name, out int id)
        //{
        //    //if (accessCode == null || Pin == null)
        //    //{
        //    //    return null;
        //    //}
        //    Name = "null";
        //    id = 0;
        //    User user = new User();
        //    SqlCommand cmd = conn.CreateCommand();
        //    cmd.CommandText = @"SELECT * FROM Users WHERE AccessCode = @accessCode AND Pin = @Pin";
        //    cmd.Parameters.AddWithValue("@AccessCode", accessCode);
        //    cmd.Parameters.AddWithValue("@Pin", Pin);
        //    conn.Open();
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    if (reader.HasRows)
        //    {
        //        while (reader.Read())
        //        {
        //            user.Userid = reader.GetInt32(0);
        //            user.Name = reader.GetString(1);
        //            user.AccessCode = reader.GetString(2);
        //            user.phoneNumber = reader.GetString(3);
        //            user.Pin = reader.GetString(4);
        //            user.NRIC = reader.GetString(5);

        //            reader.Close();
        //            conn.Close();
        //            return user;
        //        }
        //    }
        //    reader.Close();
        //    conn.Close();
        //    return null;
        //}
    }
}
