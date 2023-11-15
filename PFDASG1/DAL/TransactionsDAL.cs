using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Transactions;
using PFDASG1.Models;
using System.Data.SqlClient;

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

        public int Transactions(Transactions transaction)
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
        public List<Transactions> getalltransactions(int userid)
        {

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement 
            cmd.CommandText = @"SELECT
    t.transactionId,
    t.description,
    t.accountId
    t.amount,
    t.transactionDate,
    t.receiverId

FROM
    Transactions AS t
INNER JOIN
    Accounts AS a
ON
    t.accountId = a.accountId
WHERE
    a.userId = @userId
ORDER BY
    t.transactionDate DESC;";
            cmd.Parameters.AddWithValue("@userID", userid);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            List<Transactions> transactionslist = new List<Transactions>();
            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    transactionslist.Add(
                    new Transactions
                    {
                        TransactionId = reader.GetInt32(0),
                        Description = reader.GetString(1),
                        AccountId = reader.GetInt32(2),
                        Amount = reader.GetDecimal(3),
                        TransactionDate = reader.GetDateTime(4),
                        ReceipientOrSenderID = reader.GetInt32(5)

                    }
                    );
                }
                reader.Close();
                conn.Close();
                return transactionslist;
            }
            else
            {
                // No rows found, return an empty list
                reader.Close();
                conn.Close();
                return new List<Transactions>();
            }
        }
    }
}
