using System;
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
            roundedBtn(btnLogin, 5);

            roundedPanel(panelUsername, 5);
            roundedPanel(panelPassword, 5);

            labelWelcome.BringToFront();

            txtPassword.UseSystemPasswordChar = true;
            picBoxEye.Visible = false; //Hidden the password when it is open

            
        }




        public static void roundedBtn(Button button, int radius)
        {
            if (button.Height < radius * 2) radius = button.Height / 2;
            if (button.Width < radius * 2) radius = button.Width / 2;

            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
            path.AddLine(radius, 0, button.Width - radius, 0);
            path.AddArc(button.Width - radius * 2, 0, radius * 2, radius * 2, 270, 90);
            path.AddLine(button.Width, radius, button.Width, button.Height - radius);
            path.AddArc(button.Width - radius * 2, button.Height - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddLine(button.Width - radius, button.Height, radius, button.Height);
            path.AddArc(0, button.Height - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();

            button.Region = new Region(path);
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
            string connectionString = "Server = localhost; Database = finacore; Uid=root; Pwd=;";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();

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


                    // If the username is Correct
                    string loginquery = "SELECT  COUNT(*) FROM login_user WHERE username = @username AND password = @password";

                    using (MySqlCommand logincmd = new MySqlCommand(loginquery, con))
                    {
                        logincmd.Parameters.AddWithValue("@username", txtUsername.Text);
                        logincmd.Parameters.AddWithValue("@password", txtPassword.Text);

                        int userCount = Convert.ToInt32(logincmd.ExecuteScalar());

                        if (userCount > 0)
                        {
                            MessageBox.Show("Login Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Hide();
                            Form formDashboard = new Dashboard();
                            formDashboard.ShowDialog();
                            this.Show();
                        }
                        

                        //Clear all the fields
                        txtUsername.Clear();
                        txtPassword.Clear();

                    }
                
                }
                catch (Exception ex)
                {
                MessageBox.Show("Error: " + ex.Message, "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
 

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

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

        private void LoginWindow_Load_1(object sender, EventArgs e)
        {
            
        }

        private void registerAccount_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Register account?", "Question", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                registerAccount RegisterAccount = new registerAccount();
                RegisterAccount.ShowDialog();
                
            }

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            txtPassword.Enabled = true;
        }

        private void txtUsername_Enter(object sender, EventArgs e)
        {
            txtPassword.Enabled = true;
        }

    }
}
