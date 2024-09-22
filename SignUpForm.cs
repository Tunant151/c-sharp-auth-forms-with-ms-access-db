using System;
using System.Windows.Forms;
using System.Data.OleDb; // For MS Access Database connection

namespace c_sharp_auth_forms_with_ms_access_db
{
    public partial class SignUpForm : Form
    {
        // Connection to MS Access Database using .mdb file in bin\Debug folder
        private OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=UserDatabase.mdb");

        public SignUpForm()
        {
            InitializeComponent();
        }

        // Show or hide password in both password fields when checkbox is checked or unchecked
        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPassword.Checked)
            {
                txtPassword.PasswordChar = '\0';  // Show password
                txtConfirmPassword.PasswordChar = '\0'; // Show confirm password
            }
            else
            {
                txtPassword.PasswordChar = '*';   // Hide password
                txtConfirmPassword.PasswordChar = '*';  // Hide confirm password
            }
        }

        // Method to check if the username already exists
        private bool IsUsernameTaken(string username)
        {
            string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @username";
            OleDbCommand checkCommand = new OleDbCommand(checkQuery, connection);
            checkCommand.Parameters.AddWithValue("@username", username);

            int count = (int)checkCommand.ExecuteScalar(); // Get the count of users with the same username

            return count > 0;  // If count > 0, username exists
        }

        // Sign Up Button functionality
        private void btnSignUp_Click(object sender, EventArgs e)
        {
            // Get input from textboxes
            string username = txtUsername.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // Validate input
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            try
            {
                connection.Open();

                // Check if the username already exists
                if (IsUsernameTaken(username))
                {
                    MessageBox.Show("The username is already taken. Please choose a different username.");
                    return; // Stop the process if username exists
                }

                // SQL query to insert new user into database
                string query = "INSERT INTO Users (Username, Email, [Password]) VALUES (@username, @Email, @Password)";
                OleDbCommand command = new OleDbCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);

                command.ExecuteNonQuery();  // Execute insert query

                MessageBox.Show("Account created successfully!");

                // Navigate to LoginForm
                LoginForm loginForm = new LoginForm();
                this.Hide();
                loginForm.Show();
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

        // Click event for login prompt (already have an account)
        private void lblLoginPrompt_Click(object sender, EventArgs e)
        {
            // Navigate back to the LoginForm
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
