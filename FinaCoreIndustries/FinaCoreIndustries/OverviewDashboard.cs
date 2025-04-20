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


namespace FinaCoreIndustries
{
    public partial class OverviewDashboard : Form
    {
        public OverviewDashboard()
        {
            InitializeComponent();
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
    }
}
