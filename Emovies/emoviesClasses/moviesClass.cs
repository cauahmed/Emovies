using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Emovies.emoviesClasses
{
    class moviesClass
    {
        //Getter and Setter Properties
        //Will act as the data carrier in our app
        public int MovieID { get; set; }
        public string MovieName { get; set; }
        public string MovieGenre { get; set; }
        public float IMDBRating { get; set; }
        public string Description { get; set; }
        public string Platform { get; set; }

        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        //Selecting Data from Database
        public DataTable Select()
        {
            //Step 1: Database Connection
            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                //Step 2: Writing SQL Query
                string sql = "SELECT * FROM Movies";
                //Creating cmd
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Creating SQL DataAdapter
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                conn.Open();
                adapter.Fill(dt);
                
            }
            catch
            {

            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
        //Inserting data into Database
        public bool Insert (moviesClass m)
        {
            //Creating a default return type and setting value to false
            bool isSuccess = false;

            //Step 1: Connect Database
            SqlConnection conn = new SqlConnection(myconnstrng);
            //Check Database Connection
            m.Checkconnection(conn);
            try
            {
                //Step 2: Create SQL Query to insert Data
                string sql = "INSERT INTO Movies (MovieName, MovieGenre, IMDBRating, Description, Platform) VALUES (@MovieName, @MovieGenre, @IMDBRating, @Description, @Platform)";
                //Creating SQL Command
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Creating Paramaters to add data
                cmd.Parameters.AddWithValue("@MovieName", m.MovieName);
                cmd.Parameters.AddWithValue("@MovieGenre", m.MovieGenre);
                cmd.Parameters.AddWithValue("@IMDBRating", m.IMDBRating);
                cmd.Parameters.AddWithValue("@Description", m.Description);
                cmd.Parameters.AddWithValue("@Platform", m.Platform);

                //Open connection to Database
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                //if the query runs successfully then the value of rows will be greater than 0 and less than 0 otherwise
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }
            }
            catch
            {

            }
            finally
            {
                conn.Close();
            }

            return isSuccess;


        }

        //Method to update data in our application database

        public bool Update(moviesClass m)
        {
            //Default return type and set value to false
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(myconnstrng);
            try
            {
                //SQL to update data in database
                string sql = "Update Movies SET MovieName=@MovieName, MovieGenre=@MovieGenre, IMDBRating=@IMDBRating, Description=@Description, Platform=@Platform WHERE MovieID=@MovieID";
                //Creating SQL Command
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Parameters for updating value
                cmd.Parameters.AddWithValue("@MovieName", m.MovieName);
                cmd.Parameters.AddWithValue("@MovieGenre", m.MovieGenre);
                cmd.Parameters.AddWithValue("@IMDBRating", m.IMDBRating);
                cmd.Parameters.AddWithValue("@Description", m.Description);
                cmd.Parameters.AddWithValue("@Platform", m.Platform);
                cmd.Parameters.AddWithValue("@MovieID", m.MovieID);

                //Open Database Connection
                conn.Open();

                int rows = cmd.ExecuteNonQuery();
                //if the query runs successfully then the value of rows will be greater than 0 and less than 0 otherwise
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }

            }
            catch
            {

            }
            finally
            {
                conn.Close();
            }
            return isSuccess;
        }

        //Method to delete Data from Database

        public bool Delete(moviesClass m)
        {
            //Create a default return value
            bool isSuccess = false;
            //Create SQL Connection
            SqlConnection conn = new SqlConnection(myconnstrng);
            m.Checkconnection(conn);

            try
            {
                //SQL to delete Data
                string sql = "DELETE FROM Movies WHERE MovieID=@MovieID";
                //Create SQL Command
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Create MovieID Parameter for deletion
                cmd.Parameters.AddWithValue("@MovieID", m.MovieID);
                //Open Database Connection
                conn.Open();

                int rows = cmd.ExecuteNonQuery();
                //if the query runs successfully then the value of rows will be greater than 0 and less than 0 otherwise
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }
            }
            catch
            {

            }
            finally
            {
                //close connection
                conn.Close();
            }
            return isSuccess;
        }

        public void Checkconnection(SqlConnection conn)
        {
            if (conn == null && conn.State == ConnectionState.Closed)
            {
                MessageBox.Show("Failed to open db connection");
            }
            else
            {
                MessageBox.Show("Successfully connected to db");
            }
        }

       

    }
}
