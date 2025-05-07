using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

            string firstName = txtFname.Text;
            string middleName = txtMname.Text;
            string lastName = txtLname.Text;
            string userName = txtUsername.Text;
            string Password = txtPassword.Text;
            string role = comboBox1.Text;


            //Convert image to binary
            byte[] imageBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                imageBytes = ms.ToArray();
            }

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    string query = "INSERT INTO login_user (firstname, middlename, lastname, username, password, image, role) values (@firstname, @middlename, @lastname, @username, @password, @image, @role)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        // Adding parameters to the query
                        cmd.Parameters.AddWithValue("@firstname",firstName);
                        cmd.Parameters.AddWithValue("@middlename", middleName);
                        cmd.Parameters.AddWithValue("@lastname", lastName);
                        cmd.Parameters.AddWithValue("@username", userName);
                        cmd.Parameters.AddWithValue("@password", Password);
                        cmd.Parameters.AddWithValue("@image", imageBytes);
                        cmd.Parameters.AddWithValue("@role", role);

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
            UploadData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtFname.Clear();
            txtMname.Clear();
            txtLname.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            comboBox1.SelectedIndex = -1;
            pictureBox1.Image = null; // Clear the image in the PictureBox
        }
        
        public void UploadData()
        {
            string connectionString = "server=localhost; Database=finacore; Uid=root; Pwd=;";
            MySqlConnection sqlConn = new MySqlConnection(connectionString );
            MySqlCommand sqlCmd = new MySqlCommand();
            MySqlDataReader sqlRD;
            DataTable sqlDt = new DataTable();

            try
            {
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;
                sqlCmd.CommandText = "SELECT * FROM login_user";

                sqlRD = sqlCmd.ExecuteReader();
                sqlDt.Load(sqlRD);
                sqlRD.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sqlConn.Close();
            }
            dataGridView1.DataSource = sqlDt;

        }

        private void txtLname_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png)| *.jpg; *.jpeg; *.png";

            if (openFileDialog.ShowDialog() == DialogResult.OK) 
            { 
                pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void picBoxHidden_Click(object sender, EventArgs e)
        {
            //Show the password of the user
            txtPassword.UseSystemPasswordChar = true; //Show Password
            picBoxHidden.Visible = false;
            picBoxEye.Visible = true;
        }

        private void picBoxEye_Click_1(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = false;  // Hide the password
            picBoxEye.Visible = false;             // Hide the "eye open" icon
            picBoxHidden.Visible = true;               // Show the "eye closed" icon
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string connectionString = "Server = localhost; Database= finacore; Uid = root; Pwd = ;";

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to update from the grid.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageBox.Show("Please select only one user to update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Assuming the user ID is in the first cell of the DataGridView
            int userId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

            string firstName = txtFname.Text;
            string middleName = txtMname.Text;
            string lastName = txtLname.Text;
            string userName = txtUsername.Text;
            string Password = txtPassword.Text;
            string role = comboBox1.Text;
            byte[] imageBytes = null;

            if (pictureBox1.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                    imageBytes = ms.ToArray();
                }
            }

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = "UPDATE login_user SET firstname = @firstname, middlename = @middlename, lastname = @lastname, username = @username, password = @password, image = @image, role = @role WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", userId);
                        cmd.Parameters.AddWithValue("@firstname", firstName);
                        cmd.Parameters.AddWithValue("@middlename", middleName);
                        cmd.Parameters.AddWithValue("@lastname", lastName);
                        cmd.Parameters.AddWithValue("@username", userName);
                        cmd.Parameters.AddWithValue("@password", Password);
                        cmd.Parameters.AddWithValue("@image", imageBytes == null ? (object)DBNull.Value : imageBytes);
                        cmd.Parameters.AddWithValue("@role", role);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User information updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // Optionally, refresh your DataGridView here to show the updated data
                            // LoadUserData();
                        }
                        else
                        {
                            MessageBox.Show("No user information was updated.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string connectionString = "Server = localhost; Database= finacore; Uid = root; Pwd = ;";

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to delete from the grid.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageBox.Show("Please select only one user to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Assuming the user ID is in the first cell of the DataGridView
            int userId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

            if (MessageBox.Show("Are you sure you want to delete this user?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    try
                    {
                        con.Open();
                        string query = "DELETE FROM login_user WHERE id = @id";

                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@id", userId);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("User deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                // Optionally, refresh your DataGridView here to show the updated data
                                // LoadUserData();
                                // You might also want to clear the input fields after deletion
                                btnCancel_Click(sender, e);
                            }
                            else
                            {
                                MessageBox.Show("Failed to delete the user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        

        private void dataGridView1_SelectionChanged_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Assuming the column order in your DataGridView matches the order
                // in your database table (id, firstname, middlename, lastname, username, password, image, role)
                // **Adjust the index numbers below if your column order is different!**

                txtFname.Text = selectedRow.Cells[1].Value?.ToString(); // Firstname
                txtMname.Text = selectedRow.Cells[2].Value?.ToString(); // Middlename
                txtLname.Text = selectedRow.Cells[3].Value?.ToString(); // Lastname
                txtUsername.Text = selectedRow.Cells[4].Value?.ToString(); // Username
                txtPassword.Text = selectedRow.Cells[5].Value?.ToString(); // Password

                // For the ComboBox, you need to find the matching item in its items collection
                string roleFromGrid = selectedRow.Cells[7].Value?.ToString(); // Role
                if (comboBox1.Items.Contains(roleFromGrid))
                {
                    comboBox1.SelectedItem = roleFromGrid;
                }
                else
                {
                    comboBox1.SelectedIndex = -1; // Or handle the case where the role isn't in the list
                }

                // For the Image (assuming it's stored as a byte array in the database)
                if (selectedRow.Cells[6].Value != DBNull.Value && selectedRow.Cells[6].Value != null)
                {
                    byte[] imageBytes = (byte[])selectedRow.Cells[6].Value;
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    pictureBox1.Image = null; // Clear the image if the database value is null
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Clear the textboxes for registration information
            txtFname.Clear();
            txtMname.Clear();
            txtLname.Clear();
            txtUsername.Clear();
            txtPassword.Clear();

            // Reset the selected index of the combo box
            comboBox1.SelectedIndex = -1; // No item selected

            // Clear the image in the picture box (if any)
            pictureBox1.Image = null;
        }
    }
}
