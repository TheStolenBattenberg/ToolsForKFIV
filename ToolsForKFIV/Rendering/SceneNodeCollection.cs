using System;
using System.Collections.Generic;
using System.Text;

using FormatKFIV.Utility;
using FormatKFIV.Asset;

namespace ToolsForKFIV.Rendering
{
    public class SceneNodeCollection : ISceneNode
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

        public List<ISceneNode> Children
        {
            get
            {
                return Children;
            }
            private set
            {
                Children = value;
            }
        }

        //Scene Node Collection creators.
        public static void FromModel(Model model)
        {
            SceneNodeCollection collection = new SceneNodeCollection();
        }

        //ISceneNode Interface
        public void Draw(Vector3f position, Vector3f rotation, Vector3f scale)
        {

        }

        //Disposal
        ~SceneNodeCollection()
        {
            Dispose(false);
        }

        protected void Dispose(bool disposeManagedObjects)
        {
            foreach (ISceneNode node in Children)
            {
                node.Dispose();
            }

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
