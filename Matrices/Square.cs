using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace MatrixTransformations
{
    // Added in Lecture 3.1
    class Square
    {
        Color color;
        private int size;
        private float weight;

        public List<Vector> vertexBuffer;

        public Square(Color color, int size = 100, float weight = 3)
        {
            this.color = color;
            this.size = size;
            this.weight = weight;

            vertexBuffer = new List<Vector>();
            vertexBuffer.Add(new Vector(-size, -size));
            vertexBuffer.Add(new Vector(size, -size));
            vertexBuffer.Add(new Vector(size, size));
            vertexBuffer.Add(new Vector(-size, size));
        }

        public void Draw(Graphics g, List<Vector> vb)
        {
            Pen pen = new Pen(color, weight);
            g.DrawLine(pen, vb[0].x, vb[0].y, vb[1].x, vb[1].y);
            g.DrawLine(pen, vb[1].x, vb[1].y, vb[2].x, vb[2].y);
            g.DrawLine(pen, vb[2].x, vb[2].y, vb[3].x, vb[3].y);
            g.DrawLine(pen, vb[3].x, vb[3].y, vb[0].x, vb[0].y);
        }
    }
}
