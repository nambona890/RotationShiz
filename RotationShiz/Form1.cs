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
        public Form1()
        {
            InitializeComponent();
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
                else if (Keyboard.IsKeyDown(Key.F))
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

                if (Keyboard.IsKeyDown(Key.Down))
                {
                    offy += 0.12;
                }
                else if (Keyboard.IsKeyDown(Key.Up))
                {
                    offy -= 0.12;
                }

            }
            this.Invalidate();
            return;
        }
        public double[,] pointsp = new double[points.Length/3,3];

        static double[,] points = new double[,]
        {
            {   -1,     1,      1       },
            {   1,      1,      1       },
            {   1,      -1,     1       },
            {   -1,     -1,     1       },

            {   -1,     1,      -1      },
            {    1,     1,      -1      },
            {    1,     -1,     -1      },
            {   -1,     -1,     -1      }
        };

        static int[,] vertInd = new int[,]
        {
            {0,1},
            {1,2},
            {2,3},
            {3,0},

            {4,0},
            {3,7},

            {4,5},
            {5,6},
            {6,7},
            {7,4},

            {1,5},
            {2,6}



        };


        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            for (int i = 0; i < points.Length / 3; i++)
            {
                pointsp[i,0] = points[i,0];
                pointsp[i,1] = points[i,1];
                pointsp[i,2] = points[i,2];
            }
            for (int i = 0; i < points.Length / 3; i++)
            {
                double dy = pointsp[i,1];
                double dz = pointsp[i,2];
                pointsp[i,1] = (dy * Math.Cos(rotx)) - (dz * Math.Sin(rotx));
                pointsp[i,2] = (dy * Math.Sin(rotx)) + (dz * Math.Cos(rotx));
            }
            for (int i = 0; i < points.Length / 3; i++)
            {
                double dx = pointsp[i,0];
                double dz = pointsp[i,2];
                pointsp[i,2] = (dz * Math.Cos(roty)) - (dx * Math.Sin(roty));
                pointsp[i,0] = (dz * Math.Sin(roty)) + (dx * Math.Cos(roty));
            }
            for (int i = 0; i < points.Length / 3; i++)
            {
                double dx = pointsp[i,0];
                double dy = pointsp[i,1];
                pointsp[i,0] = (dx * Math.Cos(rotz)) - (dy * Math.Sin(rotz));
                pointsp[i,1] = (dx * Math.Sin(rotz)) + (dy * Math.Cos(rotz));
            }

            Brush bBrush = (Brush)Brushes.Black;
            Brush wBrush = (Brush)Brushes.White;
            Pen wPen = (Pen)Pens.White;
            e.Graphics.FillRectangle(bBrush, 0, 0, this.Width, this.Height);
            int size = Math.Min(this.Width, this.Height);
            for (int i = 0; i < vertInd.Length/2; i++)
            {
                int x1 = Convert.ToInt32(((pointsp[vertInd[i, 0],0] + offx) * (2 / (pointsp[vertInd[i, 0],2] + offz))) * size / 4) + this.Width / 2;
                int y1 = Convert.ToInt32(((pointsp[vertInd[i, 0],1] + offy) * (2 / (pointsp[vertInd[i, 0],2] + offz))) * size / 4) + this.Height / 2;
                int x2 = Convert.ToInt32(((pointsp[vertInd[i, 1],0] + offx) * (2 / (pointsp[vertInd[i, 1],2] + offz))) * size / 4) + this.Width / 2;
                int y2 = Convert.ToInt32(((pointsp[vertInd[i, 1],1] + offy) * (2 / (pointsp[vertInd[i, 1],2] + offz))) * size / 4) + this.Height / 2;
                e.Graphics.DrawLine(wPen, x1, y1, x2, y2);
            }
        }
    }
}
