using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Laborotornaya_rabota
{
    public partial class Form1 : Form
    {
        MenuStrip ms;
        ToolStrip ts;
        Panel panel;
        OpenFileDialog openFile;
        SaveFileDialog saveFile;
        PictureBox pb;
        ImageList imageList;
        TrackBar tb;
        ComboBox cb;
        Label lbl;

        bool drawing;
        int historyCounter;

        GraphicsPath currentPath;
        Point oldLocation;
        public Pen currentPen;
        Color historyColor;
        List<Image> History;

        ToolStripMenuItem Solid = new ToolStripMenuItem("Solid");
        ToolStripMenuItem Dot = new ToolStripMenuItem("Dot");
        ToolStripMenuItem DashDotDot = new ToolStripMenuItem("DashDotDot");

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

            ms = new MenuStrip();
            ms.Dock = DockStyle.Top;
            ToolStripMenuItem File = new ToolStripMenuItem("File");
            ToolStripMenuItem New = new ToolStripMenuItem("New", new_file);
            ToolStripMenuItem Open = new ToolStripMenuItem("Open", open);
            ToolStripMenuItem Save = new ToolStripMenuItem("Save", save);
            ToolStripMenuItem Exit = new ToolStripMenuItem("Exit", exit);

            Image undo = new Bitmap("../../../Undo.png");
            Image redo = new Bitmap("../../../redo.png");
            Image penpng = new Bitmap("../../../pen.png");
            Image about = new Bitmap("../../../About.png");

            ToolStripMenuItem Edit = new ToolStripMenuItem("Edit");
            ToolStripMenuItem Undo = new ToolStripMenuItem("Undo", undo);
            ToolStripMenuItem Redo = new ToolStripMenuItem("Redo", redo);
            ToolStripMenuItem PenMenu = new ToolStripMenuItem("Pen", penpng);
            ToolStripMenuItem Colors = new ToolStripMenuItem("Color");
            ToolStripMenuItem Style = new ToolStripMenuItem("Style");
            
            ToolStripMenuItem Help = new ToolStripMenuItem("Help");
            ToolStripMenuItem About = new ToolStripMenuItem("About", about);

            PenMenu.DropDownItems.Add(Colors);
            PenMenu.DropDownItems.Add(Style);
            Style.DropDownItems.Add(Solid);
            Style.DropDownItems.Add(Dot);
            Style.DropDownItems.Add(DashDotDot);
            Solid.Checked = true;

            About.ShortcutKeys = Keys.F1;
            New.ShortcutKeys = Keys.Control | Keys.N;
            Open.ShortcutKeys = Keys.F3;
            Save.ShortcutKeys = Keys.F2;
            Exit.ShortcutKeys = Keys.Alt | Keys.X;
            Undo.ShortcutKeys = Keys.Control | Keys.Z;
            Redo.ShortcutKeys = Keys.Control | Keys.Y;

            File.DropDownItems.Add(New);
            File.DropDownItems.Add(Open);
            File.DropDownItems.Add(Save);
            File.DropDownItems.Add(Exit);
            ms.Items.Add(File);

            Edit.DropDownItems.Add(Undo);
            Edit.DropDownItems.Add(Redo);
            Edit.DropDownItems.Add(PenMenu);
            ms.Items.Add(Edit);

            ms.Items.Add(Help);
            Help.DropDownItems.Add(About);

            this.Controls.Add(ms);
            
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

            ts = new ToolStrip();
            ts.Items.Add(new_file);
            ts.Items.Add(open);
            ts.Items.Add(save);
            ts.Items.Add("image");
            ts.Items.Add(exit);
            ts.Dock = DockStyle.Left;
            this.Controls.Add(ts);


            panel = new Panel();
            tb = new TrackBar();
            lbl = new Label();
            panel.Location = new Point(ts.Width, ts.Height - 50);
            panel.Size = new Size(Width - 75, 28);
            panel.BorderStyle = BorderStyle.Fixed3D;

            tb.Size = new Size(panel.Width / 3, panel.Height);
            tb.Location = new Point(514, 0);
            tb.SetRange(1,50);

            lbl.Text = tb.Value.ToString();
            tb.Scroll += Tb_Scroll;
            panel.Controls.Add(lbl);

            this.Controls.Add(panel);
            panel.Controls.Add(tb);

            pb = new PictureBox();
            pb.BorderStyle = BorderStyle.Fixed3D;
            pb.Location = new Point(ts.Width, ms.Height);
            pb.Size = new Size(775, 480);

            this.Controls.Add(pb);

            
            drawing = false;
            currentPen = new Pen(Color.Black);
            currentPen.Width = tb.Value;
            pb.Image = null;

            pb.MouseDown += Pb_MouseDown;
            pb.MouseUp += Pb_MouseUp;
            pb.MouseMove += Pb_MouseMove;

            tb.Scroll += Tb_Scroll1;

            History = new List<Image>();
            
        }

        private void Colors_Click(object? sender, EventArgs e)
        {
            ColorsForm colorsForm = new ColorsForm(currentPen.Color);
            colorsForm.Owner = this;
            colorsForm.ShowDialog();
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

        private void Tb_Scroll1(object? sender, EventArgs e)
        {
            currentPen.Width = tb.Value;
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
        }

        private void Pb_MouseUp(object? sender, MouseEventArgs e)
        {
            History.RemoveRange(historyCounter + 1, History.Count - historyCounter - 1);
            History.Add(new Bitmap(pb.Image));
            if (historyCounter + 1 < 10) historyCounter++;
            if (History.Count - 1 == 10) History.RemoveAt(0);
            drawing = false;
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

        private void New_Click(object? sender, EventArgs e)
        {
            History.Clear();
            historyCounter = 0;
            Bitmap pic = new Bitmap(775, 480);
            pb.Image = pic;
            History.Add(new Bitmap(pb.Image));
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
        }

        private void Open_Click(object? sender, EventArgs e)
        {
            OpenFileDialog OP = new OpenFileDialog();
            OP.Filter = "PNG Image|*.png |JPEG Image|*.jpg |Bitmap Image|*.bmp |Gif Image|*.gif";
            OP.Title = "Open an image file";
            OP.FilterIndex = 1;
            if (OP.ShowDialog() != DialogResult.Cancel)
            {
                pb.Load(OP.FileName);
            }
            pb.AutoSize = true;
        }

        private void Save_Click(object? sender, EventArgs e)
        {
            SaveFileDialog SaveDlg = new SaveFileDialog();
            SaveDlg.Filter = "PNG Image|*.png |JPEG Image|*.jpg |Bitmap Image|*.bmp |Gif Image|*.gif";
            SaveDlg.Title = "Save an image file";
            SaveDlg.FilterIndex = 1;
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
        }
    }
}
