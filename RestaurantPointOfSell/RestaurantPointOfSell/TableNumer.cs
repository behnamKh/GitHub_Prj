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
    public partial class TableNumer : Form
    {
        public TableNumer()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            textBox1.Text += b.Text;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DataTransfer.tableNumber = textBox1.Text;
            this.Close();
        }

        private void TableNumer_Load(object sender, EventArgs e)
        {
            this.Location = new Point(10, Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);
        }
    }
}
