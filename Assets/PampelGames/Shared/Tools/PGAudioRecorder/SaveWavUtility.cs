// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.IO;
using UnityEngine;

// AudioClip extension to save to WAV format.
namespace PampelGames.Shared.Tools
{
    public static class SaveWavUtility
    {
        const int HEADER_SIZE = 44;

        public static bool Save(AudioClip clip, string filepath)
        {
            if (!filepath.ToLower().EndsWith(".wav"))
            {
                filepath += ".wav";
            }

            var filepathDirectory = Path.GetDirectoryName(filepath);
            if (!Directory.Exists(filepathDirectory))
            {
                Directory.CreateDirectory(filepathDirectory);
            }

            using (var fileStream = new FileStream(filepath, FileMode.Create))
            {
                byte[] data = AudioClipToByteArray(clip);
                WriteWavHeader(fileStream, clip);
                fileStream.Write(data, 0, data.Length);
            }

            return true; 
        }

        static byte[] AudioClipToByteArray(AudioClip clip)
        {
            var samples = new float[clip.samples];

            clip.GetData(samples, 0);

            var intData = new byte[samples.Length * 2];
            var rescaleFactor = 32767; // to convert float to Int16

            for (int i = 0; i < samples.Length; i++)
            {
                var shortValue = (short) (samples[i] * rescaleFactor);
                var bytes = System.BitConverter.GetBytes(shortValue);

                intData[i * 2] = bytes[0];
                intData[i * 2 + 1] = bytes[1];
            }

            return intData;
        }

        static void WriteWavHeader(FileStream stream, AudioClip clip)
        {
            var hz = clip.frequency;
            var channels = clip.channels;
            var samples = clip.samples;

            stream.Seek(0, SeekOrigin.Begin);

            // Byte 0 to Byte 3: "RIFF"
            byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
            stream.Write(riff, 0, 4);

            // Byte 4 to Byte 7: Length of rest of file
            uint fileLength = (uint) (HEADER_SIZE + samples * 2);
            stream.Write(System.BitConverter.GetBytes(fileLength), 0, 4);

            // Byte 8 to Byte 11: "WAVE"
            byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
            stream.Write(wave, 0, 4);

            // Byte 12 to Byte 15: "fmt "
            byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
            stream.Write(fmt, 0, 4);

            // Byte 16 to Byte 19: Length of first subchunk
            uint subchunk1 = 16;
            stream.Write(System.BitConverter.GetBytes(subchunk1), 0, 4);

            // Byte 20 to Byte 21: Audio Format
            ushort audioFormat = 1;
            stream.Write(System.BitConverter.GetBytes(audioFormat), 0, 2);

            // Byte 22 to Byte 23: Number of channels
            stream.Write(System.BitConverter.GetBytes((ushort)channels), 0, 2);

            // Byte 24 to Byte 27: Sample rate
            stream.Write(System.BitConverter.GetBytes(hz), 0, 4);

            // Byte 28 to Byte 31: Byte rate
            uint byteRate = (uint) (hz * channels * 2);
            stream.Write(System.BitConverter.GetBytes(byteRate), 0, 4);

            // Byte 32 to Byte 33: Sample alignment
            ushort blockAlign = (ushort) (channels * 2);
            stream.Write(System.BitConverter.GetBytes(blockAlign), 0, 2);

            // Byte 34 to Byte 35: Bits per sample
            ushort bitsPerSample = 16;
            stream.Write(System.BitConverter.GetBytes(bitsPerSample), 0, 2);

            // Byte 36 to Byte 39: "data"
            byte[] dataString = System.Text.Encoding.UTF8.GetBytes("data");
            stream.Write(dataString, 0, 4);

            // Byte 40 to Byte 43: Length of audio data
            uint dataLength = (uint) (samples * channels * 2);
            stream.Write(System.BitConverter.GetBytes(dataLength), 0, 4);
        }
    }
}