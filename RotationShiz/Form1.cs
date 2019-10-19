using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

namespace RotationShiz
{
    public partial class Form1 : Form
    {
        Timer timer = new Timer();

        double rotx = 0;
        double roty = 0;
        double rotz = 0;

        double offx = 0;
        double offy = 0;
        double offz = 4;

        double fov = Math.PI / 8;
        public double[][] vertices;
        public int[][] tris;
        public Form1()
        {
            InitializeComponent();
            List<List<double>> impVer = new List<List<double>>();
            List<List<int>> impTris = new List<List<int>>();


        byte[] rawData = Properties.Resources.mesh;
            string strData = Encoding.UTF8.GetString(rawData, 0, rawData.Length);
            

            string[] words = strData.Split('\n');

            foreach (string lines in words)
            {
                List<double> tmpVer = new List<double>();
                List<int> tmpInd = new List<int>();

                string[] values = lines.Split(' ');
                
                switch (values[0])
                {
                    case "v":
                        double tmpDb1;
                        Double.TryParse(values[1], out tmpDb1);
                        tmpVer.Add(tmpDb1);
                        Double.TryParse(values[2], out tmpDb1);
                        tmpVer.Add(tmpDb1);
                        Double.TryParse(values[3], out tmpDb1);
                        tmpVer.Add(tmpDb1);

                        impVer.Add(tmpVer);

                        break;
                    case "f":
                        int tmpDb2;
                        Int32.TryParse(values[1], out tmpDb2);
                        tmpInd.Add(tmpDb2);
                        Int32.TryParse(values[2], out tmpDb2);
                        tmpInd.Add(tmpDb2);
                        Int32.TryParse(values[3], out tmpDb2);
                        tmpInd.Add(tmpDb2);

                        impTris.Add(tmpInd);
                        break;
                }
            }
            vertices = impVer.Select(list => list.ToArray()).ToArray();
            tris = impTris.Select(list => list.ToArray()).ToArray();

            timer.Enabled = true;
            timer.Interval = 20;  /* 100 millisec */
            timer.Tick += new EventHandler(TimerCallback);
        }
        private void TimerCallback(object sender, EventArgs e)
        {
            if (Form.ActiveForm == this)
            {
                if (Keyboard.IsKeyDown(Key.E))
                {
                    rotz += 0.08;
                }
                else if (Keyboard.IsKeyDown(Key.Q))
                {
                    rotz -= 0.08;
                }

                if (Keyboard.IsKeyDown(Key.S))
                {
                    rotx += 0.08;
                }
                else if (Keyboard.IsKeyDown(Key.W))
                {
                    rotx -= 0.08;
                }

                if (Keyboard.IsKeyDown(Key.D))
                {
                    roty += 0.08;
                }
                else if (Keyboard.IsKeyDown(Key.A))
                {
                    roty -= 0.08;
                }

                if (Keyboard.IsKeyDown(Key.R))
                {
                    offz += 0.12;
                }
                else if (Keyboard.IsKeyDown(Key.F)&&offz>1.5)
                {
                    offz -= 0.12;
                }

                if (Keyboard.IsKeyDown(Key.Right))
                {
                    offx += 0.12;
                }
                else if (Keyboard.IsKeyDown(Key.Left))
                {
                    offx -= 0.12;
                }

                if (Keyboard.IsKeyDown(Key.Up))
                {
                    offy += 0.12;
                }
                else if (Keyboard.IsKeyDown(Key.Down))
                {
                    offy -= 0.12;
                }

                if (Keyboard.IsKeyDown(Key.OemPlus) && fov < Math.PI/3)
                {
                    fov += 0.01;
                }
                else if (Keyboard.IsKeyDown(Key.OemMinus) && fov > Math.PI/16)
                {
                    fov -= 0.01;
                }

                if (Keyboard.IsKeyDown(Key.P))
                {
                    rotx = 0;
                    roty = 0;
                    rotz = 0;

                    offx = 0;
                    offy = 0;
                    offz = 4;

                    fov = Math.PI / 8;
                }

            }
            this.Invalidate();
            return;
        }

        //static double[,] points = new double[,]
        //{
        //    {   -1,     1,      1       },
        //    {   1,      1,      1       },
        //    {   1,      -1,     1       },
        //    {   -1,     -1,     1       },

        //    {   -1,     1,      -1      },
        //    {    1,     1,      -1      },
        //    {    1,     -1,     -1      },
        //    {   -1,     -1,     -1      }
        //};

        //static int[,] vertInd = new int[,]
        //{
        //    {0,1},
        //    {1,2},
        //    {2,3},
        //    {3,0},

        //    {4,0},
        //    {3,7},

