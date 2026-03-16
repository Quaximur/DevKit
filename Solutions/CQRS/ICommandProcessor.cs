namespace DevKit.Solutions
{
    public interface ICommandProcessor
    {
        public void AddHandler(ICommandHandler handler);
        public void AddHandler<TCommand>(ICommandHandler<TCommand> handler) where TCommand : ICommand;
        public void Process<TCommand>(TCommand command) where TCommand : ICommand;
        public void Process(ICommand command);
    }
}