using System;
using System.Windows.Forms;
using System.Data.OleDb; // For MS Access Database connection

namespace c_sharp_auth_forms_with_ms_access_db
{
    public partial class LoginForm : Form
    {
        // Connection string to MS Access Database using .mdb file in bin\Debug folder
        private OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=UserDatabase.mdb");

        public LoginForm()
        {
            InitializeComponent();
        }

        // Close application when clicking close button
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Show or hide password when checkbox is checked or unchecked
        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPassword.Checked)
            {
                txtPassword.PasswordChar = '\0';  // Show password
            }
            else
            {
                txtPassword.PasswordChar = '*';   // Hide password
            }
        }

        // Login button functionality
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Get username and password from textboxes
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // SQL query to verify user credentials
            string query = "SELECT * FROM Users WHERE Username = ? AND [Password] = ?";

            // Try connecting and verifying the credentials
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);
                OleDbDataReader reader = command.ExecuteReader();

                // Check if the user exists
                if (reader.HasRows)
                {
                    // Successfully logged in
                    MessageBox.Show("Login successful!");

                    // Open the dashboard
                    DashboardForm dashboard = new DashboardForm(username);
                    this.Hide();  // Hide the login form
                    dashboard.Show();  // Show the dashboard form
                }
                else
                {
                    // Login failed
                    MessageBox.Show("Invalid username or password. Please try again.");
                }

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

        // Click event for SignUp label, it opens the SignUp form
        private void lblSignUpPrompt_Click(object sender, EventArgs e)
        {
            // Open SignUp form
            SignUpForm signUpForm = new SignUpForm();
            this.Hide();
            signUpForm.Show();
        }
    }
}
