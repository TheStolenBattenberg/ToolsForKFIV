using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FormatKFIV.FileFormat;

namespace ToolsForKFIV.UI.Control
{
    public partial class ToolFFParam : UserControl
    {
        public ToolFFParam()
        {
            InitializeComponent();
        }

        #region Data Grid Events
        private void ToolFFParamEditor_Resize(object sender, EventArgs e)
        {
            tpeDataGrid.AutoResizeColumns();
        }

        #endregion

        #region Editor Modes
        public void SetModeReverbParam(FFParamReverb paramReverb)
        {
            //Clear Previous Defined Data
            tpeDataGrid.Columns.Clear();
            tpeDataGrid.Rows.Clear();

            //Create Columns
            tpeDataGrid.Columns.Add("revType", "Reverb Type");
            tpeDataGrid.Columns.Add("depthL",  "Depth L");
            tpeDataGrid.Columns.Add("depthR",  "Depth R");
            tpeDataGrid.Columns.Add("delay",   "Delay");
            tpeDataGrid.Columns.Add("feedbak", "Feedback");
            tpeDataGrid.Columns.Add("volL",    "Volume L");
            tpeDataGrid.Columns.Add("volR",    "Volume R");
            tpeDataGrid.Columns.Add("entID",   "ID");

            //Fill in rows...
            for(int i = 0; i < paramReverb.ParamCount; ++i)
            {
                tpeDataGrid.Rows.Add(paramReverb[i].ReverbMode, 
                    paramReverb[i].DepthL,
                    paramReverb[i].DepthR,
                    paramReverb[i].Delay,
                    paramReverb[i].Feedback,
                    paramReverb[i].VolumeL,
                    paramReverb[i].VolumeR,
                    i);
            }
        }

        #endregion
    }
}
