using System.Collections.Generic;
using FormatKFIV.Utility;

namespace FormatKFIV.Asset
{
    public class Scene
    {
        public struct Chunk
        {
            public Vector3f position;
            public Vector3f rotation;
            public Vector3f scale;

            public int drawModelID;
            public int collisionModelID;

            public int drawAABB;
            public int collisionAABB;
        }
        public struct Object
        {
            public Vector3f position;
            public Vector3f rotation;
            public Vector3f scale;

            public int classID;
            public byte[] classParams;

            public int drawModelID;
            public int collisionModelID;
            public int textureID;
        }
        public struct Item
        {
            public Vector3f position;
            public Vector3f rotation;
            public Vector3f scale;

            public int classID;

            public int omdID;
            public int texID;
        }

        //Data
        public List<Model> aabbData = new List<Model>();
        public List<Model> omdData = new List<Model>();
        public List<Model> om2Data = new List<Model>();
        public List<Model> cskData = new List<Model>();
        public List<Texture> texData = new List<Texture>();

        public List<Chunk> chunks = new List<Chunk>();
        public List<Object> objects = new List<Object>();
        public List<Item> items = new List<Item>();
    }
}
