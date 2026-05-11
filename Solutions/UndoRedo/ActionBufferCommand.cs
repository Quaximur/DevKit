using System;

namespace DevKit.UndoRedo
{
    public class ActionBufferCommand : IBufferCommand
    {
        private readonly Action _onExecute;
        private readonly Action _onRollback;

        public ActionBufferCommand(Action execute, Action rollback)
        {
            _onExecute = execute;
            _onRollback = rollback;
        }

        public void Execute()
        {
            _onExecute?.Invoke();
        }

        public void Rollback()
        {
            _onRollback?.Invoke();
        }
    }
}
