using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ToolsForKFIV.UI.Control;
using ToolsForKFIV.UI;

using FormatKFIV.Asset;
using FormatKFIV.FileFormat;

namespace ToolsForKFIV
{
    public partial class MainWindow : Form
    {
        #region Tool Controls
        private ToolFFini controltool_FileINI = null;
        private ToolFFParam controltool_FilePRM = null;
        private ToolFFImage controltool_FileImage = null;
        private ToolFFModel controltool_FileModel = null;
        private ToolFFScene controltool_Scene = null;
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            //Initialize Tool Controls

            //INI Tool
            controltool_FileINI = new ToolFFini();
            controltool_FileINI.Dock = DockStyle.Fill;
            controltool_FileINI.Margin = Padding.Empty;

            //PRM Tool
            controltool_FilePRM = new ToolFFParam();
            controltool_FilePRM.Dock = DockStyle.Fill;
            controltool_FilePRM.Margin = Padding.Empty;

            //Generic Image Tool
            controltool_FileImage = new ToolFFImage();
            controltool_FileImage.Dock = DockStyle.Fill;
            controltool_FileImage.Margin = Padding.Empty;

            //Gneric Model Tool
            controltool_FileModel = new ToolFFModel();
            controltool_FileModel.Dock = DockStyle.Fill;
            controltool_FileModel.Margin = Padding.Empty;

            //Scene Tool
            controltool_Scene = new ToolFFScene();
            controltool_Scene.Dock = DockStyle.Fill;
            controltool_Scene.Margin = Padding.Empty;

            //Put reference into resource manager
            ResourceManager.winMain = this;
        }

        #region mwMenuStrip Events
        private void openKFIVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Open File Dialog
            DialogResult openResult = mwOpenKFIVDialog.ShowDialog();

            switch(openResult)
            {
                case DialogResult.OK:
                    //Get Found File Information
                    string foundFile = mwOpenKFIVDialog.FileName;
                    string filePath  = Path.GetDirectoryName(foundFile);

                    //Make sure DAT exists at  expected relative location
                    if(!File.Exists(filePath+ "\\DATA\\KF4.DAT"))
                    {
                        Logger.LogWarn("KF4.DAT does not exist.");
                        return;
                    }

                    //Load EXE
                    ResourceManager.KFIVEXE = null;

                    //Load DAT
                    if(ResourceManager.KFIVDAT != null) //Free Old KFIV data
                    {
                        ResourceManager.KFIVDAT.Clear();
                        ResourceManager.KFIVDAT = null;
                        GC.Collect();   //Force GC to run to free the old KFIVDAT
                    }
                    ResourceManager.KFIVDAT = FFResourceDAT.LoadFromFile(filePath + "\\DATA\\KF4.DAT");

                    //Enumurate DAT files into FileTree
                    mwFileTree.EnumurateDATFiles(ResourceManager.KFIVDAT);
                    break;

                default:
                    Logger.LogInfo(":(");
                    break;
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WinSettings winSettings = new WinSettings();
            winSettings.ShowDialog();
        }
        #endregion

        public void OpenTool(TreeNode fileNode)
        {
            //Clear all current controls in the tool panel.
            mwSplit.Panel2.Controls.Clear();

            //Grab the buffer for this file
            byte[] fileBuffer = ResourceManager.KFIVDAT[(int)fileNode.Tag].buffer;
            string fileExtension = Path.GetExtension(fileNode.Text);

            //Do some insane shit to support many different format types easily.
            object formatHandler;
            FEType formatType;
            if(ResourceManager.FormatIsSupported(fileExtension, fileBuffer, out formatType, out formatHandler))
            {
                switch(formatType)
                {
                    case FEType.Model:
                        Logger.LogInfo("Casting To Generic Model Handler...");
                        FIFormat<Model> mdlHandler = (FIFormat<Model>)formatHandler;

                        Logger.LogInfo("Doing Model Import...");
                        Model   ResultingModel;
                        object  ResultingMTexture;

                        ResultingModel = mdlHandler.LoadFromMemory(ResourceManager.KFIVDAT[(int)fileNode.Tag].buffer, out ResultingMTexture, out _, out _);

                        mwSplit.Panel2.Controls.Add(controltool_FileModel);

                        Logger.LogInfo("Bringing in the UI");

                        if (ResultingModel != null)
                        {
                            if (ResultingMTexture != null)
                            {
                                controltool_FileModel.SetModelFile(ResultingModel, (Texture)ResultingMTexture);
                                Console.WriteLine("Model with internal texture");
                            }
                            else
                            {
                                controltool_FileModel.SetModelFile(ResultingModel);
                            }
                        }
                        break;

                    case FEType.Texture:
                        Logger.LogInfo("Casting To Generic Texture Handler...");
                        FIFormat<Texture> texHandler = (FIFormat<Texture>)formatHandler;

                        Logger.LogInfo("Doing Model Import...");
                        Texture ResultingTexture;

                        ResultingTexture = texHandler.LoadFromMemory(ResourceManager.KFIVDAT[(int)fileNode.Tag].buffer, out _, out _, out _);

                        Logger.LogInfo("Doing pretty names...");
                        for(int i = 0; i < ResultingTexture.SubimageCount; ++i)
                        {
                            Texture.ImageBuffer? ibNullable = ResultingTexture.GetSubimage(i);
                            if(ibNullable.HasValue)
                            {
                                //Do pretty names
                                string outName;
                                if(ResourceManager.PrettyNamesData.GetPrettyName(ibNullable.Value.data, out outName))
                                {
                                    ResultingTexture.SetSubimageName(i, outName);
                                }
                            }
                        }

                        Logger.LogInfo("Bringing in the UI");
                        if (ResultingTexture != null)
                            controltool_FileImage.SetTextureData(ResultingTexture);

                        mwSplit.Panel2.Controls.Add(controltool_FileImage);
                        break;

                    case FEType.Scene:
                        Logger.LogInfo("Casting To Generic Scene Handler...");
                        FIFormat<Scene> sceneHandler = (FIFormat<Scene>)formatHandler;

                        Logger.LogInfo("Doing Scene Import...");
                        Scene ResultingScene;
                        ResultingScene = sceneHandler.LoadFromMemory(ResourceManager.KFIVDAT[(int)fileNode.Tag].buffer, out _, out _, out _);

                        Logger.LogInfo("Bringing in the UI");
                        mwSplit.Panel2.Controls.Add(controltool_Scene);

                        if (ResultingScene != null)
                        {
                            controltool_Scene.SetSceneData(ResultingScene);
                        }

                        
                        break;
                }

                return;
            }
        }
    }
}
