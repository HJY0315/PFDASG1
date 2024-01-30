using PFDASG1.Models;
using System.Data.SqlClient;
using System.Security;

namespace PFDASG1.DAL
{
    public class CardActivationDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        private static readonly Random random = new Random();

        public CardActivationDAL()
        {
            //Read ConnectionString from appsettings.json file
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

        public int cardVerification(CreditCard cardinfo, out string message)
        {
            message = "";
            int cardID = 0;

            string concatenatedCardNumber = $"{cardinfo.cardNumber1}{cardinfo.cardNumber2}";

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement 
            cmd.CommandText = @"SELECT * FROM CreditCards";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end
            while (reader.Read())
            {
                string last8DigitsOfCardNumber = reader.GetString(2).Substring(Math.Max(0, reader.GetString(2).Length - 8));

                if ((last8DigitsOfCardNumber == concatenatedCardNumber) &&
                (reader.GetString(6).ToLower() == cardinfo.cardHolderName.ToLower()) &&
                (reader.GetDateTime(3).Month == cardinfo.expirationMonth) && (reader.GetDateTime(3).Year == cardinfo.expirationYear) &&
                (reader.GetString(4) == cardinfo.cvv))
                {
                    if (reader.GetString(5) == "Unactivated")
                    {
                        message = "";
                        cardID = reader.GetInt32(0);
                        break;
                    }
                    else
                    {
                        message = "Card had been activated before. Please try again.";
                        break;
                    }
                }
                else
                {
                    message = "Invalid card information.";
                }
            }

            conn.Close();
            return cardID;
        }

        public bool UpdateCardStatus(int cardID)
        {
            bool updateSuccessful = false;

            try
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE CreditCards SET cardStatus = @cardStatus WHERE creditCardId = @creditCardID";

                    cmd.Parameters.AddWithValue("@cardStatus", "Activated");
                    cmd.Parameters.AddWithValue("@creditCardID", cardID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();

                    // Check if the update was successful (at least one row affected)
                    updateSuccessful = rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                updateSuccessful = false;
            }

            return updateSuccessful;
        }

        public bool VerifySecurityQuestionAnswer(string userID, CreditCard cardinfo, string question, out string message)
        {
            bool success = false;
            message = "";
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement 
            cmd.CommandText = "SELECT securityQuestion, answer FROM Users WHERE userId = @userId";
            cmd.Parameters.AddWithValue("@userId", userID);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            // Read all the records (if exist)
            while (reader.Read())
            {
                if ((reader.GetString(0) == question) && (reader.GetString(1).ToLower() == cardinfo.answer.ToLower()))
                {
                    success = true;
                }
                else
                {
                    message = "Verification unsuccessful. Please try again.";
                    break;
                }
            }

            conn.Close();
            return success;
        }
    }
}
