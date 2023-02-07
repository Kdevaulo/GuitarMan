using System;
using System.Collections.Generic;
using System.Linq;

using Cysharp.Threading.Tasks;

using GuitarMan.Utils;

using SimpleFileBrowser;

using UnityEngine;
using UnityEngine.Networking;

namespace GuitarMan.FileLoadSystem
{
    public class FileLoadController : IDisposable
    {
        private readonly FileLoadView _fileLoadView;

        private readonly SoundLoadEventsModel _soundLoadEventsModel;

        private List<AudioClip> _loadedSounds = new List<AudioClip>();

        public FileLoadController(FileLoadView fileLoadView, SoundLoadEventsModel soundLoadEventsModel)
        {
            _fileLoadView = fileLoadView;
            _soundLoadEventsModel = soundLoadEventsModel;

            fileLoadView.LoadButtonClicked += HandleLoadButtonClick;
            soundLoadEventsModel.AnalysisFinished += HandleAnalysisFinished;
        }

        void IDisposable.Dispose()
        {
            _fileLoadView.LoadButtonClicked -= HandleLoadButtonClick;
            _soundLoadEventsModel.AnalysisFinished -= HandleAnalysisFinished;
        }

        public void Initialize()
        {
            FileBrowser.SetFilters(true,
                new FileBrowser.Filter(FileLoadConstants.SoundsFilterName, FileLoadConstants.SoundsFilterExtensions));

            FileBrowser.SetDefaultFilter(FileLoadConstants.DefaultFilter);
            FileBrowser.SetExcludedExtensions(FileLoadConstants.ExcludedExtensions);
            FileBrowser.AddQuickLink(FileLoadConstants.ShortcutName, FileLoadConstants.ShortcutPath);
        }

        private void HandleLoadButtonClick()
        {
            FileBrowser.ShowLoadDialog(HandlePathsSelected, HandleDialogCancelled, FileBrowser.PickMode.Files, true);
        }

        private void HandlePathsSelected(string[] paths)
        {
            LoadFilesAsync(paths).Forget();
        }

        private void HandleDialogCancelled()
        {
            _fileLoadView.VisualizeLoadFinished();
        }

        private void HandleAnalysisFinished()
        {
            _fileLoadView.VisualizeLoadFinished();
        }

        private async UniTask LoadFilesAsync(string[] paths)
        {
            _fileLoadView.VisualizeLoadStarted();

            foreach (var path in paths)
            {
                await TryLoadFileAsync(path);
            }

            _soundLoadEventsModel.HandleAudioClipsLoaded(_loadedSounds);
        }

        private async UniTask TryLoadFileAsync(string path)
        {
            var urlPath = UnityWebRequest.EscapeURL(path);

            AudioClip audioClip;

            using (UnityWebRequest unityWebRequest =
                   UnityWebRequestMultimedia.GetAudioClip("file:///" + urlPath, AudioType.UNKNOWN))
            {
                var request = unityWebRequest.SendWebRequest();

                await request;

                audioClip = DownloadHandlerAudioClip.GetContent(unityWebRequest);
                audioClip.name = RegexUtils.GetSoundName(path);
            }

            _loadedSounds.Add(audioClip);

            // todo: needed to load audio data async, 400ms is a large freeze maybe AudioClip.loadInBackground will help
            audioClip.LoadAudioData();

            await UniTask.WaitUntil(() => audioClip.loadState == AudioDataLoadState.Loaded);
        }
    }
}