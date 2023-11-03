// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace PampelGames.Shared.Tools
{
    public static class PGSaveSystem
    {
        /// <summary>
        ///     Saves any Serializable data.
        /// </summary>
        /// <param name="data">Serializable data (classes must be marked with [Serializable]). Can also be a list of classes.</param>
        /// <param name="saveFilePrefix">Prefix for the file name.</param>
        /// <param name="slotIndex">Slot index for the file, also included in the name.</param>
        /// <param name="encryptionKey">Encryption key for the file.</param>
        public static void Save<T>(T data, string saveFilePrefix, int slotIndex, string encryptionKey = "PGSaveSystemEncryptionKey") where T : new()
        {
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, data);
            var encryptedData = Encryption.Encrypt(ms.ToArray(), encryptionKey);

            var saveFileName = saveFilePrefix+"_"+ slotIndex + ".dat";
            var file = File.Create(Application.persistentDataPath + "/" + saveFileName);
            file.Write(encryptedData, 0, encryptedData.Length);
            file.Close();
        }


        /// <summary>
        ///     Loads the saved data.
        /// </summary>
        /// <param name="saveFilePrefix">Prefix for the file name.</param>
        /// <param name="slotIndex">Slot index for the file, also included in the name.</param>
        /// <param name="encryptionKey">Encryption key for the file.</param>
        /// <returns>Loaded data.</returns>
        public static T Load<T>(string saveFilePrefix, int slotIndex, string encryptionKey = "PGSaveSystemEncryptionKey") where T : new()
        {
            var saveFileName = saveFilePrefix+"_"+ slotIndex + ".dat";
            var saveFilePath = Application.persistentDataPath + "/" + saveFileName;

            if (File.Exists(saveFilePath))
            {
                var encryptedData = File.ReadAllBytes(saveFilePath);
                var decryptedData = Encryption.Decrypt(encryptedData, encryptionKey);
                var ms = new MemoryStream(decryptedData);

                var bf = new BinaryFormatter();
                var data = (T) bf.Deserialize(ms);
                return data;
            }

            // return a new instance of T if the file doesn't exist
            return new T();
        }
    }
}