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
