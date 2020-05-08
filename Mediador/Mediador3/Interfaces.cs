#pragma warning disable CA1040 // Avoid empty interfaces
using System.Threading.Tasks;

namespace Mediador3
{
    public interface IValidator { }
    public interface ICommandValidator<TCommand> : IValidator where TCommand : ICommand
    {
        Task<bool> Validate(TCommand command);
    }
    
    public interface ICommand { }
    public interface ICommand<T> : ICommand { };
    
    public interface IHandler { }
    public interface ICommandHandler<TCommand> : IHandler where TCommand : ICommand
    {
        Task Handle(TCommand command);
    }
    public interface ICommandHandler<TCommand, TResult> : IHandler where TCommand : ICommand<TResult>
    {
        Task<TResult> Handle(TCommand command);
    }

    public interface INotification { };
    public interface INotificationListener { };
    public interface INotificationListener<TNotification> : INotificationListener where TNotification : INotification 
    {
        Task OnNotification(TNotification data);
    }
    
    public interface IMediador
    {
        Task<TResult> SendAsync<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>;
        Task SendASync<TCommand>(TCommand command) where TCommand : ICommand;
        Task NotifyAllAsync<TNotification>(TNotification notification) where TNotification : INotification;
    }
}
#pragma warning restore CA1040 // Avoid empty interfaces