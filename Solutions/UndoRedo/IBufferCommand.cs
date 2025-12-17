namespace DevKit.UndoRedo
{
    public interface IBufferCommand
    {
        public void Execute();
        public void Rollback();
    }
}
