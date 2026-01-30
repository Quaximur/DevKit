using System;
using System.Collections.Generic;

namespace DevKit.UndoRedo
{
    public class CommandBuffer
    {
        public IReadOnlyList<IBufferCommand> UndoCommandsBuffer => _undoCommandsBuffer;
        public IReadOnlyList<IBufferCommand> RedoCommandsBuffer => _redoCommandsBuffer;

        private readonly int _capacity;
        private readonly LimitedRingBuffer<IBufferCommand> _undoCommandsBuffer;
        private readonly LimitedRingBuffer<IBufferCommand> _redoCommandsBuffer;

        public CommandBuffer(int capacity = 100)
        {
            _capacity = capacity;
            _undoCommandsBuffer = new(_capacity);
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
            if (_undoCommandsBuffer.Count == 0)
                return;

            var commandToUndo = _undoCommandsBuffer.RemoveLast();
            PushCommandToRedoBuffer(commandToUndo);
            commandToUndo?.Rollback();
            callback?.Invoke();
        }

        public void RedoLastCommand(Action callback = null)
        {
            if (_redoCommandsBuffer.Count == 0)
                return;

            var commandToRedo = _redoCommandsBuffer.RemoveLast();
            PushCommandToUndoBuffer(commandToRedo);
            commandToRedo?.Execute();
            callback?.Invoke();
        }

        public void Clear()
        {
            _undoCommandsBuffer.Clear();
            _redoCommandsBuffer.Clear();
        }

        private void PushCommandToUndoBuffer(IBufferCommand command)
        {
            if (_undoCommandsBuffer.Count >= _capacity)
                _undoCommandsBuffer.RemoveFirst();

            _undoCommandsBuffer.AddLast(command);
        }

        private void PushCommandToRedoBuffer(IBufferCommand command)
        {
            if (_redoCommandsBuffer.Count >= _capacity)
                _redoCommandsBuffer.RemoveFirst();

            _redoCommandsBuffer.AddLast(command);
        }
    }
}
