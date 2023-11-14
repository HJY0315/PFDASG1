using System.Data.SqlClient;

namespace PFDASG1.DAL
{
    public class TransactionsDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public TransactionsDAL()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("OCBCConnectionString");
            conn = new SqlConnection(strConn);
        }


    }
}
