using Emovies.emoviesClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Emovies
{
    public partial class Emovies : Form
    {
        public Emovies()
        {
            InitializeComponent();
        }
        moviesClass m = new moviesClass();

        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the value from the Search text box
            string keyword = txtSearch.Text;

            SqlConnection conn = new SqlConnection(myconnstrng);
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM Movies WHERE MovieName LIKE '%"+keyword+ "%' OR MovieGenre LIKE '%" + keyword + "%' OR Description LIKE '%" + keyword + "%'", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dgvMovieList.DataSource = dt;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Get the values from the input fields
            m.MovieName = txtMovieName.Text;
            m.MovieGenre = txtMovieGenre.Text;
            m.IMDBRating = float.Parse(txtIMDBRating.Text);
            m.Description = txtDescription.Text;
            m.Platform = cmbPlatform.Text;

            //Insert data into database using movieClass methods
            bool success = m.Insert(m);
            if(success == true)
            {
                //successfully added movie
                MessageBox.Show("New movie successfully inserted");
                //call the clear method here
                Clear();
            }
            else
            {
                //failed to add movie
                MessageBox.Show("Failed to add new movie. Try Again");
            }
            //Load Data on Data Gridview
            DataTable dt = m.Select();
            dgvMovieList.DataSource = dt;
        }

        private void txtIMDBRating_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if(ch == 46 && txtIMDBRating.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch!=8 && ch!=46)
            {
                e.Handled = true;
            }
        }

        private void Emovies_Load(object sender, EventArgs e)
        {
            //Load Data on Data Gridview
            DataTable dt = m.Select();
            dgvMovieList.DataSource = dt;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Method to clear all fields
        public void Clear()
        {
            txtMovieID.Text = "";
            txtMovieName.Text = "";
            txtMovieGenre.Text = "";
            txtIMDBRating.Text = "";
            txtDescription.Text = "";
            cmbPlatform.Text = "";

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get the Data from textboxes
            m.MovieID = int.Parse(txtMovieID.Text);
            m.MovieName = txtMovieName.Text;
            m.MovieGenre = txtMovieGenre.Text;
            m.IMDBRating = float.Parse(txtIMDBRating.Text);
            m.Description = txtDescription.Text;
            m.Platform = cmbPlatform.Text;

            //Update data in Database
            bool success = m.Update(m);
            if(success == true)
            {
                //Updated Successfully
                MessageBox.Show("Movie has been successfully updated.");
                //Refresh Data on Data Gridview
                DataTable dt = m.Select();
                dgvMovieList.DataSource = dt;
                //Call clear method
                Clear();
            }
            else
            {
                //Failed to update data
                MessageBox.Show("Failed to update Movie.");
            }
        }

        private void dgvMovieList_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Get the Data from Data Grid View and load it in the textviews respectively
            //Identify the row which is clicked
            int rowIndex = e.RowIndex;
            txtMovieID.Text = dgvMovieList.Rows[rowIndex].Cells[0].Value.ToString();
            txtMovieName.Text = dgvMovieList.Rows[rowIndex].Cells[1].Value.ToString();
            txtMovieGenre.Text = dgvMovieList.Rows[rowIndex].Cells[2].Value.ToString();
            txtIMDBRating.Text = dgvMovieList.Rows[rowIndex].Cells[3].Value.ToString();
            txtDescription.Text = dgvMovieList.Rows[rowIndex].Cells[4].Value.ToString();
            cmbPlatform.Text = dgvMovieList.Rows[rowIndex].Cells[5].Value.ToString();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //Call the clear method
            Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Get MovieId from the application
            m.MovieID = Convert.ToInt32(txtMovieID.Text);
            bool success = m.Delete(m);
            if(success == true)
            {
                //Successfully Deleted Moview
                MessageBox.Show("Movie Successfully Deleted.");
                //Refresh Data on Data Gridview
                DataTable dt = m.Select();
                dgvMovieList.DataSource = dt;
                //Call clear method
                Clear();
            }
            else
            {
                //Failed to Delete Moview
                MessageBox.Show("Failed to delete Movie. Try Again.");
            }
        }
    }
}
