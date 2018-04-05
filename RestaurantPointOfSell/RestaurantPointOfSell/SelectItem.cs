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
    public partial class SelectItem : Form
    {
        public SelectItem()
        {
            InitializeComponent();
        }
        RPOSDBEntities db = new RPOSDBEntities(dbConnection.BuildEntityConnection());
        private void SelectItem_Load(object sender, EventArgs e)
        {
            this.Width= Convert.ToInt16(Screen.PrimaryScreen.Bounds.Width * 0.66);
            this.Height = Convert.ToInt16(Screen.PrimaryScreen.Bounds.Height);
            this.Location = new Point(0, 0);
            label1.Text = DataTransfer.groupName;
            label1.Location = new Point(0,0);
            label1.Height = Convert.ToInt16(this.Width * 0.07);
            label1.Width = this.Width;
            panel1.Width = Convert.ToInt16(this.Width * 0.90);
            panel1.Height = Convert.ToInt16(this.Height * 0.8);
            panel1.Location = new Point(this.ClientSize.Width / 2 - panel1.Size.Width / 2,( this.ClientSize.Height / 2 - panel1.Size.Height / 2));
            pictureBox7.Height = label1.Height;
            pictureBox7.Width = pictureBox7.Height;

            pictureBox1.Height = label1.Height;
            pictureBox1.Width = pictureBox1.Height;
            pictureBox7.Location = new Point(1, 1);
            pictureBox7.Location = new Point(this.Width - pictureBox7.Width, 1);
            panel1.Anchor = AnchorStyles.None;
            Task t = Task.Run(() => {
            var items = db.t_items.Where(x => x.group_id == DataTransfer.groupId).OrderBy(x=>x.name).ToList();
            int repetition = 0;
            int w = 4;
            foreach (var item in items)
            {
                Button dynamicbutton = new Button();
                Label dynamiclabel = new Label();
                dynamiclabel.Text = "$"+item.price.Value.ToString();
                dynamiclabel.AutoSize = false;
                
                dynamicbutton.Tag = item.price.Value;
                dynamicbutton.Click += Dynamicbutton_Click;
                dynamicbutton.Text = item.name;
                dynamicbutton.Visible = true;
                if ((Convert.ToInt16(Screen.PrimaryScreen.Bounds.Width * 0.12) + repetition * Convert.ToInt16(Screen.PrimaryScreen.Bounds.Width * 0.15)) > panel1.Width)
                {
                    repetition = 0;
                    w += Convert.ToInt16(Screen.PrimaryScreen.Bounds.Width * 0.15);
                }
                dynamicbutton.Location = new Point(4 + repetition * Convert.ToInt16(Screen.PrimaryScreen.Bounds.Width * 0.15), w);
                dynamiclabel.Location = new Point(4 + repetition * Convert.ToInt16(Screen.PrimaryScreen.Bounds.Width * 0.15), w + Convert.ToInt16(Screen.PrimaryScreen.Bounds.Width * 0.12));
                dynamicbutton.Height = Convert.ToInt16(Screen.PrimaryScreen.Bounds.Width * 0.12);
                dynamicbutton.Width = Convert.ToInt16(Screen.PrimaryScreen.Bounds.Width * 0.12);
                dynamiclabel.Height = Convert.ToInt16(Screen.PrimaryScreen.Bounds.Width * 0.02);
                dynamiclabel.Width = Convert.ToInt16(Screen.PrimaryScreen.Bounds.Width * 0.12);
                dynamicbutton.BackColor = Color.FromArgb(40, 40, 40);
                dynamicbutton.ForeColor = Color.White;
                dynamicbutton.Font = new Font("Lucida Console", 16);
                dynamiclabel.Font = new Font("Lucida Console", 20);
                dynamiclabel.BackColor = Color.Khaki;
                dynamiclabel.TextAlign = ContentAlignment.MiddleCenter;
                dynamicbutton.Show();
                panel1.Invoke(new Action(() => panel1.Controls.Add(dynamicbutton)));
                panel1.Invoke(new Action(() => panel1.Controls.Add(dynamiclabel)));
                
                repetition++;

            }
            });
          
                //var ex = db.T_extra.Where(x => x.ItemGroupId == DataTransfer.groupId).ToList();
                //if (ex.Count > 0)
                //{
                //pictureBox1.Visible = true;
                //}
                //else
                //{
                //    pictureBox1.Visible = false;
                //}

            
        }

        private void Dynamicbutton_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            DataTransfer.itemName = b.Text;
            DataTransfer.itemPrice = float.Parse( b.Tag.ToString());
            OrderNumber on = new OrderNumber();
            on.ShowDialog();
            DataTransfer.invoice.Rows.Add(b.Text,DataTransfer.itemNumber, (DataTransfer.itemNumber*float.Parse(b.Tag.ToString())),DataTransfer.itemComment);
            var i = DataTransfer.invoice.Rows[0];

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

            ((Performa)System.Windows.Forms.Application.OpenForms["Performa"]).fillGV();





        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            DataTransfer.groupId = 0;
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ExtraItems ex = new ExtraItems();
            ex.Show();
        }
    }
}
