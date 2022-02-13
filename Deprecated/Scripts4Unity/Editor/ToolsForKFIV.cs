using UnityEngine;
using UnityEditor;

using System.IO;

public class ToolsForKFIV
{
    [MenuItem("ToolsForKFIV/Import Layout")]
    public static void ImportKFIVMapLayout()
    {
        string pathToLayout = EditorUtility.OpenFilePanel("Open Layout...", "", "");
        string meshAssets = EditorUtility.OpenFolderPanel("Mesh Folder (In Project Assets)", "Assets/", "");

        if (pathToLayout.Length == 0 || meshAssets.Length == 0)
            return;

        //Hacky way to make mesh assets relative to project dir
        int ind = meshAssets.IndexOf("Assets");
        meshAssets = meshAssets.Substring(ind, meshAssets.Length - ind);

        GameObject parent = new GameObject(Path.GetFileName(meshAssets));

        using (BinaryReader binr = new BinaryReader(File.Open(pathToLayout, FileMode.Open)))
        {
            //Keep reading until we reach the end...
            while(binr.BaseStream.Position < binr.BaseStream.Length)
            {
                // Position
                float PX = binr.ReadSingle();
                float PY = binr.ReadSingle();
                float PZ = binr.ReadSingle();
                binr.ReadSingle();  //Padding

                // Rotation
                float RX = binr.ReadSingle();
                float RY = binr.ReadSingle();
                float RZ = binr.ReadSingle();
                binr.ReadSingle();  //Padding

                // Scale
                float SX = binr.ReadSingle();
                float SY = binr.ReadSingle();
                float SZ = binr.ReadSingle();
                binr.ReadSingle();  //Padding

                //Mesh Info
                ushort RenderMesh    = binr.ReadUInt16();
                ushort CollisionMesh = binr.ReadUInt16();

                //Read ALL remaining, usless (for this purpose) bytes
                binr.ReadBytes(76);

                if (RenderMesh == 0xFFFF)   //Skip Invalid Rendermeshes
                    continue;

                
                //Instansiate a new mesh instance
                string assetPath = meshAssets + "/mesh_"+ RenderMesh.ToString("D4") + ".obj";
                Object CurrentMeshPrefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));
                
                if (CurrentMeshPrefab != null)
                {
                    GameObject pref = PrefabUtility.InstantiatePrefab(CurrentMeshPrefab) as GameObject;
                        pref.transform.position    = new Vector3(PX / 256f, PY / 256f, PZ / 256f);
                        pref.transform.eulerAngles = new Vector3(RX, RY, RZ);
                        pref.transform.localScale  = new Vector3(SX, SY, SZ);

                    pref.transform.parent = parent.transform;
                }
            }
        }
    }
}
