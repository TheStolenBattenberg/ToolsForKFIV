using System;
using System.Collections.Generic;
using System.Text;

using FormatKFIV.Utility;
using FormatKFIV.Asset;
using System.ComponentModel;

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

        [Category("Misc"), Description("The name of the node")]
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

            for(int i = 0; i < model.MeshCount; ++i)
            {
                var staticMeshKFIV = model.GetMesh(i);
                
                //Build vertex buffer
                float[] vertexArray = new float[144 * staticMeshKFIV.numTriangle];
                int vertexStride = 0;

                foreach (Model.Triangle tri in staticMeshKFIV.triangles)
                {
                    for(int j = 0; j < 3; ++j)
                    {
                        //Position
                        Model.Vertex vertex = model.GetVertex(tri.vIndices[j]); 
                        vertexArray[vertexStride + 0] = vertex.X;
                        vertexArray[vertexStride + 1] = vertex.Y;
                        vertexArray[vertexStride + 2] = vertex.Z;

                        //Normal
                        Model.Normal normal = model.GetNormal(tri.nIndices[j]); 
                        vertexArray[vertexStride + 3] = normal.X;
                        vertexArray[vertexStride + 4] = normal.Y;
                        vertexArray[vertexStride + 5] = normal.Z;

                        //Texcoord
                        Model.Texcoord texcoord = model.GetTexcoord(tri.tIndices[j]);
                        vertexArray[vertexStride + 6] = texcoord.U;
                        vertexArray[vertexStride + 7] = texcoord.V;

                        //Colour
                        Model.Colour colour = model.GetColour(tri.cIndices[j]);
                        vertexArray[vertexStride + 8] = colour.R;
                        vertexArray[vertexStride + 9] = colour.G;
                        vertexArray[vertexStride + 10] = colour.B;
                        vertexArray[vertexStride + 11] = colour.A;

                        vertexStride += 12;
                    }
                }

                //Add static mesh to children
                IMesh staticMesh = new TriangleMesh(ref vertexArray, (int) (staticMeshKFIV.numTriangle * 3));
                var staticMeshNode = new SceneNodeStaticMesh(staticMesh)
                {
                    Position = new Vector3f(0, 0, 0),
                    Rotation = new Vector3f(0, 0, 0),
                    Scale = new Vector3f(1, 1, 1)
                };

                Children.Add(staticMeshNode);
            }
        }


        //ISceneNode Interface
        public void Draw(Vector3f position, Vector3f rotation, Vector3f scale)
        {
            if (!Visible)
                return;

            foreach (ISceneNode child in Children)
            {
                child.Draw(Vector3f.Add(Position, position), rotation, Vector3f.Multiply(Scale, scale));
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
