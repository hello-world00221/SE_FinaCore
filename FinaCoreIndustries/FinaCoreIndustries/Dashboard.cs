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
        public Dashboard(string fullName, Image image)
        {
            InitializeComponent();
            label10.Text = "Welcome " + fullName;

            if (image != null )
            {
                picBoxUser.Image = image;
                
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

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            Client_List clientList = new Client_List();
            clientList.TopLevel = false;
            panel1.Controls.Add(clientList);
            clientList.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            TransactionHistory transactionHistory = new TransactionHistory();
            transactionHistory.TopLevel = false;
            panel1.Controls.Add(transactionHistory);
            transactionHistory.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            FinanceHistory financeHistory = new FinanceHistory();
            financeHistory.TopLevel = false;
            panel1.Controls.Add(financeHistory);
            financeHistory.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadOverviewDashboard();
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

        private void button5_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            registerAccount RegisterAccount = new registerAccount();
            RegisterAccount.TopLevel = false;
            panel1.Controls.Add(RegisterAccount);
            RegisterAccount.Show();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {

        }
    }
}
