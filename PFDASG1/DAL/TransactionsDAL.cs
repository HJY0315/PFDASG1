using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Transactions;
using PFDASG1.Models;
using System.Data.SqlClient;
using System.Data;

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

        //private bool IsValidPhoneNumber(string phoneNumber)
        //{
        //    // Validate the format and length of the phone number
        //    // ...
        //}

        //public int Add(Transactions transaction, int senderId)
        //{
        //    if (!IsValidPhoneNumber(transaction.RecipientID))
        //    {
        //        throw new ArgumentException("Invalid recipient phone number format.");
        //    }

        //    // Retrieve recipient's account ID using the validated phone number
        //    int recipientId = GetAccountIdFromPhoneNumber(transaction.RecipientID);

        //    // Verify that the sender has sufficient funds to make the transfer
        //    if (transaction.Amount > GetAccountBalance(senderId))
        //    {
        //        throw new InvalidOperationException("Insufficient funds for transaction.");
        //    }

        //    // Create a SqlCommand object from the connection object
        //    SqlCommand cmd = conn.CreateCommand();

        //    // Specify an INSERT SQL statement
        //    cmd.CommandText = @"INSERT INTO Transactions (transactionId, description, accountId, amount, transactionDate, receiverId, senderId)
        //                OUTPUT INSERTED.MemberID
        //                VALUES (@transactionId, @description, @accountId, @amount, @transactionDate, @receiverId, @senderId)";

        //    // Add parameters with validated and sanitized values
        //    cmd.Parameters.AddWithValue("@transactionId", transaction.TransactionId);
        //    cmd.Parameters.AddWithValue("@description", transaction.Description);
        //    cmd.Parameters.AddWithValue("@accountId", senderId);
        //    cmd.Parameters.AddWithValue("@amount", transaction.Amount);
        //    cmd.Parameters.AddWithValue("@transactionDate", DateTime.Now);
        //    cmd.Parameters.AddWithValue("@receiverId", recipientId);
        //    cmd.Parameters.AddWithValue("@senderId", senderId);

        //    // Execute the INSERT SQL statement and retrieve the auto-generated transaction ID
        //    conn.Open();
        //    transaction.TransactionId = (int)cmd.ExecuteScalar();
        //    conn.Close();

        //    return transaction.TransactionId;
        //}
        public List<Transactions> getalltransactions(int userid)
        {

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement 
            cmd.CommandText = @"SELECT
   t.transactionId,
   t.description,
   t.accountId,
   t.amount,
   t.transactionDate,
   t.receiverId,
   t.senderId

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
                        RecipientID = reader.GetInt32(5),
                        SenderID = reader.GetInt32(6)

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