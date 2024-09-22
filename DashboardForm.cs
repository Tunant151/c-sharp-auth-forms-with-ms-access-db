using System;
using System.Windows.Forms;
using System.Data.OleDb; // For MS Access Database connection
using System.Data;

namespace c_sharp_auth_forms_with_ms_access_db
{
    public partial class DashboardForm : Form
    {
        // Connection to MS Access Database using .mdb file in bin\Debug folder
        private OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=UserDatabase.mdb");
        private string loggedInUsername; // Store the logged-in username

        public DashboardForm(string username)
        {
            InitializeComponent();
            loggedInUsername = username;
            LoadDashboardData();  // Load data on form load
        }

        // Method to load data into the DataGridView
        private void LoadDashboardData()
        {
            // Show only the username
            lblUser.Text = loggedInUsername;

            // SQL query to get all users
            string query = "SELECT Username, Email, [Password] FROM Users";

            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbDataReader reader = command.ExecuteReader();

                // Initialize a DataTable to hold the data
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);

                // Set the data source of the DataGridView to the DataTable
                dataGridViewUsers.DataSource = dataTable;

                // Set the total users label
                lblTotalUsers.Text = dataTable.Rows.Count.ToString();

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        // Logout button functionality
        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Go back to the login form
            LoginForm loginForm = new LoginForm();
            this.Hide();
            loginForm.Show();
        }

        // Close application when clicking close button
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
