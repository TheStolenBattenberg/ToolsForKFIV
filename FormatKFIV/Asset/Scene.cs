using System.Collections.Generic;
using System;

using FormatKFIV.FileFormat;
using FormatKFIV.Utility;

namespace FormatKFIV.Asset
{
    public class Scene
    {
        public struct Chunk
        {
            public Vector4f Position;
            public Vector4f Rotation;
            public Vector4f Scale;

            public int drawModelID;    //Should be -1 when non existant
            public int hitcModelID;    //Should be -1 when non existant
            public uint Flags;
        }
        public struct Object
        {
            public Vector4f Position;
            public Vector4f Rotation;
            public Vector4f Scale;

            public int ClassId;
            public int MeshId;
            public int TextureId;
        }

        //Properties
        public int ModelCount
        {
            get { return sceneModel.Count; }
        }
        public int ChunkCount
        {
            get { return sceneChunk.Count; }
        }

        //Members
        public List<Texture> sceneTexture = new List<Texture>();
        public List<Model> sceneModel = new List<Model>();
        public List<Chunk> sceneChunk = new List<Chunk>();
        public List<Object> sceneObject = new List<Object>();

        public List<Model> scenePieceCSK = new List<Model>();

        public int AddModel(Model mdl)
        {
            sceneModel.Add(mdl);
            return sceneModel.Count - 1;
        }
        public int AddChunk(Chunk chunk)
        {
            sceneChunk.Add(chunk);
            return sceneChunk.Count - 1;
        }
        public int AddTexture(Texture tex)
        {
            sceneTexture.Add(tex);
            return sceneTexture.Count - 1;
        }

        public Model GetModel(int index)
        {
            return sceneModel[index];
        }
        public Chunk GetChunk(int index)
        {
            return sceneChunk[index];
        }
    }
}
