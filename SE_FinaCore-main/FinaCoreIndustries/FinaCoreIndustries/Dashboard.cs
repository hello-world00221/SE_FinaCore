using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace FinaCoreIndustries
{
    public partial class Dashboard : Form
    {
        private string userRole;
        public Dashboard(string fullName, Image image, string role)
        {
            InitializeComponent();
            label10.Text = fullName;
            userRole = role;

            if (image != null )
            {
                picBoxUser.Image = image;
                
            }

            //Restrict  Register button
            if (userRole.ToLower() != "admin")
            {
                rjButton5.Visible = false; // Hide the Register button
            }

            LoadOverviewDashboard();//Automatically Open the HomePage
        }

        private void LoadOverviewDashboard()
        {
            panel1.Controls.Clear();
            OverviewDashboard overviewDashboard = new OverviewDashboard();
            overviewDashboard.TopLevel = false;
            overviewDashboard.Dock = DockStyle.Fill; // Ensures it fits inside the panel
            panel1.Controls.Add(overviewDashboard);
            overviewDashboard.Show();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToLongDateString();
            label2.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToLongTimeString();
            
        }

        private void Home_Click(object sender, EventArgs e)
        {
            LoadOverviewDashboard();
        }

        private void Transactions_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            Client_List clientList = new Client_List();
            clientList.TopLevel = false;
            panel1.Controls.Add(clientList);
            clientList.Show();
        }

        private void Records_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            TransactionHistory transactionHistory = new TransactionHistory();
            transactionHistory.TopLevel = false;
            panel1.Controls.Add(transactionHistory);
            transactionHistory.Show();
        }

        private void FHistory_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            FinanceHistory financeHistory = new FinanceHistory();
            financeHistory.TopLevel = false;
            panel1.Controls.Add(financeHistory);
            financeHistory.Show();
        }

        private void regAccount_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            registerAccount RegisterAccount = new registerAccount();
            RegisterAccount.TopLevel = false;
            panel1.Controls.Add(RegisterAccount);
            RegisterAccount.Show();
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            //Logout button

            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            Help help = new Help();
            help.TopLevel = false;
            panel1.Controls.Add(help);
            help.Show();
        }
    }
}
