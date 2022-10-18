using System;

namespace ToolsForKFIV.Rendering
{
    public interface IMesh : IDisposable
    {
        int VBO { get; }
        int VAO { get; }

        void Draw();
    }
}
