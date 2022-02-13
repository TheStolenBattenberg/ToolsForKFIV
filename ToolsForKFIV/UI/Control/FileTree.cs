using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FormatKFIV.FileFormat;
using FormatKFIV.Utility;

namespace ToolsForKFIV.UI.Control
{
    public partial class FileTree : UserControl
    {
        public FileTree()
        {
            InitializeComponent();
        }

        /// <summary>Clears current path structure</summary>
        public void ClearEnumuratedFiles()
        {
            ftTreeView.Nodes.Clear();
        }

        /// <summary>Adds path structure from a DAT Resource File</summary>
        public void EnumurateDATFiles(FFResourceDAT datFile)
        {
            //Create Main Parent Node.
            TreeNode baseDATNode = ftTreeView.Nodes.Add("KFIV.DAT");
            baseDATNode.Name = "KFIV.DAT";
            baseDATNode.Tag = -1;    //-1 Represents a directory for us.
            baseDATNode.ImageIndex = 0;
            baseDATNode.SelectedImageIndex = baseDATNode.ImageIndex;

            //First Pass creates directory structure.
            for (int i = 0; i < datFile.FileCount; ++i)
            {
                //Get File Name
                string datFilePath = datFile[i].name;

                if(datFilePath.IndexOf("/") > 0)
                {
                    string[] splitPath = datFilePath.Split('/');

                    TreeNode parentNode = baseDATNode;
                    //How many splits were there? defines the search count.
                    for(int j = 0; j < splitPath.Length-1; ++j)
                    {
                        if(!parentNode.Nodes.ContainsKey(splitPath[j]))
                        {
                            parentNode = parentNode.Nodes.Add(splitPath[j]);
                            parentNode.Name = splitPath[j];
                            parentNode.Tag  = -1;   //-1 Represents a directory for us.
                            parentNode.ImageIndex = 0;
                            parentNode.SelectedImageIndex = parentNode.ImageIndex;
                        }
                        else
                        {
                            parentNode = parentNode.Nodes.Find(splitPath[j], false)[0];
                        }
                    }
                }
            }

            //Second Pass adds files to directory structure
            for(int i = 0; i < datFile.FileCount; ++i)
            {
                //Get File Name
                string datFilePath = datFile[i].name;

                //Split the file name by path seperator
                string[] splitPath = datFilePath.Split('/');

                //Find Node Path
                TreeNode parentNode = baseDATNode;

                for (int j = 0; j < splitPath.Length; ++j)
                {
                    //Is file
                    if(j == (splitPath.Length-1))
                    {
                        TreeNode fNode = parentNode.Nodes.Add(splitPath[j]);

                        FileTypeIdentifier.FileFormatType iden = FileTypeIdentifier.IdentifyByExtension(splitPath[j]);
                        fNode.Tag = i;
                        fNode.ImageIndex = 1 + ((int)iden);
                        fNode.SelectedImageIndex = fNode.ImageIndex;

                        continue;
                    }

                    parentNode = parentNode.Nodes.Find(splitPath[j], false)[0];
                }
            }
        }

        #region Tree View Events
        private void ftTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ResourceManager.winMain.OpenTool(e.Node);
        }

        #endregion
    }
}
