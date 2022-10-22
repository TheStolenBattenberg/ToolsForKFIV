using System;
using System.Collections.Generic;
using System.ComponentModel;

using FormatKFIV.Utility;
using FormatKFIV.Asset;


namespace ToolsForKFIV.Rendering
{
    public class SceneNodeCollection : ISceneNode
    {
        #region Data and Properties
        //Data
        private List<ISceneNode> _children;
        private Vector3f _position;
        private Vector3f _rotation;
        private Vector3f _scale;
        private bool _visible;
        private string _name = "Collection Node";
        private SceneDraw _drawFlags = SceneDraw.Default;

        //Properties
        [Browsable(false)]
        public List<ISceneNode> Children
        {
            get
            {
                return _children;
            }
            private set
            {
                _children = value;
            }
        }

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

        #endregion

        /// <summary> Construct an empty SceneNodeCollection</summary>
        public SceneNodeCollection()
        {
            Children = new List<ISceneNode>();
        }

        /// <summary> Construct a SceneNodeCollection from a FormatKFIV model asset </summary>
        /// <param name="model">The Format KFIV model</param>
        public SceneNodeCollection(Model model)
        {
            Children = new List<ISceneNode>();
            Visible = true;

            foreach(Model.Mesh mesh in model.Meshes)
            {
                //Skip empty meshes
                if(mesh.PrimitiveCount <= 0)
                {
                    continue;
                }

                ISceneNode staticMesh = new SceneNodeStaticMesh(model, mesh);
                Children.Add(staticMesh);
            }
        }


        //ISceneNode Interface
        public void Draw(SceneDraw flags, Vector3f position, Vector3f rotation, Vector3f scale)
        {
            if (!Visible || (flags & DrawFlags) == 0)
                return;

            foreach (ISceneNode child in Children)
            {
                child.Draw(flags, Vector3f.Add(Position, position), Vector3f.Add(Rotation, rotation), Vector3f.Multiply(Scale, scale));
            }
        }


        //IDisposal Interface
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
