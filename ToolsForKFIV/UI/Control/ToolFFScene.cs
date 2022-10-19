using System;
using System.Windows.Forms;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

using FormatKFIV.Asset;
using FormatKFIV.FileFormat;
using FormatKFIV.Utility;

using ToolsForKFIV.Utility;
using ToolsForKFIV.Rendering;

using ToolsForKFIV.UI.SceneNodeWidget;

namespace ToolsForKFIV.UI.Control
{
    public partial class ToolFFScene : UserControl
    {
        //Data
        private GLShader[] glShaders;
        private GLModel[]  glSceneModels;
        private GLTexture[] glSceneTextures;

        private Vector3 cameraTo = new Vector3(0, 0, 0), cameraFrom = new Vector3(8, 4, 0);
        private float   camLookX = 0, camLookY = 0;
        private Vector2 mousePosition = new Vector2(0,0), lastMousePosition = new Vector2(0, 0);
        private bool mouseOnWindow = false;

        private Matrix4 matView, matProjection;

        Scene currentScene = null;
        IScene scene = null;

        public ToolFFScene()
        {
            InitializeComponent();
        }

        public void SetSceneData(Scene newScene)
        {
            if(scene != null)
            {
                scene.Dispose();
                scene = null;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();

            currentScene = newScene;
            scene = new Scene3D(currentScene);

            //Populate Scene Node View
            stSceneNodeTree.Nodes.Clear();

            foreach(ISceneNode sceneNode in scene.Nodes)
            {
                RecursiveTreeViewNodes(null, sceneNode);
            }
        }

        private void RecursiveTreeViewNodes(TreeNode treeNode, ISceneNode sceneNode)
        {
            TreeNode node = null;
            switch (sceneNode)
            {
                case SceneNodeCollection snc:

                    if (treeNode == null)
                    {
                        node = stSceneNodeTree.Nodes.Add(sceneNode.Name, sceneNode.Name);
                        node.Tag = sceneNode;
                    }
                    else
                    {
                        node =  treeNode.Nodes.Add(sceneNode.Name, sceneNode.Name);
                        node.Tag = sceneNode;
                    }

                    foreach (ISceneNode childNode in snc.Children)
                    {
                        RecursiveTreeViewNodes(node, childNode);
                    }

                    break;

                default:
                    if (treeNode == null)
                    {
                        node = stSceneNodeTree.Nodes.Add(sceneNode.Name, sceneNode.Name);
                        node.Tag = sceneNode;
                    }
                    else
                    {
                        node = treeNode.Nodes.Add(sceneNode.Name, sceneNode.Name);
                        node.Tag = sceneNode;
                    }
                    break;
            }
        }

        #region Preview Events
        //GLControl
        private void stPreviewGL_Load(object sender, EventArgs e)
        {
            //Initialize Shaders
            glShaders = new GLShader[2];
            glShaders[0] = new GLShader(ResourceManager.ProgramDirectory + "Resource\\shd_3DColour.vs", ResourceManager.ProgramDirectory + "Resource\\shd_3DColour.fs");
            glShaders[1] = new GLShader(ResourceManager.ProgramDirectory + "Resource\\shd_3DNormTexColour.vs", ResourceManager.ProgramDirectory + "Resource\\shd_3DNormTexColour.fs");
            glShaders[1].SetUniformInt1("sDiffuse", 0);

            //Initialize Scene Viewer Models
            glSceneModels = new GLModel[3];
            glSceneModels[0] = GLModel.Generate3DGrid();

            //Initialize Scene Viewer Textures
            glSceneTextures = new GLTexture[2];
            glSceneTextures[0] = GLTexture.Generate44Grid();
            glSceneTextures[1] = GLTexture.Generate44White();
        }

        private void stSceneNodeTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ISceneNode node = (ISceneNode)e.Node.Tag;

            propertyGrid1.SelectedObject = node;
        }

        private void stSceneNodeTree_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            stPreviewGL.Invalidate();
        }

