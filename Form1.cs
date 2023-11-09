using System.Drawing.Imaging;

namespace Laborotornaya_rabota
{
    public partial class Form1 : Form
    {
        MenuStrip ms;
        ToolStrip ts;
        Panel panel;
        ColorDialog cd;
        OpenFileDialog openFile;
        SaveFileDialog saveFile;
        PictureBox pb;
        ImageList imageList;
        TrackBar tb;
        ComboBox cb;
        Label lbl;
        
        public Form1()
        {
            this.Height= 600;
            this.Width= 850;
            this.Text = "Graf red";

            Image new_file = new Bitmap("../../../new.png");
            Image open = new Bitmap("../../../open.png");
            Image save = new Bitmap("../../../Save.png");
            Image exit = new Bitmap("../../../Exit.png");

            ms = new MenuStrip();
            ms.Dock = DockStyle.Top;
            ToolStripMenuItem File = new ToolStripMenuItem("File");
            ToolStripMenuItem New = new ToolStripMenuItem("New", new_file);
            ToolStripMenuItem Open = new ToolStripMenuItem("Open",open);
            ToolStripMenuItem Save = new ToolStripMenuItem("Save",save);
            ToolStripMenuItem Exit = new ToolStripMenuItem("Exit",exit);

            File.DropDownItems.Add(New);
            File.DropDownItems.Add(Open);
            File.DropDownItems.Add(Save);
            File.DropDownItems.Add(Exit);
            ms.Items.Add(File);

            New.ShortcutKeys = Keys.Control | Keys.N;
            Open.ShortcutKeys = Keys.F3;
            Save.ShortcutKeys = Keys.F2;
            Exit.ShortcutKeys = Keys.Alt | Keys.X;

            Save.Click += Save_Click;

            Image undo = new Bitmap("../../../Undo.png");
            Image redo = new Bitmap("../../../redo.png");
            Image pen = new Bitmap("../../../pen.png");
            Image about = new Bitmap("../../../About.png");

            ToolStripMenuItem Edit = new ToolStripMenuItem("Edit");
            ToolStripMenuItem Undo = new ToolStripMenuItem("Undo", undo);
            ToolStripMenuItem Redo = new ToolStripMenuItem("Redo", redo);
            Edit.DropDownItems.Add(Undo);
            Edit.DropDownItems.Add(Redo);
            Undo.ShortcutKeys = Keys.Control | Keys.Z;
            Undo.Click += Undo_Click;
            Redo.ShortcutKeys = Keys.Control | Keys.Shift | Keys.Z;
            ToolStripMenuItem Pen = new ToolStripMenuItem("Pen",pen);
            Edit.DropDownItems.Add(Pen);

            ToolStripMenuItem Color = new ToolStripMenuItem("Color");
            ToolStripMenuItem Style = new ToolStripMenuItem("Style");
            Pen.DropDownItems.Add(Color);
            Pen.DropDownItems.Add(Style);

            ToolStripMenuItem Solid = new ToolStripMenuItem("Solid");
            ToolStripMenuItem Dot = new ToolStripMenuItem("Dot");
            ToolStripMenuItem DashDotDot = new ToolStripMenuItem("DashDotDot");
            Style.DropDownItems.Add(Solid);
            Style.DropDownItems.Add(Dot);
            Style.DropDownItems.Add(DashDotDot);
            ms.Items.Add(Edit);

            ToolStripMenuItem Help = new ToolStripMenuItem("Help");
            ToolStripMenuItem About = new ToolStripMenuItem("About", about);
            Help.DropDownItems.Add(About);
            About.ShortcutKeys = Keys.F1;
            About.Click += About_Click;
            ms.Items.Add(Help);
            this.Controls.Add(ms);

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
            panel.Location = new Point(ts.Width, ts.Height-50);
            panel.Size = new Size(Width-75, 28);
            panel.BorderStyle= BorderStyle.Fixed3D;

            tb.Size = new Size(panel.Width / 3, panel.Height);
            //tb.Location = new Point(panel.Location.X+tb.Width*2, panel.Location.Y);

            //lbl.Location = new Point(ts.Width, ts.Height-50);
            lbl.Text = tb.Value.ToString();
            tb.Scroll += Tb_Scroll;
            panel.Controls.Add(lbl);
            panel.Controls.Add(tb);
            this.Controls.Add(panel);
            

            pb = new PictureBox();
            pb.BorderStyle = BorderStyle.Fixed3D;
            pb.Location = new Point(ts.Width, ms.Height);
            pb.Size = new Size(775,480);

            this.Controls.Add(pb);

            Image pic = new Bitmap(775, 480);
            pb.Image = pic;
            if (pb.Image == null)
            {
                MessageBox.Show("You need create a new file!");
                return;
            }
        }

        private void Save_Click(object? sender, EventArgs e)
        {
            SaveFileDialog SaveDlg = new SaveFileDialog();
            SaveDlg.Filter = "PNG Image|*.png |JPEG Image|*.jpg |Bitmap Image|*.bmp |Gif Image|*.gif";
            SaveDlg.Title = "Save an image file";
            SaveDlg.FilterIndex = 1;

            if (SaveDlg.FileName!="")
            {
                System.IO.FileStream fs = (System.IO.FileStream)SaveDlg.OpenFile();
                switch(SaveDlg.FilterIndex)
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

        private void Undo_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("rabotaet","work",MessageBoxButtons.OK);
        }

        private void Tb_Scroll(object? sender, EventArgs e)
        {
            lbl.Text = tb.Value.ToString();
        }
    }
}