// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;

namespace PampelGames.Shared.Tools
{
    public class MicrophoneRecorderToWav : MonoBehaviour
    {
        [Tooltip("Records as long as the button is held down.")]
        public KeyCode record = KeyCode.Space;

        public int maxRecordTime = 15;
        
        private AudioClip _audioClip;
        private bool _isRecording;
        private const int SampleRate = 16000; // Audio sample rate.
        private const string SavePath = "C:/Users/Jari/Desktop/my_audio.wav";

        void Start()
        {
            foreach (var device in Microphone.devices)
            {
                Debug.Log("Name: " + device);
            }
        }
    
        private void Update()
        {
            if (Input.GetKeyDown(record))
            {
                StartRecording();
            }

            if (Input.GetKeyUp(record))
            {
                StopRecording();
                SaveRecordingToWavFile();
            }
        }

        private void StartRecording()
        {
            _audioClip = Microphone.Start(null, true, maxRecordTime, SampleRate);
            _isRecording = true;
        }

        private void StopRecording()
        {
            if (!_isRecording) return;
            Microphone.End(null);
            _isRecording = false;
        }

        private void SaveRecordingToWavFile()
        {
            if (_audioClip == null) return;
            // Save recorded AudioClip to .wav file.
            SaveWavUtility.Save(_audioClip, SavePath);
        }
    }
}