using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

using OpenTK.Graphics.OpenGL;

using FormatKFIV.Asset;

namespace ToolsForKFIV.Utility
{
    /// <summary>Container for 3D data formatted for use with OpenGL.</summary>
    public class GLModel
    {
        /// <summary>Structure for storing a single mesh</summary>
        private struct GLMesh
        {
            public int VAOHandle;
            public int VBOHandle;
            public int VertexCount;
        }

        //Properties
        public int MeshCount
        {
            get { return GLMeshes.Count; }
        }

        //Members
        private List<GLMesh> GLMeshes = new List<GLMesh>();

        /// <summary>Static function generates a 3D grid for use in a model viewer.</summary>
        /// <returns>A GLModel containing a grid model</returns>
        public static GLModel Generate3DGrid()
        {
            //Create model to return
            GLModel glModel = new GLModel();
            GLMesh glMesh = new GLMesh();
            glMesh.VertexCount = 0;

            //Grid Colour array
            Color[] lineColour =
            {
                Color.FromArgb(255, 250, 250, 250), //0
                Color.FromArgb(255, 245, 245, 245), //1
                Color.FromArgb(255, 238, 238, 238), //2
                Color.FromArgb(255, 224, 224, 224), //3
                Color.FromArgb(255, 189, 189, 189), //4
                Color.FromArgb(255, 158, 158, 158), //5
                Color.FromArgb(255, 117, 117, 117), //6
                Color.FromArgb(255, 97,  97,  97),  //7
                Color.FromArgb(255, 66,  66,  66),  //8
            };

            //Create vertex data
            List<float> VertexDataList = new List<float>();

            for (int i = -8; i <= 8; ++i)
            {
                //I like the effect this gives, but need to improve line generation to make it sweet af.
                Color curColour = lineColour[Math.Abs(i)];

                if (i != 0)
                {
                    VertexDataList.AddRange(new float[] { i, 0f, -8f });
                    VertexDataList.AddRange(new float[] { curColour.R / 255f, curColour.G / 255f, curColour.B / 255f, curColour.A / 255f });
                    VertexDataList.AddRange(new float[] { i, 0f, 8f });
                    VertexDataList.AddRange(new float[] { curColour.R / 255f, curColour.G / 255f, curColour.B / 255f, curColour.A / 255f });
                    VertexDataList.AddRange(new float[] { -8f, 0f, i });
                    VertexDataList.AddRange(new float[] { curColour.R / 255f, curColour.G / 255f, curColour.B / 255f, curColour.A / 255f });
                    VertexDataList.AddRange(new float[] { 8f, 0f, i });
                    VertexDataList.AddRange(new float[] { curColour.R / 255f, curColour.G / 255f, curColour.B / 255f, curColour.A / 255f });

                    glMesh.VertexCount += 4;
                }
                else
                {
                    VertexDataList.AddRange(new float[] { i, 0f, -8f });
                    VertexDataList.AddRange(new float[] { 244 / 255f, 67 / 255f, 54 / 255f, 1f });
                    VertexDataList.AddRange(new float[] { i, 0f, 8f });
                    VertexDataList.AddRange(new float[] { 244 / 255f, 67 / 255f, 54 / 255f, 1f });
                    VertexDataList.AddRange(new float[] { -8f, 0f, i });
                    VertexDataList.AddRange(new float[] { 3 / 255f, 169 / 255f, 244 / 255f, 1f });
                    VertexDataList.AddRange(new float[] { 8f, 0f, i });
                    VertexDataList.AddRange(new float[] { 3 / 255f, 169 / 255f, 244 / 255f, 1f });
                    VertexDataList.AddRange(new float[] { 0f, -8f, 0f });
                    VertexDataList.AddRange(new float[] { 76 / 255f, 175 / 255f, 80 / 255f, 1f });
                    VertexDataList.AddRange(new float[] { 0f,  8f, 0f });
                    VertexDataList.AddRange(new float[] { 76 / 255f, 175 / 255f, 80 / 255f, 1f });

                    glMesh.VertexCount += 6;
                }
            }

            float[] GridVertexData = VertexDataList.ToArray();

            //Generate VBO
            glMesh.VBOHandle = GL.GenBuffer();

            //Generate VAO
            glMesh.VAOHandle = GL.GenVertexArray();
            GL.BindVertexArray(glMesh.VAOHandle);

            //Fill VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, glMesh.VBOHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, GridVertexData.Length * sizeof(float), GridVertexData, BufferUsageHint.StaticDraw);

            //Set Attribute Pointers
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 28, 0);   //Position
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 28, 12);  //Colour
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            //Stop Accidental writes into this VAO by unbinding it
            GL.BindVertexArray(0);

