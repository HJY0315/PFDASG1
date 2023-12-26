using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Transactions;
using PFDASG1.Models;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

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
        //    // Regular expression pattern for validating phone numbers
        //    const string phoneNumberRegexPattern = @"^\d{10}$";

        //    // Create a Regex object using the pattern
        //    Regex phoneNumberRegex = new Regex(phoneNumberRegexPattern);

        //    // Match the input phone number against the pattern
        //    return phoneNumberRegex.IsMatch(phoneNumber);
        //}
        public User GetUserFromPhoneNumber(string phoneNumber)
        {
            // Open a connection to the database
            conn.Open();

            // Create a SqlCommand object to retrieve the account ID
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Users u JOIN Accounts a ON u.userId = a.userId WHERE u.phoneNumber = @phoneNumber";
            cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber); // Add the phone number to the query

            // Execute the query and retrieve the account ID
            //User user = (User)cmd.ExecuteReaderAsync();

            // Close the connection to the database
            //conn.Close();

            SqlDataReader reader = cmd.ExecuteReader();

            User user = new User();
            while (reader.Read())
            {
                user.Userid = reader.GetInt32(0);
                user.Name = reader.GetString(1);
                user.AccessCode = reader.GetString(2);
                user.phoneNumber = reader.GetString(3);
                user.Pin = reader.GetString(4);
                user.NRIC = reader.GetString(5);
            }

            reader.Close();
            conn.Close();
            // Return the retrieved account ID
            return user;
        }

        public List<User> GetUsersByPhoneNumber(string phoneNumber)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT Userid, Name, AccessCode, phoneNumber, Pin, NRIC FROM Users WHERE phoneNumber LIKE '%' + @phoneNumber + '%'";
            cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            List<User> userList = new List<User>();

            while (reader.Read())
            {
                userList.Add(new User
                {
                    Userid = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    AccessCode = reader.GetString(2),
                    phoneNumber = reader.GetString(3),
                    Pin = reader.GetString(4),
                    NRIC = reader.GetString(5)
                });
            }

            // Close reader and connection after use
            reader.Close();
            conn.Close();

            return userList;
        }

        
        public List<User> GetAllUser()
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT Userid, Name, AccessCode, phoneNumber, Pin, NRIC FROM Users Order by userId";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            List<User> userList = new List<User>();
            while (reader.Read())
            {
                userList.Add(new User
                {
                    Userid = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    AccessCode = reader.GetString(2),
                    phoneNumber = reader.GetString(3),
                    Pin = reader.GetString(4),
                    NRIC = reader.GetString(5)
                });

            }

            reader.Close();
            conn.Close();

            return userList;
        }




        //public decimal GetAccountBalance(int senderId)
        //{
        //    // Open a connection to the database
        //    conn.Open();

        //    // Create a SqlCommand object to retrieve the account balance
        //    SqlCommand cmd = conn.CreateCommand();
        //    cmd.CommandText = "SELECT accountBalance FROM Accounts WHERE userId = @senderId";
        //    cmd.Parameters.AddWithValue("@senderId", senderId);

        //    // Execute the query and retrieve the account balance
        //    //decimal accountBalance = (decimal)cmd.ExecuteScalar();
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    Account account = new Account();
        //    while (reader.Read())
        //    {
        //        account.accountBalance = reader.GetDecimal(2);
        //    }
        //    reader.Close();
        //    // Close the connection to the database
        //    conn.Close();

        //    // Return the retrieved account balance
        //    return account.accountBalance;
        //}

        public int Add(TransactionViewModel transactionViewModel)
        {
            //if (!IsValidPhoneNumber(num))
            //{
            //    throw new ArgumentException("Invalid recipient phone number format.");
            //}



            //if (transactionViewModel.Amount > GetAccountBalance(transactionViewModel.senderID))
            //{
            //    throw new InvalidOperationException("Insufficient funds for transaction.");
            //}

            conn.Open();
            SqlCommand deductCmd = conn.CreateCommand();
            deductCmd.CommandText = @"UPDATE Accounts SET accountBalance = accountBalance - @amount WHERE accountId = @senderId";
            deductCmd.Parameters.AddWithValue("@amount", transactionViewModel.Amount);
            deductCmd.Parameters.AddWithValue("@senderId", transactionViewModel.senderID);

            // Execute the UPDATE SQL statement
            deductCmd.ExecuteNonQuery();

            // Create a SQL statement to add the transaction amount to the recipient's account balance
            SqlCommand addCmd = conn.CreateCommand();
            addCmd.CommandText = @"UPDATE Accounts SET accountBalance = accountBalance + @amount WHERE accountId = @receiverId";
            addCmd.Parameters.AddWithValue("@amount", transactionViewModel.Amount);
            addCmd.Parameters.AddWithValue("@receiverId", transactionViewModel.receipient.Userid);

            // Execute the UPDATE SQL statement
            addCmd.ExecuteNonQuery();
            // Create a SqlCommand object to insert the transaction data
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Transactions ( description, accountId, amount, transactionDate, receiverId, senderId)
                            VALUES ( @description, @accountId, @amount, @transactionDate, @receiverId, @senderId)";


            // Add parameters with validated and sanitized values
            //cmd.Parameters.AddWithValue("@transactionId", transactionViewModel.TransactionId);
            cmd.Parameters.AddWithValue("@description", transactionViewModel.Description == null ? DBNull.Value : transactionViewModel.Description);
            cmd.Parameters.AddWithValue("@accountId", transactionViewModel.senderID);
            cmd.Parameters.AddWithValue("@amount", transactionViewModel.Amount);
            cmd.Parameters.AddWithValue("@transactionDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@receiverId", transactionViewModel.receipient.Userid);
            cmd.Parameters.AddWithValue("@senderId", transactionViewModel.senderID);

            // Execute the INSERT SQL statement
            cmd.ExecuteNonQuery();

            conn.Close();

            return transactionViewModel.TransactionId;
        }
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
                              (t.senderId = @id OR t.receiverId = @id)
                            ORDER BY
                              t.transactionDate DESC";
            cmd.Parameters.AddWithValue("@id", userid);
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
                        Description = reader.IsDBNull(1) ? null : reader.GetString(1), // Check if Description is null before setting it
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