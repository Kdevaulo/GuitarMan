using UnityEngine;
using UnityEngine.Assertions;

namespace GuitarMan.Utils
{
    public class WithoutLastRandomizer : IRandomizer
    {
        private int _minValue;

        private int _maxValue;

        private int _lastIndex = -1;

        /// <summary>
        /// Randomizer utility (maxValue - minValue should be >= 2)
        /// </summary>
        /// <param name="minValue">Inclusive</param>
        /// <param name="maxValue">Exclusive</param>
        void IRandomizer.Initialize(int minValue, int maxValue)
        {
            Assert.IsTrue(maxValue - minValue >= 2);
            _minValue = minValue;
            _maxValue = maxValue;
        }

        /// <returns>Random value, but not the last got value (can't return 2 identical values in a row)</returns>
        int IRandomizer.GetIndex()
        {
            int value;

            do
            {
                value = Random.Range(_minValue, _maxValue);
            } while (value == _lastIndex && _lastIndex != -1);

            _lastIndex = value;

            return value;
        }
    }
}