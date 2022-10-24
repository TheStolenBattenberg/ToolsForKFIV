using System;
using System.Collections.Generic;
using System.Text;

using FormatKFIV.Asset;
using FormatKFIV.Utility;

using ToolsForKFIV.Utility;

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

            var sceneDrawGeometryRoot = new SceneNodeCollection()
            {
                Position = Vector3f.Zero,
                Rotation = Vector3f.Zero,
                Scale = Vector3f.One,
                Visible = true,
                Name = "Draw Geometry",
                DrawFlags = SceneDraw.Geometry
            };

            var sceneCollisionGeometryRoot = new SceneNodeCollection
            {
                Position = Vector3f.Zero,
                Rotation = Vector3f.Zero,
                Scale = Vector3f.One,
                Visible = true,
                Name = "Collision Geometry",
                DrawFlags = SceneDraw.Collision
            };

            //Load scene Textures
            uint currentTex = 0;
            foreach(Texture tex in scene.texData)
            {
                for(int i = 0; i < tex.SubimageCount; ++i)
                {
                    Texture.ImageBuffer? subImage = tex.GetSubimage(i);

                    if (subImage != null)
                    {
                        Console.WriteLine($"Texture [{currentTex}] SubImage [{i}] = 0x{subImage.Value.UID}");

                        if (ResourceManager.glTextures.ContainsKey(subImage.Value.UID))
                        {
                            continue;
                        }

                        GLTexture glTexture = GLTexture.GenerateFromSubImage(tex, i);

                        ResourceManager.glTextures.Add(subImage.Value.UID, glTexture);
                    }
                }
                currentTex++;
            }

            foreach (Scene.Chunk chunk in scene.chunks)
            {
                //Construct draw geometry
                if(chunk.drawModelID >= 0)
                {
                    var sceneNode = new SceneNodeCollection(scene.omdData[chunk.drawModelID])
                    {
                        Position = new Vector3f(chunk.position.X, chunk.position.Y, chunk.position.Z),
                        Rotation = new Vector3f(chunk.rotation.X, chunk.rotation.Y, chunk.rotation.Z),
                        Scale = new Vector3f(chunk.scale.X, chunk.scale.Y, chunk.scale.Z),
                        Name = $"GeoDrawChunk ({chunk.drawModelID.ToString("D4")})",
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

                    sceneDrawGeometryRoot.Children.Add(sceneNode);
                }

                //Construct collision geometry
                if(chunk.collisionModelID >= 0)
                {
                    var sceneNode = new SceneNodeCollection(scene.cskData[chunk.collisionModelID])
                    {
                        Position = new Vector3f(chunk.position.X, chunk.position.Y, chunk.position.Z),
                        Rotation = new Vector3f(chunk.rotation.X, chunk.rotation.Y, chunk.rotation.Z),
                        Scale = new Vector3f(chunk.scale.X, chunk.scale.Y, chunk.scale.Z),
                        Name = $"GeoCollisionChunk ({chunk.collisionModelID.ToString("D4")})",
                        DrawFlags = SceneDraw.Collision
                    };

                    if (chunk.collisionAABB >= 0)
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

                    sceneCollisionGeometryRoot.Children.Add(sceneNode);
                }
            }

            Nodes.Add(sceneDrawGeometryRoot);
            Nodes.Add(sceneCollisionGeometryRoot);

            //Construct scene objects
            int numObject = 0, numPointLight = 0;
            foreach (Scene.Object obj in scene.objects)
            {
                ISceneNode sceneNode = null;

                switch(obj.classID)
                {
                    case 0x01FB:
                    case 0x01FC:
                        int radius = (obj.classParams[11] << 8) | obj.classParams[10];
                        sceneNode = new SceneNodePointLight(Colour.FromARGB(0xFF, obj.classParams[4], obj.classParams[6], obj.classParams[8]), (float)radius)
                        {
                            Name = $"Point Light #{numPointLight}",
                            DrawFlags = SceneDraw.PointLight
                        };

                        numPointLight++;
                        break;

                    //Until we find exactly what value in the object struct decides if the object uses a OM2 model, this is  the best way.
                    case 0x001A:
                    case 0x0020:
                    case 0x0041:
                    case 0x0044:
                    case 0x0045:
                    case 0x0046:
                        if (obj.drawModelID >= 0)
                        {
                            sceneNode = new SceneNodeCollection(scene.om2Data[obj.drawModelID])
                            {
                                Name = $"Object #{numObject}",
                                DrawFlags = SceneDraw.Object
                            };

                            numObject++;
                        }
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

            //Construct Scene Items
            int numItem = 0;
            foreach(Scene.Item item in scene.items)
            {
                var sceneNode = new SceneNodeCollection(scene.omdData[item.omdID])
                {
                    Position = new Vector3f(item.position.X, item.position.Y, item.position.Z),
                    Rotation = new Vector3f(item.rotation.X, item.rotation.Y, item.rotation.Z),
                    Scale = new Vector3f(item.scale.X, item.scale.Y, item.scale.Z),
                    Name = $"Item #{numObject}",
                    DrawFlags = SceneDraw.Object
                };

                Nodes.Add(sceneNode);

                numItem++;
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
