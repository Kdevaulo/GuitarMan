using System;
using System.Collections.Generic;
using System.Numerics;

using Cysharp.Threading.Tasks;

using DSPLib;

using SimpleFileBrowser;

using UnityEngine;
using UnityEngine.Networking;

namespace GuitarMan.FileLoadSystem
{
    public class FileLoadController : IDisposable
    {
        private readonly FileLoadView _fileLoadView;

        private AudioClip _loadedSound;

        private List<SoundSpectrumData> _soundDataCollection = new List<SoundSpectrumData>();

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

        private List<SpectrumData> AnalyzeSound(AudioClip audioClip)
        {
            audioClip.LoadAudioData();

            var numChannels = audioClip.channels;
            var numTotalSamples = audioClip.samples;

            float[] samples = new float[numTotalSamples * numChannels];
            audioClip.GetData(samples, 0);

            return GetSpectrumDataCollection(ref samples, numChannels, numTotalSamples, audioClip.frequency);
        }

        private async UniTask AnalyzeFilesAsync(string[] paths)
        {
            _fileLoadView.VisualizeLoadStarted();

            foreach (var path in paths)
            {
                await TryLoadFileAsync(path);

                _soundDataCollection.Add(new SoundSpectrumData(_loadedSound, AnalyzeSound(_loadedSound)));
            }

            _fileLoadView.VisualizeLoadFinished();
        }

        private async UniTask TryLoadFileAsync(string path)
        {
            path = UnityWebRequest.EscapeURL(path);

            using (UnityWebRequest unityWebRequest =
                   UnityWebRequestMultimedia.GetAudioClip("file:///" + path, AudioType.UNKNOWN))
            {
                var request = unityWebRequest.SendWebRequest();

                await request;

                _loadedSound = DownloadHandlerAudioClip.GetContent(unityWebRequest);
            }
        }

        private List<SpectrumData> GetSpectrumDataCollection(ref float[] samples, int numChannels, int numTotalSamples,
            int sampleRate)
        {
            try
            {
                List<SpectrumData> data = new List<SpectrumData>();

                // We only need to retain the samples for combined channels over the time domain
                float[] preProcessedSamples = new float[numTotalSamples];

                int numProcessed = 0;
                float combinedChannelAverage = 0f;
                for (int i = 0; i < samples.Length; i++)
                {
                    combinedChannelAverage += samples[i];

                    // Each time we have processed all channels samples for a point in time, we will store the average of the channels combined
                    if ((i + 1) % numChannels == 0)
                    {
                        preProcessedSamples[numProcessed] = combinedChannelAverage / numChannels;
                        numProcessed++;
                        combinedChannelAverage = 0f;
                    }
                }

                // Once we have our audio sample data prepared, we can execute an FFT to return the spectrum data over the time domain
                const int spectrumSampleSize = 1024;
                int iterations = preProcessedSamples.Length / spectrumSampleSize;

                FFT fft = new FFT();
                fft.Initialize(spectrumSampleSize);

                double[] sampleChunk = new double[spectrumSampleSize];

                for (int i = 0; i < iterations; i++)
                {
                    // Grab the current 1024 chunk of audio sample data
                    Array.Copy(preProcessedSamples, i * spectrumSampleSize, sampleChunk, 0, spectrumSampleSize);

                    // Apply our chosen FFT Window
                    double[] windowCoefs = DSP.Window.Coefficients(DSP.Window.Type.Hanning, spectrumSampleSize);
                    double[] scaledSpectrumChunk = DSP.Math.Multiply(sampleChunk, windowCoefs);
                    double scaleFactor = DSP.Window.ScaleFactor.Signal(windowCoefs);

                    // Perform the FFT and convert output (complex numbers) to Magnitude
                    Complex[] fftSpectrum = fft.Execute(scaledSpectrumChunk);
                    double[] scaledFFTSpectrum = DSP.ConvertComplex.ToMagnitude(fftSpectrum);
                    scaledFFTSpectrum = DSP.Math.Multiply(scaledFFTSpectrum, scaleFactor);

                    // These 1024 magnitude values correspond (roughly) to a single point in the audio timeline
                    float curSongTime = GetTimeFromIndex(i, sampleRate) * spectrumSampleSize;

                    data.Add(new SpectrumData(Array.ConvertAll(scaledFFTSpectrum, x => (float) x), curSongTime));
                }

                return data;
            }
            catch (Exception e)
            {
                // Catch exceptions here since the background thread won't always surface the exception to the main thread
                Debug.Log(e.ToString());
                return null;
            }
        }

        private float GetTimeFromIndex(int index, int sampleRate)
        {
            return 1f / sampleRate * index;
        }
    }
}