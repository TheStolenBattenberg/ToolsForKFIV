using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ToolsForKFIV.UI.Control
{
    public class PictureBoxUnfiltered : PictureBox
    {
        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            paintEventArgs.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            base.OnPaint(paintEventArgs);
        }
    }
}
