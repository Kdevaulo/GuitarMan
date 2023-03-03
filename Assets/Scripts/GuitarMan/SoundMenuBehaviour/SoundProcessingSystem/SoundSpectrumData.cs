using System.Collections.Generic;

using UnityEngine;

namespace GuitarMan.SoundMenuBehaviour.SoundProcessingSystem
{
    public class SoundSpectrumData
    {
        private readonly AudioClip _audioClip;

        private readonly List<SpectrumData> _spectrumDataCollection;

        public SoundSpectrumData(AudioClip audioClip, List<SpectrumData> spectrumDataCollection)
        {
            _audioClip = audioClip;
            _spectrumDataCollection = spectrumDataCollection;
        }
    }
}