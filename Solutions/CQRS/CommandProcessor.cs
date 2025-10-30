using System;
using System.Collections.Generic;

namespace DevKit.Solutions
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly Dictionary<Type, ICommandHandler> _handlers = new();

        public CommandProcessor()
        {
        }

        public void AddHandler(ICommandHandler handler)
        {
            var type = handler.CommandType;

            if (_handlers.ContainsKey(type))
                LogExists(type.Name);

            _handlers[type] = handler;
            LogAdded(type.Name);
        }

        public void AddHandler<TCommand>(ICommandHandler<TCommand> handler) where TCommand : ICommand
        {
            var type = typeof(TCommand);

            if (_handlers.ContainsKey(type))
                LogExists(type.Name);

            _handlers[type] = handler;
            LogAdded(type.Name);
        }

        // public void Process<TCommand>(TCommand command) where TCommand : ICommand
        // {
        //     if (!_handlers.TryGetValue(typeof(TCommand), out var handler))
        //         throw new InvalidOperationException($"No handler registered for command {typeof(TCommand).Name}");

        //     ((ICommandHandler<TCommand>)handler).Handle(command);
        // }

        public void Process(ICommand command)
        {
            var type = command.GetType();
            if (!_handlers.TryGetValue(command.GetType(), out var handler))
                throw new InvalidOperationException($"No handler registered for command {type.Name}");

            handler.Handle(command);
        }

        private void LogAdded(string commandName)
        {
            UnityEngine.Debug.Log($"Handler registered for command {commandName}");
        }
        
        private void LogExists(string commandName)
        {
            UnityEngine.Debug.LogWarning($"Handler already registered for command {commandName}");
        }
        
    }
}