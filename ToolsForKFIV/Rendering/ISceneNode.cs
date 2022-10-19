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
        Vector3f Position { get; }

        [Category("Transformation"), Description("Specifies something")]
        Vector3f Rotation { get; }

        [Category("Transformation"), Description("Specifies something")]
        Vector3f Scale { get; }

        bool Visible { get; }
        string Name { get; }
        SceneDraw DrawFlags { get; }

        void Draw(Vector3f position, Vector3f rotation, Vector3f scale);
    }
}
