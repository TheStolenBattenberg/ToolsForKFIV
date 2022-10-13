using System;
using System.Windows.Forms;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

using FormatKFIV.Asset;
using FormatKFIV.FileFormat;

using ToolsForKFIV.Utility;

namespace ToolsForKFIV.UI.Control
{
    public partial class ToolFFScene : UserControl
    {
        //Members
        private GLShader[] glShaders;
        private GLModel[]  glSceneModels;
        private GLTexture[] glSceneTextures;

        private Vector3 cameraTo = new Vector3(0, 0, 0), cameraFrom = new Vector3(8, 4, 0);
        private float   camLookX = 0, camLookY = 0;
        private float lastMouseX, lastMouseY;


        private Matrix4 matModel, matView, matProjection;

        private GLScene currentGLScene = null;
        private Scene currentScene = null;

        public ToolFFScene()
        {
            InitializeComponent();
        }

        public void SetSceneData(Scene newScene)
        {
            if (currentGLScene != null)
            {
                currentGLScene.Destroy();
                currentGLScene = null;
            }
            currentScene = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            currentScene = newScene;
            currentGLScene = new GLScene();
            currentGLScene.BuildScene(currentScene);
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
            glSceneModels[1] = GLModel.GenerateLineSphere(Color.LightYellow);   //Point Light
            glSceneModels[2] = GLModel.GenerateLineSphere(Color.LawnGreen);     //Weird Thing...

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

            //Calculate Look at
            matView = Matrix4.LookAt(cameraFrom, cameraTo, Vector3.UnitY);

            //Calculate VP Matrix
            Matrix4 mVP = matView * matProjection;

            //Draw 3D Grid
            glShaders[0].Bind();
            glShaders[0].SetUniformMat4("uMVPMatrix", mVP, false);
            glSceneModels[0].DrawLines();

            //
            // Draw Map Chunks
            //
            glShaders[1].Bind();
            glSceneTextures[0].Bind(TextureUnit.Texture0, TextureTarget.Texture2D);

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
            }

            //
            // Draw Objects
            //
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

            }


            GL.UseProgram(0);
            stPreviewGL.SwapBuffers();
        }

        //GLControl Input
        private void timer1_Tick(object sender, EventArgs e)
        {
            if ((MouseButtons & MouseButtons.Right) > 0)
            {
                bool aKeyDown = false;

                if (InputManager.KeyIsDown(87))
                {
                    cameraFrom.X += MathF.Cos(camLookX) / 8f;
                    cameraFrom.Z += MathF.Sin(camLookX) / 8f;
                    cameraFrom.Y -= MathF.Sin(camLookY * 0.0174533f) / 8f;
                    aKeyDown = true;
                }else
                if (InputManager.KeyIsDown(83))
                {
                    cameraFrom.X -= MathF.Cos(camLookX) / 8f;
                    cameraFrom.Z -= MathF.Sin(camLookX) / 8f;
                    cameraFrom.Y += MathF.Sin(camLookY * 0.0174533f) / 8f;
                    aKeyDown = true;
                }

                if (aKeyDown)
                    stPreviewGL.Invalidate();
            }
        }
        private void stPreviewGL_MouseMove(object sender, MouseEventArgs e)
        {
            bool IsInside = 
                (e.X > stPreviewGL.Location.X) & 
                (e.Y > stPreviewGL.Location.Y) &
                (e.X < stPreviewGL.Location.X + stPreviewGL.Size.Width) &
                (e.Y < stPreviewGL.Location.Y + stPreviewGL.Size.Height);

            if (!IsInside)
                return;

            if ((MouseButtons & MouseButtons.Right) > 0)
            {
                camLookX -= (lastMouseX - e.X) / 64f;
                camLookY -= (lastMouseY - e.Y);
                camLookY = Math.Clamp(camLookY, -90f, 90f);

                float f_x = MathF.Sqrt(MathF.Pow(91, 2) - MathF.Pow(camLookY, 2));

                cameraTo.X = cameraFrom.X + (MathF.Cos(camLookX) * f_x);
                cameraTo.Z = cameraFrom.Z + (MathF.Sin(camLookX) * f_x);
                cameraTo.Y = cameraFrom.Y - camLookY;
            }

            lastMouseX = e.X;
            lastMouseY = e.Y;
            stPreviewGL.Invalidate();
        }
        #endregion 
    }

}
