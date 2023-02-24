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

        private readonly Dictionary<SoundView, SubscriptionContainer> _subscriptionsByViews =
            new Dictionary<SoundView, SubscriptionContainer>();

        private readonly Dictionary<SoundView, AudioClip> _audioClipsByViews = new Dictionary<SoundView, AudioClip>();

        public SoundsContainerController(SoundContainerView soundContainerView,
            SoundLoadEventsModel soundLoadEventsModel, AudioPlayer menuAudioPlayer)
        {
            _loadedFilesContainer = soundContainerView.LoadedFilesContainer;
            _soundViewPrefab = soundContainerView.SoundViewPrefab;
            _soundLoadEventsModel = soundLoadEventsModel;
            _menuAudioPlayer = menuAudioPlayer;

            soundLoadEventsModel.AnalysisFinished += UpdateSoundsList;
        }

        void IDisposable.Dispose()
        {
            _soundLoadEventsModel.AnalysisFinished -= UpdateSoundsList;

            foreach (var pair in _subscriptionsByViews)
            {
                pair.Key.SoundPlayClicked -= pair.Value.PlayClickedSubscription;
                pair.Key.RemoveSoundClicked -= pair.Value.RemoveClickedSubscription;
            }

            _subscriptionsByViews.Clear();
        }

        private void UpdateSoundsList()
        {
            var clips = _soundLoadEventsModel.GetLoadedClips();

            foreach (var clip in clips)
            {
                var soundView = CreateView(clip);

                soundView.Initialize();

                SubscribeView(soundView);

                _audioClipsByViews.Add(soundView, clip);
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

        private void SubscribeView(SoundView view)
        {
            var subscriptionContainer = new SubscriptionContainer()
            {
                PlayClickedSubscription = () => HandleSoundPlayClicked(view),
                RemoveClickedSubscription = () => HandleSoundRemoveClicked(view)
            };

            _subscriptionsByViews.Add(view, subscriptionContainer);

            view.SoundPlayClicked += subscriptionContainer.PlayClickedSubscription;
            view.RemoveSoundClicked += subscriptionContainer.RemoveClickedSubscription;
        }

        private void DestroyView(SoundView view)
        {
            var target = view.gameObject;

            _subscriptionsByViews.Remove(view);
            _audioClipsByViews.Remove(view);

            target.SetActive(false);
            Object.Destroy(target);
        }

        private void HandleSoundPlayClicked(SoundView view)
        {
            _menuAudioPlayer.PlayClip(_audioClipsByViews[view]);
        }

        private void HandleSoundRemoveClicked(SoundView view)
        {
            DestroyView(view);
        }
    }
}