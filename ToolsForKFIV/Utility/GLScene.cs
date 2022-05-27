using System;
using System.Collections.Generic;
using System.Text;

using FormatKFIV.Asset;

namespace ToolsForKFIV.Utility
{
    public class GLScene
    {
        public List<GLModel>   scenePieceMdl    = new List<GLModel>();
        public List<GLModel> scenePieceCSK      = new List<GLModel>();

        //Objects
        public List<GLTexture> sceneSObjTexture = new List<GLTexture>();
        public List<GLModel>   sceneSObjModel   = new List<GLModel>();

        public void BuildScene(Scene scene)
        {
            //Build Objects
            foreach (Texture tex in scene.sceneTexture)
            {
                sceneSObjTexture.Add(GLTexture.GenerateFromAsset(tex));
            }

            //Build Scene
            for (int i = 0; i < scene.ModelCount; ++i)
            {
                scenePieceMdl.Add(GLModel.GenerateFromAsset(scene.GetModel(i)));
            }
       
            foreach(Model cskm in scene.scenePieceCSK)
            {
                scenePieceCSK.Add(GLModel.GenerateFromAsset(cskm));
            }
        }

        public void Destroy()
        {
            //Destroy Static Object
            foreach(GLTexture tex in sceneSObjTexture)
            {
                tex.Destroy();
            }
            sceneSObjTexture.Clear();
            sceneSObjTexture = null;
            foreach(GLModel mdl in sceneSObjModel)
            {
                mdl.Destroy();
            }
            sceneSObjModel.Clear();
            sceneSObjModel = null;

            //Destroy Map Piece
            foreach(GLModel mdl in scenePieceMdl)
            {
                mdl.Destroy();
            }
            scenePieceMdl.Clear();
            scenePieceMdl = null;
            foreach (GLModel mdl in scenePieceCSK)
            {
                mdl.Destroy();
            }
            scenePieceCSK.Clear();
            scenePieceCSK = null;
        }
    }
}
