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
    public partial class PrintReciept : Form
    {
        public PrintReciept()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {

            RichTextBox textbox = richTextBox1;
            PrintDocument docToPrint = new PrintDocument();
            docToPrint.PrintPage += new PrintPageEventHandler(PrintPageHandler);
            docToPrint.DocumentName = "print___";
            printDialog1.Document = docToPrint;

           
                docToPrint.Print();
            

        }
        private void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            
                RichTextBox textbox = richTextBox1;
                StringReader reader = new StringReader(textbox.Text);
                float linesPerPage = 0.0f;
                float yPosition = 0.0f;
                int count = 0;
                float leftMargin = e.MarginBounds.Left;
                float rightMargin = e.MarginBounds.Right;
                float topMargin = e.MarginBounds.Top;
                string line = null;
           
                Font printFont = textbox.Font; //maybe do selection font
                SolidBrush printBrush = new SolidBrush(textbox.ForeColor);//Maybe do selection color
                int charPos = 0;
                int xPosition = (int)leftMargin;

                linesPerPage = e.MarginBounds.Height / printFont.GetHeight(e.Graphics);

                while (count < linesPerPage && ((line = reader.ReadLine()) != null))
                {
                    xPosition = (int)leftMargin;
                    yPosition = topMargin + (count * printFont.GetHeight(e.Graphics));
                    count++;
                    for (int i = 0; i < line.Length; i++)
                    {
                        textbox.Select(charPos, 1);
                        if ((xPosition + ((int)e.Graphics.MeasureString(textbox.SelectedText, textbox.SelectionFont).Width)) > rightMargin)
                        {
                            count++;
                            if (!(count < linesPerPage))
                            {
                                break;
                            }
                            xPosition = (int)leftMargin;
                            yPosition = topMargin + (count * printFont.GetHeight(e.Graphics));
                        }
                        printBrush = new SolidBrush(textbox.SelectionColor);
                        e.Graphics.DrawString(textbox.SelectedText, textbox.SelectionFont, printBrush, new PointF(xPosition, yPosition));
                        xPosition += ((int)e.Graphics.MeasureString(textbox.SelectedText, textbox.SelectionFont).Width);
                        charPos++;
                    }
                }

                if (line != null)
                {
                    e.HasMorePages = true;
                }
                else
                {
                    e.HasMorePages = false;
                    printBrush.Dispose();
                }

            }
        }
    }

