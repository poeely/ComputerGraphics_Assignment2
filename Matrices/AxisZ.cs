﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace MatrixTransformations
{
    class AxisZ
    {
        private int size;

        public List<Vector> vb;

        public AxisZ(int size = 100)
        {
            this.size = size;

            vb = new List<Vector>();
            vb.Add(new Vector(0, 0));
            vb.Add(new Vector(0, 0, size));
        }

        public void Draw(Graphics g, List<Vector> vb)
        {
            Pen pen = new Pen(Color.Blue, 2f);
            g.DrawLine(pen, vb[0].x, vb[0].y, vb[1].x, vb[1].y);
            Font font = new Font("Arial", 10);
            PointF p = new PointF(vb[1].x, vb[1].y);
            g.DrawString("z", font, Brushes.Green, p);
        }
    }
}
