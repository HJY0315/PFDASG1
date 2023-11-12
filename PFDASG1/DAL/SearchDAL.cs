using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using PFDASG1.Models;

namespace PFDASG1.DAL
{
    public class SearchDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public SearchDAL()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "OCBCConnectionString");
            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }

        public List<Pages> GetAllPages()
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Pages Order by pageID";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            List<Pages> pagesList = new List<Pages>();
            while (reader.Read())
            {
                pagesList.Add(new Pages
                {
                    pageID = reader.GetInt32(0),
                    pageTitle = reader.GetString(1),
                    url = reader.GetString(2),
                });

            }

            reader.Close();
            conn.Close();

            return pagesList;
        }
    }
}
