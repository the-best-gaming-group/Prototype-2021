using System.IO;
using System;
using System.Text;
using Platformer.Mechanics;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Platformer.Core
{
    public static class SaveFileManager
    {
        public static void DeleteSaveFile(string fp)
        {
            try
            {
                if (File.Exists(fp))
                {
                    File.Delete(fp);
                }
            }
            catch (Exception)
            {
                Debug.LogError("Failed to delete save file: " + fp);
            }
        }
        public static bool WriteToSaveFile(string fp, Checkpoint checkpoint)
        {
            var json = JsonUtility.ToJson(checkpoint);
            try
            {
                DeleteSaveFile(fp);
                using (FileStream fs = File.OpenWrite(fp))
                {
                    byte[] jsonBytes = new UTF8Encoding(true).GetBytes(json);
                    fs.Write(jsonBytes, 0, jsonBytes.Length);
                    fs.Close();
                    jsonBytes = null;
                }
                return true;
            }
            catch (Exception)
            {
                Debug.LogError("Failed to write new save file: " + fp);
                json = null;
                return false;
            }
        }
        public static async Task<Checkpoint> ReadFromSaveFile(string fp)
        {
            byte[] bytes = new byte[int.MaxValue];
            string json;
            try
            {
                Debug.Log("Opening File");
                using (FileStream fs = File.OpenRead(fp))
                {
                    Debug.Log("Reading File");
                    await fs.ReadAsync(bytes);
                    fs.Close();
                }
                Debug.Log("Encoding File");
                json = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                bytes = null;
                Debug.Log("Converting File");
                var checkpoint = JsonUtility.FromJson<Checkpoint>(json);
                json = null;
                Debug.Log("Returning Checkpoint");
                return checkpoint;
            }
            catch (Exception)
            {
                Debug.LogError("Failed to load save file: " + fp);
                bytes = null;
                json = null;
                return null;
            }
            
        }
    }
}