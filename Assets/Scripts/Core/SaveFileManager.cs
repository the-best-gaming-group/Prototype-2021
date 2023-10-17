using System.IO;
using System;
using System.Text;
using Platformer.Mechanics;
using UnityEngine;
using System.Threading.Tasks;

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
        public static async Task<Checkpoint> ReadFromSaveFile(string fp)
        {
            try
            {
                Debug.Log("Opening File");
                using FileStream fs = File.OpenRead(fp);
                byte[] bytes = new byte[int.MaxValue];
                Debug.Log("Reading File");
                await fs.ReadAsync(bytes);
                Debug.Log("Encoding File");
                var json = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                fs.Close();
                Debug.Log("Converting File");
                var checkpoint = JsonUtility.FromJson<Checkpoint>(json);
                Debug.Log("Returning Checkpoint");
                return checkpoint;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
    }
}