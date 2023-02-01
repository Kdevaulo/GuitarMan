using GuitarMan.Utils;

using UnityEngine;

namespace GuitarMan.AudioAnalyzationSystem
{
    public static class AudioAnalyzer
    {
        public static void CreateFrequencyFor8Groups(ref float[] frequencyValues, in float[] leftChannelSamples,
            in float[] rightChannelSamples)
        {
            const int groupsCount = 8;
            const float frequencyScale = 10;

            var counter = 0;

            for (int i = 0; i < groupsCount; i++)
            {
                var samplesCount = (int) Mathf.Pow(2, i + 1);

                var average = 0f;

                if (i == groupsCount - 1)
                {
                    samplesCount += 2;
                    // note: addition 2 bytes to last group is hack to cover 512 bytes at all instead of 510
                }

                for (int k = 0; k < samplesCount; k++)
                {
                    average += leftChannelSamples[counter] + rightChannelSamples[counter] * (counter + 1);
                    counter++;
                }

                average /= counter;
                frequencyValues[i] = average * frequencyScale;
            }
        }

        public static void CreateFrequencyFor64Groups(ref float[] frequencyValues, in float[] leftChannelSamples,
            in float[] rightChannelSamples)
        {
            const int groupsCount = 64;
            const float frequencyScale = 80;

            var power = 0;
            var counter = 0;
            var samplesCount = 1;

            for (int i = 0; i < groupsCount; i++)
            {
                var average = 0f;

                if (i == 16 || i == 32 || i == 40 || i == 48 || i == 56)
                {
                    samplesCount = (int) Mathf.Pow(2, ++power);
                    if (power == 3)
                    {
                        samplesCount -= 2;
                        // note: hack to use 6 samples instead of 8 (to avoid using more than 512 samples)
                    }
                }

                for (int k = 0; k < samplesCount; k++)
                {
                    average += leftChannelSamples[counter] + rightChannelSamples[counter] * (counter + 1);
                    counter++;
                }

                average /= counter;
                frequencyValues[i] = average * frequencyScale;
            }
        }

        public static void CreateSmoothValuesGroups(ref float[] smoothValues, ref float[] bufferGroupDecrease,
            in float[] frequencyValues, float smoothnessCoefficient)
        {
            const float decreaseValue = 0.005f;

            var groupsCount = smoothValues.Length;

            ArrayUtils.CheckingForEqualArraysLengths(groupsCount, bufferGroupDecrease.Length,
                frequencyValues.Length);

            for (int i = 0; i < groupsCount; i++)
            {
                var currentFrequency = frequencyValues[i];
                var currentSmooth = smoothValues[i];

                if (currentFrequency > currentSmooth)
                {
                    smoothValues[i] = currentFrequency;
                    bufferGroupDecrease[i] = decreaseValue;
                }
                else
                {
                    bufferGroupDecrease[i] = (bufferGroupDecrease[i] - currentFrequency) / groupsCount;
                    smoothValues[i] = smoothnessCoefficient * currentSmooth +
                                      (1 - smoothnessCoefficient) * currentFrequency;
                }
            }
        }

        public static void CreateNormalizedValuesGroups(ref float[] highestValues,
            ref float[] normalizedFrequencyValues,
            ref float[] normalizedSmoothValues, in float[] frequencyValues, in float[] smoothValues)
        {
            var groupsCount = highestValues.Length;

            ArrayUtils.CheckingForEqualArraysLengths(groupsCount, normalizedFrequencyValues.Length,
                normalizedSmoothValues.Length, frequencyValues.Length, smoothValues.Length);

            for (int i = 0; i < groupsCount; i++)
            {
                var currentFrequency = frequencyValues[i];
                var currentMax = highestValues[i];

                if (currentFrequency > currentMax)
                {
                    highestValues[i] = currentMax = currentFrequency;
                }

                if (currentMax != 0)
                {
                    normalizedFrequencyValues[i] = currentFrequency / currentMax;
                    normalizedSmoothValues[i] = smoothValues[i] / currentMax;
                }
            }
        }

        public static void CreateAmplitudes(ref float amplitude, ref float smoothAmplitude, ref float highestAmplitude,
            in float[] frequencyValues, in float[] smoothValues)
        {
            var groupsCount = frequencyValues.Length;

            ArrayUtils.CheckingForEqualArraysLengths(groupsCount, smoothValues.Length);

            var currentSmoothAmplitude = 0f;
            var currentAmplitude = 0f;

            for (int i = 0; i < groupsCount; i++)
            {
                currentSmoothAmplitude += smoothValues[i];

                currentAmplitude += frequencyValues[i];
            }

            if (highestAmplitude < currentAmplitude)
            {
                highestAmplitude = currentAmplitude;
            }

            amplitude = currentAmplitude / highestAmplitude;
            smoothAmplitude = currentSmoothAmplitude / highestAmplitude;
        }
    }
}