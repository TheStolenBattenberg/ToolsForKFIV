using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;

namespace FormatKFIV.FileFormat
{
    //Class name gracefully stolen from IvanDSM.
    public class FFPrettyNames
    {
        private Dictionary<string, string> prettyNamesMD5 = new Dictionary<string, string>();
        private MD5 HasherMD5;

        /// <summary>Initializes PrettyNames</summary>
        public void Initialize()
        {
            HasherMD5 = MD5.Create();
        }

        /// <summary>Loads PrettyNames from File</summary>
        public void LoadPrettyNames(string path)
        {
            Console.WriteLine(path);

            if(!File.Exists(path))
            {
                Console.WriteLine("Couldn't load pretty names from file");
                return;
            }

            using (StreamReader sr = new StreamReader(File.OpenRead(path)))
            {
                while(!sr.EndOfStream)
                {
                    //Get pretty name entry from file.
                    string prettyEntry = sr.ReadLine();

                    //Don't do comments or empty lines
                    if(prettyEntry.StartsWith(';') || prettyEntry.Length < 24)
                        continue;

                    string[] prettySplit = prettyEntry.Split(',', StringSplitOptions.RemoveEmptyEntries);

                    //Make sure split is valid
                    if (prettySplit.Length != 2)
                        continue;

                    //Add pretty name entry
                    if(!prettyNamesMD5.ContainsKey(prettySplit[0]))
                    {
                        prettyNamesMD5.Add(prettySplit[0], prettySplit[1]);
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("PrettyName MD5 Collision!");
                        Console.WriteLine("MD5: " + prettySplit[0]);
                        Console.WriteLine("COL: " + prettySplit[1] + " -> " + prettyNamesMD5[prettySplit[0]]);
                    }
                }
            }
        }

        public bool GetPrettyName(byte[] buffer, out string prettyName)
        {
            //Compute MD5 Hash
            byte[] hashArr = HasherMD5.ComputeHash(buffer);
            string md5Hash = Convert.ToBase64String(hashArr);
            
            //Hash exists in pretty name map?
            if(prettyNamesMD5.ContainsKey(md5Hash))
            {
                //Found hash in pretty name map.
                prettyName = prettyNamesMD5[md5Hash];
                return true;
            }

            //Didn't find a pretty name
            Console.WriteLine("Non Found MD5 = " + md5Hash);
            prettyName = "";
            return false;
        }
    }
}
