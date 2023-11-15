using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Transactions;
using PFDASG1.Models;

namespace PFDASG1.DAL
{
    public class TransactionsDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        private Transaction transaction;

        public TransactionsDAL()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("OCBCConnectionString");
            conn = new SqlConnection(strConn);
        }

        public int add(Transactions transaction)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO Transactions (transactionId, description, accountid, amount, transactionDate, receiverId)
                                OUTPUT INSERTED.MemberID
                                VALUES(@transactionId, @description, @accountid, @amount, @transactionDate, @receiverId)"
            ;

            cmd.Parameters.AddWithValue("@transactionId", transaction.TransactionId);
            cmd.Parameters.AddWithValue("@description", transaction.Description);
            cmd.Parameters.AddWithValue("@accountid", transaction.AccountId);
            cmd.Parameters.AddWithValue("@transactionDate", transaction.TransactionDate);
            cmd.Parameters.AddWithValue("@receiverId", transaction.ReceipientOrSenderID);
            //A connection to database must be opened before any operations made.   
            conn.Open();

            transaction.TransactionId = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return transaction.TransactionId;
        }
    }
}
