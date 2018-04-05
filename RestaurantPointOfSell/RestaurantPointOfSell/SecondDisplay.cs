using RestaurantPointOfSell.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestaurantPointOfSell
{
    public partial class SecondDisplay : Form
    {
        public SecondDisplay()
        {
            InitializeComponent();
        }
        private void CreateIfMissing(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }
        private void SecondDisplay_Load(object sender, EventArgs e)
        {
            timer1.Enabled=true;
            panel1.Width = this.Width / 2;
            panel1.Height = this.Height;
            panel1.Location = new Point(0, 0);
            label2.Width = this.Width / 2;
            label2.Height = 200;
            label2.Location=new Point(this.Width/2,0);
            label1.Visible = false;
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\img";
            CreateIfMissing(path);
            // fillInvoice();
        }

        public void fillInvoice()
        {
            label1.Visible = true;
            label2.Visible = true;
            panel1.Visible = true;
            label1.Text = "";
            label2.Text = "";
            string temp = "";
            string s = "\n Order's Items: \n\n";
            int count = 0;
            float sum = 0;
            int len = 0;
            foreach (DataRow item in DataTransfer.invoice.Rows)
            {
                temp= string.Format("\n {0}- {1}X  {2}", count + 1, item[1].ToString(), item[0].ToString());
                len = 50 - temp.Length;
                for (int i = 0; i < len; i++)
                {
                    temp += " ";
                }
                s += temp + "$" + item[2].ToString();
                s += "\n      ----------------------      \n";
                sum += float.Parse(item[2].ToString());
                count++;

            }
            label2.Text = "Total Price: $" + sum.ToString("n2");
            label1.Text = s;
        }
        public void closeInvoice()
        {

            label2.Visible = false;
            panel1.Visible = false;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {

           
            var rand = new Random();
            string path= Path.GetDirectoryName(Application.ExecutablePath)+"\\img";
            var files = Directory.GetFiles(path, "*.jpg");
            pictureBox1.Image = System.Drawing.Bitmap.FromFile(files[rand.Next(files.Length)]);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
