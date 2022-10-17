using System;
using FormatKFIV.Utility;

using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ToolsForKFIV.Rendering
{
    public class SceneNodeStaticMesh : ISceneNode
    {
        //Node Transformation
        public Vector3f Position 
        { 
            get
            {
                return Position;
            }
            private set
            {
                Position = value;
            }
        }
        public Vector3f Rotation
        {
            get
            {
                return Rotation;
            }
            private set
            {
                Rotation = value;
            }
        }
        public Vector3f Scale
        {
            get
            {
                return Scale;
            }
            private set
            {
                Scale = value;
            }
        }
        private Matrix4 Transform;

        //Node Data
        IMesh StaticMesh;

        //ISceneNode Interface
        public void Draw(Vector3f position, Vector3f rotation, Vector3f scale)
        {
            //Construct transformation.
            Matrix4 translationMatrix = Matrix4.CreateTranslation(position.X, Position.Y, Position.Z);
            Matrix4 rotationMatrixX = Matrix4.CreateRotationX(rotation.X);
            Matrix4 rotationMatrixY = Matrix4.CreateRotationY(rotation.Y);
            Matrix4 rotationMatrixZ = Matrix4.CreateRotationZ(rotation.Z);
            Matrix4 scaleMatrix = Matrix4.CreateScale(scale.X, scale.Y, scale.Z);
            Transform = ((rotationMatrixX * rotationMatrixY * rotationMatrixZ) * scaleMatrix) * translationMatrix;

            //We need to set the transformation on to the current shader, and this bit of magic does the trick.
            int currentProgram = GL.GetInteger(GetPName.CurrentProgram);
            int worldMatrixLoc = GL.GetUniformLocation(currentProgram, "matrixWorld");
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
