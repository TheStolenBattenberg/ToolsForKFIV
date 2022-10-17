using System;
using System.Collections.Generic;
using System.Text;



namespace ToolsForKFIV.Rendering
{
    public interface IScene
    {
        List<ISceneNode> Nodes { get; }
    }
}
