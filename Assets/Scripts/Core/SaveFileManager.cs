using System.IO;
using System;
using System.Text;
using Platformer.Mechanics;
using UnityEngine;

namespace Platformer.Core
{
    public static class SaveFileManager
    {
        public static bool WriteToSaveFile(string fp, Checkpoint checkpoint)
        {
            var json = JsonUtility.ToJson(checkpoint);
            try
            {
                if (File.Exists(fp))
                {
                    File.Delete(fp);
                }
                using (FileStream fs = File.OpenWrite(fp))
                {
                    byte[] jsonBytes = new UTF8Encoding(true).GetBytes(json);
                    fs.Write(jsonBytes, 0, jsonBytes.Length);
                    fs.Close();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public static bool ReadFromSaveFile(string fp, out Checkpoint checkpoint)
        {
            try
            {
                using FileStream fs = File.OpenRead(fp);
                byte[] bytes = new byte[int.MaxValue];
                fs.Read(bytes);
                var json = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                fs.Close();
                checkpoint = JsonUtility.FromJson<Checkpoint>(json);
                return true;
            }
            catch (Exception)
            {
                checkpoint = null;
                return false;
            }
            
        }
    }
}