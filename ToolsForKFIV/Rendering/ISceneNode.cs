using System;
using System.Collections.Generic;
using System.Text;

using FormatKFIV.Utility;

namespace ToolsForKFIV.Rendering
{
    public interface ISceneNode : IDisposable
    {
        //Scene Node Transformation
        Vector3f Position { get; }
        Vector3f Rotation { get; }
        Vector3f Scale { get; }
        bool Visible { get; }

        void Draw(Vector3f position, Vector3f rotation, Vector3f scale);
    }
}
