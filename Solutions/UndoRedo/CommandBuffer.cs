using System;
using System.Collections.Generic;

namespace DevKit.UndoRedo
{
    public class CommandBuffer
    {
        public IReadOnlyList<IBufferCommand> UndoLongCommandsBuffer => _undoLongCommandsBuffer;
        public IReadOnlyList<IBufferCommand> RedoLongCommandsBuffer => _redoCommandsBuffer;

        private readonly int _capacity;
        private readonly LimitedRingBuffer<IBufferCommand> _undoLongCommandsBuffer;
        private readonly LimitedRingBuffer<IBufferCommand> _redoCommandsBuffer;

        public CommandBuffer(int capacity = 100)
        {
            _capacity = capacity;
            _undoLongCommandsBuffer = new(_capacity);
            _redoCommandsBuffer = new(_capacity);
        }

        public void ExecuteCommand(IBufferCommand command, Action callback = null)
        {
            if (command != null)
            {
                if (_redoCommandsBuffer.Count > 0)
                    _redoCommandsBuffer.Clear();

                command.Execute();
                PushCommandToUndoBuffer(command);
            }

            callback?.Invoke();
        }

        public void UndoLastCommand(Action callback = null)
        {
            if (_undoLongCommandsBuffer.Count == 0)
                return;

            var longCommandToUndo = _undoLongCommandsBuffer.RemoveLast();
            PushCommandToRedoBuffer(longCommandToUndo);
            longCommandToUndo?.Rollback();
            callback?.Invoke();
        }

        public void RedoLastCommand(Action callback = null)
        {
            if (_redoCommandsBuffer.Count == 0)
                return;

            var longCommandToRedo = _redoCommandsBuffer.RemoveLast();
            PushCommandToUndoBuffer(longCommandToRedo);
            longCommandToRedo?.Execute();
            callback?.Invoke();
        }

        public void Clear()
        {
            _undoLongCommandsBuffer.Clear();
            _redoCommandsBuffer.Clear();
        }

        private void PushCommandToUndoBuffer(IBufferCommand command)
        {
            if (_undoLongCommandsBuffer.Count >= _capacity)
                _undoLongCommandsBuffer.RemoveFirst();

            _undoLongCommandsBuffer.AddLast(command);
        }

        private void PushCommandToRedoBuffer(IBufferCommand longCommand)
        {
            if (_redoCommandsBuffer.Count >= _capacity)
                _redoCommandsBuffer.RemoveFirst();

            _redoCommandsBuffer.AddLast(longCommand);
        }
    }
}
