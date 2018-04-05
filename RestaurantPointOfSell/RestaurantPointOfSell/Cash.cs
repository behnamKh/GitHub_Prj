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
    public partial class Cash : Form
    {
        public Cash()
        {
            InitializeComponent();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        float cash = 0;
        private void Cash_Load(object sender, EventArgs e)
        {
            this.Width = Convert.ToInt16(Screen.PrimaryScreen.Bounds.Width * 0.66);
            this.Height = Convert.ToInt16(Screen.PrimaryScreen.Bounds.Height*0.95);
            this.Location = new Point(0, 0);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            cash += 100;
            textBox1.Text = cash.ToString("n2");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            cash += 50;
            textBox1.Text = cash.ToString("n2");
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            cash += 20;
            textBox1.Text = cash.ToString("n2");
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            cash += 10;
            textBox1.Text = cash.ToString("n2");
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            cash += 5;
            textBox1.Text = cash.ToString("n2");
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            
            cash=0;
            textBox1.Text = cash.ToString("n2");
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            DataTransfer.total = float.Parse(textBox1.Text);
            this.Close();
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            cash += 2;
            textBox1.Text = cash.ToString("n2");
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            cash += 1;
            textBox1.Text = cash.ToString("n2");
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            cash += 0.50f;
            textBox1.Text = cash.ToString("n2");
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            cash += 0.20f;
            textBox1.Text = cash.ToString("n2");
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            cash += 0.10f;
            textBox1.Text = cash.ToString("n2");
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            cash += 0.05f;
            textBox1.Text = cash.ToString("n2");
        }
    }
}
