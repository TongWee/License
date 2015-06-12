using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace License
{
    public partial class Form1 : Form
    {
        private string curFileName;
        private System.Drawing.Image curImage;
        private List<Point> corners;
        private bool ready = false;
        public Form1()
        {
             InitializeComponent();
            corners = new List<Point>();
        }

        private void 打开ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog opnDlg = new OpenFileDialog();
            opnDlg.Filter = "所有图像文件 | *.bmp; *.pcx; *.png; *.jpg; *.gif;" +
                "*.tif; *.ico; *.dxf; *.cgm; *.cdr; *.wmf; *.eps; *.emf|" +
                "位图( *.bmp; *.jpg; *.png;...) | *.bmp; *.pcx; *.png; *.jpg; *.gif; *.tif; *.ico|" +
                "矢量图( *.wmf; *.eps; *.emf;...) | *.dxf; *.cgm; *.cdr; *.wmf; *.eps; *.emf";
            opnDlg.Title = "打开图像文件";
            opnDlg.ShowHelp = true;
            if (opnDlg.ShowDialog() == DialogResult.OK)
            {
                curFileName = opnDlg.FileName;
                try
                {
                    curImage = Image.FromFile(curFileName);
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }
            }
            pictureBox1.Image = curImage;
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!ready)
            {
                if (curImage != null && corners.Count < 4)
                {
                    Point point = new Point(e.X, e.Y);
                    Point point_end = new Point(e.X + 2, e.Y + 2);
                    corners.Add(point);
                }
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (!ready&&corners.Count <= 4)
            {
                //Graphics g = pictureBox1.CreateGraphics();
                Pen pen = new Pen(Color.GreenYellow, 5);
                foreach (Point p in corners)
                {
                    e.Graphics.DrawRectangle(pen, p.X,p.Y,5,5);
                }
            }
        }

        private void 透视变换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PerspectiveTransform p_transform;
            
                
                
        }
    
    
    
    
    }

    
}
