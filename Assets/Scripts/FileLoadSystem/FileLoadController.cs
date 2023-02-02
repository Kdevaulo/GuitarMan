using System;
using System.IO;

using SimpleFileBrowser;

using UnityEngine;

namespace GuitarMan.FileLoadSystem
{
    public class FileLoadController : IDisposable
    {
        private readonly FileLoadView _fileLoadView;

        public FileLoadController(FileLoadView fileLoadView)
        {
            _fileLoadView = fileLoadView;
            fileLoadView.LoadButtonClicked += HandleLoadButtonClick;
        }

        void IDisposable.Dispose()
        {
            _fileLoadView.LoadButtonClicked -= HandleLoadButtonClick;
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
            //FileBrowser.ShowLoadDialog(HandlePathsSelected, HandleDialogCancelled, FileBrowser.PickMode.Files, true);

            TryLoadFile();
        }

        private void HandlePathsSelected(string[] paths)
        {
            _fileLoadView.VisualizeLoadFinished();
        }

        private void HandleDialogCancelled()
        {
            _fileLoadView.VisualizeLoadStarted();
        }

        private void TryLoadFile()
        {
            var audioSource = _fileLoadView.AudioSource;

            Debug.Log(audioSource.clip.loadState);

            audioSource.clip.LoadAudioData();

            Debug.Log(audioSource.clip.loadState);

            float[] samples = new float[audioSource.clip.samples * audioSource.clip.channels];
            audioSource.clip.GetData(samples, 0);

            Debug.Log(audioSource.clip.frequency);
            Debug.Log(audioSource.clip.length);

            var t = audioSource.clip.frequency * audioSource.clip.length;
            Debug.Log($"length equals == {samples.Length == t}, t = {t}");
        }
    }
}