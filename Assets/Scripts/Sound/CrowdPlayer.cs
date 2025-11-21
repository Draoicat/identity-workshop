using System;
using System.Linq;
using Core;
using UnityEngine;
using Event = Core.Event;

namespace Sound
{
    public class CrowdPlayer : MonoBehaviour
    {
        [SerializeField] private Parties party;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            Results.Instance.OnResultsGot += PlaySound;
            switch (party)
            {
                case Parties.Optimism:
                    _audioSource.pitch = 2.5f;
                    break;
                case Parties.Shyness:
                    _audioSource.pitch = 1.5f;
                    break;
                case Parties.AI:
                    _audioSource.pitch = 0.5f;
                    break;
                case Parties.Selfless:
                    _audioSource.pitch = -0.5f;
                    break;
                case Parties.Anger:
                    _audioSource.pitch = -1.5f;
                    break;
            }
        }

        private void PlaySound(Event gameEvent)
        {
            if (Results.Instance.ChosenSolutionForCurrentStep.supportingParties.Contains(party))
                _audioSource.Play();
        }
    }
}