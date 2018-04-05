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
    public partial class ManageBills : Form
    {
        public ManageBills()
        {
            InitializeComponent();
        }
        RPOSDBEntities db = new RPOSDBEntities(dbConnection.BuildEntityConnection());
        private void ManageBills_Load(object sender, EventArgs e)
        {
            this.Height = Convert.ToInt16(Screen.PrimaryScreen.Bounds.Height * 0.8);
            this.Width = Convert.ToInt16(Screen.PrimaryScreen.Bounds.Width * 0.4);
            this.Location = new Point(10, Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);
            dataGridView1.Height = Convert.ToInt16(this.Height * 0.80);
            panel1.Height = Convert.ToInt16(this.Height * 0.12);
            FillGrid();
        }

        private void FillGrid()
        {
            try
            {

           
            DateTime tom = DateTime.Today.AddDays(1);
            DateTime yes = DateTime.Today.AddSeconds(-1);
            var q1 = from x in db.T_invoice
                     where x.OrderTime.Value < tom && x.OrderTime > yes && x.CustomerName != "0"
                     orderby x.Payment descending
                     select new { x.id, Order_Number = x.OrderNo, Order_Time = x.OrderTime, Payment = x.Payment, TakeAway_DineIn = x.TD, Total_Price = x.Total };



            dataGridView1.DataSource = q1.ToList();
            int w = dataGridView1.Width;
            dataGridView1.Columns[0].Width = 1;
            dataGridView1.Columns[1].Width = Convert.ToInt16(w * 0.15);
            dataGridView1.Columns[2].Width = Convert.ToInt16(w * 0.3);
            dataGridView1.Columns[3].Width = Convert.ToInt16(w * 0.2);
            dataGridView1.Columns[4].Width = Convert.ToInt16(w * 0.2);
            dataGridView1.Columns[5].Width = Convert.ToInt16(w * 0.1);
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                item.Height = 40;
                if (item.Cells[3].Value.ToString() == "Not Paid")
                {
                    item.Cells[0].Style.BackColor = Color.Red;
                    item.Cells[0].Style.ForeColor = Color.White;

                    item.Cells[1].Style.BackColor = Color.Red;
                    item.Cells[1].Style.ForeColor = Color.White;

                    item.Cells[2].Style.BackColor = Color.Red;
                    item.Cells[2].Style.ForeColor = Color.White;

                    item.Cells[3].Style.BackColor = Color.Red;
                    item.Cells[3].Style.ForeColor = Color.White;

                    item.Cells[4].Style.BackColor = Color.Red;
                    item.Cells[4].Style.ForeColor = Color.White;

                    item.Cells[5].Style.BackColor = Color.Red;
                    item.Cells[5].Style.ForeColor = Color.White;
                }
            }
            dataGridView1.ClearSelection();
            }
            catch 
            {

               
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataTransfer.mId = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            DataTransfer.mOrderNum = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            DataTransfer.mPayment = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            DataTransfer.mTD = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            DataTransfer.mTotal =float.Parse( dataGridView1.CurrentRow.Cells[5].Value.ToString());

            Performa p = new Performa();
            p.TopMost = true;
            p.Show();

            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {


                DialogResult r = MessageBox.Show("Are you sure to remove the Bill?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (r == DialogResult.Yes)
                {
                    int id = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                    var q = db.T_invoice.FirstOrDefault(x => x.id == id);
                    q.CustomerName = "0";
                    db.SaveChanges();
                    FillGrid();
                    
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index >= 0)
            {
                DataTransfer.PaidOrderNum = dataGridView1.CurrentRow.Cells[1].Value.ToString();

                CardOrCash coc = new CardOrCash();
                coc.ShowDialog();
                if (DataTransfer.Paidmethod != "")
                {
                    int id = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                    var q = db.T_invoice.FirstOrDefault(x => x.id == id);
                    q.Payment = DataTransfer.Paidmethod;
                    db.SaveChanges();
                    FillGrid();

                }
            }
            DataTransfer.PaidOrderNum = "";
            DataTransfer.Paidmethod = "";
        }
    }
}
