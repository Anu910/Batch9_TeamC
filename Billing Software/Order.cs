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
    public partial class Order : Form
    {
        DataTable dt=new DataTable();
        /*DataRow dr;*/
        String amt;
        public static String  invcNo;
        public static Double grndtotal=0.00;
        private static Random random = new Random();
        public Order()
        {
            InitializeComponent();
            /*dt.Columns.Add("ProductName");
            dt.Columns.Add("Price(per Unit)");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Amount");
            dt.Columns.Add("Action");*/

        }
        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars,6)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            wlcmlab.Text = "Welcome Dear "+ Login.UName +"!!";
            string conn = ConfigurationManager.ConnectionStrings["Con"].ConnectionString;
            SqlConnection sqlconn=new SqlConnection(conn);
            string sqlquery = "select * from [dbo].[ProductTable]";
            SqlCommand cmd = new SqlCommand(sqlquery, sqlconn);
            SqlDataAdapter sda=new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            DataRow row = dt.NewRow();
            row[0] = 0;
            row[1] = "Please select";
            dt.Rows.InsertAt(row, 0);
            comboBox1.SelectedItem = null;
            
            comboBox1.ValueMember = "ProductId";
            comboBox1.DisplayMember = "ProductName";
            comboBox1.DataSource= dt;
            
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string conn = ConfigurationManager.ConnectionStrings["Con"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(conn);
            string sqlquery = "select * from [dbo].[ProductTable] where ProductName='"+comboBox1.Text+"'";
            SqlCommand cmd = new SqlCommand(sqlquery, sqlconn);
            sqlconn.Open();
            SqlDataReader sdr=cmd.ExecuteReader();
            while (sdr.Read())
            {
                textBox1.Text=sdr[2].ToString();
            }
            sqlconn.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            double q=Convert.ToDouble(numericUpDown1.Value);
            textBox2.Text=(q* Convert.ToDouble(textBox1.Text )).ToString();
            
            amt = textBox2.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
            row.Cells[0].Value = comboBox1.Text;
            row.Cells[1].Value = textBox1.Text; 
            row.Cells[2].Value = numericUpDown1.Value;
            row.Cells[3].Value = amt;
            grndtotal += Convert.ToDouble(amt);
            row.Cells[4].Value = "Remove";
            //.Cells[5].Value = "Update";
           
            /*dr = dt.NewRow();
           dr["ProductName"] = comboBox1.Text;
            dr["Price(per Unit)"] = textBox1.Text;
            dr["Quantity"] = numericUpDown1.Value;
            dr["Amount"] = amt;
            dr["Action"] = "Remove";
            dt.Rows.Add(dr);
            dataGridView1.DataSource= dt;*/
            dataGridView1.Rows.Add(row);
            dataGridView1.Columns["ProductName"].ReadOnly = true;
            //dataGridView1.Columns["Price(Per Unit)"].ReadOnly = true;
            dataGridView1.Columns["Amount"].ReadOnly = true;
            comboBox1.Text = "Please select";
            textBox1.Text = "0";
            numericUpDown1.Value = 0;
            textBox2.Text = "";
            
            textBox3.Text = grndtotal.ToString();
            

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dataGridView1.Columns[e.ColumnIndex].Name == "Action")
            {
                int row = dataGridView1.CurrentCell.RowIndex;
                if (MessageBox.Show("Are you sure of removing Item?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Double rmAmt=Convert.ToDouble(dataGridView1.Rows[row].Cells[3].Value);
                    grndtotal-=rmAmt;
                    textBox3.Text=grndtotal.ToString();
                    dataGridView1.Rows.RemoveAt(row);
                }

            }
            else
            {
                if(dataGridView1.Columns[e.ColumnIndex].Name == "Update")
                {
                    
                }
            }
        }
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String ordID = RandomString();
            invcNo = ordID;
            if(textBox3.Text=="" || textBox3.Text == "0")
            {
                MessageBox.Show("Please add Items to Proceed","Message",MessageBoxButtons.OK, MessageBoxIcon.Warning); 
            }
            else
            {
               
                //save details to database
                string conn = ConfigurationManager.ConnectionStrings["Con"].ConnectionString;
                SqlConnection sqlconn = new SqlConnection(conn);
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    string sqlquery = "insert into [dbo].[OrderDetails] values (@OrderId,@userName,@ProductName,@Quantity,@Amount)";
                    sqlconn.Open();
                    SqlCommand cmd = new SqlCommand(sqlquery, sqlconn);
                    if (dr.IsNewRow) continue;
                    {
                        cmd.Parameters.AddWithValue("@OrderId", ordID);
                        cmd.Parameters.AddWithValue("@userName", Login.UName);
                        cmd.Parameters.AddWithValue("@ProductName", dr.Cells["ProductName"].Value ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Quantity", Convert.ToInt32(dr.Cells["Quantity"].Value));
                        cmd.Parameters.AddWithValue("@Amount", Convert.ToDouble(dr.Cells["Amount"].Value));
                        
                    }
                    cmd.ExecuteNonQuery();
                    sqlconn.Close();
                }
                
                Bill form2 = new Bill();
                form2.Show();
                this.Hide();


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Login loginform=new Login();
            loginform.Show();
            this.Hide();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string unitprice, oldamt,newamt;
            int newQuantity;
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Quantity")
            {
                DataGridViewRow row=dataGridView1.Rows[e.RowIndex];
                unitprice=row.Cells[1].Value.ToString();
                newQuantity = Convert.ToInt32(row.Cells[2].Value);
                oldamt = row.Cells[3].Value.ToString();
                newamt= (newQuantity*Convert.ToDouble(unitprice)).ToString();
                row.Cells[3].Value = newamt;
                grndtotal -= Convert.ToDouble(oldamt);
                grndtotal += Convert.ToDouble(newamt);
                textBox3.Text =grndtotal.ToString();

            }
            
        }
    }
}
