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
    public partial class Performa : Form
    {
        public Performa()
        {
            InitializeComponent();
        }
        float _total = 0;
        RPOSDBEntities db = new RPOSDBEntities(dbConnection.BuildEntityConnection());
        private void Performa_Load(object sender, EventArgs e)
        {
           
            this.Width = Convert.ToInt16(Screen.PrimaryScreen.Bounds.Width - Convert.ToInt16(Screen.PrimaryScreen.Bounds.Width * 0.66));
            this.Height = Convert.ToInt16(Screen.PrimaryScreen.Bounds.Height * 0.95);
            this.Location = new Point(Convert.ToInt16(Screen.PrimaryScreen.Bounds.Width * 0.66), 0);
            l_time.Location = new Point(0, 0);
            l_time.Width = this.Width;
            pictureBox7.Width = l_time.Height;
            pictureBox7.Height = l_time.Height;
            pictureBox7.Location = new Point(this.Width - pictureBox7.Width, 0);
            dataGridView1.Width = this.Width;
             dataGridView1.Height = Convert.ToInt16(this.Height * 0.4);
            dataGridView1.Location = new Point(0, l_time.Height);
            l_total.Width = this.Width;
            l_total.Location = new Point(0, dataGridView1.Height + l_time.Height);
            panel3.Location = new Point(0, dataGridView1.Height + l_time.Height + l_total.Height);
            panel3.Width = this.Width;
           
            l_item.Location = new Point(2, 2);
            txtItem.Width = Convert.ToInt16(panel3.Width * 0.4);
            txtItem.Location = new Point(2, l_item.Height + 3);
            l_price.Location= new Point(txtItem.Width + 5, 2);
            txtPrice.Width= Convert.ToInt16(panel3.Width * 0.3);
            txtPrice.Location = new Point(txtItem.Width+5, l_item.Height + 3);
            panel3.Height = l_item.Height + txtItem.Height + 15;
            pictureBox1.Height = panel3.Height - 2;
            pictureBox1.Width = pictureBox1.Height;
            pictureBox1.Location = new Point(panel3.Width - pictureBox1.Width - 2, 2);

            panel1.Location = new Point(0, dataGridView1.Height + l_time.Height + l_total.Height+panel3.Height);
            panel1.Width = this.Width;
            
            
            button1.Width = Convert.ToInt16(this.Width * 0.4);
            button1.Height = button1.Width / 2;
            panel1.Height = button1.Height + 5;
            panel2.Height = button1.Height + 5;
            button2.Width = Convert.ToInt16(this.Width * 0.4);
          
            panel2.Width = this.Width;
            panel2.Location = new Point(0, dataGridView1.Height + l_time.Height + l_total.Height + panel1.Height  + panel3.Height);
            button2.Height = button2.Width / 2;
            button1.Location = new Point(5, 1);
            button2.Location = new Point(button1.Width + 15, 1);
            button4.Width = Convert.ToInt16(this.Width * 0.45);
            button3.Width = Convert.ToInt16(this.Width * 0.2);
            button5.Width = Convert.ToInt16(this.Width * 0.3);
            button4.Location = new Point(5, 1);
            button3.Location = new Point(button4.Width+10, 1);
            button5.Location = new Point(button4.Width+button3.Width + 20, 1);
            button4.Height = button1.Height;
            button3.Height = button1.Height;
            button5.Height = button1.Height;
            button6.Height = button1.Height;
            button6.Width = Convert.ToInt16(this.Width * 0.80);
            button6.Location = new Point(this.Width/2-button6.Width/2, dataGridView1.Height + l_time.Height + l_total.Height + panel1.Height+panel2.Height+panel3.Height);
            txtPrice.Text = "0";
            txtItem.Text = "";
                
            if (DataTransfer.mId == 0)
            {


                Task t = Task.Run(() =>
                {
                    DateTime tom = DateTime.Today.AddDays(1);
                    DateTime yes = DateTime.Today.AddSeconds(-1);
                    var q1 = db.T_invoice.Where(x => x.OrderTime.Value < tom && x.OrderTime > yes).ToList();
                    string ordernumber = (100 + q1.Count()).ToString();
                    var check = q1.FirstOrDefault(x => x.OrderNo == ordernumber);
                    if (check != null)
                    {
                        ordernumber = (100 + q1.Count() + 1).ToString();
                    }
                    T_invoice ti = new T_invoice() { OrderTime=System.DateTime.Now,OrderNo=ordernumber };
                    db.T_invoice.Add(ti);
                    db.SaveChanges();
                    idInvoice = ti.id;
                   
                   
                    
                    
                    l_time.Invoke(new Action(() => l_time.Text = "Order#: " + ordernumber));
                });
            }
            else
            {
                l_time.Text = "Order Num: D" + DataTransfer.mOrderNum.ToString();
                var q = db.T_invoiceItems.Where(x => x.id_invoice == DataTransfer.mId).ToList();
                foreach (var item in q)
                {
                    DataTransfer.invoice.Rows.Add(item.ItemName, item.ItemNo, item.ItemPrice, item.ItemComment);
                }
                if (DataTransfer.mPayment=="Card")
                {
                    
                    button4.BackColor = Color.White;
                    button3.BackColor = Color.Yellow;
                    button5.BackColor = Color.White;
                    button4.Text = "$Cash";
                }
                else if (DataTransfer.mPayment == "Not Paid")
                {
                    button4.BackColor = Color.White;
                    button3.BackColor = Color.White;
                    button5.BackColor = Color.Red;

                    button4.Text = "$Cash";
                }
                else
                {
                    button4.Text = DataTransfer.mPayment;
                    button4.BackColor = Color.Yellow;
                    button3.BackColor = Color.White;
                    button5.BackColor = Color.White;
                }

                if (DataTransfer.mTD== "Take Away")
                {
                    button2.BackColor = Color.Green;
                    button1.BackColor = Color.DarkOrange;
                    button1.Text = "Dine In";
                }
                else
                {
                    button1.BackColor = Color.Green;
                    button2.BackColor = Color.DarkOrange;
                    
                    button1.Text =  DataTransfer.mTD;
                }
                
                fillGV();
            }

        }
        public  void fillGV()
        {
            dataGridView1.DataSource = DataTransfer.invoice;
            
            dataGridView1.Refresh();
            if (dataGridView1.RowCount == 0)
            {
                try
                {

               
                ((SecondDisplay)System.Windows.Forms.Application.OpenForms["SecondDisplay"]).closeInvoice();
                }
                catch 
                {

                  
                }
                var q = db.T_invoice.FirstOrDefault(x => x.id == idInvoice);
                if (q != null)
                {
                    db.T_invoice.Remove(q);
                    db.SaveChanges();

                }
                this.Close();
            }
            else
            {
                txtPrice.Text = "0";
                txtItem.Text = "";
                float total = 0;
                foreach (DataGridViewRow item in dataGridView1.Rows)
                {
                    total += float.Parse(item.Cells[3].Value.ToString());
                    dataGridView1.Rows[item.Index].Cells[0].Value = item.Index + 1;
                    dataGridView1.Rows[item.Index].Height = 50;

                }
                if (DataTransfer.mId == 0)
                {
                    l_total.Text = "Total Price: $" + total.ToString();
                }
                else
                    l_total.Text = "Total Price: $" + total.ToString() + "\n - Previous Payment: $" + DataTransfer.mTotal.ToString() ;
                _total = total;
                dataGridView1.Columns[0].Width = Convert.ToInt16(this.Width * 0.09);

                dataGridView1.Columns[1].Width = Convert.ToInt16(this.Width * 0.6);
                dataGridView1.Columns[2].Width = Convert.ToInt16(this.Width * 0.15);
                dataGridView1.Columns[3].Width = Convert.ToInt16(this.Width * 0.15);
                dataGridView1.Columns[4].Width = 1;
                dataGridView1.ClearSelection();
                try
                {

                
                ((SecondDisplay)System.Windows.Forms.Application.OpenForms["SecondDisplay"]).fillInvoice();
                }
                catch 
                {

                  
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
          
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataTransfer.invoice.Rows.RemoveAt(int.Parse(dataGridView1.CurrentRow.Index.ToString()));
            fillGV();
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
                button1.BackColor = Color.Green;
                button2.BackColor = Color.DarkOrange;
            TableNumer tn = new TableNumer();
            tn.ShowDialog();
            button1.Text = "Table No: " + DataTransfer.tableNumber;
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
                button2.BackColor = Color.Green;
                button1.BackColor = Color.DarkOrange;
            button1.Text = "Dine In";


        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (_total > 0)
            {


                Cash c = new Cash();
                c.ShowDialog();
                if (DataTransfer.total!=0)
                {
                    button4.Text = "Cash: $" + DataTransfer.total + "\n Change: $" + (DataTransfer.total - _total).ToString();
                    button4.BackColor = Color.Yellow;
                    button3.BackColor = Color.White;
                    button5.BackColor = Color.White;
                }
                else
                {
                    button4.BackColor = Color.White;
                    button3.BackColor = Color.White;
                    button5.BackColor = Color.Red;

                    button4.Text = "$Cash";
                }
                   

                
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button4.BackColor = Color.White;
            button3.BackColor = Color.Yellow;
            button5.BackColor = Color.White;
            button4.Text = "$Cash";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button4.BackColor = Color.White;
            button3.BackColor = Color.White;
            button5.BackColor = Color.Red;
            
            button4.Text = "$Cash";
        }
        string pay = "";
        string dt = "";
        int idInvoice = 0;
        private void button6_Click(object sender, EventArgs e)
        {
            


                if (button4.BackColor == Color.Yellow)
                {
                    pay = button4.Text;
                }
                else if (button3.BackColor == Color.Yellow)
                {
                    pay = button3.Text;
                }
                else
                {
                    pay = button5.Text;
                }

                if (button1.BackColor == Color.Green)
                {
                    dt = button1.Text;
                }
                else
                {
                    dt = button2.Text;
                }
            if (DataTransfer.mId == 0)
            {
                

                var ti = db.T_invoice.FirstOrDefault(x => x.id == idInvoice);
                ti.OrderTime = DateTime.Now;
                ti.Payment = pay;
                ti.TD = dt;
                ti.Total = _total;
                ti.OrderNo = l_time.Text.Substring(8, l_time.Text.Length - 8);
                foreach (DataGridViewRow item in dataGridView1.Rows)
                {
                    T_invoiceItems tii = new T_invoiceItems()
                    {
                        id_invoice = idInvoice,
                        ItemComment = item.Cells[4].Value.ToString(),
                        ItemName = item.Cells[1].Value.ToString(),
                        ItemNo = int.Parse(item.Cells[2].Value.ToString()),
                        ItemPrice = float.Parse(item.Cells[3].Value.ToString()),


                    };
                    db.T_invoiceItems.Add(tii);
                }
                db.SaveChanges();
            }
            else
            {
                var q = db.T_invoiceItems.Where(x => x.id_invoice == DataTransfer.mId).ToList();
                if (q.Count>0)
                {
                    db.T_invoiceItems.RemoveRange(q);
                    
                    var invo = db.T_invoice.FirstOrDefault(x => x.id == DataTransfer.mId);
                    invo.OrderTime = DateTime.Now;
                    invo.Payment = pay;
                    invo.TD = dt;
                    invo.Total = _total;
                    invo.OrderNo = "D" + DataTransfer.mOrderNum;
                    foreach (DataGridViewRow item in dataGridView1.Rows)
                    {
                        T_invoiceItems ti = new T_invoiceItems()
                        {
                            id_invoice = DataTransfer.mId,
                            ItemComment = item.Cells[4].Value.ToString(),
                            ItemName = item.Cells[1].Value.ToString(),
                            ItemNo = int.Parse(item.Cells[2].Value.ToString()),
                            ItemPrice = float.Parse(item.Cells[3].Value.ToString()),


                        };
                        db.T_invoiceItems.Add(ti);
                    }
                    db.SaveChanges();
                }
              
            }
            Print();
        }

        private void Print()
        {
            if (Properties.Settings.Default.Dev==true)
            {
                
                DataTransfer.pcustomer = l_time.Text;
                DataTransfer.ppay = pay;
                DataTransfer.ptd = dt;

                DataTransfer.ptotal = _total.ToString();
                BillPrint pb = new BillPrint();
                pb.ShowDialog();
                try
                {


                    ((SecondDisplay)System.Windows.Forms.Application.OpenForms["SecondDisplay"]).closeInvoice();
                }
                catch
                {


                }
            }
            else
            {
                T_printBillQu pb = new T_printBillQu() {

                    idInvoice = idInvoice,
                };
                db.T_printBillQu.Add(pb);
                db.SaveChanges();
            }
            
            this.Close();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
           
            DataTransfer.invoice.Clear();

            DataTransfer.mId = 0;
            DataTransfer.mOrderNum = "";
            DataTransfer.mPayment = "";
            DataTransfer.mTD = "";
            DataTransfer.mTotal = 0;
            var q = db.T_invoice.FirstOrDefault(x => x.id == idInvoice);
            if (q!=null)
            {
                db.T_invoice.Remove(q);
                db.SaveChanges();

            }
            this.Close();

        }

        private void Performa_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataTransfer.groupId = 0;
            DataTransfer.groupName = "";
            DataTransfer.invoice.Clear();
            DataTransfer.itemComment = "";
            DataTransfer.itemName = "";
            DataTransfer.itemNumber = 1;
            DataTransfer.itemPrice = 0;
            DataTransfer.pcustomer = "";
            DataTransfer.ppay = "";
            DataTransfer.ptd = "";
            DataTransfer.tableNumber = "";
            DataTransfer.total = 0;

            DataTransfer.mId = 0;
            DataTransfer.mOrderNum = "";
            DataTransfer.mPayment = "";
            DataTransfer.mTD = "";
            DataTransfer.mTotal = 0;
        }

        private void txtPrice_Enter(object sender, EventArgs e)
        {
            
        }

        private void txtPrice_MouseClick(object sender, MouseEventArgs e)
        {
            Cash c = new Cash();
            c.ShowDialog();
            if (DataTransfer.total != 0)
            {
                txtPrice.Text = DataTransfer.total.ToString();
                DataTransfer.total = 0;
            }
            else
                txtPrice.Text = "0";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (txtItem.Text!="" && txtPrice.Text!="0")
            {
                OrderNumber on = new OrderNumber();
                on.ShowDialog();
                if (DataTransfer.itemNumber>0)
                {
                    DataTransfer.invoice.Rows.Add(txtItem.Text, DataTransfer.itemNumber, float.Parse(txtPrice.Text.ToString()) * DataTransfer.itemNumber, DataTransfer.itemComment);
                    fillGV();
                }
            }
        }
    }
}
