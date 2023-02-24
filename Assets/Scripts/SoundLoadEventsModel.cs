using System;
using System.Collections.Generic;

using GuitarMan.SoundProcessingSystem;

using UnityEngine;

namespace GuitarMan
{
    public class SoundLoadEventsModel
    {
        public event Action SoundsListUpdated = delegate { };
        public event Action AnalysisFinished = delegate { };
        public event Action AudioClipsLoaded = delegate { };

        private List<AudioClip> _loadedClips;

        private List<SoundSpectrumData> _spectrumDataCollection;

        public void HandleAnalysisFinished(List<SoundSpectrumData> spectrumDataCollection)
        {
            _spectrumDataCollection = spectrumDataCollection;

            AnalysisFinished.Invoke();
        }

        public void HandleAudioClipsLoaded(List<AudioClip> audioClips)
        {
            _loadedClips = audioClips;

            AudioClipsLoaded.Invoke();
        }

        public void HandleSoundsListUpdated()
        {
            SoundsListUpdated.Invoke();
        }

        public List<AudioClip> GetLoadedClips()
        {
            return _loadedClips;
        }

        public List<SoundSpectrumData> GetSpectrumData()
        {
            return _spectrumDataCollection;
        }
    }
}