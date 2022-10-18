using System;
using System.Windows.Forms;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

using FormatKFIV.Asset;
using FormatKFIV.FileFormat;

using ToolsForKFIV.Utility;

using ToolsForKFIV.Rendering;

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
        private Scene currentScene = null;
        private SceneDraw drawFlags = SceneDraw.Default;

        //NEW!!!
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
            GL.ClearColor(Settings.mtBgCC);
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

            scene.Draw(drawFlags);

            /*
            for (int i = 0; i < currentScene.ChunkCount; ++i)
            {
                //Get a chunk
                Scene.Chunk chunk = currentScene.GetChunk(i);

                //Build chunk transformation
                Matrix4 MAT_TRANS = Matrix4.CreateTranslation(chunk.Position.X / 256f, -chunk.Position.Y / 256f, -chunk.Position.Z / 256f);
                Matrix4 MAT_SCALE = Matrix4.CreateScale(chunk.Scale.X, chunk.Scale.Y, chunk.Scale.Z);
                Matrix4 MAT_ROTTX = Matrix4.CreateRotationX((MathF.PI * 2) - chunk.Rotation.X);
                Matrix4 MAT_ROTTY = Matrix4.CreateRotationY((MathF.PI * 2) - chunk.Rotation.Y);
                Matrix4 MAT_ROTTZ = Matrix4.CreateRotationZ((MathF.PI * 2) - chunk.Rotation.Z);
                matModel = (MAT_ROTTY * MAT_ROTTX * MAT_ROTTZ) * MAT_SCALE * MAT_TRANS;

                glShaders[1].SetUniformMat4("uMVPMatrix", matModel * mVP, false);

                //Draw Chunk
                if (chunk.drawModelID != -1)
                {
                    currentGLScene.scenePieceMdl[chunk.drawModelID].DrawTriangles();
                }

                if(chunk.hitcModelID != -1 && stTSEnableCollision.Checked)
                {
                    GL.Disable(EnableCap.CullFace);
                    currentGLScene.scenePieceCSK[chunk.hitcModelID].DrawTriangles();
                    GL.Enable(EnableCap.CullFace);
                }
            }*/

            //
            // Draw Objects
            //
            /*
            foreach(Scene.Object obj in currentScene.sceneObject)
            {
                //Build Transform
                Matrix4 MAT_TRANS = Matrix4.CreateTranslation(obj.Position.X / 256f, -obj.Position.Y / 256f, -obj.Position.Z / 256f);
                Matrix4 MAT_SCALE = Matrix4.CreateScale(obj.Scale.X, obj.Scale.Y, obj.Scale.Z);
                Matrix4 MAT_ROTTX = Matrix4.CreateRotationX((MathF.PI * 2) - obj.Rotation.X);
                Matrix4 MAT_ROTTY = Matrix4.CreateRotationY((MathF.PI * 2) - obj.Rotation.Y);
                Matrix4 MAT_ROTTZ = Matrix4.CreateRotationZ((MathF.PI * 2) - obj.Rotation.Z);

                matModel = (MAT_ROTTY * MAT_ROTTZ * MAT_ROTTX ) * MAT_SCALE * MAT_TRANS;

                //Do Object Draw
                switch (obj.ClassId)
                {
                    //Special Types
                    case 0x01FB:    //Point Light 1
                    case 0x01FC:    //Point Light 2
                        if (stTSEnableLight.Checked)
                        {
                            glShaders[0].Bind();
                            glShaders[0].SetUniformMat4("uMVPMatrix", matModel * mVP, false);

                            glSceneModels[1].DrawLines();
                        }
                        continue;

                    //Static Type
                    default:
                        if (stTSEnableObj.Checked)
                        {
                            glShaders[1].Bind();
                            glShaders[1].SetUniformMat4("uMVPMatrix", matModel * mVP, false);

                            if (obj.MeshId != -1)
                            {
                                if (obj.TextureId != -1)
                                {
                                    currentGLScene.sceneSObjTexture[obj.TextureId].Bind(TextureUnit.Texture0, TextureTarget.Texture2D);
                                }

                                currentGLScene.scenePieceMdl[obj.MeshId].DrawTriangles();
                            }
                        }
                           break;

                }

            }*/

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
