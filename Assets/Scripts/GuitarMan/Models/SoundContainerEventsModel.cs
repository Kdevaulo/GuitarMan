using System;
using System.Collections.Generic;

using GuitarMan.SoundMenuBehaviour.SoundProcessingSystem;

using UnityEngine;

namespace GuitarMan.Models
{
    public class SoundContainerEventsModel
    {
        public event Action SoundsListUpdated = delegate { };
        public event Action AnalysisFinished = delegate { };
        public event Action AudioClipsLoaded = delegate { };
        public event Action StartGameCalled = delegate { };

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

        public void HandleStartGame()
        {
            StartGameCalled.Invoke();
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