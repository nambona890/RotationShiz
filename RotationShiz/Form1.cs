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
        public int[][] uvtris;
        public double[][] uv;
        public bool textured = false;
        public Bitmap tex;

        bool tabPress = false;

        int meshIndex = 0;
        public Form1()
        {
            InitializeComponent();

            ImportObj(Properties.Resources.muncher);

            timer.Enabled = true;
            timer.Interval = 20;  /* 100 millisec */
            timer.Tick += new EventHandler(TimerCallback);
        }

        private void ImportObj(byte[] r)
        {
            textured = false;
            
            List<List<double>> impVer = new List<List<double>>();
            List<List<int>> impTris = new List<List<int>>();


            byte[] rawData = r;
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
        }

        private void ImportTexObj(byte[] r,Bitmap tx)
        {
            textured = true;
            tex = tx;

            List<List<double>> impVer = new List<List<double>>();
            List<List<int>> impTris = new List<List<int>>();

            List<List<double>> impUV = new List<List<double>>();
            List<List<int>> impUVTris = new List<List<int>>();


            byte[] rawData = r;
            string strData = Encoding.UTF8.GetString(rawData, 0, rawData.Length);


            string[] words = strData.Split('\n');

            foreach (string lines in words)
            {
                List<double> tmpVer = new List<double>();
                List<int> tmpInd = new List<int>();

                List<double> tmpUV = new List<double>();
                List<int> tmpUVInd = new List<int>();

                string[] values = lines.Split(new Char[] { ' ', '/' });

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
                    case "vt":
                        double tmpDb2;
                        Double.TryParse(values[1], out tmpDb2);
                        tmpUV.Add(tmpDb2);
                        Double.TryParse(values[2], out tmpDb2);
                        tmpUV.Add(tmpDb2);

                        impUV.Add(tmpUV);

                        break;
                    case "f":
                        int tmpDb3,tmpDb4;
                        Int32.TryParse(values[1], out tmpDb3);
                        tmpInd.Add(tmpDb3);
                        Int32.TryParse(values[3], out tmpDb3);
                        tmpInd.Add(tmpDb3);
                        Int32.TryParse(values[5], out tmpDb3);
                        tmpInd.Add(tmpDb3);

                        impTris.Add(tmpInd);

                        Int32.TryParse(values[2], out tmpDb4);
                        tmpUVInd.Add(tmpDb4);
                        Int32.TryParse(values[4], out tmpDb4);
                        tmpUVInd.Add(tmpDb4);
                        Int32.TryParse(values[6], out tmpDb4);
                        tmpUVInd.Add(tmpDb4);

                        impUVTris.Add(tmpUVInd);
                        break;
                }
            }
            vertices = impVer.Select(list => list.ToArray()).ToArray();
            tris = impTris.Select(list => list.ToArray()).ToArray();

            uv = impUV.Select(list => list.ToArray()).ToArray();
            uvtris = impUVTris.Select(list => list.ToArray()).ToArray();
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

                if (Keyboard.IsKeyDown(Key.Left))
                {
                    offx += 0.12;
                }
                else if (Keyboard.IsKeyDown(Key.Right))
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

                if(Keyboard.IsKeyDown(Key.Tab)&&!tabPress)
                {
                    rotx = 0;
                    roty = 0;
                    rotz = 0;

                    offx = 0;
                    offy = 0;
                    offz = 4;

                    fov = Math.PI / 8;
                    meshIndex = Mod(meshIndex + 1, 5);
                    switch(meshIndex)
                    {
                        case 0:
                            ImportObj(Properties.Resources.muncher);
                            break;
                        case 1:
                            ImportObj(Properties.Resources.isabelle);
                            break;
                        case 2:
                            ImportObj(Properties.Resources.hagrid);
                            break;
                        case 3:
                            ImportObj(Properties.Resources.carl);
                            break;
                        case 4:
                            ImportTexObj(Properties.Resources.prism,Properties.Resources.obama);
                            break;
                    }
                    tabPress = true;
                }
                else if(!Keyboard.IsKeyDown(Key.Tab)&&tabPress)
                {
                    tabPress = false;
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
        public List<List<Double>> triDraw;
        public List<List<Double>> uvDraw;


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            double[][] vertsP = new double[vertices.GetLength(0)][];
            for (int i = 0; i < vertsP.Length; ++i)
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
            triDraw = new List<List<Double>>();
            uvDraw = new List<List<Double>>();
            for (int i = 0; i < tris.GetLength(0); i++)
            {
                double ux = (vertsP[tris[i][1] - 1][0] + offx) - (vertsP[tris[i][0] - 1][0] + offx);
                double uy = (vertsP[tris[i][1] - 1][1] + offy) - (vertsP[tris[i][0] - 1][1] + offy);
                double uz = (vertsP[tris[i][1] - 1][2] + offz) - (vertsP[tris[i][0] - 1][2] + offz);
                double vx = (vertsP[tris[i][2] - 1][0] + offx) - (vertsP[tris[i][0] - 1][0] + offx);
                double vy = (vertsP[tris[i][2] - 1][1] + offy) - (vertsP[tris[i][0] - 1][1] + offy);
                double vz = (vertsP[tris[i][2] - 1][2] + offz) - (vertsP[tris[i][0] - 1][2] + offz);
                double nx = (uy * vz) - (uz * vy);
                double ny = (uz * vx) - (ux * vz);
                double nz = (ux * vy) - (uy * vx);

                double nl = Math.Sqrt((nx * nx) + (ny * ny) + (nz * nz));
                nx /= nl;
                ny /= nl;
                nz /= nl;

                if (nx * (vertsP[tris[i][0] - 1][0] + offx) +
                    ny * (vertsP[tris[i][0] - 1][1] + offy) +
                    nz * (vertsP[tris[i][0] - 1][2] + offz)
                    < 0)
                {


                    double lightx = 0;
                    double lighty = 0;
                    double lightz = -1;
                    double lightl = Math.Sqrt((lightx * lightx) + (lighty * lighty) + (lightz * lightz));
                    lightx /= lightl;
                    lighty /= lightl;
                    lightz /= lightl;
                    double dp = (nx * lightx) + (ny * lighty) + (nz * lightz);

                    List<double> tTri = new List<double>();
                    List<double> tUV = new List<double>();
                    double tempfov = 1 / Math.Tan(fov);
                    tTri.Add(-(((vertsP[tris[i][0] - 1][0] + offx) * (tempfov / (vertsP[tris[i][0] - 1][2] + offz))) * size / 4) + this.Width / 2);
                    tTri.Add(-(((vertsP[tris[i][0] - 1][1] + offy) * (tempfov / (vertsP[tris[i][0] - 1][2] + offz))) * size / 4) + this.Width / 2);
                    tTri.Add(-(((vertsP[tris[i][1] - 1][0] + offx) * (tempfov / (vertsP[tris[i][1] - 1][2] + offz))) * size / 4) + this.Width / 2);
                    tTri.Add(-(((vertsP[tris[i][1] - 1][1] + offy) * (tempfov / (vertsP[tris[i][1] - 1][2] + offz))) * size / 4) + this.Width / 2);
                    tTri.Add(-(((vertsP[tris[i][2] - 1][0] + offx) * (tempfov / (vertsP[tris[i][2] - 1][2] + offz))) * size / 4) + this.Width / 2);
                    tTri.Add(-(((vertsP[tris[i][2] - 1][1] + offy) * (tempfov / (vertsP[tris[i][2] - 1][2] + offz))) * size / 4) + this.Width / 2);
                    tTri.Add((vertsP[tris[i][0] - 1][2] + vertsP[tris[i][1] - 1][2] + vertsP[tris[i][2] - 1][2])/3);
                    tTri.Add(dp);

                    triDraw.Add(tTri);
                    if (textured)
                    {
                        tUV.Add(uv[uvtris[i][0] - 1][0]);
                        tUV.Add(uv[uvtris[i][0] - 1][1]);
                        tUV.Add(uv[uvtris[i][1] - 1][0]);
                        tUV.Add(uv[uvtris[i][1] - 1][1]);
                        tUV.Add(uv[uvtris[i][2] - 1][0]);
                        tUV.Add(uv[uvtris[i][2] - 1][1]);
                    }

                    uvDraw.Add(tUV);


                }

            }
            double[,] triDrawAr = new double[triDraw.Count,14];
            for (int i = 0; i < triDrawAr.GetLength(0); i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    triDrawAr[i, j] = triDraw[i].ToArray()[j];
                }
                if (textured)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        triDrawAr[i, j + 8] = uvDraw[i].ToArray()[j];
                    }
                }
            }
            triDrawAr = triDrawAr.OrderByDescending(x => x[6]);
            for (int i = 0; i < triDrawAr.GetLength(0); i++)
            {
                double tempfov = 1 / Math.Tan(fov);
                int x1 = Convert.ToInt32(triDrawAr[i, 0]);
                int y1 = Convert.ToInt32(triDrawAr[i, 1]);
                int x2 = Convert.ToInt32(triDrawAr[i, 2]);
                int y2 = Convert.ToInt32(triDrawAr[i, 3]);
                int x3 = Convert.ToInt32(triDrawAr[i, 4]);
                int y3 = Convert.ToInt32(triDrawAr[i, 5]);
                float u1 = 0;
                float v1 = 0;
                float u2 = 0;
                float v2 = 0;
                float u3 = 0;
                float v3 = 0;
                if (textured)
                {
                    u1 = Convert.ToSingle(triDrawAr[i, 8]);
                    v1 = Convert.ToSingle(triDrawAr[i, 9]);
                    u2 = Convert.ToSingle(triDrawAr[i, 10]);
                    v2 = Convert.ToSingle(triDrawAr[i, 11]);
                    u3 = Convert.ToSingle(triDrawAr[i, 12]);
                    v3 = Convert.ToSingle(triDrawAr[i, 13]);
                }

                //DrawTriangle(wPen, x1, y1, x2, y2, x3, y3, e);
                if (textured)
                {
                    int color = ExtMath.Clamp(Convert.ToInt32(triDrawAr[i, 7] * 256), 10, 255);
                    Pen cPen = new Pen(Color.FromArgb(color, 0, color));
                    FillTriangle(cPen, x1, y1, x2, y2, x3, y3, e);
                    FillTexTriangle(cPen, x1, y1, x2, y2, x3, y3, u1, v1, u2, v2, u3, v3, e);
                }
                else
                {
                    int color = ExtMath.Clamp(Convert.ToInt32(triDrawAr[i, 7] * 256), 10, 255);
                    Pen cPen = new Pen(Color.FromArgb(color, color, color));
                    FillTriangle(cPen, x1, y1, x2, y2, x3, y3, e);
                }

            }
            Font font = new Font(FontFamily.GenericMonospace, size / 50);
            e.Graphics.DrawString("FOV: " + Convert.ToInt32(fov * 180) +
                                    "\nRotX: " + Mod(Convert.ToInt32(rotx * (180 / Math.PI)), 360) +
                                    "\nRotY: " + Mod(Convert.ToInt32(roty * (180 / Math.PI)), 360) +
                                    "\nRotZ: " + Mod(Convert.ToInt32(rotz * (180 / Math.PI)), 360) +
                                    "\nX: " + offx.ToString("f2") +
                                    "\nY: " + offy.ToString("f2") +
                                    "\nZ: " + offz.ToString("f2") +
                                    "\nTris: " + tris.GetLength(0) +
                                    "\nRendered tris: " + triDraw.Count, font, wBrush, 0, 0);
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

        private void DrawTriangle(Pen pen,float x1,float y1,float x2,float y2,float x3,float y3,PaintEventArgs e)
        {
            e.Graphics.DrawLine(pen, x1, y1, x2, y2);
            e.Graphics.DrawLine(pen, x2, y2, x3, y3);
            e.Graphics.DrawLine(pen, x3, y3, x1, y1);
        }

        private void FillTriangle(Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, PaintEventArgs e)
        {
            float[,] tri = new float[,]
            {
                { x1,y1 },
                { x2,y2 },
                { x3,y3 }
            };

            tri = tri.OrderBy(x => x[1]);

            if (tri[1, 1] == tri[2, 1])
            {
                FillBottomTriangle(pen, tri[0, 0], tri[0, 1], tri[1, 0], tri[1, 1], tri[2, 0], tri[2, 1], e);
            }
            else if (tri[0, 1] == tri[1, 1])
            {
                FillTopTriangle(pen, tri[0, 0], tri[0, 1], tri[1, 0], tri[1, 1], tri[2, 0], tri[2, 1], e);
            }
            else
            {
                float nx = (tri[0, 0] + ((tri[1, 1] - tri[0, 1]) / (tri[2, 1] - tri[0, 1])) * (tri[2, 0] - tri[0, 0]));
                float ny = tri[1, 1];
                FillBottomTriangle(pen, tri[0, 0], tri[0, 1], tri[1, 0], tri[1, 1], nx, ny, e);
                FillTopTriangle(pen, tri[1, 0], tri[1, 1], nx, ny, tri[2, 0], tri[2, 1], e);
            }
        }

        private void FillBottomTriangle(Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, PaintEventArgs e)
        {
            float slope1 = (x2 - x1) / (y2 - y1);
            float slope2 = (x3 - x1) / (y3 - y1);

            float curx1 = x1;
            float curx2 = x1;
            for (int scanline = Convert.ToInt32(y1); scanline <= y2; scanline++)
            {
                e.Graphics.DrawLine(pen, curx1, scanline, curx2, scanline);
                curx1 += slope1;
                curx2 += slope2;
            }
        }

        private void FillTopTriangle(Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, PaintEventArgs e)
        {
            float slope1 = (x3 - x1) / (y3 - y1);
            float slope2 = (x3 - x2) / (y3 - y2);

            float curx1 = x3;
            float curx2 = x3;
            for (int scanline = Convert.ToInt32(y3); scanline > y1; scanline--)
            {
                e.Graphics.DrawLine(pen, curx1, scanline, curx2, scanline);
                curx1 -= slope1;
                curx2 -= slope2;
            }
        }

        private void FillTexTriangle(Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, float u1, float v1, float u2, float v2, float u3, float v3, PaintEventArgs e)
        {
            float[,] tri = new float[,]
            {
                { x1,y1,u1,v1 },
                { x2,y2,u2,v2 },
                { x3,y3,u3,v3 }
            };

            tri = tri.OrderBy(x => x[1]);

            if (tri[1, 1] == tri[2, 1])
            {
                FillBottomTexTriangle(pen, tri[0, 0], tri[0, 1], tri[1, 0], tri[1, 1], tri[2, 0], tri[2, 1], tri[0, 2], tri[0, 3], tri[1, 2], tri[1, 3], tri[2, 2], tri[2, 3], e);
            }
            else if (tri[0, 1] == tri[1, 1])
            {
                FillTopTexTriangle(pen, tri[0, 0], tri[0, 1], tri[1, 0], tri[1, 1], tri[2, 0], tri[2, 1], tri[0, 2], tri[0, 3], tri[1, 2], tri[1, 3], tri[2, 2], tri[2, 3], e);
            }
            else
            {
                float midperc = (tri[1, 1] - tri[0, 1]) / (tri[2, 1] - tri[0, 1]);
                float nx = (tri[0, 0] + midperc * (tri[2, 0] - tri[0, 0]));
                float ny = tri[1, 1];
                float nu = (tri[0, 2] + midperc * (tri[2, 2] - tri[0, 2]));
                float nv = tri[1, 3];
                FillBottomTexTriangle(pen, tri[0, 0], tri[0, 1], tri[1, 0], tri[1, 1], nx, ny, tri[0, 2], tri[0, 3], tri[1, 2], tri[1, 3], nu, nv, e);
                FillTopTexTriangle(pen, tri[1, 0], tri[1, 1], nx, ny, tri[2, 0], tri[2, 1], tri[0, 2], tri[0, 3], nu, nv, tri[2, 2], tri[2, 3], e);
            }
        }

        private void FillBottomTexTriangle(Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, float u1, float v1, float u2, float v2, float u3, float v3, PaintEventArgs e)
        {
            float slope1 = (x2 - x1) / (y2 - y1);
            float slope2 = (x3 - x1) / (y3 - y1);
            //float uslope1 = (u2 - u1) / (v2 - v1);
            //float uslope2 = (u3 - u1) / (v3 - v1);
            float width = tex.Width;
            float height = tex.Height;
            float curx1 = x1;
            float curx2 = x1;
            float curu1 = u1;
            float curu2 = u1;
            float curv1 = v1;
            float curv2 = v1;
            float uslope1 = (u2 - u1) / (y2 - y1);
            float uslope2 = (u3 - u1) / (y3 - y1);
            float vslope1 = (v2 - v1) / (y2 - y1);
            float vslope2 = (v3 - v1) / (y3 - y1);


            for (int scanline = Convert.ToInt32(y1); scanline <= y2; scanline++)
            {
                float curu3 = curu1;
                float curv3 = curv1;
                float uslope3 = (curu2 - curu1) / (curx2 - curx1);
                float vslope3 = (curv2 - curv1) / (curx2 - curx1);
                for (int x4 = Convert.ToInt32(curx1); x4 < curx2; x4++)
                {

                    int px = Convert.ToInt32(ExtMath.Clamp(curu3 * width, 0, width - 1));
                    int py = Convert.ToInt32(ExtMath.Clamp(curv3 * height, 0, height - 1));
                    Brush brush = new SolidBrush(tex.GetPixel(px, py));
                    e.Graphics.FillRectangle(brush, x4, scanline, 1, 1);
                    curu3 += uslope3;
                    curv3 += vslope3;
                }
                curu1 += uslope1;
                curu2 += uslope2;
                curv1 += vslope1;
                curv2 += vslope2;
                curx1 += slope1;
                curx2 += slope2;
            }
        }

        private void FillTopTexTriangle(Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, float u1, float v1, float u2, float v2, float u3, float v3, PaintEventArgs e)
        {
            float slope1 = (x3 - x1) / (y3 - y1);
            float slope2 = (x3 - x2) / (y3 - y2);

            float width = tex.Width;
            float height = tex.Height;
            float curx1 = x1;
            float curx2 = x2;
            float curu1 = u1;
            float curu2 = u2;
            float curv1 = v1;
            float curv2 = v2;
            float uslope1 = (u3 - u1) / (y3 - y1);
            float uslope2 = (u3 - u2) / (y3 - y2);
            float vslope1 = (v3 - v1) / (y3 - y1);
            float vslope2 = (v3 - v2) / (y3 - y2);



            for (int scanline = Convert.ToInt32(y1); scanline <= y2; scanline++)
            {
                float curu3 = curu1;
                float curv3 = curv1;
                float uslope3 = (curu2 - curu1) / (curx2 - curx1);
                float vslope3 = (curv2 - curv1) / (curx2 - curx1);
                for (int x4 = Convert.ToInt32(curx1); x4 < curx2; x4++)
                {
                    int px = Convert.ToInt32(ExtMath.Clamp(curu3 * width, 0, width - 1));
                    int py = Convert.ToInt32(ExtMath.Clamp(curv3 * height, 0, height - 1));
                    Brush brush = new SolidBrush(tex.GetPixel(px, py));
                    e.Graphics.FillRectangle(brush, x4, scanline, 1, 1);
                    curu3 += uslope3;
                    curv3 += vslope3;
                }
                curu1 += uslope1;
                curu2 += uslope2;
                curv1 += vslope1;
                curv2 += vslope2;
                curx1 += slope1;
                curx2 += slope2;
            }
        }

    }
    public static class ExtMath
    {
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
    }

    public static class MultiDimensionalArrayExtensions
    {
        /// <summary>
        ///   Orders the two dimensional array by the provided key in the key selector.
        /// </summary>
        /// <typeparam name="T">The type of the source two-dimensional array.</typeparam>
        /// <param name="source">The source two-dimensional array.</param>
        /// <param name="keySelector">The selector to retrieve the column to sort on.</param>
        /// <returns>A new two dimensional array sorted on the key.</returns>
        public static T[,] OrderBy<T>(this T[,] source, Func<T[], T> keySelector)
        {
            return source.ConvertToSingleDimension().OrderBy(keySelector).ConvertToMultiDimensional();
        }
        /// <summary>
        ///   Orders the two dimensional array by the provided key in the key selector in descending order.
        /// </summary>
        /// <typeparam name="T">The type of the source two-dimensional array.</typeparam>
        /// <param name="source">The source two-dimensional array.</param>
        /// <param name="keySelector">The selector to retrieve the column to sort on.</param>
        /// <returns>A new two dimensional array sorted on the key.</returns>
        public static T[,] OrderByDescending<T>(this T[,] source, Func<T[], T> keySelector)
        {
            return source.ConvertToSingleDimension().
                OrderByDescending(keySelector).ConvertToMultiDimensional();
        }
        /// <summary>
        ///   Converts a two dimensional array to single dimensional array.
        /// </summary>
        /// <typeparam name="T">The type of the two dimensional array.</typeparam>
        /// <param name="source">The source two dimensional array.</param>
        /// <returns>The repackaged two dimensional array as a single dimension based on rows.</returns>
        private static IEnumerable<T[]> ConvertToSingleDimension<T>(this T[,] source)
        {
            T[] arRow;

            for (int row = 0; row < source.GetLength(0); ++row)
            {
                arRow = new T[source.GetLength(1)];

                for (int col = 0; col < source.GetLength(1); ++col)
                    arRow[col] = source[row, col];

                yield return arRow;
            }
        }
        /// <summary>
        ///   Converts a collection of rows from a two dimensional array back into a two dimensional array.
        /// </summary>
        /// <typeparam name="T">The type of the two dimensional array.</typeparam>
        /// <param name="source">The source collection of rows to convert.</param>
        /// <returns>The two dimensional array.</returns>
        private static T[,] ConvertToMultiDimensional<T>(this IEnumerable<T[]> source)
        {
            T[,] twoDimensional;
            T[][] arrayOfArray;
            int numberofColumns;

            arrayOfArray = source.ToArray();
            numberofColumns = (arrayOfArray.Length > 0) ? arrayOfArray[0].Length : 0;
            twoDimensional = new T[arrayOfArray.Length, numberofColumns];

            for (int row = 0; row < arrayOfArray.GetLength(0); ++row)
                for (int col = 0; col < numberofColumns; ++col)
                    twoDimensional[row, col] = arrayOfArray[row][col];

            return twoDimensional;
        }
    }

}
