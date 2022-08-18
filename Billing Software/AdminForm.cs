using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace Billing_Software
{
    public partial class AdminForm : Form
    {

        public AdminForm()
        {
            InitializeComponent();
        }
        public static String id;
        public static bool isUpdate;
        private void AdminForm_Load(object sender, EventArgs e)
        {
            String conn = ConfigurationManager.ConnectionStrings["Con"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(conn);
            sqlcon.Open();
            SqlDataAdapter da;
            DataTable dt;
            da=new SqlDataAdapter("Select * from [dbo].[ProductTable]",sqlcon);
            dt= new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["ProductName"].ReadOnly = true;
            dataGridView1.Columns["ProductPrice"].ReadOnly = true;

            DataGridViewButtonColumn column=new DataGridViewButtonColumn();
            column.Name ="Update";
            column.DataPropertyName = "Update";
            column.Text = "Update";
            column.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(column);


            DataGridViewButtonColumn column1 = new DataGridViewButtonColumn();
            column1.Name = "Remove";
            column1.DataPropertyName = "Remove";
            column1.Text = "Remove";
            column1.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(column1);

            sqlcon.Close();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Update")
            {
                int row = dataGridView1.CurrentCell.RowIndex;
                id = dataGridView1.Rows[row].Cells[0].Value.ToString();
                String prname = dataGridView1.Rows[row].Cells[1].Value.ToString();
                String price = dataGridView1.Rows[row].Cells[2].Value.ToString();
                ProductUpdate productUpdate = new ProductUpdate(prname,price);
                productUpdate.Show();
                this.Hide();
                isUpdate = true;

            }
            else
            {
                if(dataGridView1.Columns[e.ColumnIndex].Name == "Remove")
                {
                    if (MessageBox.Show("Are you sure of removing Item?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        int row = dataGridView1.CurrentCell.RowIndex;
                        id = dataGridView1.Rows[row].Cells[0].Value.ToString();
                        String prname = dataGridView1.Rows[row].Cells[1].Value.ToString();
                        String conn = ConfigurationManager.ConnectionStrings["Con"].ConnectionString;
                        SqlConnection sqlcon = new SqlConnection(conn);
                        sqlcon.Open();
                        SqlCommand cmd = new SqlCommand("delete [dbo].[ProductTable] where ProductId=" + id, sqlcon);
                        cmd.ExecuteNonQuery();
                        this.Hide();
                        AdminForm form = new AdminForm();
                        form.Show();
                    }
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Login loginform=new Login();
            loginform.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isUpdate = false;
            this.Hide();
            ProductUpdate productUpdate = new ProductUpdate("","");
            productUpdate.Show();

        }
    }
}
