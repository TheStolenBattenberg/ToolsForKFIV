using System;
using System.IO;

using System.Text.Json;
using System.Text.Json.Serialization;


namespace KFIV.Format.GLTF
{
    public class GLTF
    {
        #region GLTF2 Types
        /**
         * Container for whole GLTF model
        **/
        public class GLTFModel
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public GLTFAsset asset { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public GLTFBuffer[] buffers { get; set; }
        }

        /**
         * Contains what could be described as a GLTF Header
        **/
        public class GLTFAsset
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public string version { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public string generator { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public string copyright { get; set; }
        }

        /**
         * Defines a binary data file
        **/
        public class GLTFBuffer
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public uint byteLength { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public string uri { get; set; }
        }

        /**
         * Defines sections of a buffer
        **/
        public class GLTFBufferView
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public uint buffer { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public uint byteLength { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public uint byteOffset { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public uint byteStride { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public uint target { get; set; }
        }

        /**
         * Defines a point in the scene
        **/
        public class GLTFNode
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public string name { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public uint[] children { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public float[] matrix { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public float[] translation { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public float[] rotation { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public float[] scale { get; set; }
        }
        #endregion

        public void Save(string path)
        {
            //Setup Serializer Options
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };

            //Setup some test data
            GLTFModel model = new GLTFModel
            {
                asset = new GLTFAsset
                {
                    version = "2.0",
                    generator = "ToolsForKFIV",
                    copyright = default
                },
                buffers = new GLTFBuffer[]
                {
                    new GLTFBuffer
                    {
                        byteLength = 0,
                        uri = "fake.bin"
                    },
                    new GLTFBuffer
                    {
                        byteLength = 5,
                        uri = "still_fake.bin"
                    }
                }
            };

            //Finally export (hopefully) good json
            string jsonString = JsonSerializer.Serialize(model, options);


            Console.WriteLine(jsonString);

        }
    }
}