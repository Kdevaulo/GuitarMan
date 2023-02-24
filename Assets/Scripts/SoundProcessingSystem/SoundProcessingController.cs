using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

using Cysharp.Threading.Tasks;

using DSPLib;

using UnityEngine;

namespace GuitarMan.SoundProcessingSystem
{
    public class SoundProcessingController : IDisposable
    {
        private readonly SoundLoadEventsModel _soundLoadEventsModel;

        private List<SoundSpectrumData> _soundSpectrumDataCollection = new List<SoundSpectrumData>();

        public SoundProcessingController(SoundLoadEventsModel soundLoadEventsModel)
        {
            _soundLoadEventsModel = soundLoadEventsModel;

            soundLoadEventsModel.AudioClipsLoaded += HandleClipsLoaded;
        }

        void IDisposable.Dispose()
        {
            _soundLoadEventsModel.AudioClipsLoaded -= HandleClipsLoaded;
        }

        private void HandleClipsLoaded()
        {
            var audioClips = _soundLoadEventsModel.GetLoadedClips();
            var target = new List<AudioClip>(audioClips);
            AnalyzeSoundsAsync(target).Forget();
        }

        private async UniTask AnalyzeSoundsAsync(List<AudioClip> audioClips)
        {
            foreach (var clip in audioClips)
            {
                var numChannels = clip.channels;
                var numTotalSamples = clip.samples;
                var frequency = clip.frequency;

                var samples = new float[numTotalSamples * numChannels];
                clip.GetData(samples, 0);

                var thread = new Thread(() => AddSpectrumData(samples, numChannels, numTotalSamples, frequency, clip));

                thread.Start();

                await UniTask.WaitUntil(() => thread.ThreadState == ThreadState.Stopped);
            }

            _soundLoadEventsModel.HandleAnalysisFinished(_soundSpectrumDataCollection);
        }

        private void AddSpectrumData(in float[] samples, int numChannels, int numTotalSamples, int frequency,
            AudioClip clip)
        {
            _soundSpectrumDataCollection.Add(new SoundSpectrumData(clip,
                GetSpectrumDataCollection(samples, numChannels, numTotalSamples, frequency)));
        }

        private List<SpectrumData> GetSpectrumDataCollection(in float[] samples, int numChannels, int numTotalSamples,
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
    }
}