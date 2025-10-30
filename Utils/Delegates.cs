public static class Delegates
{
    public delegate void ActionRef<T>(ref T item);
    public delegate void ActionRef<T1, T2>(ref T1 item1, T2 item2);
}