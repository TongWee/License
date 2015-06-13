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
        private System.Drawing.Bitmap curImage;
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
                    curImage = (Bitmap)Image.FromFile(curFileName);
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }
            }
            pictureBox1.Image = curImage;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!ready)
            {
                if (curImage != null && corners.Count < 4)
                {
                    Point point = new Point(e.X, e.Y);                    
                    corners.Add(point);
                }
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (!ready&&corners.Count <= 4)
            {
                Pen pen = new Pen(Color.GreenYellow, 5);
                foreach (Point p in corners)
                {
                    e.Graphics.DrawRectangle(pen, p.X,p.Y,5,5);
                }
            }
        }

        private void 透视变换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (corners.Count == 4)
            {
                Point center = new Point(0,0);
                Graphics g = pictureBox1.CreateGraphics();
                Pen pen = new Pen(Color.OrangeRed, 3);                
                for (int i = 0; i < (int)corners.Count; i++)
                {
                    center.X += corners[i].X;
                    center.Y += corners[i].Y;
                }
                center.X = center.X / 4; 
                center.Y = center.Y / 4;
                sortCorners(corners, center);               
                ready = true;
                PerspectiveTransform p = PerspectiveTransform.quadrilateralToQuadrilateral(
                                                                                                   0, 0,
                                                                                                 440, 0,
                                                                                                 440, 140,
                                                                                                 0, 140,
                                                                                                 corners[0].X, corners[0].Y,
                                                                                                 corners[1].X, corners[1].Y,
                                                                                                 corners[2].X, corners[2].Y,
                                                                                                 corners[3].X, corners[3].Y                                                                                               
                                                                                                 );
                float[] _corners_2 = { 0, 0, 440, 0, 440, 140, 0, 140 };
                Bitmap trans_img = Resource.license;
                pictureBox2.Image = trans_img;
                Point _q;                
                for (int i = 0; i < trans_img.Width; i++)
                {
                    for (int j = 0; j < trans_img.Height; j++)
                    {
                        _q = p.transformPoints(new Point(i, j));
                        if (_q.X < curImage.Width && _q.Y < curImage.Height) 
                        { 
                            trans_img.SetPixel(i, j, Color.FromArgb(curImage.GetPixel(_q.X, _q.Y).R, curImage.GetPixel(_q.X, _q.Y).G, curImage.GetPixel(_q.X, _q.Y).B));
                        }
                        else
                            trans_img.SetPixel(i, j,Color.White);
                    }
                }
                p.transformPoints(_corners_2);
                g.DrawRectangle(pen, _corners_2[0], _corners_2[1], 3, 3);
                g.DrawRectangle(pen, _corners_2[2], _corners_2[3], 3, 3);
                g.DrawRectangle(pen, _corners_2[4], _corners_2[5], 3, 3);
                g.DrawRectangle(pen, _corners_2[6], _corners_2[7], 3, 3);
              }
            else
                MessageBox.Show("请选择四个点");
        }

        private void 重置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            corners.Clear();
            ready = false;
            pictureBox1.Image = curImage;
            Bitmap trans_img = Resource.license; 
            pictureBox2.Image = trans_img;
        }    
        private void sortCorners(List<Point> corners, Point center)
        {
            List<Point> top = new List<Point>();
            List<Point> bot = new List<Point>();
	        for (int i = 0; i < (int)corners.Count; i++)
	        {
		        if (corners[i].Y < center.Y)
			        top.Add(corners[i]);
		        else
			        bot.Add(corners[i]);
	        }
	        Point tl = top[0].X > top[1].X ? top[1] : top[0];
	        Point tr = top[0].X > top[1].X ? top[0] : top[1];
	        Point bl = bot[0].X > bot[1].X ? bot[1] : bot[0];
	        Point br = bot[0].X > bot[1].X ? bot[0] : bot[1];
	        corners.Clear();
	        corners.Add(tl);
	        corners.Add(tr);
            corners.Add(br);
            corners.Add(bl);
        }
    }  
}
