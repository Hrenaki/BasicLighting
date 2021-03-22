using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using ShaderCompiler;
using System.IO;

namespace BasicLighting
{
    public class Window : GameWindow
    {
        private float[] vertexes = new float[]
        {
            -1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
            -1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
            1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
            1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
            
            -1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, -1.0f,
            1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, -1.0f,
            1.0f, 1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, -1.0f,
            -1.0f, 1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, -1.0f,

            -1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f, -1.0f, 0.0f, 0.0f,
            -1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, -1.0f, 0.0f, 0.0f,
            -1.0f, 1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, -1.0f, 0.0f, 0.0f,
            -1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f, -1.0f, 0.0f, 0.0f,

            1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f, 0.0f,
            1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f, 0.0f,
            1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f, 0.0f,
            1.0f, 1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f, 0.0f,

            -1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, -1.0f, 0.0f,
            -1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, -1.0f, 0.0f,
            1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, -1.0f, 0.0f,
            1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, -1.0f, 0.0f,

            1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f,
            -1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f,
            -1.0f, 1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f,
            1.0f, 1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f    
        };

        private uint[] poligons = new uint[]
        {
            0, 1, 2, 3,
            4, 5, 6, 7,
            8, 9, 10, 11,
            12, 13, 14, 15,
            16, 17, 18, 19,
            20, 21, 22, 23
        };

        Matrix4 model;

        string pathToVertexShader; 
        string pathToFragShader; 

        Shader shader;

        int vertexesVBO;
        int vertexesVAO;
        int vertexesEBO;

        Matrix4 view, proj;
        float speed = 0.5f;

        Vector3 position = new Vector3(0.0f, 0.0f, 10f);
        Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
        Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
        Vector3 right = new Vector3(1.0f, 0.0f, 0.0f);

        Vector3 ambient = new Vector3(.2f, .2f, .2f);
        Vector3 lampColor = new Vector3(1.0f, 1.0f, 1.0f);
        Vector3 lampPos1 = new Vector3(0.0f, -.5f, 5.0f);
        Vector3 lampPos2 = new Vector3(0.0f, .5f, 5.0f);

        public Window(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
            pathToVertexShader = Path.Combine(Environment.CurrentDirectory, "shader.vert");
            pathToFragShader = Path.Combine(Environment.CurrentDirectory, "shader.frag");
            model = Matrix4.Identity;

            CursorVisible = false;
            CursorGrabbed = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(ambient.X, ambient.Y, ambient.Z, .5f);

            vertexesVBO = GL.GenBuffer();
            vertexesEBO = GL.GenBuffer();
            vertexesVAO = GL.GenVertexArray();

            GL.BindVertexArray(vertexesVAO);
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexesVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexes.Length * sizeof(float), vertexes, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 10 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 10 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 10 * sizeof(float), 7 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vertexesEBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, poligons.Length * sizeof(float), poligons, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            GL.Enable(EnableCap.DepthTest);
            shader = new Shader(pathToVertexShader, pathToFragShader);
            base.OnLoad(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            shader.Use();

            view = Matrix4.LookAt(position, position + front, up);
            proj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), Width / (float)Height, 1.0f, 100.0f);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("proj", proj);
            shader.SetMatrix4("model", model);
            shader.SetVector3("ambient", ambient);
            shader.SetVector3("lampColor", lampColor);
            shader.SetVector3("lampPos", lampPos1);
            shader.SetVector3("lampPos2", lampPos2);
            shader.SetVector3("viewPos", position);

            GL.BindVertexArray(vertexesVAO);
            GL.DrawElements(PrimitiveType.Quads, poligons.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);

            SwapBuffers();
            base.OnRenderFrame(e);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Keyboard.GetState().IsKeyDown(Key.Escape))
                Exit();

            base.OnUpdateFrame(e);
        }
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (!Focused)
                return;
            model = Matrix4.CreateFromQuaternion(Quaternion.FromAxisAngle(Vector3.UnitX, -e.YDelta / 100.0f)) * model;
            base.OnMouseMove(e);
        }
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if (!Focused)
                return;

            // Camera moving
            if (e.Key == Key.LControl)
                position += speed * front;
            if (e.Key == Key.LShift)
                position -= speed * front;
            if (e.Key == Key.D)
                position += speed * right;
            if (e.Key == Key.A)
                position -= speed * right;
            if (e.Key == Key.W)
                position += speed * up;
            if (e.Key == Key.S)
                position -= speed * up;

            base.OnKeyDown(e);
        }
        protected override void OnUnload(EventArgs e)
        {
            shader.Dispose();
            GL.DeleteBuffer(vertexesVBO);
            base.OnUnload(e);
        }
    }
}
