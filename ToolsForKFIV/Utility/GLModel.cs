using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

using FormatKFIV.Asset;
using FormatKFIV.Utility;

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

            public Vector3f position;
            public Vector3f rotation;
            public Vector3f scale;
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
            GLModel glModel = new GLModel();
            GLMesh glMesh = new GLMesh();
            glMesh.VertexCount = 0;

            Color[] lineColour =
            {
                Color.FromArgb(0xFF, 0xF0, 0xF0, 0xF0),
                Color.FromArgb(0xFF, 0xE8, 0xE8, 0xE8),
                Color.FromArgb(0xFF, 0xE0, 0xE0, 0xE0),
                Color.FromArgb(0xFF, 0xD8, 0xD8, 0xD8),
                Color.FromArgb(0xFF, 0xD0, 0xD0, 0xD0),
                Color.FromArgb(0xFF, 0xC8, 0xC8, 0xC8),
                Color.FromArgb(0xFF, 0xC0, 0xC0, 0xC0),
                Color.FromArgb(0xFF, 0xB8, 0xB8, 0xB8),
                Color.FromArgb(0xFF, 0xB0, 0xB0, 0xB0),
                Color.FromArgb(0xFF, 0xA8, 0xA8, 0xA8),
                Color.FromArgb(0xFF, 0xA0, 0xA0, 0xA0),
                Color.FromArgb(0xFF, 0x98, 0x98, 0x98),
                Color.FromArgb(0xFF, 0x90, 0x90, 0x90),
                Color.FromArgb(0xFF, 0x88, 0x88, 0x88),
                Color.FromArgb(0xFF, 0x80, 0x80, 0x80),
                Color.FromArgb(0xFF, 0x78, 0x78, 0x78)
            };

            List<float> VertexDataList = new List<float>();

            int gridWidth = 15, gridHeight = 15;

            Color xAxisC = ResourceManager.settings.mtXAxC.ToColor();
            Color yAxisC = ResourceManager.settings.mtYAxC.ToColor();
            Color zAxisC = ResourceManager.settings.mtZAxC.ToColor();

            VertexDataList.AddRange(new float[] { 0, 0f, -100 * gridHeight});
            VertexDataList.AddRange(new float[] { xAxisC.R / 255f, xAxisC.G / 255f, xAxisC.B / 255f, 1f });
            VertexDataList.AddRange(new float[] { 0, 0f,  100 * gridHeight});
            VertexDataList.AddRange(new float[] { xAxisC.R / 255f, xAxisC.G / 255f, xAxisC.B / 255f, 1f });
            VertexDataList.AddRange(new float[] { -100 * gridWidth, 0f, 0 });
            VertexDataList.AddRange(new float[] { zAxisC.R / 255f, zAxisC.G / 255f, zAxisC.B / 255f, 1f });
            VertexDataList.AddRange(new float[] {  100 * gridWidth, 0f, 0 });
            VertexDataList.AddRange(new float[] { zAxisC.R / 255f, zAxisC.G / 255f, zAxisC.B / 255f, 1f });
            VertexDataList.AddRange(new float[] { 0f, -100 * (gridWidth / 2), 0f });
            VertexDataList.AddRange(new float[] { yAxisC.R / 255f, yAxisC.G / 255f, yAxisC.B / 255f, 1f });
            VertexDataList.AddRange(new float[] { 0f, 100 * (gridWidth / 2), 0f });
            VertexDataList.AddRange(new float[] { yAxisC.R / 255f, yAxisC.G / 255f, yAxisC.B / 255f, 1f });
            glMesh.VertexCount += 6;

            Color C;
            for(int i = -gridWidth; i <= gridWidth; ++i)
            {
                for (int j = -gridHeight; j <= gridHeight; ++j)
                {
                    if(j == 0)
                    {
                        continue;
                    }

                    C = lineColour[Math.Abs(j)];

                    VertexDataList.AddRange(new float[] { -100 * gridWidth, 0f, 100 * j });
                    VertexDataList.AddRange(new float[] { C.R / 255f, C.G / 255f, C.B / 255f, 1.0f });
                    VertexDataList.AddRange(new float[] { 100 * gridWidth, 0f, 100 * j });
                    VertexDataList.AddRange(new float[] { C.R / 255f, C.G / 255f, C.B / 255f, 1.0f });

                    glMesh.VertexCount += 2;
                }

                if(i == 0)
                {
                    continue;
                }

                C = lineColour[Math.Abs(i)];

                VertexDataList.AddRange(new float[] { 100 * i, 0f, -100 * gridHeight });
                VertexDataList.AddRange(new float[] { C.R / 255f, C.G / 255f, C.B / 255f, 1.0f });
                VertexDataList.AddRange(new float[] { 100 * i, 0f,  100 * gridHeight });
                VertexDataList.AddRange(new float[] { C.R / 255f, C.G / 255f, C.B / 255f, 1.0f });

                glMesh.VertexCount += 2;
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
            List<float> vertexData = new List<float>();

            foreach(Model.Mesh mesh in m.Meshes)
            {
                if (mesh.PrimitiveCount <= 0)
                {
                    Logger.LogWarn("Tried to create GLMesh from a Model.Mesh with zero primitives");
                    continue;
                }

                vertexData.Clear();

                switch(mesh.primitives[0])
                {
                    case Model.LinePrimitive linep:
                        Logger.LogWarn("Line primitives not supported on GLModel!");
                        continue;

                    case Model.TrianglePrimitive trianglep:
                        foreach(Model.TrianglePrimitive primitive in mesh.primitives)
                        {
                            for(int i = 0; i < 3; ++i)
                            {
                                Model.Components VC = m.Vertices[primitive.Indices[0 + i]];
                                Model.Components NC = m.Normals[primitive.Indices[3 + i]];
                                Model.Components TC = m.Texcoords[primitive.Indices[6 + i]];
                                Model.Components CC = m.Colours[primitive.Indices[9 + i]];

                                vertexData.AddRange(new float[] { VC.X, VC.Y, VC.Z });
                                vertexData.AddRange(new float[] { NC.X, NC.Y, NC.Z });
                                vertexData.AddRange(new float[] { TC.U, TC.V });
                                vertexData.AddRange(new float[] { CC.R, CC.G, CC.B, CC.A });
                            }
                        }
                        break;
                }

                GLMesh glMesh = new GLMesh();
                glMesh.VertexCount = vertexData.Count / 12;

                glMesh.position = mesh.position;
                glMesh.rotation = mesh.rotation;
                glMesh.scale = mesh.scale;

                float[] vertexDataArray = vertexData.ToArray();

                glMesh.VBOHandle = GL.GenBuffer();
                glMesh.VAOHandle = GL.GenVertexArray();

                GL.BindVertexArray(glMesh.VAOHandle);
                GL.BindBuffer(BufferTarget.ArrayBuffer, glMesh.VBOHandle);
                GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertexDataArray.Length, vertexDataArray, BufferUsageHint.StaticDraw);

                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 48, 0);
                GL.EnableVertexAttribArray(0);

                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, true, 48, 12);
                GL.EnableVertexAttribArray(1);

                GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 48, 24);
                GL.EnableVertexAttribArray(2);

                GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 48, 32);
                GL.EnableVertexAttribArray(3);

                GL.BindVertexArray(0);

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

            //Build Mesh Transform
            Matrix4 translation = Matrix4.CreateTranslation(mesh.position.X, mesh.position.Y, mesh.position.Z);
            Matrix4 rotationX = Matrix4.CreateRotationX(mesh.rotation.X);
            Matrix4 rotationY = Matrix4.CreateRotationY(mesh.rotation.Y);
            Matrix4 rotationZ = Matrix4.CreateRotationZ(mesh.rotation.Z);
            Matrix4 scale = Matrix4.CreateScale(mesh.scale.X, mesh.scale.Y, mesh.scale.Z);

            Matrix4 transform = ((rotationX * rotationY * rotationZ) * scale) * translation;

            //We need to set the transformation on to the current shader, and this bit of magic does the trick.
            int currentProgram = GL.GetInteger(GetPName.CurrentProgram);
            int worldMatrixLoc = GL.GetUniformLocation(currentProgram, "worldMatrix");
            GL.UniformMatrix4(worldMatrixLoc, false, ref transform);

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
