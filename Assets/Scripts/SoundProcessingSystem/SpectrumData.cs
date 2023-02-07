namespace GuitarMan.SoundProcessingSystem
{
    public class SpectrumData
    {
        private readonly float[] _spectrumData;

        private readonly float _currentSoundTime;

        public SpectrumData(float[] spectrumData, float currentSoundTime)
        {
            _spectrumData = spectrumData;
            _currentSoundTime = currentSoundTime;
        }

        public float[] GetData()
        {
            return _spectrumData;
        }

        public float GetTime()
        {
            return _currentSoundTime;
        }
    }
}