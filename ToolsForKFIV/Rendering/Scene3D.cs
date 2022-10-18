using System;
using System.Collections.Generic;
using System.Text;

using FormatKFIV.Asset;
using FormatKFIV.Utility;

namespace ToolsForKFIV.Rendering
{
    public class Scene3D : IScene
    {
        #region Data and Properties
        //Data
        private List<ISceneNode> _nodes;

        //Properties
        public List<ISceneNode> Nodes
        {
            get
            {
                return _nodes;
            }
            private set
            {
                _nodes = value;
            }
        }

        #endregion

        /// <summary> Construct an empty scene </summary>
        public Scene3D()
        {
            Nodes = new List<ISceneNode>();
        }

        /// <summary> Construct from a FormatKFIV Scene </summary>
        /// <param name="scene">The Scene</param>
        public Scene3D(Scene scene)
        {
            Nodes = new List<ISceneNode>();

            //Construct scene objects
            foreach (Scene.Object obj in scene.sceneObject)
            {
                if(obj.MeshId > 0)
                {
                    var sceneNode = new SceneNodeCollection(scene.GetModel(obj.MeshId))
                    {
                        Position = new Vector3f(obj.Position.X / 256f, -obj.Position.Y / 256f, -obj.Position.Z / 256f),
                        Rotation = new Vector3f(obj.Rotation.X, obj.Rotation.Y, obj.Rotation.Z),
                        Scale = new Vector3f(obj.Scale.X, obj.Scale.Y, obj.Scale.Z)
                    };

                    Nodes.Add(sceneNode);
                }
            }
        }


        //ISceneNode Interface
        public void Draw()
        {
            foreach(ISceneNode node in Nodes)
            {
                node.Draw(Vector3f.Zero, Vector3f.Zero, new Vector3f(1, 1, 1));
            }
        }


        //IDisposal Interface
        ~Scene3D()
        {
            Dispose(false);
        }
        protected void Dispose(bool disposeManagedObjects)
        {
            foreach (ISceneNode node in Nodes)
            {
                node.Dispose();
            }
            Nodes.Clear();

            if (disposeManagedObjects) { }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
