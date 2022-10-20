using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using FormatKFIV.Utility;

namespace ToolsForKFIV.Rendering
{
    public interface ISceneNode : IDisposable
    {
        //Scene Node Transformation
        [Category("Transformation"), Description("Specifies something")]
        Vector3f Position { get; set; }

        [Category("Transformation"), Description("Specifies something")]
        Vector3f Rotation { get; set; }

        [Category("Transformation"), Description("Specifies something")]
        Vector3f Scale { get; set; }

        bool Visible { get; }
        string Name { get; }
        SceneDraw DrawFlags { get; }

        void Draw(SceneDraw flags, Vector3f position, Vector3f rotation, Vector3f scale);
    }
}
