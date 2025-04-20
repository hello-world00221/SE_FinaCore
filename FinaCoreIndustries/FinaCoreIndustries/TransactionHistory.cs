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
    public partial class TransactionHistory : Form
    {
        public TransactionHistory()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void TransactionHistory_Load(object sender, EventArgs e)
        {
            UploadPurchasedRecords();
            ViewClients();
            ViewTransactions();
        }

        public void UploadPurchasedRecords()
        {
            string connectionString = "server=localhost;Database=finacore; Uid=root; Pwd=;";
            MySqlConnection sqlConn = new MySqlConnection(connectionString);
            MySqlCommand sqlCmd = new MySqlCommand();
            MySqlDataReader sqlRD;
            DataTable sqlDT = new DataTable();

            try
            {
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;
                sqlCmd.CommandText = "SELECT * FROM item_purchased";

                sqlRD = sqlCmd.ExecuteReader();
                sqlDT.Load(sqlRD);
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
            dataGridView1.DataSource = sqlDT;
        }

        public void ViewClients()
        {
            string connectionString = "server=localhost;Database=finacore; Uid=root; Pwd=;";
            MySqlConnection sqlConn = new MySqlConnection(connectionString);
            MySqlCommand sqlCmd = new MySqlCommand();
            MySqlDataReader sqlRD;
            DataTable sqlDT = new DataTable();

            try
            {
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;
                sqlCmd.CommandText = "SELECT * FROM company_client";

                sqlRD = sqlCmd.ExecuteReader();
                sqlDT.Load(sqlRD);
                sqlRD.Close();

            } 
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
            finally
            {
                sqlConn.Close();
            }
            dataGridView2.DataSource = sqlDT;
        }

        public void ViewTransactions()
        {
            string connectionString = "server=localhost;Database=finacore; Uid=root; Pwd=;";
            MySqlConnection sqlConn = new MySqlConnection(connectionString);
            MySqlCommand sqlCmd = new MySqlCommand();
            MySqlDataReader sqlRD;
            DataTable sqlDT = new DataTable();

            try
            {
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;
                sqlCmd.CommandText = "SELECT * FROM company_transactions";

                sqlRD = sqlCmd.ExecuteReader();
                sqlDT.Load(sqlRD);
                sqlRD.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
            finally
            {
                sqlConn.Close();
            }
            dataGridView3.DataSource = sqlDT;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnGenSOA_Click(object sender, EventArgs e)
        {

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }
    }
}
