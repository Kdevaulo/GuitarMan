namespace GuitarMan
{
    public interface IAudioAnalyzer
    {
        float Amplitude { get; }
        float SmoothAmplitude { get; }
        float[] FrequencyValues { get; }
        float[] SmoothValues { get; }
        float[] NormalizedFrequencyValues { get; }
        float[] NormalizedSmoothValues { get; }
    }
}