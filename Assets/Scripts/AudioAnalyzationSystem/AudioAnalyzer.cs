using UnityEngine;

namespace GuitarMan.AudioAnalyzationSystem
{
    [RequireComponent(typeof(AudioSource)),
     AddComponentMenu(nameof(AudioAnalyzationSystem) + "/" + nameof(AudioAnalyzer))]
    public class AudioAnalyzer : MonoBehaviour
    {
        public float[] FrequencyValues => _frequencyValues;
        public float[] SmoothValues => _smoothValues;
        public float[] NormalizedFrequencyValues => _normalizedFrequencyValues;
        public float[] NormalizedSmoothValues => _normalizedSmoothValues;

        [SerializeField, Range(0.5f, 0.99f)] private float _smoothnessCoefficient = 0.95f;

        private const float DecreaseValue = 0.005f;

        private const int GroupsCount = 8;

        private const int FrequencyScale = 10;

        private readonly float[] _samples = new float[512];

        private readonly float[] _frequencyValues = new float[GroupsCount];

        private readonly float[] _smoothValues = new float[GroupsCount];

        private readonly float[] _bufferGroupDecrease = new float[GroupsCount];

        private readonly float[] _normalizedSmoothValues = new float[GroupsCount];

        private readonly float[] _normalizedFrequencyValues = new float[GroupsCount];

        private readonly float[] _highestValues = new float[GroupsCount];

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);

            CreateFrequencyValuesGroups();

            CreateSmoothValuesGroups();
            CreateNormalizedValuesGroups();
        }

        private void CreateFrequencyValuesGroups()
        {
            var counter = 0;

            for (int i = 0; i < GroupsCount; i++)
            {
                var samplesCount = (int) Mathf.Pow(2, i + 1);

                var average = 0f;

                if (i == GroupsCount - 1)
                {
                    samplesCount += 2;
                    // note: addition 2 bytes to last group is hack to cover 512 bytes at all instead of 510
                }

                for (int k = 0; k < samplesCount; k++)
                {
                    average += _samples[counter] * (counter + 1);
                    counter++;
                }

                average /= counter;
                _frequencyValues[i] = average * FrequencyScale;
            }
        }

        private void CreateSmoothValuesGroups()
        {
            for (int i = 0; i < GroupsCount; i++)
            {
                var currentFrequency = _frequencyValues[i];
                var currentSmooth = _smoothValues[i];

                if (currentFrequency > currentSmooth)
                {
                    _smoothValues[i] = currentFrequency;
                    _bufferGroupDecrease[i] = DecreaseValue;
                }
                else
                {
                    _bufferGroupDecrease[i] = (_bufferGroupDecrease[i] - currentFrequency) / 8;
                    _smoothValues[i] = _smoothnessCoefficient * currentSmooth +
                                       (1 - _smoothnessCoefficient) * currentFrequency;
                }
            }
        }

        private void CreateNormalizedValuesGroups()
        {
            for (int i = 0; i < GroupsCount; i++)
            {
                var currentFrequency = _frequencyValues[i];
                var currentMax = _highestValues[i];

                if (currentFrequency > currentMax)
                {
                    _highestValues[i] = currentMax = currentFrequency;
                }

                if (currentMax != 0)
                {
                    _normalizedFrequencyValues[i] = currentFrequency / currentMax;
                    _normalizedSmoothValues[i] = _smoothValues[i] / currentMax;
                }
            }
        }
    }
}