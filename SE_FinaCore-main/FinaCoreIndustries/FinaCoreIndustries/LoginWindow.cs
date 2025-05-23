﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;



namespace FinaCoreIndustries
{
    public partial class LoginWindow : Form
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginWindow_Load(object sender, EventArgs e)
        {
            roundedPanel(panelUsername, 5);
            roundedPanel(panelPassword, 5);

            labelWelcome.BringToFront();

            txtPassword.UseSystemPasswordChar = true;
            picBoxEye.Visible = false; //Hidden the password when it is open

            
        }

        public void roundedPanel(Panel panel, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
            path.AddLine(radius, 0, panel.Width - radius, 0);
            path.AddArc(panel.Width - radius * 2, 0, radius * 2, radius * 2, 270, 90);
            path.AddLine(panel.Width, radius, panel.Width, panel.Height - radius);
            path.AddArc(panel.Width - radius * 2, panel.Height - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddLine(panel.Width - radius, panel.Height, radius, panel.Height);
            path.AddArc(0, panel.Height - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();

            panel.Region = new Region(path);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //Check if ALL CAPS input before anything else
            if(!string.IsNullOrEmpty(txtUsername.Text) && txtUsername.Text == txtUsername.Text.ToUpper() &&
                !string.IsNullOrEmpty(txtPassword.Text) && txtPassword.Text == txtPassword.Text.ToUpper())
            {
                lblCapsWarning.Text = "Username and Password are in ALL CAPS. Please Check you input.";

                return; //Stop processing here
            }
            else
            {
                lblCapsWarning.Text = "";
            }

            //Accessing to the database

            string connectionString = "Server = localhost; Database = finacore; Uid=root; Pwd=;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    //Validation for the username

                    string validationQuery = "SELECT COUNT(*) FROM login_user WHERE username =  @username";
                    using (MySqlCommand cmd = new MySqlCommand(validationQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@username", txtUsername.Text);

                        int userExist = Convert.ToInt32(cmd.ExecuteScalar());

                        if (userExist == 0)
                        {
                            MessageBox.Show("Username and Password do not match, Please enter it correctly.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //Clear all the fields
                            txtUsername.Clear();
                            txtPassword.Clear();
                            txtPassword.Enabled = false; //Disabling the password until valid username
                            return;// Stop here if the username is invalid
                        }
                    }


                    //Fetch the data of the user's firstname, lastname, username ,and password

                    string loginquery = "SELECT firstname, lastname, image, role FROM login_user WHERE username = @username AND password = @password";

                    using (MySqlCommand logincmd = new MySqlCommand(loginquery, con))
                    {
                        logincmd.Parameters.AddWithValue("@username", txtUsername.Text);
                        logincmd.Parameters.AddWithValue("@password", txtPassword.Text);

                        using (MySqlDataReader reader = logincmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string firstName = reader["firstname"].ToString();
                                string lastName = reader["lastname"].ToString();
                                string role = reader["role"].ToString();
                                string fullName = firstName + " " + lastName;

                                //Get the image from the database
                                byte[] imageBytes = reader["image"] as byte[];

                                //Check if the image is existing in the database
                                Image userImage = null;
                                if(imageBytes != null && imageBytes.Length > 0)
                                {
                                    using (MemoryStream ms = new MemoryStream(imageBytes))
                                    {
                                        userImage = Image.FromStream(ms);
                                    }
                                }

                                MessageBox.Show("Welcome " +  fullName  + ",  Login Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Hide();
                                Form formDashboard = new Dashboard(fullName ,userImage, role);
                                formDashboard.ShowDialog();
                                this.Show();

                            }

                            //Clear all the fields
                            txtUsername.Clear();
                            txtPassword.Clear();

                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

    }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void picBoxHidden_Click(object sender, EventArgs e)
        {
            //Show the password of the user
            txtPassword.UseSystemPasswordChar = false; //Show password
            picBoxHidden.Visible = false;
            picBoxEye.Visible = true;
        }

        private void picBoxEye_Click_1(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;  // Hide the password
            picBoxEye.Visible = false;             // Hide the "eye open" icon
            picBoxHidden.Visible = true;               // Show the "eye closed" icon
        }

        private void LoginWindow_Load_1(object sender, EventArgs e)
        {
            
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            txtPassword.Enabled = true;
        }

       

        private void registerAccount_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Register account?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                registerAccount RegisterAccount = new registerAccount();
                RegisterAccount.ShowDialog();

            }
        }

        private void Login_Click(object sender, EventArgs e)
        {
            //Check if ALL CAPS input before anything else
            if (!string.IsNullOrEmpty(txtUsername.Text) && txtUsername.Text == txtUsername.Text.ToUpper() &&
                !string.IsNullOrEmpty(txtPassword.Text) && txtPassword.Text == txtPassword.Text.ToUpper())
            {
                lblCapsWarning.Text = "Username and Password are in ALL CAPS. Please Check you input.";

                return; //Stop processing here
            }
            else
            {
                lblCapsWarning.Text = "";
            }

            //Accessing to the database

            string connectionString = "Server = localhost; Database = finacore; Uid=root; Pwd=;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    //Validation for the username

                    string validationQuery = "SELECT COUNT(*) FROM login_user WHERE username =  @username";
                    using (MySqlCommand cmd = new MySqlCommand(validationQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@username", txtUsername.Text);

                        int userExist = Convert.ToInt32(cmd.ExecuteScalar());

                        if (userExist == 0)
                        {
                            MessageBox.Show("Username and Password do not match, Please enter it correctly.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //Clear all the fields
                            txtUsername.Clear();
                            txtPassword.Clear();
                            txtPassword.Enabled = false; //Disabling the password until valid username
                            return;// Stop here if the username is invalid
                        }
                    }


                    //Fetch the data of the user's firstname, lastname, username ,and password

                    string loginquery = "SELECT firstname, lastname, image, role FROM login_user WHERE username = @username AND password = @password";

                    using (MySqlCommand logincmd = new MySqlCommand(loginquery, con))
                    {
                        logincmd.Parameters.AddWithValue("@username", txtUsername.Text);
                        logincmd.Parameters.AddWithValue("@password", txtPassword.Text);

                        using (MySqlDataReader reader = logincmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string firstName = reader["firstname"].ToString();
                                string lastName = reader["lastname"].ToString();
                                string role = reader["role"].ToString();
                                string fullName = firstName + " " + lastName;

                                //Get the image from the database
                                byte[] imageBytes = reader["image"] as byte[];

                                //Check if the image is existing in the database
                                Image userImage = null;
                                if (imageBytes != null && imageBytes.Length > 0)
                                {
                                    using (MemoryStream ms = new MemoryStream(imageBytes))
                                    {
                                        userImage = Image.FromStream(ms);
                                    }
                                }

                                MessageBox.Show("Welcome " + fullName + ",  Login Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Hide();
                                Form formDashboard = new Dashboard(fullName, userImage, role);
                                formDashboard.ShowDialog();
                                this.Show();

                            }

                            //Clear all the fields
                            txtUsername.Clear();
                            txtPassword.Clear();

                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void rjButton1_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        // LOGIN CUSTOM BUTTON

    }
}
