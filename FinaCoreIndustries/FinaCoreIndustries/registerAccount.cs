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

                    string query = "INSERT INTO login_user (firstname, middlename, lastname, username, password, image) values (@firstname, @middlename, @lastname, @username, @password, @image)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        // Adding parameters to the query
                        cmd.Parameters.AddWithValue("@firstname",firstName);
                        cmd.Parameters.AddWithValue("@middlename", middleName);
                        cmd.Parameters.AddWithValue("@lastname", lastName);
                        cmd.Parameters.AddWithValue("@username", userName);
                        cmd.Parameters.AddWithValue("@password", Password);
                        cmd.Parameters.AddWithValue("@image", imageBytes);

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
            txtLname.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
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
    }
}
