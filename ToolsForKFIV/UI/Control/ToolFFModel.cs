using System;
using System.Windows.Forms;
using System.Windows.Input;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using ToolsForKFIV.Utility;

using FormatKFIV.FileFormat;
using FormatKFIV.Asset;

namespace ToolsForKFIV.UI.Control
{
    public partial class ToolFFModel : UserControl
    {
        //ComboBox Item
        private class ITComboboxItem
        {
            public ITComboboxItem(string text, FIFormat<Model> value)
            {
                Text = text;
                Value = value;
            }

            public string Text { get; set; }
            public FIFormat<Model> Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        //Members
        private List<FIFormat<Model>> exportableFormat = new List<FIFormat<Model>>();

        private Matrix4 matrixProjection;
        private Matrix4 matrixView;
        private Vector3 vectorCEye = new Vector3(0, 0, 16);
        private float cameraX = 225 * 0.0174533f, cameraY = 45;
        private float cameraZoom = 0.2f;
        private int mouseLastX, mouseLastY;

        private Model model;
        private Texture texture;

        private GLShader shader3DNormTexColour;
        private GLShader shader3DColour;
        private GLTexture textureFile = null;
        private GLModel modelGrid;
        private GLModel modelFile = null;

        public ToolFFModel()
        {
            InitializeComponent();
            Disposed += OnDispose;
        }

        #region General Events
        private void OnDispose(object sender, EventArgs e)
        {
            if (shader3DColour != null)
                shader3DColour.Destroy();

            if (shader3DNormTexColour != null)
                shader3DNormTexColour.Destroy();

            if (textureFile != null)
                textureFile.Destroy();

            if (modelGrid != null)
                modelGrid.Destroy();

            if (modelFile != null)
                modelFile.Destroy();
        }

        #endregion
        #region Export Tab Events
        private void tmBtSelExpPath_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                //Copy format details for FIFormat Parameters
                FIFormat<Model> fmt = ((ITComboboxItem)tmLbExpFormat.SelectedItem).Value;
                sfd.Filter = fmt.Parameters.FormatFilter;

                //Show the dialog
                switch (sfd.ShowDialog())
                {
                    case DialogResult.OK:
                        tmTbExpPath.Text = sfd.FileName;    
                        break;
                }
            }
        }
        private void tmTvExport_Enter(object sender, EventArgs e)
        {
            //clear exportable format list
            exportableFormat.Clear();
            tmLbExpFormat.Items.Clear();

            //Get all exportable texture formats
            exportableFormat.AddRange(ResourceManager.GetExportableModelFormats());

            //Add Exportable formats to list
            foreach (FIFormat<Model> fmt in exportableFormat)
            {
                tmLbExpFormat.Items.Add(new ITComboboxItem(fmt.Parameters.FormatDescription, fmt));
            }
            tmLbExpFormat.SelectedItem = tmLbExpFormat.Items[0];
        }
        private void tmBtExport_Click(object sender, EventArgs e)
        {
            if (model != null)
            {
                FIFormat<Model> mdlFmt = ((ITComboboxItem)tmLbExpFormat.SelectedItem).Value;
                mdlFmt.SaveToFile(tmTbExpPath.Text, model);
            }

            if (texture != null)
            {
                FIFormat<Texture> texFmt = new FFTexturePNG();
                texFmt.SaveToFile(tmTbExpPath.Text + ".png", texture);
            }
        }