        //    {4,5},
        //    {5,6},
        //    {6,7},
        //    {7,4},

        //    {1,5},
        //    {2,6}



        //};


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            double[][] vertsP = new double[vertices.GetLength(0)][];
            for(int i=0;i<vertsP.Length;++i)
            {
                vertsP[i] = new double[3];
            }
            for (int i = 0; i < vertices.GetLength(0); i++)
            {
                vertsP[i][0] = vertices[i][0];
                vertsP[i][1] = vertices[i][1];
                vertsP[i][2] = vertices[i][2];
            }
            for (int i = 0; i < vertices.GetLength(0); i++)
            {
                double dy = vertsP[i][1];
                double dz = vertsP[i][2];
                vertsP[i][1] = (dy * Math.Cos(rotx)) - (dz * Math.Sin(rotx));
                vertsP[i][2] = (dy * Math.Sin(rotx)) + (dz * Math.Cos(rotx));
            }
            for (int i = 0; i < vertices.GetLength(0); i++)
            {
                double dx = vertsP[i][0];
                double dz = vertsP[i][2];
                vertsP[i][2] = (dz * Math.Cos(roty)) - (dx * Math.Sin(roty));
                vertsP[i][0] = (dz * Math.Sin(roty)) + (dx * Math.Cos(roty));
            }
            for (int i = 0; i < vertices.GetLength(0); i++)
            {
                double dx = vertsP[i][0];
                double dy = vertsP[i][1];
                vertsP[i][0] = (dx * Math.Cos(rotz)) - (dy * Math.Sin(rotz));
                vertsP[i][1] = (dx * Math.Sin(rotz)) + (dy * Math.Cos(rotz));
            }

            Brush bBrush = (Brush)Brushes.Black;
            Brush wBrush = (Brush)Brushes.White;
            Pen wPen = (Pen)Pens.White;
            e.Graphics.FillRectangle(bBrush, 0, 0, this.Width, this.Height);
            int size = Math.Min(this.Width, this.Height);
            Font font = new Font(FontFamily.GenericMonospace, size / 40);
            e.Graphics.DrawString(  "FOV: " + Convert.ToInt32(fov * 180) +
                                    "\nRotX: " + Mod(Convert.ToInt32(rotx * 180), 360) +
                                    "\nRotY: " + Mod(Convert.ToInt32(roty * 180), 360) +
                                    "\nRotZ: " + Mod(Convert.ToInt32(rotz * 180), 360) +
                                    "\nX: " + offx.ToString("f2") +
                                    "\nY: " + offy.ToString("f2") +
                                    "\nZ: " + offz.ToString("f2"), font, wBrush, 0, 0);
            for (int i = 0; i < tris.GetLength(0); i++)
            {
                double tempfov = 1 / Math.Tan(fov);
                int x1 = Convert.ToInt32(((vertsP[tris[i][0] - 1][0] + offx) * (tempfov / (vertsP[tris[i][0] - 1][2] + offz))) * size / 4) + this.Width / 2;
                int y1 = -Convert.ToInt32(((vertsP[tris[i][0] - 1][1] + offy) * (tempfov / (vertsP[tris[i][0] - 1][2] + offz))) * size / 4) + this.Height / 2;
                int x2 = Convert.ToInt32(((vertsP[tris[i][1] - 1][0] + offx) * (tempfov / (vertsP[tris[i][1] - 1][2] + offz))) * size / 4) + this.Width / 2;
                int y2 = -Convert.ToInt32(((vertsP[tris[i][1] - 1][1] + offy) * (tempfov / (vertsP[tris[i][1] - 1][2] + offz))) * size / 4) + this.Height / 2;
                int x3 = Convert.ToInt32(((vertsP[tris[i][2] - 1][0] + offx) * (tempfov / (vertsP[tris[i][2] - 1][2] + offz))) * size / 4) + this.Width / 2;
                int y3 = -Convert.ToInt32(((vertsP[tris[i][2] - 1][1] + offy) * (tempfov / (vertsP[tris[i][2] - 1][2] + offz))) * size / 4) + this.Height / 2;
                DrawTriangle(wPen, x1, y1, x2, y2, x3, y3, e);
            }

            int Mod(int a, int n)
            {
                int result = a % n;
                if ((result < 0 && n > 0) || (result > 0 && n < 0))
                {
                    result += n;
                }
                return result;
            }
        }

        private void DrawTriangle(Pen pen,float x1,float y1,float x2,float y2,float x3,float y3,PaintEventArgs e)
        {
            e.Graphics.DrawLine(pen, x1, y1, x2, y2);
            e.Graphics.DrawLine(pen, x2, y2, x3, y3);
            e.Graphics.DrawLine(pen, x3, y3, x1, y1);
        }
    }
}
