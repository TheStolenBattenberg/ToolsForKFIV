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
    public partial class ToolFFini : UserControl
    {
        public ToolFFini()
        {
            InitializeComponent();
        }

        public void SetINIContent(FFINI iniFile)
        {
            tiTextBox.Lines = iniFile.Lines;
        }
    }
}