            VertexDataList.Clear();

            glModel.GLMeshes.Add(glMesh);

            return glModel;
        }

        /// <summary>Static function generates a sphere, represented by lines.</summary>
        /// <returns>GLModel of a line sphere</returns>
        public static GLModel GenerateLineSphere(Color C)
        {
            //Initialize GLModel and GLMesh
            GLModel glModel = new GLModel();
            GLMesh  glMesh = new GLMesh();
            glMesh.VertexCount = 0;

            //Number and size of circle segments.
            float segments = 16;
            float segmentSize = (float)((Math.PI * 2f) / segments);

            //Vertex Data Storage.
            List<float> VertexDataList = new List<float>();

            //Build line sphere
            for(int i = 0; i < segments; ++i)
            {
                float point1X = MathF.Cos(segmentSize * i);
                float point1Y = MathF.Sin(segmentSize * i);
                float point2X = MathF.Cos(segmentSize * (i + 1));
                float point2Y = MathF.Sin(segmentSize * (i + 1));

                //XZ Axis
                VertexDataList.AddRange(new float[] { point1X, 0.0f, point1Y });
                VertexDataList.AddRange(new float[] { C.R / 255f, C.G / 255f, C.B / 255f, 1.0f });
                VertexDataList.AddRange(new float[] { point2X, 0.0f, point2Y });
                VertexDataList.AddRange(new float[] { C.R / 255f, C.G / 255f, C.B / 255f, 1.0f });

                //XY Axis
                VertexDataList.AddRange(new float[] { point1X, point1Y, 0.0f });
                VertexDataList.AddRange(new float[] { C.R / 255f, C.G / 255f, C.B / 255f, 1.0f });
                VertexDataList.AddRange(new float[] { point2X, point2Y, 0.0f });
                VertexDataList.AddRange(new float[] { C.R / 255f, C.G / 255f, C.B / 255f, 1.0f });

                //ZY Axis
                VertexDataList.AddRange(new float[] { 0.0f, point1Y, point1X });
                VertexDataList.AddRange(new float[] { C.R / 255f, C.G / 255f, C.B / 255f, 1.0f });
                VertexDataList.AddRange(new float[] { 0.0f, point2Y, point2X });
                VertexDataList.AddRange(new float[] { C.R / 255f, C.G / 255f, C.B / 255f, 1.0f });

                glMesh.VertexCount += 6;
            }

            //Generate GLMesh and GLModel.
            float[] VertexDataArray = VertexDataList.ToArray();

            //Generate VBO
            glMesh.VBOHandle = GL.GenBuffer();

            //Generate VAO
            glMesh.VAOHandle = GL.GenVertexArray();
            GL.BindVertexArray(glMesh.VAOHandle);

            //Fill VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, glMesh.VBOHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, VertexDataArray.Length * sizeof(float), VertexDataArray, BufferUsageHint.StaticDraw);

            //Set Attribute Pointers
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 28, 0);   //Position
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 28, 12);  //Colour
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            //Stop Accidental writes into this VAO by unbinding it
            GL.BindVertexArray(0);

            VertexDataList.Clear();

            glModel.GLMeshes.Add(glMesh);

            return glModel;
        }

        /// <summary>Static function generates an OpenGL formatted model from an KFIV Asset Model</summary>
        /// <param name="m">The model for the data.</param>
        /// <returns>A GLModel of the input FormatKFIV Model</returns>
        public static GLModel GenerateFromAsset(Model m)
        {
            //Create model to return
            GLModel dataModel = new GLModel();

            //Generate Vertex Buffer.
            List<float> VertexDataList = new List<float>();

            for(int i = 0; i < m.MeshCount; ++i)
            {
                Model.Mesh mesh = m.GetMesh(i);
                GLMesh glMesh = new GLMesh();

                foreach (Model.Triangle tri in mesh.triangles)
                {
                    for(int j = 0; j < 3; ++j)
                    {
                        Model.Vertex V = m.GetVertex(tri.vIndices[j]);
                        VertexDataList.Add(V.X);
                        VertexDataList.Add(V.Y);
                        VertexDataList.Add(V.Z);

                        Model.Normal N = m.GetNormal(tri.nIndices[j]);
                        VertexDataList.Add(N.X);
                        VertexDataList.Add(N.Y);
                        VertexDataList.Add(N.Z);

                        Model.Texcoord T = m.GetTexcoord(tri.tIndices[j]);
                        VertexDataList.Add(T.U);
                        VertexDataList.Add(T.V);

                        Model.Colour C = m.GetColour(tri.cIndices[j]);
                        VertexDataList.Add(C.R);
                        VertexDataList.Add(C.G);
                        VertexDataList.Add(C.B);
                        VertexDataList.Add(C.A);
                    }
                }

                //Put vertices into GLMesh
                glMesh.VertexCount = (VertexDataList.Count / 12);

                //Build GLMesh
                float[] ModelVertexData = VertexDataList.ToArray();

                glMesh.VBOHandle = GL.GenBuffer();
                glMesh.VAOHandle = GL.GenVertexArray();

                GL.BindVertexArray(glMesh.VAOHandle);
                GL.BindBuffer(BufferTarget.ArrayBuffer, glMesh.VBOHandle);
                GL.BufferData(BufferTarget.ArrayBuffer, ModelVertexData.Length * sizeof(float), ModelVertexData, BufferUsageHint.StaticDraw);

                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 48, 0);   //Position
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, true, 48, 12);   //Normal
                GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 48, 24);  //Texcoord
                GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 48, 32);  //Colour
                GL.EnableVertexAttribArray(0);
                GL.EnableVertexAttribArray(1);
                GL.EnableVertexAttribArray(2);
                GL.EnableVertexAttribArray(3);

                GL.BindVertexArray(0);

                ModelVertexData = null;
                VertexDataList.Clear();

                dataModel.GLMeshes.Add(glMesh);
            }

            return dataModel;
        }

        /// <summary>Draw entire nodel with line primitives</summary>
        public void DrawLines()
        {
            foreach(GLMesh mesh in GLMeshes)
            {
                GL.BindVertexArray(mesh.VAOHandle);
                GL.DrawArrays(PrimitiveType.Lines, 0, mesh.VertexCount);
            }
        }

        /// <summary>Draw entire nodel with triangle primitives</summary>
        public void DrawTriangles()
        {
            foreach (GLMesh mesh in GLMeshes)
            {
                GL.BindVertexArray(mesh.VAOHandle);
                GL.DrawArrays(PrimitiveType.Triangles, 0, mesh.VertexCount);
            }
        }

        /// <summary>Draw a single mesh with triangle primitives</summary>
        public void DrawTriangleMesh(int index)
        {
            if (index < 0 || index > GLMeshes.Count)
                return;

            GLMesh mesh = GLMeshes[index];

            GL.BindVertexArray(mesh.VAOHandle);
            GL.DrawArrays(PrimitiveType.Triangles, 0, mesh.VertexCount);
        }

        /// <summary>Destroy the OpenGL model</summary>
        public void Destroy()
        {
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            foreach (GLMesh mesh in GLMeshes)
            {
                GL.DeleteVertexArray(mesh.VAOHandle);
                GL.DeleteBuffer(mesh.VBOHandle);
            }

            GLMeshes.Clear();
            GLMeshes = null;
        }
    }
}
