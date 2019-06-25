using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixTransformations
{
    public class Vector
    {
        public float x, y, z;

        public Vector()
        {
            x = y = z = 0f;
        }
        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0f;
        }

        public Vector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public static Vector operator +(Vector v1, Vector v2)
        {
            Vector sum = new Vector();
            sum.x = v1.x + v2.x;
            sum.y = v1.y + v2.y;
            return sum;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", x, y, z);
        }
    }
}
