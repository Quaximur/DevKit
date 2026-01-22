namespace DevKit.Saves
{
    public interface ICopyable<T> where T : ICopyable<T>
    {
        public T Copy();
    }
}