using UnityEngine;

namespace GuitarMan.AudioAnalyzationSystem
{
    [AddComponentMenu(nameof(AudioAnalyzationSystem) + "/" + nameof(AudioAnalyzer64Groups))]
    public sealed class AudioAnalyzer64Groups : MonoBehaviour, IAudioAnalyzer
    {
        public float Amplitude => _amplitude;
        public float SmoothAmplitude => _smoothAmplitude;
        public float[] FrequencyValues => _frequencyValues;
        public float[] SmoothValues => _smoothValues;
        public float[] NormalizedFrequencyValues => _normalizedFrequencyValues;
        public float[] NormalizedSmoothValues => _normalizedSmoothValues;

        [SerializeField, Range(0.5f, 0.99f)] private float _smoothnessCoefficient = 0.95f;

        [SerializeField] private AudioSource _audioSource;

        private const int GroupsCount = 64;

        private readonly float[] _leftChannelSamples = new float[512];

        private readonly float[] _rightChannelSamples = new float[512];

        private float[] _frequencyValues = new float[GroupsCount];

        private float[] _smoothValues = new float[GroupsCount];

        private float[] _bufferGroupDecrease = new float[GroupsCount];

        private float[] _normalizedSmoothValues = new float[GroupsCount];

        private float[] _normalizedFrequencyValues = new float[GroupsCount];

        private float[] _highestValues = new float[GroupsCount];

        private float _amplitude;

        private float _smoothAmplitude;

        private float _highestAmplitude;

        private void Start()
        {
            // note: initialize highest values array with 1f for fixing start max values bug
            for (var i = 0; i < _highestValues.Length; i++)
            {
                _highestValues[i] = 1f;
            }
        }

        private void FixedUpdate()
        {
            _audioSource.GetSpectrumData(_leftChannelSamples, 0, FFTWindow.Blackman);
            _audioSource.GetSpectrumData(_rightChannelSamples, 1, FFTWindow.Blackman);

            AudioAnalyzer.CreateFrequencyFor64Groups(ref _frequencyValues, _leftChannelSamples, _rightChannelSamples);
            AudioAnalyzer.CreateSmoothValuesGroups(ref _smoothValues, ref _bufferGroupDecrease, _frequencyValues,
                _smoothnessCoefficient);
            AudioAnalyzer.CreateNormalizedValuesGroups(ref _highestValues, ref _normalizedFrequencyValues,
                ref _normalizedSmoothValues, _frequencyValues, _smoothValues);
            AudioAnalyzer.CreateAmplitudes(ref _amplitude, ref _smoothAmplitude, ref _highestAmplitude,
                _frequencyValues, _smoothValues);
        }
    }
}