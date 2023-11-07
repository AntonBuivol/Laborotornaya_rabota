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

            Image New = new Bitmap("../../../new.png");
            Image open = new Bitmap("../../../open.png");
            Image save = new Bitmap("../../../Save.png");
            Image exit = new Bitmap("../../../Exit.png");

            ms = new MenuStrip();
            ms.Dock = DockStyle.Top;
            ToolStripMenuItem File = new ToolStripMenuItem("File");
            File.DropDownItems.Add("New", New);
            File.DropDownItems.Add("Open", open);
            File.DropDownItems.Add("Save", save);
            File.DropDownItems.Add("Exit", exit);
            ms.Items.Add(File);

            Image undo = new Bitmap("../../../Undo.png");
            Image redo = new Bitmap("../../../redo.png");
            Image pen = new Bitmap("../../../pen.png");
            Image about = new Bitmap("../../../About.png");

            ToolStripMenuItem Edit = new ToolStripMenuItem("Edit");
            Edit.DropDownItems.Add("Undo", undo);
            Edit.DropDownItems.Add("Redo", redo);
            ToolStripMenuItem Pen = new ToolStripMenuItem("Pen");
            Edit.DropDownItems.Add(Pen);
            Pen.DropDownItems.Add("Color");
            ToolStripMenuItem Style = new ToolStripMenuItem("Style");
            Pen.DropDownItems.Add(Style);
            Style.DropDownItems.Add("Solid");
            Style.DropDownItems.Add("Dot");
            Style.DropDownItems.Add("DashDotDot");
            ms.Items.Add(Edit);

            ToolStripMenuItem Help = new ToolStripMenuItem("Help");
            Help.DropDownItems.Add("About", about);
            ms.Items.Add(Help);
            this.Controls.Add(ms);

            ts = new ToolStrip();
            ts.Items.Add("image");
            ts.Items.Add(save);
            ts.Items.Add(open);
            ts.Items.Add("image");
            ts.Items.Add(exit);
            ts.Dock = DockStyle.Left;
            this.Controls.Add(ts);

            panel = new Panel();
            tb = new TrackBar();
            panel.Location = new Point(ts.Width, ts.Height-50);
            panel.Size = new Size(Width-75, 28);
            panel.BorderStyle= BorderStyle.Fixed3D;

            tb.Size = new Size(panel.Width / 3, panel.Height);
            //tb.Location = new Point(panel.Location.X+tb.Width*2, panel.Location.Y);

            this.Controls.Add(panel);
            panel.Controls.Add(tb);

            pb = new PictureBox();
            pb.BorderStyle = BorderStyle.Fixed3D;
            pb.Location = new Point(ts.Width, ms.Height);
            pb.Size = new Size(Width - 75,Height-120);

            this.Controls.Add(pb);
        }
    }
}