using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace FinaCoreIndustries
{
    public partial class Client_List : Form
    {
        public Client_List()
        {
            InitializeComponent();
        }

        private void ClearFields()
        {
            // Clear TextBoxes
            txtID.Clear();
            txtName.Clear();
            txtCode.Clear();
            txtContact.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            txtTransID.Clear();
            txtCID.Clear();
            txtTransCode.Clear();
            txtAmt.Clear();
            txtPID.Clear();
            txtPCode.Clear();
            txtPName.Clear();
            txtPAmt.Clear();

            // Reset ComboBoxes
            cmbStatus.SelectedIndex = -1;
            cmbType.SelectedIndex = -1;
            cmbPType.SelectedIndex = -1;

            // Reset DateTimePickers
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            dateTimePicker3.Value = DateTime.Now;

            // Reset NumericUpDown
            numericUpDown1.Value = 0;
        }


        private void btnInvoice_Click(object sender, EventArgs e)
        {

        }

        private void btnSOA_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btn1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btn_Click(object sender, EventArgs e)
        {

        }

      

        private void btnAddClient_Click_1(object sender, EventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void btnInvoice_Click_1(object sender, EventArgs e)
        {

        }

        private void btn1_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void btn_Click_1(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {

        }

       
       

        private void txtID_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnReset_Click(object sender, EventArgs e)
        {

        }


        private void Client_List_Load(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int iTransID = rnd.Next(15, 9876543); // Generate this first
            int iPid = iTransID; // Assign iPid to be the same as iTransID

            int iCode = rnd.Next(14, 9876543);
            int iTransCode = rnd.Next(16, 9876543);
            int iPCode = rnd.Next(18, 9876543);

            txtPID.Text = iPid.ToString();       // Will be same as iTransID
            txtTransID.Text = iTransID.ToString(); // You should have this textbox
            txtCode.Text = iCode.ToString();
            txtTransCode.Text = iTransCode.ToString();
            txtPCode.Text = iPCode.ToString();

        }

        

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void cmbPType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void txtPAmt_TextChanged(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Database=finacore;Uid=root;Pwd=;";


            //For client information
            string clientID = txtID.Text;
            string clientName = txtName.Text;
            string clientCode = txtCode.Text;
            string contact = txtContact.Text;
            string email = txtEmail.Text;
            string address = txtAddress.Text;
            string status = cmbStatus.Text;
            DateTime dateAdded = dateTimePicker1.Value;

            //For transaction
            string transactionID = txtTransID.Text;
            string ClientID = txtCID.Text;
            string cCode = txtTransCode.Text;
            string Type = cmbType.Text;
            string Amt = txtAmt.Text;
            DateTime dateAdded1 = dateTimePicker2.Value;

            //For Purchased
            string purchasedID = txtPID.Text;
            string productCode = txtPCode.Text;
            string productName = txtPName.Text;
            string pType = cmbPType.Text;
            string pAmount = txtPAmt.Text;
            string Qty = numericUpDown1.Text;
            DateTime dateAdded2 = dateTimePicker3.Value;

            // Check if all required client fields are filled
            if (string.IsNullOrEmpty(clientID) || string.IsNullOrEmpty(clientName) || string.IsNullOrEmpty(clientCode) ||
                string.IsNullOrEmpty(contact) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(address) ||
                string.IsNullOrEmpty(status))
            {
                MessageBox.Show("Please fill in all client information fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Stop the execution if not all required fields are filled
            }

            // Check if all required transaction fields are filled
            if (string.IsNullOrEmpty(transactionID) || string.IsNullOrEmpty(ClientID) || string.IsNullOrEmpty(cCode) ||
                string.IsNullOrEmpty(Type) || string.IsNullOrEmpty(Amt))
            {
                MessageBox.Show("Please fill in all transaction information fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Stop the execution if not all required fields are filled
            }

            // Check if all required purchased item fields are filled
            if (string.IsNullOrEmpty(purchasedID) || string.IsNullOrEmpty(productCode) || string.IsNullOrEmpty(productName) ||
                string.IsNullOrEmpty(pType) || string.IsNullOrEmpty(pAmount) || string.IsNullOrEmpty(Qty))
            {
                MessageBox.Show("Please fill in all purchased item information fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Stop the execution if not all required fields are filled
            }


            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    //For Client_Info
                    // SQL query to insert the client data into COMPANY_CLIENTS table
                    string query = "INSERT INTO company_client (client_id, client_name, client_code, contact, email, address, status, client_date)" +
                        "VALUES (@clientId, @clientName, @clientCode, @Contact, @Email, @Address, @Status, @DateAdded)";



                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        // Adding parameters to the query
                        cmd.Parameters.AddWithValue("@clientId", clientID);
                        cmd.Parameters.AddWithValue("@clientName", clientName);
                        cmd.Parameters.AddWithValue("@clientCode", clientCode);
                        cmd.Parameters.AddWithValue("@Contact", contact);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@DateAdded", dateAdded);

                        // Execute the query to insert the data
                        cmd.ExecuteNonQuery();
                    }

                    //For Transactions
                    string transactionQuery = "INSERT INTO company_transactions (transaction_id, client_transaction_id, transaction_code, transaction_type, transaction_amount, transaction_date)" +
                        "VALUES (@TransID, @TransClientId, @TransCode, @TransType, @TransAmt,@DateAdded1)";


                    //Adding parameters to the query
                    using (MySqlCommand cmd = new MySqlCommand(transactionQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@TransID", transactionID);
                        cmd.Parameters.AddWithValue("@TransClientId", ClientID);
                        cmd.Parameters.AddWithValue("@TransCode", cCode);
                        cmd.Parameters.AddWithValue("@TransType", Type);
                        cmd.Parameters.AddWithValue("@TransAmt", Amt);
                        cmd.Parameters.AddWithValue("@DateAdded1", dateAdded1);

                        //Execute the query to insert the data

                        cmd.ExecuteNonQuery();

                    }

                    //Prepare for the purchased query

                    string purchasedQuery = "INSERT INTO item_purchased(transaction_purchased_id, item_code, item_name, item_type, item_amount, item_quantity, purchased_date)" +
                        "VALUES (@PurchasedID, @ItemCode, @ItemName, @ItemType, @ItemAmount, @Item_Quantity, @DateAdded2 )";



                    //Adding parameters to the query
                    using (MySqlCommand cmd = new MySqlCommand(purchasedQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@PurchasedID", purchasedID);
                        cmd.Parameters.AddWithValue("@ItemCode", productCode);
                        cmd.Parameters.AddWithValue("@ItemName", productName);
                        cmd.Parameters.AddWithValue("@ItemType", pType);
                        cmd.Parameters.AddWithValue("@ItemAmount", pAmount);
                        cmd.Parameters.AddWithValue("@Item_Quantity", Qty);
                        cmd.Parameters.AddWithValue("@DateAdded2", dateAdded2);


                        //Execure the query to insert data
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Information has been save successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();//Automatic clear after saving the data.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Clear client information fields
            txtID.Clear();
            txtName.Clear();
            txtCode.Clear();
            txtContact.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            cmbStatus.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;

            // Generate new random numbers
            Random rnd = new Random();
            int iTransID = rnd.Next(15, 9876543);
            int iPid = iTransID; // Assign iPid to be the same as iTransID
            int iCode = rnd.Next(14, 9876543);
            int iTransCode = rnd.Next(16, 9876543);
            int iPCode = rnd.Next(18, 9876543);

            // Update the textboxes with the new random codes
            txtPID.Text = iPid.ToString();
            txtTransID.Text = iTransID.ToString();
            txtCode.Text = iCode.ToString();
            txtTransCode.Text = iTransCode.ToString();
            txtPCode.Text = iPCode.ToString();

            // Clear transaction information fields
            txtCID.Clear();
            cmbType.SelectedIndex = -1;
            txtAmt.Clear();
            dateTimePicker2.Value = DateTime.Now;

            // Clear purchased item information fields
            txtPName.Clear();
            cmbPType.SelectedIndex = -1;
            txtPAmt.Clear();
            numericUpDown1.Value = 0;
            dateTimePicker3.Value = DateTime.Now;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Clear all the input fields here
            txtID.Clear();
            txtName.Clear();
            txtCode.Clear();
            txtContact.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            cmbStatus.SelectedIndex = -1; // Reset combo box selection

            txtTransID.Clear();
            txtCID.Clear();
            txtTransCode.Clear();
            cmbType.SelectedIndex = -1;

            txtAmt.Clear();

            txtPID.Clear();
            txtPCode.Clear();
            txtPName.Clear();
            cmbPType.SelectedIndex = -1;
            txtPAmt.Clear();
            numericUpDown1.Value = 0; // Reset numeric up-down value
        }

        
    }
 }

