using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laborotornaya_rabota
{
    public partial class ColorsForm : Form
    {
        HScrollBar redScrollBar, greenScrollBar, blueScrollBar;
        NumericUpDown redNumeric, greenNumeric, blueNumeric;
        PictureBox pb;
        Label redlbl, greenlbl, bluelbl;
        Button okbtn, cancelbtn, otherbtn;
        Color colorResult;
        Form1 main;

        public ColorsForm(Color color)
        {
            InitializeComponent();
            this.Height = 250;
            this.Width = 475;
            this.Text = "Colors";

            redlbl = new Label();
            redlbl.Text = "Red";
            redlbl.Size = new Size(40, 15);
            redlbl.Location = new Point(7, 50);

            greenlbl = new Label();
            greenlbl.Text = "Green";
            greenlbl.Size = new Size(40, 15);
            greenlbl.Location = new Point(7, 80);

            bluelbl = new Label();
            bluelbl.Text = "Blue";
            bluelbl.Size = new Size(40, 15);
            bluelbl.Location = new Point(7, 110);

            this.Controls.Add(redlbl);
            this.Controls.Add(greenlbl);
            this.Controls.Add(bluelbl);

            redScrollBar = new HScrollBar();
            redScrollBar.Minimum = 0;
            redScrollBar.Maximum = 255;
            redScrollBar.LargeChange = 1;
            redScrollBar.Size = new Size(200,20);
            redScrollBar.Location = new Point(60, 50);

            greenScrollBar = new HScrollBar();
            greenScrollBar.Minimum = 0;
            greenScrollBar.Maximum = 255;
            greenScrollBar.LargeChange = 1;
            greenScrollBar.Size = new Size(200, 20);
            greenScrollBar.Location = new Point(60, 80);

            blueScrollBar = new HScrollBar();
            blueScrollBar.Minimum = 0;
            blueScrollBar.Maximum = 255;
            blueScrollBar.LargeChange = 1;
            blueScrollBar.Size = new Size(200, 20);
            blueScrollBar.Location = new Point(60, 110);

            this.Controls.Add(redScrollBar);
            this.Controls.Add(greenScrollBar);
            this.Controls.Add(blueScrollBar);

            redNumeric = new NumericUpDown();
            redNumeric.Minimum = 0;
            redNumeric.Maximum = 255;
            redNumeric.Increment = 1;
            redNumeric.Size = new Size(50, 20);
            redNumeric.Location = new Point(270,50);

            greenNumeric = new NumericUpDown();
            greenNumeric.Minimum = 0;
            greenNumeric.Maximum = 255;
            greenNumeric.Increment = 1;
            greenNumeric.Size = new Size(50, 20);
            greenNumeric.Location = new Point(270, 80);

            blueNumeric = new NumericUpDown();
            blueNumeric.Minimum = 0;
            blueNumeric.Maximum = 255;
            blueNumeric.Increment = 1;
            blueNumeric.Size = new Size(50, 20);
            blueNumeric.Location = new Point(270, 110);

            this.Controls.Add(redNumeric);
            this.Controls.Add(greenNumeric);
            this.Controls.Add(blueNumeric);

            pb = new PictureBox();
            pb.BorderStyle = BorderStyle.Fixed3D;
            pb.Location = new Point(335,50);
            pb.Size = new Size(100, 100);

            this.Controls.Add(pb);

            okbtn = new Button();
            okbtn.Text = "Ok";
            okbtn.Size = new Size(55,25);
            okbtn.Location = new Point(7, 160);

            cancelbtn = new Button();
            cancelbtn.Text = "Cancel";
            cancelbtn.Size = new Size(55, 25);
            cancelbtn.Location = new Point(65, 160);

            otherbtn = new Button();
            otherbtn.Text = "Other Colors";
            otherbtn.Size = new Size(100, 25);
            otherbtn.Location = new Point(335, 160);

            this.Controls.Add(okbtn);
            this.Controls.Add(cancelbtn);
            this.Controls.Add(otherbtn);

            redScrollBar.Tag = redNumeric;
            greenScrollBar.Tag = greenNumeric;
            blueScrollBar.Tag = blueNumeric;

            redNumeric.Tag = redScrollBar;
            greenNumeric.Tag = greenScrollBar;
            blueNumeric.Tag = blueScrollBar;

            redNumeric.Value = color.R;
            greenNumeric.Value = color.G;
            blueNumeric.Value = color.B;

            redScrollBar.ValueChanged += RedScrollBar_ValueChanged;
            redNumeric.ValueChanged += RedNumeric_ValueChanged;

            greenScrollBar.ValueChanged += GreenScrollBar_ValueChanged;
            greenNumeric.ValueChanged += GreenNumeric_ValueChanged;

            blueScrollBar.ValueChanged += BlueScrollBar_ValueChanged;
            blueNumeric.ValueChanged += BlueNumeric_ValueChanged;


            okbtn.Click += Okbtn_Click;
            otherbtn.Click += Otherbtn_Click;
            cancelbtn.Click += Cancelbtn_Click;

            main = this.Owner as Form1;
        }

        private void Cancelbtn_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void Okbtn_Click(object? sender, EventArgs e)
        {
            UpdateColor();
            
            this.Close();
        }

        private void Otherbtn_Click(object? sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if(colorDialog.ShowDialog() == DialogResult.OK)
            {
                redScrollBar.Value = colorDialog.Color.R;
                greenScrollBar.Value = colorDialog.Color.G;
                blueScrollBar.Value = colorDialog.Color.B;

                colorResult = colorDialog.Color;
                UpdateColor();
            }
        }

        private void UpdateColor()
        {
            colorResult = Color.FromArgb(redScrollBar.Value, greenScrollBar.Value, blueScrollBar.Value);
            pb.BackColor = colorResult;
        }

        private void BlueNumeric_ValueChanged(object? sender, EventArgs e)
        {
            NumericUpDown numericUpDown = (NumericUpDown)sender;
            ScrollBar scrollBar = (ScrollBar)numericUpDown.Tag;
            scrollBar.Value = (int)numericUpDown.Value;
        }

        private void BlueScrollBar_ValueChanged(object? sender, EventArgs e)
        {
            ScrollBar scrollBar = (ScrollBar)sender;
            NumericUpDown numericUpDown = (NumericUpDown)scrollBar.Tag;
            numericUpDown.Value = scrollBar.Value;

            UpdateColor();
        }

        private void GreenNumeric_ValueChanged(object? sender, EventArgs e)
        {
            NumericUpDown numericUpDown = (NumericUpDown)sender;
            ScrollBar scrollBar = (ScrollBar)numericUpDown.Tag;
            scrollBar.Value = (int)numericUpDown.Value;
        }

        private void GreenScrollBar_ValueChanged(object? sender, EventArgs e)
        {
            ScrollBar scrollBar = (ScrollBar)sender;
            NumericUpDown numericUpDown = (NumericUpDown)scrollBar.Tag;
            numericUpDown.Value = scrollBar.Value;

            UpdateColor();
        }

        private void RedNumeric_ValueChanged(object? sender, EventArgs e)
        {
            NumericUpDown numericUpDown = (NumericUpDown)sender;
            ScrollBar scrollBar = (ScrollBar)numericUpDown.Tag;
            scrollBar.Value = (int)numericUpDown.Value;
        }

        private void RedScrollBar_ValueChanged(object? sender, EventArgs e)
        {
            ScrollBar scrollBar = (ScrollBar)sender;
            NumericUpDown numericUpDown = (NumericUpDown)scrollBar.Tag;
            numericUpDown.Value = scrollBar.Value;

            UpdateColor();
        }
    }
}
