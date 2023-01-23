namespace GuitarMan
{
    public interface IRandomizer
    {
        void Initialize(int min,int max);
        int GetIndex();
    }
}