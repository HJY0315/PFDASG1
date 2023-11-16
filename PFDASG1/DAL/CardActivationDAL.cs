using PFDASG1.Models;
using System.Data.SqlClient;

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
                if ((reader.GetString(2) == cardinfo.cardNumber) &&
                (reader.GetString(6).ToLower() == cardinfo.cardHolderName.ToLower()) && (reader.GetDateTime(3) == cardinfo.expirationDate) &&
                (reader.GetString(4) == cardinfo.cvv) && (reader.GetString(8) == cardinfo.securityQuestion) &&
                (reader.GetString(9) == cardinfo.answer))
                {
                    if (reader.GetString(5) == "Unactivated")
                    {
                        cardID = Convert.ToInt32(reader.GetString(0));
                        break;
                    }
                    else
                    {
                        message = "Card had been activated before. Please try again.";
                    }
                }
            }
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
    }
}
