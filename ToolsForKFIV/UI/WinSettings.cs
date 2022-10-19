using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FormatKFIV.Utility;

namespace ToolsForKFIV.UI
{
    public partial class WinSettings : Form
    {
        public WinSettings()
        {
            InitializeComponent();

            //Load Configuration
            wsBGColPreview.BackColor = ResourceManager.settings.mtBgCC.ToColor(); //Background Colour
            wsLBBGCol.ForeColor = Color.FromArgb(255, 255 - wsBGColPreview.BackColor.R, 255 - wsBGColPreview.BackColor.G, wsBGColPreview.BackColor.B);
            wsXAColPreview.BackColor = ResourceManager.settings.mtXAxC.ToColor(); //X Axis Colour
            wsLBXACol.ForeColor = Color.FromArgb(255, 255 - wsXAColPreview.BackColor.R, 255 - wsXAColPreview.BackColor.G, wsXAColPreview.BackColor.B);
            wsYAColPreview.BackColor = ResourceManager.settings.mtYAxC.ToColor(); //X Axis Colour
            wsLBYACol.ForeColor = Color.FromArgb(255, 255 - wsYAColPreview.BackColor.R, 255 - wsYAColPreview.BackColor.G, wsYAColPreview.BackColor.B);
            wsZAColPreview.BackColor = ResourceManager.settings.mtZAxC.ToColor(); //X Axis Colour
            wsLBZACol.ForeColor = Color.FromArgb(255, 255 - wsZAColPreview.BackColor.R, 255 - wsZAColPreview.BackColor.G, wsZAColPreview.BackColor.B);

            cbDisplayGridAxis.Checked = ResourceManager.settings.mtShowGridAxis;

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

                ResourceManager.settings.mtBgCC = Colour.FromColor(wsColourPicker.Color);
            }
        }

        private void wsBtSetXA_Click(object sender, EventArgs e)
        {
            if (wsColourPicker.ShowDialog() == DialogResult.OK)
            {
                wsXAColPreview.BackColor = wsColourPicker.Color;
                wsLBXACol.ForeColor = Color.FromArgb(255, 255 - wsColourPicker.Color.R, 255 - wsColourPicker.Color.G, 255 - wsColourPicker.Color.B);

                ResourceManager.settings.mtXAxC = Colour.FromColor(wsColourPicker.Color);
            }
        }

        private void wsBtSetYA_Click(object sender, EventArgs e)
        {
            if (wsColourPicker.ShowDialog() == DialogResult.OK)
            {
                wsYAColPreview.BackColor = wsColourPicker.Color;
                wsLBYACol.ForeColor = Color.FromArgb(255, 255 - wsColourPicker.Color.R, 255 - wsColourPicker.Color.G, 255 - wsColourPicker.Color.B);

                ResourceManager.settings.mtYAxC = Colour.FromColor(wsColourPicker.Color);
            }
        }

        private void wsBtSetZA_Click(object sender, EventArgs e)
        {
            if (wsColourPicker.ShowDialog() == DialogResult.OK)
            {
                wsZAColPreview.BackColor = wsColourPicker.Color;
                wsLBZACol.ForeColor = Color.FromArgb(255, 255 - wsColourPicker.Color.R, 255 - wsColourPicker.Color.G, 255 - wsColourPicker.Color.B);

                ResourceManager.settings.mtZAxC = Colour.FromColor(wsColourPicker.Color);
            }
        }
        #endregion

        private void WinSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            ResourceManager.settings.mtShowGridAxis = cbDisplayGridAxis.Checked;

            ResourceManager.settings.SaveConfiguration();
            Logger.LogInfo("Configuration Saved!");
        }
    }
}
