using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixTransformations
{
    class Matrix
    {
        public float[,] e;
        public int m; // Num of rows
        public int n; // Num of columns

        public Matrix(int m, int n)
        {
            this.m = m;
            this.n = n;
            e = new float[this.m, this.n];

            for (int col = 0; col < n; col++)
                for (int row = 0; row < m; row++)
                    e[row, col] = 0;  
        }

        public Matrix(Vector v)
        {
            m = 4;
            n = 1;
            e = new float[m, n];

            e[0, 0] = v.x;
            e[1, 0] = v.y;
            e[2, 0] = v.z;
            e[3, 0] = 1;
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if (m1.m != m2.m || m1.n != m2.n)
                return m1;

            for (int col = 0; col < m1.n; col++)
                for (int row = 0; row < m1.m; row++)
                    m1.e[row, col] += m2.e[row, col];

            return m1;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (m1.m != m2.m || m1.n != m2.n)
                return m1;

            for (int col = 0; col < m1.n; col++)
                for (int row = 0; row < m1.m; row++)
                    m1.e[row, col] -= m2.e[row, col];
            
            return m1;
        }

        public static Matrix operator *(Matrix m1, float f)
        {
            for (int col = 0; col < m1.n; col++)
                for (int row = 0; row < m1.m; row++)
                    m1.e[row, col] *= f;
            return m1;
        }

        public static Matrix operator *(float f, Matrix m1)
        {
            return m1 * f;
        }
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.n != m2.m) 
            {
                Console.WriteLine("[MATRIX]: Multiplication cannot be performed: n is not equal to m.");
                return m1;
            }

            Matrix matrix = new Matrix(m1.m, m2.n);
            for (int col = 0; col < matrix.n; col++)
                for (int row = 0; row < matrix.m; row++)
                    for (int k = 0; k < matrix.m; k++)
                        matrix.e[row, col] += m1.e[row, k] * m2.e[k, col];

            return matrix;
        }

        public static Vector operator *(Matrix m1, Vector v)
        {
            Matrix vectorMatrix = new Matrix(v);
            m1 *= vectorMatrix;
            return m1.ToVector();
        }

        public static Vector operator *(Vector v, Matrix m1)
        {
            return m1 * v;
        }

        public static Matrix Identity(int size = 4)
        {
            // 1 0 0 0
            // 0 1 0 0
            // 0 0 1 0
            // 0 0 0 1

            Matrix identity = new Matrix(size, size);
            for (int i = 0; i < size; i++)
                identity.e[i, i] = 1;
            return identity;
        }

        // Returns a total matrix for translation, rotation and scalar.
        public static Matrix RST(Vector tv, Vector rv, float s)
        {
            Matrix rst = Scale(s);
            rst *= Rotate(rv);
            rst *= Translate(tv); 
            return rst;
        }

        // * * * * * * Translation * * * * * * //
        public static Matrix Translate(Vector tv)
        {
            return Translate(tv.x, tv.y, tv.z);
        }

        public static Matrix Translate(float x, float y, float z)
        {
            // 1 0 0 x
            // 0 1 0 y
            // 0 0 1 z
            // 0 0 0 1

            Matrix translation = Identity();
            translation.e[0, 3] = x;
            translation.e[1, 3] = y;
            translation.e[2, 3] = z;
            return translation;
        }

        // * * * * * * Scalar * * * * * * //
        public static Matrix Scale(float s, int size = 4)
        {
            // s 0 0 0
            // 0 s 0 0
            // 0 0 s 0
            // 0 0 0 1

            Matrix scale = Identity(size);
            for (int i = 0; i < (size - 1); i++)
                scale.e[i, i] = s;

            return scale;
        }

        // * * * * * * Rotation * * * * * * //
        public static Matrix Rotate(Vector rv)
        {
            return Rotate(rv.x, rv.y, rv.z);
        }

        // Returns a matri to rotate over all axis
        public static Matrix Rotate(float x, float y, float z)
        {
            Matrix rotation = Identity();
            Matrix xRotation = RotateOverX(x);
            Matrix yRotation = RotateOverY(y);
            Matrix zRotation = RotateOverZ(z);
            rotation = yRotation * xRotation * zRotation;

            return rotation;
        }

        // Returns a matrix to rotate over the Z-axis
        public static Matrix RotateOverZ(float degrees)
        {
            Matrix rotation = Identity();
            rotation.e[0, 0] = (float)Math.Cos(degrees * (float)Math.PI / 180f);
            rotation.e[0, 1] = (float)-Math.Sin(degrees * (float)Math.PI / 180f);
            rotation.e[1, 0] = (float)Math.Sin(degrees * (float)Math.PI / 180f);
            rotation.e[1, 1] = (float)Math.Cos(degrees * (float)Math.PI / 180f);
            
            return rotation;
        }

        // Returns a matrix to rotate over the X-axis
        public static Matrix RotateOverX(float degrees)
        {
            Matrix rotation = Identity();
            rotation.e[1, 1] = (float)Math.Cos(degrees * (float)Math.PI / 180f);
            rotation.e[1, 2] = (float)-Math.Sin(degrees * (float)Math.PI / 180f);
            rotation.e[2, 1] = (float)Math.Sin(degrees * (float)Math.PI / 180f);
            rotation.e[2, 2] = (float)Math.Cos(degrees * (float)Math.PI / 180f);

            return rotation;
        }

        // Returns a matrix to rotate over the Y-axis
        public static Matrix RotateOverY(float degrees)
        {
            Matrix rotation = Identity();
            rotation.e[0, 0] = (float)Math.Cos(degrees * (float)Math.PI / 180f);
            rotation.e[0, 2] = (float)Math.Sin(degrees * (float)Math.PI / 180f);
            rotation.e[2, 0] = (float)-Math.Sin(degrees * (float)Math.PI / 180f);
            rotation.e[2, 2] = (float)Math.Cos(degrees * (float)Math.PI / 180f);

            return rotation;
        }

        // Creates a view matrix for the tranformation of the camera.
        public static Matrix CreateViewMatrix(float r, float phi, float theta)
        {
            Matrix viewMatrix = Identity();

            // Convert to degrees
            phi = (phi / 180.0f) * (float)Math.PI;
            theta = (theta / 180.0f) * (float)Math.PI;

            viewMatrix.e[0, 0] = (float) -Math.Sin(theta);                      viewMatrix.e[0, 1] = (float)Math.Cos(theta);
            viewMatrix.e[1, 0] = (float)(-Math.Cos(theta) * Math.Cos(phi));     viewMatrix.e[1, 1] = (float) (-Math.Cos(phi) * Math.Sin(theta));  viewMatrix.e[1, 2] = (float)Math.Sin(phi);
            viewMatrix.e[2, 0] = (float)(Math.Cos(theta) * Math.Sin(phi));      viewMatrix.e[2, 1] = (float)(Math.Sin(theta) * Math.Sin(phi));    viewMatrix.e[2, 2] = (float)Math.Cos(phi); viewMatrix.e[2, 3] = -r;

            return viewMatrix;
        }

        /// <summary>
        /// A 'weak' perspective creates the illusion of perspective by scaling the object
        /// based on its distance from the camera (z).
        /// </summary>
        /// <param name="d"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Matrix CreateWeakPerspective(float d, float z)
        {
            Matrix p = new Matrix(2, 4);
            p.e[0, 0] = -(d / z);
            p.e[1, 1] = -(d / z);
            return p;
        }

        public static Matrix CreatePerspective(float width, float height, float fov, float aspect, float near, float far)
        {
            Matrix p = new Matrix(4, 4);
            p.e[0, 0] = 1f / (float)Math.Tan(fov / 2) * aspect;
            p.e[1, 1] = 1f / (float)Math.Tan(fov / 2);
            p.e[2, 2] = far / (far - near);
            p.e[2, 3] = 1;
            p.e[3, 2] = (-near * far) / (far - near);
            return p;
        }

        public Matrix Inverse()
        {
            for (int col = 0; col < n; col++)
            {
                for (int row = 0; row < m; row++)
                {
                    if (col == row) continue;
                    e[row, col] *= -1;
                }   
            }
            return this;  
        }

        public Vector ToVector()
        {
            if (m >= 3)
                return new Vector(e[0, 0], e[1, 0], e[2, 0]);
            else
                return new Vector(e[0, 0], e[1, 0]);
        }

        public override string ToString()
        {
            string line = "";

            for (int row = 0; row < m; row++)
            {
                line += "[";
                for (int col = 0; col < n; col++)
                    line += e[row, col] + ", ";
                line += "]" + "\n";
            } 

            return line;
        }
    }
}
