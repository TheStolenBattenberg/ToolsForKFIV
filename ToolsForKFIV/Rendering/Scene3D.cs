using System;
using System.Collections.Generic;
using System.Text;

namespace ToolsForKFIV.Rendering
{
    public class Scene3D : IScene
    {
        public List<ISceneNode> Nodes
        {
            get
            {
                return Nodes;
            }
            private set
            {
                Nodes = value;
            }
        }

    }
}
