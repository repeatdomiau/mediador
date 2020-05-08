using System.Threading.Tasks;

namespace Mediador2
{
    public interface ICommand { }
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task Handle(TCommand command);
    }
    public interface ICommandValidator<TCommand> where TCommand : ICommand
    {
        Task<bool> Validate(TCommand command);
    }
    
    public interface ICommand<T> : ICommand { };
    public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<TResult> Handle(TCommand command);
    }
    public interface ICommandValidator<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<bool> Validate(TCommand command);
    }

    public interface INotification { };
    public interface INotificationListener<TNotification> where TNotification : INotification 
    {
        Task OnNotification(TNotification data);
    }
    
    public interface IMediador
    {
        Task<TResult> Send<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>;
        Task Send<TCommand>(TCommand command) where TCommand : ICommand;
        Task NotifyAll<TNotification>(TNotification notification) where TNotification : INotification;
    }
}
