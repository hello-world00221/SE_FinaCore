using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using OfficeOpenXml;
using System.IO;

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
                    InsertDataIntoOracle(dt);
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

        private void InsertDataIntoOracle(DataTable dt)
        {
            string connectionString = "User Id=finacore;Password=finacoreindustries;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SID=xe)));";

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    foreach (DataRow row in dt.Rows)
                    {
                        using (OracleCommand cmd = new OracleCommand(@"
                            INSERT INTO COMPANY_CLIENTS 
                            (CLIENT_ID, CLIENT_NAME, CLIENT_CODE, CONTACT, EMAIL, ADDRESS, STATUS, CLIENT_DATE) 
                            VALUES (:ClientID, :ClientName, :ClientCode, :Contact, :Email, :Address, :Status, TO_DATE(:DateAdded, 'YYYY-MM-DD'))", conn))
                        {
                            cmd.Parameters.Add(":ClientID", OracleDbType.Int64).Value = row["CLIENT_ID"];
                            cmd.Parameters.Add(":ClientName", OracleDbType.Varchar2,255).Value = row["CLIENT_NAME"];
                            cmd.Parameters.Add(":ClientCode", OracleDbType.Varchar2,9).Value = row["CLIENT_CODE"];
                            cmd.Parameters.Add(":Contact", OracleDbType.Varchar2,9).Value = row["CONTACT"];
                            cmd.Parameters.Add(":Email", OracleDbType.Varchar2,255).Value = row["EMAIL"];
                            cmd.Parameters.Add(":Address", OracleDbType.Clob).Value = row["ADDRESS"];
                            cmd.Parameters.Add(":Status", OracleDbType.Varchar2,10).Value = row["STATUS"];
                           
                            if (DateTime.TryParse(row["CLIENT_DATE"].ToString(), out DateTime clientDate))
                            {
                                cmd.Parameters.Add(":DateAdded",OracleDbType.Date).Value = clientDate;
                            }

                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Data Import Successfully");
                }
                catch (Exception ex) 
                {
                    MessageBox.Show("Database Error: " + ex.Message);
                
                }
            }
        }
    }
}
