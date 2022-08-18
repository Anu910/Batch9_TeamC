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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        public static String UName = "";
        private void button1_Click(object sender, EventArgs e)
        {
            if(utxt.Text!=String.Empty && ptxt.Text != String.Empty ) 
            {
                if (utxt.Text != "IamAdmin")
                {
                    string conn = ConfigurationManager.ConnectionStrings["Con"].ConnectionString;
                    SqlConnection sqlconn = new SqlConnection(conn);
                    String sqlquery = "Select * from [dbo].[Customer] where userName=@userName and password=@password";
                    sqlconn.Open();
                    SqlCommand cmd = new SqlCommand(sqlquery, sqlconn);
                    cmd.Parameters.AddWithValue("@userName", utxt.Text);
                    cmd.Parameters.AddWithValue("@password", ptxt.Text);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    cmd.ExecuteNonQuery();
                    UName = utxt.Text;
                    if (dataTable.Rows.Count > 0)
                    {
                        this.Hide();
                        Order form1 = new Order();
                        form1.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Credentials Login again", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        utxt.Text = "";
                        ptxt.Text = "";
                    }
                }
                else
                {
                    if(utxt.Text=="IamAdmin" && ptxt.Text == "admin")
                    {
                        AdminForm af = new AdminForm();
                        this.Hide();
                        af.Show();
                    }
                }

            }
            else
            {
                MessageBox.Show("Please enter all the Values","Message",MessageBoxButtons.OK,MessageBoxIcon.Error);
                utxt.Text = "";
                ptxt.Text = "";
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Register register=new Register();
            register.Show();
            this.Hide();
        }
    }
}
