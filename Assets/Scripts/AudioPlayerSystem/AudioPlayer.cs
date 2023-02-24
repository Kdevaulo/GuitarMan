using UnityEngine;

namespace GuitarMan.AudioPlayerSystem
{
    public class AudioPlayer
    {
        private readonly AudioSource _audioSource;

        public AudioPlayer(AudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        public void PlayClip(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }
}