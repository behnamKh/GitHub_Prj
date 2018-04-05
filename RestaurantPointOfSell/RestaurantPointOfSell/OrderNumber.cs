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
    public partial class OrderNumber : Form
    {
        public OrderNumber()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DataTransfer.itemNumber = int.Parse(textBox1.Text);
            DataTransfer.itemComment = textBox2.Text;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "1";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "2";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "3";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "4";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void OrderNumber_Load(object sender, EventArgs e)
        {

            this.Location = new Point(10, Screen.PrimaryScreen.Bounds.Height/2-this.Height/2);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
