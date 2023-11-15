using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Laborotornaya_rabota
{
    public partial class Form1 : Form
    {
        MenuStrip ms;
        ToolStrip ts;
        Panel panel;
        PictureBox pb, pbsize;
        TrackBar tb;
        Label lbl, lblXY;

        bool drawing;
        int historyCounter;

        GraphicsPath currentPath;
        Point oldLocation;
        public Pen currentPen;
        List<Image> History;

        ToolStripMenuItem Solid = new ToolStripMenuItem("Solid");
        ToolStripMenuItem Dot = new ToolStripMenuItem("Dot");
        ToolStripMenuItem DashDotDot = new ToolStripMenuItem("DashDotDot");

        float zoom = 1.0f;
        public Form1()
        {
            InitializeComponent();
            this.Height = 600;
            this.Width = 850;
            this.Text = "Graf red";

            Image new_file = new Bitmap("../../../new.png");
            Image open = new Bitmap("../../../open.png");
            Image save = new Bitmap("../../../Save.png");
            Image exit = new Bitmap("../../../Exit.png");
            Image undo = new Bitmap("../../../Undo.png");
            Image redo = new Bitmap("../../../redo.png");
            Image penpng = new Bitmap("../../../pen.png");
            Image about = new Bitmap("../../../About.png");
            Image palitra = new Bitmap("../../../palitra.png");


            ms = new MenuStrip();
            ms.Dock = DockStyle.Top;
            ToolStripMenuItem File = new ToolStripMenuItem("File");
            ToolStripMenuItem New = new ToolStripMenuItem("New", new_file);
            ToolStripMenuItem Open = new ToolStripMenuItem("Open", open);
            ToolStripMenuItem Save = new ToolStripMenuItem("Save", save);
            ToolStripMenuItem Exit = new ToolStripMenuItem("Exit", exit);

            ToolStripMenuItem Edit = new ToolStripMenuItem("Edit");
            ToolStripMenuItem Undo = new ToolStripMenuItem("Undo", undo);
            ToolStripMenuItem Redo = new ToolStripMenuItem("Redo", redo);
            ToolStripMenuItem PenMenu = new ToolStripMenuItem("Pen", penpng);
            ToolStripMenuItem Colors = new ToolStripMenuItem("Color",palitra);
            ToolStripMenuItem Style = new ToolStripMenuItem("Style");
            
            ToolStripMenuItem Help = new ToolStripMenuItem("Help");
            ToolStripMenuItem About = new ToolStripMenuItem("About", about);

            PenMenu.DropDownItems.Add(Colors);
            PenMenu.DropDownItems.Add(Style);
            Style.DropDownItems.Add(Solid);
            Style.DropDownItems.Add(Dot);
            Style.DropDownItems.Add(DashDotDot);
            Solid.Checked = true;

            File.DropDownItems.Add(New);
            File.DropDownItems.Add(Open);
            File.DropDownItems.Add(Save);
            File.DropDownItems.Add(Exit);

            Edit.DropDownItems.Add(Undo);
            Edit.DropDownItems.Add(Redo);
            Edit.DropDownItems.Add(PenMenu);

            Help.DropDownItems.Add(About);

            ms.Items.Add(File);
            ms.Items.Add(Edit);
            ms.Items.Add(Help);
            
            About.Click += About_Click;
            Save.Click += Save_Click;
            New.Click += New_Click;
            Open.Click += Open_Click;
            Undo.Click += Undo_Click;
            Redo.Click += Redo_Click;
            Solid.Click += Solid_Click;
            Dot.Click += Dot_Click;
            DashDotDot.Click += DashDotDot_Click;
            Colors.Click += Colors_Click;
            Exit.Click += Exit_Click;

            About.ShortcutKeys = Keys.F1;
            New.ShortcutKeys = Keys.Control | Keys.N;
            Open.ShortcutKeys = Keys.F3;
            Save.ShortcutKeys = Keys.Control | Keys.S;
            Exit.ShortcutKeys = Keys.Alt | Keys.X;
            Undo.ShortcutKeys = Keys.Control | Keys.Z;
            Redo.ShortcutKeys = Keys.Control | Keys.Y;


            ts = new ToolStrip();
            ToolStripMenuItem NewTs = new ToolStripMenuItem(new_file);
            ToolStripMenuItem OpenTs = new ToolStripMenuItem(open);
            ToolStripMenuItem SaveTs = new ToolStripMenuItem(save);
            ToolStripMenuItem ColorsTs = new ToolStripMenuItem(palitra);
            ToolStripMenuItem ExitTs = new ToolStripMenuItem(exit);

            NewTs.Click += NewTs_Click; ;
            OpenTs.Click += OpenTs_Click; ;
            SaveTs.Click += SaveTs_Click;
            ColorsTs.Click += ColorsTs_Click;
            ExitTs.Click += ExitTs_Click;

            ts.Items.Add(NewTs);
            ts.Items.Add(OpenTs);
            ts.Items.Add(SaveTs);
            ts.Items.Add(ColorsTs);
            ts.Items.Add(ExitTs);
            ts.Dock = DockStyle.Left;

            this.Controls.Add(ms);
            this.Controls.Add(ts);


            //panel
            panel = new Panel();
            tb = new TrackBar();
            lbl = new Label();
            lblXY= new Label();

            panel.Location = new Point(ts.Width, ts.Height - 50);
            panel.Size = new Size(Width - 75, 28);
            panel.BorderStyle = BorderStyle.Fixed3D;
            panel.Text = "0,0";
            panel.ForeColor = Color.Black;

            tb.Size = new Size(panel.Width / 3, panel.Height);
            tb.Location = new Point(514, 0);
            tb.SetRange(1,50);

            lbl.Text = tb.Value.ToString();
            lbl.Location = new Point(485,0);
            lbl.Size = new Size(20, 20);

            lblXY.Size = new Size(100, 20);
            lblXY.Text = "X: , Y: ";
            
            panel.Controls.Add(lblXY);
            panel.Controls.Add(tb);
            panel.Controls.Add(lbl);
            this.Controls.Add(panel);

            tb.Scroll += Tb_Scroll;

            //picturebox
            pb = new PictureBox();
            pb.BorderStyle = BorderStyle.Fixed3D;
            pb.Location = new Point(ts.Width, ms.Height);
            pb.Size = new Size(775, 480);

            pbsize = new PictureBox();
            pbsize.Size = new Size(775, 480);
            this.Controls.Add(pb);
            
            pb.Image = null;

            pb.MouseDown += Pb_MouseDown;
            pb.MouseUp += Pb_MouseUp;
            pb.MouseMove += Pb_MouseMove;
            pb.MouseWheel += Pb_MouseWheel;
            pb.SendToBack();

            drawing = false;
            currentPen = new Pen(Color.Black);
            currentPen.Width = tb.Value;
            History = new List<Image>();
            
        }

        private void Pb_MouseWheel(object? sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                if (e.Delta > 0)
                {
                    zoom *= 1.1f;
                }
                else
                {
                    zoom /= 1.1f;
                }

                Zoom();
            }
        }

        private void Zoom()
        {
            int newWidth = (int)(pbsize.Width * zoom);
            int newHeight = (int)(pbsize.Height * zoom);
            Bitmap pic = new Bitmap(pbsize.Image, newWidth, newHeight);
            pb.Image = pic;
        }

        private void DashDotDot_Click(object? sender, EventArgs e)
        {
            currentPen.DashStyle = DashStyle.DashDotDot;

            Solid.Checked = false;
            Dot.Checked = false;
            DashDotDot.Checked = true;
        }

        private void Dot_Click(object? sender, EventArgs e)
        {
            currentPen.DashStyle = DashStyle.Dot;

            Solid.Checked = false;
            Dot.Checked = true;
            DashDotDot.Checked = false;
        }

        private void Solid_Click(object? sender, EventArgs e)
        {
            currentPen.DashStyle = DashStyle.Solid;

            Solid.Checked = true;
            Dot.Checked = false;
            DashDotDot.Checked = false;
        }

        private void Redo_Click(object? sender, EventArgs e)
        {
            if (historyCounter < History.Count - 1)
            {
                pb.Image = new Bitmap(History[++historyCounter]);
            }
            else MessageBox.Show("Ajalugu pole");
        }

        private void Undo_Click(object? sender, EventArgs e)
        {
            if (History.Count != 0 && historyCounter != 0)
            {
                pb.Image = new Bitmap(History[--historyCounter]);
            }
            else MessageBox.Show("Ajalugu pole");
        }

        private void Pb_MouseMove(object? sender, MouseEventArgs e)
        {
            if(drawing)
            {
                Graphics g = Graphics.FromImage(pb.Image);
                currentPath.AddLine(oldLocation, e.Location);
                g.DrawPath(currentPen,currentPath);
                oldLocation = e.Location;
                g.Dispose();
                pb.Invalidate();
            }
            lblXY.Text = $"X: {e.X}, Y: {e.Y}";
        }

        private void Pb_MouseUp(object? sender, MouseEventArgs e)
        {
            History.RemoveRange(historyCounter + 1, History.Count - historyCounter - 1);
            History.Add(new Bitmap(pb.Image));
            if (historyCounter + 1 < 10) historyCounter++;
            if (History.Count - 1 == 10) History.RemoveAt(0);
            drawing = false;
            pbsize.Image = pb.Image;
            try
            {
                currentPath.Dispose();
            }
            catch { };
        }

        private void Pb_MouseDown(object? sender, MouseEventArgs e)
        {
            if (pb.Image == null)
            {
                MessageBox.Show("You need create a new file!");
                return;
            }
            if(e.Button == MouseButtons.Left)
            {
                drawing = true;
                oldLocation = e.Location;
                currentPath = new GraphicsPath();
            }
        }


        private void ExitTs_Click(object? sender, EventArgs e)
        {
            if (pb.Image != null)
            {
                var result = MessageBox.Show("Сохранить текущее изображение перед созданием нового рисунка?", "Предупреждение", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.No: break;
                    case DialogResult.Yes: Save_Click(sender, e); break;
                    case DialogResult.Cancel: return;
                }
            }
            this.Close();
        }

        private void Exit_Click(object? sender, EventArgs e)
        {
            if (pb.Image != null)
            {
                var result = MessageBox.Show("Сохранить текущее изображение перед созданием нового рисунка?", "Предупреждение", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.No: break;
                    case DialogResult.Yes: Save_Click(sender, e); break;
                    case DialogResult.Cancel: return;
                }
            }
            this.Close();
        }

        private void ColorsTs_Click(object? sender, EventArgs e)
        {
            ColorsForm colorsForm = new ColorsForm(currentPen.Color);
            colorsForm.Owner = this;
            colorsForm.ShowDialog();
        }

        private void Colors_Click(object? sender, EventArgs e)
        {
            ColorsForm colorsForm = new ColorsForm(currentPen.Color);
            colorsForm.Owner = this;
            colorsForm.ShowDialog();
        }

        private void NewTs_Click(object? sender, EventArgs e)
        {
            History.Clear();
            historyCounter = 0;
            if (pb.Image != null)
            {
                var result = MessageBox.Show("Сохранить текущее изображение перед созданием нового рисунка?", "Предупреждение", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.No: break;
                    case DialogResult.Yes: Save_Click(sender, e); break;
                    case DialogResult.Cancel: return;
                }
            }
            Bitmap pic = new Bitmap(775, 480);
            pb.Image = pic;
            pbsize.Image = pb.Image;
            History.Add(new Bitmap(pb.Image));
        }

        private void New_Click(object? sender, EventArgs e)
        {
            History.Clear();
            historyCounter = 0;
            
            if (pb.Image != null)
            {
                var result = MessageBox.Show("Сохранить текущее изображение перед созданием нового рисунка?", "Предупреждение", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.No: break;
                    case DialogResult.Yes: Save_Click(sender, e); break;
                    case DialogResult.Cancel: return;
                }
            }
            Bitmap pic = new Bitmap(775, 480);
            pb.Image = pic;
            pbsize.Image = pb.Image;
            History.Add(new Bitmap(pb.Image));
        }

        private void OpenTs_Click(object? sender, EventArgs e)
        {
            if (pb.Image != null)
            {
                var result = MessageBox.Show("Сохранить текущее изображение перед созданием нового рисунка?", "Предупреждение", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.No: break;
                    case DialogResult.Yes: Save_Click(sender, e); break;
                    case DialogResult.Cancel: return;
                }
            }
            History.Clear();
            historyCounter = 0;
            Bitmap pic = new Bitmap(775, 480);
            pb.Image = pic;
            History.Add(new Bitmap(pb.Image));
            OpenFileDialog OP = new OpenFileDialog();
            OP.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            OP.Title = "Open an image file";
            OP.FilterIndex = 1;
            if (OP.ShowDialog() != DialogResult.Cancel)
            {
                pb.Load(OP.FileName);
                pbsize.Image = pb.Image;
            }
            pb.AutoSize = true;
        }

        private void Open_Click(object? sender, EventArgs e)
        {
            if (pb.Image != null)
            {
                var result = MessageBox.Show("Сохранить текущее изображение перед созданием нового рисунка?", "Предупреждение", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.No: break;
                    case DialogResult.Yes: Save_Click(sender, e); break;
                    case DialogResult.Cancel: return;
                }
            }
            History.Clear();
            historyCounter = 0;
            Bitmap pic = new Bitmap(775, 480);
            pb.Image = pic;
            History.Add(new Bitmap(pb.Image));
            OpenFileDialog OP = new OpenFileDialog();
            OP.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            OP.Title = "Open an image file";
            OP.FilterIndex = 1;
            if (OP.ShowDialog() != DialogResult.Cancel)
            {
                pb.Load(OP.FileName);
                pbsize.Image = pb.Image;
            }
            pb.AutoSize = true;
        }

        private void SaveTs_Click(object? sender, EventArgs e)
        {
            
            SaveFileDialog SaveDlg = new SaveFileDialog();
            SaveDlg.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            SaveDlg.Title = "Save an image file";
            SaveDlg.FilterIndex = 1;
            SaveDlg.ShowDialog();
            
            if (SaveDlg.FileName != "")
            {
                if (System.IO.File.Exists(SaveDlg.FileName))
                {
                        
                }
                else 
                {
                    System.IO.FileStream fs = (System.IO.FileStream)SaveDlg.OpenFile();

                    switch (SaveDlg.FilterIndex)
                    {
                        case 1:
                            this.pb.Image.Save(fs, ImageFormat.Png);
                            break;
                        case 2:
                            this.pb.Image.Save(fs, ImageFormat.Jpeg);
                            break;
                        case 3:
                            this.pb.Image.Save(fs, ImageFormat.Bmp);
                            break;
                        case 4:
                            this.pb.Image.Save(fs, ImageFormat.Gif);
                            break;
                    }
                    fs.Close();
                }
            }
        }

        private void Save_Click(object? sender, EventArgs e)
        {
            SaveFileDialog SaveDlg = new SaveFileDialog();
            SaveDlg.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            SaveDlg.Title = "Save an image file";
            SaveDlg.FilterIndex = 1;
            SaveDlg.ShowDialog();
            if (System.IO.File.Exists(SaveDlg.FileName))
            {

            }
            if (SaveDlg.FileName != "")
            {
                
                System.IO.FileStream fs = (System.IO.FileStream)SaveDlg.OpenFile();

                switch (SaveDlg.FilterIndex)
                {
                    case 1:
                        this.pb.Image.Save(fs, ImageFormat.Png);
                        break;
                    case 2:
                        this.pb.Image.Save(fs, ImageFormat.Jpeg);
                        break;
                    case 3:
                        this.pb.Image.Save(fs, ImageFormat.Bmp);
                        break;
                    case 4:
                        this.pb.Image.Save(fs, ImageFormat.Gif);
                        break;
                }
                fs.Close();
            }
        }

        private void About_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Avtor: Anton Buivol\nVersion 0.1\nMozhno Nazhimat na knopki", "About", MessageBoxButtons.OK);
        }

        private void Tb_Scroll(object? sender, EventArgs e)
        {
            lbl.Text = tb.Value.ToString();
            currentPen.Width = tb.Value;
        }
    }
}
