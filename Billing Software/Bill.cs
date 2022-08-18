using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace Billing_Software
{
    public partial class Bill : Form
    {
        public Bill()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'billingDBDataSet.OrderDetails' table. You can move, or remove it, as needed.

            billTotxt.Text = Login.UName;
            // mtxt.Text = Register.mobile;
            invcTxt.Text = Order.invcNo;
            amtTxt.Text = Order.grndtotal.ToString();
            this.BindGrid();


        }
        private void BindGrid()
        {
            string conn = ConfigurationManager.ConnectionStrings["Con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT ProductName,Quantity,Amount FROM [dbo].[OrderDetails] where OrderId='" + Order.invcNo + "'", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);

                            //Set AutoGenerateColumns False
                            dataGridView1.AutoGenerateColumns = false;

                            //Set Columns Count
                            dataGridView1.ColumnCount = 3;

                            //Add Columns
                            dataGridView1.Columns[0].Name = "ProductName";
                            dataGridView1.Columns[0].HeaderText = "ProductName";
                            dataGridView1.Columns[0].DataPropertyName = "ProductName";

                            dataGridView1.Columns[1].HeaderText = "Quantity";
                            dataGridView1.Columns[1].Name = "Quantity";
                            dataGridView1.Columns[1].DataPropertyName = "Quantity";

                            dataGridView1.Columns[2].Name = "Amount";
                            dataGridView1.Columns[2].HeaderText = "Amount";
                            dataGridView1.Columns[2].DataPropertyName = "Amount";
                            dataGridView1.DataSource = dt;
                            dataGridView1.Columns["Amount"].ReadOnly = true;
                            dataGridView1.Columns["ProductName"].ReadOnly = true;
                            dataGridView1.Columns["Quantity"].ReadOnly = true;
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //remove from database
            string conn = ConfigurationManager.ConnectionStrings["Con"].ConnectionString;
            SqlConnection con = new SqlConnection(conn);
            string query = "Delete from [dbo].[OrderDetails] where OrderId= '" + this.invcTxt.Text + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader myreader;
           
            con.Open();
            myreader = cmd.ExecuteReader();
            //MessageBox.Show("successfully data Deleted", "user information");
            while (myreader.Read())
            {
            }
            con.Close();
            Order.grndtotal= 0;
            Order form1 = new Order();
            form1.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Login login=new Login();
            login.Show();
        }

        /*string conn = ConfigurationManager.ConnectionStrings["Con"].ConnectionString;
        SqlConnection sqlconn = new SqlConnection(conn);
        sqlconn.Open();
        SqlCommand cmd = new SqlCommand("SELECT ProductName,Quantity,Amount FROM [dbo].[OrderDetails] where OrderId='" + Form1.invcNo + "'", sqlconn);

        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        sda.Fill(dt);
        *//*cmd.CommandType = CommandType.Text;

        DataTable dt = new DataTable();*//*

        //dt.Load(cmd.ExecuteReader());
        dataGridView1.DataSource = dt;
        sqlconn.Close();*/
    }

            
        

        
    }

