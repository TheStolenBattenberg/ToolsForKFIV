using System;
using System.Collections.Generic;
using System.Text;



namespace ToolsForKFIV.Rendering
{
    public interface IScene : IDisposable
    {
        List<ISceneNode> Nodes { get; }

        void Draw();
    }
}
