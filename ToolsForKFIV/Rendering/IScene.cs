using System;
using System.Collections.Generic;
using System.Text;



namespace ToolsForKFIV.Rendering
{
    [Flags]
    public enum SceneDraw : uint
    {
        Geometry   = 0x00000001,
        Object     = 0x00000002,
        PointLight = 0x00000004,
        RenderAABB = 0x00000008,
        Collision  = 0x00000010,
        Default    = 0xFFFFFFFF
    }

    public interface IScene : IDisposable
    {
        List<ISceneNode> Nodes { get; }

        void Draw(SceneDraw flags);
    }
}
