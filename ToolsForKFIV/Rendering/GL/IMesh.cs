using System;

namespace ToolsForKFIV.Rendering
{
    public interface IMesh : IDisposable
    {
        int glVBO { get; }
        int glVAO { get; }

        void Draw();
    }
}
