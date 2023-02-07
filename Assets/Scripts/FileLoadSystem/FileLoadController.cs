using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

using Cysharp.Threading.Tasks;

using DSPLib;

using GuitarMan.Utils;

using SimpleFileBrowser;

using TMPro;

using UnityEngine;
using UnityEngine.Networking;

namespace GuitarMan.FileLoadSystem
{
    public class FileLoadController : IDisposable
    {
        private readonly FileLoadView _fileLoadView;

        private AudioClip _loadedSound;

        private List<SoundSpectrumData> _soundDataCollection = new List<SoundSpectrumData>();

        private List<SpectrumData> _spectrumDataCollection;

        private Transform _loadedFilesContainer;

        private TextMeshProUGUI _songViewPrefab;

        public FileLoadController(FileLoadView fileLoadView)
        {
            _fileLoadView = fileLoadView;
            _loadedFilesContainer = _fileLoadView.GetLoadedFilesContainer();
            _songViewPrefab = _fileLoadView.GetSongViewPrefab();

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
            FileBrowser.ShowLoadDialog(HandlePathsSelected, HandleDialogCancelled, FileBrowser.PickMode.Files, true);
        }

        private void HandlePathsSelected(string[] paths)
        {
            AnalyzeFilesAsync(paths).Forget();
        }

        private void HandleDialogCancelled()
        {
            _fileLoadView.VisualizeLoadFinished();
        }

        private async UniTask AnalyzeFilesAsync(string[] paths)
        {
            _fileLoadView.VisualizeLoadStarted();

            foreach (var path in paths)
            {
                await TryLoadFileAsync(path);

                await AnalyzeSoundAsync();

                _soundDataCollection.Add(new SoundSpectrumData(_loadedSound, _spectrumDataCollection));

                CreateSongView(_loadedSound.name);
            }

            _fileLoadView.VisualizeLoadFinished();
        }

        private async UniTask AnalyzeSoundAsync()
        {
            var numChannels = _loadedSound.channels;
            var numTotalSamples = _loadedSound.samples;
            var frequency = _loadedSound.frequency;

            var samples = new float[numTotalSamples * numChannels];
            _loadedSound.GetData(samples, 0);

            var thread = new Thread(() => SetSpectrumData(ref samples, numChannels, numTotalSamples, frequency));

            thread.Start();

            await UniTask.WaitUntil(() => thread.ThreadState == ThreadState.Stopped);
        }

        private void SetSpectrumData(ref float[] samples, int numChannels, int numTotalSamples, int frequency)
        {
            _spectrumDataCollection =
                GetSpectrumDataCollection(ref samples, numChannels, numTotalSamples, frequency);
        }

        private async UniTask TryLoadFileAsync(string path)
        {
            var urlPath = UnityWebRequest.EscapeURL(path);

            using (UnityWebRequest unityWebRequest =
                   UnityWebRequestMultimedia.GetAudioClip("file:///" + urlPath, AudioType.UNKNOWN))
            {
                var request = unityWebRequest.SendWebRequest();

                await request;

                _loadedSound = DownloadHandlerAudioClip.GetContent(unityWebRequest);
                _loadedSound.name = RegexUtils.GetSoundName(path);
            }

            // todo: needed to load audio data async, 400ms is a large freeze maybe AudioClip.loadInBackground will help
            _loadedSound.LoadAudioData();

            await UniTask.WaitUntil(() => _loadedSound.loadState == AudioDataLoadState.Loaded);
        }

        private List<SpectrumData> GetSpectrumDataCollection(ref float[] samples, int numChannels, int numTotalSamples,
            int sampleRate)
        {
            try
            {
                float[] preProcessedSamples = new float[numTotalSamples];

                int numProcessed = 0;
                float combinedChannelAverage = 0f;
                for (int i = 0; i < samples.Length; i++)
                {
                    combinedChannelAverage += samples[i];

                    if ((i + 1) % numChannels == 0)
                    {
                        preProcessedSamples[numProcessed] = combinedChannelAverage / numChannels;
                        numProcessed++;
                        combinedChannelAverage = 0f;
                    }
                }

                const int spectrumSampleSize = 1024;
                int iterations = preProcessedSamples.Length / spectrumSampleSize;

                List<SpectrumData> data = new List<SpectrumData>(iterations);

                FFT fft = new FFT();
                fft.Initialize(spectrumSampleSize);

                double[] sampleChunk = new double[spectrumSampleSize];

                for (int i = 0; i < iterations; i++)
                {
                    Array.Copy(preProcessedSamples, i * spectrumSampleSize, sampleChunk, 0, spectrumSampleSize);

                    double[] windowCoefs = DSP.Window.Coefficients(DSP.Window.Type.Hanning, spectrumSampleSize);
                    double[] scaledSpectrumChunk = DSP.Math.Multiply(sampleChunk, windowCoefs);
                    double scaleFactor = DSP.Window.ScaleFactor.Signal(windowCoefs);

                    Complex[] fftSpectrum = fft.Execute(scaledSpectrumChunk);
                    double[] scaledFFTSpectrum = DSP.ConvertComplex.ToMagnitude(fftSpectrum);
                    scaledFFTSpectrum = DSP.Math.Multiply(scaledFFTSpectrum, scaleFactor);

                    float curSongTime = GetTimeFromIndex(i, sampleRate) * spectrumSampleSize;

                    data.Add(new SpectrumData(Array.ConvertAll(scaledFFTSpectrum, x => (float) x), curSongTime));
                }

                return data;
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return null;
            }
        }

        private float GetTimeFromIndex(int index, int sampleRate)
        {
            return 1f / sampleRate * index;
        }

        private void CreateSongView(string targetName)
        {
            // todo: add dynamic scroll area update while adding new item
            var songView = UnityEngine.Object.Instantiate(_songViewPrefab, _loadedFilesContainer);
            songView.text = targetName;
        }
    }
}