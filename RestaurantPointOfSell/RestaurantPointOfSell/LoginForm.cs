using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestaurantPointOfSell.Model;
using RestaurantPointOfSell.Services;
using System.Threading;

namespace RestaurantPointOfSell
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }
       
        private void LoginForm_Load(object sender, EventArgs e)
        {
            RPOSDBEntities db = new RPOSDBEntities(dbConnection.BuildEntityConnection());
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;

            var t = Task.Run(() => {

                try
                {
                    
                    pictureBox1.Invoke(new Action(() => pictureBox1.Visible = true));
                    
                    label1.Invoke(new Action(() => label1.Visible = true));
                    var q = db.t_items.ToList();
                    Thread.Sleep(3000);
                    var admin = db.T_appSettings.FirstOrDefault(x => x.item == "ap");
                    if (admin==null)
                    {
                        T_appSettings aps = new T_appSettings()
                        {
                                item="ap",
                                value="admin"
                        };
                        db.T_appSettings.Add(aps);
                       
                    }
                    var norp = db.T_appSettings.Where(x => x.item == "norp").FirstOrDefault();
                    if (norp == null)
                    {
                        T_appSettings apps = new T_appSettings()
                        {
                            item = "norp",
                            value = 1.ToString()
                        };
                        db.T_appSettings.Add(apps);
                        DataTransfer.SPrintNum = 1;
                    }
                    else
                    {
                        DataTransfer.SPrintNum = int.Parse(norp.value);
                    }

                    var res = db.T_appSettings.FirstOrDefault(x => x.item == "rn");
                    if (res == null)
                    {
                        T_appSettings apps = new T_appSettings()
                        {
                            item = "rn",
                            value = ""
                        };
                        db.T_appSettings.Add(apps);

                    }
                    else
                    {
                        DataTransfer.SResturantName = res.value;   
                    }
                    var man = db.T_appSettings.FirstOrDefault(x => x.item == "mp");
                    if (man == null)
                    {
                        T_appSettings apps = new T_appSettings()
                        {
                            item = "mp",
                            value = "2016"
                        };
                        db.T_appSettings.Add(apps);

                    }

                    var device = db.T_appSettings.FirstOrDefault(x => x.item == "d");
                    if (device == null)
                    {
                        T_appSettings apps = new T_appSettings()
                        {
                            item = "d",
                            value = "0"
                        };
                        db.T_appSettings.Add(apps);

                    }
                    
                                

                    db.SaveChanges();
                    string a = DateTime.Now.ToString();
                    var E = db.T_appSettings.FirstOrDefault(x => x.item == "e");
                    DateTime _e = Convert.ToDateTime(E.value);
                    if (DateTime.Now <= _e)
                    {


                        pictureBox1.Invoke(new Action(() => pictureBox1.Visible = false));
                        pictureBox2.Invoke(new Action(() => pictureBox2.Visible = true));
                        textBox1.Invoke(new Action(() => textBox1.Visible = true));
                        textBox1.Invoke(new Action(() => textBox1.Focus()));
                        label1.Invoke(new Action(() => label1.Text = "Please Enter The Password"));
                        return true;
                    }
                    else
                    {
                        label1.Invoke(new Action(() => { label1.Text = "The Application Has been Expired! \n Please contact us by Ben.Devenv@Gmail.com";
                            label1.ForeColor = Color.Red;
                        }));
                        pictureBox1.Invoke(new Action(() => pictureBox1.Visible = false));
                        textBox1.Invoke(new Action(() => textBox1.Visible = false));
                        pictureBox2.Invoke(new Action(() => pictureBox2.Visible = false));
                        return false;
                    }
                            
                    
                }
                catch (Exception ex)
                {
                    label1.Invoke(new Action(() => label1.Text = "Unable to connect to database! \n "+ex.Message));
                    pictureBox1.Invoke(new Action(() => pictureBox1.Visible = false));
                    textBox1.Invoke(new Action(() => textBox1.Visible = false));
                    pictureBox2.Invoke(new Action(() => pictureBox2.Visible = false));
                    return false;
                }
            });
            

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            RPOSDBEntities db = new RPOSDBEntities(dbConnection.BuildEntityConnection());
            string q = db.T_appSettings.FirstOrDefault(x => x.item == "ap").value.ToString();
            if (textBox1.Text == q)
            {
                DataTransfer.authen = true;
                if (Properties.Settings.Default.Dev==false)
                {
                    var dev = db.T_appSettings.FirstOrDefault(x => x.item == "d");
                    dev.value = "1";
                    db.SaveChanges();
                }
                

                this.Close();
            }
            else
            {
                label1.Text = "The password is wrong!";
                label1.ForeColor = Color.Red;
                textBox1.Text = "";
                textBox1.Focus();

            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.B))
            {
                AppSetting apps = new AppSetting();
                apps.ShowDialog();
                LoginForm_Load(null, null);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar==13)
            {
                pictureBox2_Click(null, null);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
