using System;

using OpenTK.Graphics.OpenGL;

namespace ToolsForKFIV.Rendering
{
    public class MeshLines : IMesh
    {
        public int glVBO
        {
            get { return glVBO; }
            set { glVBO = value; }
        }
        public int glVAO
        {
            get { return glVAO; }
            private set { glVAO = value; }
        }
        public int vertexCount
        {
            get
            {
                return vertexCount;
            }

            private set
            {
                vertexCount = value;
            }
        }

        public void Draw()
        {
            GL.BindVertexArray(glVAO);
            GL.DrawArrays(PrimitiveType.Lines, 0, vertexCount);
        }

        //Disposal
        ~MeshLines()
        {
            Dispose(false);
        }
        protected void Dispose(bool disposeManagedObjects)
        {
            GL.BindVertexArray(0);
            GL.DeleteVertexArray(glVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(glVBO);

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