        #endregion
        #region Preview Tab Events
        private void tmOGL_Load(object sender, EventArgs e)
        {
            //Initialize Shaders
            shader3DColour = new GLShader(ResourceManager.ProgramDirectory + "Resource\\shd_3DColour.vs", ResourceManager.ProgramDirectory + "Resource\\shd_3DColour.fs");
            shader3DNormTexColour = new GLShader(ResourceManager.ProgramDirectory + "Resource\\shd_3DNormTexColour.vs", ResourceManager.ProgramDirectory + "Resource\\shd_3DNormTexColour.fs");
            
            //Initialize Models
            //modelAxis = GLModel.Generate3DAxis();
            modelGrid = GLModel.Generate3DGrid();

            //Initialize Camera
            double f_x = Math.Sqrt(Math.Pow(91, 2) - Math.Pow(cameraY, 2)) * 0.2;
            vectorCEye.X = (float)(Math.Cos(cameraX) * f_x);
            vectorCEye.Z = (float)(Math.Sin(cameraX) * f_x);
            vectorCEye.Y = cameraY * 0.2f;
        }
        private void tmOGL_Resize(object sender, EventArgs e)
        {
            tmOGL.MakeCurrent();

            //Set Viewport
            GL.Viewport(0, 0, tmOGL.ClientSize.Width, tmOGL.ClientSize.Height);

            //Compute Projection Matrix
            matrixProjection = Matrix4.CreatePerspectiveFieldOfView(0.7f, (float)(((float)tmOGL.ClientSize.Width) / ((float)tmOGL.ClientSize.Height)), 0.1f, 8192f);
        }
        private void tmOGL_Paint(object sender, PaintEventArgs e)
        {
            //Compute View Matrix/MVP Matrix
            matrixView = Matrix4.LookAt(vectorCEye, Vector3.Zero, Vector3.UnitY);

            //Do OpenGL
            tmOGL.MakeCurrent();
            GL.ClearColor(ResourceManager.settings.mtBgCC.ToColor());
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            //Draw Editor Shit
            shader3DColour.Bind();
            shader3DColour.SetUniformMat4("cameraMatrix", matrixView * matrixProjection, false);
            
            if (ResourceManager.settings.mtShowGridAxis)
            {
                modelGrid.DrawLines();
            }

            //Draw Loaded File
            if (modelFile != null)
            {
                shader3DNormTexColour.Bind();
                shader3DNormTexColour.SetUniformMat4("cameraMatrix", matrixView * matrixProjection, false);
                shader3DNormTexColour.SetUniformMat4("worldMatrix", Matrix4.Identity, false);
                shader3DNormTexColour.SetUniformInt1("sDiffuse", 0);

                if (textureFile != null)
                {
                    textureFile.Bind(TextureUnit.Texture0, TextureTarget.Texture2D);
                }

                if(modelFile != null)
                {
                    for(int i = 0; i < modelFile.MeshCount; ++i)
                    {
                        if (!tmLbMeshes.GetItemChecked(i))
                            continue;

                        modelFile.DrawTriangleMesh(i);
                    }
                }
            }

            tmOGL.SwapBuffers();
        }
        private void tmOGL_MouseMove(object sender, MouseEventArgs e)
        {
            bool IsInside = (e.X > tmOGL.Location.X) & (e.Y > tmOGL.Location.Y) &
                            (e.X < tmOGL.Location.X + tmOGL.Size.Width) &
                            (e.Y < tmOGL.Location.Y + tmOGL.Size.Height);

            //Do Camera Rotation
            if ((MouseButtons & MouseButtons.Left) > 0 && IsInside == true)
            {
                cameraX -= (mouseLastX - e.X) / 32f;
                cameraY -= (mouseLastY - e.Y);
                cameraY = Math.Clamp(cameraY, -90, 90);

                tmOGL.Invalidate();
            }

            //Do Camera Zoom
            if((MouseButtons & MouseButtons.Right) > 0 && IsInside == true)
            {
                cameraZoom -= (mouseLastY - e.Y) / 100f;
                cameraZoom = Math.Clamp(cameraZoom, 0.02f, 32f);

                tmOGL.Invalidate();
            }

            double f_x = Math.Sqrt(Math.Pow(91, 2) - Math.Pow(cameraY, 2)) * cameraZoom;

            vectorCEye.X = (float)(Math.Cos(cameraX) * f_x);
            vectorCEye.Z = (float)(Math.Sin(cameraX) * f_x);
            vectorCEye.Y = cameraY * cameraZoom;

            mouseLastX = e.X;
            mouseLastY = e.Y;
        }
        private void tmLbMeshes_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            tmOGL.Invalidate();
        }
        #endregion

        public void SetModelFile(Model mod, Texture tex = null)
        {
            if (modelFile != null)
            {
                modelFile.Destroy();
                modelFile = null;
                model = null;
            }

            if (textureFile != null)
            {
                textureFile.Destroy();
                textureFile = null;
            }

            if (tex != null)
            {
                textureFile = GLTexture.GenerateFromAsset(tex);
                texture = tex;
            }
            else
            {
                textureFile = GLTexture.Generate44White();
            }

            modelFile = GLModel.GenerateFromAsset(mod);
            model = mod;

            int tsCounter = 0;
            foreach(Model.TextureSlot ts in model.TextureSlots)
            {
                Console.WriteLine($"Texture Slot [{tsCounter}]: {ts.slotKey.ToString("X8")}");

                tsCounter++;
            }

            SetViewModel(0);

            tmOGL.Invalidate();
        }
        public void SetViewModel(int index)
        {
            if (index < 0)
                return;

            //Fill mesh list
            tmLbMeshes.Items.Clear();
            for (int i = 0; i < model.MeshCount; ++i)
            {
                tmLbMeshes.Items.Add(new ITComboboxItem("Mesh #" + i.ToString("D4"), null));
                tmLbMeshes.SetItemChecked(i, true);
            }

            //Clear properties display
            tmDgModelProperties.Rows.Clear();

            //Fill properties display
            tmDgModelProperties.Rows.Add("Total Vertex", model.VertexCount);
            tmDgModelProperties.Rows.Add("Total Normal", model.NormalCount);
            tmDgModelProperties.Rows.Add("Total Texcoords", model.TexcoordCount);
            tmDgModelProperties.Rows.Add("Total Primitives", model.PrimitiveCount);
        }
    }
}
