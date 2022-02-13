using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ToolsForKFIV.UI
{
    public partial class WinSettings : Form
    {
        public WinSettings()
        {
            InitializeComponent();

            //Tooltips
            ttprettifyassetnames.SetToolTip(cbPrettifyAssetNames, "Enable prettifying of asset names");
        }

        #region Model Tool Events
        //COLOUR PICKERS
        private void button1_Click(object sender, EventArgs e)
        {
            if(wsColourPicker.ShowDialog() == DialogResult.OK)
            {
                wsBGColPreview.BackColor = wsColourPicker.Color;
                wsLBBGCol.ForeColor = Color.FromArgb(255, 255 - wsColourPicker.Color.R, 255 - wsColourPicker.Color.G, 255 - wsColourPicker.Color.B);
            }
        }

        private void wsBtSetXA_Click(object sender, EventArgs e)
        {
            if (wsColourPicker.ShowDialog() == DialogResult.OK)
            {
                wsXAColPreview.BackColor = wsColourPicker.Color;
                wsLBXACol.ForeColor = Color.FromArgb(255, 255 - wsColourPicker.Color.R, 255 - wsColourPicker.Color.G, 255 - wsColourPicker.Color.B);
            }
        }

        #endregion
    }
}
