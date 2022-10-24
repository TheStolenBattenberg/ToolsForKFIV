using System;
using System.ComponentModel;

using FormatKFIV.Utility;
using FormatKFIV.Asset;

using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ToolsForKFIV.Rendering
{
    public class SceneNodeStaticMesh : ISceneNode
    {
        #region Data and Properties
        //Data
        private bool _visible;
        private Vector3f _position;
        private Vector3f _rotation;
        private Vector3f _scale;
        private Matrix4 Transform;
        private IMesh StaticMesh;
        private string _name = "Static Mesh Node";
        private SceneDraw _drawFlags = SceneDraw.Default;
        private int _textureSlot = -1;
        private uint _textureUID = 0x00000000;

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
            get 
            { 
                return _visible; 
            }
            set 
            { 
                _visible = value; 
            }
        }

        [Browsable(false)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [Category("Rendering"), Description("The texture used for rendering.")]
        public int TextureSlot
        {
            get { return _textureSlot; }
        }
        public uint TextureUID
        {
            get { return _textureUID; }
        }

        [Category("Visibility"), Description("Visibilty filter, 'Default' is always visible.")]
        public SceneDraw DrawFlags
        {
            get { return _drawFlags; }
            set { _drawFlags = value; }
        }


        #endregion

        //Constructors
        public SceneNodeStaticMesh()
        {

        }
        public SceneNodeStaticMesh(IMesh mesh)
        {
            StaticMesh = mesh;
            Visible = true;
        }
        public SceneNodeStaticMesh(Model model, Model.Mesh mesh)
        {
            float[] vertexBuffer = new float[144 * mesh.PrimitiveCount];
            int vertexOffset = 0;

            switch (mesh.primitives[0])
            {
                case Model.TrianglePrimitive trianglep:
                    foreach(Model.TrianglePrimitive p in mesh.primitives)
                    {
                        for(int i = 0; i < 3; ++i)
                        {
                            Model.Components VC = model.Vertices[p.Indices[0 + i]];
                            Model.Components NC = model.Normals[p.Indices[3 + i]];
                            Model.Components TC = model.Texcoords[p.Indices[6 + i]];
                            Model.Components CC = model.Colours[p.Indices[9 + i]];

                            vertexBuffer[vertexOffset + 0] = VC.X;
                            vertexBuffer[vertexOffset + 1] = VC.Y;
                            vertexBuffer[vertexOffset + 2] = VC.Z;
                            vertexBuffer[vertexOffset + 3] = NC.X;
                            vertexBuffer[vertexOffset + 4] = NC.Y;
                            vertexBuffer[vertexOffset + 5] = NC.Z;
                            vertexBuffer[vertexOffset + 6] = TC.U;
                            vertexBuffer[vertexOffset + 7] = TC.V;
                            vertexBuffer[vertexOffset + 8] = CC.R;
                            vertexBuffer[vertexOffset + 9] = CC.G;
                            vertexBuffer[vertexOffset + 10] = CC.B;
                            vertexBuffer[vertexOffset + 11] = CC.A;

                            vertexOffset += 12;
                        }
                    }

                    Name = "Static Mesh Node (Triangle Mesh)";
                    StaticMesh = new TriangleMesh(ref vertexBuffer, mesh.PrimitiveCount * 3);
                    _textureSlot = -1;
                    _textureUID  = 0x00000000;

                    if (mesh.textureSlot != -1)
                    {
                        _textureSlot = mesh.textureSlot;
                        _textureUID = model.TextureSlots[_textureSlot].slotKey;

                        //Ensure only valid texture slots are rendered.
                        if (model.TextureSlots[mesh.textureSlot].slotKey == 0x00000000)
                        {
                            _textureSlot = -1;
                            _textureUID = 0x00000000;
                        }

                        if (!ResourceManager.glTextures.ContainsKey(model.TextureSlots[mesh.textureSlot].slotKey))
                        {
                            _textureSlot = -1;
                            _textureUID = 0x00000000;
                        }
                    }

                    break;

                case Model.LinePrimitive linep:
                    foreach(Model.LinePrimitive p in mesh.primitives)
                    {
                        for(int i = 0; i < 2; ++i)
                        {
                            Model.Components VC = model.Vertices[p.Indices[0 + i]];
                            Model.Components CC = model.Colours[p.Indices[2 + i]];

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
                    Name = "Static Mesh Node (Line Mesh)";
                    StaticMesh = new LineMesh(ref vertexBuffer, mesh.PrimitiveCount * 2);
                    _textureSlot = -1;
                    _textureUID = 0x00000000;
                    break;

            }

            Visible = true;
            Position = mesh.position;
            Rotation = mesh.rotation;
            Scale = mesh.scale;
        }

        //Node Setters and Getters
        public void SetMesh(IMesh mesh)
        {
            StaticMesh = mesh;
        }
        public void SetVisibility(bool visible)
        {
            Visible = visible;
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

            if(_textureSlot != -1)
            {
                //Set my personal texture.
                ResourceManager.glTextures[_textureUID].Bind(TextureUnit.Texture0, TextureTarget.Texture2D);
            }
            else
            {
                ResourceManager.glTextures[0xDEADBEEF].Bind(TextureUnit.Texture0, TextureTarget.Texture2D);
            }

            StaticMesh.Draw();

            ResourceManager.glTextures[0xDEADBEEF].Bind(TextureUnit.Texture0, TextureTarget.Texture2D);
        }

        //Disposal
        ~SceneNodeStaticMesh()
        {
            Dispose(false);
        }

        protected void Dispose(bool disposeManagedObjects)
        {
            if(StaticMesh != null)
            {
                StaticMesh.Dispose();
                StaticMesh = null;
            }

            if (disposeManagedObjects) { }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}
