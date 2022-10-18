using System;
using FormatKFIV.Utility;

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

        //Properties
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
        public bool Visible
        {
            get 
            { 
                return _visible; 
            }
            private set 
            { 
                _visible = value; 
            }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
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
        public void Draw(Vector3f position, Vector3f rotation, Vector3f scale)
        {
            if (!Visible)
                return;

            //Construct transformation.
            Vector3f worldp = Vector3f.Add(Position, position);
            Vector3f worldr = Vector3f.Zero;
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
