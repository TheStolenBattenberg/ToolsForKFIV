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

            //Construct scene geometry
            var sceneDrawGeometryRoot = new SceneNodeCollection()
            {
                Position = Vector3f.Zero,
                Rotation = Vector3f.Zero,
                Scale = Vector3f.One,
                Visible = true,
                Name = "Map Geometry",
                DrawFlags = SceneDraw.Geometry
            };

            foreach (Scene.Chunk chunk in scene.chunks)
            {
                if(chunk.drawModelID >= 0)
                {
                    var sceneNode = new SceneNodeCollection(scene.omdData[chunk.drawModelID])
                    {
                        Position = new Vector3f(chunk.position.X, chunk.position.Y, chunk.position.Z),
                        Rotation = new Vector3f(chunk.rotation.X, chunk.rotation.Y, chunk.rotation.Z),
                        Scale = new Vector3f(chunk.scale.X, chunk.scale.Y, chunk.scale.Z),
                        Name = $"GeoChunk ({chunk.drawModelID.ToString("D4")})",
                        DrawFlags = SceneDraw.Geometry
                    };

                    if(chunk.drawAABB >= 0)
                    {
                        var aabbNode = new SceneNodeStaticMesh(scene.aabbData[chunk.drawAABB], scene.aabbData[chunk.drawAABB].Meshes[0])
                        {
                            Position = Vector3f.Zero,
                            Rotation = Vector3f.Zero,
                            Scale = Vector3f.One,
                            Name = $"Render AABB",
                            DrawFlags = SceneDraw.RenderAABB
                        };

                        sceneNode.Children.Add(aabbNode);
                    }

                    if(chunk.collisionAABB >= 0)
                    {
                        var aabbNode = new SceneNodeStaticMesh(scene.aabbData[chunk.collisionAABB], scene.aabbData[chunk.collisionAABB].Meshes[0])
                        {
                            Position = Vector3f.Zero,
                            Rotation = Vector3f.Zero,
                            Scale = Vector3f.One,
                            Name = $"Collision AABB",
                            DrawFlags = SceneDraw.RenderAABB
                        };

                        sceneNode.Children.Add(aabbNode);
                    }

                    //Chunk AABBs
                    sceneDrawGeometryRoot.Children.Add(sceneNode);
                }
            }

            Nodes.Add(sceneDrawGeometryRoot);

            //Construct scene objects
            int numObject = 0, numPointLight = 0;
            foreach (Scene.Object obj in scene.objects)
            {
                ISceneNode sceneNode = null;

                switch(obj.classID)
                {
                    case 0x01FB:
                    case 0x01FC:
                        sceneNode = new SceneNodePointLight()
                        {
                            Name = $"Point Light #{numPointLight}",
                            DrawFlags = SceneDraw.PointLight
                        };

                        numPointLight++;
                        break;

                    default:
                        if (obj.drawModelID >= 0)
                        {
                            sceneNode = new SceneNodeCollection(scene.omdData[obj.drawModelID])
                            {
                                Name = $"Object #{numObject}",
                                DrawFlags = SceneDraw.Object
                            };

                            numObject++;
                        }
                        break;
                }

                if(sceneNode != null)
                {
                    sceneNode.Position = new Vector3f(obj.position.X, obj.position.Y, obj.position.Z);
                    sceneNode.Rotation = new Vector3f(obj.rotation.X, obj.rotation.Y, obj.rotation.Z);
                    sceneNode.Scale = new Vector3f(obj.scale.X, obj.scale.Y, obj.scale.Z);
                    Nodes.Add(sceneNode);
                }
            }
        }


        //ISceneNode Interface
        public void Draw(SceneDraw flags)
        {
            foreach(ISceneNode node in Nodes)
            {
                if((flags & node.DrawFlags) == 0)
                {
                    continue;
                }

                node.Draw(flags, Vector3f.Zero, Vector3f.Zero, Vector3f.One);
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
