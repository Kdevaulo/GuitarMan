using System;
using System.Collections.Generic;

using UnityEngine;

namespace GuitarMan
{
    public class SoundLoadEventsModel
    {
        public event Action AnalysisFinished = delegate { };
        public event Action AudioClipsLoaded = delegate { };

        private List<AudioClip> _loadedClips;

        public void HandleAnalysisFinished()
        {
            AnalysisFinished.Invoke();
        }

        public void HandleAudioClipsLoaded(List<AudioClip> audioClips)
        {
            _loadedClips = audioClips;

            AudioClipsLoaded.Invoke();
        }

        public List<AudioClip> GetLoadedClips()
        {
            return _loadedClips;
        }
    }
}