using System;
using Core;
using UnityEngine;
using Event = Core.Event;

namespace Sound
{
    public class CountdownSoundEffect : MonoBehaviour
    {
        private AudioSource _audioSource;
        
        public AudioClip countdownSoundEffect;
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _audioSource.loop = true;
            EventManager.Instance.OnVoteStarted += StartSound;
            EventManager.Instance.OnVoteEnded += StopSound;
        }

        private void StartSound(Event _)
        {
            _audioSource.Play();
        }

        private void StopSound(Event _)
        {
            _audioSource.Stop();
        }

        private void OnDestroy()
        {
            EventManager.Instance.OnVoteStarted -= StartSound;
            EventManager.Instance.OnVoteEnded -= StopSound;
        }
    }
}
