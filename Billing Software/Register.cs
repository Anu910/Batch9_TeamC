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
    public partial class Register : Form
    {
        public static String mobile="";

        public Register()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox5.Text != String.Empty || ptxt.Text != String.Empty || utxt.Text != String.Empty || mtxt.Text!=String.Empty || etxt.Text!= String.Empty)
            {
                //mobile = mtxt.Text;
                if (ptxt.Text == textBox5.Text)
                {
                    string conn = ConfigurationManager.ConnectionStrings["Con"].ConnectionString;
                    SqlConnection sqlconn = new SqlConnection(conn);
                    string sqlquery = "insert into [dbo].[Customer] values (@userName,@email,@password,@mobileNumber)";
                    sqlconn.Open();
                    SqlCommand cmd = new SqlCommand(sqlquery, sqlconn);
                    cmd.Parameters.AddWithValue("@userName", utxt.Text);
                    cmd.Parameters.AddWithValue("@email", etxt.Text);
                    cmd.Parameters.AddWithValue("@password", ptxt.Text);
                    cmd.Parameters.AddWithValue("@mobileNumber", mtxt.Text);
                    cmd.ExecuteNonQuery();
                    
                    lblMsg.Text = "User " + utxt.Text + " successfully registered please login";
                    sqlconn.Close();
                    
                }
                else
                {
                    MessageBox.Show("Password doesn't match", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter value in all field.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Login loginform = new Login();
            loginform.Show();
        }

       
    }
}
