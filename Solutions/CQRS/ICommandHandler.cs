using System;

namespace DevKit.Solutions
{
    public interface ICommandHandler<in TCommand> : ICommandHandler where TCommand : ICommand
    {
        public void Handle(TCommand command);

        void ICommandHandler.Handle(ICommand command)
        {
            Handle((TCommand)command);
        }
    }

    public interface ICommandHandler
    {
        public Type CommandType { get; }
        public void Handle(ICommand command);
    }
}