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
    public partial class ProductUpdate : Form
    {
        public ProductUpdate(String x,String y)
        {
            InitializeComponent();
            pname.Text = x;
            price.Text = y; 
            if(pname.Text =="" && price.Text == "")
            {
                infoMsg.Text = "Add Product Details";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pname.Text != String.Empty && price.Text != String.Empty)
            {
                String conn = ConfigurationManager.ConnectionStrings["Con"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(conn);
                if (AdminForm.isUpdate)
                {
                    String sqlquery = "update [dbo].[ProductTable] set ProductName=@ProductName,ProductPrice=@ProductPrice where ProductId=" + AdminForm.id;
                    sqlcon.Open();
                    SqlCommand cmd = new SqlCommand(sqlquery, sqlcon);
                    cmd.Parameters.AddWithValue("@ProductName", pname.Text);
                    cmd.Parameters.AddWithValue("@ProductPrice", price.Text);
                    cmd.ExecuteNonQuery();
                    lblmsg.Text = "Product Details Updated successfully!!";
                    sqlcon.Close();
                }
                else
                {
                    String query = "select * from [dbo].[ProductTable] where ProductName = '" + pname.Text + "'";
                    sqlcon.Open();
                    SqlCommand cmd1=new SqlCommand(query, sqlcon);
                    SqlDataReader adr=cmd1.ExecuteReader();
                    if (adr.HasRows)
                    {
                        MessageBox.Show("This Product already exists in your store!!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        sqlcon.Close();
                        string sqlquery = "insert into [dbo].[ProductTable] values (@ProductName,@ProductPrice)";
                        sqlcon.Open();
                        SqlCommand cmd = new SqlCommand(sqlquery, sqlcon);
                        cmd.Parameters.AddWithValue("@ProductName", pname.Text);
                        cmd.Parameters.AddWithValue("@ProductPrice", price.Text);
                        cmd.ExecuteNonQuery();
                        lblmsg.Text = "Product Details Added successfully!!";
                        sqlcon.Close();
                        AdminForm.isUpdate = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter valid product name and price", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
           
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            
            AdminForm adminForm = new AdminForm();
            adminForm.Show();
        }
    }
}
