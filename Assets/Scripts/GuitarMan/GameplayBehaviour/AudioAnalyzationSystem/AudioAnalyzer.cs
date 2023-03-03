using UnityEngine;

namespace GuitarMan.GameplayBehaviour.AudioAnalyzationSystem
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

        private readonly float[] _multiChannelSamples = new float[512];

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
            // todo: change to offline method
            _audioSource.GetSpectrumData(_leftChannelSamples, 0, FFTWindow.Hanning);
            _audioSource.GetSpectrumData(_rightChannelSamples, 1, FFTWindow.Hanning);

            int numProcessed = 0;
            float combinedChannelAverage = 0f;
            var numChannels = _audioSource.clip.channels;

            for (int i = 0; i < _leftChannelSamples.Length; i++)
            {
                combinedChannelAverage += _leftChannelSamples[i] + _rightChannelSamples[i];

                // Each time we have processed all channels samples for a point in time, we will store the average of the channels combined
                if ((i + 1) % numChannels == 0)
                {
                    _multiChannelSamples[numProcessed] = combinedChannelAverage / numChannels;
                    numProcessed++;
                    combinedChannelAverage = 0f;
                }
            }

            AudioAnalyzerUtils.CreateFrequencyForGroups(ref _frequencyValues, _multiChannelSamples,
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