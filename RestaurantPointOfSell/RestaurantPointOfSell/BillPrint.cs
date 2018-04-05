using RestaurantPointOfSell.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestaurantPointOfSell
{
    public partial class BillPrint : Form
    {
        public BillPrint()
        {
            InitializeComponent();
        }
        PrintDocument pdoc;
        private void BillPrint_Load(object sender, EventArgs e)
        {
            string s = DateTime.Now.ToString() + "\n";
            string item = "";
            s += DataTransfer.ptd;

            s += "\n Order Number:"+DataTransfer.pcustomer;
            int c = 0;
            for (int i = 0; i < DataTransfer.invoice.Rows.Count; i++)
            {
                item = "";
                item = "\n" + (i + 1).ToString() + "- " + DataTransfer.invoice.Rows[i][1] + "X " +
                    DataTransfer.invoice.Rows[i][0];
                c = 50 - item.Length;
                for (int j = 0; j < c; j++)
                {
                    item += " ";
                }
                s += item + "$" + DataTransfer.invoice.Rows[i][2] + "\n\t" +
                    DataTransfer.invoice.Rows[i][3] + "\n-----------------------------";
            }
            s += "\n ";
            s += "Total Price: $" + DataTransfer.ptotal;
            s += "\n Payment: " + DataTransfer.ppay;

            richTextBox1.Text = s;
            int start = richTextBox1.GetFirstCharIndexFromLine(0);

            int length = richTextBox1.Lines[0].Length;

            richTextBox1.Select(start, length);
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox1.Select(0, 0);
            
           

            pdoc = new PrintDocument();
            PaperSize psize = new PaperSize("Custom", 450, 820);
            pdoc.DefaultPageSettings.PaperSize = psize;
            pdoc.PrintPage += new PrintPageEventHandler(pdoc_PrintPage);
            for (int i = 0; i < DataTransfer.SPrintNum; i++)
            {
                pdoc.Print();
            }
           
            clearData();


        }
        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            char[] param = { '\n' };

            if (printDialog1.PrinterSettings.PrintRange == PrintRange.Selection)
            {
                lines = richTextBox1.SelectedText.Split(param);
            }
            else
            {
                lines = richTextBox1.Text.Split(param);
            }

            int i = 0;
            char[] trimParam = { '\r' };
            foreach (string s in lines)
            {
                lines[i++] = s.TrimEnd(trimParam);
            }
        }
        private int linesPrinted;
        private string[] lines;

        private void OnPrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            PaperSize pkCustomSize1 = new PaperSize("First custom size", 100, 200);
            printDocument1.DefaultPageSettings.PaperSize = pkCustomSize1;
            int x = 2;
            int y = 2;
            Brush brush = new SolidBrush(richTextBox1.ForeColor);

            while (linesPrinted < lines.Length)
            {
                e.Graphics.DrawString(lines[linesPrinted++],
                    richTextBox1.Font, brush, x, y);
                y += 15;
                if (y >= e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            linesPrinted = 0;
            e.HasMorePages = false;
        }
        private void clearData()
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
            this.Close();
        }

        void pdoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Font font = new Font("Lucida Console", 9);
            Font fontTitr = new Font("Lucida Console", 12, FontStyle.Bold);
            Font fontfooter = new Font("Lucida Console", 6, FontStyle.Bold);
            var brush = new SolidBrush(Color.Black);
            float fontHeight = font.GetHeight();
            int startX = 5;
            int startY = 35;
            int Offset = 5;
            string resName = DataTransfer.SResturantName;
            string comment;
            
            int kComment = 0;
            if (resName.Length<32)
            {
                int k = (25 - resName.Length) / 2;
                for (int i = 0; i < k; i++)
                {
                    resName = " "+resName;
                }
            }
            graphics.DrawString(resName, fontTitr, brush, startX, startY + Offset);
            Offset = Offset + 30;
            graphics.DrawString(DataTransfer.ptd, font, brush, startX, startY + Offset);
            Offset = Offset + 30;
            graphics.DrawString(DataTransfer.pcustomer, fontTitr, brush, startX, startY + Offset);
            Offset = Offset + 40;
            graphics.DrawString(DateTime.Now.ToString(), font, brush, startX, startY + Offset);
            Offset = Offset + 20;
           
           
            graphics.DrawString("----------------------------------", font, brush, startX, startY + Offset);
            Offset = Offset + 20;
            string item = "";
            int c = 0;
            int lineCount = 1;
            for (int i = 0; i < DataTransfer.invoice.Rows.Count; i++)
            {
                lineCount = 1;
                item = "";
                if (DataTransfer.invoice.Rows[i][0].ToString().Length > 30)
                {
                    DataTransfer.invoice.Rows[i][0] = DataTransfer.invoice.Rows[i][0].ToString().Insert(30, "\n");
                    lineCount++;
                }
                item = "\n" + (i + 1).ToString() + "- " + DataTransfer.invoice.Rows[i][1] + "X \n " +
                    DataTransfer.invoice.Rows[i][0]+"\n";
                lineCount+=2;
                c = 32 - DataTransfer.invoice.Rows[i][2].ToString().Length;
                for (int j = 0; j < c; j++)
                {
                    item += " ";
                }
                graphics.DrawString(item + "$" + DataTransfer.invoice.Rows[i][2], font, brush, startX, startY + Offset);
                Offset = Offset + 20*lineCount;
                comment = "";
                comment = DataTransfer.invoice.Rows[i][3].ToString();
                kComment = 1;
                if (comment.Length>32)
                {
                    kComment++;
                    comment = comment.Insert(30, "\n");
                }
                graphics.DrawString("@ "+ comment, font, brush, startX, startY + Offset);
                Offset = Offset + 20*kComment;
                graphics.DrawString("----------------------------------", font, brush, startX, startY + Offset);
                Offset = Offset + 20;

            }
           

            
            graphics.DrawString("\n Total Price: $" + DataTransfer.ptotal, font, brush, startX, startY + Offset);
            Offset = Offset + 30;
            
            graphics.DrawString("Payment: " + DataTransfer.ppay, font, brush, startX, startY + Offset);
            
            Offset = Offset + 40;
            graphics.DrawString("Developed by: www.StarPerse.com" , fontfooter, brush, startX, startY + Offset);
            e.HasMorePages = false;
        }
    }
}
