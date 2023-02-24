using System;
using System.Collections.Generic;

using GuitarMan.AudioPlayerSystem;

using UnityEngine;

using Object = UnityEngine.Object;

namespace GuitarMan.SoundsContainerBehaviour
{
    public class SoundsContainerController : IDisposable
    {
        private readonly SoundLoadEventsModel _soundLoadEventsModel;

        private readonly AudioPlayer _menuAudioPlayer;

        private readonly Transform _loadedFilesContainer;

        private readonly SoundView _soundViewPrefab;

        private readonly AudioSource _audioSource;

        private readonly Dictionary<Action, SoundView> _soundPlayActionViews = new Dictionary<Action, SoundView>();

        private readonly Dictionary<SoundView, AudioClip> _audioClipsByViews = new Dictionary<SoundView, AudioClip>();

        public SoundsContainerController(SoundContainerView soundContainerView,
            SoundLoadEventsModel soundLoadEventsModel, AudioPlayer menuAudioPlayer)
        {
            _loadedFilesContainer = soundContainerView.LoadedFilesContainer;
            _soundViewPrefab = soundContainerView.SoundViewPrefab;
            _soundLoadEventsModel = soundLoadEventsModel;
            _menuAudioPlayer = menuAudioPlayer;

            soundLoadEventsModel.AnalysisFinished += HandleAnalysisFinished;
        }

        void IDisposable.Dispose()
        {
            _soundLoadEventsModel.AnalysisFinished -= HandleAnalysisFinished;

            foreach (var pair in _soundPlayActionViews)
            {
                pair.Value.SoundPlayClicked -= pair.Key;
            }

            _soundPlayActionViews.Clear();
        }

        private void HandleAnalysisFinished()
        {
            var clips = _soundLoadEventsModel.GetLoadedClips();

            foreach (var clip in clips)
            {
                var soundView = CreateView(clip);

                soundView.Initialize();

                Action currentAction = () => HandleSoundPlayClicked(soundView);

                _soundPlayActionViews.Add(currentAction, soundView);
                _audioClipsByViews.Add(soundView, clip);

                soundView.SoundPlayClicked += currentAction;
            }

            _soundLoadEventsModel.HandleSoundsListUpdated();
        }

        private SoundView CreateView(AudioClip audioClip)
        {
            // todo: add dynamic scroll area update while adding new item
            var songView = Object.Instantiate(_soundViewPrefab, _loadedFilesContainer);
            songView.SetText(audioClip.name);

            return songView;
        }

        private void HandleSoundPlayClicked(SoundView view)
        {
            _menuAudioPlayer.PlayClip(_audioClipsByViews[view]);
        }
    }
}