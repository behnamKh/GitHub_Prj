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
    public partial class Report: Form
    {
        public Report()
        {
            InitializeComponent();
        }
        RPOSDBEntities db = new RPOSDBEntities(dbConnection.BuildEntityConnection());
        private void Report_Load(object sender, EventArgs e)
        {
            this.Height = 110;
            txt_Password.Focus();
            Task t = Task.Run(() =>
            {
                var q = db.T_appSettings.FirstOrDefault(x => x.item == "mp");
                if (q==null)
                {
                    T_appSettings apps = new T_appSettings()
                    {
                        item="mp",
                        value="2016"
                    };
                    db.T_appSettings.Add(apps);
                    db.SaveChanges();
                }



            });
           // fillgv1();
        }

        private void fillgv1()
        {
            DateTime s = Convert.ToDateTime(dateTimePicker1.Text).Date.AddSeconds(-1);
            DateTime s2 = Convert.ToDateTime(dateTimePicker2.Text).Date.AddDays(1).AddSeconds(-1);
            Task t = Task.Run(() =>
            {

                // var q = db.T_invoice.Where(x => x.OrderTime >= s && x.OrderTime <= s2).ToList();
                var q = (from i in db.T_invoice
                         where i.OrderTime >= s && i.OrderTime <= s2 &&i.CustomerName!="0"
                         select new { id=i.id,OrderNumber = i.OrderNo, OrderTime = i.OrderTime, TakeAwayOrDineIn = i.TD, Payment = i.Payment, Total = i.Total }).ToList();
                dataGridView1.Invoke(new Action(() => dataGridView1.DataSource = q));
                int count = dataGridView1.RowCount;
                l_count.Invoke(new Action(() => l_count.Text = string.Format("Number of bills: {0}", count.ToString())));
                double total = q.Sum(x => x.Total).Value;
                l_total.Invoke(new Action(() => l_total.Text = string.Format("Total: ${0}", total.ToString())));

            }).ContinueWith(x=>Gv1Ref());
           
            
        }

        private void Gv1Ref()
        {
            int w = dataGridView1.Width;
            dataGridView1.Invoke(new Action(() => {
                dataGridView1.Columns[0].Width = Convert.ToInt16(w * 0.01);
                dataGridView1.Columns[1].Width =Convert.ToInt16( w * 0.15);
                dataGridView1.Columns[2].Width = Convert.ToInt16(w * 0.3);
                dataGridView1.Columns[3].Width = Convert.ToInt16(w * 0.25);
                dataGridView1.Columns[4].Width = Convert.ToInt16(w * 0.15);
                dataGridView1.Columns[5].Width = Convert.ToInt16(w * 0.10);


            }));
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            fillgv1();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            fillgv1();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           
            Task t = Task.Run(() => {
                int id = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                // var BillsItem = db.T_invoiceItems.Where(x => x.id_invoice == id).ToList();
                var BillsItem = from i in db.T_invoiceItems
                                where i.id_invoice == id
                                select new {Name=i.ItemName,Qty=i.ItemNo,Price=i.ItemPrice,Comment=i.ItemComment };
                dataGridView2.Invoke(new Action(() => dataGridView2.DataSource = BillsItem.ToList()));



            }).ContinueWith(x=>GV2Ref());
        }

        private void GV2Ref()
        {
            int w = dataGridView2.Width;
            dataGridView2.Invoke(new Action(() => {
                dataGridView2.Columns[0].Width = Convert.ToInt16(w * 0.45);
                dataGridView2.Columns[1].Width = Convert.ToInt16(w * 0.15);
                dataGridView2.Columns[2].Width = Convert.ToInt16(w * 0.15);
                dataGridView2.Columns[3].Width = Convert.ToInt16(w * 0.2);
                


            }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var q = db.T_appSettings.FirstOrDefault(x => x.item == "mp");
            if (txt_Password.Text==q.value.ToString())
            {
                this.Height = 620;
                panel1.Visible = false;
                fillgv1();
            }
        }
    }
}
