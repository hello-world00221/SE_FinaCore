using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
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
            dataGridView3.DataSource = sqlDT;
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
            dataGridView1.DataSource = sqlDT;
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
            dataGridView2.DataSource = sqlDT;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }




        //Print the data
        private void btnPrint_Click_1(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
            printDialog.Document = printDocument;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            float yPos = 10;
            int leftMargin = e.MarginBounds.Left;
            int topMargin = e.MarginBounds.Top;
            float lineHeight = 15f;

            // Print Company Clients Table
            e.Graphics.DrawString("Company Clients", new Font("Arial", 16, FontStyle.Bold), Brushes.Black, leftMargin, yPos);
            yPos += 20;
            PrintDataTable(e, dataGridView1.DataSource as DataTable, leftMargin, ref yPos, lineHeight);
            yPos += 30; // Add some space

            // Print Company Transactions Table
            e.Graphics.DrawString("Company Transactions", new Font("Arial", 16, FontStyle.Bold), Brushes.Black, leftMargin, yPos);
            yPos += 20;
            PrintDataTable(e, dataGridView2.DataSource as DataTable, leftMargin, ref yPos, lineHeight);
            yPos += 30; // Add some space

            // Print Item Purchased Table
            e.Graphics.DrawString("Item Purchased", new Font("Arial", 16, FontStyle.Bold), Brushes.Black, leftMargin, yPos);
            yPos += 20;
            PrintDataTable(e, dataGridView3.DataSource as DataTable, leftMargin, ref yPos, lineHeight);
        }

        private void PrintDataTable(PrintPageEventArgs e, DataTable dt, int leftMargin, ref float yPos, float lineHeight)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                // Print Headers
                float currentX = leftMargin;
                foreach (DataColumn column in dt.Columns)
                {
                    e.Graphics.DrawString(column.ColumnName, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, currentX, yPos);
                    currentX += 100; // Adjust column width as needed
                }
                yPos += lineHeight;
                e.Graphics.DrawLine(Pens.Black, leftMargin, yPos, leftMargin + (dt.Columns.Count * 100), yPos); // Draw a separator line
                yPos += 5;

                // Print Rows
                foreach (DataRow row in dt.Rows)
                {
                    currentX = leftMargin;
                    foreach (object item in row.ItemArray)
                    {
                        e.Graphics.DrawString(item.ToString(), new Font("Arial", 10), Brushes.Black, currentX, yPos);
                        currentX += 100; // Adjust column width as needed
                    }
                    yPos += lineHeight;

                    // Check if a new page is needed
                    if (yPos > e.MarginBounds.Bottom - lineHeight)
                    {
                        e.HasMorePages = true;
                        return;
                    }
                }
                e.HasMorePages = false;
            }
            else
            {
                e.Graphics.DrawString("No data to print.", new Font("Arial", 10), Brushes.Black, leftMargin, yPos);
                yPos += lineHeight;
            }
        }


        private string soaContentToPrint = "";

        private void btnGenerateSoa_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get selected client record
                DataGridViewRow clientRow = dataGridView1.SelectedRows[0];
                string clientId = clientRow.Cells["client_id"].Value?.ToString();
                string clientName = clientRow.Cells["client_name"].Value?.ToString();
                string clientCode = clientRow.Cells["client_code"].Value?.ToString();
                string contact = clientRow.Cells["contact"].Value?.ToString();
                string email = clientRow.Cells["email"].Value?.ToString();
                string address = clientRow.Cells["address"].Value?.ToString();

                // Start building SOA content
                soaContentToPrint = $"STATEMENT OF ACCOUNT\n\n" +
                                    $"Client Information:\n" +
                                    $"Client ID: {clientId}\n" +
                                    $"Client Name: {clientName}\n" +
                                    $"Client Code: {clientCode}\n" +
                                    $"Contact: {contact}\n" +
                                    $"Email: {email}\n" +
                                    $"Address: {address}\n\n" +
                                    $"Transaction Details:\n";

                // Loop through transactions matching the selected client_id
                bool hasTransactions = false;
                foreach (DataGridViewRow transactionRow in dataGridView2.Rows)
                {
                    if (transactionRow.Cells["client_id"].Value?.ToString() == clientId)
                    {
                        hasTransactions = true;
                        string transactionId = transactionRow.Cells["transaction_id"].Value?.ToString();
                        string transactionCode = transactionRow.Cells["transaction_code"].Value?.ToString();
                        string transactionType = transactionRow.Cells["transaction_type"].Value?.ToString();
                        string transactionAmount = transactionRow.Cells["transaction_amount"].Value?.ToString();
                        string transactionDate = transactionRow.Cells["transaction_date"].Value?.ToString();

                        soaContentToPrint += $"\nTransaction ID: {transactionId}\n" +
                                             $"Transaction Code: {transactionCode}\n" +
                                             $"Transaction Type: {transactionType}\n" +
                                             $"Transaction Amount: {transactionAmount}\n" +
                                             $"Transaction Date: {transactionDate}\n";
                    }
                }

                if (!hasTransactions)
                {
                    soaContentToPrint += "\nNo transactions found for this client.\n";
                }

                // Show the SOA
                MessageBox.Show(soaContentToPrint, "Statement of Account");

                // Prepare to print
                PrintDialog printDialog = new PrintDialog();
                PrintDocument printDocument = new PrintDocument();
                printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage); // Correct method name
                printDialog.Document = printDocument;

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDocument.Print();
                }
            }
            else
            {
                MessageBox.Show("Please select one client from the Client Records table.", "Selection Required");
            }
        }

        private void PrintDocument_PrintPage1(object sender, PrintPageEventArgs e)
        {
            Font printFont = new Font("Arial", 12);
            SolidBrush printBrush = new SolidBrush(Color.Black);

            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;

            foreach (string line in soaContentToPrint.Split('\n'))
            {
                e.Graphics.DrawString(line, printFont, printBrush, x, y);
                y += printFont.GetHeight(e.Graphics);
            }

            e.HasMorePages = false;

            printBrush.Dispose();
            printFont.Dispose();
        }
    }

}