        private void propertyGrid1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void stPreviewGL_Resize(object sender, EventArgs e)
        {
            stPreviewGL.MakeCurrent();

            //Set Viewport
            GL.Viewport(0, 0, stPreviewGL.ClientSize.Width, stPreviewGL.ClientSize.Height);

            //Calculate Projection
            matProjection = Matrix4.CreatePerspectiveFieldOfView(0.7f, (float)(((float)stPreviewGL.ClientSize.Width) / ((float)stPreviewGL.ClientSize.Height)), 0.1f, 256f);
        }

        private void stPreviewGL_Paint(object sender, PaintEventArgs e)
        {
            stPreviewGL.MakeCurrent();
            GL.ClearColor(ResourceManager.settings.mtBgCC.ToColor());
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Texture2D);

            matView = Matrix4.LookAt(cameraFrom, cameraTo, Vector3.UnitY);
            Matrix4 mVP = matView * matProjection;

            glShaders[1].Bind();
            glShaders[1].SetUniformMat4("cameraMatrix", mVP, false);
            glSceneTextures[0].Bind(TextureUnit.Texture0, TextureTarget.Texture2D);

            SceneDraw myDrawFlags = 0;
            myDrawFlags |= SceneDraw.Geometry;
            myDrawFlags |= stTSEnableLight.Checked ? SceneDraw.PointLight : 0;
            myDrawFlags |= stTSEnableObj.Checked ? SceneDraw.Object : 0;

            scene.Draw(myDrawFlags);

            stPreviewGL.SwapBuffers();
        }

        //GLControl Input
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Most Position Logic
            lastMousePosition.X = mousePosition.X;
            lastMousePosition.Y = mousePosition.Y;

            if (mouseOnWindow)
            {
                mousePosition.X = Cursor.Position.X;
                mousePosition.Y = Cursor.Position.Y;
            }

            if ((MouseButtons & MouseButtons.Right) > 0)
            {
                //Mouse Input
                camLookX -= ((lastMousePosition.X - mousePosition.X) * 0.0174533f) / 4f;
                camLookY -= ((lastMousePosition.Y - mousePosition.Y)) / 4f;
                camLookY = Math.Clamp(camLookY, -90f, 90f);

                float f_x = MathF.Sqrt(MathF.Pow(91, 2) - MathF.Pow(camLookY, 2));

                //Keyboard Input
                int wKey = InputManager.KeyIsDown(87) ? 1 : 0;
                int aKey = InputManager.KeyIsDown(65) ? 1 : 0;
                int sKey = InputManager.KeyIsDown(83) ? 1 : 0;
                int dKey = InputManager.KeyIsDown(68) ? 1 : 0;
                int xAxis = (aKey - dKey);
                int yAxis = (wKey - sKey);

                float speed = 1.0f;
                if(xAxis != 0 && yAxis != 0)
                {
                    speed = 0.70710678118f;
                }

                if(yAxis != 0)
                {
                    cameraFrom.X += ((MathF.Cos(camLookX) * f_x) * 0.0039f) * yAxis * speed;
                    cameraFrom.Z += ((MathF.Sin(camLookX) * f_x) * 0.0039f) * yAxis * speed;
                    cameraFrom.Y += (-(camLookY * 0.0039f)) * yAxis * speed;
                }

                if(xAxis != 0)
                {
                    cameraFrom.X += ((MathF.Cos(camLookX - 1.57079632679f) * f_x) * 0.0039f) * xAxis * speed;
                    cameraFrom.Z += ((MathF.Sin(camLookX - 1.57079632679f) * f_x) * 0.0039f) * xAxis * speed;
                }

                //Set Camera To
                cameraTo.X = cameraFrom.X + (MathF.Cos(camLookX) * f_x);
                cameraTo.Z = cameraFrom.Z + (MathF.Sin(camLookX) * f_x);
                cameraTo.Y = cameraFrom.Y - camLookY;

                stPreviewGL.Invalidate();
            }
        }

        private void stPreviewGL_MouseMove(object sender, MouseEventArgs e)
        {
            mouseOnWindow = 
                (e.X > stPreviewGL.Location.X) &
                (e.Y > stPreviewGL.Location.Y) &
                (e.X < stPreviewGL.Location.X + stPreviewGL.Size.Width) &
                (e.Y < stPreviewGL.Location.Y + stPreviewGL.Size.Height);
        }
        #endregion 
    }

}
