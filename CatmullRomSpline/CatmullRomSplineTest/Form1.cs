using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CatmullRomSplines;

namespace CatmullRomSplineTest {
    public partial class Form1 : Form {

        private List<Vector2> vectors = new List<Vector2>();

        public Form1() {
            InitializeComponent();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                vectors.Add(new Vector2(e.X, e.Y));
            } else if (e.Button == MouseButtons.Right) {
                vectors.Clear();
            }

            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e) {
            var graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (var vector in vectors) {
                graphics.DrawEllipse(Pens.Red, (float)vector.X - 3, (float)vector.Y - 3, 6, 6);
            }

            if (vectors.Count > 2) {
                var curve = CatmullRomSpline.Calculate(vectors.ToArray(), (float)trackBar1.Value / 100, trackBar2.Value, checkBox1.Checked);
                for (var i = 0; i < curve.Length - 1; ++i) {
                    graphics.DrawLine(Pens.Blue, (float)curve[i].X, (float)curve[i].Y, (float)curve[i + 1].X, (float)curve[i + 1].Y);
                }
                for (var i = 0; i < curve.Length; ++i) {
                    graphics.DrawEllipse(Pens.Blue, (float)curve[i].X - 1, (float)curve[i].Y - 1, 2, 2);
                }
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e) {
            label1.Text = (float)trackBar1.Value / 100 + "";
            pictureBox1.Invalidate();
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e) {
            label4.Text = trackBar2.Value + "";
            pictureBox1.Invalidate();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            pictureBox1.Invalidate();
        }
    }
}
