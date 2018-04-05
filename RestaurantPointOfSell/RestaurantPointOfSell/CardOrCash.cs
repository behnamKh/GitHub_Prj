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
    public partial class CardOrCash: Form
    {
        public CardOrCash()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTransfer.Paidmethod = button1.Text;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTransfer.Paidmethod = button2.Text;
            this.Close();
        }

        private void CardOrCash_Load(object sender, EventArgs e)
        {
            this.Location = new Point(10, Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);
            label1.Text = string.Format("Order Number {0} paid by?", DataTransfer.PaidOrderNum);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            DataTransfer.Paidmethod = "";
            this.Close();
        }
    }
}
