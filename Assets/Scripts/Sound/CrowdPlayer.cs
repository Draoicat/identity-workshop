using Core;
using UnityEngine;
using Event = Core.Event;

namespace Sound
{
    public class CrowdPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            Results.Instance.OnResultsGot += PlaySound;
        }

        private void PlaySound(Event gameEvent)
        {
            
        }
    }
}