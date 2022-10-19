using System.Windows.Forms;

using FormatKFIV.Utility;

namespace ToolsForKFIV.UI.SceneNodeWidget
{
    public partial class SNTransform : UserControl
    {
        public SNTransform(Vector3f position, Vector3f rotation, Vector3f scale)
        {
            InitializeComponent();

            ntPosX.Value = (decimal)position.X;
            ntPosY.Value = (decimal)position.Y;
            ntPosZ.Value = (decimal)position.Z;
            ntRotX.Value = (decimal)rotation.X;
            ntRotY.Value = (decimal)rotation.Y;
            ntRotZ.Value = (decimal)rotation.Z;
            ntScaleX.Value = (decimal)scale.X;
            ntScaleY.Value = (decimal)scale.Y;
            ntScaleZ.Value = (decimal)scale.Z;
        }
    }
}
