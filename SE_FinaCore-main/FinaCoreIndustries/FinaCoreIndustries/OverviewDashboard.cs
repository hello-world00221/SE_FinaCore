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
using OfficeOpenXml;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Printing;
using System.IO.Pipelines;
using FinaCoreIndustries;


namespace FinaCoreIndustries
{
    
    public partial class OverviewDashboard : Form
    {
        string connectionString = "Server = localhost; Database= finacore; Uid= root; Pwd=;";

        

        public OverviewDashboard()
        {
            InitializeComponent();
            LoadRecentClient();
            LoadLatestAgenda();
            LoadOverviewData();
            txtAgenda.ReadOnly = true;
        }

        private void LoadRecentClient()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"SELECT client_name FROM company_client ORDER BY client_date DESC LIMIT 1;";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            label5.Text = reader["client_name"].ToString();
                        }
                        else
                        {
                            label5.Text = "No Client Found";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching client: " + ex.Message);
            }
        }

        private void LoadLatestAgenda()
        {
            string query = "SELECT agenda_text FROM company_agenda ORDER BY agenda_date DESC LIMIT 1";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtAgenda.Text = reader["agenda_text"].ToString();
                    }
                    else
                    {
                        txtAgenda.Text = "No Agenda Found.";
                    }
                }
            }
        }

        private void btnImportData_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files|*.xlsx;*.xls",
                Title = "Select an Excel File"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                DataTable dt = new DataTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    InsertDataIntoMySql(dt);
                }
            }
        }

        private DataTable ReadExcelData(string filePath)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    if (worksheet == null)
                    {
                        MessageBox.Show("No Worksheet found in excel file.");
                        return null;

                    }
                    int colCount = worksheet.Dimension.End.Column;
                    int rowCount = worksheet.Dimension.End.Row;

                    for (int col = 1; col <= colCount; col++)
                    {
                        dt.Columns.Add(worksheet.Cells[1, col].Text.Trim());
                    }

                    for (int row = 1; row <= rowCount; row++)
                    {
                        DataRow dr = dt.NewRow();
                        for (int col = 1; col <= colCount; col++)
                        {
                            dr[col - 1] = worksheet.Cells[row, col].Text.Trim();
                        }
                        dt.Rows.Add(dr);

                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading excel file: " + ex.Message);
                return null;
            }
        }

        private void InsertDataIntoMySql(DataTable dt)
        {
            string connectionString = "Server = localhost; Database= finacore; Uid= root; Pwd=;";


            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    foreach (DataRow row in dt.Rows)
                    {
                        using (MySqlCommand cmd = new MySqlCommand(@"
                            INSERT INTO company_client 
                            (client_id, client_name, client_code, contact, email, address, status, client_date) 
                            VALUES (@client_id, @client_name, @client_code, @contact, @email, @address, @status, @DateAdded)" , conn))
                        {
                            cmd.Parameters.AddWithValue("@client_id",  row["client_id"]);
                            cmd.Parameters.AddWithValue("@client_name", row["client_name"]);
                            cmd.Parameters.AddWithValue("@client_code", row["client_code"]);
                            cmd.Parameters.AddWithValue("@contact", row["contact"]);
                            cmd.Parameters.AddWithValue("@email", row["email"]);
                            cmd.Parameters.AddWithValue("@address", row["address"]);
                            cmd.Parameters.AddWithValue("@status", row["status"]);
                            cmd.Parameters.AddWithValue("@DateAdded", row["client_date"]);

                            if (DateTime.TryParse(row["client_date"].ToString(), out DateTime clientDate))
                            {
                                cmd.Parameters.AddWithValue("@DateAdded", clientDate);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@DateAdded", DBNull.Value);
                            }

                                cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Data Import Successfully");
                }
                catch (Exception ex) 
                {
                    MessageBox.Show("MySql Error: " + ex.Message);
                
                }
            }
        }

        private void OverviewDashboard_Load(object sender, EventArgs e)
        {
            ViewTransactionRecords_Chart();
            LoadLatestAgenda();
        }

        public void ViewTransactionRecords_Chart()
        {
            string connectionString = "Server = localhost; Database = finacore; Uid = root; Pwd =;";
            string query = @"SELECT MONTH(transaction_date) AS trans_month, transaction_type, SUM(transaction_amount) AS total_amount FROM company_transactions GROUP BY trans_month, transaction_type ORDER BY trans_month, transaction_type;";


            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);


                    //Clear the previous chart data

                    chart2.Series.Clear();
                    chart2.ChartAreas.Clear();
                    chart2.Titles.Clear();

                    //SetUp Chart Area

                    ChartArea chartArea = new ChartArea("MainArea");
                    chart2.ChartAreas.Add(chartArea);

                    //Get unique transaction types

                    var transactionTypes = dt.AsEnumerable().Select(r => r["transaction_type"].ToString()).Distinct();


                    //Create a series for each transaction type

                    foreach (string transType in transactionTypes)
                    {
                        Series series = new Series(transType);
                        series.ChartType = SeriesChartType.Column;

                        //Filter the data for the transaction

                        var filteredRows = dt.Select($"transaction_type = '{transType}'");

                        foreach (var row in filteredRows)
                        {
                            int month = Convert.ToInt32(row["trans_month"]);
                            decimal amount = Convert.ToDecimal(row["total_amount"]);
                            series.Points.AddXY(month, amount);
                        }
                        chart2.Series.Add(series);
                    }

                    chart2.Titles.Add("Monthly Transactions Amount by Types");
                    chart2.ChartAreas[0].AxisX.Title = "Month";
                    chart2.ChartAreas[0].AxisY.Title = "Amount (₱)";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
        }

        private void LoadOverviewData()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                //Total Sales

                var totalSalesCmd = new MySqlCommand(
                "SELECT SUM(transaction_amount) FROM company_transactions WHERE transaction_type = 'Purchased'", connection);
                var totalSales = totalSalesCmd.ExecuteScalar();
                label2.Text = $"₱ {Convert.ToDecimal(totalSales):N2}";


                //Loan Payed 

                var loanPayedCmd = new MySqlCommand("SELECT SUM(transaction_amount) FROM company_transactions WHERE transaction_type = 'Paying'", connection);

                var loanPayed = loanPayedCmd.ExecuteScalar();

                label3.Text = $"₱ {Convert.ToDecimal(loanPayed):N2}";


                //Most Purchased Items

                var mostPurchasedItems = new MySqlCommand(@"SELECT item_name FROM item_purchased GROUP BY item_name ORDER BY SUM(item_quantity) DESC LIMIT 1", connection);

                var mostPurchased = mostPurchasedItems.ExecuteScalar();

                label4.Text = mostPurchased?.ToString() ?? "N/A";
            }
        }

        private void btnAgenda_Click(object sender, EventArgs e)
        {
            string agendaText = txtAgendaOnly.Text.Trim();

            if (string.IsNullOrWhiteSpace(agendaText)) return;

            string query = "INSERT INTO company_agenda (agenda_text) VALUES (@agenda)";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@agenda", agendaText);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            txtAgendaOnly.Clear();
            LoadLatestAgenda();
        }

        PrintDocument soaPrintDocument = new PrintDocument();
        PrintPreviewDialog previewDialog = new PrintPreviewDialog();

        private DataTable soaTransactionTable = new DataTable();
        private string clientName = "", clientCode = "", clientEmail = "", clientAddress = "";


        private decimal totalAmount = 0;

        private void btnSOA_Click(object sender, EventArgs e)
        {
            int clientId = 1; // <-- Replace this with actual selected client ID from UI (e.g. a ComboBox or DataGridView)

            string connStr = "Server=localhost;Database=finacore;Uid=root;Pwd=;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                // Fetch client info
                string clientQuery = @"SELECT * FROM company_client WHERE client_id = @client_id";
                using (MySqlCommand cmd = new MySqlCommand(clientQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@client_id", clientId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            clientName = reader["client_name"].ToString();
                            clientCode = reader["client_code"].ToString();
                            clientEmail = reader["email"].ToString();
                            clientAddress = reader["address"].ToString();
                        }
                    }
                }

                // Fetch transaction history
                string transQuery = @"SELECT transaction_date, transaction_type, transaction_amount 
                              FROM company_transactions 
                              WHERE client_transaction_id = @client_id 
                              ORDER BY transaction_date ASC";

                using (MySqlCommand cmd = new MySqlCommand(transQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@client_id", clientId);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    soaTransactionTable.Clear();
                    adapter.Fill(soaTransactionTable);
                }

                // Compute total
                totalAmount = soaTransactionTable.AsEnumerable()
                    .Sum(row => row.Field<decimal>("transaction_amount"));
            }

            // Show print preview
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += SOA_PrintDocument_PrintPage;
            PrintPreviewDialog preview = new PrintPreviewDialog
            {
                Document = doc,
                Width = 1000,
                Height = 800
            };
            preview.ShowDialog();
        }

        private void SOA_PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font titleFont = new Font("Arial", 16, FontStyle.Bold);
            Font subTitleFont = new Font("Arial", 12, FontStyle.Bold);
            Font normalFont = new Font("Arial", 11);
            Font smallFont = new Font("Arial", 8);
            Pen linePen = new Pen(Color.Black, 1);

            int leftMargin = 50;
            int rightMargin = 775; // approximate A4 width
            int y = 50;

            // Header
            g.DrawString("Statement of Account", titleFont, Brushes.Black, leftMargin, y);
            g.DrawString("No. 0003439", normalFont, Brushes.Black, leftMargin, y + 40);

            // Draw FINACORE Details on the Right Side
            int headerRightX = rightMargin - 280;
            int headerY = y;

            Font bigTitleFont = new Font("Arial", 40, FontStyle.Bold);
            Font companyInfoFont = new Font("Arial", 8);

            // Split and draw "FINACORE" in two colors
            string finaText = "FINA";
            string coreText = "CORE";

            // Measure width of "FINA" to position "CORE" correctly
            SizeF finaSize = g.MeasureString(finaText, bigTitleFont);
            int finaX = headerRightX - 10;
            int coreX = finaX + (int)finaSize.Width - 25;

            g.DrawString(finaText, bigTitleFont, Brushes.Red, finaX, headerY - 10);
            g.DrawString(coreText, bigTitleFont, Brushes.Black, coreX, headerY - 10);
            headerY += 50;
            g.DrawString("(Trade Name: FINA CORE Technology Finance)", companyInfoFont, Brushes.Black, headerRightX, headerY); headerY += 20;
            g.DrawString("9/F Phil.AXA Life Centre (f.k.a: PSBank Tower)", companyInfoFont, Brushes.Black, headerRightX, headerY); headerY += 15;
            g.DrawString("Sen. Gil Puyat Ave., Makati City, Philippines", companyInfoFont, Brushes.Black, headerRightX, headerY); headerY += 15;
            g.DrawString("Tels.: (632) 7759-5595 to 96 ; (632) 3491-7360 to 61", companyInfoFont, Brushes.Black, headerRightX, headerY); headerY += 15;
            g.DrawString("Fax: (632) 7759-5597", companyInfoFont, Brushes.Black, headerRightX, headerY); headerY += 15;
            g.DrawString("NON VAT REG TIN: 005-026-386-00000", companyInfoFont, Brushes.Black, headerRightX, headerY);
            y += 100;

            g.DrawString("Client: " + clientName, normalFont, Brushes.Black, leftMargin, y); y += 20;
            g.DrawString("Address: " + clientAddress, normalFont, Brushes.Black, leftMargin, y); y += 20;
            g.DrawString("TIN: 000-000-000", normalFont, Brushes.Black, leftMargin, y); y += 20;

            g.DrawString("Attn.: ___________________________", normalFont, Brushes.Black, leftMargin, y);
            g.DrawString("Date: " + DateTime.Now.ToString("yyyy-MM-dd"), normalFont, Brushes.Black, headerRightX, y);
            y += 40;

            // Draw top line of table
            g.DrawLine(linePen, leftMargin, y, rightMargin, y); y += 2;

            // Column titles
            int particularsX = leftMargin + 2;
            int amountX = rightMargin - 225;
            g.DrawString("Particulars", subTitleFont, Brushes.Black, particularsX, y);
            g.DrawString("Amount", subTitleFont, Brushes.Black, amountX + 10, y);
            y += 25;

            // Header bottom line
            g.DrawLine(linePen, leftMargin, y, rightMargin, y);

            // Vertical lines for table
            int rowHeight = 25;
            int tableTopY = y;
            int numRows = 11;
            int tableBottomY = tableTopY + (numRows * rowHeight);

            g.DrawLine(linePen, 50, 250, 50, 920);
            g.DrawLine(linePen, 775, 250, 775, 920);
            g.DrawLine(linePen, 550, 250, 550, 820);
            g.DrawLine(linePen, 425, 820, 425, 920);
            g.DrawLine(linePen, leftMargin, 920, rightMargin, 920);

            int Prepare = leftMargin + 5;
            int Certi = rightMargin - 355;
            g.DrawString("Prepared:", subTitleFont, Brushes.Black, Prepare, 830);
            g.DrawString("Certified Correct:", subTitleFont, Brushes.Black, Certi + 10, 830);
            y += 25;

            g.DrawLine(linePen, leftMargin, 820, rightMargin, 820);

            int particulars = leftMargin + 5;
            int amount = rightMargin - 355;
            g.DrawString("Received:", subTitleFont, Brushes.Black, particulars, 880);
            g.DrawString("Date Received:", subTitleFont, Brushes.Black, amount + 10, 880);
            y += 25;

            g.DrawLine(linePen, leftMargin, 870, rightMargin, 870);



            // Left Footer
            int footerLeftY = 980;

            g.DrawString("30 Beits. (50x3) 3001-4500 OCN SAU0000524928", smallFont, Brushes.Black, leftMargin, footerLeftY); footerLeftY += 12;
            g.DrawString("Date Issued: Oct-15-2019: Valid until: Oct-14-2024", smallFont, Brushes.Black, leftMargin, footerLeftY); footerLeftY += 12;
            g.DrawString("CVJ PRINTING SERVICES 261 Vito Cruz, Ext, La Par MXL. Oly", smallFont, Brushes.Black, leftMargin, footerLeftY); footerLeftY += 12;
            g.DrawString("Tel. 478-2191 NON VAT REG TIN: 215-585-585-00000", smallFont, Brushes.Black, leftMargin, footerLeftY);

            // Right Footer
            int footerRightX = rightMargin - 355;
            int footerRightY = 980;
            g.DrawString("This document is not valid for claiming input taxes.", smallFont, Brushes.Black, footerRightX, footerRightY); footerRightY += 12;
            g.DrawString("12-14-2018 top 12-14-2003", smallFont, Brushes.Black, footerRightX, footerRightY); footerRightY += 12;
            g.DrawString("This Statement of Account shall be valid for five (5)", smallFont, Brushes.Black, footerRightX, footerRightY);
            g.DrawString("years from the date of ATP", smallFont, Brushes.Black, footerRightX, footerRightY + 12);
        }

    }
}
