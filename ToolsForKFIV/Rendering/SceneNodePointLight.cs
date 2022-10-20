using System;
using System.ComponentModel;

using FormatKFIV.Utility;
using FormatKFIV.Asset;

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


        //ISceneNode Interface
        public void Draw(SceneDraw flags, Vector3f position, Vector3f rotation, Vector3f scale)
        {

        }


        //IDisposal Interface
        ~SceneNodePointLight()
        {
            Dispose(false);
        }
        protected void Dispose(bool disposeManagedObjects)
        {

            if (disposeManagedObjects) {}
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
