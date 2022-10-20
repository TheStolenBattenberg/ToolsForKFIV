using System;

using OpenTK.Graphics.OpenGL;

namespace ToolsForKFIV.Rendering
{
    public class LineMesh : IMesh
    {
        //Data
        private int _vbo;
        private int _vao;
        private int _vertexCount;

        //Properties
        public int VBO
        {
            get { return _vbo; }
            private set { _vbo = value; }
        }
        public int VAO
        {
            get { return _vao; }
            private set { _vao = value; }
        }
        public int VertexCount
        {
            get
            {
                return _vertexCount;
            }
            private set
            {
                _vertexCount = value;
            }
        }

        public LineMesh(ref float[] vertices, int vertexCount)
        {
            VertexCount = vertexCount;

            //Construct GL Mesh
            VBO = GL.GenBuffer();
            VAO = GL.GenVertexArray();

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 48, 0);   //Position
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 48, 32);  //Colour
            GL.EnableVertexAttribArray(3);

            GL.BindVertexArray(0);
        }

        public void Draw()
        {
            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Lines, 0, _vertexCount);
        }

        //Disposal
        ~LineMesh()
        {
            Dispose(false);
        }
        protected void Dispose(bool disposeManagedObjects)
        {
            GL.BindVertexArray(0);
            GL.DeleteVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VBO);

            if (disposeManagedObjects)
            {

            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
