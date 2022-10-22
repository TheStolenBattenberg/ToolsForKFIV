using System;
using System.ComponentModel;

using FormatKFIV.Utility;
using FormatKFIV.Asset;


using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ToolsForKFIV.Rendering
{
    public class SceneNodePointLight : ISceneNode
    {
        //Data
        private Vector3f _position;
        private Vector3f _rotation;
        private Vector3f _scale;
        private bool _visible;
        private string _name = "Point Light Node";
        private SceneDraw _drawFlags = SceneDraw.Default;
        private Matrix4 Transform;
        private IMesh StaticMesh;

        //Properties
        [Category("Transform"), Description("The position of the scene node")]
        public Vector3f Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        [Category("Transform"), Description("The rotation of the scene node")]
        public Vector3f Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
            }
        }

        [Category("Transform"), Description("the scale of the scene node")]
        public Vector3f Scale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;
            }
        }

        [Category("Visibility"), Description("Base visibilty of the node, ignoring visibility filters.")]
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        //[Category("Misc"), Description("The name of the node")]
        [Browsable(false)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [Category("Visibility"), Description("Visibilty filter, 'Default' is always visible.")]
        public SceneDraw DrawFlags
        {
            get { return _drawFlags; }
            set { _drawFlags = value; }
        }



        public SceneNodePointLight(Colour rgb, float radius)
        {
            Model lineSphere = Model.GenerateLineSphere(Vector3f.Zero, radius, 32, Colour.FromARGB(0xFF, rgb.R, rgb.G, rgb.B));
            Model.Mesh mesh = lineSphere.Meshes[0];

            float[] vertexBuffer = new float[144 * mesh.PrimitiveCount];
            int vertexOffset = 0;

            foreach(Model.LinePrimitive p in mesh.primitives)
            {
                for (int i = 0; i < 2; ++i)
                {
                    Model.Components VC = lineSphere.Vertices[p.Indices[0 + i]];
                    Model.Components CC = lineSphere.Colours[p.Indices[2 + i]];
                    vertexBuffer[vertexOffset + 0] = VC.X;
                    vertexBuffer[vertexOffset + 1] = VC.Y;
                    vertexBuffer[vertexOffset + 2] = VC.Z;
                    vertexBuffer[vertexOffset + 3] = 0f;
                    vertexBuffer[vertexOffset + 4] = 0f;
                    vertexBuffer[vertexOffset + 5] = 0f;
                    vertexBuffer[vertexOffset + 6] = 0f;
                    vertexBuffer[vertexOffset + 7] = 0f;
                    vertexBuffer[vertexOffset + 8] = CC.R;
                    vertexBuffer[vertexOffset + 9] = CC.G;
                    vertexBuffer[vertexOffset + 10] = CC.B;
                    vertexBuffer[vertexOffset + 11] = CC.A;

                    vertexOffset += 12;
                }
            }

            StaticMesh = new LineMesh(ref vertexBuffer, mesh.PrimitiveCount * 2);

            Visible = true;
        }


        //ISceneNode Interface
        public void Draw(SceneDraw flags, Vector3f position, Vector3f rotation, Vector3f scale)
        {
            if (!Visible || (flags & DrawFlags) == 0)
                return;

            //Construct transformation.
            Vector3f worldp = Vector3f.Add(Position, position);
            Vector3f worldr = Vector3f.Add(Rotation, rotation);
            Vector3f worlds = Vector3f.Multiply(Scale, scale);
            Matrix4 translationMatrix = Matrix4.CreateTranslation(worldp.X, worldp.Y, worldp.Z);
            Matrix4 rotationMatrixX = Matrix4.CreateRotationX(worldr.X);
            Matrix4 rotationMatrixY = Matrix4.CreateRotationY(worldr.Y);
            Matrix4 rotationMatrixZ = Matrix4.CreateRotationZ(worldr.Z);
            Matrix4 scaleMatrix = Matrix4.CreateScale(worlds.X, worlds.Y, worlds.Z);
            Transform = ((rotationMatrixX * rotationMatrixY * rotationMatrixZ) * scaleMatrix) * translationMatrix;

            //We need to set the transformation on to the current shader, and this bit of magic does the trick.
            int currentProgram = GL.GetInteger(GetPName.CurrentProgram);
            int worldMatrixLoc = GL.GetUniformLocation(currentProgram, "worldMatrix");
            GL.UniformMatrix4(worldMatrixLoc, false, ref Transform);

            StaticMesh.Draw();
        }


        //IDisposal Interface
        ~SceneNodePointLight()
        {
            Dispose(false);
        }
        protected void Dispose(bool disposeManagedObjects)
        {
            if (StaticMesh != null)
            {
                StaticMesh.Dispose();
                StaticMesh = null;
            }

            if (disposeManagedObjects) {}
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
