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
using System.Windows.Forms.DataVisualization.Charting;


namespace FinaCoreIndustries
{
    public partial class FinanceHistory : Form
    {
        public FinanceHistory()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            
        }

        private void FinanceHistory_Load(object sender, EventArgs e)
        {
            ViewItemPurchased_Chart();
            viewTransactionType_Chart();


        }


        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        public void ViewItemPurchased_Chart()
        {
            string connectionString = "Server = localhost; Database = finacore; Uid = root; Pwd =;";
            string query = "SELECT item_type, SUM(item_quantity) AS total_quantity FROM item_purchased GROUP BY item_type";

            try
            {
                using(MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);


                    //Clear existing data in the chart
                    chart1.Series.Clear();
                    chart1.ChartAreas.Clear();
                    chart1.Titles.Clear();

                    //Create Chart Area

                    ChartArea chartArea = new ChartArea();
                    chart1.ChartAreas.Add(chartArea);

                    //Create new Series

                    Series series = new Series("Item Types");
                    series.ChartType = SeriesChartType.Pie;
                    series.IsValueShownAsLabel = true; //show the values in pie chart

                    //Add data points

                    foreach (DataRow row in dt.Rows)
                    {
                        string itemType = row["item_type"].ToString();
                        int quantity = Convert.ToInt32(row["total_quantity"]);
                        series.Points.AddXY(itemType, quantity);
                    }

                    chart1.Legends.Clear();
                    chart1.Legends.Add(new Legend("Legend 1"));

                    chart1.Titles.Add("Purchased Items By Type");
                    chart1.Series.Add(series);

                }
            } catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }


        }


        public void viewTransactionType_Chart()
        {
            string connectionString = "Server = localhost; Database = finacore; Uid = root; Pwd =;";
            string query = @"SELECT MONTH(transaction_date) AS trans_month, transaction_type, SUM(transaction_amount) AS total_amount FROM company_transactions GROUP BY trans_month, transaction_type ORDER BY trans_month, transaction_type;";


            try
            {
                using(MySqlConnection conn = new MySqlConnection(connectionString))
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
                    chart2.ChartAreas[0].AxisY.Title = "Transaction Amount (₱)";

                }
            } 
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
        }
    }
 }
