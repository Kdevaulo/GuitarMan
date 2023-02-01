using UnityEngine;

namespace GuitarMan.AudioAnalyzationSystem
{
    [AddComponentMenu(nameof(AudioAnalyzationSystem) + "/" + nameof(AudioAnalyzer))]
    public class AudioAnalyzer : MonoBehaviour
    {
        public float Amplitude => _amplitude;
        public float SmoothAmplitude => _smoothAmplitude;
        public float[] FrequencyValues => _frequencyValues;
        public float[] SmoothValues => _smoothValues;
        public float[] NormalizedFrequencyValues => _normalizedFrequencyValues;
        public float[] NormalizedSmoothValues => _normalizedSmoothValues;

        [SerializeField, Range(0.5f, 0.99f)] private float _smoothnessCoefficient = 0.95f;

        [SerializeField] private Groups _groups;

        [SerializeField] private AudioSource _audioSource;

        private readonly float[] _leftChannelSamples = new float[512];

        private readonly float[] _rightChannelSamples = new float[512];

        private float[] _frequencyValues;

        private float[] _smoothValues;

        private float[] _bufferGroupDecrease;

        private float[] _normalizedSmoothValues;

        private float[] _normalizedFrequencyValues;

        private float[] _highestValues;

        private float _amplitude;

        private float _smoothAmplitude;

        private float _highestAmplitude;

        private void Awake()
        {
            var groupsCount = (int) _groups;

            _frequencyValues = new float[groupsCount];

            _smoothValues = new float[groupsCount];

            _bufferGroupDecrease = new float[groupsCount];

            _normalizedSmoothValues = new float[groupsCount];

            _normalizedFrequencyValues = new float[groupsCount];

            _highestValues = new float[groupsCount];
        }

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

            AudioAnalyzerUtils.CreateFrequencyForGroups(ref _frequencyValues, _leftChannelSamples, _rightChannelSamples,
                _groups);
            AudioAnalyzerUtils.CreateSmoothValuesGroups(ref _smoothValues, ref _bufferGroupDecrease, _frequencyValues,
                _smoothnessCoefficient);
            AudioAnalyzerUtils.CreateNormalizedValuesGroups(ref _highestValues, ref _normalizedFrequencyValues,
                ref _normalizedSmoothValues, _frequencyValues, _smoothValues);
            AudioAnalyzerUtils.CreateAmplitudes(ref _amplitude, ref _smoothAmplitude, ref _highestAmplitude,
                _frequencyValues, _smoothValues);
        }
    }
}