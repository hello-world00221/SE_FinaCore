using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace FinaCoreIndustries
{
    public partial class registerAccount : Form
    {
        public registerAccount()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string connectionString = "Server = localhost; Database= finacore; Uid = root; Pwd = ;";

            string userName = txtUsername.Text;
            string Password = txtPassword.Text;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    string query = "INSERT INTO login_user (username, password) values (@username, @password)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        // Adding parameters to the query
                        cmd.Parameters.AddWithValue("@username", userName);
                        cmd.Parameters.AddWithValue("@password", Password);

                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Information has been save successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                
            }

        }

        private void registerAccount_Load(object sender, EventArgs e)
        {

        }
    }
}
