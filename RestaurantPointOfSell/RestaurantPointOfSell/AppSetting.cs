using RestaurantPointOfSell.Model;
using RestaurantPointOfSell.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestaurantPointOfSell
{
    public partial class AppSetting : Form
    {
        public AppSetting()
        {
            InitializeComponent();
        }
        RPOSDBEntities db = new RPOSDBEntities(dbConnection.BuildEntityConnection());
        private void AppSetting_Load(object sender, EventArgs e)
        {
            this.Height = 100;
            textBox4.Focus();
            try
            {
                
                var q = db.T_appSettings.ToList();
               
                txtAdminPass.Text= q.FirstOrDefault(x => x.item == "ap").value;
                var printn = q.FirstOrDefault(x => x.item == "pn");

                
                   var norp= q.Where(x => x.item == "norp").FirstOrDefault();
                    

                    txtPrintNum.Value = int.Parse(norp.value.ToString());

                var man = q.Where(x => x.item == "mp").FirstOrDefault();


                txtManPass.Text= man.value;


                checkBox1.Checked = (bool)Properties.Settings.Default.Dev;




            }
            catch 
            {
                
                
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar==13)
            {
                if (textBox4.Text=="b"+DateTime.Now.Hour+DateTime.Now.Minute)
                {
                    this.Height = 500;
                    panel1.Visible = false;
                    string conn = db.Database.Connection.ConnectionString;
                    System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(db.Database.Connection.ConnectionString);

                    string server = builder.DataSource;
                    string database = builder.InitialCatalog;
                    txtserver.Text = server;
                    txtdbname.Text = database;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {



                string connString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};MultipleActiveResultSets=True", txtserver.Text, txtdbname.Text, txtuser.Text, txt_pass.Text);
                //  connString = string.Format(txtserver.Text, txtdbname.Text, txtuser.Text, txt_pass.Text);

                dbConnection.interInfoConn(connString);
                MessageBox.Show("Connection set successfully...");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString(),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var res = db.T_appSettings.FirstOrDefault(x => x.item == "rn");
           
                res.value = txtResName.Text;


            var admin= db.T_appSettings.FirstOrDefault(x => x.item == "ap");
            admin.value = txtAdminPass.Text;

            var man = db.T_appSettings.FirstOrDefault(x => x.item == "mp");
            man.value = txtManPass.Text;

            var print=  db.T_appSettings.FirstOrDefault(x => x.item == "norp");
            print.value = txtPrintNum.Value.ToString();

            var ex = db.T_appSettings.FirstOrDefault(x => x.item == "e");
            if (ex!=null)
            {
                DateTime d = Convert.ToDateTime(ex.value);
                d=d.AddDays(int.Parse(numD.Value.ToString())).AddMonths(int.Parse(numM.Value.ToString()));
                ex.value = d.ToShortDateString();
            }
           

            db.SaveChanges();
            Properties.Settings.Default.Dev = checkBox1.Checked;
            Properties.Settings.Default.Save();
            this.Close();
        }
    }
}
