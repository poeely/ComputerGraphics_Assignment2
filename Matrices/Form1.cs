using MatrixTransformations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Matrices
{
    public partial class Form1 : Form
    {
        // Default Values
        float _d = 800, _r = 10, _theta = -100, _phi = -10;
        // Actual values
        float d, r, theta, phi;

        AxisX x_axis;
        AxisY y_axis;
        AxisZ z_axis;

        Cube cube;

        Matrix modelMatrix;
        Matrix viewMatrix;
        Matrix viewPort;

        Vector position;
        Vector rotation;
        float scale = 1f;

        List<Vector> vb;

        Timer animationTimer;
        bool reachedMax = false;
        int phase = 0;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            this.Width = 800;
            this.Height = 600;

            this.KeyPress += new KeyPressEventHandler(Form1_KeyPress);
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);

            animationTimer = new Timer();
            animationTimer.Interval = 50;
            animationTimer.Tick += new EventHandler(Tick);

            Setup();
        }

        private void Setup()
        {
            vb = new List<Vector>();

            // * * * * Create World Matrix * * * * * * //
            Reset();

            // * * * * Create Viewport * * * * * * //
            Vector center = new Vector(this.Width / 2, this.Height / 2);
            viewPort = Matrix.Translate(center);

            // * * * * Create Cube * * * * * * //
            cube = new Cube(Color.Black);

            // * * * * Create Axis * * * * * * //
            x_axis = new AxisX(2);
            y_axis = new AxisY(2);
            z_axis = new AxisZ(2);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'x')
                rotation.x += 1f;
            else if (e.KeyChar == 'X')
                rotation.x -= 1f;
            else if (e.KeyChar == 'y')
                rotation.y += 1f;
            else if (e.KeyChar == 'Y')
                rotation.y -= 1f;
            else if (e.KeyChar == 'z')
                rotation.z += 1f;
            else if (e.KeyChar == 'Z')
                rotation.z -= 1f;
            else if (e.KeyChar == 's')
                scale += 0.01f;
            else if (e.KeyChar == 'S')
                scale -= 0.01f;
            else if (e.KeyChar == 'c')
                Reset();
            else if (e.KeyChar == 't')
                theta += 1f;
            else if (e.KeyChar == 'T')
                theta -= 1f;
            else if (e.KeyChar == 'p')
                phi += 1f;
            else if (e.KeyChar == 'P')
                phi -= 1f;
            else if (e.KeyChar == 'd')
                d += 1f;
            else if (e.KeyChar == 'D')
                d -= 1f;
            else if (e.KeyChar == 'r')
                r += 1f;
            else if (e.KeyChar == 'R')
                r -= 1f;
            else if (e.KeyChar == 'a')
            {
                Reset();
                if (animationTimer.Enabled)
                    animationTimer.Stop();
                else
                    animationTimer.Start();
            }
            Refresh();
        }

        // Handle KeyDown events
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.PageUp)
            {
                position.z += 1f;
            }
            else if (e.KeyCode == Keys.PageDown)
            {
                position.z -= 1f;
            }
            else if (e.KeyCode == Keys.Left)
            {
                position.x += 1f;
            }
            else if (e.KeyCode == Keys.Right)
            {
                position.x -= 1f;
            }
            else if (e.KeyCode == Keys.Up)
            {
                position.y += 1f;
            }
            else if (e.KeyCode == Keys.Down)
            {
                position.y -= 1f;
            }

            Refresh();
        }

        // Reset values
        private void Reset()
        {
            animationTimer.Stop();
            r = _r;
            d = _d;
            theta = _theta;
            phi = _phi;

            modelMatrix = Matrix.Identity();
            position = new Vector();
            rotation = new Vector();
            scale = 1f;
        }

        // Prepare & Draw the axis and cube
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            viewMatrix = Matrix.CreateViewMatrix(r, phi, theta);

            modelMatrix = Matrix.Translate(position) * (Matrix.Rotate(rotation) * (Matrix.Scale(scale)));
            
            Prepare(cube.vb, modelMatrix);
            cube.Draw(e.Graphics, vb);

            Prepare(x_axis.vb, Matrix.Identity());
            x_axis.Draw(e.Graphics, vb);
            
            Prepare(y_axis.vb, Matrix.Identity());
            y_axis.Draw(e.Graphics, vb);
            
            Prepare(z_axis.vb, Matrix.Identity());
            z_axis.Draw(e.Graphics, vb);

            label1.Text = "Scale: " + scale + "   S/s";
            label2.Text = "TransX: " + position.x + "   Left/Right";
            label3.Text = "TransY: " + position.y + "   Up/Down";
            label4.Text = "TransZ: " + position.z + "   PgDn/PgUp";
            label5.Text = "RotX: " + rotation.x + "   x/X";
            label6.Text = "RotY: " + rotation.y + "   y/Y";
            label7.Text = "RotZ: " + rotation.z + "   z/Z";

            label8.Text = "r: " + r + "   r/R";
            label9.Text = "d: " + d + "   d/D";
            label10.Text = "phi: " + phi + "   p/P";
            label11.Text = "theta: " + theta + "   t/T";

            label12.Text = "Phase: " + phase;
        }

        // Apply the necessary matrices to the vertices in the buffer
        private void Prepare(List<Vector> vertexBuffer, Matrix modelMatrix)
        {
            vb.Clear();
            vb.AddRange(vertexBuffer);

            for (int i = 0; i < vb.Count; i++)
            {
                var temp = new Matrix(vb[i]);

                temp = modelMatrix * temp;
                temp = viewMatrix * temp;
                temp = Matrix.CreateWeakPerspective(d, temp.e[2, 0]) * temp;
                temp.Inverse();
                                
                vb[i] = temp.ToVector();
            }

            // Viewport Transformation
            TransformVertices(viewPort, vb);
        }

        // Apply matrix to vertices in a buffer
        private void TransformVertices(Matrix matrix, List<Vector> vertexBuffer)
        {
            for (int i = 0; i < vertexBuffer.Count; i++)
                vertexBuffer[i] *= matrix;
        }

        // Tick which updates the animation every 50ms
        private void Tick(object sender, EventArgs e)
        {
            Animate();
            Invalidate(true);
        }

        // Animate loop
        private void Animate()
        {
            if (phase == 0 || phase == 1)
                theta -= 1f;
            if (phase == 2)
                phi += 1f;
            // Phase 4: Return theta and phi to starting values
            if(phase == 3)
            {
                if(phi != _phi) phi -= 1f;
                if(theta != _theta) theta += 1f;
                if (phi == _phi && theta == _theta)
                    phase = 0;
            }
            // Phase 1: Scale until 1.5 and shrink (stepsize 0.01)
            if(phase == 0)
                scale = DoPhase(scale, 0.01f, 1f, 1.5f);
            // Phase 2: Rotate 45 degrees over X-axis and back
            else if(phase == 1)
                rotation.x = DoPhase(rotation.x, 1f, 0f, 45f);
            // Phase 3: Rotate 45 degrees over Y-axis and back
            else if (phase == 2)
                rotation.y = DoPhase(rotation.y, 1f, 0f, 45f);

            
        }

        private float DoPhase(float value, float stepSize, float min, float max)
        {
            value += reachedMax ? -stepSize : stepSize;

            if (value >= max)
                reachedMax = true;
            if (!(value <= min)) return value;
            reachedMax = false;
            value = min;
            phase += 1 % 3;
            return value;
        }
    }
}
