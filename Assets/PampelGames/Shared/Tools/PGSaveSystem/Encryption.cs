// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Text;

namespace PampelGames.Shared.Tools
{
    internal static class Encryption
    {
        
        public static byte[] Encrypt(byte[] data, string encryptionKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
            var encryptedData = new byte[data.Length];

            for (var i = 0; i < data.Length; i++) encryptedData[i] = (byte) (data[i] ^ keyBytes[i % keyBytes.Length]);

            return encryptedData;
        }

        public static byte[] Decrypt(byte[] data, string encryptionKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
            var decryptedData = new byte[data.Length];

            for (var i = 0; i < data.Length; i++) decryptedData[i] = (byte) (data[i] ^ keyBytes[i % keyBytes.Length]);

            return decryptedData;
        }
    
    }
}
