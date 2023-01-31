using UnityEngine;
using UnityEngine.Assertions;

namespace GuitarMan.AudioAnalyzationSystem
{
    [AddComponentMenu(nameof(AudioAnalyzationSystem) + "/" + nameof(AudioVisualizer))]
    public class AudioVisualizer : MonoBehaviour
    {
        [SerializeField] private Transform[] _transforms;

        [SerializeField] private AudioAnalyzer _audioAnalyzer;

        [SerializeField] private bool _oneDirectionScale;

        [SerializeField] private bool _useSmooth;

        [SerializeField] private int _scaleMultiplier;

        [SerializeField] private int _startScale;

        private void Awake()
        {
            Assert.IsNotNull(_audioAnalyzer);
            Assert.IsTrue(_useSmooth
                ? _audioAnalyzer.NormalizedSmoothValues.Length == _transforms.Length
                : _audioAnalyzer.NormalizedFrequencyValues.Length == _transforms.Length);
        }

        private void Update()
        {
            SetTransformData(_useSmooth
                ? _audioAnalyzer.NormalizedSmoothValues
                : _audioAnalyzer.NormalizedFrequencyValues);
        }

        private void SetTransformData(float[] source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                var localScale = _transforms[i].localScale;

                var dynamicScale = source[i] * _scaleMultiplier + _startScale;

                _transforms[i].localScale = new Vector3(localScale.x, dynamicScale, localScale.z);

                if (_oneDirectionScale)
                {
                    var localPosition = _transforms[i].localPosition;

                    _transforms[i].localPosition = new Vector3(localPosition.x, dynamicScale / 2, localPosition.z);
                }
            }
        }
    }
}