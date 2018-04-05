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

namespace RestaurantPointOfSell
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        RPOSDBEntities db = new RPOSDBEntities(dbConnection.BuildEntityConnection());
        private void Main_Load(object sender, EventArgs e)
        {
            LoginForm lf = new LoginForm();
            lf.ShowDialog();
            if (DataTransfer.authen == false)
            {
                Application.Exit();
            }

            DataTransfer.invoice = new DataTable();
            DataTransfer.invoice.TableName = "invoice";
            DataTransfer.invoice.Columns.Add("Name");
            DataTransfer.invoice.Columns.Add("Number");
            DataTransfer.invoice.Columns.Add("Price");
            DataTransfer.invoice.Columns.Add("Comment");



            if (Properties.Settings.Default.Dev)
            {
                try
                {


                    SecondDisplay dlg = new SecondDisplay();
                    Screen[] screens = Screen.AllScreens;
                    if (screens.Count() > 1)
                    {
                        Rectangle bounds = screens[1].Bounds;
                        dlg.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
                        dlg.StartPosition = FormStartPosition.Manual;
                        dlg.Show();

                    }

                }
                catch
                {


                }

                pictureBox1.Visible = true;
                pictureBox1.Width = Convert.ToInt16(this.Size.Width * 0.1);
                pictureBox1.Height = pictureBox1.Width;
                pictureBox1.Location = new Point(10, 10);
                pictureBox2.Visible = true;
                pictureBox2.Width = Convert.ToInt16(this.Size.Width * 0.1);
                pictureBox2.Height = pictureBox2.Width;
                pictureBox2.Location = new Point(10 + pictureBox1.Width, 10);
                //timer1.Enabled = true;
               timer1.Start();
            }
            panel1.Width =Convert.ToInt16( this.Size.Width * 0.66);
            panel1.Height= Convert.ToInt16(this.Size.Height * 0.66);
            panel1.Location = new Point(10, Convert.ToInt16(this.Size.Height * 0.3));
            Task t = Task.Run(() => {
            int repetition = 0;
            int w = 4;
                try
                {

               
            var group = db.T_itemGroups.ToList();
                
                foreach (var item in group)
                {
                    Button dynamicbutton = new Button();
                    dynamicbutton.Tag = item.id;
                    dynamicbutton.Click += Dynamicbutton_Click;
                    dynamicbutton.Text = item.name;
                    dynamicbutton.Visible = true;
                    if ((Convert.ToInt16(this.Size.Width * 0.12) + repetition * Convert.ToInt16(this.Size.Width * 0.15)) > panel1.Width)
                    {
                        repetition = 0;
                        w += Convert.ToInt16(this.Size.Width * 0.15);
                    }
                    dynamicbutton.Location = new Point(4 + repetition * Convert.ToInt16(this.Size.Width * 0.15), w);

                    dynamicbutton.Height = Convert.ToInt16(this.Size.Width * 0.12);
                    dynamicbutton.Width = Convert.ToInt16(this.Size.Width * 0.12);
                    dynamicbutton.BackColor = Color.DarkCyan;
                    dynamicbutton.ForeColor = Color.White;
                    dynamicbutton.Font = new Font("Lucida Console", 16);
                    dynamicbutton.Show();
                    panel1.Invoke(new Action(() => panel1.Controls.Add(dynamicbutton))) ;
                    repetition++;
                }
                }
                catch 
                {

                 
                }
            });

        }
        

        private void Dynamicbutton_Click(object sender, EventArgs e)
        {
            try
            {

          
            Button b = (Button)sender;
            int id = int.Parse(b.Tag.ToString());
            DataTransfer.groupId = id;
            DataTransfer.groupName = b.Text;
            FormCollection fc = Application.OpenForms;
            bool flag = false;
            foreach (Form frm in fc)
            {
                if (frm.Text == "Performa")
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                Performa p = new Performa();

                p.TopMost = true;
                p.Show();
            }

            SelectItem si = new SelectItem();
            
            si.Show();
            }
            catch 
            {

              
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Report r = new Report();
            r.ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["Performa"];

            if (fc != null)
                fc.Close();

            ManageBills mb = new ManageBills();
            mb.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {

                Task t = Task.Run(() => {
                var check = db.T_appSettings.FirstOrDefault(x => x.item == "d");
                //if (check.value == "1")
                //{
                  var lPrint = db.T_printBillQu.ToList();
                if (lPrint.Count>0)
                {
                        bool to = false;
                        float _to = 0;
                    foreach (var item in lPrint)
                    {
                        var qInvoice = db.T_invoice.FirstOrDefault(x => x.id == item.idInvoice);
                            if (qInvoice.OrderNo==null)
                            {
                                DataTransfer.pcustomer = "Order#: ";
                            }
                            else
                        DataTransfer.pcustomer ="Order#: "+ qInvoice.OrderNo;
                            if (qInvoice.Payment==null)
                            {
                                DataTransfer.ppay = "";
                            }else
                        DataTransfer.ppay = qInvoice.Payment;
                            if (qInvoice.TD==null)
                            {
                                DataTransfer.ptd = "";
                            }else
                        DataTransfer.ptd = qInvoice.TD;
                            if (qInvoice.Total != null)
                            {


                                to = float.TryParse(qInvoice.Total.Value.ToString(), out _to);
                                if (to == false)
                                {
                                    var q = db.T_invoiceItems.Where(x => x.id_invoice == item.idInvoice).ToList();
                                    if (q.Count > 0)
                                    {
                                        _to = float.Parse(q.Sum(x => x.ItemPrice).Value.ToString());
                                    }
                                    else _to = 0;
                                }
                                DataTransfer.ptotal = _to.ToString();
                            }
                            else
                                DataTransfer.ptotal = "0";

                       var qInvoiceItem = db.T_invoiceItems.Where(x => x.id_invoice == item.idInvoice).ToList();
                        foreach (var item1 in qInvoiceItem)
                        {
                            DataTransfer.invoice.Rows.Add(item1.ItemName, item1.ItemNo, item1.ItemPrice, item1.ItemComment);
                        }
                        BillPrint pb = new BillPrint();
                        pb.ShowDialog();

                        db.T_printBillQu.Remove(item);
                    }
                    db.SaveChanges();
                }
               
           // }
                });
            }
            catch 
            {

               
            }
        }
    }
}
